using System;
using Server;
using Server.Network;
using Server.Mobiles;
using System.Collections;

namespace Server.Items
{
	public class EventLeverToggle : BaseToggle
	{
		public enum LeverType
		{
			WELever,
			NSLever,
			WESwitch,
			NSSwitch,
			Advanced,
		}
		private LeverType m_type;
		private TimeSpan m_delay;
		private int m_state1,m_state2,m_str;
		private byte m_state;
		private bool m_delayable;
		
		private bool m_ShowProps = true;
		private bool m_ShowMessages = true;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool ShowProps {
			get { return m_ShowProps; }
			set { m_ShowProps = value; InvalidateProperties(); }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool ShowMessages {
			get { return m_ShowMessages; }
			set { m_ShowMessages = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan Delay
		{
			get{ return m_delay; }
			set{ m_delay = value; InvalidateProperties();}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int RequiredStr
		{
			get{ return m_str; }
			set{ m_str = value; InvalidateProperties();}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int State1_ID
		{
			get{ return m_state1; }
			set{ 
				if (m_type==LeverType.Advanced)
					m_state1 = value;
					InvalidateProperties();
				}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int State2_ID
		{
			get{ return m_state2; }
			set{ m_state2 = value; InvalidateProperties();}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public LeverType Type
		{
			get { return m_type; }
			set { m_type = value; 
					switch (m_type)
					{
						case LeverType.NSLever:
						{
							m_state1 = 0x1093;
							m_state2 = 0x1095;
							break;
						}
							
						case LeverType.WELever:
						{
							m_state1 = 0x108C;
							m_state2 = 0x108E;
							break;
						}
							
						case LeverType.NSSwitch:
						{
							m_state1 = 0x1092;
							m_state2 = 0x1091;
							break;
						}
							
						case LeverType.WESwitch:
						{
							m_state1 = 0x108F;
							m_state2 = 0x1090;
							break;
						}
					
					}
					if (m_state ==0)
					{
						ItemID = m_state1;
					}else
					{
						ItemID = m_state2;
					}
					InvalidateProperties();
				}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Delayable
		{
			get { return m_delayable; }
			set { m_delayable = value; }
		}

		private void Delay_Callback(object state)
		{	
			if (state is Mobile)
			{
				Switch(true,state as Mobile);
			}else
			{
				Switch(true,null);
			}			
		}


		[Constructable]
		public EventLeverToggle():this(0x108C)
		{ 
		}
		
		[Constructable]
		public EventLeverToggle(int ItemID):base(ItemID)
		{
			Name = "EventLeverToggle"; 
			m_delayable = false;
			m_str = 10;
			Type = LeverType.WELever;
			Visible = true;
		}
		
		private void Switch(bool tick, Mobile who)
		{
			if (m_delayable && m_state!=0 && !tick)
				return;
			
			if (m_state==0 && !tick)
			{
				m_state = 1;
				ItemID = m_state2;
				if (m_delayable)
					Timer.DelayCall( m_delay, new TimerStateCallback( Delay_Callback), null );
				
			}else
			{
				m_state = 0;
				ItemID = m_state1;
			}
			
			if (Link!=null && !Link.Deleted)
			{
				Link.Toggle(m_state,who ,Rnd());
			}
		}
		
		
		
		public override void OnDoubleClick( Mobile m ) 
      	{  
        	if ( m.InRange( this.GetWorldLocation(), 1 ) )
        	{
        		if (m.Str >= m_str )
        		{
        			Switch(false,m);
        			if ((m_state == 1 && !m_delayable)||m_state == 0)
        			{
        				if(m_ShowMessages)
        					m.SendMessage("Lever moved.");
        			}
        			
        		}else if(m_ShowMessages)
        		{
        			m.SendMessage("You are to weak to move with this lever");
        		}
        	
        	}else if(m_ShowMessages)
        	{
        		m.SendMessage("You are too far from lever");
        	}
		}

		public EventLeverToggle( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write((int) 1 ); // version
			writer.Write((bool)m_delayable);
			writer.Write((TimeSpan) m_delay );
			writer.Write((byte)m_state);
			writer.Write((int)m_state1);
			writer.Write((int)m_state2);
			writer.Write((int)m_str);
			writer.Write((int)m_type);
			writer.Write((bool)m_ShowProps);
			writer.Write((bool)m_ShowMessages);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_delayable= reader.ReadBool();
			m_delay = reader.ReadTimeSpan();
			m_state = reader.ReadByte();
			m_state1 = reader.ReadInt();
			m_state2 = reader.ReadInt();
			m_str = reader.ReadInt();
			m_type = (LeverType)reader.ReadInt();
			
			if(version > 0)
			{
				m_ShowProps = reader.ReadBool();
				m_ShowMessages = reader.ReadBool();
			}
			
			if ( Link!=null && !Link.Deleted)
				Timer.DelayCall( TimeSpan.FromSeconds(20), new TimerStateCallback( Delay_Callback), null );
		}

		protected override bool ShowDetails {
			get { return m_ShowProps; }
		}
	/*	public override void GetProperties(ObjectPropertyList list)
		{
			if(m_ShowProps)
				base.GetProperties(list);
		}*/
	}
}
