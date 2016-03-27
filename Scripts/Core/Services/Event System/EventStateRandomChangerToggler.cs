using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class EventStateRandomChangerToggler : BaseToggler
	{
		private int m_min;
		private int m_max;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int MinStateIndex 
		{
			get{return m_min; }
			set{m_min = value; InvalidateProperties();}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxStateIndex 
		{
			get{return m_max; }
			set{m_max = value; InvalidateProperties();}
		}	
		
		
		[Constructable]
		public EventStateRandomChangerToggler():this(0xFA7)
		{	
		}
			
		public EventStateRandomChangerToggler(int ItemID):base(ItemID)
		{
			Name="EventStateRandomChangerToggler";
			m_min = 1;
			m_max = 1;
		}
		
		
		public override bool Toggle(byte state, Mobile who, int sid)
		{
			if (sid==lsid)
				return false;
			lsid= sid;
			
			bool ven = false;
			if (state!=0)
				ven = (Link == null || Link.Deleted)?true:Link.Toggle((byte)Utility.RandomMinMax(m_min,m_max),who,sid);
			else
				ven = (Link == null || Link.Deleted)?true:Link.Toggle(state,who,sid);
			return true&ven;
		}
		
		
		public EventStateRandomChangerToggler( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write((int)0);//version
			writer.Write((int)m_min);
			writer.Write((int)m_max);
		}
				
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_min = reader.ReadInt();
			m_max = reader.ReadInt();
		}
	}
}
