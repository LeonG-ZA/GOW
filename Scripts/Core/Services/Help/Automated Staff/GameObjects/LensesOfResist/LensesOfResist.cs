using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.ContextMenus;
using Server.Gumps;
using Server.Engines.Quests;
using Server.Engines.Quests.Necro;
using Server.Items;
using Server.Spells;
using Server.Spells.Fourth;
using Server.Targeting;

namespace Server.Items.Staff
{
    public class LensesOfResist : BaseGlasses
	{
        private Mobile m_Owner;
        public Point3D m_HomeLocation;
        public Map m_HomeMap;

        public override int BasePhysicalResistance { get { return 75; } }
        public override int BaseFireResistance { get { return 75; } }
        public override int BaseColdResistance { get { return 75; } }
        public override int BasePoisonResistance { get { return 75; } }
        public override int BaseEnergyResistance { get { return 75; } }

        [Constructable]
        public LensesOfResist(): base()
        {
            LootType = LootType.Blessed;
            Weight = 1.0;
            Hue = 2406;
            Name = "An Unassigned Pair of Lenses";

            Attributes.NightSight = 1;
        }
   
        public LensesOfResist(Serial serial): base(serial)
		{
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D HomeLocation
        {
            get { return m_HomeLocation; }
            set { m_HomeLocation = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map HomeMap
        {
            get { return m_HomeMap; }
            set { m_HomeMap = value; }
        }

        private class GoHomeEntry : ContextMenuEntry
        {
            private LensesOfResist m_Item;
            private Mobile m_Mobile;

            public GoHomeEntry(Mobile from, Item item): base(5134) // uses "Goto Loc" entry
            {
                m_Item = (LensesOfResist)item;
                m_Mobile = from;
            }

            public override void OnClick()
            {
                m_Mobile.Location = m_Item.HomeLocation;
                if (m_Item.HomeMap != null)
                    m_Mobile.Map = m_Item.HomeMap;
            }
        }

        private class SetHomeEntry : ContextMenuEntry
        {
            private LensesOfResist m_Item;
            private Mobile m_Mobile;

            public SetHomeEntry(Mobile from, Item item): base(2055) // uses "Mark" entry
            {
                m_Item = (LensesOfResist)item;
                m_Mobile = from;
            }

            public override void OnClick()
            {
                m_Item.HomeLocation = m_Mobile.Location;
                m_Item.HomeMap = m_Mobile.Map;
                m_Mobile.SendMessage("The home location on your lenses have been set to your current position.");
            }
        }

        public static void GetContextMenuEntries(Mobile from, Item item, List<ContextMenuEntry> list)
        {
            list.Add(new GoHomeEntry(from, item));
            list.Add(new SetHomeEntry(from, item));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (m_Owner == null)
            {
                return;
            }
            else
            {
                if (m_Owner != from)
                {
                    from.SendMessage("These lenses are not yours to use.");
                    return;
                }
                else
                {
                    base.GetContextMenuEntries(from, list);
                    LensesOfResist.GetContextMenuEntries(from, this, list);
                }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel > AccessLevel.Player)
            {
                if (from != null)             
                {
                    from.SendMessage("");
                }
            }

            if (m_Owner == null && from.AccessLevel == AccessLevel.Player)
            {
                from.SendMessage("Only the server administration can use this item!");
                //this.Delete();
            }

            else if (m_Owner == null && from.AccessLevel == AccessLevel.Counselor)
            {
                from.SendMessage("Only the server administration can use this item!");
                //this.Delete();
            }

            else if (m_Owner == null)
            {
                m_Owner = from;
                this.Name = m_Owner.Name.ToString() + "'s Lenses of Resist";
                this.HomeLocation = from.Location;
                this.HomeMap = from.Map;
                from.SendMessage("These lenses have been assigned to you.");
            }
            else
            {
                if (m_Owner != from)
                {
                    from.SendMessage("This item has not been assigned to you!");
                    return;
                }
            }
        }

        public override bool OnEquip(Mobile from)
        {
            if (from.AccessLevel < AccessLevel.GameMaster)
            {
                from.SendMessage("These lenses can only be used by server administrators!");
                //this.Delete();
            }
            return true;
        }
	
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );		
			writer.Write( (int) 1 ); // version

            writer.Write(m_HomeLocation);
            writer.Write(m_HomeMap);
            writer.Write(m_Owner);
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );			
			int version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    m_HomeLocation = reader.ReadPoint3D();
                    m_HomeMap = reader.ReadMap();
                    m_Owner = reader.ReadMobile();
                } goto case 0;
                case 0:
                {
                    break;
                }
            }
        }
    }
}