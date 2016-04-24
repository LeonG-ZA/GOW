using System;
using Server;

namespace Server.HouseConfiguration
{
    class HouseConfig
    {
        /// <summary>
        /// House Settings
        /// ShardHousesMax 0 = 1, 1 = 2, 2 = 3 ect
        /// </summary>
        public static int HouseHousesMax = 0;
        public const int HouseMaxCoOwners = 15;
        public static int HouseMaxFriends { get { return !Core.AOS ? 50 : 140; } }
        public static int HouseMaxBans { get { return !Core.AOS ? 50 : 140; } }
        public const bool HouseDecayEnabled = true;
        public static TimeSpan HouseDecayPeriod { get { return TimeSpan.FromDays(5.0); } }
        public const int HouseMaximumBarkeepCount = 2;
    }
}