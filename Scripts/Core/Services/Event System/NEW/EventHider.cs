using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public enum HideState:byte
	{
		HiddenItem,
		RevealedItem,
		HidenMobile,
		RevealedMobile
	}
	public class EventHider : BaseToggler
	{
		private Item m_item;
		private TimeSpan m_delay = TimeSpan.Zero;
		private HideState m_hids = HideState.HidenMobile;
		[CommandProperty( AccessLevel.GameMaster )]
		public HideState DefaultState
		{
			get{ return m_hids; }
			set{ m_hids = value; InvalidateProperties();}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan Delay
		{
			get{ return m_delay; }
			set{ m_delay = value; InvalidateProperties();}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Item Item 
		{
			get{ return m_item; }
			set{ m_item = value; InvalidateProperties();}
		}
		
		private void Delay_Callback(object state)
		{	
			this.Toggle(0,state as Mobile ,Rnd());
		}
		
		[Constructable]
		public EventHider():this(0x1ED0)
		{	
		}
			
		public EventHider(int ItemID):base(ItemID)
		{
			Name="EventHider";

		}		
		public override bool Toggle(byte state, Mobile who, int sid)
		{
			if (sid==lsid)
				return false;
			lsid= sid;

			bool send = false;
			
			
			if (m_hids == HideState.HiddenItem || m_hids == HideState.RevealedItem)
			{
				if (!(m_item == null || m_item.Deleted))
				{
					
					if (state == 0)
					{
						m_item.Visible = (m_hids == HideState.HiddenItem)?true:false;
					}else
					{
						m_item.Visible = (m_hids == HideState.RevealedItem)?true:false;
						if (m_delay != TimeSpan.Zero )
							Timer.DelayCall( m_delay, new TimerStateCallback( Delay_Callback ), who );
					}
					send = true;
				}
			}
			else
			{
				if (!(who ==null || who.Deleted))
				{
					
					if (state == 0)
					{
						who.Hidden = (m_hids == HideState.HidenMobile)?true:false;
					}else
					{
						who.Hidden = (m_hids == HideState.RevealedMobile)?true:false;
						if (m_delay != TimeSpan.Zero )
							Timer.DelayCall( m_delay, new TimerStateCallback( Delay_Callback ), who );
					}
					send = true;
				}
			}
			bool ven = (Link == null || Link.Deleted)?true:Link.Toggle(state,who,sid);
			return send&ven;
		}
		
		
		public EventHider( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write((int)0);//version 
			writer.Write(m_item);
			writer.Write((TimeSpan) m_delay ); 
			writer.Write((byte)m_hids);
		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_item = reader.ReadItem() as Item;	
			m_delay = reader.ReadTimeSpan();
			m_hids = (HideState)reader.ReadByte();
		}
	}
}
