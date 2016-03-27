using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class EventXMLSetItem : BaseToggler
	{
		private XmlSpawner m_spawner;
		private Item m_item;
		private bool m_NulifyOnZero;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Item SetedItem
		{
			get {
				return m_item;
			}
			set {
				m_item = value;
				InvalidateProperties();
			}
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public bool NulifyOnZero {
			get {
				return m_NulifyOnZero;
			}
			set {
				m_NulifyOnZero = value;
				InvalidateProperties();
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public XmlSpawner XMLSpawner
		{
			get{ return m_spawner; }
			set{ m_spawner = value; InvalidateProperties();}
		}
		
		[Constructable]
		public EventXMLSetItem():this(0xFB5)
		{
		}
		
		public EventXMLSetItem(int ItemID):base(ItemID)
		{
			Name="EventXMLSetItem";

		}
		public override bool Toggle(byte state, Mobile who, int sid)
		{
			if (sid==lsid)
				return false;
			lsid= sid;
			
			bool send = false;
			if (!(m_spawner == null || m_spawner.Deleted))
			{
				
				if (state == 0 && m_NulifyOnZero)
				{
					m_spawner.SetItem = null;
				}else if (state !=0)
				{
					m_spawner.SetItem= m_item;
				}
				send = true;
			}
			bool ven = (Link == null || Link.Deleted)?true:Link.Toggle(state,who,sid);
			return send&ven;
		}
		
		
		public EventXMLSetItem( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write((int)0);//version
			writer.Write(m_spawner);
			writer.Write(m_NulifyOnZero);
			writer.Write(m_item);
		}
			
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_spawner = reader.ReadItem() as XmlSpawner;
			m_NulifyOnZero = reader.ReadBool();
			m_item = reader.ReadItem();
		}
	}
}
