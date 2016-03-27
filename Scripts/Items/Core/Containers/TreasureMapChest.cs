using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Server.ContextMenus;
using Server.Engines.PartySystem;
using Server.Misc;
using Server.Multis;
using Server.Gumps;
using Server.Network;
using Reward = Server.Engines.Quests.BaseReward;
using Server.FeaturesConfiguration;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Items
{
    public class TreasureMapChest : LockableContainer
    {
        private static readonly Type[] m_Artifacts = new Type[]
        {
            typeof(CandelabraOfSouls),      typeof(GoldBricks),          typeof(PhillipsWoodenSteed),
            typeof(ArcticDeathDealer),      typeof(BlazeOfDeath),        typeof(BurglarsBandana),
            typeof(CavortingClub),          typeof(DreadPirateHat),		 typeof(AdmiralHeartyRum),
            typeof(IolosLute),              typeof(GwennosHarp),         typeof(EnchantedTitanLegBone),
            typeof(LunaLance),              typeof(NightsKiss),          typeof(NoxRangersHeavyCrossbow),
            typeof(PolarBearMask),          typeof(VioletCourage),       typeof(ShieldOfInvulnerability),
            typeof(ColdBlood),              typeof(AlchemistsBauble),    typeof(CaptainQuacklebushsCutlass),
			typeof(ForgedPardon),           typeof(HeartOfTheLion),      typeof(ShipModelOfTheHMSCape)
        };

        private int m_Level;
        private DateTime m_DeleteTime;
        private Timer m_Timer;
        private Mobile m_Owner;
        private bool m_Temporary;
        private List<Mobile> m_Guardians;
        private List<Item> m_Lifted = new List<Item>();
        [Constructable]
        public TreasureMapChest(int level, Map map)
            : this(null, level, false, map )
        {
        }

        public TreasureMapChest(Mobile owner, int level, bool temporary, Map map)
            : base(0xE40)
        {
            m_Owner = owner;
            m_Level = level;
            m_DeleteTime = DateTime.UtcNow + TimeSpan.FromHours(3.0);

            m_Temporary = temporary;
            m_Guardians = new List<Mobile>();

            m_Timer = new DeleteTimer(this, m_DeleteTime);
            m_Timer.Start();

            Fill(this, level, map);
        }

        public TreasureMapChest(Serial serial)
            : base(serial)
        {
        }

        public static Type[] Artifacts
        {
            get
            {
                return m_Artifacts;
            }
        }
        public override int LabelNumber
        {
            get
            {
                return 3000541;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int Level
        {
            get
            {
                return m_Level;
            }
            set
            {
                m_Level = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get
            {
                return m_Owner;
            }
            set
            {
                m_Owner = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime DeleteTime
        {
            get
            {
                return m_DeleteTime;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool Temporary
        {
            get
            {
                return m_Temporary;
            }
            set
            {
                m_Temporary = value;
            }
        }
        public List<Mobile> Guardians
        {
            get
            {
                return m_Guardians;
            }
        }
        public override bool IsDecoContainer
        {
            get
            {
                return false;
            }
        }

        public static void Fill(LockableContainer cont, int level, Map map)
        {
            if (FeaturesConfig.FeatHStreasureMapsEnabled)
            {
                bool isFelucca = (cont.Map == Map.Felucca);
                bool inTokuno = (cont.Map == Map.Tokuno);
                bool SoS = false;

                if (cont.IsShipwreckedItem == true)
                {
                    SoS = true;
                }

                #region Lock & Trap
                cont.Movable = false;
                cont.Locked = true;
                int numberItems;

                if (level == 0)
                {
                    cont.LockLevel = 0; // Can't be unlocked

                    cont.DropItem(new Gold(Utility.RandomMinMax(500, 1000)));

                    if (Utility.RandomDouble() < 0.75)
                        cont.DropItem(new TreasureMap(0, Map.Trammel));
                }
                else
                {
                    cont.TrapType = TrapType.ExplosionTrap;
                    cont.TrapPower = level * 25;
                    cont.TrapLevel = level;
                    cont.TrapEnabled = true;

                    // = Chest Lock Levels (~40% to open at cont.RequiredSkill / 100% at cont.MaxLockLevel)
                    switch (level)
                    {
                        case 1: cont.RequiredSkill = 5; cont.LockLevel = -25; cont.MaxLockLevel = 55; break;
                        case 2: cont.RequiredSkill = 45; cont.LockLevel = 25; cont.MaxLockLevel = 75; break;
                        case 3: cont.RequiredSkill = 65; cont.LockLevel = 45; cont.MaxLockLevel = 95; break;
                        case 4: cont.RequiredSkill = 75; cont.LockLevel = 55; cont.MaxLockLevel = 105; break;
                        case 5: cont.RequiredSkill = 75; cont.LockLevel = 55; cont.MaxLockLevel = 105; break;
                        case 6: cont.RequiredSkill = 80; cont.LockLevel = 60; cont.MaxLockLevel = 110; break;
                        case 7: cont.RequiredSkill = 80; cont.LockLevel = 60; cont.MaxLockLevel = 110; break;
                    }
                #endregion

                    #region GOLD
                    cont.DropItem(new Gold(level * 5000));
                    #endregion

                    #region TREASURE MAP or MiB
                    if (!SoS)
                    {
                        if (Utility.RandomDouble() < 0.2)
                            cont.DropItem(new TreasureMap(level, MapItem.GetRandomFacet()));
                        else if ((level < 7) && (Utility.RandomDouble() < 0.025))
                            cont.DropItem(new TreasureMap(level + 1, MapItem.GetRandomFacet()));
                        else if (Utility.RandomDouble() > 0.8)
                        {
                            if (Utility.RandomDouble() > 0.5)
                                cont.DropItem(new MessageInABottle(Map.Felucca));
                            else
                                cont.DropItem(new MessageInABottle(Map.Trammel));
                        }
                    }
                    else if (!SoS)
                    {
                        int soslvl = Utility.Random(level) + 1;

                        if (Utility.RandomDouble() < 0.2)
                            cont.DropItem(new TreasureMap(soslvl, MapItem.GetRandomFacet()));
                    }
                    #endregion

                    #region LEVEL 8 ARCANE SCROLLS
                    for (int i = 0; i < level; ++i)
                    {
                        cont.DropItem(Loot.RandomScroll(57, 64, SpellbookType.Regular));
                    }
                    #endregion

                    #region MAGIC ITEMS
                    Item item;

                    numberItems = 24 + (level * 8);
                    for (int i = 0; i < numberItems; ++i)
                    {
                        if (Core.AOS)
                            item = Loot.RandomArmorOrShieldOrWeaponOrJewelry(inTokuno, false);
                        else
                            item = Loot.RandomArmorOrShieldOrWeapon(inTokuno, false);

                        if (item is BaseWeapon)
                        {
                            BaseWeapon weapon = (BaseWeapon)item;

                            if (Core.AOS)
                            {
                                int attributeCount;
                                int min, max;

                                GetRandomAOSStatsHS(level, out attributeCount, out min, out max);

                                BaseRunicTool.ApplyAttributesTo(weapon, attributeCount, min, max);
                            }
                            else
                            {
                                weapon.DamageLevel = (WeaponDamageLevel)Utility.Random(6);
                                weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random(6);
                                weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random(6);
                            }

                            cont.DropItem(item);
                        }
                        else if (item is BaseArmor)
                        {
                            BaseArmor armor = (BaseArmor)item;

                            if (Core.AOS)
                            {
                                int attributeCount;
                                int min, max;

                                GetRandomAOSStatsHS(level, out attributeCount, out min, out max);

                                BaseRunicTool.ApplyAttributesTo(armor, attributeCount, min, max);
                            }
                            else
                            {
                                armor.ProtectionLevel = (ArmorProtectionLevel)Utility.Random(6);
                                armor.Durability = (ArmorDurabilityLevel)Utility.Random(6);
                            }

                            cont.DropItem(item);
                        }
                        else if (item is BaseHat)
                        {
                            BaseHat hat = (BaseHat)item;

                            if (Core.AOS)
                            {
                                int attributeCount;
                                int min, max;

                                GetRandomAOSStatsHS(level, out attributeCount, out min, out  max);

                                BaseRunicTool.ApplyAttributesTo(hat, attributeCount, min, max);
                            }

                            cont.DropItem(item);
                        }
                        else if (item is BaseJewel)
                        {
                            int attributeCount;
                            int min, max;

                            GetRandomAOSStatsHS(level, out attributeCount, out min, out max);

                            BaseRunicTool.ApplyAttributesTo((BaseJewel)item, attributeCount, min, max);

                            cont.DropItem(item);
                        }
                    }
                }
                    #endregion

                #region REAGENTS
                int reagents;
                if (level == 0)
                {
                    reagents = 10;
                }
                else
                {
                    reagents = 3 + level;
                }

                for (int i = 0; i < reagents; i++)
                {
                    Item itr = Loot.RandomPossibleReagent();
                    itr.Amount = Utility.RandomMinMax(40, 60);
                    cont.DropItem(itr);
                }

                #endregion

                #region Gems
                int gemCount = level * 3;

                for (int i = 0; i < gemCount; i++)
                {
                    Item item = Loot.RandomGem();
                    cont.DropItem(item);
                }
                #endregion

                #region Essences
                if (level >= 2)
                {
                    int essenceCount = level;

                    for (int i = 0; i < essenceCount; i++)
                    {
                        Item item = Loot.RandomEssence();
                        cont.DropItem(item);
                    }
                }
                #endregion
                /*
                #region IMBUING INGREDIENTS
                if (level >= 2)
                {
                    if (!SoS)
                    {
                        Item ire = new EssencePrecision(level);
                        int rndE = Utility.Random(13) + 1;
                        if (rndE == 1) { ire = new EssencePrecision(level); }
                        if (rndE == 2) { ire = new EssenceAchievement(level); }
                        if (rndE == 3) { ire = new EssenceBalance(level); }
                        if (rndE == 4) { ire = new EssenceControl(level); }
                        if (rndE == 5) { ire = new EssenceDiligence(level); }
                        if (rndE == 6) { ire = new EssenceDirection(level); }
                        if (rndE == 7) { ire = new EssenceFeeling(level); }
                        if (rndE == 8) { ire = new EssenceOrder(level); }
                        if (rndE == 9) { ire = new EssencePassion(level); }
                        if (rndE == 10) { ire = new EssencePersistence(level); }
                        if (rndE == 11) { ire = new EssenceSingularity(level); }
                        if (rndE >= 12) { ire = new AbyssalCloth(level); }
                        cont.DropItem(ire);
                    }
                    else if (SoS && Utility.RandomDouble() < 0.25)
                    {
                        Item ire = new EssencePrecision();
                        int rndE = Utility.Random(49) + 1;
                        if (rndE <= 2) { ire = new EssencePrecision(Utility.Random(level) + 1); }
                        else if (rndE <= 4) { ire = new EssenceAchievement(Utility.Random(level) + 1); }
                        else if (rndE <= 6) { ire = new EssenceBalance(Utility.Random(level) + 1); }
                        else if (rndE <= 8) { ire = new EssenceControl(Utility.Random(level) + 1); }
                        else if (rndE <= 10) { ire = new EssenceDiligence(Utility.Random(level) + 1); }
                        else if (rndE <= 12) { ire = new EssenceDirection(Utility.Random(level) + 1); }
                        else if (rndE <= 14) { ire = new EssenceFeeling(Utility.Random(level) + 1); }
                        else if (rndE <= 16) { ire = new EssenceOrder(Utility.Random(level) + 1); }
                        else if (rndE <= 18) { ire = new EssencePassion(Utility.Random(level) + 1); }
                        else if (rndE <= 20) { ire = new EssencePersistence(Utility.Random(level) + 1); }
                        else if (rndE <= 22) { ire = new EssenceSingularity(Utility.Random(level) + 1); }
                        else if (rndE == 23) { ire = new AbyssalCloth(Utility.Random(level) + 1); }
                        else if (rndE == 24) { ire = new ArcanicRuneStone(); }
                        else if (rndE == 25) { ire = new BottleIchor(); }
                        else if (rndE == 26) { ire = new ChagaMushroom(Utility.Random(level) + 1); }
                        else if (rndE == 27) { ire = new CrushedGlass(); }
                        else if (rndE == 28) { ire = new CrystalShards(); }
                        else if (rndE == 29) { ire = new CrystallineBlackrock(); }
                        else if (rndE == 30) { ire = new DaemonClaw(Utility.Random(level) + 1); }
                        else if (rndE == 31) { ire = new ElvenFletchings(); }
                        else if (rndE == 32) { ire = new FaeryDust(); }
                        else if (rndE == 33) { ire = new GoblinBlood(Utility.Random(level) + 1); }
                        else if (rndE == 34) { ire = new LavaSerpenCrust(Utility.Random(level) + 1); }
                        else if (rndE == 35) { ire = new PowderedIron(); }
                        else if (rndE == 36) { ire = new RaptorTeeth(Utility.Random(level) + 1); }
                        else if (rndE == 37) { ire = new ReflectiveWolfEye(); }
                        else if (rndE == 38) { ire = new SeedRenewal(); }
                        else if (rndE == 39) { ire = new SilverSerpentVenom(Utility.Random(level) + 1); }
                        else if (rndE == 40) { ire = new SilverSnakeSkin(); }
                        else if (rndE == 41) { ire = new SlithTongue(Utility.Random(level) + 1); }
                        else if (rndE == 42) { ire = new SpiderCarapace(Utility.Random(level) + 1); }
                        else if (rndE == 43) { ire = new UndyingFlesh(Utility.Random(level) + 1); }
                        else if (rndE == 44) { ire = new VialOfVitriol(Utility.Random(level) + 1); }
                        else if (rndE == 45) { ire = new VoidOrb(); }
                        else if (rndE <= 47) { ire = new WhitePearl(Utility.Random(level) + 1); }
                        else if (rndE <= 49) { ire = new DelicateScales(Utility.Random(level) + 1); }

                        cont.DropItem(ire);
                    }
                }
                #endregion
                 */

                #region Special loot
                if (0.2 > Utility.RandomDouble())
                    cont.DropItem(ScrollOfAlacrity.CreateRandom());

                if (map == Map.Felucca && (level * 0.15) > Utility.RandomDouble())
                {
                    Item item = ScrollOfTranscendence.CreateRandom(5);
                    cont.DropItem(item);
                }

                if ((level * 0.05) > Utility.RandomDouble())
                    cont.DropItem(new CreepingVines());

                if ((level * 0.1) > Utility.RandomDouble())
                    cont.DropItem(new TastyTreat());

                if ((level * 0.05) > Utility.RandomDouble())
                    cont.DropItem(BaseReward.RandomRecipe());

                if (0.5 > Utility.RandomDouble())
                    cont.DropItem(new TreasureMap(level < 6 && 0.25 > Utility.RandomDouble() ? level + 1 : level));

                if (0.25 > Utility.RandomDouble())
                    cont.DropItem(new SFSkeletonKey());

                if (0.2 > Utility.RandomDouble())
                    cont.DropItem(new MessageInABottle());

                if (level >= 5)
                {
                    if (0.1 > Utility.RandomDouble())
                        cont.DropItem(new ForgedPardon());

                    //if (0.09 > Utility.RandomDouble())
                        //cont.DropItem(new ManaPhasingOrb());

                    //if (0.09 > Utility.RandomDouble())
                        //cont.DropItem(new RunedSashOfWarding());

                    //if (0.09 > Utility.RandomDouble())
                        //cont.DropItem(map == Map.TerMur ? new GargishSurgeShield() : new SurgeShield());
                }
                #endregion

                #region Artifacts
                if (level >= 6)
                {
                    Item item = (Item)Activator.CreateInstance(m_Artifacts[Utility.Random(m_Artifacts.Length)]);
                    cont.DropItem(item);
                }
                #endregion
            }
            else
            {
                cont.Movable = false;
                cont.Locked = true;
                int numberItems;

                if (level == 0)
                {
                    cont.LockLevel = 0; // Can't be unlocked

                    cont.DropItem(new Gold(Utility.RandomMinMax(50, 100)));

                    if (Utility.RandomDouble() < 0.75)
                        cont.DropItem(new TreasureMap(0, Map.Trammel));
                }
                else
                {
                    cont.TrapType = TrapType.ExplosionTrap;
                    cont.TrapPower = level * 25;
                    cont.TrapLevel = level;

                    switch (level)
                    {
                        case 1:
                            cont.RequiredSkill = 36;
                            break;
                        case 2:
                            cont.RequiredSkill = 76;
                            break;
                        case 3:
                            cont.RequiredSkill = 84;
                            break;
                        case 4:
                            cont.RequiredSkill = 92;
                            break;
                        case 5:
                            cont.RequiredSkill = 100;
                            break;
                        case 6:
                            cont.RequiredSkill = 100;
                            break;
                    }

                    cont.LockLevel = cont.RequiredSkill - 10;
                    cont.MaxLockLevel = cont.RequiredSkill + 40;

                    cont.DropItem(new Gold(level * 1000));

                    for (int i = 0; i < level * 5; ++i)
                        cont.DropItem(Loot.RandomScroll(0, 63, SpellbookType.Regular));

                    if (Core.SE)
                    {
                        switch (level)
                        {
                            case 1:
                                numberItems = 5;
                                break;
                            case 2:
                                numberItems = 10;
                                break;
                            case 3:
                                numberItems = 15;
                                break;
                            case 4:
                                numberItems = 38;
                                break;
                            case 5:
                                numberItems = 50;
                                break;
                            case 6:
                                numberItems = 60;
                                break;
                            default:
                                numberItems = 0;
                                break;
                        }
                    }
                    else
                        numberItems = level * 6;

                    for (int i = 0; i < numberItems; ++i)
                    {
                        Item item;

                        if (Core.AOS)
                            item = Loot.RandomArmorOrShieldOrWeaponOrJewelry();
                        else
                            item = Loot.RandomArmorOrShieldOrWeapon();

                        if (item is BaseWeapon)
                        {
                            BaseWeapon weapon = (BaseWeapon)item;

                            if (Core.AOS)
                            {
                                int attributeCount;
                                int min, max;

                                GetRandomAOSStats(out attributeCount, out min, out max);

                                BaseRunicTool.ApplyAttributesTo(weapon, attributeCount, min, max);
                            }
                            else
                            {
                                weapon.DamageLevel = (WeaponDamageLevel)Utility.Random(6);
                                weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random(6);
                                weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random(6);
                            }

                            cont.DropItem(item);
                        }
                        else if (item is BaseArmor)
                        {
                            BaseArmor armor = (BaseArmor)item;

                            if (Core.AOS)
                            {
                                int attributeCount;
                                int min, max;

                                GetRandomAOSStats(out attributeCount, out min, out max);

                                BaseRunicTool.ApplyAttributesTo(armor, attributeCount, min, max);
                            }
                            else
                            {
                                armor.ProtectionLevel = (ArmorProtectionLevel)Utility.Random(6);
                                armor.Durability = (ArmorDurabilityLevel)Utility.Random(6);
                            }

                            cont.DropItem(item);
                        }
                        else if (item is BaseHat)
                        {
                            BaseHat hat = (BaseHat)item;

                            if (Core.AOS)
                            {
                                int attributeCount;
                                int min, max;

                                GetRandomAOSStats(out attributeCount, out min, out max);

                                BaseRunicTool.ApplyAttributesTo(hat, attributeCount, min, max);
                            }

                            cont.DropItem(item);
                        }
                        else if (item is BaseJewel)
                        {
                            int attributeCount;
                            int min, max;

                            GetRandomAOSStats(out attributeCount, out min, out max);

                            BaseRunicTool.ApplyAttributesTo((BaseJewel)item, attributeCount, min, max);

                            cont.DropItem(item);
                        }
                    }
                }
                int reagents;
                if (level == 0)
                    reagents = 12;
                else
                    reagents = level * 3;

                for (int i = 0; i < reagents; i++)
                {
                    Item item = Loot.RandomPossibleReagent();
                    item.Amount = Utility.RandomMinMax(40, 60);
                    cont.DropItem(item);
                }

                int gems;
                if (level == 0)
                    gems = 2;
                else
                    gems = level * 3;

                for (int i = 0; i < gems; i++)
                {
                    Item item = Loot.RandomGem();
                    cont.DropItem(item);
                }

                if (level == 6 && Core.AOS)
                    cont.DropItem((Item)Activator.CreateInstance(m_Artifacts[Utility.Random(m_Artifacts.Length)]));
            }
        }

        public override bool CheckLocked(Mobile from)
        {
            if (!Locked)
                return false;

            if (Level == 0 && from.AccessLevel < AccessLevel.GameMaster)
            {
                foreach (Mobile m in Guardians)
                {
                    if (m.Alive)
                    {
                        if (Core.HS)
                        {
                            from.SendLocalizedMessage(1116233); // You must defeat the guardians of the chest before you can open it.
                            return false;
                        }
                        else
                        {
                            from.SendLocalizedMessage(1046448); // You must first kill the guardians before you may open this chest.
                            return true;
                        }
                    }
                }

                LockPick(from);
                return false;
            }
            else
            {
                return base.CheckLocked(from);
            }
        }

        public override bool CheckItemUse(Mobile from, Item item)
        {
            return CheckLoot(from, item != this) && base.CheckItemUse(from, item);
        }

        public override bool CheckLift(Mobile from, Item item, ref LRReason reject)
        {
            return CheckLoot(from, true) && base.CheckLift(from, item, ref reject);
        }

        public override void OnItemLifted(Mobile from, Item item)
        {
            bool notYetLifted = !m_Lifted.Contains(item);

            from.RevealingAction();

            if (notYetLifted)
            {
                m_Lifted.Add(item);

                if (0.1 >= Utility.RandomDouble()) // 10% chance to spawn a new monster
                    TreasureMap.Spawn(m_Level, GetWorldLocation(), Map, from, false);
            }

            base.OnItemLifted(from, item);
        }

        public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
        {
            if (m.AccessLevel < AccessLevel.GameMaster)
            {
                m.SendLocalizedMessage(1048122, "", 0x8A5); // The chest refuses to be filled with treasure again.
                return false;
            }

            return base.CheckHold(m, item, message, checkItems, plusItems, plusWeight);
        }

        public override int LockPick(Mobile from)
        {
            if (Picker == null && 0.05 > Utility.RandomDouble())
            {
                Item item = PickRandomItemFromChest();

                if (item != null)
                {
                    Grubber grubber = new Grubber();
                    grubber.PackItem(item);

                    grubber.MoveToWorld(TreasureMap.GetRandomSpawnLocation(Location, Map), Map);

                    grubber.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "*a grubber appears and ganks a piece of your loot*");
                }
            }

            return base.LockPick(from);
        }

        private Item PickRandomItemFromChest()
        {
            var items = Items.Where(i => (!(i is Gold) && !(i is DustPile))).ToArray();
            return items[Utility.Random(items.Length)];
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2); // version

            writer.Write(m_Guardians, true);
            writer.Write((bool)m_Temporary);

            writer.Write(m_Owner);

            writer.Write((int)m_Level);
            writer.WriteDeltaTime(m_DeleteTime);
            writer.Write(m_Lifted, true);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 2:
                    {
                        m_Guardians = reader.ReadStrongMobileList();
                        m_Temporary = reader.ReadBool();

                        goto case 1;
                    }
                case 1:
                    {
                        m_Owner = reader.ReadMobile();

                        goto case 0;
                    }
                case 0:
                    {
                        m_Level = reader.ReadInt();
                        m_DeleteTime = reader.ReadDeltaTime();
                        m_Lifted = reader.ReadStrongItemList();

                        if (version < 2)
                            m_Guardians = new List<Mobile>();

                        break;
                    }
            }

            if (!m_Temporary)
            {
                m_Timer = new DeleteTimer(this, m_DeleteTime);
                m_Timer.Start();
            }
            else
            {
                Delete();
            }
        }

        public override void OnAfterDelete()
        {
            if (m_Timer != null)
                m_Timer.Stop();

            m_Timer = null;

            base.OnAfterDelete();
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.Alive)
                list.Add(new RemoveEntry(from, this));
        }

        public void BeginRemove(Mobile from)
        {
            if (!from.Alive)
                return;

            from.CloseGump(typeof(RemoveGump));
            from.SendGump(new RemoveGump(from, this));
        }

        public void EndRemove(Mobile from)
        {
            if (Deleted || from != m_Owner || !from.InRange(GetWorldLocation(), 3))
                return;

            from.SendLocalizedMessage(1048124, "", 0x8A5); // The old, rusted chest crumbles when you hit it.
            Delete();
        }

        private static void GetRandomAOSStats(out int attributeCount, out int min, out int max)
        {
            int rnd = Utility.Random(15);
			
            if (Core.SE)
            {
                if (rnd < 1)
                {
                    attributeCount = Utility.RandomMinMax(3, 5);
                    min = 50;
                    max = 100;
                }
                else if (rnd < 3)
                {
                    attributeCount = Utility.RandomMinMax(2, 5);
                    min = 40;
                    max = 80;
                }
                else if (rnd < 6)
                {
                    attributeCount = Utility.RandomMinMax(2, 4);
                    min = 30;
                    max = 60;
                }
                else if (rnd < 10)
                {
                    attributeCount = Utility.RandomMinMax(1, 3);
                    min = 20;
                    max = 40;
                }
                else
                {
                    attributeCount = 1;
                    min = 10;
                    max = 20;
                }
            }
            else
            {
                if (rnd < 1)
                {
                    attributeCount = Utility.RandomMinMax(2, 5);
                    min = 20;
                    max = 70;
                }
                else if (rnd < 3)
                {
                    attributeCount = Utility.RandomMinMax(2, 4);
                    min = 20;
                    max = 50;
                }
                else if (rnd < 6)
                {
                    attributeCount = Utility.RandomMinMax(2, 3);
                    min = 20;
                    max = 40;
                }
                else if (rnd < 10)
                {
                    attributeCount = Utility.RandomMinMax(1, 2);
                    min = 10;
                    max = 30;
                }
                else
                {
                    attributeCount = 1;
                    min = 10;
                    max = 20;
                }
            }
        }

        private static void GetRandomAOSStatsHS(int lvl, out int attributeCount, out int min, out int max)
        {
            int rnd = Utility.Random(15);

            if (rnd == 0)
            {
                attributeCount = Utility.RandomMinMax(5, 6);
                min = 40 + (lvl * 5); max = 100;
            }
            if (rnd <= 1)
            {
                attributeCount = Utility.RandomMinMax(4, 6);
                min = 25 + (lvl * 5); max = 100;
            }
            else if (rnd <= 4)
            {
                attributeCount = Utility.RandomMinMax(3, 5);
                min = 15 + (lvl * 5); max = 100;
            }
            else if (rnd <= 7)
            {
                attributeCount = Utility.RandomMinMax(2, 4);
                min = 10 + (lvl * 5); max = 100;
            }
            else if (rnd <= 11)
            {
                attributeCount = Utility.RandomMinMax(2, 3);
                min = 10 + (lvl * 5); max = 100;
            }
            else
            {
                attributeCount = Utility.RandomMinMax(1, 3);
                min = 10 + (lvl * 4); max = 100;
            }
        }

        private bool CheckLoot(Mobile m, bool criminalAction)
        {
            if (m_Temporary)
                return false;

            if (m.AccessLevel >= AccessLevel.GameMaster || m_Owner == null || m == m_Owner)
                return true;

            Party p = Party.Get(m_Owner);

            if (p != null && p.Contains(m))
                return true;

            Map map = Map;

            if (map != null && (map.Rules & MapRules.HarmfulRestrictions) == 0)
            {
                if (criminalAction)
                    m.CriminalAction(true);
                else
                    m.SendLocalizedMessage(1010630); // Taking someone else's treasure is a criminal offense!

                return true;
            }

            m.SendLocalizedMessage(1010631); // You did not discover this chest!
            return false;
        }

        private class RemoveGump : Gump
        {
            private readonly Mobile m_From;
            private readonly TreasureMapChest m_Chest;
            public RemoveGump(Mobile from, TreasureMapChest chest)
                : base(15, 15)
            {
                m_From = from;
                m_Chest = chest;
                Closable = false;
                Disposable = false;
                AddPage(0);
                AddBackground(30, 0, 240, 240, 2620);
                AddHtmlLocalized(45, 15, 200, 80, 1048125, 0xFFFFFF, false, false); // When this treasure chest is removed, any items still inside of it will be lost.
                AddHtmlLocalized(45, 95, 200, 60, 1048126, 0xFFFFFF, false, false); // Are you certain you're ready to remove this chest?
                AddButton(40, 153, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddHtmlLocalized(75, 155, 180, 40, 1048127, 0xFFFFFF, false, false); // Remove the Treasure Chest
                AddButton(40, 195, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddHtmlLocalized(75, 197, 180, 35, 1006045, 0xFFFFFF, false, false); // Cancel
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (info.ButtonID == 1)
                    m_Chest.EndRemove(m_From);
            }
        }

        private class RemoveEntry : ContextMenuEntry
        {
            private readonly Mobile m_From;
            private readonly TreasureMapChest m_Chest;
            public RemoveEntry(Mobile from, TreasureMapChest chest)
                : base(6149, 3)
            {
                m_From = from;
                m_Chest = chest;

                Enabled = (from == chest.Owner);
            }

            public override void OnClick()
            {
                if (m_Chest.Deleted || m_From != m_Chest.Owner || !m_From.CheckAlive())
                    return;

                m_Chest.BeginRemove(m_From);
            }
        }

        private class DeleteTimer : Timer
        {
            private readonly Item m_Item;
            public DeleteTimer(Item item, DateTime time)
                : base(time - DateTime.UtcNow)
            {
                m_Item = item;
                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                m_Item.Delete();
            }
        }
    }
}