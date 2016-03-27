using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class EventDamageToggler : BaseToggler
	{
		public enum DungDmgType
		{
			Fixed,
			Procentual,
			ProcentualFromActualValue,
		}
		
		private bool m_cankill;
		private TimeSpan m_paralyzefor;
		private int m_hpmod,m_stmod,m_mnmod;
		private DungDmgType m_dmgtype;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanKill 
		{
			get{return m_cankill; }
			set{m_cankill = value;InvalidateProperties();}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public DungDmgType TypeOfDmg
		{
			get{return m_dmgtype; }
			set{m_dmgtype = value;InvalidateProperties();}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan ParalyzeDuration 
		{
			get{return m_paralyzefor; }
			set{m_paralyzefor = value;InvalidateProperties();}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int HPmod 
		{
			get{return m_hpmod; }
			set{m_hpmod = value;InvalidateProperties();}
		}		
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int STmod 
		{
			get{return m_stmod; }
			set{m_stmod = value;InvalidateProperties();}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int MNmod 
		{
			get{return m_mnmod; }
			set{m_mnmod = value;InvalidateProperties();}
		}		
		
		
		[Constructable]
		public EventDamageToggler():this(0x26CE)
		{	
		}
			
		public EventDamageToggler(int ItemID):base(ItemID)
		{
			Name="EventDamageToggler";
			m_cankill= true;
			m_dmgtype = DungDmgType.Fixed;
		}		
		
		
		public override bool Toggle(byte state, Mobile who, int sid)
		{
			if (sid==lsid)
				return false;
			lsid= sid;
			bool send  = false;
			if (state >0)
			{
				if (!(who==null ||who.Deleted))
				{
					if (m_paralyzefor!= TimeSpan.Zero )
						who.Paralyze( m_paralyzefor );
					switch (m_dmgtype)
					{
						case DungDmgType.Procentual:
							{
								who.Hits -=(int)( who.HitsMax * (double)m_hpmod /100);
								if (who.Hits > who.HitsMax)
									who.Hits = who.HitsMax;
								if (m_cankill)
									if(who.Hits <= 0)
									who.Kill();
								
								who.Stam -=(int)( who.StamMax * (double)m_stmod /100);
								if (who.Stam > who.StamMax)
									who.Stam = who.StamMax;
								
								who.Mana -=(int)( who.ManaMax *(double)m_mnmod /100);
								if (who.Mana > who.ManaMax)
									who.Mana = who.ManaMax;
								break;
							}
							
						case DungDmgType.ProcentualFromActualValue:
							{
								who.Hits -=(int)( who.Hits * ( Math.Abs((double)m_hpmod) /100));
								if (who.Hits > who.HitsMax)
									who.Hits = who.Hits;
								if (m_cankill)
									if(who.Hits <= 0)
									who.Kill();
								
								who.Stam -=(int)( who.Stam * (Math.Abs((double)m_stmod) /100));
								if (who.Stam > who.StamMax)
									who.Stam = who.Stam;
								
								who.Mana -=(int)( who.Mana * (Math.Abs((double)m_mnmod) /100));
								if (who.Mana > who.ManaMax)
									who.Mana = who.Mana;
								break;
							}
							default :
							{
								who.Hits -= m_hpmod;
								if (m_cankill)
									if(who.Hits <= 0)
									who.Kill();
								who.Stam -= m_stmod;
								who.Mana -= m_mnmod;
								break;
							}
					}
					send = true;
				}
			}
			bool ven = (Link == null || Link.Deleted)?true:Link.Toggle(state,who,sid);
			return send&ven;
		}
		
		
		public EventDamageToggler( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write((bool)m_cankill);
			writer.Write((TimeSpan)m_paralyzefor);
			writer.Write((int)m_hpmod);
			writer.Write((int)m_stmod);
			writer.Write((int)m_mnmod);
			writer.Write((int)m_dmgtype);
		}
		
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			m_cankill = reader.ReadBool();
			m_paralyzefor = reader.ReadTimeSpan();
			m_hpmod = reader.ReadInt();
			m_stmod	= reader.ReadInt();
			m_mnmod	= reader.ReadInt();
			m_dmgtype =(DungDmgType)reader.ReadInt();
		}
	}
}
