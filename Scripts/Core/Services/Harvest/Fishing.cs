using System;
using System.Collections.Generic;
using Server.Engines.Quests;
using Server.Engines.Quests.Collector;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.FSHSR;

namespace Server.Engines.Harvest
{
    public class Fishing : HarvestSystem
    {
        private static Fishing m_System;

        public static Fishing System
        {
            get
            {
                if (m_System == null)
                    m_System = new Fishing();

                return m_System;
            }
        }

        private readonly HarvestDefinition m_Definition;

        public HarvestDefinition Definition
        {
            get
            {
                return this.m_Definition;
            }
        }

        private Fishing()
        {
            HarvestResource[] res;
            HarvestVein[] veins;

            #region Fishing
            HarvestDefinition fish = new HarvestDefinition();

            // Resource banks are every 8x8 tiles
            fish.BankWidth = 8;
            fish.BankHeight = 8;

            // Every bank holds from 5 to 15 fish
            fish.MinTotal = 5;
            fish.MaxTotal = 15;

            // A resource bank will respawn its content every 10 to 20 minutes
            fish.MinRespawn = TimeSpan.FromMinutes(10.0);
            fish.MaxRespawn = TimeSpan.FromMinutes(20.0);

            // Skill checking is done on the Fishing skill
            fish.Skill = SkillName.Fishing;

            // Set the list of harvestable tiles
            fish.Tiles = m_WaterTiles;
            fish.RangedTiles = true;

            // Players must be within 4 tiles to harvest
            fish.MaxRange = 4;

            // One fish per harvest action
            fish.ConsumedPerHarvest = 1;
            fish.ConsumedPerFeluccaHarvest = 1;

            // The fishing
            fish.EffectActions = new int[] { 12 };
            fish.EffectSounds = new int[0];
            fish.EffectCounts = new int[] { 1 };
            fish.EffectDelay = TimeSpan.Zero;
            fish.EffectSoundDelay = TimeSpan.FromSeconds(8.0);

            fish.NoResourcesMessage = 503172; // The fish don't seem to be biting here.
            fish.FailMessage = 503171; // You fish a while, but fail to catch anything.
            fish.TimedOutOfRangeMessage = 500976; // You need to be closer to the water to fish!
            fish.OutOfRangeMessage = 500976; // You need to be closer to the water to fish!
            fish.PackFullMessage = 503176; // You do not have room in your backpack for a fish.
            fish.ToolBrokeMessage = 503174; // You broke your fishing pole.

            res = new HarvestResource[]
            {
                new HarvestResource(00.0, 00.0, 100.0, 1043297, typeof(Fish))
            };

            veins = new HarvestVein[]
            {
                new HarvestVein(100.0, 0.0, res[0], null)
            };

            fish.Resources = res;
            fish.Veins = veins;

            if (Core.ML)
            {
                fish.BonusResources = new BonusHarvestResource[]
                {
                    new BonusHarvestResource(0, 97.0, null, null), //set to same chance as mining ml gems
                    new BonusHarvestResource(80.0, 1.0, 1072597, typeof(WhitePearl)),
                    new BonusHarvestResource( 80.0, 2.0, 1113764, typeof( DelicateScales ) )
                };
            }

            this.m_Definition = fish;
            this.Definitions.Add(fish);
            #endregion
        }

        public override void OnConcurrentHarvest(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            from.SendLocalizedMessage(500972); // You are already fishing.
        }

        private class MutateEntry
        {
            public readonly double m_ReqSkill;

            public readonly double m_MinSkill;

            public readonly double m_MaxSkill;

            public readonly bool m_DeepWater;
            public readonly bool m_RequiresDungeon;
            public readonly bool m_RequiresBait;
            public readonly Map m_RequiredMap;
            public readonly string m_RequiredRegion;

            public readonly Type[] m_Types;

            public MutateEntry(double reqSkill, double minSkill, double maxSkill, bool deepWater, bool dungeon, bool bait, Map map, string region, params Type[] types)
            {
                this.m_ReqSkill = reqSkill;
                this.m_MinSkill = minSkill;
                this.m_MaxSkill = maxSkill;
                this.m_DeepWater = deepWater;
                this.m_RequiresDungeon = dungeon;
                this.m_RequiresBait = bait;
                this.m_RequiredMap = map;
                this.m_RequiredRegion = region;
                this.m_Types = types;
            }
        }

