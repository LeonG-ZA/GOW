using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Items;
using Server.Regions;
using System.Collections;
using Server.SkillHandlers;
using Server.Gumps;

namespace Server.Items
{
	public class EventRegionControl : RegionControl, IToggle, IToggler
	{
		#region Itoggle
		private Item m_togler;
		[CommandProperty( AccessLevel.GameMaster )]
		public Item EventLink
		{
			get{ return m_togler;}
			set
			{
				if(value is IToggler || value == null)
				{
					m_togler = value;
				}
				InvalidateProperties();
			}
		}
		
		public IToggler Link
		{
			get{ return m_togler as IToggler;}
			set
			{
				m_togler = value as Item;
			}
		}
		#endregion
		
		#region EventSystem connections
		private bool m_HandlePlayers = true;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool HandlePlayers {
			get { return m_HandlePlayers; }
			set
			{
				if(m_HandleAllMobiles)
					value = false;
				m_HandlePlayers = value;
			}
		}
		private bool m_HandlePets = false;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool HandlePets {
			get { return m_HandlePets; }
			set
			{
				if(m_HandleAllMobiles)
					value = false;
				m_HandlePets = value;
			}
		}
		private bool m_HandleAllMobiles = false;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool HandleAllMobiles {
			get { return m_HandleAllMobiles; }
			set
			{
				if(m_HandleAllMobiles)
					value = false;
				m_HandleAllMobiles = value;
			}
		}
		private bool m_HandleCreatures = false;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool HandleCreatures {
			get { return m_HandleCreatures; }
			set
			{
				if(m_HandleAllMobiles)
					value = false;
				m_HandleCreatures = value;
			}
		}
		
		private Item m_SignalTogler;
		[CommandProperty( AccessLevel.GameMaster )]
		public Item SignalLink
		{
			get{ return m_SignalTogler;}
			set
			{
				if(value is IToggler || value ==null)
				{
					m_SignalTogler = value;
				}
				InvalidateProperties();
			}
		}
		public IToggler signalLink
		{
			get{ return m_SignalTogler as IToggler;}
			set
			{
				m_SignalTogler = value as Item;
			}
		}
		
		
		private Item m_DeathTogler;
		[CommandProperty( AccessLevel.GameMaster )]
		public Item DeathLink
		{
			get{ return m_DeathTogler;}
			set
			{
				if(value is IToggler || value == null)
				{
					m_DeathTogler = value;
				}
				InvalidateProperties();
			}
		}
		public IToggler deathLink
		{
			get{ return m_DeathTogler as IToggler;}
			set
			{
				m_DeathTogler = value as Item;
			}
		}
		
		private Item m_EnterTogler;
		[CommandProperty( AccessLevel.GameMaster )]
		public Item EnterLink
		{
			get{ return m_EnterTogler;}
			set
			{
				if(value is IToggler || value == null)
				{
					m_EnterTogler = value;
				}
				InvalidateProperties();
			}
		}
		public IToggler enterLink
		{
			get{ return m_EnterTogler as IToggler;}
			set
			{
				m_EnterTogler = value as Item;
			}
		}
		
		private Item m_ExitTogler;
		[CommandProperty( AccessLevel.GameMaster )]
		public Item ExitLink
		{
			get{ return m_ExitTogler;}
			set
			{
				if(value is IToggler || value ==null)
				{
					m_ExitTogler = value;
				}
				InvalidateProperties();
			}
		}
		public IToggler exitLink
		{
			get{ return m_ExitTogler as IToggler;}
			set
			{
				m_ExitTogler = value as Item;
			}
		}
		
		#endregion
		
