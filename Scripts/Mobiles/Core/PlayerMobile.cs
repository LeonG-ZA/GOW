#region References
using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Server.Buff.Icons;
using Server.Accounting;
using Server.ContextMenus;
using Server.Engines.BulkOrders;
using Server.Engines.ChampionSpawns;
using Server.Engines.ConPVP;
using Server.Engines.Craft;
using Server.Engines.Help;
using Server.Engines.MyShard;
using Server.Engines.PartySystem;
using Server.Engines.Quests;
using Server.Engines.XmlSpawner2;
using Server.Ethics;
using Server.Factions;
using Server.Guilds;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Movement;
using Server.Multis;
using Server.Network;
using Server.Regions;
using Server.Engines.Loyalty;
using Server.SkillHandlers;
using Server.Spells;
using Server.Spells.Bushido;
using Server.Spells.Fifth;
using Server.Spells.Fourth;
using Server.Spells.Necromancy;
using Server.Spells.Ninjitsu;
using Server.Spells.Seventh;
using Server.Spells.Sixth;
using Server.Spells.Spellweaving;
using Server.Targeting;
using Server.MainConfiguration;
using RankDefinition = Server.Guilds.RankDefinition;
using Server.CharConfiguration;
using Server.LogConfiguration;
using System.Threading;
using Server.FeaturesConfiguration;
using Server.Spells.Bard;
using Server.Engines.Collections;
using Server.Engines.Quests.HumilityCloak;
using Server.TestCenterConfiguration;
#endregion

namespace Server.Mobiles
{
    #region Enums
    [Flags]
    public enum PlayerFlag
    {
        None = 0x00000000,
        Glassblowing = 0x00000001,
        Masonry = 0x00000002,
        SandMining = 0x00000004,
        StoneMining = 0x00000008,
        ToggleMiningStone = 0x00000010,
        KarmaLocked = 0x00000020,
        AutoRenewInsurance = 0x00000040,
        UseOwnFilter = 0x00000080,
        PublicMyShard = 0x00000100,
        PagingSquelched = 0x00000200,
        Young = 0x00000400,
        AcceptGuildInvites = 0x00000800,
        DisplayChampionTitle = 0x00001000,
        HasStatReward = 0x00002000,
        RefuseTrades = 0x00004000,
        Bedlam = 0x00010000,

        Arcanist = 0x00060000,
        GemMining = 0x00080000,
        ToggleMiningGem = 0x00100000,
        BasketWeaving = 0x00200000,
        AbyssEntry = 0x00400000,
        CanSummonFiend = 0x00600000,
        CanSummonFey = 0x00080000,
        FriendOfTheLibrary = 0x01000000,

        MechanicalLife = 0x04000000,
        Parox = 0x08000000,
        SacredQuest = 0x10000000,
        DisabledPvpWarning = 0x20000000,
        CanBuyCarpets = 0x40000000,
    }

    public enum NpcGuild
    {
        None,
        MagesGuild,
        WarriorsGuild,
        ThievesGuild,
        RangersGuild,
        HealersGuild,
        MinersGuild,
        MerchantsGuild,
        TinkersGuild,
        TailorsGuild,
        FishermensGuild,
        BardsGuild,
        BlacksmithsGuild
    }

    public enum SolenFriendship
    {
        None,
        Red,
        Black
    }
    #endregion

    public class PlayerMobile : Mobile, IHonorTarget
    {
        #region Mount Blocking
        public void SetMountBlock(BlockMountType type, TimeSpan duration, bool dismount)
        {
            if (dismount)
            {
                BaseMount.Dismount(this, this, type, duration, false);
            }
            else
            {
                BaseMount.SetMountPrevention(this, type, duration);
            }
        }
        #endregion

        private class CountAndTimeStamp
        {
            private int m_Count;
            private DateTime m_Stamp;

            public DateTime TimeStamp { get { return m_Stamp; } }

            public int Count
            {
                get { return m_Count; }
                set
                {
                    m_Count = value;
                    m_Stamp = DateTime.UtcNow;
                }
            }
        }

        private DesignContext m_DesignContext;

        private NpcGuild m_NpcGuild;
        private DateTime m_NpcGuildJoinTime;
        private TimeSpan m_NpcGuildGameTime;
        private PlayerFlag m_Flags;
        private int m_Profession;

        public AdvancedCharacterState ACState;

        public int Deaths;

        // Sacred Quest
        private DateTime m_SacredQuestNextChance;

        // Humility Cloak Quest
        private HumilityQuestStatus m_HumilityQuestStatus;
        private DateTime m_HumilityQuestNextChance;

        // Doom Artifacts
        private int m_DoomCredits;

        private int m_NonAutoreinsuredItems;
        // number of items that could not be automaitically reinsured because gold in bank was not enough

        /*
        * a value of zero means, that the mobile is not executing the spell. Otherwise,
        * the value should match the BaseMana required
        */
        private int m_ExecutesLightningStrike; // move to Server.Mobiles??

        private string m_LastPromotionCode;

        [CommandProperty(AccessLevel.Administrator)]
        public string LastPromotionCode { get { return m_LastPromotionCode; } set { m_LastPromotionCode = value; } }

        private DateTime m_LastOnline;
        private RankDefinition m_GuildRank;

        private bool m_NextEnhanceSuccess;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool NextEnhanceSuccess { get { return m_NextEnhanceSuccess; } set { m_NextEnhanceSuccess = value; } }


        private int m_GuildMessageHue, m_AllianceMessageHue;

        private List<Mobile> m_AutoStabled;
        private List<Mobile> m_AllFollowers;
        private List<Mobile> m_RecentlyReported;
        private DateTime m_PromoGiftLast;
        private DateTime m_LastTimePaged;

        #region Getters & Setters
        public List<Mobile> RecentlyReported { get { return m_RecentlyReported; } set { m_RecentlyReported = value; } }

        public List<Mobile> AutoStabled { get { return m_AutoStabled; } }

        public bool NinjaWepCooldown { get; set; }

        public List<Mobile> AllFollowers
        {
            get
            {
                if (m_AllFollowers == null)
                {
                    m_AllFollowers = new List<Mobile>();
                }
                return m_AllFollowers;
            }
        }

