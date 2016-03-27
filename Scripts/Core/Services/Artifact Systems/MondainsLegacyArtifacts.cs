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
    public static class MondainsLegacy
    {
        public static Type[] PigmentList { get { return m_PigmentList; } }
        public static Type[] Artifacts { get { return m_Artifacts; } }

        private static readonly Type[] m_PigmentList = new Type[]
        {
            typeof(ZooRewardBodySash), typeof(ZooRewardBonnet), typeof(ZooRewardCloak), typeof(ZooRewardFloppyHat),
            //typeof(RoyalZooLeatherChest), typeof(RoyalZooLeatherFemaleChest), typeof(RoyalZooStuddedLegs), typeof(RoyalZooStuddedGloves),
            //typeof(RoyalZooStuddedGorget), typeof(RoyalZooStuddedArms), typeof(RoyalZooStuddedChest), typeof(RoyalZooStuddedFemaleChest),
            //typeof(RoyalZooPlateHelm), typeof(RoyalZooPlateFemaleChest), typeof(RoyalZooPlateChest), typeof(RoyalZooPlateArms),
            //typeof(RoyalZooPlateGorget), typeof(RoyalZooPlateGloves), typeof(RoyalZooPlateLegs),
            //typeof(KeeoneansChainMail),
            //typeof(VesperOrderShield), typeof(VesperChaosShield),typeof(TreatiseonAlchemyTalisman),
            //typeof(PrimerOnArmsTalisman), typeof(MyBookTalisman), typeof(TalkingtoWispsTalisman), typeof(GrammarOfOrchishTalisman),
            //typeof(BirdsofBritanniaTalisman), typeof(TheLifeOfTravelingMinstrelTalisman), typeof(MaceAndShieldGlasses), typeof(GlassesOfTheArts),
            //typeof(WizardsCrystalGlasses), typeof(TradesGlasses),typeof(TreasuresAndTrinketsGlasses),
            typeof(MinaxsArmor), typeof(ClaininsSpellbook),typeof(BlackthornsKryss), typeof(SwordOfJustice), typeof(GeoffreysAxe),
            //typeof(FoldedSteelGlasses), typeof(LyricalGlasses), typeof(AnthropomorphistGlasses),
            //typeof(LightOfWayGlasses), typeof(NecromanticGlasses), typeof(MaritimeGlasses),
            /*typeof(PoisonedGlasses),*/ typeof(GypsyHeaddress), typeof(NystulsWizardsHat),
            typeof(JesterHatOfChuckles)
        };

        private static readonly Type[] m_Artifacts = new Type[]
        {
            typeof(AegisOfGrace), typeof(BladeDance), typeof(BloodwoodSpirit), typeof(Bonesmasher),
            typeof(Boomstick), typeof(BrightsightLenses), typeof(FeyLeggings), typeof(FleshRipper),
            typeof(HelmOfSwiftness), typeof(PadsOfTheCuSidhe), typeof(QuiverOfRage), typeof(QuiverOfElements),
            typeof(RaedsGlory), typeof(RighteousAnger), typeof(RobeOfTheEclipse), typeof(RobeOfTheEquinox),
            typeof(SoulSeeker), typeof(TalonBite), typeof(TotemOfVoid), typeof(WildfireBow),
            typeof(Windsong), typeof(FeyLeggingsHuman)
        };

        public static bool CheckArtifactChance(Mobile m, BaseCreature bc)
        {
            if (!Core.ML)
                return false;

            return Paragon.CheckArtifactChance(m, bc);
        }

        public static void GiveArtifactTo(Mobile pm)
        {
            Item item = Activator.CreateInstance(m_Artifacts[Utility.Random(m_Artifacts.Length)]) as Item;

            if (item == null)
            {
                return;
            }

            if (pm.AddToBackpack(item))
            {
                pm.SendLocalizedMessage(1072223); // An item has been placed in your backpack.
                pm.SendLocalizedMessage(1062317); // For your valor in combating the fallen beast, a special artifact has been bestowed on you.
            }
            else if (pm.BankBox.TryDropItem(pm, item, false))
            {
                pm.SendLocalizedMessage(1072224); // An item has been placed in your bank box.
                pm.SendLocalizedMessage(1062317); // For your valor in combating the fallen beast, a special artifact has been bestowed on you.
            }
            else
            {
                // Item was placed at feet by m.AddToBackpack
                pm.SendLocalizedMessage(1072523); // You find an artifact, but your backpack and bank are too full to hold it.
            }
            EffectPool.ArtifactDrop(pm);
        }

        public static bool CheckML(Mobile from)
        {
            return CheckML(from, true);
        }

        public static bool CheckML(Mobile from, bool message)
        {
            if (from == null || from.NetState == null)
                return false;

            if (from.NetState.SupportsExpansion(Expansion.ML))
                return true;

            if (message)
                from.SendLocalizedMessage(1072791); // You must upgrade to Mondain's Legacy in order to use that item.

            return false;
        }

        public static bool IsMLRegion(Region region)
        {
            return region.IsPartOf("Twisted Weald") ||
                   region.IsPartOf("Sanctuary") ||
                   region.IsPartOf("Prism of Light") ||
                   region.IsPartOf("Citadel") ||
                   region.IsPartOf("Bedlam") ||
                   region.IsPartOf("Blighted Grove") ||
                   region.IsPartOf("Painted Caves") ||
                   region.IsPartOf("Palace of Paroxysmus") ||
                   region.IsPartOf("Labyrinth");
        }

        public static void Initialize()
        {
            CommandSystem.Register("Quests", AccessLevel.GameMaster, new CommandEventHandler(Quests_OnCommand));
        }

        [Usage("Quests")]
        [Description("Pops up a quest list from targeted player.")]
        private static void Quests_OnCommand(CommandEventArgs e)
        {
            Mobile m = e.Mobile;
            m.SendMessage("Target a player to view their quests.");

            m.BeginTarget(-1, false, Server.Targeting.TargetFlags.None, new TargetCallback(
                delegate(Mobile from, object targeted)
                {
                    if (targeted is PlayerMobile)
                        m.SendGump(new MondainQuestGump((PlayerMobile)targeted));
                    else
                        m.SendMessage("That is not a player!");
                }));
        }
    }
}