using System;
using Server;
using System.IO;

namespace Server.LogConfiguration
{
    class LogConfig
    {
        /// <summary>
        /// Log Configuration Settings
        /// </summary>
        public static string LogCrashGuard = "Logs/Crash {0}.log";
        public static string LogIpLimits = "Logs/ipLimits.log";
        public static string LogAttacksLimits = "Logs/throttle.log";
        public const string LogRemoteAdminLogBaseDirectory = "Logs";
        public const string LogRemoteAdminLogSubDirectory = "RemoteAdmin";
        public static bool LogRemoteAdminLoggingEnabled = false;
        public static bool LogRemoteAdminLoggingInitializedEnabled = false;
        public static bool LogCommandLoggingEnabled = false;
        public static bool LogSpeechLoggingEnabled = false;
        public static readonly TimeSpan LogSpeechEntryDuration = TimeSpan.FromMinutes(20.0);
        public static readonly int LogSpeechMaxLength = 0;
        public static bool LogPlayerLoggingEnabled = false;
        public static bool LogPvPLoggingEnabled = false;
        public static bool LogPvMLoggingEnabled = false;
        public static string LogFilePath = Path.Combine("Saves", "Disguises", "Persistence.bin");
        public static string LogHelpFilePath = Path.Combine("Saves", "Help", "Pages.bin");
    }
}