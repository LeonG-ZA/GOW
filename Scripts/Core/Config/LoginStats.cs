using System;
using Server.Network;
using System.Collections.Generic;
using Server.MainConfiguration;
using Server.Accounting;
using System.IO;
using Server.LogConfiguration;

namespace Server.Misc
{
    public class LoginStats
    {
        public static void Initialize()
        {
            // Register our event handler
            EventSink.Login += new LoginEventHandler(EventSink_Login);

            if (LogConfig.LogPlayerLoggingEnabled)
            {
                if (!Directory.Exists("Logs"))
                    Directory.CreateDirectory("Logs");

                string directory = "Logs/Activity";

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }
        }

        private static void EventSink_Login(LoginEventArgs args)
        {
            Mobile m = args.Mobile;
            int userCount = NetState.Instances.Count;
            int itemCount = World.Items.Count;
            int mobileCount = World.Mobiles.Count;

            if (LogConfig.LogPlayerLoggingEnabled)
            {
                string msg = " Has logged on to " + args.Mobile.Name + ".";

                try
                {
                    string path = Core.BaseDirectory;

                    Account acct = m.Account as Account;

                    string name = (acct == null ? m.Name : acct.Username);
                    AppendPath(ref path, "Logs");
                    AppendPath(ref path, "Activity");
                    path = Path.Combine(path, String.Format("{0}.log", name));
                    using (StreamWriter sw = new StreamWriter(path, true))
                        sw.WriteLine("{0}: {1}: {2}", DateTime.UtcNow, m.NetState, msg);
                }
                catch
                {
                }
            }

            if (MainConfig.MainWelcomeMsgCountEnabled)
            {
                m.SendMessage("Welcome, {0}! There {1} currently {2} user{3} online, with {4} item{5} and {6} mobile{7} in the world.",
                    args.Mobile.Name,
                    userCount == 1 ? "is" : "are",
                    userCount, userCount == 1 ? "" : "s",
                    itemCount, itemCount == 1 ? "" : "s",
                    mobileCount, mobileCount == 1 ? "" : "s");
            }

            List<Server.Engines.Quests.MondainQuester> listMobilesQuester = new List<Server.Engines.Quests.MondainQuester>();
            foreach (Mobile m_mobile in World.Mobiles.Values)
            {
                Server.Engines.Quests.MondainQuester mQuester = m_mobile as Server.Engines.Quests.MondainQuester;
                if (mQuester != null)
                    listMobilesQuester.Add(mQuester);
            }

            foreach (Server.Engines.Quests.MondainQuester quester in listMobilesQuester)
            {
                if (args.Mobile.NetState != null)
                {
                    string name = string.Empty;
                    if (quester.Name != null)
                        name += quester.Name;
                    if (quester.Title != null)
                        name += " " + quester.Title;
                    args.Mobile.NetState.Send(new DisplayWaypoint(quester.Serial, quester.X, quester.Y, quester.Z, quester.Map.MapID, 4, name));
                }
            }

            if (m.IsStaff())
            {
                Server.Engines.Help.PageQueue.Pages_OnCalled(m);
            }
        }

        public static void AppendPath(ref string path, string toAppend)
        {
            path = Path.Combine(path, toAppend);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}