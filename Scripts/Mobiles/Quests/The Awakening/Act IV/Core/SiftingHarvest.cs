using System.Collections;
using System.Collections.Generic;
using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Regions;

namespace Server.Engines.Harvest
{
    public class Siftingharvest : HarvestSystem
    {
        private static Siftingharvest m_System;

        public static Siftingharvest System
        {
            get
            {
                if (m_System == null)
                    m_System = new Siftingharvest();

                return m_System;
            }
        }

        private HarvestDefinition m_Sifting;

        public HarvestDefinition Sifting
        {
            get { return m_Sifting; }
        }

        private Siftingharvest()
        {
            HarvestResource[] res;
            HarvestVein[] veins;

            #region Archeological
            HarvestDefinition Sifting = m_Sifting = new HarvestDefinition();

            // Resource banks are every 6x6 tiles
            Sifting.BankWidth = 6;
            Sifting.BankHeight = 6;

            // Every bank holds from 10 to 15 rock
            Sifting.MinTotal = 1;
            Sifting.MaxTotal = 3;

            // A resource bank will respawn its content every 10 to 20 minutes
            Sifting.MinRespawn = TimeSpan.FromMinutes(10.0);
            Sifting.MaxRespawn = TimeSpan.FromMinutes(20.0);

            // Skill checking is done on the Mining skill
            //Sifting.Skill = SkillName.Mining;

            // Set the list of harvestable tiles
            Sifting.Tiles = m_SandTiles;

            // Players must be within 2 tiles to harvest
            Sifting.MaxRange = 2;

            // One ore per harvest action
            Sifting.ConsumedPerHarvest = 1;
            Sifting.ConsumedPerFeluccaHarvest = 1;

            // The digging effect
            Sifting.EffectActions = new int[] { 11 };
            Sifting.EffectSounds = new int[] { 0x125, 0x126 };
            Sifting.EffectCounts = new int[] { 1 };
            Sifting.EffectDelay = TimeSpan.FromSeconds(1.6);
            Sifting.EffectSoundDelay = TimeSpan.FromSeconds(0.9);

            Sifting.NoResourcesMessage = 1152489;//The ground here does not look right for sifting. 
            //Sifting.DoubleHarvestMessage = 1152489;//The ground here does not look right for sifting. 
            //Sifting.TimedOutOfRangeMessage = 1152484; // You sift for a while but fail to find anything.
            Sifting.OutOfRangeMessage = 500446; // That is too far away.
            //Sifting.FailMessage = 1152484;//You sift for a while but fail to find anything. 
            Sifting.PackFullMessage = 1152501;//Your backpack and bank cannot hold the item so it was lost. 
            Sifting.ToolBrokeMessage = 1044038; // You have worn out your tool!

            res = new HarvestResource[]
	    {
		new HarvestResource( 0.0, 0.0, 100.0, 1152485, typeof (AntiquityFragment) )//You sift for a while and discover an antiquity fragment! 
	    };
            veins = new HarvestVein[]
	    {
		new HarvestVein( 100.0, 0.0, res[0], null )
	    };
            Sifting.Resources = res;
            Sifting.Veins = veins;

            Definitions.Add(Sifting);
            #endregion
        }

        public override bool CheckHarvest(Mobile from, Item tool)
        {
            if (!base.CheckHarvest(from, tool))
                return false;

            if (from.Mounted || from.Flying)
            {
                from.SendLocalizedMessage(501864); // You can't dig while riding.
                return false;
            }
            else if (from.IsBodyMod && !from.Body.IsHuman)
            {
                from.SendLocalizedMessage(501865); // You can't dig while polymorphed.
                return false;
            }

            return true;
        }

        public override bool CheckHarvest(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            if (!base.CheckHarvest(from, tool, def, toHarvest))
                return false;

            if (from.Mounted || from.Flying)
            {
                from.SendLocalizedMessage(501864); // You can't mine while riding.
                return false;
            }
            else if (from.IsBodyMod && !from.Body.IsHuman)
            {
                from.SendLocalizedMessage(501865); // You can't mine while polymorphed.
                return false;
            }

            return true;
        }

