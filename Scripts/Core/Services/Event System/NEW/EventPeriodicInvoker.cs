using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class EventPeriodicInvoker : BaseToggle
	{
		private bool m_random = false;
		private TimeSpan m_minDelay = TimeSpan.FromSeconds(1);
		private TimeSpan m_maxDelay = TimeSpan.FromSeconds(1);
		private bool m_CanDeactivate =true;
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Random 
		{
			get {return m_random;}
			set {m_random = value;InvalidateProperties();}
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanDeactivate 
		{
			get {return m_CanDeactivate;}
			set {m_CanDeactivate = value;InvalidateProperties();}
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
			set
			{
				if (value < TimeSpan.FromMilliseconds(500))
					m_minDelay = TimeSpan.FromMilliseconds(500);
				else
					m_minDelay = value; 
				InvalidateProperties();
			}
		}
				
		[Constructable]
		public EventPeriodicInvoker():this(0x104B)
		{	
		}
			
		public EventPeriodicInvoker(int ItemID):base(ItemID)
		{
			Name="EventPeriodicInvoker";
			Timer.DelayCall( m_minDelay, new TimerStateCallback( Delay_Callback ), null );
		}
		
		private void Delay_Callback(object state)
		{	
			if (this==null || this.Deleted)
				return;
			Timer t;
			if (m_random || m_maxDelay > m_minDelay)
			{
				t=Timer.DelayCall(  TimeSpan.FromMilliseconds(Utility.RandomMinMax((int)m_minDelay.TotalMilliseconds,(int)m_maxDelay.TotalMilliseconds)), new TimerStateCallback( Delay_Callback ), null );
			}else
			{
				t=Timer.DelayCall( m_minDelay, new TimerStateCallback( Delay_Callback ),null );
			}
			if (Link==null || Link.Deleted)
				return;
			t.Priority = TimerPriority.TwoFiftyMS;			
			if (m_CanDeactivate)
				Link.Toggle(0,null,Rnd());
			Link.Toggle(1,null,Rnd());
		}		
		public EventPeriodicInvoker( Serial serial ) : base( serial ){}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write((int)0);//version 
			writer.Write((bool)m_random);
			writer.Write((TimeSpan)m_minDelay);
			writer.Write((TimeSpan)m_maxDelay);
			writer.Write((bool)m_CanDeactivate);
		}	
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_random = reader.ReadBool();
			m_minDelay = reader.ReadTimeSpan();
			m_maxDelay = reader.ReadTimeSpan();
			m_CanDeactivate = reader.ReadBool();
			Timer.DelayCall( TimeSpan.FromSeconds(30) ,new TimerStateCallback( Delay_Callback ), null );
		}
	}
}