        public RankDefinition GuildRank
        {
            get
            {
                if (AccessLevel >= AccessLevel.GameMaster)
                {
                    return RankDefinition.Leader;
                }
                else
                {
                    return m_GuildRank;
                }
            }
            set { m_GuildRank = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int GuildMessageHue { get { return m_GuildMessageHue; } set { m_GuildMessageHue = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AllianceMessageHue { get { return m_AllianceMessageHue; } set { m_AllianceMessageHue = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Profession { get { return m_Profession; } set { m_Profession = value; } }

        public int StepsTaken { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public NpcGuild NpcGuild { get { return m_NpcGuild; } set { m_NpcGuild = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NpcGuildJoinTime { get { return m_NpcGuildJoinTime; } set { m_NpcGuildJoinTime = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextBODTurnInTime { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastOnline { get { return m_LastOnline; } set { m_LastOnline = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public long LastMoved { get { return LastMoveTime; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan NpcGuildGameTime { get { return m_NpcGuildGameTime; } set { m_NpcGuildGameTime = value; } }

        // Doom Artifacts
        [CommandProperty(AccessLevel.GameMaster)]
        public int DoomCredits
        {
            get { return m_DoomCredits; }
            set { m_DoomCredits = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime PromoGiftLast
        {
            get { return m_PromoGiftLast; }
            set { m_PromoGiftLast = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastTimePaged
        {
            get { return m_LastTimePaged; }
            set { m_LastTimePaged = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime SacredQuestNextChance
        {
            get { return m_SacredQuestNextChance; }
            set { m_SacredQuestNextChance = value; }
        }

        // Humility Cloak Quest
        [CommandProperty(AccessLevel.GameMaster)]
        public HumilityQuestStatus HumilityQuestStatus
        {
            get { return m_HumilityQuestStatus; }
            set { m_HumilityQuestStatus = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime HumilityQuestNextChance
        {
            get { return m_HumilityQuestNextChance; }
            set { m_HumilityQuestNextChance = value; }
        }


        [CommandProperty(AccessLevel.GameMaster)]
        public bool DisabledPvpWarning
        {
            get { return GetFlag(PlayerFlag.DisabledPvpWarning); }
            set { SetFlag(PlayerFlag.DisabledPvpWarning, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanBuyCarpets
        {
            get { return GetFlag(PlayerFlag.CanBuyCarpets); }
            set { SetFlag(PlayerFlag.CanBuyCarpets, value); }
        }

        private int m_ToTItemsTurnedIn;

        [CommandProperty(AccessLevel.GameMaster)]
        public int ToTItemsTurnedIn { get { return m_ToTItemsTurnedIn; } set { m_ToTItemsTurnedIn = value; } }

        private int m_ToTTotalMonsterFame;

        [CommandProperty(AccessLevel.GameMaster)]
        public int ToTTotalMonsterFame { get { return m_ToTTotalMonsterFame; } set { m_ToTTotalMonsterFame = value; } }

        public int ExecutesLightningStrike { get { return m_ExecutesLightningStrike; } set { m_ExecutesLightningStrike = value; } }

        private int m_VASTotalMonsterFame;

        [CommandProperty(AccessLevel.GameMaster)]
        public int VASTotalMonsterFame { get { return m_VASTotalMonsterFame; } set { m_VASTotalMonsterFame = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ToothAche { get { return BaseSweet.GetToothAche(this); } set { BaseSweet.SetToothAche(this, value, true); } }
        #endregion

        #region PlayerFlags
        public PlayerFlag Flags { get { return m_Flags; } set { m_Flags = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool MechanicalLife { get { return GetFlag(PlayerFlag.MechanicalLife); } set { SetFlag(PlayerFlag.MechanicalLife, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Arcanist
        {
            get { return GetFlag(PlayerFlag.Arcanist); }
            set { SetFlag(PlayerFlag.Arcanist, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanSummonFey
        {
            get { return GetFlag(PlayerFlag.CanSummonFey); }
            set { SetFlag(PlayerFlag.CanSummonFey, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanSummonFiend
        {
            get { return GetFlag(PlayerFlag.CanSummonFiend); }
            set { SetFlag(PlayerFlag.CanSummonFiend, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool FriendOfTheLibrary
        {
            get { return GetFlag(PlayerFlag.FriendOfTheLibrary); }
            set { SetFlag(PlayerFlag.FriendOfTheLibrary, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool PagingSquelched { get { return GetFlag(PlayerFlag.PagingSquelched); } set { SetFlag(PlayerFlag.PagingSquelched, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Glassblowing { get { return GetFlag(PlayerFlag.Glassblowing); } set { SetFlag(PlayerFlag.Glassblowing, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Masonry { get { return GetFlag(PlayerFlag.Masonry); } set { SetFlag(PlayerFlag.Masonry, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool SandMining { get { return GetFlag(PlayerFlag.SandMining); } set { SetFlag(PlayerFlag.SandMining, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool StoneMining { get { return GetFlag(PlayerFlag.StoneMining); } set { SetFlag(PlayerFlag.StoneMining, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool GemMining { get { return GetFlag(PlayerFlag.GemMining); } set { SetFlag(PlayerFlag.GemMining, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool BasketWeaving { get { return GetFlag(PlayerFlag.BasketWeaving); } set { SetFlag(PlayerFlag.BasketWeaving, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ToggleMiningStone { get { return GetFlag(PlayerFlag.ToggleMiningStone); } set { SetFlag(PlayerFlag.ToggleMiningStone, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AbyssEntry { get { return GetFlag(PlayerFlag.AbyssEntry); } set { SetFlag(PlayerFlag.AbyssEntry, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Parox { get { return GetFlag(PlayerFlag.Parox); } set { SetFlag(PlayerFlag.Parox, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool SacredQuest { get { return GetFlag(PlayerFlag.SacredQuest); } set { SetFlag(PlayerFlag.SacredQuest, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ToggleMiningGem { get { return GetFlag(PlayerFlag.ToggleMiningGem); } set { SetFlag(PlayerFlag.ToggleMiningGem, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool KarmaLocked { get { return GetFlag(PlayerFlag.KarmaLocked); } set { SetFlag(PlayerFlag.KarmaLocked, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AutoRenewInsurance { get { return GetFlag(PlayerFlag.AutoRenewInsurance); } set { SetFlag(PlayerFlag.AutoRenewInsurance, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool UseOwnFilter { get { return GetFlag(PlayerFlag.UseOwnFilter); } set { SetFlag(PlayerFlag.UseOwnFilter, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool PublicMyShard
        {
            get { return GetFlag(PlayerFlag.PublicMyShard); }
            set
            {
                SetFlag(PlayerFlag.PublicMyShard, value);
                InvalidateMyShard();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool AcceptGuildInvites { get { return GetFlag(PlayerFlag.AcceptGuildInvites); } set { SetFlag(PlayerFlag.AcceptGuildInvites, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool HasStatReward { get { return GetFlag(PlayerFlag.HasStatReward); } set { SetFlag(PlayerFlag.HasStatReward, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool RefuseTrades { get { return GetFlag(PlayerFlag.RefuseTrades); } set { SetFlag(PlayerFlag.RefuseTrades, value); } }

        private long m_ShamePoints;

        [CommandProperty(AccessLevel.GameMaster)]
        public long ShamePoints
        {
            get { return m_ShamePoints; }
            set
            {
                m_ShamePoints = value;
                InvalidateProperties();
            }
        }

        private DateTime m_SSNextSeed;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime SSNextSeed { get { return m_SSNextSeed; } set { m_SSNextSeed = value; } }

        private DateTime m_SSSeedExpire;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime SSSeedExpire { get { return m_SSSeedExpire; } set { m_SSSeedExpire = value; } }

        private Point3D m_SSSeedLocation;

        public Point3D SSSeedLocation { get { return m_SSSeedLocation; } set { m_SSSeedLocation = value; } }

        private Map m_SSSeedMap;

        public Map SSSeedMap { get { return m_SSSeedMap; } set { m_SSSeedMap = value; } }

        #endregion

        #region Auto Arrow Recovery
        private readonly Dictionary<Type, int> m_RecoverableAmmo = new Dictionary<Type, int>();

        public Dictionary<Type, int> RecoverableAmmo { get { return m_RecoverableAmmo; } }

        public void RecoverAmmo()
        {
            if (Core.SE && Alive)
            {
                foreach (var kvp in m_RecoverableAmmo)
                {
                    if (kvp.Value > 0)
                    {
                        Item ammo = null;

                        try
                        {
                            ammo = Activator.CreateInstance(kvp.Key) as Item;
                        }
                        catch
                        { }

                        if (ammo != null)
                        {
                            string name = ammo.Name;
                            ammo.Amount = kvp.Value;

                            if (name == null)
                            {
                                if (ammo is Arrow)
                                {
                                    name = "arrow";
                                }
                                else if (ammo is Bolt)
                                {
                                    name = "bolt";
                                }
                            }

                            if (name != null && ammo.Amount > 1)
                            {
                                name = String.Format("{0}s", name);
                            }

                            if (name == null)
                            {
                                name = String.Format("#{0}", ammo.LabelNumber);
                            }

                            PlaceInBackpack(ammo);
                            SendLocalizedMessage(1073504, String.Format("{0}\t{1}", ammo.Amount, name)); // You recover ~1_NUM~ ~2_AMMO~.
                        }
                    }
                }

                m_RecoverableAmmo.Clear();
            }
        }
        #endregion

        private DateTime m_AnkhNextUse;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime AnkhNextUse { get { return m_AnkhNextUse; } set { m_AnkhNextUse = value; } }

        private DateTime m_NextGemOfSalvationUse;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextGemOfSalvationUse
        {
            get { return m_NextGemOfSalvationUse; }
            set { m_NextGemOfSalvationUse = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Bedlam { get { return GetFlag(PlayerFlag.Bedlam); } set { SetFlag(PlayerFlag.Bedlam, value); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan DisguiseTimeLeft { get { return DisguiseTimers.TimeRemaining(this); } }

        private DateTime m_PeacedUntil;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime PeacedUntil { get { return m_PeacedUntil; } set { m_PeacedUntil = value; } }

        [CommandProperty(AccessLevel.Decorator)]
        public override string TitleName
        {
            get
            {
                string title = Titles.ComputeFameTitle(this);
                return title.Length > 0 ? title : RawName;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime AcceleratedStart { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public SkillName AcceleratedSkill { get; set; }

        private BardMastery m_BardMastery;
        private ResistanceType m_BardElementDamage;
        private DateTime m_NextBardMasterySwitch;
        private int m_BardMasteryLearnedMask;

        [CommandProperty(AccessLevel.GameMaster)]
        public ResistanceType BardElementDamage
        {
            get { return m_BardElementDamage; }
            set { m_BardElementDamage = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BardMasteryLearnedMask
        {
            get { return m_BardMasteryLearnedMask; }
            set { m_BardMasteryLearnedMask = value; }
        }

        public BardMastery BardMastery
        {
            get { return m_BardMastery; }
            set
            {
                m_BardMastery = value;
                m_BardMasteryLearnedMask |= m_BardMastery.Mask;
            }
        }

        public DateTime NextBardMasterySwitch
        {
            get { return m_NextBardMasterySwitch; }
            set { m_NextBardMasterySwitch = value; }
        }

        public bool HasLearnedBardMastery(BardMastery mastery)
        {
            return (m_BardMasteryLearnedMask & mastery.Mask) != 0;
        }

        public double[] ChampionTiers = new double[11];
        public int[] SuperChampionTiers = new int[12];
        public DateTime LastTierLoss;
        public DateTime LastChampionTierLoss;
        public DateTime LastSuperChampionTierLoss;

        public static void CheckTitlesAtrophy(Mobile from)
        {
            PlayerMobile pm = from as PlayerMobile;

            if (pm == null)
                return;

            try
            {
                if ((pm.LastTierLoss + TimeSpan.FromDays(1.0)) < DateTime.UtcNow) // TODO: verify
                {
                    for (int i = 0; i < pm.ChampionTiers.Length; i++)
                    {
                        pm.ChampionTiers[i]--; // TODO: verify

                        if (pm.ChampionTiers[i] < 0)
                            pm.ChampionTiers[i] = 0;
                    }

                    pm.LastTierLoss = DateTime.UtcNow;
                }

                if ((pm.LastChampionTierLoss + TimeSpan.FromDays(7.0)) < DateTime.UtcNow) // TODO: verify
                {
                    for (int i = 1; i < pm.SuperChampionTiers.Length; i++)
                    {
                        pm.SuperChampionTiers[i]--; // TODO: verify

                        if (pm.SuperChampionTiers[i] < 0)
                            pm.SuperChampionTiers[i] = 0;
                    }

                    pm.LastChampionTierLoss = DateTime.UtcNow;
                }

                if ((pm.LastSuperChampionTierLoss + TimeSpan.FromDays(30.0)) < DateTime.UtcNow) // // TODO: verify
                {
                    pm.SuperChampionTiers[0]--; // TODO: verify

                    if (pm.SuperChampionTiers[0] < 0)
                        pm.SuperChampionTiers[0] = 0;

                    pm.LastSuperChampionTierLoss = DateTime.UtcNow;
                }
            }
            catch
            {
            }
        }

        public static Direction GetDirection4(Point3D from, Point3D to)
        {
            int dx = from.X - to.X;
            int dy = from.Y - to.Y;

            int rx = dx - dy;
            int ry = dx + dy;

            Direction ret;

            if (rx >= 0 && ry >= 0)
            {
                ret = Direction.West;
            }
            else if (rx >= 0 && ry < 0)
            {
                ret = Direction.South;
            }
            else if (rx < 0 && ry < 0)
            {
                ret = Direction.East;
            }
            else
            {
                ret = Direction.North;
            }

            return ret;
        }

        public override bool OnDroppedItemToWorld(Item item, Point3D location)
        {
            if (!base.OnDroppedItemToWorld(item, location))
            {
                return false;
            }

            if (Core.AOS)
            {
                IPooledEnumerable mobiles = Map.GetMobilesInRange(location, 0);

                foreach (Mobile m in mobiles)
                {
                    if (m.Z >= location.Z && m.Z < location.Z + 16)
                    {
                        mobiles.Free();
                        return false;
                    }
                }

                mobiles.Free();
            }

            BounceInfo bi = item.GetBounce();

            if (bi != null)
            {
                Type type = item.GetType();

                if (type.IsDefined(typeof(FurnitureAttribute), true) || type.IsDefined(typeof(DynamicFlipingAttribute), true))
                {
                    var objs = type.GetCustomAttributes(typeof(FlipableAttribute), true);

                    if (objs != null && objs.Length > 0)
                    {
                        FlipableAttribute fp = objs[0] as FlipableAttribute;

                        if (fp != null)
                        {
                            var itemIDs = fp.ItemIDs;

                            Point3D oldWorldLoc = bi.m_WorldLoc;
                            Point3D newWorldLoc = location;

                            if (oldWorldLoc.X != newWorldLoc.X || oldWorldLoc.Y != newWorldLoc.Y)
                            {
                                Direction dir = GetDirection4(oldWorldLoc, newWorldLoc);

                                if (itemIDs.Length == 2)
                                {
                                    switch (dir)
                                    {
                                        case Direction.North:
                                        case Direction.South: item.ItemID = itemIDs[0]; break;
                                        case Direction.East:
                                        case Direction.West: item.ItemID = itemIDs[1]; break;
                                    }
                                }
                                else if (itemIDs.Length == 4)
                                {
                                    switch (dir)
                                    {
                                        case Direction.South: item.ItemID = itemIDs[0]; break;
                                        case Direction.East: item.ItemID = itemIDs[1]; break;
                                        case Direction.North: item.ItemID = itemIDs[2]; break;
                                        case Direction.West: item.ItemID = itemIDs[3]; break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        private bool m_Flying;

        [CommandProperty(AccessLevel.GameMaster)]
        public override bool Flying
        {
            get { return m_Flying; }
            set
            {
                if (m_Flying != value)
                {
                    m_Flying = value;
                    Delta(MobileDelta.Flags);

                    if (!m_Flying)
                    {
                        this.RemoveBuff(BuffIcon.Flying);
                    }
                    else
                    {
                        BuffInfo.AddBuff(this, new BuffInfo(BuffIcon.Flying, 1112193, 1112567)); // Flying & You are flying.
                    }
                }
            }
        }

        public override int GetPacketFlags()
        {
            int flags = base.GetPacketFlags();

            if (m_Flying)
            {
                flags |= 0x04;
            }

            return flags;
        }

        public override int GetOldPacketFlags()
        {
            int flags = base.GetOldPacketFlags();

            return flags;
        }

        public bool GetFlag(PlayerFlag flag)
        {
            return ((m_Flags & flag) != 0);
        }

        public void SetFlag(PlayerFlag flag, bool value)
        {
            if (value)
            {
                m_Flags |= flag;
            }
            else
            {
                m_Flags &= ~flag;
            }
        }

        public DesignContext DesignContext { get { return m_DesignContext; } set { m_DesignContext = value; } }

        public static void Initialize()
        {
            if (FastwalkPrevention)
            {
                PacketHandlers.RegisterThrottler(0x02, MovementThrottle_Callback);
            }

            EventSink.Login += OnLogin;
            EventSink.Logout += OnLogout;
            EventSink.Connected += EventSink_Connected;
            EventSink.Disconnected += EventSink_Disconnected;
            EventSink.TargetedSkillUse += new TargetedSkillUseEventHandler(Targeted_Skill);
            EventSink.TargetedItemUse += new TargetedItemUseEventHandler(Targeted_Item);
            EventSink.EquipMacro += new EquipMacroEventHandler(Equip_Macro);
            EventSink.UnequipMacro += new UnequipMacroEventHandler(Unequip_Macro);

            if (Core.SE)
            {
                Timer.DelayCall(TimeSpan.Zero, CheckPets);
            }
        }

        private static void Unequip_Macro(UnequipMacroEventArgs e)
        {
            try
            {
                if (e.NetState != null && e.NetState.Mobile != null && e.List != null)
                {
                    for (int i = 0; i < e.List.Count; ++i)
                    {
                        //Layer layer = Layer.GetAt( e.List[i] ); 
                        Item item = e.NetState.Mobile.FindItemOnLayer((Layer)(e.List[i]));

                        if (item != null && item.Layer != Layer.Hair && item.Layer != Layer.FacialHair)
                        {
                            e.NetState.Mobile.Backpack.DropItem(item);
                        }
                    }
                }
            }
            catch { }
        }

        private static void Equip_Macro(EquipMacroEventArgs e)
        {
            try
            {
                if (e.NetState != null && e.NetState.Mobile != null && e.List != null)
                {
                    for (int i = 0; i < e.List.Count; ++i)
                    {
                        //Item item = World.FindItem( e.List[i] ); 
                        Item item = World.FindItem(e.List[i]);
                        if (item != null && item.IsChildOf(e.NetState.Mobile))
                        {
                            e.NetState.Mobile.EquipItem(item);
                        }
                    }
                }
            }
            catch { }
        }

        private static void Targeted_Item(TargetedItemUseEventArgs e)
        {
            try
            {
                Item from = World.FindItem(e.Source.Serial);
                Mobile to = World.FindMobile(e.Target.Serial);
                Item toI = World.FindItem(e.Target.Serial);

                if (from != null)
                {
                    if (to != null)
                    {
                        e.NetState.Mobile.TargetLocked = true;
                        e.NetState.Mobile.Use(from);
                        e.NetState.Mobile.Target.Invoke(e.NetState.Mobile, to);
                    }
                    else if (toI != null)
                    {
                        e.NetState.Mobile.TargetLocked = true;
                        e.NetState.Mobile.Use(from);
                        e.NetState.Mobile.Target.Invoke(e.NetState.Mobile, toI);
                    }
                }
            }
            catch { }
            finally { e.NetState.Mobile.TargetLocked = false; }
        }

        private static void Targeted_Skill(TargetedSkillUseEventArgs e)
        {
            Mobile from = e.NetState.Mobile;

            try
            {
                int SkillId = e.SkillID;
                Mobile to = World.FindMobile(e.Target.Serial);
                Item toI = World.FindItem(e.Target.Serial);

                if (to != null)
                {
                    from.TargetLocked = true;
                    if (from.UseSkill(e.SkillID))
                    {
                        from.Target.Invoke(from, to);
                    }
                }
                else if (toI != null)
                {
                    from.TargetLocked = true;
                    if (from.UseSkill(e.SkillID))
                    {
                        from.Target.Invoke(from, toI);
                    }
                }
            }
            catch { }
            finally { from.TargetLocked = false; }
        }

        private static void CheckPets()
        {
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m is PlayerMobile)
                {
                    PlayerMobile pm = (PlayerMobile)m;

                    if (((!pm.Mounted || (pm.Mount != null && pm.Mount is EtherealMount)) && (pm.AllFollowers.Count > pm.AutoStabled.Count)) || (pm.Mounted && (pm.AllFollowers.Count > (pm.AutoStabled.Count + 1))))
                    {
                        pm.AutoStablePets(); /* autostable checks summons, et al: no need here */
                    }
                }
            }
        }

        public override void OnSkillInvalidated(Skill skill)
        {
            if (Core.AOS && skill.SkillName == SkillName.MagicResist)
            {
                UpdateResistances();
            }
        }

        public override int GetMaxResistance(ResistanceType type)
        {
            if (IsStaff())
            {
                return int.MaxValue;
            }

            int max = base.GetMaxResistance(type);

            if (type != ResistanceType.Physical && 60 < max && CurseSpell.UnderEffect(this))
            {
                max = 60;
            }

            if (Core.ML && Race == Race.Elf && type == ResistanceType.Energy)
            {
                max += 5; //Intended to go after the 60 max from curse
            }

            return max;
        }

        protected override void OnRaceChange(Race oldRace)
        {
            if (oldRace == Race.Gargoyle && this.Flying)
            {
                Flying = false;
                Send(SpeedControl.Disable);
                BuffInfo.RemoveBuff(this, BuffIcon.Flying);
            }
            ValidateEquipment();
            UpdateResistances();
        }

        public override int MaxWeight { get { return (((Core.ML && Race == Race.Human) ? 100 : 40) + (int)(3.5 * Str)); } }

        private int m_LastGlobalLight = -1, m_LastPersonalLight = -1;

        public override void OnNetStateChanged()
        {
            m_LastGlobalLight = -1;
            m_LastPersonalLight = -1;
        }

        public override void ComputeBaseLightLevels(out int global, out int personal)
        {
            global = LightCycle.ComputeLevelFor(this);

            bool racialNightSight = (Core.ML && Race == Race.Elf);

            if (LightLevel < 21 && AccessLevel >= AccessLevel.Counselor)
            {
                personal = 21;
            }
            else if (LightLevel < 21 && (AosAttributes.GetValue(this, AosAttribute.NightSight) > 0 || racialNightSight))
            {
                personal = 21;
            }
            else
            {
                personal = LightLevel;
            }
        }

        public override void CheckLightLevels(bool forceResend)
        {
            NetState ns = NetState;

            if (ns == null)
            {
                return;
            }

            int global, personal;

            ComputeLightLevels(out global, out personal);

            if (!forceResend)
            {
                forceResend = (global != m_LastGlobalLight || personal != m_LastPersonalLight);
            }

            if (!forceResend)
            {
                return;
            }

            m_LastGlobalLight = global;
            m_LastPersonalLight = personal;

            ns.Send(GlobalLightLevel.Instantiate(global));
            ns.Send(new PersonalLightLevel(this, personal));
        }

        public override int GetMinResistance(ResistanceType type)
        {
            int magicResist = (int)(Skills[SkillName.MagicResist].Value * 10);
            int min = int.MinValue;

            if (magicResist >= 1000)
            {
                min = 40 + ((magicResist - 1000) / 50);
            }
            else if (magicResist >= 400)
            {
                min = (magicResist - 400) / 15;
            }

            if (min > MaxPlayerResistance)
            {
                min = MaxPlayerResistance;
            }

            int baseMin = base.GetMinResistance(type);

            if (min < baseMin)
            {
                min = baseMin;
            }

            return min;
        }

        public override void OnManaChange(int oldValue)
        {
            base.OnManaChange(oldValue);
            if (m_ExecutesLightningStrike > 0)
            {
                if (Mana < m_ExecutesLightningStrike)
                {
                    SpecialMove.ClearCurrentMove(this);
                }
            }
        }

        private static void OnLogin(LoginEventArgs e)
        {
            Mobile from = e.Mobile;
            PlayerMobile pm = (PlayerMobile)from;
            NetState ns = pm.NetState;
            Account acct = from.Account as Account;

            SacrificeVirtue.CheckAtrophy(from);
            JusticeVirtue.CheckAtrophy(from);
            CompassionVirtue.CheckAtrophy(from);
            HonorVirtue.CheckAtrophy(from);
            ValorVirtue.CheckAtrophy(from);

            CheckTitlesAtrophy(from);

            if (AccountHandler.LockdownLevel > AccessLevel.VIP)
            {
                string notice;

                if (acct == null || !acct.HasAccess(from.NetState))
                {
                    if (from.IsPlayer())
                    {
                        notice = "The server is currently under lockdown. No players are allowed to log in at this time.";
                    }
                    else
                    {
                        notice = "The server is currently under lockdown. You do not have sufficient access level to connect.";
                    }

                    Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(Disconnect), from);
                }
                else if (from.AccessLevel >= AccessLevel.Administrator)
                {
                    notice = "The server is currently under lockdown. As you are an administrator, you may change this from the [Admin gump.";
                }
                else
                {
                    notice = "The server is currently under lockdown. You have sufficient access level to connect.";
                }

                from.SendGump(new NoticeGump(1060637, 30720, notice, 0xFFC000, 300, 140, null, null));
                return;
            }

            if (from.Race == Race.Human || from.Race == Race.Elf)
            {
                if (ns != null && ns.IsKRClient)
                {
                    if (pm.KRStartingQuestStep == 0)
                    {
                        pm.KRStartingQuestStep++;
                    }
                    else if (pm.KRStartingQuestStep > 0 || pm.KRStartingQuestStep < 32)
                    {
                        KRStartingQuest.DoStep(pm);
                    }
                    //if (pm.LastOnline >= DateTime.MinValue)
                    //pm.KRStartingQuestStep++;
                    /// else if (pm.KRStartingQuestStep > 0)
                    //KRStartingQuest.DoStep(pm);
                    ///pm.SendAsciiMessage("testing Login");
                }
                //else if (pm.KRStartingQuestStep > 0)
                //pm.KRStartingQuestStep = 0;
            }

            if (pm != null)
            {
                pm.ClaimAutoStabledPets();
            }

            if (pm != null && pm.Young && acct != null && acct.Young)
            {
                TimeSpan ts = Accounting.Account.YoungDuration - acct.TotalGameTime;
                int hours = Math.Max((int)ts.TotalHours, 0);

                pm.SendAsciiMessage("You will enjoy the benefits and relatively safe status of a young player for {0} more hour{1}.", hours, hours != 1 ? "s" : "");
            }
        }

        private bool m_NoDeltaRecursion;

        public void ValidateEquipment()
        {
            if (m_NoDeltaRecursion || Map == null || Map == Map.Internal)
            {
                return;
            }

            if (Items == null)
            {
                return;
            }

            m_NoDeltaRecursion = true;
            Timer.DelayCall(TimeSpan.Zero, ValidateEquipment_Sandbox);
        }

        private void ValidateEquipment_Sandbox()
        {
            try
            {
                if (Map == null || Map == Map.Internal)
                {
                    return;
                }

                var items = Items;

                if (items == null)
                {
                    return;
                }

                bool moved = false;

                int str = Str;
                int dex = Dex;
                int intel = Int;
                int factionItemCount = 0;

                Mobile from = this;
                Ethic ethic = Ethic.Find(from);

                for (int i = items.Count - 1; i >= 0; --i)
                {
                    if (i >= items.Count)
                    {
                        continue;
                    }

                    Item item = items[i];

                    if ((item.SavedFlags & 0x100) != 0)
                    {
                        if (item.Hue != Ethic.Hero.Definition.PrimaryHue)
                        {
                            item.SavedFlags &= ~0x100;
                        }
                        else if (ethic != Ethic.Hero)
                        {
                            from.AddToBackpack(item);
                            moved = true;
                            continue;
                        }
                    }
                    else if ((item.SavedFlags & 0x200) != 0)
                    {
                        if (item.Hue != Ethic.Evil.Definition.PrimaryHue)
                        {
                            item.SavedFlags &= ~0x200;
                        }
                        else if (ethic != Ethic.Evil)
                        {
                            from.AddToBackpack(item);
                            moved = true;
                            continue;
                        }
                    }

                    if (item is BaseWeapon)
                    {
                        BaseWeapon weapon = (BaseWeapon)item;

                        bool drop = false;

                        if (dex < weapon.DexRequirement)
                        {
                            drop = true;
                        }
                        else if (str < ItemAttributes.Scale(weapon.StrRequirement, 100 - weapon.GetLowerStatReq()))
                        {
                            drop = true;
                        }
                        else if (intel < weapon.IntRequirement)
                        {
                            drop = true;
                        }
                        else if (weapon.RequiredRace != null && weapon.RequiredRace != Race)
                        {
                            drop = true;
                        }

                        if (drop)
                        {
                            string name = weapon.Name;

                            if (name == null)
                            {
                                name = String.Format("#{0}", weapon.LabelNumber);
                            }

                            from.SendLocalizedMessage(1062001, name); // You can no longer wield your ~1_WEAPON~
                            from.AddToBackpack(weapon);
                            moved = true;
                        }
                    }
                    else if (item is BaseArmor)
                    {
                        BaseArmor armor = (BaseArmor)item;

                        bool drop = false;

                        if (!armor.AllowMaleWearer && !from.Female && from.AccessLevel < AccessLevel.GameMaster)
                        {
                            drop = true;
                        }
                        else if (!armor.AllowFemaleWearer && from.Female && from.AccessLevel < AccessLevel.GameMaster)
                        {
                            drop = true;
                        }
                        else if (armor.RequiredRace != null && armor.RequiredRace != Race)
                        {
                            drop = true;
                        }
                        else
                        {
                            int strBonus = armor.ComputeStatBonus(StatType.Str), strReq = armor.ComputeStatReq(StatType.Str);
                            int dexBonus = armor.ComputeStatBonus(StatType.Dex), dexReq = armor.ComputeStatReq(StatType.Dex);
                            int intBonus = armor.ComputeStatBonus(StatType.Int), intReq = armor.ComputeStatReq(StatType.Int);

                            if (dex < dexReq || (dex + dexBonus) < 1)
                            {
                                drop = true;
                            }
                            else if (str < strReq || (str + strBonus) < 1)
                            {
                                drop = true;
                            }
                            else if (intel < intReq || (intel + intBonus) < 1)
                            {
                                drop = true;
                            }
                        }

                        if (drop)
                        {
                            string name = armor.Name;

                            if (name == null)
                            {
                                name = String.Format("#{0}", armor.LabelNumber);
                            }

                            if (armor is BaseShield)
                            {
                                from.SendLocalizedMessage(1062003, name); // You can no longer equip your ~1_SHIELD~
                            }
                            else
                            {
                                from.SendLocalizedMessage(1062002, name); // You can no longer wear your ~1_ARMOR~
                            }

                            from.AddToBackpack(armor);
                            moved = true;
                        }
                    }
                    else if (item is BaseClothing)
                    {
                        BaseClothing clothing = (BaseClothing)item;

                        bool drop = false;

                        if (!clothing.AllowMaleWearer && !from.Female && from.AccessLevel < AccessLevel.GameMaster)
                        {
                            drop = true;
                        }
                        else if (!clothing.AllowFemaleWearer && from.Female && from.AccessLevel < AccessLevel.GameMaster)
                        {
                            drop = true;
                        }
                        else if (clothing.RequiredRace != null && clothing.RequiredRace != Race)
                        {
                            drop = true;
                        }
                        else
                        {
                            int strBonus = clothing.ComputeStatBonus(StatType.Str);
                            int strReq = clothing.ComputeStatReq(StatType.Str);

                            if (str < strReq || (str + strBonus) < 1)
                            {
                                drop = true;
                            }
                        }

                        if (drop)
                        {
                            string name = clothing.Name;

                            if (name == null)
                            {
                                name = String.Format("#{0}", clothing.LabelNumber);
                            }

                            from.SendLocalizedMessage(1062002, name); // You can no longer wear your ~1_ARMOR~

                            from.AddToBackpack(clothing);
                            moved = true;
                        }
                    }

                    FactionItem factionItem = FactionItem.Find(item);

                    if (factionItem != null)
                    {
                        bool drop = false;

                        Faction ourFaction = Faction.Find(this);

                        if (ourFaction == null || ourFaction != factionItem.Faction)
                        {
                            drop = true;
                        }
                        else if (++factionItemCount > FactionItem.GetMaxWearables(this))
                        {
                            drop = true;
                        }

                        if (drop)
                        {
                            from.AddToBackpack(item);
                            moved = true;
                        }
                    }
                }

                if (moved)
                {
                    from.SendLocalizedMessage(500647); // Some equipment has been moved to your backpack.
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                m_NoDeltaRecursion = false;
            }
        }

        public override void Delta(MobileDelta flag)
        {
            base.Delta(flag);

            if ((flag & MobileDelta.Stat) != 0)
            {
                ValidateEquipment();
            }

            if ((flag & (MobileDelta.Name | MobileDelta.Hue)) != 0)
            {
                InvalidateMyShard();
            }
        }

        private static void Disconnect(object state)
        {
            NetState ns = ((Mobile)state).NetState;

            if (ns != null)
            {
                ns.Dispose();
            }
        }

        private static void OnLogout(LogoutEventArgs e)
        {
            Mobile from = e.Mobile;

            if (e.Mobile is PlayerMobile)
            {
                if (from.Alive)
                {
                    ((PlayerMobile)e.Mobile).AutoStablePets();
                }

                if (((PlayerMobile)e.Mobile).Transport != null)
                {
                    ((PlayerMobile)e.Mobile).Transport.LeaveCommand((PlayerMobile)e.Mobile);
                }
            }
        }

        private static void EventSink_Connected(ConnectedEventArgs e)
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;

            if (pm != null)
            {
                pm.m_SessionStart = DateTime.UtcNow;

                if (pm.m_Quest != null)
                {
                    pm.m_Quest.StartTimer();
                }

                QuestHelper.StartTimer(pm);

                pm.BedrollLogout = false;
                pm.LastOnline = DateTime.UtcNow;
            }

            DisguiseTimers.StartTimer(e.Mobile);

            Timer.DelayCall(TimeSpan.Zero, new TimerStateCallback(ClearSpecialMovesCallback), e.Mobile);

            Account acc = e.Mobile.Account as Account;

            if (acc != null && acc.Young && acc.YoungTimer == null)
            {
                acc.YoungTimer = new YoungTimer(acc);
                acc.YoungTimer.Start();
            }
        }

        private static void ClearSpecialMovesCallback(object state)
        {
            Mobile from = (Mobile)state;

            SpecialMove.ClearAllMoves(from);
        }

        private static void EventSink_Disconnected(DisconnectedEventArgs e)
        {
            Mobile from = e.Mobile;
            DesignContext context = DesignContext.Find(from);

            if (context != null)
            {
                /* Client disconnected
                *  - Remove design context
                *  - Eject all from house
                *  - Restore relocated entities
                */
                // Remove design context
                DesignContext.Remove(from);

                // Eject all from house
                from.RevealingAction();

                foreach (Item item in context.Foundation.GetItems())
                {
                    item.Location = context.Foundation.BanLocation;
                }

                foreach (Mobile mobile in context.Foundation.GetMobiles())
                {
                    mobile.Location = context.Foundation.BanLocation;
                }

                // Restore relocated entities
                context.Foundation.RestoreRelocatedEntities();
            }

            PlayerMobile pm = e.Mobile as PlayerMobile;

            if (pm != null)
            {
                pm.m_GameTime += (DateTime.UtcNow - pm.m_SessionStart);

                if (pm.m_Quest != null)
                {
                    pm.m_Quest.StopTimer();
                }

                QuestHelper.StopTimer(pm);

                pm.m_SpeechLog = null;
                pm.LastOnline = DateTime.UtcNow;
            }

            DisguiseTimers.StopTimer(from);

            Account acc = e.Mobile.Account as Account;

            if (acc != null && acc.YoungTimer != null)
            {
                acc.YoungTimer.Stop();
                acc.YoungTimer = null;
            }

            if (acc != null && pm != null)
            {
                acc.TotalGameTime += DateTime.UtcNow - pm.SessionStart;
            }
        }

        public override void RevealingAction()
        {
            if (m_DesignContext != null)
            {
                return;
            }

            InvisibilitySpell.RemoveTimer(this);

            base.RevealingAction();
        }

        public override void OnHiddenChanged()
        {
            base.OnHiddenChanged();

            this.RemoveBuff(BuffIcon.Invisibility);
            //Always remove, default to the hiding icon EXCEPT in the invis spell where it's explicitly set

            if (!Hidden)
            {
                this.RemoveBuff(BuffIcon.HidingAndOrStealth);
            }
            else // if( !InvisibilitySpell.HasTimer( this ) )
            {
                BuffInfo.AddBuff(this, new BuffInfo(BuffIcon.HidingAndOrStealth, 1075655)); //Hidden/Stealthing & You Are Hidden
            }
        }

        public override void OnSubItemAdded(Item item)
        {
            if (AccessLevel < AccessLevel.GameMaster && item.IsChildOf(Backpack))
            {
                int maxWeight = WeightOverloading.GetMaxWeight(this);
                int curWeight = BodyWeight + TotalWeight;

                if (curWeight > maxWeight)
                {
                    SendLocalizedMessage(1019035, true, String.Format(" : {0} / {1}", curWeight, maxWeight));
                }
            }

            base.OnSubItemAdded(item);
        }

        public override bool CanBeHarmful(Mobile target, bool message, bool ignoreOurBlessedness)
        {
            if (m_DesignContext != null || (target is PlayerMobile && ((PlayerMobile)target).m_DesignContext != null))
            {
                return false;
            }

            if (Peaced)
            {
                //!+ TODO: message
                return false;
            }

            if ((target is BaseVendor && ((BaseVendor)target).IsInvulnerable) || target is PlayerVendor || target is TownCrier)
            {
                if (message)
                {
                    if (target.Title == null)
                    {
                        SendMessage("{0} the vendor cannot be harmed.", target.Name);
                    }
                    else
                    {
                        SendMessage("{0} {1} cannot be harmed.", target.Name, target.Title);
                    }
                }

                return false;
            }

            return base.CanBeHarmful(target, message, ignoreOurBlessedness);
        }

        public override bool CanBeBeneficial(Mobile target, bool message, bool allowDead)
        {
            if (m_DesignContext != null || (target is PlayerMobile && ((PlayerMobile)target).m_DesignContext != null))
            {
                return false;
            }

            return base.CanBeBeneficial(target, message, allowDead);
        }

        public override bool CheckContextMenuDisplay(IEntity target)
        {
            return (m_DesignContext == null);
        }

        public override void OnItemAdded(Item item)
        {
            base.OnItemAdded(item);

            if (item is BaseArmor || item is BaseWeapon)
            {
                Hits = Hits;
                Stam = Stam;
                Mana = Mana;
            }

            if (NetState != null)
            {
                CheckLightLevels(false);
            }

            InvalidateMyShard();
        }

        public override void OnItemRemoved(Item item)
        {
            base.OnItemRemoved(item);

            if (item is BaseArmor || item is BaseWeapon)
            {
                Hits = Hits;
                Stam = Stam;
                Mana = Mana;
            }

            if (NetState != null)
            {
                CheckLightLevels(false);
            }

            InvalidateMyShard();
        }

        public override double ArmorRating
        {
            get
            {
                //BaseArmor ar;
                double rating = 0.0;

                AddArmorRating(ref rating, NeckArmor);
                AddArmorRating(ref rating, HandArmor);
                AddArmorRating(ref rating, HeadArmor);
                AddArmorRating(ref rating, ArmsArmor);
                AddArmorRating(ref rating, LegsArmor);
                AddArmorRating(ref rating, ChestArmor);
                AddArmorRating(ref rating, ShieldArmor);

                return VirtualArmor + VirtualArmorMod + rating;
            }
        }

        private void AddArmorRating(ref double rating, Item armor)
        {
            BaseArmor ar = armor as BaseArmor;

            if (ar != null && (!Core.AOS || ar.ArmorAttributes.MageArmor == 0))
            {
                rating += ar.ArmorRatingScaled;
            }
        }

        #region [Stats]Max
        [CommandProperty(AccessLevel.GameMaster)]
        public override int HitsMax
        {
            get
            {
                int strBase;
                int strOffs = GetStatOffset(StatType.Str);

                if (Core.AOS)
                {
                    strBase = Str; //this.Str already includes GetStatOffset/str
                    strOffs = AosAttributes.GetValue(this, AosAttribute.BonusHits);

                    if (Core.ML && strOffs > 25 && IsPlayer())
                    {
                        strOffs = 25;
                    }

                    if (AnimalForm.UnderTransformation(this, typeof(BakeKitsune)) ||
                        AnimalForm.UnderTransformation(this, typeof(GreyWolf)))
                    {
                        strOffs += 20;
                    }
                }
                else
                {
                    strBase = RawStr;
                }

                return (strBase / 2) + 50 + strOffs;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public override int StamMax { get { return base.StamMax + AosAttributes.GetValue(this, AosAttribute.BonusStam); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public override int ManaMax
        {
            get
            {
                return base.ManaMax + AosAttributes.GetValue(this, AosAttribute.BonusMana) +
                       ((Core.ML && Race == Race.Elf) ? 20 : 0);
            }
        }
        #endregion

        // Collection Reward Titles
        private ArrayList m_CollectionTitles;
        private int m_CurrentCollectionTitle;

        public ArrayList CollectionTitles
        {
            get { return m_CollectionTitles; }
            set { m_CollectionTitles = value; }
        }

        public int CurrentCollectionTitle
        {
            get { return m_CurrentCollectionTitle; }
            set { m_CurrentCollectionTitle = value; InvalidateProperties(); }
        }

        private DateTime m_NextPBlow;

        public DateTime NextPBlow
        {
            get { return m_NextPBlow; }
            set { m_NextPBlow = value; }
        }

        private DateTime m_NextDisarm;

        public DateTime NextDisarm
        {
            get { return m_NextDisarm; }
            set { m_NextDisarm = value; }
        }

        private DateTime m_LastExplo;
        private DateTime m_LastArrow;

        public DateTime LastExplo
        {
            get { return m_LastExplo; }
            set { m_LastExplo = value; }
        }

        public DateTime LastArrow
        {
            get { return m_LastArrow; }
            set { m_LastArrow = value; }
        }

        // Floor Traps
        private int m_FloorTrapsPlaced;

        public int FloorTrapsPlaced
        {
            get { return m_FloorTrapsPlaced; }
            set { m_FloorTrapsPlaced = value; }
        }

        private LoyaltyInfo m_LoyaltyInfo;

        [CommandProperty(AccessLevel.GameMaster)]
        public LoyaltyInfo LoyaltyInfo
        {
            get { return m_LoyaltyInfo; }
            set { }
        }

        private TieredQuestInfo m_TieredQuestInfo;

        [CommandProperty(AccessLevel.GameMaster)]
        public TieredQuestInfo TieredQuestInfo
        {
            get { return m_TieredQuestInfo; }
            set { }
        }

        private DateTime m_LastForgedPardonUse;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastForgedPardonUse
        {
            get { return m_LastForgedPardonUse; }
            set { m_LastForgedPardonUse = value; }
        }

        private DateTime m_NextTenthAnniversarySculptureUse;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextTenthAnniversarySculptureUse
        {
            get { return m_NextTenthAnniversarySculptureUse; }
            set { m_NextTenthAnniversarySculptureUse = value; }
        }

        #region Starting Quest
        private byte m_KRStartingQuestStep;

        [CommandProperty(AccessLevel.GameMaster)]
        public byte KRStartingQuestStep
        {
            get { return m_KRStartingQuestStep; }
            set
            {
                m_KRStartingQuestStep = value;

                KRStartingQuest.DoStep(this);
            }
        }

        public void CheckKRStartingQuestStep(int step)
        {
            if (m_KRStartingQuestStep < step && m_KRStartingQuestStep != 0)
            {
                CloseGump(typeof(KRStartingQuestGump));
                CloseGump(typeof(KRStartingQuestCancelGump));

                KRStartingQuestStep = (byte)step;
            }
        }

        public void FinishKRStartingQuest()
        {
            m_KRStartingQuestStep = 32;
        }

        #endregion

        #region Stat Getters/Setters
        [CommandProperty(AccessLevel.GameMaster)]
        public override int Str
        {
            get
            {
                if (Core.ML && IsPlayer())
                {
                    return Math.Min(base.Str, 150);
                }

                return base.Str;
            }
            set { base.Str = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public override int Int
        {
            get
            {
                if (Core.ML && IsPlayer())
                {
                    return Math.Min(base.Int, 150);
                }

                return base.Int;
            }
            set { base.Int = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public override int Dex
        {
            get
            {
                if (Core.ML && IsPlayer())
                {
                    return Math.Min(base.Dex, 150);
                }

                return base.Dex;
            }
            set { base.Dex = value; }
        }
        #endregion

        public override void OnFinishMeditation()
        {
            BuffInfo.RemoveBuff(this, BuffIcon.ActiveMeditation);
        }

        public override bool Move(Direction d)
        {
            NetState ns = NetState;

            if (ns != null)
            {
                if (HasGump(typeof(ResurrectGump)))
                {
                    if (Alive)
                    {
                        CloseGump(typeof(ResurrectGump));
                    }
                    else
                    {
                        SendLocalizedMessage(500111); // You are frozen and cannot move.
                        return false;
                    }
                }
            }

            int speed = ComputeMovementSpeed(d);

            bool res;

            if (!Alive)
            {
                MovementImpl.IgnoreMovableImpassables = true;
            }

            res = base.Move(d);

            MovementImpl.IgnoreMovableImpassables = false;

            if (!res)
            {
                return false;
            }

            m_NextMovementTime += speed;

            return true;
        }

        public override bool CheckMovement(Direction d, out int newZ)
        {
            DesignContext context = m_DesignContext;

            if (context == null)
            {
                return base.CheckMovement(d, out newZ);
            }

            HouseFoundation foundation = context.Foundation;

            newZ = foundation.Z + HouseFoundation.GetLevelZ(context.Level, context.Foundation);

            int newX = X, newY = Y;
            Movement.Movement.Offset(d, ref newX, ref newY);

            int startX = foundation.X + foundation.Components.Min.X + 1;
            int startY = foundation.Y + foundation.Components.Min.Y + 1;
            int endX = startX + foundation.Components.Width - 1;
            int endY = startY + foundation.Components.Height - 2;

            return (newX >= startX && newY >= startY && newX < endX && newY < endY && Map == foundation.Map);
        }

        public override bool AllowItemUse(Item item)
        {
            if (m_DuelContext != null && !m_DuelContext.AllowItemUse(this, item))
            {
                return false;
            }

            return DesignContext.Check(this);
        }

        public SkillName[] AnimalFormRestrictedSkills { get { return m_AnimalFormRestrictedSkills; } }

        private readonly SkillName[] m_AnimalFormRestrictedSkills = new[]
		{
			SkillName.ArmsLore, SkillName.Begging, SkillName.Discordance, SkillName.Forensics, SkillName.Inscribe,
			SkillName.ItemID, SkillName.Meditation, SkillName.Peacemaking, SkillName.Provocation, SkillName.RemoveTrap,
			SkillName.SpiritSpeak, SkillName.Stealing, SkillName.TasteID
		};

        public override bool AllowSkillUse(SkillName skill)
        {
            if (AnimalForm.UnderTransformation(this))
            {
                for (int i = 0; i < m_AnimalFormRestrictedSkills.Length; i++)
                {
                    if (m_AnimalFormRestrictedSkills[i] == skill)
                    {
                        AnimalFormContext context = AnimalForm.GetContext(this);

                        if (skill == SkillName.Stealing && context.StealingMod != null && context.StealingMod.Value > 0)
                        {
                            continue;
                        }

                        SendLocalizedMessage(1070771); // You cannot use that skill in this form.
                        return false;
                    }
                }
            }

            if (m_DuelContext != null && !m_DuelContext.AllowSkillUse(this, skill))
            {
                return false;
            }

            return DesignContext.Check(this);
        }

        private bool m_LastProtectedMessage;
        private int m_NextProtectionCheck = 10;

        public virtual void RecheckTownProtection()
        {
            m_NextProtectionCheck = 10;

            GuardedRegion reg = (GuardedRegion)Region.GetRegion(typeof(GuardedRegion));
            bool isProtected = (reg != null && !reg.IsDisabled());

            if (isProtected != m_LastProtectedMessage)
            {
                if (isProtected)
                {
                    SendLocalizedMessage(500112); // You are now under the protection of the town guards.
                }
                else
                {
                    SendLocalizedMessage(500113); // You have left the protection of the town guards.
                }

                m_LastProtectedMessage = isProtected;
            }
        }

        public override void MoveToWorld(Point3D loc, Map map)
        {
            base.MoveToWorld(loc, map);

            RecheckTownProtection();
        }

        public override void SetLocation(Point3D loc, bool isTeleport)
        {
            if (!isTeleport && IsPlayer())
            {
                // moving, not teleporting
                int zDrop = (Location.Z - loc.Z);

                if (zDrop > 20) // we fell more than one story
                {
                    Hits -= ((zDrop / 20) * 10) - 5; // deal some damage; does not kill, disrupt, etc
                }
            }

            base.SetLocation(loc, isTeleport);

            if (isTeleport || --m_NextProtectionCheck == 0)
            {
                RecheckTownProtection();
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from == this)
            {
                if (Alive && IsPlayer() && Core.SA && CharConfig.CharLoyaltyEnabled)
                {
                    list.Add(new CallbackEntry(1049594, new ContextCallback(LoyaltyRatingMenu)));
                }

                if (m_CollectionTitles.Count >= 2)
                {
                    list.Add(new CallbackEntry(6229, new ContextCallback(SelectRewardTitle)));
                }

                if (m_Quest != null)
                {
                    m_Quest.GetContextMenuEntries(list);
                }

                if (Alive && InsuranceEnabled)
                {
                    list.Add(new CallbackEntry(6201, new ContextCallback(ToggleItemInsurance)));
                    list.Add(new CallbackEntry(1114299, new ContextCallback(() => ItemInsuranceMenu.SendGump(this))));
                }

                if (DisabledPvpWarning)
                {
                    list.Add(new CallbackEntry(1113797, new ContextCallback(EnablePvpWarning)));
                }

                BaseHouse house = BaseHouse.FindHouseAt(this);

                if (house != null)
                {
                    if (Alive && house.InternalizedVendors.Count > 0 && house.IsOwner(this))
                    {
                        list.Add(new CallbackEntry(6204, GetVendor));
                    }

                    if (house.IsAosRules && !Region.IsPartOf(typeof(SafeZone))) // Dueling
                    {
                        list.Add(new CallbackEntry(6207, LeaveHouse));
                    }
                }

                if (m_JusticeProtectors.Count > 0)
                {
                    list.Add(new CallbackEntry(6157, CancelProtection));
                }

                if (Alive)
                {
                    list.Add(new CallbackEntry(6210, ToggleChampionTitleDisplay));

                    #region Insurance and Trades
                    if (Core.HS)
                        list.Add(new CallbackEntry(RefuseTrades ? 1154112 : 1154113, new ContextCallback(ToggleTrades))); // Allow Trades / Refuse Trades 
                    #endregion
                }

                if (Alive)
                {
                    QuestHelper.GetContextMenuEntries(list);
                }
            }
            else
            {
                if (Core.TOL && from.InRange(this, 2))
                {
                    list.Add(new CallbackEntry(1077728, () => OpenTrade(from))); // Trade
                }

                if (Alive && Core.Expansion >= Expansion.AOS)
                {
                    Party theirParty = from.Party as Party;
                    Party ourParty = Party as Party;

                    if (theirParty == null && ourParty == null)
                    {
                        list.Add(new AddToPartyEntry(from, this));
                    }
                    else if (theirParty != null && theirParty.Leader == from)
                    {
                        if (ourParty == null)
                        {
                            list.Add(new AddToPartyEntry(from, this));
                        }
                        else if (ourParty == theirParty)
                        {
                            list.Add(new RemoveFromPartyEntry(from, this));
                        }
                    }
                }

                BaseHouse curhouse = BaseHouse.FindHouseAt(this);

                if (curhouse != null)
                {
                    if (Alive && Core.Expansion >= Expansion.AOS && curhouse.IsAosRules && curhouse.IsFriend(from))
                    {
                        list.Add(new EjectPlayerEntry(from, this));
                    }
                }
            }
        }

        private void CancelProtection()
        {
            for (int i = 0; i < m_JusticeProtectors.Count; ++i)
            {
                Mobile prot = m_JusticeProtectors[i];

                string args = String.Format("{0}\t{1}", Name, prot.Name);

                prot.SendLocalizedMessage(1049371, args);// The protective relationship between ~1_PLAYER1~ and ~2_PLAYER2~ has been ended.
                SendLocalizedMessage(1049371, args);// The protective relationship between ~1_PLAYER1~ and ~2_PLAYER2~ has been ended.
            }

            m_JusticeProtectors.Clear();
        }

        #region Insurance
        private void ToggleItemInsurance()
        {
            if (!CheckAlive())
            {
                return;
            }

            this.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(ToggleItemInsurance_Callback));
            SendLocalizedMessage(1060868); // Target the item you wish to toggle insurance status on <ESC> to cancel
        }

        private void ToggleItemInsurance_Callback(Mobile from, object obj)
        {
            if (!CheckAlive())
            {
                return;
            }

            Item item = obj as Item;

            if (item == null || !item.IsChildOf(this))
            {
                this.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(ToggleItemInsurance_Callback));
                SendLocalizedMessage(1060871, "", 0x23); // You can only insure items that you have equipped or that are in your backpack
            }
            else if (item.Insured)
            {
                item.Insured = false;

                SendLocalizedMessage(1060874, "", 0x35); // You cancel the insurance on the item

                this.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(ToggleItemInsurance_Callback));
                SendLocalizedMessage(1060868, "", 0x23); // Target the item you wish to toggle insurance status on <ESC> to cancel
            }
            else if (!ItemInsuranceHelper.CanInsure(item) || item.BlessedFor == from)
            {
                if (item.LootType == LootType.Blessed || item.LootType == LootType.Newbied || item.BlessedFor == from)
                    SendLocalizedMessage(1060870, "", 0x23); // That item is blessed and does not need to be insured

                this.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(ToggleItemInsurance_Callback));
                SendLocalizedMessage(1060869, "", 0x23); // You cannot insure that
            }
            else
            {
                if (!item.PayedInsurance)
                {
                    int cost = ItemInsuranceHelper.GetInsuranceCost(item);

                    if (Banker.Withdraw(from, cost))
                    {
                        SendLocalizedMessage(1060398, cost.ToString()); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
                        item.PayedInsurance = true;
                    }
                    else
                    {
                        SendLocalizedMessage(1061079, "", 0x23); // You lack the funds to purchase the insurance
                        return;
                    }
                }

                item.Insured = true;

                SendLocalizedMessage(1060873, "", 0x23); // You have insured the item

                this.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(ToggleItemInsurance_Callback));
                SendLocalizedMessage(1060868, "", 0x23); // Target the item you wish to toggle insurance status on <ESC> to cancel
            }
        }
        #endregion

        private void ToggleTrades()
        {
            RefuseTrades = !RefuseTrades;
        }

        private void SelectRewardTitle()
        {
            CloseGump(typeof(SelectRewardTitleGump));
            SendGump(new SelectRewardTitleGump(this, m_CurrentCollectionTitle));
        }

        private void GetVendor()
        {
            BaseHouse house = BaseHouse.FindHouseAt(this);

            if (CheckAlive() && house != null && house.IsOwner(this) && house.InternalizedVendors.Count > 0)
            {
                CloseGump(typeof(ReclaimVendorGump));
                SendGump(new ReclaimVendorGump(house));
            }
        }

        private void LeaveHouse()
        {
            BaseHouse house = BaseHouse.FindHouseAt(this);

            if (house != null)
            {
                Location = house.BanLocation;
            }
        }

        private void LoyaltyRatingMenu()
        {
            CloseGump(typeof(LoyaltyGump));
            SendGump(new LoyaltyGump(this));
        }

        private void EnablePvpWarning()
        {
            DisabledPvpWarning = false;
            SendLocalizedMessage(1113798); // Your PvP warning query has been re-enabled.
        }

        private delegate void ContextCallback();

        private class CallbackEntry : ContextMenuEntry
        {
            private readonly ContextCallback m_Callback;

            public CallbackEntry(int number, ContextCallback callback)
                : this(number, -1, callback)
            { }

            public CallbackEntry(int number, int range, ContextCallback callback)
                : base(number, range)
            {
                m_Callback = callback;
            }

            public override void OnClick()
            {
                if (m_Callback != null)
                {
                    m_Callback();
                }
            }
        }

        public override void DisruptiveAction()
        {
            if (Meditating)
            {
                this.RemoveBuff(BuffIcon.ActiveMeditation);
            }

            base.DisruptiveAction();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (this == from && !Warmode)
            {
                IMount mount = Mount;

                if (mount != null && !DesignContext.Check(this))
                {
                    return;
                }
            }

            base.OnDoubleClick(from);
        }

        public override void DisplayPaperdollTo(Mobile to)
        {
            if (DesignContext.Check(this))
            {
                base.DisplayPaperdollTo(to);
            }
        }

        private static bool m_NoRecursion;

        public override bool CheckEquip(Item item)
        {
            if (!base.CheckEquip(item))
            {
                return false;
            }

            if (m_DuelContext != null && !m_DuelContext.AllowItemEquip(this, item))
            {
                return false;
            }

            FactionItem factionItem = FactionItem.Find(item);

            if (factionItem != null)
            {
                Faction faction = Faction.Find(this);

                if (faction == null)
                {
                    SendLocalizedMessage(1010371); // You cannot equip a faction item!
                    return false;
                }
                else if (faction != factionItem.Faction)
                {
                    SendLocalizedMessage(1010372); // You cannot equip an opposing faction's item!
                    return false;
                }
                else
                {
                    int maxWearables = FactionItem.GetMaxWearables(this);

                    for (int i = 0; i < Items.Count; ++i)
                    {
                        Item equiped = Items[i];

                        if (item != equiped && FactionItem.Find(equiped) != null)
                        {
                            if (--maxWearables == 0)
                            {
                                SendLocalizedMessage(1010373); // You do not have enough rank to equip more faction items!
                                return false;
                            }
                        }
                    }
                }
            }

            if (AccessLevel < AccessLevel.GameMaster && item.Layer != Layer.Mount && HasTrade)
            {
                BounceInfo bounce = item.GetBounce();

                if (bounce != null)
                {
                    if (bounce.m_Parent is Item)
                    {
                        Item parent = (Item)bounce.m_Parent;

                        if (parent == Backpack || parent.IsChildOf(Backpack))
                        {
                            return true;
                        }
                    }
                    else if (bounce.m_Parent == this)
                    {
                        return true;
                    }
                }

                SendLocalizedMessage(1004042); // You can only equip what you are already carrying while you have a trade pending.
                return false;
            }

            return true;
        }

        public override bool CheckTrade(Mobile to, Item item, SecureTradeContainer cont, bool message, bool checkItems, int plusItems, int plusWeight)
        {
            int msgNum = 0;

            if (cont == null)
            {
                if (to.Holding != null)
                {
                    msgNum = 1062727; // You cannot trade with someone who is dragging something.
                }
                else if (HasTrade)
                {
                    msgNum = 1062781; // You are already trading with someone else!
                }
                else if (to.HasTrade)
                {
                    msgNum = 1062779; // That person is already involved in a trade
                }
                else if (to is PlayerMobile && ((PlayerMobile)to).RefuseTrades)
                {
                    msgNum = 1154111; // ~1_NAME~ is refusing all trades. 
                }
            }

            if (msgNum == 0 && item != null)
            {
                if (cont != null)
                {
                    plusItems += cont.TotalItems;
                    plusWeight += cont.TotalWeight;
                }

                if (Backpack == null || !Backpack.CheckHold(this, item, false, checkItems, plusItems, plusWeight))
                {
                    msgNum = 1004040; // You would not be able to hold this if the trade failed.
                }
                else if (to.Backpack == null || !to.Backpack.CheckHold(to, item, false, checkItems, plusItems, plusWeight))
                {
                    msgNum = 1004039; // The recipient of this trade would not be able to carry this.
                }
                else
                {
                    msgNum = CheckContentForTrade(item);
                }
            }

            if (msgNum == 0)
            {
                return true;
            }

            if (!message)
            {
                return false;
            }

            if (msgNum == 1154111)
            {
                SendLocalizedMessage(msgNum, RawName);
            }
            else
            {
                SendLocalizedMessage(msgNum);
            }

            return false;
        }

        private static int CheckContentForTrade(Item item)
        {
            if (item is TrapableContainer && ((TrapableContainer)item).TrapType != TrapType.None)
            {
                return 1004044; // You may not trade trapped items.
            }

            if (StolenItem.IsStolen(item))
            {
                return 1004043; // You may not trade recently stolen items.
            }

            if (item is Container)
            {
                foreach (Item subItem in item.Items)
                {
                    int msg = CheckContentForTrade(subItem);

                    if (msg != 0)
                    {
                        return msg;
                    }
                }
            }

            return 0;
        }

        public override bool CheckNonlocalDrop(Mobile from, Item item, Item target)
        {
            if (!base.CheckNonlocalDrop(from, item, target))
            {
                return false;
            }

            if (from.AccessLevel >= AccessLevel.GameMaster)
            {
                return true;
            }

            Container pack = Backpack;
            if (from == this && HasTrade && (target == pack || target.IsChildOf(pack)))
            {
                BounceInfo bounce = item.GetBounce();

                if (bounce != null && bounce.m_Parent is Item)
                {
                    Item parent = (Item)bounce.m_Parent;

                    if (parent == pack || parent.IsChildOf(pack))
                    {
                        return true;
                    }
                }

                SendLocalizedMessage(1004041); // You can't do that while you have a trade pending.
                return false;
            }

            return true;
        }

        protected override void OnLocationChange(Point3D oldLocation)
        {
            CheckLightLevels(false);

            if (m_DuelContext != null)
            {
                m_DuelContext.OnLocationChanged(this);
            }

            DesignContext context = m_DesignContext;

            if (context == null || m_NoRecursion)
            {
                return;
            }

            m_NoRecursion = true;

            HouseFoundation foundation = context.Foundation;

            int newX = X, newY = Y;
            int newZ = foundation.Z + HouseFoundation.GetLevelZ(context.Level, context.Foundation);

            int startX = foundation.X + foundation.Components.Min.X + 1;
            int startY = foundation.Y + foundation.Components.Min.Y + 1;
            int endX = startX + foundation.Components.Width - 1;
            int endY = startY + foundation.Components.Height - 2;

            if (newX >= startX && newY >= startY && newX < endX && newY < endY && Map == foundation.Map)
            {
                if (Z != newZ)
                {
                    Location = new Point3D(X, Y, newZ);
                }

                m_NoRecursion = false;
                return;
            }

            Location = new Point3D(foundation.X, foundation.Y, newZ);
            Map = foundation.Map;

            m_NoRecursion = false;
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m is BaseCreature && !((BaseCreature)m).Controlled)
            {
                return (!Alive || !m.Alive || IsDeadBondedPet || m.IsDeadBondedPet) || (Hidden && IsStaff());
            }

            if (Region.IsPartOf(typeof(SafeZone)) && m is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)m;

                if (pm.DuelContext == null || pm.DuelPlayer == null || !pm.DuelContext.Started || pm.DuelContext.Finished ||
                    pm.DuelPlayer.Eliminated)
                {
                    return true;
                }
            }
            return base.OnMoveOver(m);
        }

        public override bool CheckShove(Mobile shoved)
        {
            if (TransformationSpell.UnderTransformation(shoved, typeof(WraithFormSpell)))
            {
                return true;
            }
            else
            {
                return base.CheckShove(shoved);
            }
        }

        protected override void OnMapChange(Map oldMap)
        {
            if ((Map != Faction.Facet && oldMap == Faction.Facet) || (Map == Faction.Facet && oldMap != Faction.Facet))
            {
                InvalidateProperties();
            }

            if (m_DuelContext != null)
            {
                m_DuelContext.OnMapChanged(this);
            }

            DesignContext context = m_DesignContext;

            if (context == null || m_NoRecursion)
            {
                return;
            }

            m_NoRecursion = true;

            HouseFoundation foundation = context.Foundation;

            if (Map != foundation.Map)
            {
                Map = foundation.Map;
            }

            m_NoRecursion = false;
        }

        public override void OnBeneficialAction(Mobile target, bool isCriminal)
        {
            if (m_SentHonorContext != null)
            {
                m_SentHonorContext.OnSourceBeneficialAction(target);
            }

            base.OnBeneficialAction(target, isCriminal);
        }

        public override void OnCombatantChange()
        {
            if (Combatant != null && FloorTrapKit.IsAssembling(this))
                FloorTrapKit.StopAssembling(this, 1113510); // You begin combat and cease trap assembly.
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            int disruptThreshold;

            if (!Core.AOS)
            {
                disruptThreshold = 0;
            }
            else if (from != null && from.Player)
            {
                disruptThreshold = 18;
            }
            else
            {
                disruptThreshold = 25;
            }

            if (amount > disruptThreshold)
            {
                BandageContext c = BandageContext.GetContext(this);

                if (c != null)
                {
                    c.Slip();
                }
            }

            if (FloorTrapKit.IsAssembling(this))
            {
                FloorTrapKit.StopAssembling(this, 1113512); // You are hit and cease trap assembly.
            }

            if (Confidence.IsRegenerating(this))
            {
                Confidence.StopRegenerating(this);
            }

            WeightOverloading.FatigueOnDamage(this, amount);

            if (m_ReceivedHonorContext != null)
            {
                m_ReceivedHonorContext.OnTargetDamaged(from, amount);
            }
            if (m_SentHonorContext != null)
            {
                m_SentHonorContext.OnSourceDamaged(from, amount);
            }

            if (amount > 0 && ParalyzingBlow.UnderEffect(this))
            {
                this.Frozen = false;
                ParalyzingBlow.m_Table.Remove(this);
            }

            if (willKill && from is PlayerMobile)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(10), ((PlayerMobile)@from).RecoverAmmo);
            }

            if (InvisibilityPotion.HasTimer(this))
            {
                InvisibilityPotion.Iterrupt(this);
            }

            if (Race == Race.Gargoyle && !willKill && !Berserk)
            {
                if (((float)(Hits - amount) / HitsMax) < 0.8)
                {
                    if (m_BerserkTimer != null)
                    {
                        m_BerserkTimer.Stop();
                    }

                    m_BerserkTimer = new BerserkTimer(this);
                    m_BerserkTimer.Start();
                }
            }

            int soulCharge = AosArmorAttributes.GetValue(this, AosArmorAttribute.SoulCharge);

            if (soulCharge > 0 && Mana < ManaMax)
            {
                int mana = (int)(amount * soulCharge / 100.0);

                if (mana > 0)
                {
                    Mana += mana;

                    Effects.SendPacket(this, Map, new TargetParticleEffect(this, 0x375A, 1, 10, 0x71, 2, 0x1AE9, 0, 0));
                    SendLocalizedMessage(1113636); // The soul charge effect converts some of the damage you received into mana.
                }
            }

            //GainBattleLust();

            base.OnDamage(amount, from, willKill);
        }

        private bool m_Berserk;
        private BerserkTimer m_BerserkTimer;

        public bool Berserk
        {
            get { return m_Berserk; }
            set { m_Berserk = value; }
        }

        public class BerserkTimer : Timer
        {
            private PlayerMobile m_Owner;

            public BerserkTimer(PlayerMobile owner)
                : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0))
            {
                m_Owner = owner;

                m_Owner.PlaySound(0x20F);
                m_Owner.PlaySound(m_Owner.Body.IsFemale ? 0x338 : 0x44A);
                m_Owner.FixedParticles(0x376A, 1, 31, 9961, 1160, 0, EffectLayer.Waist);
                m_Owner.FixedParticles(0x37C4, 1, 31, 9502, 43, 2, EffectLayer.Waist);

                BuffInfo.AddBuff(m_Owner, new BuffInfo(BuffIcon.Berserk, 1080449, 1115021, "15\t3", false));

                m_Owner.Berserk = true;
            }

            protected override void OnTick()
            {
                float percentage = (float)m_Owner.Hits / m_Owner.HitsMax;

                if (percentage >= 0.8)
                    RemoveEffect();
            }

            public void RemoveEffect()
            {
                m_Owner.PlaySound(0xF8);
                BuffInfo.RemoveBuff(m_Owner, BuffIcon.Berserk);

                m_Owner.Berserk = false;

                Stop();
            }
        }

        public override void Resurrect()
        {
            bool wasAlive = Alive;

            base.Resurrect();

            if (Alive && !wasAlive)
            {
                Item deathRobe = new DeathRobe();

                if (!EquipItem(deathRobe))
                {
                    deathRobe.Delete();
                }

                if (this.NetState != null && this.NetState.IsKRClient)
                {
                    List<BaseHealer> listHealers = new List<BaseHealer>();
                    List<MondainQuester> listQuesters = new List<MondainQuester>();
                    foreach (Mobile m_mobile in World.Mobiles.Values)
                    {
                        MondainQuester mQuester = m_mobile as MondainQuester;
                        if (mQuester != null)
                        {
                            listQuesters.Add(mQuester);
                        }

                        BaseHealer mHealer = m_mobile as BaseHealer;
                        if (mHealer != null)
                        {
                            listHealers.Add(mHealer);
                        }
                    }

                    if (this.Corpse != null)
                    {
                        this.NetState.Send(new DisplayWaypoint(this.Corpse.Serial, this.Corpse.X, this.Corpse.Y, this.Corpse.Z, this.Corpse.Map.MapID, 1, this.Name));
                    }

                    foreach (BaseHealer healer in listHealers)
                    {
                        this.NetState.Send(new HideWaypoint(healer.Serial));
                    }

                    foreach (MondainQuester quester in listQuesters)
                    {
                        string name = string.Empty;
                        if (quester.Name != null)
                        {
                            name += quester.Name;
                        }

                        if (quester.Title != null)
                        {
                            name += " " + quester.Title;
                        }
                        this.Send(new DisplayWaypoint(quester.Serial, quester.X, quester.Y, quester.Z, quester.Map.MapID, 4, name));
                    }
                }

                if (AcceleratedStart > DateTime.UtcNow)
                {
                    BuffInfo.AddBuff(this, new BuffInfo(BuffIcon.ArcaneEmpowerment, 1078511, 1078512, AcceleratedSkill.ToString()));
                }
            }
        }

        public override double RacialSkillBonus
        {
            get
            {
                if (Core.ML && Race == Race.Human)
                {
                    return 20.0;
                }

                return 0;
            }
        }

        public override double GetRacialSkillBonus(SkillName skill)
        {
            if (Core.ML && this.Race == Race.Human)
            {
                return 20.0;
            }

            if (Core.SA && this.Race == Race.Gargoyle)
            {
                if (skill == SkillName.Imbuing)
                {
                    return 30.0;
                }

                if (skill == SkillName.Throwing)
                {
                    return 20.0;
                }
            }

            return RacialSkillBonus;
        }

        public override void OnWarmodeChanged()
        {
            CheckKRStartingQuestStep(17);

            if (!Warmode)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(10), RecoverAmmo);
            }
        }

        private static void GiftOfLifeRes_Callback(object state)
        {
            Mobile m = (Mobile)state;

            if (!m.Alive)
            {
                m.SendGump(new ResurrectGump(m));
            }

            GiftOfLifeSpell.RemoveEffect(m);
        }

        private PlayerMobile m_InsuranceAward;
        private int m_InsuranceCost;
        private int m_InsuranceBonus;

        private List<Item> m_EquipSnapshot;

        public List<Item> EquipSnapshot { get { return m_EquipSnapshot; } }

        private bool FindItems_Callback(Item item)
        {
            if (!item.Deleted && (item.LootType == LootType.Blessed || item.Insured))
            {
                //if (Backpack != item.ParentEntity)
                if (Backpack != item.Parent)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool OnBeforeDeath()
        {
            NetState state = NetState;

            if (state != null)
            {
                state.CancelAllTrades();
            }

            DropHolding();

            if (Core.AOS && Backpack != null && !Backpack.Deleted)
            {
                var ilist = Backpack.FindItemsByType<Item>(FindItems_Callback);

                for (int i = 0; i < ilist.Count; i++)
                {
                    Backpack.AddItem(ilist[i]);
                }
            }

            m_EquipSnapshot = new List<Item>(Items);

            m_InsuranceCost = 0;
            m_InsuranceAward = FindInsuranceAward();

            if (m_InsuranceAward != null)
            {
                m_InsuranceAward.m_InsuranceBonus = 0;
            }

            if (m_ReceivedHonorContext != null)
            {
                m_ReceivedHonorContext.OnTargetKilled();
            }
            if (m_SentHonorContext != null)
            {
                m_SentHonorContext.OnSourceKilled();
            }

            Flying = false;

            if (m_BerserkTimer != null)
            {
                m_BerserkTimer.RemoveEffect();
            }

            if (SilverSaplingSeed.CanBeResurrected(this))
            {
                Timer.DelayCall<Mobile>(TimeSpan.FromSeconds(1.0), new TimerStateCallback<Mobile>(SilverSaplingSeed.OfferResurrection), this);
            }
            else if (GiftOfLifeSpell.UnderEffect(this))
            {
                Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerStateCallback(GiftOfLifeRes_Callback), this);
            }
            else if (Backpack != null)
            {
                GemOfSalvation gem = Backpack.FindItemByType<GemOfSalvation>();

                if (gem != null)
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(2.0), new TimerStateCallback<PlayerMobile>(gem.Use), this);
                }

                ShrineGem shrineGem = Backpack.FindItemByType<ShrineGem>();

                if (shrineGem != null)
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(2.0), new TimerStateCallback<PlayerMobile>(shrineGem.Use), this);
                }
            }

            AnimalForm.RemoveContext(this, true);

            RecoverAmmo();

            CheckKRStartingQuestStep(25);

            if (Transport != null)
                Transport.LeaveCommand(this);

            ++Deaths;

            if (Sphynx.UnderEffect(this))
            {
                SendLocalizedMessage(1060859); // The effects of the Sphynx have worn off.

                Sphynx.m_Table.Remove(this);
            }

            if (LogConfig.LogPvPLoggingEnabled)
            {
                PlayerMobile attacker = this.LastKiller as PlayerMobile;
                PlayerMobile victim = this as PlayerMobile;

                if (attacker is PlayerMobile && victim is PlayerMobile)
                {
                    string path = Core.BaseDirectory;
                    AppendPath(ref path, "Logs");
                    AppendPath(ref path, "PvP");
                    //path = Path.Combine(path, String.Format("{0}.log", attacker.Name));
                    path = Path.Combine(path, String.Format("Victim.{0}.log", victim.Name));
                    //path = Path.Combine(path, String.Format("Kills.log"));

                    using (StreamWriter sw = new StreamWriter(path, true))
                    {
                        sw.WriteLine("######################################################");
                        sw.WriteLine("Attacker: " + attacker.Name);
                        sw.WriteLine("Attacker's Account: " + attacker.Account);
                        sw.WriteLine("Victim: " + victim.Name);
                        sw.WriteLine("Victim's Account: " + victim.Account);
                        sw.WriteLine("Victim's Location of Death: " + victim.Location);
                        sw.WriteLine("Victim's Time of Death: " + DateTime.UtcNow);
                        sw.Close();
                    }
                }
            }
            return base.OnBeforeDeath();
        }

        private PlayerMobile FindInsuranceAward()
        {
            Mobile killer = FindMostRecentDamager(false);

            if (killer is BaseCreature)
            {
                Mobile master = ((BaseCreature)killer).GetMaster();

                if (master != null)
                {
                    killer = master;
                }
            }

            if (killer == this)
            {
                killer = null;
            }

            return killer as PlayerMobile;
        }

        private bool CheckInsuranceOnDeath(Item item)
        {
            if (InsuranceEnabled && item.Insured)
            {
                int itemCost = ItemInsuranceHelper.GetInsuranceCost(item);
                int halvedItemCost = itemCost / 2;

                if (AutoRenewInsurance)
                {
                    int victimCost = itemCost;

                    if (Banker.Withdraw(this, victimCost))
                    {
                        m_InsuranceCost += victimCost;
                        item.PayedInsurance = true;
                    }
                    else
                    {
                        SendLocalizedMessage(1061079, "", 0x23); // You lack the funds to purchase the insurance
                        item.PayedInsurance = false;
                        item.Insured = false;
                    }
                }
                else
                {
                    item.PayedInsurance = false;
                    item.Insured = false;
                }

                if (m_InsuranceAward != null)
                {
                    int killerAward = ((Faction.Find(m_InsuranceAward) == null)
                                            ? halvedItemCost
                                            : itemCost);

                    if (Banker.Deposit(m_InsuranceAward, killerAward))
                        m_InsuranceAward.m_InsuranceBonus += killerAward;
                }

                return true;
            }

            return false;
        }

        public override DeathMoveResult GetParentMoveResultFor(Item item)
        {
            if (CheckInsuranceOnDeath(item) && !Young)
            {
                return DeathMoveResult.MoveToBackpack;
            }

            DeathMoveResult res = base.GetParentMoveResultFor(item);

            if (res == DeathMoveResult.MoveToCorpse && item.Movable && Young)
            {
                res = DeathMoveResult.MoveToBackpack;
            }

            return res;
        }

        public override DeathMoveResult GetInventoryMoveResultFor(Item item)
        {
            if (CheckInsuranceOnDeath(item) && !Young)
            {
                return DeathMoveResult.MoveToBackpack;
            }

            DeathMoveResult res = base.GetInventoryMoveResultFor(item);

            if (res == DeathMoveResult.MoveToCorpse && item.Movable && Young)
            {
                res = DeathMoveResult.MoveToBackpack;
            }

            return res;
        }

        public override void OnDeath(Container c)
        {
            if (m_NonAutoreinsuredItems > 0)
            {
                SendLocalizedMessage(1061115);
            }

            base.OnDeath(c);

            m_EquipSnapshot = null;

            HueMod = -1;
            NameMod = null;
            SavagePaintExpiration = TimeSpan.Zero;

            SetHairMods(-1, -1);

            PolymorphSpell.StopTimer(this);
            IncognitoSpell.StopTimer(this);
            DisguiseTimers.RemoveTimer(this);

            EndAction(typeof(PolymorphSpell));
            EndAction(typeof(IncognitoSpell));

            MeerMage.StopEffect(this, false);

            if (Flying)
            {
                Flying = false;
                BuffInfo.RemoveBuff(this, BuffIcon.Flying);
            }

            StolenItem.ReturnOnDeath(this, c);

            if (m_PermaFlags.Count > 0)
            {
                m_PermaFlags.Clear();

                if (c is Corpse)
                {
                    ((Corpse)c).Criminal = true;
                }

                if (Stealing.ClassicMode)
                {
                    Criminal = true;
                }
            }

            if (Kills >= 5 && DateTime.UtcNow >= m_NextJustAward)
            {
                Mobile m = FindMostRecentDamager(false);

                if (m is BaseCreature)
                {
                    m = ((BaseCreature)m).GetMaster();
                }

                if (m != null && m is PlayerMobile && m != this)
                {
                    bool gainedPath = false;

                    int pointsToGain = 0;

                    pointsToGain += (int)Math.Sqrt(GameTime.TotalSeconds * 4);
                    pointsToGain *= 5;
                    pointsToGain += (int)Math.Pow(Skills.Total / 250, 2);

                    if (VirtueHelper.Award(m, VirtueName.Justice, pointsToGain, ref gainedPath))
                    {
                        if (gainedPath)
                        {
                            m.SendLocalizedMessage(1049367); // You have gained a path in Justice!
                        }
                        else
                        {
                            m.SendLocalizedMessage(1049363); // You have gained in Justice.
                        }

                        m.FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Waist);
                        m.PlaySound(0x1F7);

                        m_NextJustAward = DateTime.UtcNow + TimeSpan.FromMinutes(pointsToGain / 3);
                    }
                }
            }

            if (m_InsuranceCost > 0)
            {
                SendLocalizedMessage(1060398, m_InsuranceCost.ToString()); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
            }

            if (m_InsuranceAward != null)
            {
                PlayerMobile pm = m_InsuranceAward;

                if (pm.m_InsuranceBonus > 0)
                {
                    pm.SendLocalizedMessage(1060397, pm.m_InsuranceBonus.ToString()); // ~1_AMOUNT~ gold has been deposited into your bank box.
                }
            }

            Mobile killer = FindMostRecentDamager(true);

            if (killer is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)killer;

                Mobile master = bc.GetMaster();
                if (master != null)
                {
                    killer = master;
                }
            }

            if (Young && m_DuelContext == null && m_KRStartingQuestStep == 0)
            {
                if (YoungDeathTeleport())
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(2.5), SendYoungDeathNotice);
                }
            }

            if (Region.IsPartOf(typeof(UnderworldDeathRegion)))
            {
                Point3D dest = new Point3D(1060, 1066, -42);

                MoveToWorld(dest, Map.TerMur);
                c.MoveToWorld(dest, Map.TerMur);

                SendLocalizedMessage(1113566); // You will find your remains at the entrance of the maze. 
            }

            if (m_DuelContext == null || !m_DuelContext.Registered || !m_DuelContext.Started || m_DuelPlayer == null || m_DuelPlayer.Eliminated || !XmlPoints.AreChallengers(this, killer))
            {
                Faction.HandleDeath(this, killer);
            }

            Guilds.Guild.HandleDeath(this, killer);

            if (m_DuelContext != null)
            {
                m_DuelContext.OnDeath(this, c);
            }

            if (this.NetState != null && this.NetState.IsKRClient)
            {
                List<BaseHealer> listHealers = new List<BaseHealer>();
                List<MondainQuester> listQuesters = new List<MondainQuester>();

                foreach (Mobile m_mobile in World.Mobiles.Values)
                {
                    MondainQuester mQuester = m_mobile as MondainQuester;
                    if (mQuester != null)
                    {
                        listQuesters.Add(mQuester);
                    }

                    BaseHealer mHealer = m_mobile as BaseHealer;
                    if (mHealer != null)
                    {
                        listHealers.Add(mHealer);
                    }
                }

                foreach (BaseHealer healer in listHealers)
                {
                    string name = string.Empty;
                    if (healer.Name != null)
                    {
                        name += healer.Name;
                    }

                    if (healer.Title != null)
                    {
                        name += " " + healer.Title;
                    }
                    this.NetState.Send(new DisplayWaypoint(healer.Serial, healer.X, healer.Y, healer.Z, healer.Map.MapID, 6, name));
                }

                foreach (MondainQuester quester in listQuesters)
                {
                    this.NetState.Send(new HideWaypoint(quester.Serial));
                }

                if (this.Corpse != null)
                {
                    this.NetState.Send(new HideWaypoint(this.Corpse.Serial));
                }
            }

            if (Region.IsPartOf("Abyss") && SSSeedExpire > DateTime.UtcNow)
            {
                SendGump(new ResurrectGump(this, ResurrectMessage.SilverSapling));
            }
        }

        public static void AppendPath(ref string path, string toAppend)
        {
            path = Path.Combine(path, toAppend);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private DateTime[] m_StuckMenuUses;

        public bool CanUseStuckMenu()
        {
            if (m_StuckMenuUses == null)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < m_StuckMenuUses.Length; ++i)
                {
                    if ((DateTime.UtcNow - m_StuckMenuUses[i]) > TimeSpan.FromDays(1.0))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void UsedStuckMenu()
        {
            if (m_StuckMenuUses == null)
            {
                m_StuckMenuUses = new DateTime[2];
            }

            for (int i = 0; i < m_StuckMenuUses.Length; ++i)
            {
                if ((DateTime.UtcNow - m_StuckMenuUses[i]) > TimeSpan.FromDays(1.0))
                {
                    m_StuckMenuUses[i] = DateTime.UtcNow;
                    return;
                }
            }
        }

        public override void OnItemLifted(Mobile from, Item item)
        {
            if (item.Parent is KRStartingQuestContainer)
            {
                CheckKRStartingQuestStep(8);
            }

            if (item.Parent is Corpse)
            {
                Corpse c = (Corpse)item.Parent;

                if (c.TotalItems == 1)
                {
                    CheckKRStartingQuestStep(21);
                }
            }

            base.OnItemLifted(from, item);
        }

        private List<Mobile> m_PermaFlags;
        private readonly List<Mobile> m_VisList;
        private readonly Hashtable m_AntiMacroTable;
        private TimeSpan m_GameTime;
        private TimeSpan m_ShortTermElapse;
        private TimeSpan m_LongTermElapse;
        private DateTime m_SessionStart;
        private DateTime m_NextSmithBulkOrder;
        private DateTime m_NextTailorBulkOrder;
        private DateTime m_NextSmithBulkOrder74;
        private DateTime m_NextTailorBulkOrder74;

        private DateTime m_SavagePaintExpiration;
        private SkillName m_Learning = (SkillName)(-1);

        private DateTime m_NextDrinkMaskOfDeathPotion;
        private DateTime m_NextDrinkExplodingTarPotion;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan NextDrinkMaskOfDeathPotion
        {
            get
            {
                TimeSpan ts = m_NextDrinkMaskOfDeathPotion - DateTime.UtcNow;

                if (ts < TimeSpan.Zero)
                {
                    ts = TimeSpan.Zero;
                }

                return ts;
            }
            set
            {
                try
                {
                    m_NextDrinkMaskOfDeathPotion = DateTime.UtcNow + value;
                }
                catch
                {
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan NextDrinkExplodingTarPotion
        {
            get
            {
                TimeSpan ts = m_NextDrinkExplodingTarPotion - DateTime.UtcNow;

                if (ts < TimeSpan.Zero)
                {
                    ts = TimeSpan.Zero;
                }

                return ts;
            }
            set
            {
                try
                {
                    m_NextDrinkExplodingTarPotion = DateTime.UtcNow + value;
                }
                catch
                {
                }
            }
        }

        private DateTime m_NextPuzzleAttempt;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextPuzzleAttempt
        {
            get { return m_NextPuzzleAttempt; }
            set { m_NextPuzzleAttempt = value; }
        }

        private DateTime m_NextLuckyCoinWish;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextLuckyCoinWish
        {
            get { return m_NextLuckyCoinWish; }
            set { m_NextLuckyCoinWish = value; }
        }

        public SkillName Learning { get { return m_Learning; } set { m_Learning = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan SavagePaintExpiration
        {
            get
            {
                TimeSpan ts = m_SavagePaintExpiration - DateTime.UtcNow;

                if (ts < TimeSpan.Zero)
                {
                    ts = TimeSpan.Zero;
                }

                return ts;
            }
            set { m_SavagePaintExpiration = DateTime.UtcNow + value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan NextSmithBulkOrder
        {
            get
            {
                TimeSpan ts = m_NextSmithBulkOrder - DateTime.UtcNow;

                if (ts < TimeSpan.Zero)
                {
                    ts = TimeSpan.Zero;
                }

                return ts;
            }
            set
            {
                try
                {
                    m_NextSmithBulkOrder = DateTime.UtcNow + value;
                }
                catch
                { }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan NextTailorBulkOrder
        {
            get
            {
                TimeSpan ts = m_NextTailorBulkOrder - DateTime.UtcNow;

                if (ts < TimeSpan.Zero)
                {
                    ts = TimeSpan.Zero;
                }

                return ts;
            }
            set
            {
                try
                {
                    m_NextTailorBulkOrder = DateTime.UtcNow + value;
                }
                catch
                { }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextSmithBulkOrder74
        {
            get
            {
                if (m_NextSmithBulkOrder74 == null)
                {
                    return DateTime.MinValue;
                }
                return m_NextSmithBulkOrder74;
            }
            set
            {
                m_NextSmithBulkOrder74 = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextTailorBulkOrder74
        {
            get
            {
                if (m_NextTailorBulkOrder74 == null)
                {
                    return DateTime.MinValue;
                }
                return m_NextTailorBulkOrder74;
            }
            set
            {
                m_NextTailorBulkOrder74 = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastEscortTime { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastPetBallTime { get; set; }

        public PlayerMobile()
        {
            m_CollectionTitles = new ArrayList();
            m_CollectionTitles.Add(1073995); // [No Title]

            m_AutoStabled = new List<Mobile>();

            m_Quests = new List<BaseQuest>();
            m_Chains = new Dictionary<QuestChain, BaseChain>();
            m_DoneQuests = new List<QuestRestartInfo>();

            m_PeacedUntil = DateTime.UtcNow;

            m_VisList = new List<Mobile>();
            m_PermaFlags = new List<Mobile>();
            m_AntiMacroTable = new Hashtable();
            m_RecentlyReported = new List<Mobile>();

            m_BOBFilter = new BOBFilter();

            m_GameTime = TimeSpan.Zero;
            m_ShortTermElapse = TimeSpan.FromHours(8.0);
            m_LongTermElapse = TimeSpan.FromHours(40.0);

            m_JusticeProtectors = new List<Mobile>();
            m_GuildRank = RankDefinition.Lowest;

            m_LoyaltyInfo = new LoyaltyInfo();
            m_TieredQuestInfo = new TieredQuestInfo();

            InvalidateMyShard();
        }

        public override bool MutateSpeech(List<Mobile> hears, ref string text, ref object context)
        {
            if (Alive)
            {
                return false;
            }

            if (Core.ML && Skills[SkillName.SpiritSpeak].Value >= 100.0)
            {
                return false;
            }

            if (Core.AOS)
            {
                for (int i = 0; i < hears.Count; ++i)
                {
                    Mobile m = hears[i];

                    if (m != this && m.Skills[SkillName.SpiritSpeak].Value >= 100.0)
                    {
                        return false;
                    }
                }
            }

            return base.MutateSpeech(hears, ref text, ref context);
        }

        public override void DoSpeech(string text, int[] keywords, MessageType type, int hue)
        {
            if (Guilds.Guild.NewGuildSystem && (type == MessageType.Guild || type == MessageType.Alliance))
            {
                Guild g = Guild as Guild;
                if (g == null)
                {
                    SendLocalizedMessage(1063142); // You are not in a guild!
                }
                else if (type == MessageType.Alliance)
                {
                    if (g.Alliance != null && g.Alliance.IsMember(g))
                    {
                        //g.Alliance.AllianceTextMessage( hue, "[Alliance][{0}]: {1}", this.Name, text );
                        g.Alliance.AllianceChat(this, text);
                        SendToStaffMessage(this, "[Alliance]: {0}", text);

                        m_AllianceMessageHue = hue;
                    }
                    else
                    {
                        SendLocalizedMessage(1071020); // You are not in an alliance!
                    }
                }
                else //Type == MessageType.Guild
                {
                    m_GuildMessageHue = hue;

                    g.GuildChat(this, text);
                    SendToStaffMessage(this, "[Guild]: {0}", text);
                }
            }
            else
            {
                base.DoSpeech(text, keywords, type, hue);
            }
        }

        private static void SendToStaffMessage(Mobile from, string text)
        {
            Packet p = null;

            foreach (NetState ns in from.GetClientsInRange(8))
            {
                Mobile mob = ns.Mobile;

                if (mob != null && mob.AccessLevel >= AccessLevel.GameMaster && mob.AccessLevel > from.AccessLevel)
                {
                    if (p == null)
                    {
                        p =
                            Packet.Acquire(
                                new UnicodeMessage(
                                    from.Serial, from.Body, MessageType.Regular, from.SpeechHue, 3, from.Language, from.Name, text));
                    }

                    ns.Send(p);
                }
            }

            Packet.Release(p);
        }

        private static void SendToStaffMessage(Mobile from, string format, params object[] args)
        {
            SendToStaffMessage(from, String.Format(format, args));
        }

        public override void Damage(int amount, Mobile from)
        {
            if (EvilOmenSpell.CheckEffect(this))
            {
                amount = (int)(amount * 1.25);
            }

            Mobile oath = BloodOathSpell.GetBloodOath(from);

            /* Per EA's UO Herald Pub48 (ML):
            * ((resist spellsx10)/20 + 10=percentage of damage resisted)
            */

            if (oath == this)
            {
                amount = (int)(amount * 1.1);

                if (amount > 35 && from is PlayerMobile) /* capped @ 35, seems no expansion */
                {
                    amount = 35;
                }

                if (Core.ML)
                {
                    from.Damage((int)(amount * (1 - (((from.Skills.MagicResist.Value * .5) + 10) / 100))), this);
                }
                else
                {
                    from.Damage(amount, this);
                }
            }

            if (from != null && Talisman is BaseTalisman)
            {
                BaseTalisman talisman = (BaseTalisman)Talisman;

                if (talisman.Protection != null && talisman.Protection.Type != null)
                {
                    Type type = talisman.Protection.Type;

                    if (type.IsAssignableFrom(from.GetType()))
                    {
                        amount = (int)(amount * (1 - (double)talisman.Protection.Amount / 100));
                    }
                }
            }

            base.Damage(amount, from);
        }

        public override ApplyPoisonResult ApplyPoison(Mobile from, Poison poison)
        {
            if (!Alive)
            {
                return ApplyPoisonResult.Immune;
            }

            if (EvilOmenSpell.CheckEffect(this))
            {
                poison = PoisonImpl.IncreaseLevel(poison);
            }

            ApplyPoisonResult result = base.ApplyPoison(from, poison);

            if (from != null && result == ApplyPoisonResult.Poisoned && PoisonTimer is PoisonImpl.PoisonTimer)
            {
                (PoisonTimer as PoisonImpl.PoisonTimer).From = from;
            }

            return result;
        }

        public override bool CheckPoisonImmunity(Mobile from, Poison poison)
        {
            if (Young && (DuelContext == null || !DuelContext.Started || DuelContext.Finished))
            {
                return true;
            }

            return base.CheckPoisonImmunity(from, poison);
        }

        public override void OnPoisonImmunity(Mobile from, Poison poison)
        {
            if (Young && (DuelContext == null || !DuelContext.Started || DuelContext.Finished))
            {
                SendLocalizedMessage(502808); // You would have been poisoned, were you not new to the land of Britannia. Be careful in the future.
            }
            else
            {
                base.OnPoisonImmunity(from, poison);
            }
        }

        public PlayerMobile(Serial s)
            : base(s)
        {
            m_VisList = new List<Mobile>();
            m_AntiMacroTable = new Hashtable();
            InvalidateMyShard();
        }

        public List<Mobile> VisibilityList { get { return m_VisList; } }
        public List<Mobile> PermaFlags { get { return m_PermaFlags; } }

        public override int Luck
        {
            get
            {
                int luck = AosAttributes.GetValue(this, AosAttribute.Luck);

                luck += m_LuckMod;

                if (FontOfFortune.HasBlessing(this, FontOfFortune.BlessingType.Luck))
                    luck += 400;

                return luck;
            }
        }

        private int m_LuckMod;
        [CommandProperty(AccessLevel.GameMaster)]
        public int LuckMod
        {
            get { return m_LuckMod != 0 ? m_LuckMod : 0; }
            set { m_LuckMod = value; InvalidateProperties(); }
        }

        public override int AttackChance { get { return AosAttributes.GetValue(this, AosAttribute.AttackChance); } }
        public override int WeaponSpeed { get { return AosAttributes.GetValue(this, AosAttribute.WeaponSpeed); } }
        public override int WeaponDamage { get { return AosAttributes.GetValue(this, AosAttribute.WeaponDamage); } }
        public override int LowerRegCost { get { return AosAttributes.GetValue(this, AosAttribute.LowerRegCost); } }
        public override int RegenHits { get { return AosAttributes.GetValue(this, AosAttribute.RegenHits); } }
        public override int RegenStam { get { return AosAttributes.GetValue(this, AosAttribute.RegenStam); } }
        public override int RegenMana { get { return AosAttributes.GetValue(this, AosAttribute.RegenMana); } }
        public override int ReflectPhysical { get { return AosAttributes.GetValue(this, AosAttribute.ReflectPhysical); } }
        public override int EnhancePotions { get { return AosAttributes.GetValue(this, AosAttribute.EnhancePotions); } }
        public override int DefendChance { get { return AosAttributes.GetValue(this, AosAttribute.DefendChance); } }
        public override int SpellDamage { get { return AosAttributes.GetValue(this, AosAttribute.SpellDamage); } }
        public override int CastRecovery { get { return AosAttributes.GetValue(this, AosAttribute.CastRecovery); } }
        public override int CastSpeed { get { return AosAttributes.GetValue(this, AosAttribute.CastSpeed); } }
        public override int LowerManaCost { get { return AosAttributes.GetValue(this, AosAttribute.LowerManaCost); } }
        public override int BonusStr { get { return AosAttributes.GetValue(this, AosAttribute.BonusStr); } }
        public override int BonusDex { get { return AosAttributes.GetValue(this, AosAttribute.BonusDex); } }
        public override int BonusInt { get { return AosAttributes.GetValue(this, AosAttribute.BonusInt); } }
        public override int BonusHits { get { return AosAttributes.GetValue(this, AosAttribute.BonusHits); } }
        public override int BonusStam { get { return AosAttributes.GetValue(this, AosAttribute.BonusStam); } }
        public override int BonusMana { get { return AosAttributes.GetValue(this, AosAttribute.BonusMana); } }

        public override bool IsHarmfulCriminal(Mobile target)
        {
            if (Stealing.ClassicMode && target is PlayerMobile && ((PlayerMobile)target).m_PermaFlags.Count > 0)
            {
                int noto = Notoriety.Compute(this, target);

                if (noto == Notoriety.Innocent)
                {
                    target.Delta(MobileDelta.Noto);
                }

                return false;
            }

            if (target is BaseCreature && ((BaseCreature)target).InitialInnocent && !((BaseCreature)target).Controlled)
            {
                return false;
            }

            if (Core.ML && target is BaseCreature && ((BaseCreature)target).Controlled &&
                this == ((BaseCreature)target).ControlMaster)
            {
                return false;
            }

            return base.IsHarmfulCriminal(target);
        }

        public bool AntiMacroCheck(Skill skill, object obj)
        {
            if (obj == null || m_AntiMacroTable == null || IsStaff())
            {
                return true;
            }

            Hashtable tbl = (Hashtable)m_AntiMacroTable[skill];
            if (tbl == null)
            {
                m_AntiMacroTable[skill] = tbl = new Hashtable();
            }

            CountAndTimeStamp count = (CountAndTimeStamp)tbl[obj];
            if (count != null)
            {
                if (count.TimeStamp + SkillCheck.AntiMacroExpire <= DateTime.UtcNow)
                {
                    count.Count = 1;
                    return true;
                }
                else
                {
                    ++count.Count;
                    if (count.Count <= SkillCheck.Allowance)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                tbl[obj] = count = new CountAndTimeStamp();
                count.Count = 1;

                return true;
            }
        }

        private void RevertHair()
        {
            SetHairMods(-1, -1);
        }

        private BOBFilter m_BOBFilter;

        public BOBFilter BOBFilter { get { return m_BOBFilter; } }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_CollectionTitles = new ArrayList();

            switch (version)
            {
                case 49:
                    {
                        LastTierLoss = reader.ReadDeltaTime();
                        LastChampionTierLoss = reader.ReadDeltaTime();
                        LastSuperChampionTierLoss = reader.ReadDeltaTime();

                        int length_super = reader.ReadInt();

                        for (int i = 0; i < length_super; i++)
                        {
                            SuperChampionTiers[i] = reader.ReadInt();
                        }

                        int length = reader.ReadInt();

                        for (int i = 0; i < length; i++)
                        {
                            ChampionTiers[i] = reader.ReadDouble();
                        }
                        goto case 48;
                    }
                case 48:
                    {
                        m_NextGemOfSalvationUse = reader.ReadDateTime();
                        goto case 47;
                    }
                case 47:
                    {
                        NextDrinkMaskOfDeathPotion = reader.ReadTimeSpan();
                        goto case 46;
                    }
                case 46:
                    {
                        m_LastForgedPardonUse = reader.ReadDateTime();

                        goto case 45;
                    }
                case 45:
                    {
                        ACState = (AdvancedCharacterState)reader.ReadInt();
                        goto case 44;
                    }
                case 44:
                    {
                        m_NextTenthAnniversarySculptureUse = reader.ReadDateTime();
                        goto case 43;
                    }
                case 43:
                    {
                        m_LuckMod = reader.ReadInt();
                        goto case 42;
                    }
                case 42:
                    {
                        m_LoyaltyInfo = new LoyaltyInfo(reader);
                        goto case 41;
                    }
                case 41:
                    {
                        m_NextLuckyCoinWish = reader.ReadDateTime();
                        goto case 40;
                    }
                case 40:
                    {
                        m_HumilityQuestStatus = (HumilityQuestStatus)reader.ReadInt();
                        m_HumilityQuestNextChance = reader.ReadDateTime();
                        goto case 39;
                    }
                case 39:
                    {
                        m_CurrentCollectionTitle = reader.ReadInt();
                        int titlecount = reader.ReadInt();

                        for (int i = 0; i < titlecount; i++)
                        {
                            int title = reader.ReadInt();
                            m_CollectionTitles.Add(title);
                        }
                        goto case 38;
                    }
                case 38:
                    {
                        bool hasAnyBardMastery = reader.ReadBool();

                        if (hasAnyBardMastery)
                        {
                            m_BardMastery = BardMastery.GetFromId(reader.ReadEncodedInt());
                            m_BardElementDamage = (ResistanceType)reader.ReadEncodedInt();
                            m_NextBardMasterySwitch = reader.ReadDateTime();
                            m_BardMasteryLearnedMask = reader.ReadEncodedInt();
                        }

                        goto case 37;
                    }
                case 37:
                    {
                        m_NextDrinkExplodingTarPotion = reader.ReadDateTime();
                        goto case 36;
                    }
                case 36:
                    {
                        Deaths = reader.ReadInt();
                        goto case 35;
                    }
                case 35:
                    {
                        m_FloorTrapsPlaced = reader.ReadInt();
                        goto case 34;
                    }
                case 34:
                    {
                        m_TieredQuestInfo = new TieredQuestInfo(reader);
                        goto case 33;
                    }
                case 33:
                    {
                        m_KRStartingQuestStep = reader.ReadByte();
                        goto case 32;
                    }
                case 32:
                    {
                        m_SacredQuestNextChance = reader.ReadDateTime();
                        goto case 31;
                    }
                case 31:
                    {
                        m_NextPuzzleAttempt = reader.ReadDateTime();
                        goto case 30;
                    }
                case 30:
                    {
                        if (reader.ReadBool())
                        {
                            m_StuckMenuUses = new DateTime[reader.ReadInt()];

                            for (int i = 0; i < m_StuckMenuUses.Length; ++i)
                            {
                                m_StuckMenuUses[i] = reader.ReadDateTime();
                            }
                        }
                        else
                        {
                            m_StuckMenuUses = null;
                        }

                        goto case 29;
                    }
                case 29:
                    {
                        try
                        {
                            NextTailorBulkOrder74 = reader.ReadDateTime();
                        }
                        catch
                        {
                            NextTailorBulkOrder74 = DateTime.MinValue;
                        }
                        goto case 28;
                    }
                case 28:
                    {
                        try
                        {
                            NextSmithBulkOrder74 = reader.ReadDateTime();
                        }
                        catch
                        {
                            NextSmithBulkOrder74 = DateTime.MinValue;
                        }
                        goto case 27;
                    }
                case 27:
                    {
                        m_PromoGiftLast = reader.ReadDateTime();
                        goto case 26;
                    }

                case 26:
                    {
                        m_LastTimePaged = reader.ReadDateTime();
                        goto case 25;
                    }
                case 25:
                    {
                        m_DoomCredits = reader.ReadInt();

                        m_SSNextSeed = reader.ReadDateTime();
                        m_SSSeedExpire = reader.ReadDateTime();
                        m_SSSeedLocation = reader.ReadPoint3D();
                        m_SSSeedMap = reader.ReadMap();

                        m_ShamePoints = reader.ReadLong();

                        m_VASTotalMonsterFame = reader.ReadInt();

                        m_Quests = QuestReader.Quests(reader, this);
                        m_Chains = QuestReader.Chains(reader);

                        goto case 24;
                    }
                case 24:
                    {
                        m_PeacedUntil = reader.ReadDateTime();

                        goto case 23;
                    }
                case 23:
                    {
                        m_AnkhNextUse = reader.ReadDateTime();

                        goto case 22;
                    }
                case 22:
                    {
                        m_AutoStabled = reader.ReadStrongMobileList();

                        goto case 21;
                    }
                case 21:
                    {
                        int recipeCount = reader.ReadInt();

                        if (recipeCount > 0)
                        {
                            m_AcquiredRecipes = new Dictionary<int, bool>();

                            for (int i = 0; i < recipeCount; i++)
                            {
                                int r = reader.ReadInt();
                                if (reader.ReadBool()) //Don't add in recipies which we haven't gotten or have been removed
                                {
                                    m_AcquiredRecipes.Add(r, true);
                                }
                            }
                        }
                        goto case 20;
                    }
                case 20:
                    {
                        m_LastHonorLoss = reader.ReadDeltaTime();
                        goto case 19;
                    }
                case 19:
                    {
                        m_LastValorLoss = reader.ReadDateTime();
                        goto case 18;
                    }
                case 18:
                    {
                        m_ToTItemsTurnedIn = reader.ReadEncodedInt();
                        m_ToTTotalMonsterFame = reader.ReadInt();
                        goto case 17;
                    }
                case 17:
                    {
                        m_AllianceMessageHue = reader.ReadEncodedInt();
                        m_GuildMessageHue = reader.ReadEncodedInt();

                        goto case 16;
                    }
                case 16:
                    {
                        int rank = reader.ReadEncodedInt();
                        int maxRank = RankDefinition.Ranks.Length - 1;
                        if (rank > maxRank)
                        {
                            rank = maxRank;
                        }

                        m_GuildRank = RankDefinition.Ranks[rank];
                        m_LastOnline = reader.ReadDateTime();
                        goto case 15;
                    }
                case 15:
                    {
                        m_SolenFriendship = (SolenFriendship)reader.ReadEncodedInt();

                        goto case 14;
                    }
                case 14:
                    {
                        m_Quest = QuestSerializer.DeserializeQuest(reader);

                        if (m_Quest != null)
                        {
                            m_Quest.From = this;
                        }

                        int count = reader.ReadEncodedInt();

                        if (count > 0)
                        {
                            m_DoneQuests = new List<QuestRestartInfo>();

                            for (int i = 0; i < count; ++i)
                            {
                                Type questType = QuestSerializer.ReadType(QuestSystem.QuestTypes, reader);
                                DateTime restartTime;

                                try
                                {
                                    restartTime = DateTime.MaxValue;
                                }
                                catch
                                {
                                    restartTime = reader.ReadDateTime();
                                }

                                m_DoneQuests.Add(new QuestRestartInfo(questType, restartTime));
                            }
                        }

                        m_Profession = reader.ReadEncodedInt();
                        goto case 13;
                    }
                case 13:
                    {
                        m_LastCompassionLoss = reader.ReadDeltaTime();
                        goto case 12;
                    }
                case 12:
                    {
                        m_CompassionGains = reader.ReadEncodedInt();

                        if (m_CompassionGains > 0)
                        {
                            m_NextCompassionDay = reader.ReadDeltaTime();
                        }

                        goto case 11;
                    }
                case 11:
                    {
                        m_BOBFilter = new BOBFilter(reader);
                        goto case 10;
                    }
                case 10:
                    {
                        if (reader.ReadBool())
                        {
                            m_HairModID = reader.ReadInt();
                            m_HairModHue = reader.ReadInt();
                            m_BeardModID = reader.ReadInt();
                            m_BeardModHue = reader.ReadInt();
                        }

                        goto case 9;
                    }
                case 9:
                    {
                        SavagePaintExpiration = reader.ReadTimeSpan();

                        if (SavagePaintExpiration > TimeSpan.Zero)
                        {
                            BodyMod = (Female ? 184 : 183);
                            HueMod = 0;
                        }

                        goto case 8;
                    }
                case 8:
                    {
                        m_NpcGuild = (NpcGuild)reader.ReadInt();
                        m_NpcGuildJoinTime = reader.ReadDateTime();
                        m_NpcGuildGameTime = reader.ReadTimeSpan();
                        goto case 7;
                    }
                case 7:
                    {
                        m_PermaFlags = reader.ReadStrongMobileList();
                        goto case 6;
                    }
                case 6:
                    {
                        NextTailorBulkOrder = reader.ReadTimeSpan();
                        goto case 5;
                    }
                case 5:
                    {
                        NextSmithBulkOrder = reader.ReadTimeSpan();
                        goto case 4;
                    }
                case 4:
                    {
                        m_LastJusticeLoss = reader.ReadDeltaTime();
                        m_JusticeProtectors = reader.ReadStrongMobileList();
                        goto case 3;
                    }
                case 3:
                    {
                        m_LastSacrificeGain = reader.ReadDeltaTime();
                        m_LastSacrificeLoss = reader.ReadDeltaTime();
                        m_AvailableResurrects = reader.ReadInt();
                        goto case 2;
                    }
                case 2:
                    {
                        m_Flags = (PlayerFlag)reader.ReadInt();
                        goto case 1;
                    }
                case 1:
                    {
                        m_LongTermElapse = reader.ReadTimeSpan();
                        m_ShortTermElapse = reader.ReadTimeSpan();
                        m_GameTime = reader.ReadTimeSpan();
                        goto case 0;
                    }
                case 0:
                    {
                        if (version < 26)
                        {
                            m_AutoStabled = new List<Mobile>();
                        }
                        break;
                    }
            }

            if (this != null && this.Region.IsPartOf(typeof(DoomLampRoom)))
            {
                Rectangle2D rect = new Rectangle2D(342, 168, 16, 16);

                int x = Utility.Random(rect.X, rect.Width);
                int y = Utility.Random(rect.Y, rect.Height);

                if (x >= 345 && x <= 352 && y >= 173 && y <= 179)
                {
                    x = 353;
                    y = 172;
                }

                this.MoveToWorld(new Point3D(x, y, -1), Map.Malas);
            }

            if (version < 29)
            {
                m_SSNextSeed = m_SSSeedExpire = DateTime.UtcNow;
                m_SSSeedLocation = Point3D.Zero;
            }

            if (m_RecentlyReported == null)
            {
                m_RecentlyReported = new List<Mobile>();
            }

            if (version < 29)
            {
                m_ShamePoints = 0;
            }

            if (m_Quests == null)
            {
                m_Quests = new List<BaseQuest>();
            }

            if (m_Chains == null)
            {
                m_Chains = new Dictionary<QuestChain, BaseChain>();
            }

            if (m_DoneQuests == null)
            {
                m_DoneQuests = new List<QuestRestartInfo>();
            }

            // Professions weren't verified on 1.0 RC0
            if (!CharacterCreation.VerifyProfession(m_Profession))
            {
                m_Profession = 0;
            }

            if (m_PermaFlags == null)
            {
                m_PermaFlags = new List<Mobile>();
            }

            if (m_JusticeProtectors == null)
            {
                m_JusticeProtectors = new List<Mobile>();
            }

            if (m_BOBFilter == null)
            {
                m_BOBFilter = new BOBFilter();
            }

            if (m_GuildRank == null)
            {
                m_GuildRank = RankDefinition.Member;
                //Default to member if going from older version to new version (only time it should be null)
            }

            if (m_LastOnline == DateTime.MinValue && Account != null)
            {
                m_LastOnline = ((Account)Account).LastLogin;
            }

            var list = Stabled;

            for (int i = 0; i < list.Count; ++i)
            {
                BaseCreature bc = list[i] as BaseCreature;

                if (bc != null)
                {
                    bc.IsStabled = true;
                    bc.StabledBy = this;
                }
            }

            if (Hidden) //Hiding is the only buff where it has an effect that's serialized.
            {
                this.AddBuff(new BuffInfo(BuffIcon.HidingAndOrStealth, 1075655));
            }

            if (m_TieredQuestInfo == null)
            {
                m_TieredQuestInfo = new TieredQuestInfo();
            }

            if (m_LoyaltyInfo == null)
            {
                m_LoyaltyInfo = new LoyaltyInfo();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            //cleanup our anti-macro table
            foreach (Hashtable t in m_AntiMacroTable.Values)
            {
                ArrayList remove = new ArrayList();
                foreach (CountAndTimeStamp time in t.Values)
                {
                    if (time.TimeStamp + SkillCheck.AntiMacroExpire <= DateTime.UtcNow)
                    {
                        remove.Add(time);
                    }
                }

                for (int i = 0; i < remove.Count; ++i)
                {
                    t.Remove(remove[i]);
                }
            }

            CheckKillDecay();

            //CheckAtrophies(this);

            base.Serialize(writer);

            writer.Write(49); // version old 52

            // Version 53
            writer.WriteDeltaTime(LastTierLoss);
            writer.WriteDeltaTime(LastChampionTierLoss);
            writer.WriteDeltaTime(LastSuperChampionTierLoss);

            writer.Write((int)SuperChampionTiers.Length);

            for (int i = 0; i < SuperChampionTiers.Length; i++)
            {
                writer.Write((int)SuperChampionTiers[i]);
            }

            writer.Write((int)ChampionTiers.Length);

            for (int i = 0; i < ChampionTiers.Length; i++)
            {
                writer.Write((double)ChampionTiers[i]);
            }

            /* Version 52 */
            writer.Write((DateTime)m_NextGemOfSalvationUse);

            /* Version 51 */
            writer.Write(NextDrinkMaskOfDeathPotion);

            /* Version 50 */
            writer.Write((DateTime)m_LastForgedPardonUse);

            /*Version 49*/
            writer.Write((int)ACState);

            /* Version 48 */
            writer.Write((DateTime)m_NextTenthAnniversarySculptureUse);

            /* Version 47 */
            writer.Write((int)m_LuckMod);

            /* Version 46 */
            LoyaltyInfo.Serialize(writer, m_LoyaltyInfo);

            /* Version 45 */
            writer.Write((DateTime)m_NextLuckyCoinWish);

            /* Version 44 */
            writer.Write((int)m_HumilityQuestStatus);
            writer.Write((DateTime)m_HumilityQuestNextChance);

            /* Version 43 */
            writer.Write((int)m_CurrentCollectionTitle);
            writer.Write((int)m_CollectionTitles.Count);

            for (int i = 0; i < m_CollectionTitles.Count; i++)
                writer.Write((int)m_CollectionTitles[i]);

            /* Version 42 */
            bool hasAnyBardMastery = (m_BardMastery != null);
            writer.Write((bool)hasAnyBardMastery);

            if (hasAnyBardMastery)
            {
                writer.WriteEncodedInt((int)m_BardMastery.Id);
                writer.WriteEncodedInt((int)m_BardElementDamage);
                writer.Write((DateTime)m_NextBardMasterySwitch);
                writer.WriteEncodedInt((int)m_BardMasteryLearnedMask);
            }

            /* Version 41 */
            writer.Write((DateTime)m_NextDrinkExplodingTarPotion);

            /* Version 40 */
            writer.Write((int)Deaths);

            /* Version 39 */
            writer.Write((int)m_FloorTrapsPlaced);

            /* Version 38*/
            TieredQuestInfo.Serialize(writer, m_TieredQuestInfo);

            /* Version 37 */
            writer.Write((byte)m_KRStartingQuestStep);

            /* Version 36 */
            writer.Write((DateTime)m_SacredQuestNextChance);

            /* Version 35 */
            writer.Write((DateTime)m_NextPuzzleAttempt);

            //version 34
            if (m_StuckMenuUses != null)
            {
                writer.Write(true);

                writer.Write(m_StuckMenuUses.Length);

                for (int i = 0; i < m_StuckMenuUses.Length; ++i)
                {
                    writer.Write(m_StuckMenuUses[i]);
                }
            }
            else
            {
                writer.Write(false);
            }
            //version 33
            writer.Write(NextTailorBulkOrder74);
            //version 32
            writer.Write(NextSmithBulkOrder74);

            //version 31
            writer.Write((DateTime)m_PromoGiftLast);

            //version 30
            writer.Write((DateTime)m_LastTimePaged);

            // Version 29
            writer.Write((int)m_DoomCredits);

            writer.Write(m_SSNextSeed);
            writer.Write(m_SSSeedExpire);
            writer.Write(m_SSSeedLocation);
            writer.Write(m_SSSeedMap);

            writer.Write(m_ShamePoints);

            writer.Write(m_VASTotalMonsterFame);

            QuestWriter.Quests(writer, m_Quests);
            QuestWriter.Chains(writer, m_Chains);

            // Version 28
            writer.Write(m_PeacedUntil);
            writer.Write(m_AnkhNextUse);
            writer.Write(m_AutoStabled, true);

            if (m_AcquiredRecipes == null)
            {
                writer.Write(0);
            }
            else
            {
                writer.Write(m_AcquiredRecipes.Count);

                foreach (var kvp in m_AcquiredRecipes)
                {
                    writer.Write(kvp.Key);
                    writer.Write(kvp.Value);
                }
            }

            writer.WriteDeltaTime(m_LastHonorLoss);

            writer.Write(m_LastValorLoss);
            writer.WriteEncodedInt(m_ToTItemsTurnedIn);
            writer.Write(m_ToTTotalMonsterFame); //This ain't going to be a small #.

            writer.WriteEncodedInt(m_AllianceMessageHue);
            writer.WriteEncodedInt(m_GuildMessageHue);

            writer.WriteEncodedInt(m_GuildRank.Rank);
            writer.Write(m_LastOnline);

            writer.WriteEncodedInt((int)m_SolenFriendship);

            QuestSerializer.Serialize(m_Quest, writer);

            if (m_DoneQuests == null)
            {
                writer.WriteEncodedInt(0);
            }
            else
            {
                writer.WriteEncodedInt(m_DoneQuests.Count);

                for (int i = 0; i < m_DoneQuests.Count; ++i)
                {
                    QuestRestartInfo restartInfo = m_DoneQuests[i];

                    QuestSerializer.Write(restartInfo.QuestType, QuestSystem.QuestTypes, writer);
                    writer.Write(restartInfo.RestartTime);
                }
            }

            writer.WriteEncodedInt(m_Profession);

            writer.WriteDeltaTime(m_LastCompassionLoss);

            writer.WriteEncodedInt(m_CompassionGains);

            if (m_CompassionGains > 0)
            {
                writer.WriteDeltaTime(m_NextCompassionDay);
            }

            m_BOBFilter.Serialize(writer);

            bool useMods = (m_HairModID != -1 || m_BeardModID != -1);

            writer.Write(useMods);

            if (useMods)
            {
                writer.Write(m_HairModID);
                writer.Write(m_HairModHue);
                writer.Write(m_BeardModID);
                writer.Write(m_BeardModHue);
            }

            writer.Write(SavagePaintExpiration);

            writer.Write((int)m_NpcGuild);
            writer.Write(m_NpcGuildJoinTime);
            writer.Write(m_NpcGuildGameTime);

            writer.Write(m_PermaFlags, true);

            writer.Write(NextTailorBulkOrder);

            writer.Write(NextSmithBulkOrder);

            writer.WriteDeltaTime(m_LastJusticeLoss);
            writer.Write(m_JusticeProtectors, true);

            writer.WriteDeltaTime(m_LastSacrificeGain);
            writer.WriteDeltaTime(m_LastSacrificeLoss);
            writer.Write(m_AvailableResurrects);

            writer.Write((int)m_Flags);

            writer.Write(m_LongTermElapse);
            writer.Write(m_ShortTermElapse);
            writer.Write(GameTime);
        }

        public void CheckKillDecay()
        {
            if (m_ShortTermElapse < GameTime)
            {
                m_ShortTermElapse += TimeSpan.FromHours(8);
                if (ShortTermMurders > 0)
                {
                    --ShortTermMurders;
                }
            }

            if (m_LongTermElapse < GameTime)
            {
                m_LongTermElapse += TimeSpan.FromHours(40);
                if (Kills > 0)
                {
                    --Kills;
                }
            }
        }

        public void ResetKillTime()
        {
            m_ShortTermElapse = GameTime + TimeSpan.FromHours(8);
            m_LongTermElapse = GameTime + TimeSpan.FromHours(40);
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime SessionStart { get { return m_SessionStart; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan GameTime
        {
            get
            {
                if (NetState != null)
                {
                    return m_GameTime + (DateTime.UtcNow - m_SessionStart);
                }
                else
                {
                    return m_GameTime;
                }
            }
        }

        public override bool CanSee(Mobile m)
        {
            if (m is CharacterStatue)
            {
                ((CharacterStatue)m).OnRequestedAnimation(this);
            }

            if (m is PlayerMobile && ((PlayerMobile)m).m_VisList.Contains(this))
            {
                return true;
            }

            if (m_DuelContext != null && m_DuelPlayer != null && !m_DuelContext.Finished && m_DuelContext.m_Tournament != null &&
                !m_DuelPlayer.Eliminated)
            {
                Mobile owner = m;

                if (owner is BaseCreature)
                {
                    BaseCreature bc = (BaseCreature)owner;

                    Mobile master = bc.GetMaster();

                    if (master != null)
                    {
                        owner = master;
                    }
                }

                if (m.IsPlayer() && owner is PlayerMobile && ((PlayerMobile)owner).DuelContext != m_DuelContext)
                {
                    return false;
                }
            }

            return base.CanSee(m);
        }

        //testing code might have to remove later
        public virtual void CheckedAnimate(int action, int frameCount, int repeatCount, bool forward, bool repeat, int delay)
        {
            if (!Mounted)
            {
                base.Animate(action, frameCount, repeatCount, forward, repeat, delay);
            }
        }

        public override void Animate(int action, int frameCount, int repeatCount, bool forward, bool repeat, int delay)
        {
            base.Animate(action, frameCount, repeatCount, forward, repeat, delay);
        }

        public override bool CanSee(Item item)
        {
            if (m_DesignContext != null && m_DesignContext.Foundation.IsHiddenToCustomizer(item))
            {
                return false;
            }

            return base.CanSee(item);
        }

        public override bool CheckSpellCast(ISpell spell)
        {
            if (FloorTrapKit.IsAssembling(this))
                FloorTrapKit.StopAssembling(this, 1113511); // You cast a spell and cease trap assembly.

            return base.CheckSpellCast(spell);
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            Faction faction = Faction.Find(this);

            if (faction != null)
            {
                faction.RemoveMember(this);
            }

            BaseHouse.HandleDeletion(this);

            DisguiseTimers.RemoveTimer(this);

            foreach (CollectionController collection in CollectionController.WorldCollections)
            {
                if (collection.Table.Contains(this))
                    collection.Table.Remove(this);
            }
        }

        public override bool NewGuildDisplay { get { return Guilds.Guild.NewGuildSystem; } }

        public delegate void PlayerPropertiesEventHandler(PlayerPropertiesEventArgs e);

        public static event PlayerPropertiesEventHandler PlayerProperties;

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            XmlPoints a = (XmlPoints)XmlAttach.FindAttachment(this, typeof(XmlPoints));

            XmlData XmlPointsTitle = (XmlData)XmlAttach.FindAttachment(this, typeof(XmlData), "XmlPointsTitle");

            if ((XmlPointsTitle != null && XmlPointsTitle.Data == "True") || a == null)
            {
                return;
            }

            if (FeaturesConfig.XmlPointsEnabled)
                list.Add(1070722, "Kills {0} / Deaths {1} : Rank={2}", a.Kills, a.Deaths, a.Rank);
        }

        public class PlayerPropertiesEventArgs : EventArgs
        {
            public PlayerMobile Player = null;
            public ObjectPropertyList PropertyList = null;

            public PlayerPropertiesEventArgs(PlayerMobile player, ObjectPropertyList list)
            {
                Player = player;
                PropertyList = list;
            }
        }

        string m_Country;
        bool m_TriedToGetCountry;

        public string Country
        {
            get
            {
                if (!m_TriedToGetCountry && m_Country == null)
                {
                    m_Country = GOW.Utilities.Country.GetNameFromCode(Language);
                    m_TriedToGetCountry = true;
                }

                return m_Country;
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_CurrentCollectionTitle > 0)
                list.Add((int)m_CollectionTitles[m_CurrentCollectionTitle]);

            if (TestCenterConfig.TestCenterEnabled)
            {
                if (AccessLevel == AccessLevel.Player)
                {
                    list.Add(1060847, String.Format("Kills: {0}	/ Deaths: {1}", this.Kills, this.Deaths));
                    list.Add(1060415, AosAttributes.GetValue(this, AosAttribute.AttackChance).ToString());
                    list.Add(1060408, AosAttributes.GetValue(this, AosAttribute.DefendChance).ToString());
                    list.Add(1060486, AosAttributes.GetValue(this, AosAttribute.WeaponSpeed).ToString()); // TODO: Show it in %
                    list.Add(1060401, AosAttributes.GetValue(this, AosAttribute.WeaponDamage).ToString());
                    list.Add(1060433, AosAttributes.GetValue(this, AosAttribute.LowerManaCost).ToString());
                }
            }

            if (FeaturesConfig.FeatCountryLocationEnabled)
            {
                if (Country != null)
                {
                    list.Add(1060658, "{0}\t{1}", "From", Country);
                }
            }

            if (Map == Faction.Facet)
            {
                PlayerState pl = PlayerState.Find(this);

                if (pl != null)
                {
                    Faction faction = pl.Faction;

                    if (faction.Commander == this)
                    {
                        list.Add(1042733, faction.Definition.PropName); // Commanding Lord of the ~1_FACTION_NAME~
                    }
                    else if (pl.Sheriff != null)
                    {
                        list.Add(1042734, "{0}\t{1}", pl.Sheriff.Definition.FriendlyName, faction.Definition.PropName);
                        // The Sheriff of  ~1_CITY~, ~2_FACTION_NAME~
                    }
                    else if (pl.Finance != null)
                    {
                        list.Add(1042735, "{0}\t{1}", pl.Finance.Definition.FriendlyName, faction.Definition.PropName);
                        // The Finance Minister of ~1_CITY~, ~2_FACTION_NAME~
                    }
                    else if (pl.MerchantTitle != MerchantTitle.None)
                    {
                        list.Add(1060776, "{0}\t{1}", MerchantTitles.GetInfo(pl.MerchantTitle).Title, faction.Definition.PropName);
                        // ~1_val~, ~2_val~
                    }
                    else
                    {
                        list.Add(1060776, "{0}\t{1}", pl.Rank.Title, faction.Definition.PropName); // ~1_val~, ~2_val~
                    }
                }
            }

            if (Core.ML)
            {
                for (int i = AllFollowers.Count - 1; i >= 0; i--)
                {
                    BaseCreature c = AllFollowers[i] as BaseCreature;

                    if (c != null && c.ControlOrder == OrderType.Guard)
                    {
                        list.Add(501129); // guarded
                        break;
                    }
                }
            }

            if (AccessLevel > AccessLevel.Player)
            {
                string color = "";
                switch (AccessLevel)
                {
                    case AccessLevel.VIP:
                        color = "#1EFF00";
                        break;
                    case AccessLevel.Counselor:
                        color = "#00BFFF";
                        break; //Deep Sky Blue
                    case AccessLevel.Decorator:
                        color = "#FF8000";
                        break;
                    case AccessLevel.Spawner:
                        color = "#E6CC80";
                        break;
                    case AccessLevel.GameMaster:
                        color = "#FF0000";
                        break; //Red
                    case AccessLevel.Seer:
                        color = "#00FF00";
                        break; //Green
                    case AccessLevel.Administrator:
                        color = "#0070FF";
                        break;
                    case AccessLevel.Developer:
                        color = "#A335EE";
                        break;
                    case AccessLevel.CoOwner:
                        color = "#FFD700";
                        break;
                    case AccessLevel.Owner:
                        color = "#FFD700";
                        break;
                }
                if (IsStaff())
                {
                    list.Add(
                        1060658, "{0}\t{1}", "Staff", String.Format("<BASEFONT COLOR={0}>{1}", color, GetAccessLevelName(AccessLevel)));
                }
                else
                {
                    list.Add(1060658, "VIP");
                }
            }

            if (PlayerProperties != null)
            {
                PlayerProperties(new PlayerPropertiesEventArgs(this, list));
            }
        }

        public override void OnSingleClick(Mobile from)
        {
            if (Map == Faction.Facet)
            {
                PlayerState pl = PlayerState.Find(this);

                if (pl != null)
                {
                    string text;
                    bool ascii = false;

                    Faction faction = pl.Faction;

                    if (faction.Commander == this)
                    {
                        text = String.Concat(
                            Female ? "(Commanding Lady of the " : "(Commanding Lord of the ", faction.Definition.FriendlyName, ")");
                    }
                    else if (pl.Sheriff != null)
                    {
                        text = String.Concat(
                            "(The Sheriff of ", pl.Sheriff.Definition.FriendlyName, ", ", faction.Definition.FriendlyName, ")");
                    }
                    else if (pl.Finance != null)
                    {
                        text = String.Concat(
                            "(The Finance Minister of ", pl.Finance.Definition.FriendlyName, ", ", faction.Definition.FriendlyName, ")");
                    }
                    else
                    {
                        ascii = true;

                        if (pl.MerchantTitle != MerchantTitle.None)
                        {
                            text = String.Concat(
                                "(", MerchantTitles.GetInfo(pl.MerchantTitle).Title.String, ", ", faction.Definition.FriendlyName, ")");
                        }
                        else
                        {
                            text = String.Concat("(", pl.Rank.Title.String, ", ", faction.Definition.FriendlyName, ")");
                        }
                    }

                    int hue = (Faction.Find(from) == faction ? 98 : 38);

                    PrivateOverheadMessage(MessageType.Label, hue, ascii, text, from.NetState);
                }
            }

            base.OnSingleClick(from);
        }

        protected override bool OnMove(Direction d)
        {
            if (!Core.SE)
            {
                return base.OnMove(d);
            }

            if (IsStaff())
            {
                return true;
            }

            if (Hidden && DesignContext.Find(this) == null) //Hidden & NOT customizing a house
            {
                if (!Mounted && Skills.Stealth.Value >= 25.0)
                {
                    bool running = (d & Direction.Running) != 0;

                    if (running)
                    {
                        if ((AllowedStealthSteps -= 2) <= 0)
                        {
                            RevealingAction();
                        }
                    }
                    else if (AllowedStealthSteps-- <= 0)
                    {
                        Stealth.OnUse(this);
                    }
                }
                else
                {
                    RevealingAction();
                }

                if (Map == Map.Felucca)
                {
                    int hiding = Skills[SkillName.Hiding].Fixed;
                    int stealth = Skills[SkillName.Stealth].Fixed;
                    int divisor = hiding + stealth;

                    foreach (Mobile m in this.GetMobilesInRange(4))
                    {
                        if (!IsValidPassiveDetector(m))
                            continue;

                        int tracking = m.Skills[SkillName.Tracking].Fixed;
                        int detectHidden = m.Skills[SkillName.DetectHidden].Fixed;

                        if (m.Race == Race.Elf)
                            tracking = 1000;

                        int distance = Math.Max(1, (int)this.GetDistanceToSqrt(m));

                        int chance;
                        if (divisor > 0)
                            chance = 50 * (tracking + detectHidden) / divisor;
                        else
                            chance = 100;

                        chance /= distance;

                        if (chance > Utility.Random(100))
                        {
                            PrivateOverheadMessage(MessageType.Regular, 0x3B2, 500814, this.NetState); // You have been revealed!
                            RevealingAction();

                            break;
                        }
                    }
                }
            }

            if (FloorTrapKit.IsAssembling(this))
            {
                FloorTrapKit.StopAssembling(this, 1113509); // You move away and cease trap assembly.
            }

            Spells.Ninjitsu.DeathStrike.AddStep(this);

            if (InvisibilityPotion.HasTimer(this))
            {
                InvisibilityPotion.Iterrupt(this);
            }

            if (this.Party is Server.Engines.PartySystem.Party)
            {
                Party party = this.Party as Party;
                if (party != null)
                {
                    foreach (PartyMemberInfo info in party.Members)
                    {
                        Mobile m = info.Mobile;
                        if (m.NetState != null && m.NetState.IsKRClient && m != this)
                        {
                            m.NetState.Send(new DisplayWaypoint(this.Serial, this.X, this.Y, this.Z, this.Map.MapID, 2, this.Name));
                        }
                    }
                }
            }

            return true;
        }

        private bool IsValidPassiveDetector(Mobile from)
        {
            if (from == this || from.AccessLevel > AccessLevel.Player)
            {
                return false;
            }

            if (from.Hidden || !from.Alive || !from.Player)
            {
                return false;
            }

            Party party = from.Party as Party;
            if (party != null && party.Contains(this))
            {
                return false;
            }

            Guild guild = from.Guild as Guild;
            if (guild != null)
            {
                if (guild.IsMember(this))
                {
                    return false;
                }

                Guild ourGuild = this.Guild as Guild;
                if (ourGuild != null && ourGuild.IsAlly(guild))
                {
                    return false;
                }
            }

            return true;
        }

        public bool BedrollLogout { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public override bool Paralyzed
        {
            get { return base.Paralyzed; }
            set
            {
                base.Paralyzed = value;

                if (value)
                {
                    this.AddBuff(new BuffInfo(BuffIcon.Paralyze, 1075827)); //Paralyze/You are frozen and can not move
                }
                else
                {
                    this.RemoveBuff(BuffIcon.Paralyze);
                }
            }
        }

        #region Mysticism AI
        [CommandProperty(AccessLevel.GameMaster)]
        public override bool Asleep
        {
            get { return base.Asleep; }
            set
            {
                base.Asleep = value;

                if (value)
                {
                    this.AddBuff(new BuffInfo(BuffIcon.Sleep, 1080139));
                }
                else
                {
                    this.RemoveBuff(BuffIcon.Sleep);
                }
            }
        }
        #endregion

        #region Ethics
        private Player m_EthicPlayer;

        [CommandProperty(AccessLevel.GameMaster)]
        public Player EthicPlayer { get { return m_EthicPlayer; } set { m_EthicPlayer = value; } }
        #endregion

        #region Factions
        public PlayerState FactionPlayerState { get; set; }
        #endregion

        #region Dueling
        private DuelContext m_DuelContext;
        private DuelPlayer m_DuelPlayer;

        public DuelContext DuelContext { get { return m_DuelContext; } }

        public DuelPlayer DuelPlayer
        {
            get { return m_DuelPlayer; }
            set
            {
                bool wasInTourny = (m_DuelContext != null && !m_DuelContext.Finished && m_DuelContext.m_Tournament != null);

                m_DuelPlayer = value;

                if (m_DuelPlayer == null)
                {
                    m_DuelContext = null;
                }
                else
                {
                    m_DuelContext = m_DuelPlayer.Participant.Context;
                }

                bool isInTourny = (m_DuelContext != null && !m_DuelContext.Finished && m_DuelContext.m_Tournament != null);

                if (wasInTourny != isInTourny)
                {
                    SendEverything();
                }
            }
        }
        #endregion

        #region Quests
        private QuestSystem m_Quest;
        private List<QuestRestartInfo> m_DoneQuests;
        private SolenFriendship m_SolenFriendship;

        public QuestSystem Quest { get { return m_Quest; } set { m_Quest = value; } }

        public List<QuestRestartInfo> DoneQuests { get { return m_DoneQuests; } set { m_DoneQuests = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public SolenFriendship SolenFriendship { get { return m_SolenFriendship; } set { m_SolenFriendship = value; } }
        #endregion

        #region Mondain's Legacy
        private List<BaseQuest> m_Quests;
        private Dictionary<QuestChain, BaseChain> m_Chains;

        public List<BaseQuest> Quests { get { return m_Quests; } }

        public Dictionary<QuestChain, BaseChain> Chains { get { return m_Chains; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Peaced
        {
            get
            {
                if (m_PeacedUntil > DateTime.UtcNow)
                {
                    return true;
                }

                return false;
            }
        }
        #endregion

        #region MyShard Invalidation
        private bool m_ChangedMyShard;

        public bool ChangedMyShard { get { return m_ChangedMyShard; } set { m_ChangedMyShard = value; } }

        public void InvalidateMyShard()
        {
            if (!Deleted && !m_ChangedMyShard)
            {
                m_ChangedMyShard = true;
                MyShard.QueueMobileUpdate(this);
            }
        }

        public override void OnKillsChange(int oldValue)
        {
            if (Young && Kills > oldValue)
            {
                Account acc = Account as Account;

                if (acc != null)
                {
                    acc.RemoveYoungStatus(0);
                }
            }

            InvalidateMyShard();
        }

        public override void OnGenderChanged(bool oldFemale)
        {
            InvalidateMyShard();
        }

        public override void OnGuildChange(BaseGuild oldGuild)
        {
            InvalidateMyShard();
        }

        public override void OnGuildTitleChange(string oldTitle)
        {
            InvalidateMyShard();
        }

        public override void OnKarmaChange(int oldValue)
        {
            InvalidateMyShard();
        }

        public override void OnFameChange(int oldValue)
        {
            InvalidateMyShard();
        }

        public override void OnSkillChange(SkillName skill, double oldBase)
        {
            if (Young && SkillsTotal >= 4500)
            {
                Account acc = Account as Account;

                if (acc != null)
                {
                    acc.RemoveYoungStatus(1019036);
                    // You have successfully obtained a respectable skill level, and have outgrown your status as a young player!
                }
            }

            InvalidateMyShard();
        }

        public override void OnAccessLevelChanged(AccessLevel oldLevel)
        {
            if (IsPlayer())
            {
                IgnoreMobiles = false;
            }
            else
            {
                IgnoreMobiles = true;
            }

            InvalidateMyShard();
        }

        public override void OnRawStatChange(StatType stat, int oldValue)
        {
            InvalidateMyShard();
        }

        public override void OnDelete()
        {
            if (m_ReceivedHonorContext != null)
            {
                m_ReceivedHonorContext.Cancel();
            }
            if (m_SentHonorContext != null)
            {
                m_SentHonorContext.Cancel();
            }

            InvalidateMyShard();
        }
        #endregion

        #region Fastwalk Prevention
        private static bool FastwalkPrevention = true; // Is fastwalk prevention enabled?

        private static int FastwalkThreshold = 400; // Fastwalk prevention will become active after 0.4 seconds

        private long m_NextMovementTime;
        private bool m_HasMoved;

        public virtual bool UsesFastwalkPrevention { get { return (IsPlayer()); } }

        public override int ComputeMovementSpeed(Direction dir, bool checkTurning)
        {
            if (checkTurning && (dir & Direction.Mask) != (Direction & Direction.Mask))
            {
                return RunMount; // We are NOT actually moving (just a direction change)
            }

            Server.Spells.Necromancy.TransformationSpell.TransformContext context = TransformationSpell.GetContext(this);

            if (context != null && context.Type == typeof(ReaperFormSpell))
            {
                return WalkFoot;
            }

            bool running = ((dir & Direction.Running) != 0);

            bool onHorse = (Mount != null || Flying);

            AnimalFormContext animalContext = AnimalForm.GetContext(this);

            if (onHorse || (animalContext != null && animalContext.SpeedBoost))
            {
                return (running ? RunMount : WalkMount);
            }

            return (running ? RunFoot : WalkFoot);
        }

        public static bool MovementThrottle_Callback(NetState ns)
        {
            PlayerMobile pm = ns.Mobile as PlayerMobile;

            if (pm == null || !pm.UsesFastwalkPrevention)
            {
                return true;
            }

            if (!pm.m_HasMoved)
            {
                // has not yet moved
                pm.m_NextMovementTime = Core.TickCount;
                pm.m_HasMoved = true;
                return true;
            }

            long ts = pm.m_NextMovementTime - Core.TickCount;

            if (ts < 0)
            {
                // been a while since we've last moved
                pm.m_NextMovementTime = Core.TickCount;
                return true;
            }

            return (ts < FastwalkThreshold);
        }
        #endregion

        #region Enemy of One
        private Type m_EnemyOfOneType;

        public Type EnemyOfOneType
        {
            get { return m_EnemyOfOneType; }
            set
            {
                Type oldType = m_EnemyOfOneType;
                Type newType = value;

                if (oldType == newType)
                {
                    return;
                }

                m_EnemyOfOneType = value;

                DeltaEnemies(oldType, newType);
            }
        }

        public bool WaitingForEnemy { get; set; }

        private void DeltaEnemies(Type oldType, Type newType)
        {
            foreach (Mobile m in GetMobilesInRange(18))
            {
                Type t = m.GetType();

                if (t == oldType || t == newType)
                {
                    NetState ns = NetState;

                    if (ns != null)
                    {
                        if (ns.StygianAbyss)
                        {
                            ns.Send(new MobileMoving(m, Notoriety.Compute(this, m)));
                        }
                        else
                        {
                            ns.Send(new MobileMovingOld(m, Notoriety.Compute(this, m)));
                        }
                    }
                }
            }
        }
        #endregion

        #region Hair and beard mods
        private int m_HairModID = -1, m_HairModHue;
        private int m_BeardModID = -1, m_BeardModHue;
        private int m_FaceModID = -1, m_FaceModHue;

        public void SetHairMods(int hairID, int beardID)
        {
            if (hairID == -1)
            {
                InternalRestoreHair(true, ref m_HairModID, ref m_HairModHue);
            }
            else if (hairID != -2)
            {
                InternalChangeHair(true, hairID, ref m_HairModID, ref m_HairModHue);
            }

            if (beardID == -1)
            {
                InternalRestoreHair(false, ref m_BeardModID, ref m_BeardModHue);
            }
            else if (beardID != -2)
            {
                InternalChangeHair(false, beardID, ref m_BeardModID, ref m_BeardModHue);
            }
        }

        private void CreateHair(bool hair, int id, int hue)
        {
            if (hair)
            {
                //TODO Verification?
                HairItemID = id;
                HairHue = hue;
            }
            else
            {
                FacialHairItemID = id;
                FacialHairHue = hue;
            }
        }

        private void InternalRestoreHair(bool hair, ref int id, ref int hue)
        {
            if (id == -1)
            {
                return;
            }

            if (hair)
            {
                HairItemID = 0;
            }
            else
            {
                FacialHairItemID = 0;
            }

            //if( id != 0 )
            CreateHair(hair, id, hue);

            id = -1;
            hue = 0;
        }

        private void InternalChangeHair(bool hair, int id, ref int storeID, ref int storeHue)
        {
            if (storeID == -1)
            {
                storeID = hair ? HairItemID : FacialHairItemID;
                storeHue = hair ? HairHue : FacialHairHue;
            }
            CreateHair(hair, id, 0);
        }
        #endregion

        #region Virtues
        private DateTime m_LastSacrificeGain;
        private DateTime m_LastSacrificeLoss;
        private int m_AvailableResurrects;

        public DateTime LastSacrificeGain { get { return m_LastSacrificeGain; } set { m_LastSacrificeGain = value; } }
        public DateTime LastSacrificeLoss { get { return m_LastSacrificeLoss; } set { m_LastSacrificeLoss = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AvailableResurrects { get { return m_AvailableResurrects; } set { m_AvailableResurrects = value; } }

        private DateTime m_NextJustAward;
        private DateTime m_LastJusticeLoss;
        private List<Mobile> m_JusticeProtectors;

        public DateTime LastJusticeLoss { get { return m_LastJusticeLoss; } set { m_LastJusticeLoss = value; } }
        public List<Mobile> JusticeProtectors { get { return m_JusticeProtectors; } set { m_JusticeProtectors = value; } }

        private DateTime m_LastCompassionLoss;
        private DateTime m_NextCompassionDay;
        private int m_CompassionGains;

        public DateTime LastCompassionLoss { get { return m_LastCompassionLoss; } set { m_LastCompassionLoss = value; } }
        public DateTime NextCompassionDay { get { return m_NextCompassionDay; } set { m_NextCompassionDay = value; } }
        public int CompassionGains { get { return m_CompassionGains; } set { m_CompassionGains = value; } }

        private DateTime m_LastValorLoss;

        public DateTime LastValorLoss { get { return m_LastValorLoss; } set { m_LastValorLoss = value; } }

        private DateTime m_LastHonorLoss;
        private HonorContext m_ReceivedHonorContext;
        private HonorContext m_SentHonorContext;
        public DateTime m_hontime;

        public DateTime LastHonorLoss { get { return m_LastHonorLoss; } set { m_LastHonorLoss = value; } }

        public DateTime LastHonorUse { get; set; }

        public bool HonorActive { get; set; }

        public HonorContext ReceivedHonorContext { get { return m_ReceivedHonorContext; } set { m_ReceivedHonorContext = value; } }
        public HonorContext SentHonorContext { get { return m_SentHonorContext; } set { m_SentHonorContext = value; } }
        #endregion

        #region Young system
        [CommandProperty(AccessLevel.GameMaster)]
        public bool Young
        {
            get { return GetFlag(PlayerFlag.Young); }
            set
            {
                SetFlag(PlayerFlag.Young, value);
                InvalidateProperties();
            }
        }

        public override string ApplyNameSuffix(string suffix)
        {
            if (Young)
            {
                if (suffix.Length == 0)
                {
                    suffix = "(Young)";
                }
                else
                {
                    suffix = String.Concat(suffix, " (Young)");
                }
            }

            #region Ethics
            if (m_EthicPlayer != null)
            {
                if (suffix.Length == 0)
                {
                    suffix = m_EthicPlayer.Ethic.Definition.Adjunct.String;
                }
                else
                {
                    suffix = String.Concat(suffix, " ", m_EthicPlayer.Ethic.Definition.Adjunct.String);
                }
            }
            #endregion

            if (Core.ML && Map == Faction.Facet)
            {
                Faction faction = Faction.Find(this);

                if (faction != null)
                {
                    string adjunct = String.Format("[{0}]", faction.Definition.Abbreviation);
                    if (suffix.Length == 0)
                    {
                        suffix = adjunct;
                    }
                    else
                    {
                        suffix = String.Concat(suffix, " ", adjunct);
                    }
                }
            }

            return base.ApplyNameSuffix(suffix);
        }

        public override TimeSpan GetLogoutDelay()
        {
            if (Young || BedrollLogout || TestCenterConfig.TestCenterEnabled)
            {
                return TimeSpan.Zero;
            }

            return base.GetLogoutDelay();
        }

        private DateTime m_LastYoungMessage = DateTime.MinValue;

        public bool CheckYoungProtection(Mobile from)
        {
            if (!Young)
            {
                return false;
            }

            if (Region is BaseRegion && !((BaseRegion)Region).YoungProtected)
            {
                return false;
            }

            if (from is BaseCreature && ((BaseCreature)from).IgnoreYoungProtection)
            {
                return false;
            }

            if (Quest != null && Quest.IgnoreYoungProtection(from))
            {
                return false;
            }

            if (m_KRStartingQuestStep > 0)
            {
                return false;
            }

            if (DateTime.UtcNow - m_LastYoungMessage > TimeSpan.FromMinutes(1.0))
            {
                m_LastYoungMessage = DateTime.UtcNow;
                SendLocalizedMessage(1019067);
                // A monster looks at you menacingly but does not attack.  You would be under attack now if not for your status as a new citizen of Britannia.
            }

            return true;
        }

        private DateTime m_LastYoungHeal = DateTime.MinValue;

        public bool CheckYoungHealTime()
        {
            if (DateTime.UtcNow - m_LastYoungHeal > TimeSpan.FromMinutes(5.0))
            {
                m_LastYoungHeal = DateTime.UtcNow;
                return true;
            }

            return false;
        }

        private static readonly Point3D[] m_TrammelDeathDestinations = new[]
		{
			new Point3D(1481, 1612, 20), new Point3D(2708, 2153, 0), new Point3D(2249, 1230, 0), new Point3D(5197, 3994, 37),
			new Point3D(1412, 3793, 0), new Point3D(3688, 2232, 20), new Point3D(2578, 604, 0), new Point3D(4397, 1089, 0),
			new Point3D(5741, 3218, -2), new Point3D(2996, 3441, 15), new Point3D(624, 2225, 0), new Point3D(1916, 2814, 0),
			new Point3D(2929, 854, 0), new Point3D(545, 967, 0), new Point3D(3465, 2559, 35)
		};

        private static readonly Point3D[] m_IlshenarDeathDestinations = new[]
		{
			new Point3D(1216, 468, -13), new Point3D(723, 1367, -60), new Point3D(745, 725, -28), new Point3D(281, 1017, 0),
			new Point3D(986, 1011, -32), new Point3D(1175, 1287, -30), new Point3D(1533, 1341, -3), new Point3D(529, 217, -44),
			new Point3D(1722, 219, 96)
		};

        private static readonly Point3D[] m_MalasDeathDestinations = new[] { new Point3D(2079, 1376, -70), new Point3D(944, 519, -71) };

        private static readonly Point3D[] m_TokunoDeathDestinations = new[] { new Point3D(1166, 801, 27), new Point3D(782, 1228, 25), new Point3D(268, 624, 15) };

        public bool YoungDeathTeleport()
        {
            if (Region.IsPartOf(typeof(Jail)) || Region.IsPartOf("Samurai start location") ||
                Region.IsPartOf("Ninja start location") || Region.IsPartOf("Ninja cave"))
            {
                return false;
            }

            Point3D loc;
            Map map;

            DungeonRegion dungeon = (DungeonRegion)Region.GetRegion(typeof(DungeonRegion));
            if (dungeon != null && dungeon.EntranceLocation != Point3D.Zero)
            {
                loc = dungeon.EntranceLocation;
                map = dungeon.EntranceMap;
            }
            else
            {
                loc = Location;
                map = Map;
            }

            Point3D[] list;

            if (map == Map.Trammel)
            {
                list = m_TrammelDeathDestinations;
            }
            else if (map == Map.Ilshenar)
            {
                list = m_IlshenarDeathDestinations;
            }
            else if (map == Map.Malas)
            {
                list = m_MalasDeathDestinations;
            }
            else if (map == Map.Tokuno)
            {
                list = m_TokunoDeathDestinations;
            }
            else
            {
                return false;
            }

            Point3D dest = Point3D.Zero;
            int sqDistance = int.MaxValue;

            for (int i = 0; i < list.Length; i++)
            {
                Point3D curDest = list[i];

                int width = loc.X - curDest.X;
                int height = loc.Y - curDest.Y;
                int curSqDistance = width * width + height * height;

                if (curSqDistance < sqDistance)
                {
                    dest = curDest;
                    sqDistance = curSqDistance;
                }
            }

            MoveToWorld(dest, map);
            return true;
        }

        private void SendYoungDeathNotice()
        {
            SendGump(new YoungDeathNotice());
        }
        #endregion

        #region Speech log
        private SpeechLog m_SpeechLog;

        public SpeechLog SpeechLog { get { return m_SpeechLog; } }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (SpeechLog.Enabled && NetState != null)
            {
                if (m_SpeechLog == null)
                {
                    m_SpeechLog = new SpeechLog();
                }

                m_SpeechLog.Add(e.Mobile, e.Speech);
                GOW.Commands.History.Refresh(this);
            }
        }
        #endregion

        #region Champion Titles
        [CommandProperty(AccessLevel.GameMaster)]
        public bool DisplayChampionTitle { get { return GetFlag(PlayerFlag.DisplayChampionTitle); } set { SetFlag(PlayerFlag.DisplayChampionTitle, value); } }

        private void ToggleChampionTitleDisplay()
        {
            if (!CheckAlive())
            {
                return;
            }

            if (DisplayChampionTitle)
            {
                SendLocalizedMessage(1062419, "", 0x23); // You have chosen to hide your monster kill title.
                DisplayChampionTitle = false;
            }
            else
            {
                SendLocalizedMessage(1062418, "", 0x23); // You have chosen to display your monster kill title.
                DisplayChampionTitle = true;
            }
        }
        #endregion

        #region Recipes
        private Dictionary<int, bool> m_AcquiredRecipes;

        public virtual bool HasRecipe(Recipe r)
        {
            if (r == null)
            {
                return false;
            }

            return HasRecipe(r.ID);
        }

        public virtual bool HasRecipe(int recipeID)
        {
            if (m_AcquiredRecipes != null && m_AcquiredRecipes.ContainsKey(recipeID))
            {
                return m_AcquiredRecipes[recipeID];
            }

            return false;
        }

        public virtual void AcquireRecipe(Recipe r)
        {
            if (r != null)
            {
                AcquireRecipe(r.ID);
            }
        }

        public virtual void AcquireRecipe(int recipeID)
        {
            if (m_AcquiredRecipes == null)
            {
                m_AcquiredRecipes = new Dictionary<int, bool>();
            }

            m_AcquiredRecipes[recipeID] = true;
        }

        public virtual void ResetRecipes()
        {
            m_AcquiredRecipes = null;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int KnownRecipes
        {
            get
            {
                if (m_AcquiredRecipes == null)
                {
                    return 0;
                }

                return m_AcquiredRecipes.Count;
            }
        }
        #endregion

        #region XML PVP Dismount Pet
        public void DismountAndStable()
        {
            BaseCreature bc = Mount as BaseCreature;

            if (Mount != null)
            {
                Mount.Rider = null;
            }

            if (bc != null)
            {
                bc.ControlTarget = null;
                bc.ControlOrder = OrderType.Stay;
                bc.Internalize();
                bc.SetControlMaster(null);
                bc.SummonMaster = null;
                bc.IsStabled = true;

                Stabled.Add(bc);
                m_AutoStabled.Add(bc);

                SendMessage("Your Mount has been Stabled !.");
            }
        }

        public void RetrivePet()
        {
            if (m_AutoStabled.Count < 1)
            {
                return;
            }

            for (int k = 0; k < m_AutoStabled.Count; ++k)
            {
                BaseCreature bc = (BaseCreature)m_AutoStabled[k];

                if (Stabled.Contains(bc))
                {
                    bc.ControlTarget = null;
                    bc.ControlOrder = OrderType.Follow;
                    bc.SetControlMaster(this);
                    bc.SummonMaster = null;

                    if (bc.Summoned)
                    {
                        bc.SummonMaster = this;
                    }

                    //bc.Loyalty = BaseCreature.MaxLoyalty; // Wonderfully happy
                    bc.Loyalty = PetLoyalty.WonderfullyHappy; // Wonderfully happy

                    bc.MoveToWorld(Location, Map);

                    bc.IsStabled = false;

                    if (m_AutoStabled.Contains(bc))
                    {
                        m_AutoStabled.Remove(bc);
                    }

                    SendMessage("Your Mount return to You !.");
                }
            }
            m_AutoStabled.Clear();
        }
        #endregion

        #region AutoStablePets
        public void AutoStablePets()
        {
            if (Core.SE && AllFollowers.Count > 0)
            {
                for (int i = m_AllFollowers.Count - 1; i >= 0; --i)
                {
                    BaseCreature pet = AllFollowers[i] as BaseCreature;

                    if (pet == null || pet.ControlMaster == null)
                    {
                        continue;
                    }

                    if (pet.Summoned)
                    {
                        if (pet.Map != Map)
                        {
                            pet.PlaySound(pet.GetAngerSound());
                            Timer.DelayCall(TimeSpan.Zero, pet.Delete);
                        }
                        continue;
                    }

                    if (pet is IMount && ((IMount)pet).Rider != null)
                    {
                        continue;
                    }

                    if ((pet is PackLlama || pet is PackHorse || pet is Beetle) &&
                        (pet.Backpack != null && pet.Backpack.Items.Count > 0))
                    {
                        continue;
                    }

                    if (pet is BaseEscortable)
                    {
                        continue;
                    }

                    pet.ControlTarget = null;
                    pet.ControlOrder = OrderType.Stay;
                    pet.Internalize();

                    pet.SetControlMaster(null);
                    pet.SummonMaster = null;

                    pet.IsStabled = true;
                    pet.StabledBy = this;

                    //pet.Loyalty = BaseCreature.MaxLoyalty; // Wonderfully happy
                    pet.Loyalty = PetLoyalty.WonderfullyHappy; // Wonderfully happy

                    Stabled.Add(pet);
                    m_AutoStabled.Add(pet);
                }
            }
        }

        public void ClaimAutoStabledPets()
        {
            if (!Core.SE || m_AutoStabled.Count <= 0)
            {
                return;
            }

            if (!Alive)
            {
                SendLocalizedMessage(1076251);
                // Your pet was unable to join you while you are a ghost.  Please re-login once you have ressurected to claim your pets.
                return;
            }

            for (int i = m_AutoStabled.Count - 1; i >= 0; --i)
            {
                BaseCreature pet = m_AutoStabled[i] as BaseCreature;

                if (pet == null || pet.Deleted)
                {
                    pet.IsStabled = false;
                    pet.StabledBy = null;

                    if (Stabled.Contains(pet))
                    {
                        Stabled.Remove(pet);
                    }

                    continue;
                }

                if ((Followers + pet.ControlSlots) <= FollowersMax)
                {
                    pet.SetControlMaster(this);

                    if (pet.Summoned)
                    {
                        pet.SummonMaster = this;
                    }

                    pet.ControlTarget = this;
                    pet.ControlOrder = OrderType.Follow;

                    pet.MoveToWorld(Location, Map);

                    pet.IsStabled = false;
                    pet.StabledBy = null;

                    //pet.Loyalty = BaseCreature.MaxLoyalty; // Wonderfully Happy
                    pet.Loyalty = PetLoyalty.WonderfullyHappy; // Wonderfully happy

                    if (Stabled.Contains(pet))
                    {
                        Stabled.Remove(pet);
                    }
                }
                else
                {
                    SendLocalizedMessage(1049612, pet.Name); // ~1_NAME~ remained in the stables because you have too many followers.
                }
            }

            m_AutoStabled.Clear();
        }
        #endregion
    }
}