using System;
using Server;

namespace Server.SpellsConfiguration
{
    class SpellsConfig
    {
        /// <summary>
        /// Magic Spell Settings
        /// </summary>
        public static readonly TimeSpan SpellsCombatHeatDelay = TimeSpan.FromSeconds(30.0);
        public static readonly bool SpellsRestrictTravelCombat = true;
        public static readonly TimeSpan SpellsAosDamageDelay = TimeSpan.FromSeconds(1.0);
        public static readonly TimeSpan SpellsOldDamageDelay = TimeSpan.FromSeconds(0.5);
        public static readonly TimeSpan SpellsNextSpellDelay = TimeSpan.FromSeconds(0.75);
        public static TimeSpan SpellsAnimateDelay = TimeSpan.FromSeconds(1.5);
        public static readonly bool RequiredMana = true;
        public static readonly bool RequiredReagents = true;
        public static readonly bool RequiredTithingandMana = true;
        public static readonly bool RequiredTithing = true;
    }
}
