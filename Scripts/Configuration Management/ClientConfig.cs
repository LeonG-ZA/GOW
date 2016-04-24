using System;
using Server;

namespace Server.ClientConfiguration
{
    class ClientConfig
    {
        /// <summary>
        /// Client Verification
        /// </summary>
        public static bool ClientDetectClientRequirementEnabled = true;
        public static bool ClientAllowRegularEnabled = true;
        public static bool ClientAllowUOTDEnabled = true;
        public static bool ClientAllowGodEnabled = true;
        public static bool ClientEncryptionEnabled = true;
        public static bool ClientAllowUnencryptedClients = true;
        public static readonly TimeSpan ClientAgeLeniency = TimeSpan.FromDays(10);
        public static readonly TimeSpan ClientGameTimeLeniency = TimeSpan.FromHours(25);
        public static readonly TimeSpan ClientKickDelay = TimeSpan.FromSeconds(20.0);
    }
}