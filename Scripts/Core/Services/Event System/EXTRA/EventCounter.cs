using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class EventCounter : BaseToggler
	{	
		private int m_countUpTo = 5;
		private int m_actualCount = 0;
		private Item m_CountLink;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Item CountLink {
			get { return m_CountLink; }
			set
			{
				if(value is IToggler)
					m_CountLink = value;
				InvalidateProperties();
			}
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public int ActualCount {
			get { return m_actualCount; }
			set { m_actualCount = value; InvalidateProperties();}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int CountUpTo {
			get { return m_countUpTo; }
			set { m_countUpTo = value;InvalidateProperties(); }
		}
		
		
		[Constructable]
		public EventCounter():this(4004)
		{
		}
		
		public EventCounter(int ItemID):base(ItemID)
		{
			Name="EventCounter";
		}
		
		private int lsidc=0;
		public override bool Toggle(byte state, Mobile who, int sid)
		{
			if (sid==lsid || sid==lsidc)
				return false;
			lsid= sid;
			
			if (m_actualCount < 0)
				m_actualCount = 0;
			if(state >0)
			{
				m_actualCount++;
			}
			
			if(m_actualCount > m_countUpTo)
			{
				if(m_CountLink!=null && !m_CountLink.Deleted)
				{
					lsidc = Rnd();
					((IToggler)m_CountLink).Toggle(1,who,lsidc);
				}
				m_actualCount = 0;
			}
			
			InvalidateProperties();
			bool ven = (Link == null || (Link as Item).Deleted)?true:Link.Toggle(state,who,sid);
			return true&ven;
		}
		
		
		public EventCounter( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write((int)0);//version
			writer.Write((int)m_actualCount);
			writer.Write((int)m_countUpTo);
			writer.Write((Item)m_CountLink);
			

		}
		
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_actualCount = reader.ReadInt();
			m_countUpTo = reader.ReadInt();
			m_CountLink = reader.ReadItem();
		}
	}
}
