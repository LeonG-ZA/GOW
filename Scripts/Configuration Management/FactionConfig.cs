using System;
using Server;

namespace Server.FactionConfiguration
{
    class FactionConfig
    {
        /// <summary>
        /// If true, the new Council of Mages stronghold location at the south of
        /// Moonglow Island will be used instead of the old location in the Magincia
        /// Parlament Building.
        /// </summary>
        public static bool NewCoMLocation = true;
        /// <summary>
        /// Setting this to false will enable the faction regions.
        /// </summary>
        public static bool RegionDisabled = true;
        /// <summary>
        /// To enable Trap Faction Cafting set this to true.
        /// </summary>
        public static bool TrapCrafting = false;
    }
}