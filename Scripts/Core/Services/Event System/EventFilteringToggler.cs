using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class EventFilteringToggler : BaseToggler
	{
		private BaseToggler m_filteredlink;
		private byte m_passedsignal;
		
		
		[CommandProperty( AccessLevel.GameMaster )]
		public BaseToggler FilteredLink
		{
			get{ return m_filteredlink;}
			set{ m_filteredlink = value;InvalidateProperties();}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public byte PassedSignalID 
		{
			get{return m_passedsignal; }
			set{m_passedsignal = value;InvalidateProperties();}
		}
		
		[Constructable]
		public EventFilteringToggler():this(0xE2E)
		{	
		}
			
		public EventFilteringToggler(int ItemID):base(ItemID)
		{
			Name="EventFilteringToggler";
		}		
		
		public override bool Toggle(byte state, Mobile who, int sid)
		{
			if (sid==lsid)
				return false;
			lsid= sid;
			
			bool ven2 = true;
			if ( state == m_passedsignal ) 
				ven2 = (FilteredLink == null || FilteredLink.Deleted)?true:FilteredLink.Toggle(state,who,sid);
			
			bool ven = (Link == null || Link.Deleted)?true:Link.Toggle(state,who,sid);
			return true&ven&ven2;
		}
		
		
		public EventFilteringToggler( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write((int)0);//version
			writer.Write((Item)m_filteredlink);
			writer.Write((byte)m_passedsignal);

		}
		
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_filteredlink = reader.ReadItem() as BaseToggler;
			m_passedsignal = reader.ReadByte();
		}
	}
}
