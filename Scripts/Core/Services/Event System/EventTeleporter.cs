using System;
using Server;
using Server.Network;
using Server.Mobiles;
namespace Server.Items
{
	public class EventTeleporter : Item, IToggle
	{
		private bool m_Active,m_players,m_pets;
		private Point3D m_PointDest;
		private Map m_MapDest;
		private bool m_SourceEffect;
		private bool m_DestEffect;
		private int m_SoundID;
		private TimeSpan m_Delay;
		private Item m_toggler;
		private TimeSpan m_toggledelay;
		private bool m_toggling;
		
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Item EventLink
		{
			get { return m_toggler; }
			set { m_toggler = value; InvalidateProperties(); }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Toggling
		{
			get { return m_toggling; }
			set { m_toggling = value; InvalidateProperties(); }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan ToggleDelay
		{
			get{ return m_toggledelay; }
			set{ m_toggledelay = value; InvalidateProperties(); }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan Delay
		{
			get{ return m_Delay; }
			set{ m_Delay = value; InvalidateProperties(); }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool TeleportPlayers
		{
			get { return m_players; }
			set { m_players = value; InvalidateProperties(); }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool TeleportPets
		{
			get { return m_pets; }
			set { m_pets = value; InvalidateProperties(); }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool SourceEffect
		{
			get{ return m_SourceEffect; }
			set{ m_SourceEffect = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool DestEffect
		{
			get{ return m_DestEffect; }
			set{ m_DestEffect = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int SoundID
		{
			get{ return m_SoundID; }
			set{ m_SoundID = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Active
		{
			get { return m_Active; }
			set { m_Active = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D PointDest
		{
			get { return m_PointDest; }
			set { m_PointDest = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Map MapDest
		{
			get { return m_MapDest; }
			set { m_MapDest = value; InvalidateProperties(); }
		}


		public IToggler Link 
		{
			get{ return m_toggler as IToggler;}
			set
			{
				m_toggler = value as Item;
			}
		}
		
		[Constructable]
		public EventTeleporter() : this( new Point3D( 0, 0, 0 ), null )
		{
		}

		[Constructable]
		public EventTeleporter( Point3D pointDest, Map mapDest) : base( 0x1BC3 )
		{
			Name = "EventTeleporter";
			Movable = false;
			Visible = false;
			m_players =true;
			m_pets=true;
			m_Active = true;
			m_PointDest = pointDest;
			m_MapDest = mapDest;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Active )
				list.Add( 1060742 ); // active
			else
				list.Add( 1060743 ); // inactive

			if ( m_MapDest != null )
				list.Add( 1060658, "Map\t{0}", m_MapDest );

			if ( m_PointDest != Point3D.Zero )
				list.Add( 1060659, "Coords\t{0}", m_PointDest );

			list.Add( 1060660, "Teleports Pets\t {0}", m_pets ? "Yes" : "No" );
			list.Add( 1060661, "Teleports Players\t {0}", m_players ? "Yes" : "No" );
		}

		public override void OnSingleClick( Mobile from )
		{
			base.OnSingleClick( from );

			if ( m_Active )
			{
				if ( m_MapDest != null && m_PointDest != Point3D.Zero )
					LabelTo( from, "{0} [{1}]", m_PointDest, m_MapDest );
				else if ( m_MapDest != null )
					LabelTo( from, "[{0}]", m_MapDest );
				else if ( m_PointDest != Point3D.Zero )
					LabelTo( from, m_PointDest.ToString() );
			}
			else
			{
				LabelTo( from, "(inactive)" );
			}
		}

		public virtual void StartTeleport( Mobile m )
		{
			if (m_toggling)
			{
				if (Link !=null && !Link.Deleted )
					Link.Toggle(1,m,Rnd());
				Timer.DelayCall( m_toggledelay, new TimerStateCallback( Toggle_Callback ), m );
			}		
			
			if ( m_Delay == TimeSpan.Zero )
				DoTeleport( m );
			else
				Timer.DelayCall( m_Delay, new TimerStateCallback( DoTeleport_Callback ), m );
		}

		private int Rnd()
		{
			return (Utility.Random(int.MaxValue));
		}
		
		private void DoTeleport_Callback( object state )
		{
			DoTeleport( (Mobile) state );
		}
		
		private void Toggle_Callback( object state )
		{
			if(Link!=null && !Link.Deleted)
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

		public virtual void DoTeleport( Mobile m )
		{
			Map map = m_MapDest;

			if ( map == null || map == Map.Internal )
				map = m.Map;

			Point3D p = m_PointDest;

			if ( p == Point3D.Zero )
				p = m.Location;
			
			if (m_pets)
				Server.Mobiles.BaseCreature.TeleportPets( m, p, map );

			bool sendEffect = ( !m.Hidden || m.AccessLevel == AccessLevel.Player );

			if ( m_SourceEffect && sendEffect )
				Effects.SendLocationEffect( m.Location, m.Map, 0x3728, 10, 10 );

			m.MoveToWorld( p, map );

			if ( m_DestEffect && sendEffect )
				Effects.SendLocationEffect( m.Location, m.Map, 0x3728, 10, 10 );

			if ( m_SoundID > 0 && sendEffect )
				Effects.PlaySound( m.Location, m.Map, m_SoundID );
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( m_Active )
			{
				if ((m is PlayerMobile && m_players) || (m is BaseCreature && (m as BaseCreature).Controlled && m_pets) )
				{
					StartTeleport( m );
					return true;
				}
			}

			return true;
		}

		public EventTeleporter( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( (bool) m_SourceEffect );
			writer.Write( (bool) m_DestEffect );
			writer.WriteEncodedInt( (int) m_SoundID );
			writer.Write( m_Active );
			writer.Write( m_PointDest );
			writer.Write( m_MapDest );
			
			writer.Write((bool)m_pets);
			writer.Write((bool)m_players);
			writer.Write( (TimeSpan) m_Delay );
			writer.Write((Item)m_toggler);
			writer.Write(m_toggledelay);
			writer.Write(m_toggling);
			 
			
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_SourceEffect = reader.ReadBool();
					m_DestEffect = reader.ReadBool();
					m_SoundID = reader.ReadEncodedInt();
					m_Active = reader.ReadBool();
					m_PointDest = reader.ReadPoint3D();
					m_MapDest = reader.ReadMap();
					m_pets = reader.ReadBool();
					m_players = reader.ReadBool();
					m_Delay =reader.ReadTimeSpan();
					m_toggler = reader.ReadItem();
					m_toggledelay=reader.ReadTimeSpan();
					m_toggling=reader.ReadBool();
					if (Link!=null && !Link.Deleted)
						Timer.DelayCall( TimeSpan.FromSeconds(20), new TimerStateCallback( Toggle_Callback ), null );
					
					break;
				}
			}
		}
	}
}
