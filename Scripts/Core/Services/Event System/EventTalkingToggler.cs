using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class EventTalkingToggler : BaseToggler
	{
		private bool m_talkoverentity,m_talkoverinvoker,m_random;
		private string[] m_list;
		private string m_hlasky="";
		private IEntity m_target;
		private int m_hue;

		
		[CommandProperty( AccessLevel.GameMaster )]
		public int MessageHue 
		{
			get{return m_hue; }
			set{m_hue = value;InvalidateProperties();}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public string Messages 
		{
			get{return m_hlasky; }
			set{m_hlasky = value;
				if (value == null)
				{
					m_list = null;
				}else
				{
					m_list = m_hlasky.Split(';');
				}
				InvalidateProperties();
				}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public IEntity Target
		{
			get{return m_target; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Item TargetItem
		{
			get{return null; }
			set{m_target = value as IEntity;InvalidateProperties();}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile TargetMobile
		{
			get{return null; }
			set{m_target = value as IEntity ;InvalidateProperties();}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool TalkOverEntity 
		{
			get{return m_talkoverentity; }
			set{m_talkoverentity = value;
				m_talkoverinvoker= (value)?false:m_talkoverinvoker;
				InvalidateProperties();
				}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool TalkOverInvoker 
		{
			get{return m_talkoverinvoker; }
			set{m_talkoverinvoker = value;
				m_talkoverentity = (value)?false:m_talkoverentity;
				InvalidateProperties();
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool RandomMessage 
		{
			get{return m_random; }
			set{m_random = value;InvalidateProperties();}
		}
		
		[Constructable]
		public EventTalkingToggler():this(0x1CE1)
		{
		}
		
		public EventTalkingToggler(int ItemID):base(ItemID)
		{
			Name="EventTalkingToggler";
		}
		public override bool Toggle(byte state, Mobile who, int sid)
		{
			if (sid==lsid)
				return false;
			lsid= sid;
			bool send = false;
			
			if (!(m_list ==null || m_list.Length==0 || state < 1 || state > m_list.Length))
			{
				string text;
				
				if (m_random)
				{
					text = m_list[Utility.RandomMinMax(0,m_list.Length-1)];
				}else
				{
					text = m_list[state-1];
				}
				
				
				if (m_talkoverentity && m_target !=null)
				{
					if (m_target is Item)
					{
						(m_target as Item).PublicOverheadMessage(0,m_hue,false,text);
					}else if (m_target is Mobile )
					{
						(m_target as Mobile).PublicOverheadMessage(0,m_hue,false,text);
					}
					
				}else if (m_talkoverinvoker && who!=null && !who.Deleted)
				{
					who.PublicOverheadMessage(0,m_hue,false,text);
				}
				send = true;
			}
			
			bool ven = (Link == null || Link.Deleted)?true:Link.Toggle(state,who,sid);
			return send&ven;
		}
				
		public EventTalkingToggler( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write((int)0);//version
			writer.Write((bool) m_talkoverentity );
			writer.Write((bool) m_talkoverinvoker);
			writer.Write((bool) m_random);
			if(m_hlasky == null)
				m_hlasky = "";
			writer.Write((string)m_hlasky);
			writer.Write((int)m_hue);
			
			if (m_target is Mobile)
			{
				writer.Write((int)1);
				writer.Write(m_target as Mobile);
			}else if (m_target is Item)
			{
				writer.Write((int)2);
				writer.Write(m_target as Item);
			}else
			{
				writer.Write((int)3);
			}
				
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_talkoverentity = reader.ReadBool();
			m_talkoverinvoker = reader.ReadBool();
			m_random = reader.ReadBool();
			m_hlasky = reader.ReadString();
			m_hue = reader.ReadInt();
			
			int id = reader.ReadInt();
			switch (id)
			{
				case 1:
				{
						m_target = reader.ReadMobile() as IEntity;
						break;
				}
				case 2:
				{
						m_target = reader.ReadItem() as IEntity;
						break;
				}
			}
			
			m_list = m_hlasky.Split(';');
			
		}
	}
}
