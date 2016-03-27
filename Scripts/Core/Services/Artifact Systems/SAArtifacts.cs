using System;
using System.IO;
using System.Xml;
using Server.Commands;
using Server.Engines.Quests;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Misc;

namespace Server
{
    public static class StygianAbyss
    {
        public static Type[] SAArtifacts { get { return m_SAArtifacts; } }

        private static readonly Type[] m_SAArtifacts = new[]
		{
			typeof(AxesOfFury), typeof(BreastplateOfTheBerserker), typeof(EternalGuardianStaff), typeof(LegacyOfDespair),
			typeof(GiantSteps), typeof(StaffOfShatteredDreams), typeof(PetrifiedSnake), typeof(StoneDragonsTooth),
			typeof(TokenOfHolyFavor), typeof(SwordOfShatteredHopes), typeof(Venom), typeof(StormCaller)
		};

        public static bool SACheckArtifactChance(Mobile m, BaseCreature bc)
        {
            if (!Core.SA)
                return false;

            return Paragon.CheckArtifactChance(m, bc);
        }

        public static void GiveSAArtifact(Mobile pm)
        {
            Item item = Activator.CreateInstance(m_SAArtifacts[Utility.Random(m_SAArtifacts.Length)]) as Item;

            if (item == null)
            {
                return;
            }

            if (pm.AddToBackpack(item))
            {
                pm.SendLocalizedMessage(1062317);
                // For your valor in combating the fallen beast, a special artifact has been bestowed on you.
                pm.SendLocalizedMessage(1072223); // An item has been placed in your backpack.
            }
            else if (pm.BankBox != null && pm.BankBox.TryDropItem(pm, item, false))
            {
                pm.SendLocalizedMessage(1062317);
                // For your valor in combating the fallen beast, a special artifact has been bestowed on you.
                pm.SendLocalizedMessage(1072224); // An item has been placed in your bank box.
            }
            else
            {
                item.MoveToWorld(pm.Location, pm.Map);
                pm.SendLocalizedMessage(1072523); // You find an artifact, but your backpack and bank are too full to hold it.
            }
            EffectPool.ArtifactDrop(pm);
        }

        public static bool CheckSA(Mobile from)
        {
            return CheckSA(from, true);
        }

        public static bool CheckSA(Mobile from, bool message)
        {
            if (from == null || from.NetState == null)
                return false;

            if (from.NetState.SupportsExpansion(Expansion.SA))
                return true;

            if (message)
                from.SendLocalizedMessage(1113763); // You must upgrade to Stygian Abyss in order to use that item.

            return false;
        }

        public static bool IsSARegion(Region region)
        {
            return region.IsPartOf("Ter Mur") ||
                   region.IsPartOf("AbyssEntrance") ||
                   region.IsPartOf("Abyss") ||
                   region.IsPartOf("StygianDragonLair") ||
                   region.IsPartOf("MedusasLair") ||
                   region.IsPartOf("NPC Encampment");
        }
    }
}