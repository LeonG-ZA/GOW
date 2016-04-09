using System;
using Server.Engines.Craft;
using System.Linq;
using Server.ContextMenus;
using System.Collections.Generic;

namespace Server.Items
{
    public interface IQuiver
    {
        int PhysicalDamage { get; }
        int FireDamage { get; }
        int ColdDamage { get; }
        int PoisonDamage { get; }
        int EnergyDamage { get; }
        int ChaosDamage { get; }
        int DirectDamage { get; }

        int DamageIncrease { get; }
    }

    [Alterable(typeof(DefTailoring), typeof(GargishLeatherWingArmor), true)]
    public class BaseQuiver : Container, ICraftable, ISetItem
    {
        public virtual int PhysicalDamage { get { return 0; } }
        public virtual int FireDamage { get { return 0; } }
        public virtual int ColdDamage { get { return 0; } }
        public virtual int PoisonDamage { get { return 0; } }
        public virtual int EnergyDamage { get { return 0; } }
        public virtual int ChaosDamage { get { return 0; } }
        public virtual int DirectDamage { get { return 0; } }
        public virtual int DamageIncrease { get { return 10; } }

        public virtual int BasePhysicalResistance { get { return 0; } }
        public virtual int BaseFireResistance { get { return 0; } }
        public virtual int BaseColdResistance { get { return 0; } }
        public virtual int BasePoisonResistance { get { return 0; } }
        public virtual int BaseEnergyResistance { get { return 0; } }

        public override int PhysicalResistance { get { return BasePhysicalResistance + m_Resistances.Physical; } }
        public override int FireResistance { get { return BaseFireResistance + m_Resistances.Fire; } }
        public override int ColdResistance { get { return BaseColdResistance + m_Resistances.Cold; } }
        public override int PoisonResistance { get { return BasePoisonResistance + m_Resistances.Poison; } }
        public override int EnergyResistance { get { return BaseEnergyResistance + m_Resistances.Energy; } }

        public override int DefaultGumpID { get { return 0x108; } }
        public override int DefaultMaxItems { get { return 1; } }
        public override int DefaultMaxWeight { get { return 50; } }
        public override double DefaultWeight { get { return 2.0; } }

        public override bool CanInsureItem { get { return true; } }

