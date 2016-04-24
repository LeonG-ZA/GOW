using System;
using Server;

namespace Server.CharConfiguration
{
    class CharConfig
    {
        /// <summary>
        /// Character Creation Settings
        /// </summary>
        public static bool CharYoungEnabled = true;
        public static int CharStartingHunger = 20;
        public static int CharStartingThirst = 20;
        public static int CharTOLSkillCap = 7200;
        public static int CharSkillCap = 7000;
        public static int CharTestCenterStatCap = 250;
        public static int CharStatCap = 225;
        public static int CharFollowersMax = 5;
        public static int CharStartingGold = 1000;
        public static int CharBaneChosenExp = 0;
        public static int CharOphidianhExp = 0;
        public static int CharGargoyleExp = 2000;
        /// <summary>
        /// Skill Settings
        /// </summary>
        public static readonly bool CharAntiMacroCode = !Core.ML;
        public static TimeSpan CharAntiMacroExpire = TimeSpan.FromMinutes(5.0);
        public const int CharAllowance = 3;
        public const int CharLocationSize = 5;
        public static int CharSkillGain = 2;
        public static bool CharGGSEnabled = true;
        public static TimeSpan CharGGSResetTime = TimeSpan.FromHours(6.0);
        public static string CharGGSSavePath = "Saves/RateInfo";
        public static string CharGGSSaveFile = "GGS.bin";
        public static bool CharROTEnabled = false;
        public static TimeSpan CharROTResetTime = TimeSpan.FromHours(6.0);
        public static string CharROTSavePath = "Saves/RateInfo";
        public static string CharROTSaveFile = "RoT.bin";
        /// <summary>
        /// Char Misc Settings
        /// </summary>
        public static bool CharInsuranceEnabled = true;
        public static bool CharBODPub74Enabled = true;
        public static bool CharLoyaltyEnabled = true;
        public static bool CharEthicsEnabled = false;
        public static bool CharHSCartographyEnabled = true;
    }
}