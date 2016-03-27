using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class EventSpeechToggle : BaseToggle
	{
		private bool m_active;
		private string m_pass;
		private int m_distance;
		private TimeSpan m_delay;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan Delay
		{
			get{ return m_delay; }
			set{ m_delay = value; InvalidateProperties();}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Active
		{
			get{ return m_active; }
			set{ m_active = value; InvalidateProperties();}
		}	
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int Range
		{
			get{ return m_distance; }
			set{ m_distance = value; InvalidateProperties();}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Pass
		{
			get{ return m_pass; }
			set{ 	if (value==null)
					{
						m_pass="";
					}else
					{
						m_pass = " "+value.ToLower()+" ";
					}
				InvalidateProperties();
				}
		}
			
		[Constructable]
		public EventSpeechToggle():this(0x1DA0)
		{	
		}
		
		[Constructable]	
		public EventSpeechToggle(int ItemID):base(ItemID)
		{
			m_active = true;
			m_pass = "";
			m_distance=0;
			Name = "EventSpeechToggle";
		}
		
		
		public EventSpeechToggle( Serial serial ) : base( serial )
		{
		}
		
		private void Toggle_Callback( object state )
		{
			if(Link!=null && !(Link as Item).Deleted)
			{
				if (state is Mobile)
				{
					Link.Toggle(0,state as Mobile ,Rnd());
				}else
				{
					Link.Toggle(0,null ,Rnd());
				}
			}
				
		}
		public override bool HandlesOnSpeech
		{
			get{ return m_active; }
		}
		public override void OnSpeech( SpeechEventArgs e )
		{
			if (e.Mobile.AccessLevel >AccessLevel.Player && e.Mobile.Hidden)
				return;
			if ( e.Type == MessageType.Emote )
				return;
			
			Mobile m = e.Mobile;
			if ( !m.InRange( GetWorldLocation(), m_distance) )
				return;
			if (e.Handled)
				return;
			string speech = " " + e.Speech.ToLower()+" ";
			if (speech.IndexOf(m_pass)>=0)
			{
				if(Link!=null && !(Link as Item).Deleted)
				{
					Link.Toggle(1,m,Rnd());
					Timer.DelayCall( m_delay, new TimerStateCallback( Toggle_Callback ), m );
					e.Handled=true;
				}
			}			
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write((int)0);//version
			writer.Write((bool) m_active);
			writer.Write((string)m_pass);
			writer.Write((int)m_distance);
			writer.Write((TimeSpan)m_delay);
		}
		
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();			
			m_active= reader.ReadBool();	
			m_pass= reader.ReadString();
			m_distance= reader.ReadInt();
			m_delay = reader.ReadTimeSpan();
			
			if(Link!=null && !(Link as Item).Deleted)
				Timer.DelayCall( TimeSpan.FromSeconds(20), new TimerStateCallback( Toggle_Callback ), null );
			
		}

		
	}
}
