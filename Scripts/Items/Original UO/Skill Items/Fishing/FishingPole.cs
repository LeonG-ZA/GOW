using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Engines.Harvest;
using Server.Network;

namespace Server.Items
{
    public class FishingPole : Item
    {
        private AosAttributes m_Attributes;

        [CommandProperty(AccessLevel.GameMaster)]
        public AosAttributes Attributes
        {
            get { return m_Attributes; }
            set { }
        }

        private int m_Charges;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Charges
        {
            get
            {
                return m_Charges;
            }
            set
            {
                m_Charges = value;
                InvalidateProperties();
            }
        }

        public virtual bool CanBeWornByGargoyles { get { return false; } }

        [Constructable]
        public FishingPole()
            : base(0x0DC0)
        {
            Baited = false;
            BaitedMob = null;
            Charges = 0;
            Layer = Layer.TwoHanded;
            Weight = 8.0;

            m_Attributes = new AosAttributes(this);
        }

        public FishingPole(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            Point3D loc = this.GetWorldLocation();
            Map map = this.Map;

            if (!from.InLOS(loc) || !from.InRange(loc, 2))
                from.LocalOverheadMessage(MessageType.Regular, 0x3E9, 1019045); // I can't reach that
            else
            {
                Fishing.System.BeginHarvesting(from, this);

                // Charges Baiting
                if (Baited == true)
                {
                    Charges--;
                    if (Charges <= 0)
                    {
                        from.SendMessage("Your Baited Fishing Pole effect has passed, and the Fishing Pole has been turned into a normal fishing pole.");
                        Baited = false;
                        BaitedMob = null;
                        Hue = 0;
                    }
                }
                else
                    return;
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            BaseHarvestTool.AddContextMenuEntries(from, this, list, Fishing.System);
        }

        public override bool CheckConflictingLayer(Mobile m, Item item, Layer layer)
        {
            if (base.CheckConflictingLayer(m, item, layer))
                return true;

            if (layer == Layer.OneHanded)
            {
                m.SendLocalizedMessage(500214); // You already have something in both hands.
                return true;
            }

            return false;
        }

        public override bool AllowEquipedCast(Mobile from)
        {
            if (base.AllowEquipedCast(from))
                return true;

            return (m_Attributes.SpellChanneling != 0);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            int prop;

            if ((prop = Attributes.WeaponDamage) != 0)
                list.Add(1060401, prop.ToString()); // damage increase ~1_val~%

            if ((prop = Attributes.DefendChance) != 0)
                list.Add(1060408, prop.ToString()); // defense chance increase ~1_val~%

            if ((prop = Attributes.BonusDex) != 0)
                list.Add(1060409, prop.ToString()); // dexterity bonus ~1_val~

            if ((prop = Attributes.EnhancePotions) != 0)
                list.Add(1060411, prop.ToString()); // enhance potions ~1_val~%

            if ((prop = Attributes.CastRecovery) != 0)
                list.Add(1060412, prop.ToString()); // faster cast recovery ~1_val~

            if ((prop = Attributes.CastSpeed) != 0)
                list.Add(1060413, prop.ToString()); // faster casting ~1_val~

            if ((prop = Attributes.AttackChance) != 0)
                list.Add(1060415, prop.ToString()); // hit chance increase ~1_val~%

            if ((prop = Attributes.BonusHits) != 0)
                list.Add(1060431, prop.ToString()); // hit point increase ~1_val~

            if ((prop = Attributes.BonusInt) != 0)
                list.Add(1060432, prop.ToString()); // intelligence bonus ~1_val~

            if ((prop = Attributes.LowerManaCost) != 0)
                list.Add(1060433, prop.ToString()); // lower mana cost ~1_val~%

            if ((prop = Attributes.LowerRegCost) != 0)
                list.Add(1060434, prop.ToString()); // lower reagent cost ~1_val~%

            //if ((prop = Attributes.LowerAmmoCost) != 0)
                //list.Add(1075208, prop.ToString()); // Lower Ammo Cost ~1_Percentage~%

            if ((prop = Attributes.Luck) != 0)
                list.Add(1060436, prop.ToString()); // luck ~1_val~

            if ((prop = Attributes.BonusMana) != 0)
                list.Add(1060439, prop.ToString()); // mana increase ~1_val~

            if ((prop = Attributes.RegenMana) != 0)
                list.Add(1060440, prop.ToString()); // mana regeneration ~1_val~

            if ((prop = Attributes.NightSight) != 0)
                list.Add(1060441); // night sight

            if ((prop = Attributes.ReflectPhysical) != 0)
                list.Add(1060442, prop.ToString()); // reflect physical damage ~1_val~%

            if ((prop = Attributes.RegenStam) != 0)
                list.Add(1060443, prop.ToString()); // stamina regeneration ~1_val~

            if ((prop = Attributes.RegenHits) != 0)
                list.Add(1060444, prop.ToString()); // hit point regeneration ~1_val~

            if ((prop = Attributes.SpellChanneling) != 0)
                list.Add(1060482); // spell channeling

            if ((prop = Attributes.SpellDamage) != 0)
                list.Add(1060483, prop.ToString()); // spell damage increase ~1_val~%

            if ((prop = Attributes.BonusStam) != 0)
                list.Add(1060484, prop.ToString()); // stamina increase ~1_val~

            if ((prop = Attributes.BonusStr) != 0)
                list.Add(1060485, prop.ToString()); // strength bonus ~1_val~

            if ((prop = Attributes.WeaponSpeed) != 0)
                list.Add(1060486, prop.ToString()); // swing speed increase ~1_val~%
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2); // version

            writer.Write((Charges));
            m_Attributes.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        if (version < 1 && this.Layer == Layer.OneHanded)
                            this.Layer = Layer.TwoHanded;

                        Charges = reader.ReadInt();
                        goto case 1;
                    }
                case 1:
                    {
                        m_Attributes = new AosAttributes(this, reader);

                        goto case 0;
                    }
                case 0:
                    {
                        if (version == 0)
                            m_Attributes = new AosAttributes(this);

                        break;
                    }
            }
        }
    }
}