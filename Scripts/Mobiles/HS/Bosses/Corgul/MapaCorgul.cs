using System;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.Gumps;
using Server.Engines.PartySystem;
using Server.Items;
using Server.Multis;

namespace Server.Items
{
    public class CorgulMap : MapItem
    {
	    public Point3D CorgulIslandloc = new Point3D(3600, 760, -5);
		public Point3D CorgulSpawn = new Point3D(6431 + Utility.RandomMinMax( -5, 5 ), 1252 + Utility.RandomMinMax( -5, 5 ), 12);
		public Point3D loc;
	 
	    #region Owner
		public Mobile m_Owner; 
				
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get{ return m_Owner; }
			set{ m_Owner = value; InvalidateProperties(); }
		}	
		#endregion
		
		private int m_Lifespan;
		public int Lifespan{ get{ return 18800; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int TimeLeft
		{
			get{ return m_Lifespan; }
			set{ m_Lifespan = value; InvalidateProperties(); }
		}
	
        [Constructable]
        public CorgulMap( Mobile owner )
        {
          	SetDisplay(1, 1, 4952, 4000, 400, 400);
			Name = "SoulBlinder Island Map";
			Hue = 2075;
			AddWorldPin( 3600, 760 ); 	
            m_Owner = owner;   		
            m_Lifespan = Lifespan;  			

            if ( m_Owner != null )
			{
				StartTimer();
			}  			
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			
			if ( Lifespan > 0 )
				list.Add( 1072517, m_Lifespan.ToString() ); // Lifespan: ~1_val~ seconds
		}
		
		public virtual void Slice()
		{
			m_Lifespan -= 10;
			
			InvalidateProperties();
			
			if ( m_Lifespan <= 0 )
				Decay();
		}
		
		public virtual void Decay()
		{
			if ( RootParent is Mobile )
			{
				Mobile parent = (Mobile) RootParent;
				
				if ( Name == null )
					parent.SendLocalizedMessage( 1072515, "#" + LabelNumber ); // The ~1_name~ expired...
				else
					parent.SendLocalizedMessage( 1072515, Name ); // The ~1_name~ expired...
					
				Effects.SendLocationParticles( EffectItem.Create( parent.Location, parent.Map, EffectItem.DefaultDuration ), 0x3728, 8, 20, 5042 );
				Effects.PlaySound( parent.Location, parent.Map, 0x201 );
			}
			else
			{
				Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x3728, 8, 20, 5042 );
				Effects.PlaySound( Location, Map, 0x201 );
			}			
			
			StopTimer();
			Delete();
		}
		
		private Timer m_Timer;	
        private Timer m_Timer2;			
		
		public virtual void StartTimer()
		{
			if ( m_Timer != null )
				return;
			
            if ( m_Timer2 != null )
				return;			
				
			m_Timer = Timer.DelayCall( TimeSpan.FromSeconds( 5 ), TimeSpan.FromSeconds( 5 ), new TimerCallback( FindOwnerLoc ) );
			m_Timer.Priority = TimerPriority.OneSecond;
			
			m_Timer2 = Timer.DelayCall( TimeSpan.FromSeconds( 10 ), TimeSpan.FromSeconds( 10 ), new TimerCallback( Slice ) );
			m_Timer2.Priority = TimerPriority.OneSecond;
		}
		
		public virtual void StopTimer()
		{
			if ( m_Timer != null )
				m_Timer.Stop();

			m_Timer = null;
			
			if ( m_Timer2 != null )
				m_Timer2.Stop();

			m_Timer2 = null;
		}

        public CorgulMap(Serial serial) : base(serial)
        {
        }
		
		public void FindOwnerLoc()
        {
		    if ( m_Owner == null )
			    return;		
				
			if (Owner.Map == Map.Trammel)	
				if (( Owner.Location.X > (CorgulIslandloc.X - 15) ) && ( Owner.Location.X < (CorgulIslandloc.X + 15) ))
				    if (( (CorgulIslandloc.Y + 15) > Owner.Location.Y ) && ( Owner.Location.Y > (CorgulIslandloc.Y - 15)))
						TeleportParty(Owner);	
		}
		
		public void TeleportParty( Mobile CorgulMapOwner )
	    {	
		    switch( Utility.Random( 4 ) )
			{
				case 0: loc = new Point3D(6447, 1156, 0); break;
				case 1: loc = new Point3D(6407, 1327, 0); break;
				case 2: loc = new Point3D(6354, 1247, 0); break;
				case 3: loc = new Point3D(6514, 1231, 0); break;
			} 	 
							
			foreach ( Item item in World.Items.Values )
			{
				if ( item is BaseBoat )
				{
				    BaseBoat boat = item as BaseBoat;
					
					if ( CorgulMapOwner.InRange( boat, 10 ) )
					{
					    boat.StopMove( false );
					    boat.Teleport(loc.X - boat.Location.X, loc.Y - boat.Location.Y, loc.Z - boat.Location.Z); 
                    }						
				}
			}	
            
           Corgul exo = new Corgul();
           exo.MoveToWorld( CorgulSpawn, Map.Trammel );

			/*
			foreach ( Corgul corg in World.Mobiles.Values )
			{
			    if (corg == null)
				{
					Corgul exo = new Corgul(); 
	        		exo.MoveToWorld( CorgulSpawn, Map.Trammel );
    			}
			}
            */
        }
		
		public virtual bool CanEnter( Mobile fighter )
		{
			return true;
		}
	
		public virtual bool CanEnter( BaseCreature pet )
		{
			return true;
		}
		
		public override void OnAfterDelete()
		{			
			StopTimer();
			Delete();
		}
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
			writer.Write( (int) m_Lifespan );
			writer.Write( (Mobile) m_Owner );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
			m_Lifespan = reader.ReadInt();
			m_Owner = reader.ReadMobile();
			
			StartTimer();
        }
    }
}