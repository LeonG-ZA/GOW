using System;
using Server;
using Server.Network;
using Server.Mobiles;
using System.Collections;
namespace Server.Items
{
	public class EventMovingParticles : BaseToggler
	{
		public enum Directions:byte
		{
			FromMobileToItem1,
			FromItem1ToMobile,
			FromItem1ToItem2
		}
		
		private Item m_item1,m_item2;
		private TimeSpan m_delay;
		private int m_hue,m_speed;
		private EventEffectType m_type;
		private int m_animid =(int)EventEffectType.Bees ;
		private Directions d = Directions.FromMobileToItem1;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Directions AnimationDirection {
			get {
				return d;
			}
			set {
				d = value;
				InvalidateProperties();
			}
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public Item Item2 {
			get {
				return m_item2;
			}
			set {
				m_item2 = value;
				InvalidateProperties();
			}
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public Item Item1 {
			get {
				return m_item1;
			}
			set {
				m_item1 = value;
				InvalidateProperties();
			}
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public int AnimationHue
		{
			get{return m_hue; }
			set{m_hue = value;InvalidateProperties();}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int AnimationItemId
		{
			get{return m_animid; }
			set{if (m_type==EventEffectType.Advanced)
					m_animid = value;
					InvalidateProperties();
				}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int AnimationSpeed
		{
			get{return m_speed; }
			set{m_speed = value;InvalidateProperties();}
		}	
		
		[CommandProperty( AccessLevel.GameMaster )]
		public EventEffectType Animation
		{
			get{return m_type; }
			set{m_type = value;
				if (value == EventEffectType.Lighting)
					return;
				if (value!=EventEffectType.Advanced)
					m_animid = (int)value;
				InvalidateProperties();
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan EffectDelay
		{
			get{return m_delay; }
			set{m_delay = value;InvalidateProperties();}
		}
		
		[Constructable]
		public EventMovingParticles():this(0xF00)
		{
		}
		
		public EventMovingParticles(int ItemID):base(ItemID)
		{
			Name="EventMovingParticles";
			m_hue = 0;
			m_type = EventEffectType.Bees;
			m_delay = TimeSpan.FromSeconds(1);
		}
		
		public override bool Toggle(byte state, Mobile who, int sid)
		{
			if (sid==lsid)
				return false;
			lsid= sid;
			
			bool send = false;
			if (state > 0)
			{		
				switch (d)
				{
					case Directions.FromItem1ToItem2:
						{
							if (!(m_item1==null || m_item2 == null || m_item1.Deleted || m_item2.Deleted))
							{
								
								Effects.SendMovingParticles(m_item1,m_item2,m_animid,m_speed,((int)(m_delay.TotalSeconds *5)),false,false,m_hue,0,0,0,0,EffectLayer.Head,0);
								send = true;
							}
							break;
						}
					case Directions.FromItem1ToMobile:
						{
							if (!(m_item1==null || m_item1.Deleted || who==null || who.Deleted))
							{
								Effects.SendMovingParticles(m_item1,who,m_animid,m_speed,(int)(m_delay.TotalSeconds *5),false,false,m_hue,0,0,0,0,EffectLayer.Head,0);
								send = true;
							}
							break;
						}
					case Directions.FromMobileToItem1:
						{
							if (!(m_item1==null || m_item1.Deleted || who==null || who.Deleted))
							{
								Effects.SendMovingParticles(who,m_item1,m_animid,m_speed,(int)(m_delay.TotalSeconds *5),false,false,m_hue,0,0,0,0,EffectLayer.Head,0);
								send = true;
							}
							break;
						}
						
				}
			}
			bool ven = (Link == null || Link.Deleted)?true:Link.Toggle(state,who,sid);
			return send&ven;
		}
		
		
		public EventMovingParticles( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write((int)1);//version
			//version 1
			writer.Write((int)m_animid);
			//version 0
			writer.Write((TimeSpan)m_delay);
			writer.Write((int)m_hue);
			writer.Write((int)m_speed);
			writer.Write((int)m_type);
			writer.Write((Item)m_item1);
			writer.Write((Item)m_item2);
			writer.Write((int)d);
			
			
		}	
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			switch (version)
			{
				case 0:
					{
						m_delay = reader.ReadTimeSpan();
						m_hue = reader.ReadInt();
						m_speed = reader.ReadInt();
						m_type = (EventEffectType)reader.ReadInt();
						m_item1 = reader.ReadItem();
						m_item2 = reader.ReadItem();
						d = (Directions)reader.ReadInt();
						break;
					}
				case 1:
					{
						m_animid = reader.ReadInt();
						goto case 0;
					}
			
			}
			
		}
	}
}
