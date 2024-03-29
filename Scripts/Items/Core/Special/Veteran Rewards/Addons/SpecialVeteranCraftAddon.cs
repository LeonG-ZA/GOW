﻿using Server.Engines.Craft;
using Server.Gumps;
using Server.Multis;

namespace Server.Items
{
    public class SpecialVeteranCraftAddon : BaseAddon, IUsesRemaining, ISecurable
    {
        private int m_UsesRemaining = 0;
        private bool m_ShowUsesRemaining = true;
        private bool m_ShowSecurity = false;
        private int m_MaxUses = 5000;

        // [Constructable]
        public SpecialVeteranCraftAddon()
            : this(0)
        {
        }

        public SpecialVeteranCraftAddon(int uses)
            : base()
        {
            Weight = 20.0;
            Movable = false;
            UsesRemaining = uses;
        }

        public SpecialVeteranCraftAddon(Serial serial)
            : base(serial)
        {
        }

        public virtual CraftSystem CraftSystem
        {
            get
            {
                return DefBlacksmithy.CraftSystem;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((int)m_Level);
            writer.Write((int)m_UsesRemaining);
            writer.Write((bool)m_ShowUsesRemaining);
            writer.Write((bool)m_ShowSecurity);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Level = (SecureLevel)reader.ReadInt();
            m_UsesRemaining = reader.ReadInt();
            m_ShowUsesRemaining = reader.ReadBool();
            m_ShowSecurity = reader.ReadBool();
        }


        private SecureLevel m_Level;

        public SecureLevel Level
        {
            get
            {
                return this.m_Level;
            }
            set
            {
                this.m_Level = value;
                RevalidateComponents();
            }
        }

        public int MaxUses
        {
            get
            {
                return m_MaxUses;
            }
        }

        public int UsesRemaining
        {
            get
            {
                return m_UsesRemaining;
            }
            set
            {
                m_UsesRemaining = value;
                RevalidateComponents();
            }
        }

        public bool ShowUsesRemaining
        {
            get
            {
                return m_ShowUsesRemaining;
            }
            set
            {
                m_ShowUsesRemaining = value;
                RevalidateComponents();
            }
        }

        public bool ShowSecurity
        {
            get
            {
                return m_ShowSecurity;
            }
            set
            {
                m_ShowSecurity = value;
                RevalidateComponents();
            }
        }

        public void RevalidateComponents()
        {
            foreach (SpecialVeteranAddonComponent svac in Components)
            {
                svac.InvalidateProperties();
            }
        }
    }
}