		#region events
		public void OnEnter(Mobile m)
		{
			if (enterLink !=null)
			{
				if(HandleAllMobiles)
					enterLink.Toggle(1,m,Rnd());
				else if (m_HandlePlayers && m.Player)
					enterLink.Toggle(1,m,Rnd());
				else if (m is BaseCreature)
				{
					BaseCreature c = m as BaseCreature;
					if(c.Controlled||c.Summoned)
					{
						if(m_HandlePets)
							enterLink.Toggle(1,m,Rnd());
					}else if(m_HandleCreatures)
					{
						enterLink.Toggle(1,m,Rnd());
					}
				}
			}
		}
		public void OnExit(Mobile m)
		{
			if (exitLink !=null)
			{
				if(HandleAllMobiles)
					exitLink.Toggle(1,m,Rnd());
				else if (m_HandlePlayers && m.Player)
					exitLink.Toggle(1,m,Rnd());
				else if (m is BaseCreature)
				{
					BaseCreature c = m as BaseCreature;
					if(c.Controlled||c.Summoned)
					{
						if(m_HandlePets)
							exitLink.Toggle(1,m,Rnd());
					}else if(m_HandleCreatures)
					{
						exitLink.Toggle(1,m,Rnd());
					}
				}
			}
		}
		public void OnDeath(Mobile m)
		{
			if (deathLink !=null)
			{
				if(HandleAllMobiles)
					deathLink.Toggle(1,m,Rnd());
				else if (m_HandlePlayers && m.Player)
					deathLink.Toggle(1,m,Rnd());
				else if (m is BaseCreature)
				{
					BaseCreature c = m as BaseCreature;
					if(c.Controlled||c.Summoned)
					{
						if(m_HandlePets)
							deathLink.Toggle(1,m,Rnd());
					}else if(m_HandleCreatures)
					{
						deathLink.Toggle(1,m,Rnd());
					}
				}
			}
		}
		
		#endregion
		
		#region IToggler
		protected int lsid; //last session id
		protected int Rnd()
		{
			return (Utility.Random(int.MaxValue));
		}
		private int nsd;
		public virtual bool Toggle(byte state, Mobile who, int sid)
		{
			if (sid ==nsd)
				return false;
			
			if (sid==lsid)
				return false;
			lsid= sid;
			
			bool ven = (Link == null || Link.Deleted)?true:Link.Toggle(state,who,sid);
			
			
			if (state !=0 && Region !=null)
			{
				List<Mobile> list;
				if(m_HandleAllMobiles || (m_HandlePlayers && m_HandlePets && m_HandleCreatures))
				{
					list = Region.GetMobiles();
				}else if(m_HandlePlayers && !m_HandlePets && !m_HandleCreatures)
				{
					list = Region.GetPlayers();
				}else
				{
					List<Mobile> buff = Region.GetMobiles();
					list= new List<Mobile>();
					for(int i = 0; i <buff.Count;i++)
					{
						if(m_HandlePlayers && buff[i].Player)
							list.Add(buff[i]);
						else if (buff[i]is BaseCreature)
						{
							BaseCreature c = buff[i] as BaseCreature;
							if(c.Controlled||c.Summoned)
							{
								if(m_HandlePets)
									list.Add(buff[i]);
							}else if(m_HandleCreatures)
							{
								list.Add(buff[i]);
							}
						}
						
					}
				}
				
				
				for(int i = 0; i<list.Count;i++)
				{
					if(signalLink !=null && !signalLink.Deleted)
						signalLink.Toggle(1,list[i],nsd = Rnd());
				}
			}
            return true;//&ven
		}
		#endregion
		
		#region serializace, konstruktory
		[Constructable]
		public EventRegionControl():base()
		{
			ItemID = 5591;
			Name = "Event Region Control";
		}

        public EventRegionControl(Serial serial) : base(serial) { }

        public override void UpdateRegion()
        {
            if (m_Region != null)
                m_Region.Unregister();

            if (this.Map != null && this.Active)
            {
                if (this != null && this.RegionArea != null && this.RegionArea.Length > 0)
                {
                    m_Region = new EventCustomRegion(this);
                    m_Region.Register();
                }
                else
                    m_Region = null;
            }
            else
                m_Region = null;
        }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1);//version
			writer.Write((bool)m_HandleAllMobiles);
			writer.Write((bool)m_HandleCreatures);
			writer.Write((bool)m_HandlePets);
			writer.Write((bool)m_HandlePlayers);

			writer.Write(m_DeathTogler);
			writer.Write(m_EnterTogler);
			writer.Write(m_ExitTogler);

			writer.Write(m_SignalTogler);
			writer.Write(m_togler);
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_HandleAllMobiles = reader.ReadBool();
			m_HandleCreatures = reader.ReadBool();
			m_HandlePets = reader.ReadBool();
			m_HandlePlayers = reader.ReadBool();

			m_DeathTogler = reader.ReadItem();
			m_EnterTogler = reader.ReadItem();
			m_ExitTogler = reader.ReadItem();

			m_SignalTogler = reader.ReadItem();
			m_togler = reader.ReadItem();
        }
        #endregion
    }
}

