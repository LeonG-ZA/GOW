using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Network;
using Server.WebConfiguration;

namespace Server.Engines.MyShard
{
    public class MyShardStatus
    {
        private static DatabaseCommandQueue m_Command;
        public static void Initialize()
        {
            if (WebConfig.WebMyShardEnabled)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(20.0), WebConfig.WebStatusUpdateInterval, new TimerCallback(Begin));

                CommandSystem.Register("UpdateWebStatus", AccessLevel.Administrator, new CommandEventHandler(UpdateWebStatus_OnCommand));
            }
        }

        [Usage("UpdateWebStatus")]
        [Description("Starts the process of updating the MyShard online status database.")]
        public static void UpdateWebStatus_OnCommand(CommandEventArgs e)
        {
            if (m_Command == null || m_Command.HasCompleted)
            {
                Begin();
                e.Mobile.SendMessage("Web status update process has been started.");
            }
            else
            {
                e.Mobile.SendMessage("Web status database is already being updated.");
            }
        }

        public static void Begin()
        {
            if (m_Command != null && !m_Command.HasCompleted)
                return;

            DateTime start = DateTime.UtcNow;
            Console.WriteLine("MyShard: Updating status database");

            try
            {
                m_Command = new DatabaseCommandQueue("MyShard: Status database updated in {0:F1} seconds", "MyShard Status Database Thread");

                m_Command.Enqueue("DELETE FROM MyShard_status");

                List<NetState> online = NetState.Instances;

                for (int i = 0; i < online.Count; ++i)
                {
                    NetState ns = online[i];
                    Mobile mob = ns.Mobile;

                    if (mob != null)
                        m_Command.Enqueue(String.Format("INSERT INTO MyShard_status (char_id) VALUES ({0})", mob.Serial.Value.ToString()));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("MyShard: Error updating status database");
                Console.WriteLine(e);
            }

            if (m_Command != null)
                m_Command.Enqueue(null);
        }
    }
}