        public override void OnHarvestFinished(Mobile from, Item tool, HarvestDefinition def, HarvestVein vein, HarvestBank bank, HarvestResource resource, object harvested)
        {
            HarvestResource res = vein.PrimaryResource;

            //if (res == resource)
            //{
            //try
            //{
            // Map map = from.Map;

            //if (map == null)
            //return;
            //if (Utility.Random(100) < 20)
            //{
            //}
            if (Utility.Random(100) < 35)
            {
                switch (Utility.Random(3))
                {
                    case 0:
                        DesertScorpion DScorp = new DesertScorpion();
                        DScorp.Location = from.Location;
                        DScorp.Map = from.Map;
                        DScorp.Combatant = from;

                        if (Utility.Random(100) < 50)
                            DScorp.IsParagon = true;

                        World.AddMobile(DScorp);
                        break;

                    case 1:
                        DesertRotworm DRot = new DesertRotworm();
                        DRot.Location = from.Location;
                        DRot.Map = from.Map;
                        DRot.Combatant = from;

                        if (Utility.Random(100) < 50)
                            DRot.IsParagon = true;

                        World.AddMobile(DRot);
                        break;

                    case 2:
                        DesertBloodWorm DBloodW = new DesertBloodWorm();
                        DBloodW.Location = from.Location;
                        DBloodW.Map = from.Map;
                        DBloodW.Combatant = from;

                        if (Utility.Random(100) < 50)
                            DBloodW.IsParagon = true;

                        World.AddMobile(DBloodW);
                        break;
                }
                from.SendLocalizedMessage(1152486);//You sift for a while and disturb the underground lair of a sleeping creature!
            }
            else
            {
                from.SendLocalizedMessage(1152484);//You sift for a while but fail to find anything. 
            }
            //}
            //catch
            //{
            //}
            //}
        }

        public override bool BeginHarvesting(Mobile from, Item tool)
        {
            if (!base.BeginHarvesting(from, tool))
                return false;

            from.SendLocalizedMessage(1152491); // Where would you like to sift?
            return true;
        }
        public override void OnHarvestStarted(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            base.OnHarvestStarted(from, tool, def, toHarvest);
            from.Emote(1152490);//"shake shake shake"
            from.RevealingAction();
        }
        public override void OnBadHarvestTarget(Mobile from, Item tool, object toHarvest)
        {
            if (toHarvest is LandTarget)
                from.SendLocalizedMessage(1152487); // You can't sift there.
            else
                from.SendLocalizedMessage(1152487); // You can't sift there.
        }
        public static void Initialize()
        {
            Array.Sort(m_SandTiles);
        }
        #region Tile lists
        private static int[] m_SandTiles = new int[]
			{
				22, 23, 24, 25, 26, 27, 28, 29, 30, 31,
				32, 33, 34, 35, 36, 37, 38, 39, 40, 41,
				42, 43, 44, 45, 46, 47, 48, 49, 50, 51,
				52, 53, 54, 55, 56, 57, 58, 59, 60, 61,
				62, 68, 69, 70, 71, 72, 73, 74, 75,

				286, 287, 288, 289, 290, 291, 292, 293, 294, 295,
				296, 297, 298, 299, 300, 301, 402, 424, 425, 426,
				427, 441, 442, 443, 444, 445, 446, 447, 448, 449,
				450, 451, 452, 453, 454, 455, 456, 457, 458, 459,
				460, 461, 462, 463, 464, 465, 642, 643, 644, 645,
				650, 651, 652, 653, 654, 655, 656, 657, 821, 822,
				823, 824, 825, 826, 827, 828, 833, 834, 835, 836,
				845, 846, 847, 848, 849, 850, 851, 852, 857, 858,
				859, 860, 951, 952, 953, 954, 955, 956, 957, 958,
				967, 968, 969, 970, 1181, 

				1447, 1448, 1449, 1450, 1451, 1452, 1453, 1454, 1455,
				1456, 1457, 1458, 1611, 1612, 1613, 1614, 1615, 1616,
				1617, 1618, 1623, 1624, 1625, 1626, 1635, 1636, 1637,
				1638, 1639, 1640, 1641, 1642, 1647, 1648, 1649, 1650
            };
        #endregion
    }
}