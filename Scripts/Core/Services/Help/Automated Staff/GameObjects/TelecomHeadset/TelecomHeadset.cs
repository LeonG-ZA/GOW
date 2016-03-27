using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.ContextMenus;
using Server.Gumps;
using Server.Engines.Quests;
using Server.Engines.Quests.Necro;
using Server.Spells;
using Server.Spells.Fourth;
using Server.Targeting;

namespace Server.Items
{
	public class CommUnit : SilverEarrings
	{
        private Mobile m_Owner;
        public Point3D m_HomeLocation;
        public Map m_HomeMap;

		private string m_Channel;
		[CommandProperty( AccessLevel.GameMaster )]
		public string Channel
		{
			get{ return m_Channel; }
			set{ m_Channel = value; }
		}
		[Constructable]
		public CommUnit()
		{
            LootType = LootType.Blessed;
            Weight = 1.0;
            Hue = 2406;
            Name = "An Unassigned Personal Communicator"; 
		}

		public override void OnDoubleClick( Mobile m )
		{
            m.Prompt = new SelectChannelPrompt(this);
            m.SendMessage(0, "Please enter the channel you wish to use.");

            if (m_Owner == null && m.AccessLevel == AccessLevel.Player)
            {
                m.SendMessage("Only the server administration can use this item!");
                //this.Delete();
            }

            else if (m_Owner == null && m.AccessLevel == AccessLevel.Counselor)
            {
                m.SendMessage("Only the server administration can use this item!");
                //this.Delete();
            }

            else if (m_Owner == null)
            {
                m_Owner = m;
                this.Name = m_Owner.Name.ToString() + "'s Personal Communicator";
                this.HomeLocation = m.Location;
                this.HomeMap = m.Map;
                m.SendMessage("This personal communicator has been assigned to you.");
            }
            else
            {
                if (m_Owner != m)
                {
                    m.SendMessage("This item has not been assigned to you!");
                    return;
                }
            }
		}

		public CommUnit( Serial serial ) : base( serial )
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
            private CommUnit m_Item;
            private Mobile m_Mobile;

            public GoHomeEntry(Mobile from, Item item): base(5134) // uses "Goto Loc" entry
            {
                m_Item = (CommUnit)item;
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
            private CommUnit m_Item;
            private Mobile m_Mobile;

            public SetHomeEntry(Mobile from, Item item): base(2055) // uses "Mark" entry
            {
                m_Item = (CommUnit)item;
                m_Mobile = from;
            }

            public override void OnClick()
            {
                m_Item.HomeLocation = m_Mobile.Location;
                m_Item.HomeMap = m_Mobile.Map;
                m_Mobile.SendMessage("The home location on your personal communicator has been set to your current position.");
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
                    from.SendMessage("This personal communicator is not yours to use.");
                    return;
                }
                else
                {
                    base.GetContextMenuEntries(from, list);
                    CommUnit.GetContextMenuEntries(from, this, list);
                }
            }
        }

        public override bool OnEquip(Mobile from)
        {
            if (from.AccessLevel < AccessLevel.GameMaster)
            {
                from.SendMessage("This personal communicator can only be used by server administrators!");
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

			writer.Write( (string) m_Channel );
		}
		
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    m_HomeLocation = reader.ReadPoint3D();
                    m_HomeMap = reader.ReadMap();
                    m_Owner = reader.ReadMobile(); 
                }   goto case 0;            
                case 0:
                {
                    m_Channel = reader.ReadString();
                    break;
                }  
            }
        }
   
		private class SelectChannelPrompt : Prompt
		{
			private CommUnit unit;

			public SelectChannelPrompt( CommUnit comm )
			{
				unit = comm;
			}
			public override void OnResponse( Mobile from, string text )
			{
				unit.Channel = text;
			}
		}
	}
}