using System;
using System.Collections;
using System.Diagnostics;
using Server.Commands;
using Server.Network;
using Server.MainConfiguration;
using Server.LogConsole;

namespace Server.Misc
{
    public class AutoRestart : Timer
    {
        public static bool Enabled = MainConfig.MainRestartEnabled;
        private static readonly TimeSpan RestartTime = MainConfig.MainRestartTime;
        private static readonly TimeSpan RestartDelay = MainConfig.MainRestartDelay;
        private static readonly TimeSpan WarningDelay = MainConfig.MainWarningDelay;
        private static bool m_Restarting;
        private static bool m_Shutdown;
        private static DateTime m_RestartTime;
        public AutoRestart()
            : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
        {
            this.Priority = TimerPriority.FiveSeconds;

            m_RestartTime = DateTime.UtcNow.Date + RestartTime;

            if (m_RestartTime < DateTime.UtcNow)
                m_RestartTime += TimeSpan.FromDays(1.0);
        }

        public static bool Restarting
        {
            get
            {
                return m_Restarting;
            }
        }
        public static void Initialize()
        {
            CommandSystem.Register("Restart", AccessLevel.Administrator, new CommandEventHandler(Restart_OnCommand));
            new AutoRestart().Start();
        }

        public static void Restart_OnCommand(CommandEventArgs e)
        {
            if (m_Restarting)
            {
                e.Mobile.SendMessage("GOW: The server is already restarting.");
            }
            else
            {
                e.Mobile.SendMessage("GOW: You have initiated a server shutdown.");
                Enabled = true;
                m_RestartTime = DateTime.UtcNow;
            }
        }

        public static void Restart_OnCommand(bool bShutdown)
        {
            if (m_Restarting)
            {
                if (bShutdown) ConsoleLog.WriteLine("GOW: The server is already shutting down.");
                else ConsoleLog.WriteLine("GOW: The server is already restarting.");
            }
            else
            {
                if (bShutdown) m_Shutdown = true;
                Enabled = true;
                m_RestartTime = DateTime.UtcNow;
            }
        }

        protected override void OnTick()
        {
            if (m_Restarting || !Enabled)
                return;

            if (DateTime.UtcNow < m_RestartTime)
                return;

            if (WarningDelay > TimeSpan.Zero && WarningDelay < RestartDelay)
            {
                Timer.DelayCall(WarningDelay, WarningDelay, new TimerCallback(Warning_Callback));
            }

            AutoSave.Save();

            m_Restarting = true;

            Timer.DelayCall(RestartDelay, new TimerCallback(Restart_Callback));
        }

        private void Warning_Callback()
        {
            TimeSpan timeleft = (m_RestartTime + RestartDelay) - DateTime.UtcNow;
            World.Broadcast(0x35, true, String.Format("The Server is going down in {0} ", timeleft), MessageType.System);
        }

        private void Restart_Callback()
        {
            if (m_Shutdown) Core.Process.Kill();
            else  Core.Kill(true);
        }
    }
}