        private static readonly MutateEntry[] m_MutateTable = new MutateEntry[]
        {
        	// Standard
            new MutateEntry(80.0, 80.0, 4080.0, true, false, false, null, null, typeof(SpecialFishingNet)),
            new MutateEntry(80.0, 80.0, 4080.0, true, false, false, null, null, typeof(BigFish)),
            new MutateEntry(90.0, 80.0, 4080.0, true, false, false, null, null, typeof(TreasureMap)),
            new MutateEntry(100.0, 80.0, 4080.0, true, false, false, null, null, typeof(MessageInABottle)),
            new MutateEntry(0.0, 125.0, -2375.0, false, false, false, null, null, typeof(PrizedFish), typeof(WondrousFish), typeof(TrulyRareFish), typeof(PeculiarFish)),
            // Footware
            new MutateEntry(0.0, 105.0, -420.0, false, false, false, null, null, typeof(Boots), typeof(Shoes), typeof(Sandals), typeof(ThighBoots)),
            // Legendary
            new MutateEntry(120.0, 120.0, 4080.0, false, false, false, null, "Destard", typeof(AbyssalDragonfish)),
            new MutateEntry(105.0, 120.0, 4080.0, true, false, false, Map.Felucca, null, typeof(BlackMarlin)),
            new MutateEntry(105.0, 105.0, 4080.0, true, false, false, Map.Trammel, null, typeof(BlueMarlin)),
            new MutateEntry(120.0, 120.0, 4080.0, false, false, false, null, "Terathan Keep", typeof(DungeonPike)),
            new MutateEntry(92.2, 92.2, 4080.0, true, false, false, Map.Tokuno, null, typeof(GiantSamuraiFish)),
            new MutateEntry(105.0, 105.0, 4080.0, true, false, false, Map.Tokuno, null, typeof(GoldenTuna)),
            new MutateEntry(92.2, 92.2, 4080.0, true, false, false, Map.Trammel, null, typeof(Kingfish)),
            new MutateEntry(92.2, 92.2, 4080.0, false, false, false, null, "Prism of Light", typeof(LanternFish)),
            new MutateEntry(92.2, 92.2, 4080.0, false, false, false, null, "Twisted Weald", typeof(RainbowFish)),
            new MutateEntry(92.2, 92.2, 4080.0, false, false, false, null, "Labyrinth", typeof(SeekerFish)),
            new MutateEntry(120.0, 120.0, 4080.0, false, false, false, Map.Ilshenar, null, typeof(SpringDragonfish)),
            new MutateEntry(92.2, 92.2, 4080.0, true, false, false, Map.Trammel, null, typeof(StoneFish)),
            new MutateEntry(120.0, 120.0, 4080.0, false, false, false, null, "Ice", typeof(WinterDragonfish)),
            new MutateEntry(92.2, 92.2, 4080.0, false, false, false, Map.Malas, null, typeof(ZombieFish)),
            // Rare
            new MutateEntry(105.4, 105.4, 4080.0, false, false, false, Map.Ilshenar, null, typeof(AutumnDragonfish)),
            new MutateEntry(0.0, 200.0, -200.0, false, false, false, null, "Labyrinth", typeof(BullFish)),
            new MutateEntry(0.0, 200.0, -200.0, false, false, false, null, "Prism of Light", typeof(CrystalFish)),
            new MutateEntry(85.8, 85.8, 4080.0, false, false, false, Map.TerMur, null, typeof(FairySalmon)),
            new MutateEntry(95.8, 95.8, 4080.0, false, false, false, null, "Shame", typeof(FireFish)),
            new MutateEntry(0.0, 200.0, -200.0, true, false, false, Map.Tokuno, null, typeof(GiantKoi)),
            new MutateEntry(89.0, 89.0, 4080.0, true, false, false, Map.Felucca, null, typeof(GreatBarracuda)),
            new MutateEntry(102.9, 102.9, 4080.0, false, false, false, Map.Malas, null, typeof(HolyMakerel)),
            new MutateEntry(110.0, 110.0, 4080.0, false, true, false, Map.TerMur, null, typeof(LavaFish)),
            new MutateEntry(0.0, 200.0, -200.0, false, false, false, null, "Doom", typeof(ReaperFish)),
            new MutateEntry(105.2, 105.2, -200.0, false, false, false, null, "Destard", typeof(SummerDragonfish)),
            new MutateEntry(110.0, 110.0, 4080.0, false, false, false, null, "Twisted Weald", typeof(SummerDragonfish)),
            new MutateEntry(81.9, 81.9, 4080.0, true, false, false, Map.Trammel, null, typeof(YellowtailBarracuda)),
            // Regualar 
            new MutateEntry(0.0, 200.0, -200.0, false, true, false, null, null, typeof(CragSnapper), typeof(CutthroatTrout), typeof(Darkfish), typeof(DemonTrout), typeof(DrakeFish), typeof(DungeonChub), typeof(GrimCisco),  typeof(InfernalTuna), typeof(LurkerFish), typeof(OrcBass), typeof(SnaggletoothBass), typeof(TormentedPike) ),
            new MutateEntry(0.0, 200.0, -200.0, true, false, false, null, null, typeof(Amberjack), typeof(BlackSeabass), typeof(BlueGrouper), typeof(MudPuppy), typeof(Bonefish), typeof(Bonito), typeof(FireFish), typeof(CaptainSnook), typeof(Cobia), typeof(GraySnapper), typeof(Haddock), typeof(MahiMahi), typeof(RedDrum), typeof(RedGrouper), typeof(RedSnook), typeof(Shad), typeof(GreatBarracuda), typeof(YellowfinTuna) ),
            new MutateEntry(0.0, 200.0, -200.0, false, false, false, null, null, typeof( BluegillSunfish ), typeof( BrookTrout ), typeof( GreenCatfish ), typeof( KokaneeSalmon ), typeof( PikeFish ), typeof( PumpkinseedSunfish ), typeof( RainbowTrout ), typeof( RedbellyBream ), typeof( SmallmouthBass ), typeof( UncommonShiner ), typeof( Walleye ), typeof( YellowPerch ) ),
           	// Normal
            new MutateEntry(0.0, 200.0, -200.0, false, false, false, null, null, new Type[1] { null })
        };

