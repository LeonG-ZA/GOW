using System;
using Server;

namespace Server.GuildConfiguration
{
    class GuildConfig
    {
        /// <summary>
        /// Guild Settings
        /// You Can Put: Core.SE / Core.ML / Core.SA / Core.AOS / false
        /// </summary>
        public static bool GuildNewGuildSystemEnabled = Core.SE;
        public static bool GuildOrderChaos { get { return !Core.SE; } }
        public static readonly int GuildRegistrationFee = 25000;
        public static readonly int GuildAbbrevLimit = 4;
        public static readonly int GuildNameLimit = 40;
        public static readonly int GuildMajorityPercentage = 66;
        public static readonly TimeSpan GuildInactiveTime = TimeSpan.FromDays(30);
    }
}