using Server.ContextMenus;
using Server.Engines.Craft;
using Server.Multis;
using System;
using System.Collections.Generic;

namespace Server.Items
{

    public class SpecialVeteranAddonComponent : AddonComponent
    {
        private SpecialVeteranCraftAddon m_Addon;

        [Constructable]
        public SpecialVeteranAddonComponent(int itemID)
            : base(itemID)
        {
            m_Addon = Addon as SpecialVeteranCraftAddon;
        }

        public SpecialVeteranAddonComponent(Serial serial)
            : base(serial)
        {
            m_Addon = Addon as SpecialVeteranCraftAddon;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            if (SpAddon == null)
            {
                m_Addon = Addon as SpecialVeteranCraftAddon;
            }
            if (SpAddon.ShowSecurity)
            {
                list.Add(SpAddon.Level.ToString());
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            SetSecureLevelEntry.AddTo(from, Addon, list);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(this, 2))
            {
                from.SendLocalizedMessage(501975);
            }
            else if (!from.CanSee(this))
            {
                from.SendLocalizedMessage(501977);
            }
            else
            {
                if (SpAddon.IsAccessibleTo(from))
                {
                    CraftSystem system = SpAddon.CraftSystem;

                    CraftContext context = system.GetContext(from);
                    from.SendGump(new CraftGump(from, system, Addon as IUsesRemaining, null));
                }
            }
        }

        public override int LabelNumber
        {
            get
            {
                return 1123577;
            }
        }// smithing press

        public override bool IsAccessibleTo(Mobile check)
        {
            if (!base.IsAccessibleTo(check))
            {
                return false;
            }

            bool accessable = false;
            BaseHouse house = BaseHouse.FindHouseAt(this);

            if (house == null)
            {
                return false;
            }

            switch (SpAddon.Level)
            {
                case SecureLevel.Owner:
                    accessable = house.IsOwner(check);
                    break;
                case SecureLevel.CoOwners:
                    accessable = house.IsCoOwner(check);
                    break;
                case SecureLevel.Friends:
                    accessable = house.IsFriend(check);
                    break;
                case SecureLevel.Guild:
                    accessable = house.IsGuildMember(check);
                    break;
                case SecureLevel.Anyone: return true;
            }

            if (!accessable)
            {
                PublicOverheadMessage(Server.Network.MessageType.Regular, 0x3E9, 1061637); // You are not allowed to access this.
            }

            return accessable;
        }

        public SpecialVeteranCraftAddon SpAddon
        {
            get
            {
                return m_Addon;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get
            {
                return SpAddon.UsesRemaining;
            }
            set
            {
                SpAddon.UsesRemaining = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ShowUsesRemaining
        {
            get
            {
                return SpAddon.ShowUsesRemaining;
            }
            set
            {
                SpAddon.ShowUsesRemaining = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ShowSecurity
        {
            get
            {
                return SpAddon.ShowSecurity;
            }
            set
            {
                SpAddon.ShowSecurity = value;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