        public override bool SpecialHarvest(Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc)
        {
            PlayerMobile player = from as PlayerMobile;

                        Container pack = from.Backpack;

            if (player != null)
            {
                QuestSystem qs = player.Quest;

                if (qs is CollectorQuest)
                {
                    QuestObjective obj = qs.FindObjective(typeof(FishPearlsObjective));

                    if (obj != null && !obj.Completed)
                    {
                        if (Utility.RandomDouble() < 0.5)
                        {
                            player.SendLocalizedMessage(1055086, "", 0x59); // You pull a shellfish out of the water, and find a rainbow pearl inside of it.

                            obj.CurProgress++;
                        }
                        else
                        {
                            player.SendLocalizedMessage(1055087, "", 0x2C); // You pull a shellfish out of the water, but it doesn't have a rainbow pearl.
                        }

						return true;
					}
				}				
			}

            return false;
        }

        public override Type MutateType(Type type, Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, HarvestResource resource)
        {
            PlayerMobile pm = from as PlayerMobile;

            if (pm != null && pm.Region.IsPartOf("Underworld"))
            {
                if (QuestHelper.HasQuest<ScrapingTheBottomQuest>(pm) && 5 > Utility.Random(100))
                    return typeof(MudPuppy);

                if (QuestHelper.HasQuest<SomethingFishyQuest>(pm) && 5 > Utility.Random(100))
                    return typeof(RedHerring);
            }

            bool deepWater = SpecialFishingNet.FullValidation(map, loc.X, loc.Y);
            bool dungeon = false;
            Region region = from.Region;

            if (region is DungeonRegion)
                dungeon = true;

            double skillBase = from.Skills[SkillName.Fishing].Base;
            double skillValue = from.Skills[SkillName.Fishing].Value;

            if (FSHSR.HSRTournamentSystem.TournamentRunning() && skillValue > 60.0)
            {
                double chance = (skillValue - 60.0) / 300; // Default 20% chance at 120 / 13% chance at 100

                if (DateTime.UtcNow.DayOfWeek == FSHSR.HSRTournamentSystem.TournyOne && dungeon == true && chance > Utility.RandomDouble())
                {
                    return typeof(ToxicTrout);
                }
                else if (DateTime.UtcNow.DayOfWeek == FSHSR.HSRTournamentSystem.TournyTwo && deepWater == true && chance > Utility.RandomDouble())
                {
                    return typeof(CottonCandySwordfish);
                }
                else if (DateTime.UtcNow.DayOfWeek == FSHSR.HSRTournamentSystem.TournyThree && from.Map == Map.Malas && chance > Utility.RandomDouble())
                {
                    return typeof(MalasMoonfish);
                }
            }

            for (int i = 0; i < m_MutateTable.Length; ++i)
            {
                MutateEntry entry = m_MutateTable[i];

                if (!deepWater && entry.m_DeepWater)
                    continue;

                if (!dungeon && entry.m_RequiresDungeon)
                    continue;

                if (entry.m_RequiredRegion != null && region.Name != entry.m_RequiredRegion)
                    continue;

                if (entry.m_RequiredMap != null && map != entry.m_RequiredMap)
                    continue;

                if (skillBase >= entry.m_ReqSkill)
                {
                    double chance = (skillValue - entry.m_MinSkill) / (entry.m_MaxSkill - entry.m_MinSkill);

                    if (chance > Utility.RandomDouble())
                        return entry.m_Types[Utility.Random(entry.m_Types.Length)];
                }
            }

            return type;
        }

