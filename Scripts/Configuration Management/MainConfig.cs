using System;
using Server;
using Server.Network;
using Server.Commands.Generic;
using Server.Commands;

namespace Server.MainConfiguration
{
    class MainConfig
    {
        public static void Initialize()
        {
            CommandSystem.Prefix = "[";
        }
        /// <summary>
        /// Expansion Settings
        /// AOS/SE/ML/SA/HS/TOL/None
        /// </summary>
        public static readonly Expansion MainExpansion = Expansion.TOL;
        /// <summary>
        /// Set Your Mul Path
        /// </summary>
        public static string MainMulPath = @"C:\Program Files (x86)\Electronic Arts\GOW";
        public static bool MainDataPathExtendedEnabled = true;
        /// <summary>
        /// Set Ip and GOW Name
        /// </summary>
        public static string MainIPAddress = "127.0.0.1";
        public static int MainPort = 2593;
        public static string MainServerName = "GOW";
        public static bool MainAutoDetectEnabled = true;
        /// <summary>
        /// Email Settings
        /// </summary>
        public static readonly string MainEmailServer = null;
        public static readonly int MainEmailPort = 25;
        public static readonly string MainFromAddress = null;
        public static readonly string MainCrashAddresses = null;
        public static readonly string MainSpeechLogPageAddresses = null;
        /// <summary>
        /// GOW Save Settings
        /// </summary>
        public static readonly TimeSpan MainDelay = TimeSpan.FromMinutes(60.0);
        public static readonly TimeSpan MainWarning = TimeSpan.FromMinutes(1.0);
        public static bool MainSavesEnabled = true;
        public static bool BackupEnabled = false;
        /// <summary>
        /// GOW Restart Settings
        /// </summary>
        public static bool MainRestartEnabled = false;
        public static readonly TimeSpan MainRestartTime = TimeSpan.FromHours(24.0);
        public static readonly TimeSpan MainRestartDelay = TimeSpan.FromMinutes(2.0);
        public static readonly TimeSpan MainWarningDelay = TimeSpan.FromMinutes(1.0);
        /// <summary>
        /// Crash Guard Settings
        /// </summary>
        public static bool MainCrashGuardEnabled = true;
        public static readonly bool MainCrashSaveBackupEnabled = false;
        public static readonly bool MainCrashRestartServerEnabled = true;
        public static readonly bool MainCrashGenerateReportEnabled = true;
        /// <summary>
        /// Misc Settings
        /// </summary>
        public static bool MainChatEnabled = true;
        public static readonly bool MainProfanityActionEnabled = false;
        public static bool MainAssistantsEnabled = false;
        public static bool MainRazornegotiatorEnabled = false;
        public static bool MainWelcomeMsgCountEnabled = false;
        /// <summary>
        /// T2A with HS ItemID
        /// </summary>
        public static bool MainItemIDEnabled = false;
        /// <summary>
        /// SA UnderWorld Room Gen
        /// </summary>
        public static bool GenerateUnderworldRoomsEnabled = true;
        /// <summary>
        /// HS Public Moongate Settings
        /// </summary>
        public static bool MainTrammelGenEnabled = true;
        public static bool MainFeluccaGenEnabled = true;
        public static bool MainIlshenarGenEnabled = true;
        public static bool MainMalasGenEnabled = true;
        public static bool MainTokunoGenEnabled = true;
        public static bool MainTerMurGenEnabled = true;
        public static bool MainFeluccaOnlyEnabled = false;
        /// <summary>
        /// Light Levels
        /// </summary>
        public const int MainDayLevel = 0;
        public const int MainNightLevel = 12;
        public const int MainDungeonLevel = 26;
        public const int MainJailLevel = 9;
        /// <summary>
        /// Misc File Paths
        /// </summary>
        public static string MainStaticExportFilePath = @".\Export\";
        public static readonly string MainAddonGenFilePath = @"AddonGenerator\{0}Addon.cs";
        public static readonly string MainMyShardFilePath1 = "MySharddb_{0}.txt";
        public static readonly string MainMyShardFilePath2 = "MySharddb_{0}_{1}.txt";
    }
}
