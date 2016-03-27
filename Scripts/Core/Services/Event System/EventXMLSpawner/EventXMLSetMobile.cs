using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class EventXMLSetMobile : BaseToggler
	{
		private XmlSpawner m_spawner;
		private Mobile m_mobile;
		private bool m_NulifyOnZero;
		private bool m_setInvoker;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool SetInvoker {
			get {
				return m_setInvoker;
			}
			set {
				m_setInvoker = value;
				InvalidateProperties();
			}
		}	
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile TriggeredMobile
		{
			get {
				return m_mobile;
			}
			set {
				m_mobile = value;
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
		public EventXMLSetMobile():this(8454)
		{
		}
		
		public EventXMLSetMobile(int ItemID):base(ItemID)
		{
			Name="EventXMLSetMobile";

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
					m_spawner.TriggerMob = null;
				}else if (state !=0)
				{
					if (!m_setInvoker)
						m_spawner.TriggerMob = m_mobile;
					else
						m_spawner.TriggerMob = who;
				}
				send = true;
			}
			bool ven = (Link == null || Link.Deleted)?true:Link.Toggle(state,who,sid);
			return send&ven;
		}
		
		
		public EventXMLSetMobile( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write((int)1);//version
			writer.Write(m_spawner);
			writer.Write(m_NulifyOnZero);
			writer.Write(m_mobile);
			writer.Write(m_setInvoker);
		}
			
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_spawner = reader.ReadItem() as XmlSpawner;
			m_NulifyOnZero = reader.ReadBool();
			m_mobile = reader.ReadMobile();
			if(version >0)
				m_setInvoker = reader.ReadBool();
		}
	}
}