        private static Map SafeMap(Map map)
        {
            if (map == null || map == Map.Internal)
                return Map.Trammel;

            return map;
        }

        public override bool CheckResources(Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, bool timed)
        {
            Container pack = from.Backpack;

            if (pack != null)
            {
                List<SOS> messages = pack.FindItemsByType<SOS>();

                for (int i = 0; i < messages.Count; ++i)
                {
                    SOS sos = messages[i];

                    if ((from.Map == Map.Felucca || from.Map == Map.Trammel) && from.InRange(sos.TargetLocation, 60))
                        return true;
                }
            }

            return base.CheckResources(from, tool, def, map, loc, timed);
        }

        #region SOS and other construction
        public override Item Construct(Type type, Mobile from)
        {
            if (type == typeof(TreasureMap))
            {
                int level;
                if (from is PlayerMobile && ((PlayerMobile)from).Young && from.Map == Map.Trammel && TreasureMap.IsInHavenIsland(from))
                    level = 0;
                else
                    level = 1;

                return new TreasureMap(level, from.Map == Map.Felucca ? Map.Felucca : Map.Trammel);
            }
            else if (type == typeof(MessageInABottle))
            {
                return new MessageInABottle(from.Map == Map.Felucca ? Map.Felucca : Map.Trammel);
            }

            Container pack = from.Backpack;

            if (pack != null)
            {
                List<SOS> messages = pack.FindItemsByType<SOS>();

                for (int i = 0; i < messages.Count; ++i)
                {
                    SOS sos = messages[i];

                    if ((from.Map == Map.Felucca || from.Map == Map.Trammel) && from.InRange(sos.TargetLocation, 60))
                    {
                        Item preLoot = null;

                        if (Utility.RandomDouble() > 0.1)
                        {
                            switch (Utility.Random(20))
                            {
                                case 0: // Body parts
                                    {
                                        int[] list = new int[]
                                    {
                                        0x1CDD, 0x1CE5, // arm
                                        0x1CE0, 0x1CE8, // torso
                                        0x1CE1, 0x1CE9, // head
                                        0x1CE2, 0x1CEC // leg
                                    };

                                        preLoot = new ShipwreckedItem(Utility.RandomList(list));
                                        break;
                                    }
                                case 1: // Bone parts
                                    {
                                        int[] list = new int[]
                                    {
                                        0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3, 0x1AE4, // skulls
                                        0x1B09, 0x1B0A, 0x1B0B, 0x1B0C, 0x1B0D, 0x1B0E, 0x1B0F, 0x1B10, // bone piles
                                        0x1B15, 0x1B16 // pelvis bones
                                    };

                                        preLoot = new ShipwreckedItem(Utility.RandomList(list));
                                        break;
                                    }
                                case 2: // Paintings and portraits
                                    {
                                        if (Utility.RandomBool())
                                            preLoot = new ShipwreckedItem(Utility.Random(0xE9F, 10));
                                        else
                                            preLoot = new ShipwreckedItem(Utility.Random(0xEC8, 2));
                                        break;
                                    }
                                case 3: // Pillows
                                    {
                                        preLoot = new ShipwreckedItem(Utility.Random(0x13A4, 11));
                                        break;
                                    }
                                case 4: // Shells
                                    {
                                        preLoot = new ShipwreckedItem(Utility.Random(0xFC4, 9));
                                        break;
                                    }
                                case 5:	//Hats
                                    {
                                        if (Utility.RandomBool())
                                            preLoot = new SkullCap();
                                        else
                                            preLoot = new TricorneHat();

                                        break;
                                    }
                                case 6: // Misc
                                    {
                                        int[] list = new int[]
                                    {
                                        0x1EB5, // unfinished barrel
                                        0xA2A, // stool
                                        0xC1F, // broken clock
                                        0x1047, 0x1048, // globe
                                        0x1EB1, 0x1EB2, 0x1EB3, 0x1EB4 // barrel staves
                                    };

                                        if (Utility.Random(list.Length + 1) == 0)
                                            preLoot = new Candelabra();
                                        else
                                            preLoot = new ShipwreckedItem(Utility.RandomList(list));

                                        break;
                                    }
                                case 7:
                                    {
                                        switch (Utility.Random(4))
                                        {
                                            case 0:
                                                {
                                                    preLoot = new Shoes();
                                                    break;
                                                }
                                            case 1:
                                                {
                                                    preLoot = new Sandals();
                                                    break;
                                                }
                                            case 2:
                                                {
                                                    preLoot = new ThighBoots();
                                                    break;
                                                }
                                            case 3:
                                                {
                                                    preLoot = new Boots();
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                                case 8:
                                    {
                                        preLoot = new ShipwreckedItem(0x1E71);
                                        break;
                                    }
                                case 9:
                                    {
                                        preLoot = new ShipwreckedItem(Utility.Random(0x1E19, 3));
                                        break;
                                    }
                                case 10:
                                    {
                                        preLoot = new ShipwreckedItem(Utility.Random(0x1E2A, 2));
                                        break;
                                    }
                                case 11:
                                    {
                                        preLoot = new ShipwreckedItem(0x1E75);
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            switch (Utility.Random(7))
                            {
                                case 0:
                                    {
                                        preLoot = new YellowPolkadotBikiniTop();
                                        break;
                                    }
                                case 1:
                                    {
                                        preLoot = new BronzedArmorOfValkyrie();
                                        break;
                                    }
                                case 2:
                                    {
                                        preLoot = new EnchantedKelpWovenLeggings();
                                        break;
                                    }
                                case 3:
                                    {
                                        preLoot = new AntiqueWeddingDress();
                                        break;
                                    }
                                case 4:
                                    {
                                        preLoot = new RunedDriftwoodBow();
                                        break;
                                    }
                                case 5:
                                    {
                                        preLoot = new ShipwreckedItem(Utility.Random(0x0D1B, 10));
                                        break;
                                    }
                                case 6:
                                    {
                                        preLoot = new ShipwreckedItem(Utility.Random(0x1EA5, 2));

                                        //preLoot = new ShipwreckedItem(Utility.Random(0x1EA3, 4));
                                        break;
                                    }
                            }
                        }

                        if (preLoot != null)
                        {
                            if (preLoot is IShipwreckedItem)
                                ((IShipwreckedItem)preLoot).IsShipwreckedItem = true;

                            return preLoot;
                        }

                        LockableContainer chest;

                        if (Utility.RandomBool())
                            chest = new MetalGoldenChest();
                        else
                            chest = new WoodenChest();

                        if (sos.IsAncient)
                            chest.Hue = 0x481;

                        TreasureMapChest.Fill(chest, Math.Max(1, Math.Min(4, sos.Level)), null);

                        if (sos.IsAncient)
                            chest.DropItem(new FabledFishingNet());
                        else
                            chest.DropItem(new SpecialFishingNet());

                        chest.Movable = true;
                        chest.Locked = false;
                        chest.TrapEnabled = false;
                        chest.TrapType = TrapType.None;
                        chest.TrapPower = 0;
                        chest.TrapLevel = 0;
                        chest.IsShipwreckedItem = true;

                        sos.Delete();

                        return chest;
                    }
                }
            }

            return base.Construct(type, from);
        }
        #endregion

        public override bool Give(Mobile m, Item item, bool placeAtFeet)
        {
            if (item is TreasureMap || item is MessageInABottle || item is SpecialFishingNet)
            {
                BaseCreature serp;

                if (0.25 > Utility.RandomDouble())
                    serp = new DeepSeaSerpent();
                else
                    serp = new SeaSerpent();

                int x = m.X, y = m.Y;

                Map map = m.Map;

                for (int i = 0; map != null && i < 20; ++i)
                {
                    int tx = m.X - 10 + Utility.Random(21);
                    int ty = m.Y - 10 + Utility.Random(21);

                    LandTile t = map.Tiles.GetLandTile(tx, ty);

                    if (t.Z == -5 && ((t.ID >= 0xA8 && t.ID <= 0xAB) || (t.ID >= 0x136 && t.ID <= 0x137)) && !Spells.SpellHelper.CheckMulti(new Point3D(tx, ty, -5), map))
                    {
                        x = tx;
                        y = ty;
                        break;
                    }
                }

                serp.MoveToWorld(new Point3D(x, y, -5), map);

                serp.Home = serp.Location;
                serp.RangeHome = 10;

                serp.PackItem(item);

                m.SendLocalizedMessage(503170); // Uh oh! That doesn't look like a fish!

                return true; // we don't want to give the item to the player, it's on the serpent
            }

            if (item is BigFish || item is WoodenChest || item is MetalGoldenChest)
                placeAtFeet = true;

            return base.Give(m, item, placeAtFeet);
        }

        public override void SendSuccessTo(Mobile from, Item item, HarvestResource resource)
        {
            if (item is MudPuppy)
            {
                from.SendLocalizedMessage(1095064); // You take the Mud Puppy and put it into your pack.  Not surprisingly, it is very muddy.

                ((MudPuppy)item).Fisher = from;
            }
            else if (item is RedHerring)
            {
                from.SendLocalizedMessage(1095047); // You take the Red Herring and put it into your pack.  The only thing more surprising than the fact that there is a fish called the Red Herring is the fact that you fished for it!

                ((RedHerring)item).Fisher = from;
            }
            if (item is BigFish)
            {
                from.SendLocalizedMessage(1042635); // Your fishing pole bends as you pull a big fish from the depths!

                ((BigFish)item).Fisher = from;
            }
            else if (item is WoodenChest || item is MetalGoldenChest)
            {
                from.SendLocalizedMessage(503175); // You pull up a heavy chest from the depths of the ocean!
            }
            else
            {
                int number;
                string name;

                if (item is BaseMagicFish)
                {
                    number = 1008124;
                    name = "a mess of small fish";
                }
                else if (item is Fish)
                {
                    number = 1008124;
                    name = "a fish";
                }
                else if (item is BaseHighSeasFish)
                {
                    number = 1008124;
                    name = item.Name;
                }
                else if (item is BaseShoes)
                {
                    number = 1008124;
                    name = item.ItemData.Name;
                }
                else if (item is TreasureMap)
                {
                    number = 1008125;
                    name = "a sodden piece of parchment";
                }
                else if (item is MessageInABottle)
                {
                    number = 1008125;
                    name = "a bottle, with a message in it";
                }
                else if (item is SpecialFishingNet)
                {
                    number = 1008125;
                    name = "a special fishing net"; // TODO: this is just a guess--what should it really be named?
                }
                else
                {
                    number = 1043297;

                    if ((item.ItemData.Flags & TileFlag.ArticleA) != 0)
                        name = "a " + item.ItemData.Name;
                    else if ((item.ItemData.Flags & TileFlag.ArticleAn) != 0)
                        name = "an " + item.ItemData.Name;
                    else
                        name = item.ItemData.Name;
                }

                NetState ns = from.NetState;

                if (ns == null)
                    return;

                if (number == 1043297 || ns.HighSeas)
                    from.SendLocalizedMessage(number, name);
                else
                    from.SendLocalizedMessage(number, true, name);
            }
        }

        public override void OnHarvestStarted(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            base.OnHarvestStarted(from, tool, def, toHarvest);

            int tileID;
            Map map;
            Point3D loc;

            if (this.GetHarvestDetails(from, tool, toHarvest, out tileID, out map, out loc))
                Timer.DelayCall(TimeSpan.FromSeconds(1.5),
                    delegate
                    {
                        if (Core.ML)
                            from.RevealingAction();

                        Effects.SendLocationEffect(loc, map, 0x352D, 16, 4);
                        Effects.PlaySound(loc, map, 0x364);
                    });
        }

        public override void OnHarvestFinished(Mobile from, Item tool, HarvestDefinition def, HarvestVein vein, HarvestBank bank, HarvestResource resource, object harvested)
        {
            base.OnHarvestFinished(from, tool, def, vein, bank, resource, harvested);

            if (Core.ML)
            {
                from.RevealingAction();
                // High Seas Charybids Bait Method calling when the you finish fishing.
                OnCharybidsBait(from, tool.Baited, tool.BaitedMob);
            }
        }

        public override object GetLock(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            return this;
        }

        #region High Seas Baiting
        public void OnCharybidsBait(Mobile from, bool Baited, string BaitedMob)
        {
            int Chance = Utility.Random(100); // 25% chance to spawn Charybids
            int x = from.X, y = from.Y;
            Map map = from.Map;

            if (Baited == true && BaitedMob == "Charybids")
            {
                if (Chance >= 75 && from.Skills.Fishing.Value >= 120)
                {
                    BaseCreature Char = new Charybdis();

                    for (int i = 0; map != null && i < 20; ++i)
                    {
                        int tx = from.X - 10 + Utility.Random(21);
                        int ty = from.Y - 10 + Utility.Random(21);

                        LandTile t = map.Tiles.GetLandTile(tx, ty);

                        if (t.Z == -5 && ((t.ID >= 0xA8 && t.ID <= 0xAB) || (t.ID >= 0x136 && t.ID <= 0x137)) && !Spells.SpellHelper.CheckMulti(new Point3D(tx, ty, -5), map))
                        {
                            x = tx;
                            y = ty;
                            break;
                        }
                    }

                    Char.MoveToWorld(new Point3D(x, y, -5), map);

                    Char.Home = Char.Location;
                    Char.RangeHome = 10;
                }
                else
                {
                    from.SendLocalizedMessage(1150858); // You see a few bubbles, but no charybdis.
                    return;
                }
            }
            else
                return;
        }
        #endregion

        public override bool BeginHarvesting(Mobile from, Item tool)
        {
            if (!base.BeginHarvesting(from, tool))
                return false;

            from.SendLocalizedMessage(500974); // What water do you want to fish in?
            return true;
        }

        public override bool CheckHarvest(Mobile from, Item tool)
        {
            if (!base.CheckHarvest(from, tool))
                return false;

            if (from.Mounted)
            {
                from.SendLocalizedMessage(500971); // You can't fish while riding!
                return false;
            }

            return true;
        }

        public override bool CheckHarvest(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            if (!base.CheckHarvest(from, tool, def, toHarvest))
                return false;

            if (from.Mounted)
            {
                from.SendLocalizedMessage(500971); // You can't fish while riding!
                return false;
            }

            return true;
        }

        private static readonly int[] m_WaterTiles = new int[]
        {
            0x00A8, 0x00AB,
            0x0136, 0x0137,
            0x5797, 0x579C,
            0x746E, 0x7485,
            0x7490, 0x74AB,
            0x74B5, 0x75D5
        };
    }
}