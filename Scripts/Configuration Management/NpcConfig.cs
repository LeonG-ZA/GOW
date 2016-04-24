using System;
using Server;

namespace Server.NpcConfiguration
{
    class NpcConfig
    {
        /// <summary>
        /// Base Vendor Settings
        /// </summary>
        public const int NpcMaxSell = 500;
        public static int NpcBribeLimitCount = 10;
        public static double NpcBribePriceRate = 1.0;
        public static bool NpcIsInvulnerableEnabled = true;
        public static bool NpcCanTeachEnabled = true;
        public static bool NpcBardImmuneEnabled = true;
        public static bool NpcPlayerRangeSensitiveEnabled = true;
        public static bool NpcBODSystemEnabled = true;
        public static bool NpcBuySellContextEnabled = true;
        /// <summary>
        /// Base Escort Settings
        /// </summary>
        public static readonly TimeSpan NpcClassicEscortDelay = TimeSpan.FromMinutes(5.0);
        public static readonly TimeSpan NpcEscortDelay = TimeSpan.FromMinutes(5.0);
        /// <summary>
        /// Pet Settings
        /// </summary>
        public const bool NpcBondingEnabled = true;
        public static TimeSpan NpcBondingDelay { get { return TimeSpan.FromDays(7.0); } }
        public static TimeSpan NpcBondingAbandonDelay { get { return TimeSpan.FromDays(1.0); } }
        public const int NpcPetMaxLoyalty = 100;
        public const int NpcPetMaxOwners = 5;
        /// <summary>
        /// Monster Settings
        /// </summary>
        public static bool NpcEnableRummaging = true;
        public const double NpcChanceToRummage = 0.5; // 50%
        public const double NpcMinutesToNextRummageMin = 1.0;
        public const double NpcMinutesToNextRummageMax = 4.0;
        public const double NpcMinutesToNextChanceMin = 0.25;
        public const double NpcMinutesToNextChanceMax = 0.75;
        /// <summary>
        /// Paragon Settings
        /// </summary>
        public static double NpcChestParagonChance = .10;                    // Chance that a paragon will carry a paragon chest
        public static double NpcParagonChocolateIngredientChance = .20;      // Chance that a paragon will drop a chocolatiering ingredient
        public static int NpcParagonHue = 0x501;                             // Paragon hue
        /// <summary>
        /// Blackrock Infected Settings
        /// </summary>
        public static double NpcChestBlackrockInfectedChance = .10;          // Chance that a Blackrock Infected will carry a Blackrock Infected chest
        public static int NpcBlackrockInfectedHue = 1175;                              // Blackrock Infected Hue
        public static TimeSpan NpcFastRegenRate = TimeSpan.FromSeconds(.5);
        public static TimeSpan NpcCPUSaverRate = TimeSpan.FromSeconds(2);
    }
}