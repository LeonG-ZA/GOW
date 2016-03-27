using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class EventXMLRespawn : BaseToggler
	{
		private XmlSpawner m_spawner;	
		[CommandProperty( AccessLevel.GameMaster )]
		public XmlSpawner XMLSpawner
		{
			get{ return m_spawner; }
			set{ m_spawner = value; InvalidateProperties();}
		}
		
		[Constructable]
		public EventXMLRespawn():this(6235)
		{
		}
		
		public EventXMLRespawn(int ItemID):base(ItemID)
		{
			Name="EventXMLRespawn";

		}
		public override bool Toggle(byte state, Mobile who, int sid)
		{
			if (sid==lsid)
				return false;
			lsid= sid;
			bool send = false;
			
			
			if (!(m_spawner == null || m_spawner.Deleted))
			{
				if (state !=0)
				{
					m_spawner.Respawn();
				}
				send = true;
			}
			bool ven = (Link == null || (Link as Item).Deleted)?true:Link.Toggle(state,who,sid);
			return send&ven;
		}
		
		
		public EventXMLRespawn( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write((int)0);//version
			writer.Write(m_spawner);
		}
			
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_spawner = reader.ReadItem() as XmlSpawner;
		}
	}
}
