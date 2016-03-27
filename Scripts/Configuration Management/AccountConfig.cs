using System;
using Server;

namespace Server.AccountConfiguration
{
    class AccountConfig
    {
        /// <summary>
        /// Account Settings
        /// </summary>
        public static readonly TimeSpan AccountYoungDuration = TimeSpan.FromHours(40.0);
        public static readonly TimeSpan AccountInactiveDuration = TimeSpan.FromDays(180.0);
        public static readonly TimeSpan AccountEmptyInactiveDuration = TimeSpan.FromDays(30.0);
        public static bool AccountAccountAttackLimiter = true;
        public static readonly int AccountMaxAccountsPerIP = 1;
        public static readonly bool AccountAutoAccountCreation = true;
        public static bool AccountRestrictCharacterDeletion = true;
        public static readonly TimeSpan AccountDeleteDelay = TimeSpan.FromDays(7.0);
        public static bool AccountAccountSecurityEnabled = false;
        public static bool AccountIPLimiterEnabled = true;
        /// <summary>
        /// true to block at connection, false to block at login request
        /// </summary>
        public static bool AccountSocketBlock = true;
        public static int AccountMaxAddresses = 10;
    }
}
