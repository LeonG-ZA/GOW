using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class EventLinkPauser : BaseToggler
	{
		protected class DelayObject
		{
			public Mobile Who;
			public byte State;
			public int Sid;
			public DelayObject(Mobile who, byte state, int sid)
			{
				Who =who;
				State =state;
				Sid =sid;
			}
		}
		private bool m_random = false;
		private TimeSpan m_minDelay = TimeSpan.FromSeconds(1);
		private TimeSpan m_maxDelay = TimeSpan.FromSeconds(1);
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Random 
		{
			get {return m_random;}
			set {m_random = value;InvalidateProperties();}
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan MaxDelay
		{
			get{ return m_maxDelay; }
			set{ m_maxDelay = value; InvalidateProperties();}
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan MinDelay
		{
			get{ return m_minDelay; }
			set{ m_minDelay = value; InvalidateProperties();}
		}
		
		
		private void Delay_Callback(object state)
		{	
			if(Link!=null && !Link.Deleted)
			{
				DelayObject x= state as DelayObject;
				if (state is DelayObject)
				{
					Link.Toggle(x.State,x.Who ,x.Sid);
				}else
				return;
			}
		}
		
		[Constructable]
		public EventLinkPauser():this(0x181B)
		{	
		}
			
		public EventLinkPauser(int ItemID):base(ItemID)
		{
			Name="EventLinkPauser";

		}		
		public override bool Toggle(byte state, Mobile who, int sid)
		{
			if (sid==lsid)
				return false;
			lsid= sid;
			Timer t;
			bool ven = true;
		
			if (m_random && m_maxDelay > m_minDelay)
			{
				t=Timer.DelayCall( TimeSpan.FromMilliseconds(Utility.RandomMinMax((int)m_minDelay.TotalMilliseconds,(int)m_maxDelay.TotalMilliseconds)), new TimerStateCallback( Delay_Callback ), new DelayObject(who,state,sid) );
			}else
			{
				t=Timer.DelayCall( m_minDelay, new TimerStateCallback( Delay_Callback ),new DelayObject(who,state,sid) );
			}
			t.Priority = TimerPriority.TwoFiftyMS;
			return ven;
		}
		
		
		public EventLinkPauser( Serial serial ) : base( serial ){}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write((int)0);//version 
			writer.Write((bool)m_random);
			writer.Write((TimeSpan)m_minDelay);
			writer.Write((TimeSpan)m_maxDelay);
		}	
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_random = reader.ReadBool();
			m_minDelay = reader.ReadTimeSpan();
			m_maxDelay = reader.ReadTimeSpan();
		}
	}
}