        private AosAttributes m_Attributes;
        private AosSkillBonuses m_AosSkillBonuses;
        private AosElementAttributes m_Resistances;
        private int m_Capacity;
        private int m_LowerAmmoCost;
        private int m_WeightReduction;
        //private int m_DamageIncrease;
        private Mobile m_Crafter;
        private ClothingQuality m_Quality;
        private bool m_PlayerConstructed;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsArrowAmmo { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosAttributes Attributes { get { return m_Attributes; } set { } }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosSkillBonuses SkillBonuses { get { return m_AosSkillBonuses; } set { } }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosElementAttributes Resistances { get { return m_Resistances; } set { } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Capacity { get { return m_Capacity; } set { m_Capacity = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int LowerAmmoCost { get { return m_LowerAmmoCost; } set { m_LowerAmmoCost = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int WeightReduction { get { return m_WeightReduction; } set { m_WeightReduction = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Crafter { get { return m_Crafter; } set { m_Crafter = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public ClothingQuality Quality { get { return m_Quality; } set { m_Quality = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool PlayerConstructed { get { return m_PlayerConstructed; } set { m_PlayerConstructed = value; } }
		
        public Item Ammo { get { return Items.Count > 0 ? Items[0] : null; } }

        public int MaxAmmo
        {
            get { return DefaultMaxWeight * 10; }
        }

        public BaseQuiver()
            : this(0x2FB7)
        {
        }

        public BaseQuiver(int itemID)
            : base(itemID)
        {
            Weight = 2.0;
            Capacity = 500;
            Layer = Layer.Cloak;
            DropSound = 0x4F;

            m_Attributes = new AosAttributes(this);
            m_AosSkillBonuses = new AosSkillBonuses(this);
            m_Resistances = new AosElementAttributes(this);  

            m_SetAttributes = new AosAttributes(this);
            m_SetSkillBonuses = new AosSkillBonuses(this);
            IsArrowAmmo = true;
        }

        public BaseQuiver(Serial serial)
            : base(serial)
        {
        }

        public override void OnAfterDuped(Item newItem)
        {
            BaseQuiver quiver = newItem as BaseQuiver;

            if (quiver == null)
            {
                return;
            }

            quiver.m_Attributes = new AosAttributes(newItem, m_Attributes);
            quiver.m_AosSkillBonuses = new AosSkillBonuses(newItem, m_AosSkillBonuses);
            quiver.m_Resistances = new AosElementAttributes(newItem, m_Resistances); 
        }

        public override void UpdateTotal(Item sender, TotalType type, int delta)
        {
            InvalidateProperties();

            base.UpdateTotal(sender, type, delta);
        }

        public override int GetTotal(TotalType type)
        {
            int total = base.GetTotal(type);

            if (type == TotalType.Weight)
            {
                total -= total * m_WeightReduction / 100;
            }

            return total;
        }

        private static readonly Type[] m_Ammo = new Type[]
        {
            typeof(Arrow), typeof(Bolt)
        };

        public bool CheckType(Item item)
        {
            Type type = item.GetType();
            Item ammo = Ammo;

            if (ammo != null)
            {
                if (ammo.GetType() == type)
                    return true;
            }
            else
            {
                for (int i = 0; i < m_Ammo.Length; i++)
                {
                    if (type == m_Ammo[i])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
        {
            if (!CheckType(item))
            {
                if (message)
                {
                    m.SendLocalizedMessage(1074836); // The container can not hold that type of object.
                }

                return false;
            }
            Item ammo = Ammo;

            if (ammo != null && ammo.Amount > 0)
            {
                if (ammo.Amount + item.Amount > MaxAmmo)
                {
                    if (message)
                        m.SendLocalizedMessage(1080017); // That container cannot hold more items.

                    return false;
                }
                /*
                if (IsArrowAmmo && item is Bolt)
                {
                    return false;
                }
                if (!IsArrowAmmo && item is Arrow)
                {
                    return false;
                }
                 */
            }

            if (Items.Count < DefaultMaxItems)
            {
                if (item.Amount <= m_Capacity)
                {
                    return base.CheckHold(m, item, message, checkItems, plusItems, plusWeight);
                }

                return false;
            }
            else if (checkItems)
            {
                return false;
            }

            if (ammo == null || ammo.Deleted)
            {
                return false;
            }

            return false;
        }

        public override void AddItem(Item dropped)
        {
            base.AddItem(dropped);

            InvalidateWeight();
        }
		
        public override void RemoveItem(Item dropped)
        {
            base.RemoveItem(dropped);

            InvalidateWeight();
            IsArrowAmmo = dropped is Arrow;
        }

        public override void OnAdded(IEntity parent)
        {
            if (parent is Mobile)
            {
                Mobile mob = (Mobile)parent;

                m_Attributes.AddStatBonuses(mob);
                m_AosSkillBonuses.AddTo(mob); 

                BaseRanged ranged = mob.Weapon as BaseRanged;

                if (ranged != null)
                {
                    ranged.InvalidateProperties();
                }

                if (IsSetItem)
                {
                    m_SetEquipped = SetHelper.FullSetEquipped(mob, SetID, Pieces);

                    if (m_SetEquipped)
                    {
                        m_LastEquipped = true;
                        SetHelper.AddSetBonus(mob, SetID);
                    }
                }
            }
        }

        public override void OnRemoved(IEntity parent)
        {
            if (parent is Mobile)
            {
                Mobile mob = (Mobile)parent;

                m_Attributes.RemoveStatBonuses(mob);
                m_AosSkillBonuses.Remove();

                if (IsSetItem && m_SetEquipped)
                {
                    SetHelper.RemoveSetBonus(mob, SetID, this);
                }
            }
        }

        public override bool OnDragLift(Mobile from)
        {
            if (Parent is Mobile && from == Parent)
            {
                if (IsSetItem && m_SetEquipped)
                {
                    SetHelper.RemoveSetBonus(from, SetID, this);
                }
            }

            return base.OnDragLift(from);
        }

        public virtual void GetSetArmorPropertiesFirst(ObjectPropertyList list)
        {
        }

        public virtual void GetSetArmorPropertiesSecond(ObjectPropertyList list)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_Quality == ClothingQuality.Exceptional)
            {
                list.Add(1063341); // exceptional
            }

            if (m_Crafter != null)
            {
                list.Add(1050043, m_Crafter.TitleName); // crafted by ~1_NAME~
            }

            m_AosSkillBonuses.GetProperties(list);

            /*
            Item ammo = Ammo;

            if (ammo != null)
            {
                if (ammo is Arrow)
                    list.Add(1075265, "{0}\t{1}", ammo.Amount, Capacity); // Ammo: ~1_QUANTITY~/~2_CAPACITY~ arrows
                else if (ammo is Bolt)
                    list.Add(1075266, "{0}\t{1}", ammo.Amount, Capacity); // Ammo: ~1_QUANTITY~/~2_CAPACITY~ bolts
            }
            else
                list.Add(1075265, "{0}\t{1}", 0, Capacity); // Ammo: ~1_QUANTITY~/~2_CAPACITY~ arrows
             */

            int ammoamount = (Ammo != null ? Ammo.Amount : 0);

            list.Add((Ammo is Bolt ? 1075266 : 1075265), "{0}\t{1}", ammoamount.ToString(), MaxAmmo.ToString()); // Ammo: ~1_QUANTITY~/~2_CAPACITY~ arrows/bolts

            int prop;

            if ((prop = m_Attributes.DefendChance) != 0)
                list.Add(1060408, prop.ToString()); // defense chance increase ~1_val~%

            if ((prop = m_Attributes.BonusDex) != 0)
                list.Add(1060409, prop.ToString()); // dexterity bonus ~1_val~

            if ((prop = m_Attributes.EnhancePotions) != 0)
                list.Add(1060411, prop.ToString()); // enhance potions ~1_val~%

            if ((prop = m_Attributes.CastRecovery) != 0)
                list.Add(1060412, prop.ToString()); // faster cast recovery ~1_val~

            if ((prop = m_Attributes.CastSpeed) != 0)
                list.Add(1060413, prop.ToString()); // faster casting ~1_val~

            if ((prop = m_Attributes.AttackChance) != 0)
                list.Add(1060415, prop.ToString()); // hit chance increase ~1_val~%

            if ((prop = m_Attributes.BonusHits) != 0)
                list.Add(1060431, prop.ToString()); // hit point increase ~1_val~

            if ((prop = m_Attributes.BonusInt) != 0)
                list.Add(1060432, prop.ToString()); // intelligence bonus ~1_val~

            if ((prop = m_Attributes.LowerManaCost) != 0)
                list.Add(1060433, prop.ToString()); // lower mana cost ~1_val~%

            if ((prop = m_Attributes.LowerRegCost) != 0)
                list.Add(1060434, prop.ToString()); // lower reagent cost ~1_val~%	

            if ((prop = m_Attributes.Luck) != 0)
                list.Add(1060436, prop.ToString()); // luck ~1_val~

            if ((prop = m_Attributes.BonusMana) != 0)
                list.Add(1060439, prop.ToString()); // mana increase ~1_val~

            if ((prop = m_Attributes.RegenMana) != 0)
                list.Add(1060440, prop.ToString()); // mana regeneration ~1_val~

            if ((prop = m_Attributes.NightSight) != 0)
                list.Add(1060441); // night sight

            if ((prop = m_Attributes.ReflectPhysical) != 0)
                list.Add(1060442, prop.ToString()); // reflect physical damage ~1_val~%

            if ((prop = m_Attributes.RegenStam) != 0)
                list.Add(1060443, prop.ToString()); // stamina regeneration ~1_val~

            if ((prop = m_Attributes.RegenHits) != 0)
                list.Add(1060444, prop.ToString()); // hit point regeneration ~1_val~

            if ((prop = m_Attributes.SpellDamage) != 0)
                list.Add(1060483, prop.ToString()); // spell damage increase ~1_val~%

            if ((prop = m_Attributes.BonusStam) != 0)
                list.Add(1060484, prop.ToString()); // stamina increase ~1_val~

            if ((prop = m_Attributes.BonusStr) != 0)
                list.Add(1060485, prop.ToString()); // strength bonus ~1_val~

            if ((prop = m_Attributes.WeaponSpeed) != 0)
                list.Add(1060486, prop.ToString()); // swing speed increase ~1_val~%

            if ((prop = m_LowerAmmoCost) > 0)
                list.Add(1075208, prop.ToString()); // Lower Ammo Cost ~1_Percentage~%

            if (DamageIncrease != 0)
                list.Add(1074762, DamageIncrease.ToString()); // Damage modifier: ~1_PERCENT~%

            if (DirectDamage != 0)
                list.Add(1079978, DirectDamage.ToString()); // Direct Damage: ~1_PERCENT~%

            if (PhysicalDamage != 0)
                list.Add(1060403, PhysicalDamage.ToString()); // physical damage ~1_val~%

            if (FireDamage != 0)
                list.Add(1060405, FireDamage.ToString()); // fire damage ~1_val~%

            if (ColdDamage != 0)
                list.Add(1060404, ColdDamage.ToString()); // cold damage ~1_val~%

            if (PoisonDamage != 0)
                list.Add(1060406, PoisonDamage.ToString()); // poison damage ~1_val~%

            if (EnergyDamage != 0)
                list.Add(1060407, EnergyDamage.ToString()); // energy damage ~1_val~%

            if (ChaosDamage != 0)
                list.Add(1072846, ChaosDamage.ToString()); // chaos damage ~1_val~%

            GetSetArmorPropertiesFirst(list);

            if (IsSetItem)
            {
                list.Add(1073491, Pieces.ToString()); // Part of a Weapon/Armor Set (~1_val~ pieces)

                if (m_SetEquipped)
                {
                    list.Add(1073492); // Full Weapon/Armor Set Present					
                    SetHelper.GetSetProperties(list, this);
                }
            }

            base.AddResistanceProperties(list);
            
            double weight = 0;
            Item ammo = Ammo;
            if (ammo != null)
            {
                weight = Ammo.Weight * Ammo.Amount;
            }

            list.Add(1072241, "{0}\t{1}\t{2}\t{3}", Items.Count, DefaultMaxItems, (int)weight, DefaultMaxWeight); // Contents: ~1_COUNT~/~2_MAXCOUNT items, ~3_WEIGHT~/~4_MAXWEIGHT~ stones
             

            if ((prop = m_WeightReduction) != 0)
                list.Add(1072210, prop.ToString()); // Weight reduction: ~1_PERCENTAGE~%

            if (IsSetItem && !m_SetEquipped)
            {
                list.Add(1072378); // <br>Only when full set is present:		
                SetHelper.GetSetProperties(list, this);
            }

            GetSetArmorPropertiesSecond(list);
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            if (m_Crafter != null)
            {
                LabelTo(from, 1050043, m_Crafter.TitleName); // crafted by ~1_NAME~
            }
        }
		
        private static void SetSaveFlag(ref SaveFlag flags, SaveFlag toSet, bool setIf)
        {
            if (setIf)
            {
                flags |= toSet;
            }
        }

        private static bool GetSaveFlag(SaveFlag flags, SaveFlag toGet)
        {
            return ((flags & toGet) != 0);
        }

        [Flags]
        private enum SaveFlag
        {
            None = 0x00000000,
            Attributes = 0x00000001,
            DamageModifier = 0x00000002,
            LowerAmmoCost = 0x00000004,
            WeightReduction = 0x00000008,
            Crafter = 0x00000010,
            Quality = 0x00000020,
            Capacity = 0x00000040,
            SetAttributes = 0x00000100,
            SetHue = 0x00000200,
            LastEquipped = 0x00000400,
            SetEquipped = 0x00000800,
            SetSkillAttributes = 0x00002000
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (from.Items.Contains(this) || (from.Backpack != null && IsChildOf(from.Backpack)))
            {
                list.Add(new RefillQuiverEntry(this));
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(2); // version

            writer.Write(IsArrowAmmo);

            SaveFlag flags = SaveFlag.None;

            m_AosSkillBonuses.Serialize(writer);
            m_Resistances.Serialize(writer);

            SetSaveFlag(ref flags, SaveFlag.Attributes, !m_Attributes.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.LowerAmmoCost, m_LowerAmmoCost != 0);
            SetSaveFlag(ref flags, SaveFlag.WeightReduction, m_WeightReduction != 0);
            SetSaveFlag(ref flags, SaveFlag.Crafter, m_Crafter != null);
            SetSaveFlag(ref flags, SaveFlag.Quality, true);
            SetSaveFlag(ref flags, SaveFlag.Capacity, m_Capacity > 0);
            SetSaveFlag(ref flags, SaveFlag.SetAttributes, !m_SetAttributes.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.SetSkillAttributes, !m_SetSkillBonuses.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.SetHue, m_SetHue != 0);
            SetSaveFlag(ref flags, SaveFlag.LastEquipped, m_LastEquipped);
            SetSaveFlag(ref flags, SaveFlag.SetEquipped, m_SetEquipped);

            writer.WriteEncodedInt((int)flags);

            if (GetSaveFlag(flags, SaveFlag.Attributes))
            {
                m_Attributes.Serialize(writer);
            }

            if (GetSaveFlag(flags, SaveFlag.LowerAmmoCost))
            {
                writer.Write((int)m_LowerAmmoCost);
            }

            if (GetSaveFlag(flags, SaveFlag.WeightReduction))
            {
                writer.Write((int)m_WeightReduction);
            }

            if (GetSaveFlag(flags, SaveFlag.Crafter))
            {
                writer.Write((Mobile)m_Crafter);
            }

            if (GetSaveFlag(flags, SaveFlag.Quality))
            {
                writer.Write((int)m_Quality);
            }

            if (GetSaveFlag(flags, SaveFlag.Capacity))
            {
                writer.Write((int)m_Capacity);
            }

            if (GetSaveFlag(flags, SaveFlag.SetAttributes))
            {
                m_SetAttributes.Serialize(writer);
            }

            if (GetSaveFlag(flags, SaveFlag.SetSkillAttributes))
            {
                m_SetSkillBonuses.Serialize(writer);
            }

            if (GetSaveFlag(flags, SaveFlag.SetHue))
            {
                writer.Write((int)m_SetHue);
            }

            if (GetSaveFlag(flags, SaveFlag.LastEquipped))
            {
                writer.Write((bool)m_LastEquipped);
            }

            if (GetSaveFlag(flags, SaveFlag.SetEquipped))
            {
                writer.Write((bool)m_SetEquipped);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            switch (version)
            {
                case 2:
                    {
                        IsArrowAmmo = reader.ReadBool();
                        goto case 1;
                    }
                case 1:
                    {
                        if (version == 1)
                        {
                            IsArrowAmmo = (Ammo == null || Ammo is Arrow);
                        }
                        m_AosSkillBonuses = new AosSkillBonuses(this, reader);
                        m_Resistances = new AosElementAttributes(this, reader);
                        goto case 0;
                    }
                case 0:
                    {
                        if (version == 0)
                        {
                            m_AosSkillBonuses = new AosSkillBonuses(this);
                            m_Resistances = new AosElementAttributes(this);
                        }

                        SaveFlag flags = (SaveFlag)reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.Attributes))
                        {
                            m_Attributes = new AosAttributes(this, reader);
                        }
                        else
                        {
                            m_Attributes = new AosAttributes(this);
                        }

                        if (GetSaveFlag(flags, SaveFlag.LowerAmmoCost))
                        {
                            m_LowerAmmoCost = reader.ReadInt();
                        }

                        if (GetSaveFlag(flags, SaveFlag.WeightReduction))
                        {
                            m_WeightReduction = reader.ReadInt();
                        }

                        if (GetSaveFlag(flags, SaveFlag.Crafter))
                        {
                            m_Crafter = reader.ReadMobile();
                        }

                        if (GetSaveFlag(flags, SaveFlag.Quality))
                        {
                            m_Quality = (ClothingQuality)reader.ReadInt();
                        }

                        if (GetSaveFlag(flags, SaveFlag.Capacity))
                        {
                            m_Capacity = reader.ReadInt();
                        }

                        if (GetSaveFlag(flags, SaveFlag.SetAttributes))
                        {
                            m_SetAttributes = new AosAttributes(this, reader);
                        }
                        else
                        {
                            m_SetAttributes = new AosAttributes(this);
                        }

                        if (GetSaveFlag(flags, SaveFlag.SetSkillAttributes))
                        {
                            m_SetSkillBonuses = new AosSkillBonuses(this, reader);
                        }
                        else
                        {
                            m_SetSkillBonuses = new AosSkillBonuses(this);
                        }

                        if (GetSaveFlag(flags, SaveFlag.SetHue))
                        {
                            m_SetHue = reader.ReadInt();
                        }

                        if (GetSaveFlag(flags, SaveFlag.LastEquipped))
                        {
                            m_LastEquipped = reader.ReadBool();
                        }

                        if (GetSaveFlag(flags, SaveFlag.SetEquipped))
                        {
                            m_SetEquipped = reader.ReadBool();
                        }
                        break;
                    }
            }
        }

        //public virtual void AlterBowDamage(ref int phys, ref int fire, ref int cold, ref int pois, ref int nrgy, ref int chaos, ref int direct)
        //{
        //}

        public void InvalidateWeight()
        {
            if (RootParent is Mobile)
            {
                Mobile m = (Mobile)RootParent;

                m.UpdateTotals();
            }
        }
		
        #region ICraftable
        public virtual int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, IUsesRemaining tool, CraftItem craftItem, int resHue)
        //public virtual int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue)
        {
            Quality = (ClothingQuality)quality;

            if (makersMark)
                Crafter = from;

            return quality;
        }

        #endregion

        #region Mondain's Legacy Sets
        public virtual SetItem SetID
        {
            get
            {
                return SetItem.None;
            }
        }
        public virtual int Pieces
        {
            get
            {
                return 0;
            }
        }

        public bool IsSetItem
        {
            get
            {
                return SetID != SetItem.None;
            }
        }

        private int m_SetHue;
        private bool m_SetEquipped;
        private bool m_LastEquipped;

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetHue
        {
            get
            {
                return m_SetHue;
            }
            set
            {
                m_SetHue = value;
                InvalidateProperties();
            }
        }

        public bool SetEquipped
        {
            get
            {
                return m_SetEquipped;
            }
            set
            {
                m_SetEquipped = value;
            }
        }

        public bool LastEquipped
        {
            get
            {
                return m_LastEquipped;
            }
            set
            {
                m_LastEquipped = value;
            }
        }

        private AosAttributes m_SetAttributes;
        private AosSkillBonuses m_SetSkillBonuses;

        [CommandProperty(AccessLevel.GameMaster)]
        public AosAttributes SetAttributes
        {
            get
            {
                return m_SetAttributes;
            }
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosSkillBonuses SetSkillBonuses
        {
            get
            {
                return m_SetSkillBonuses;
            }
            set
            {
            }
        }
        #endregion

        public class RefillQuiverEntry : ContextMenuEntry
        {
            private readonly BaseQuiver m_quiver;

            public RefillQuiverEntry(BaseQuiver bq)
                : base(6230)
            {
                m_quiver = bq;
            }

            bool Refill<T>(Mobile m, Container c) where T : Item
            {
                List<T> list = c.FindItemsByType<T>(true).ToList();
                if (list.Count > 0)
                {
                    int amt = 0;
                    list = list.OrderByDescending(e => e.Amount).ToList();

                    int famount = m_quiver.Ammo == null ? 0 : m_quiver.Ammo.Amount;
                    if (m_quiver.Ammo != null)
                        m_quiver.Ammo.Delete();

                    while ((famount < m_quiver.Capacity) && (list.Count > 0))
                    {
                        T data = list[list.Count - 1];
                        int remaining = m_quiver.Capacity - famount;
                        if (data.Amount > remaining)
                        {
                            famount += remaining;
                            amt += remaining;
                            data.Amount -= remaining;
                        }
                        else
                        {
                            famount += data.Amount;
                            amt += data.Amount;
                            data.Delete();
                            list.RemoveAt(list.Count - 1);
                        }
                    }

                    if ((amt > 0) && (m != null))
                    {
                        T obj = (T)Activator.CreateInstance(typeof(T));
                        obj.Amount = famount;
                        m_quiver.AddItem(obj);
                        m.SendLocalizedMessage(1072664, amt.ToString());
                        return true;
                    }
                }
                return false;
            }

            public override void OnClick()
            {
                if ((m_quiver == null) || m_quiver.Deleted)
                    return;

                object owner = m_quiver.Parent;
                while (owner != null)
                {
                    if (owner is Mobile)
                        break;
                    if (owner is Item)
                    {
                        owner = ((Item)owner).Parent;
                        continue;
                    }
                    owner = null;
                }

                if (owner == null)
                    return;

                if (!(owner is Mobile))
                    return;

                Mobile m = (Mobile)owner;


                if (m.Backpack == null)
                    return;

                if (!(m.Items.Contains(m_quiver) || m_quiver.IsChildOf(m.Backpack)))
                    return;

                // Try to fill from the bank box
                if ((m.BankBox != null) && (m.BankBox.Opened))
                {
                    if (m_quiver.IsArrowAmmo ? Refill<Arrow>(m, m.BankBox) : Refill<Bolt>(m, m.BankBox))
                        return;
                }

                // Otherwise look for secure containers within two tiles
                var items = m.Map.GetItemsInRange(m.Location, 1);
                foreach (Item i in items)
                {
                    if (!(i is Container))
                        continue;

                    Container c = (Container)i;

                    if (!c.IsSecure || !c.IsAccessibleTo(m))
                        continue;

                    if (m_quiver.IsArrowAmmo ? Refill<Arrow>(m, c) : Refill<Bolt>(m, c))
                        return;
                }

                m.SendLocalizedMessage(1072673); //There are no source containers nearby.
            }
        }
    }
}