using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Misc;
using Server.Spells;
using Server.Spells.Third;
using Server.Spells.Sixth;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a virtuebane corpse" )]
    public class Virtuebane : BaseCreature
	{		
		public virtual int StrikingRange{ get{ return 12; } }
	
		[Constructable]
		public Virtuebane() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 1071 ;
            Name = "Virtuebane";
			BaseSoundID = 357;

			SetStr( 812, 999 );
			SetDex( 507, 669 );
			SetInt( 1206, 1389 );

			SetHits( 50000 );
			SetStam( 507, 669 );
			SetMana( 1206, 1389 );

			SetDamage( 25, 30 );

			SetDamageType( ResistanceType.Physical, 25 );
			SetDamageType( ResistanceType.Fire, 75 );

			SetResistance( ResistanceType.Physical, 40, 53 );
			SetResistance( ResistanceType.Fire, 67, 73 );
			SetResistance( ResistanceType.Cold, 50, 62 );
			SetResistance( ResistanceType.Poison, 60, 73 );
			SetResistance( ResistanceType.Energy, 60, 73 );

			SetSkill( SkillName.Wrestling, 90.0 );
			SetSkill( SkillName.Tactics, 90.0 );
			SetSkill( SkillName.MagicResist, 110.0 );
			SetSkill( SkillName.Poisoning, 120.0 );
			SetSkill( SkillName.Magery, 110.0 );
			SetSkill( SkillName.EvalInt, 110.0 );
			SetSkill( SkillName.Meditation, 110.0 );
			
			
			//PackResources( 8 );
			//PackTalismans( 5 );
			
			Fame = 24000;
			Karma = -24000;
			
			m_Change = DateTime.UtcNow;
			m_Stomp = DateTime.UtcNow;
		}
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.AosSuperBoss, 8 );
		}	
		
		public override void OnThink()
		{
			base.OnThink();
			
			if ( Combatant != null )
			{
				if ( m_Change < DateTime.UtcNow && Utility.RandomDouble() < 0.2 )
					ChangeOpponent();					
				
				if ( m_Stomp < DateTime.UtcNow && Utility.RandomDouble() < 0.1 )
					HoofStomp();
			}
				// exit ilsh 1313, 936, 32
		}
		
		public override void Damage( int amount, Mobile from )
		{
			base.Damage( amount, from );
						
			if ( Combatant == null || Hits > HitsMax * 0.05 || Utility.RandomDouble() > 0.1 )
				return;	
							
			new InvisibilitySpell( this, null ).Cast();
			
			Target target = Target;
			
			if ( target != null )
				target.Invoke( this, this );
				
			Timer.DelayCall( TimeSpan.FromSeconds( 2 ), new TimerCallback( Teleport ) );
		}
		
		public override void OnDeath( Container c )
		{
			base.OnDeath( c );			
			
			if ( Utility.RandomDouble() < 0.6 )
				c.DropItem( new VEEarrings() );
				
			if ( Utility.RandomDouble() < 0.50 )
			{
				switch ( Utility.Random( 6 ) )
				{
					case 0: c.DropItem( new VEPlateArms() ); break;
					case 1: c.DropItem( new VEPlateChest() ); break;
					case 2: c.DropItem( new VEPlateLegs() ); break;
					case 3: c.DropItem( new VEPlateHelm() ); break;
					case 4: c.DropItem( new VEPlateGorget() ); break;
					case 5: c.DropItem( new VEPlateGloves() ); break;
				}
			}		
		}

		public override int Meat{ get{ return 5; } }
		public override MeatType MeatType{ get{ return MeatType.Ribs; } }

        public override bool GivesMLMinorArtifact { get { return true; } }
        public override bool Unprovokable{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }		
		public override Poison HitPoison{ get{ return Poison.Deadly; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public Virtuebane( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
			
			m_Change = DateTime.UtcNow;
			m_Stomp = DateTime.UtcNow;
		}
		
		private DateTime m_Change;
		private DateTime m_Stomp;
		
		public void Teleport()
		{										
			// 20 tries to teleport
			for ( int tries = 0; tries < 20; tries ++ )
			{
				int x = Utility.RandomMinMax( 5, 7 ); 
				int y = Utility.RandomMinMax( 5, 7 );
				
				if ( Utility.RandomBool() )
					x *= -1;
					
				if ( Utility.RandomBool() )
					y *= -1;
				
				Point3D p = new Point3D( X + x, Y + y, 0 );
				IPoint3D po = new LandTarget( p, Map ) as IPoint3D;
				
				if ( po == null )
					continue;
					
				SpellHelper.GetSurfaceTop( ref po );

				if ( InRange( p, 12 ) && InLOS( p ) && Map.CanSpawnMobile( po.X, po.Y, po.Z ) )
				{					
					
					Point3D from = Location;
					Point3D to = new Point3D( po );
	
					Location = to;
					ProcessDelta();
					
					FixedParticles( 0x376A, 9, 32, 0x13AF, EffectLayer.Waist );
					PlaySound( 0x1FE );
										
					break;					
				}
			}		
			
			RevealingAction();
		}
		
		public void ChangeOpponent()
		{
			Mobile agro, best = null;
			double distance, random = Utility.RandomDouble();
			
			if ( random < 0.75 )
			{			
				// find random target relatively close
				for ( int i = 0; i < Aggressors.Count && best == null; i ++ )
				{
					agro = Validate( Aggressors[ i ].Attacker );
					
					if ( agro == null )
						continue;				
				
					distance = StrikingRange - GetDistanceToSqrt( agro );
					
					if ( distance > 0 && distance < StrikingRange - 2 && InLOS( agro.Location ) )
					{
						distance /= StrikingRange;
						
						if ( random < distance )
							best = agro;
					}
				}		
			}	
			else
			{
				int damage = 0;
				
				// find a player who dealt most damage
				for ( int i = 0; i < DamageEntries.Count; i ++ )
				{
					agro = Validate( DamageEntries[ i ].Damager );
					
					if ( agro == null )
						continue;
					
					distance = GetDistanceToSqrt( agro );
						
					if ( distance < StrikingRange && DamageEntries[ i ].DamageGiven > damage && InLOS( agro.Location ) )
					{
						best = agro;
						damage = DamageEntries[ i ].DamageGiven;
					}
				}
			}
			
			if ( best != null )
			{
				// teleport
                //best.Location = AltarPeerless.GetSpawnPosition(Location, Map, 1);
				best.FixedParticles( 0x376A, 9, 32, 0x13AF, EffectLayer.Waist );
				best.PlaySound( 0x1FE );
				
				Timer.DelayCall( TimeSpan.FromSeconds( 1 ), new TimerCallback( delegate()
				{
					// poison
					best.ApplyPoison( this, HitPoison );
					best.FixedParticles( 0x374A, 10, 15, 5021, EffectLayer.Waist );
					best.PlaySound( 0x474 );
				} ) );
				
				m_Change = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( 5, 10 ) );
			}
		}
		
		public void HoofStomp()
		{		
			foreach ( Mobile m in GetMobilesInRange( StrikingRange ) )
			{
				Mobile valid = Validate( m );
				
				if ( valid != null && Affect( valid ) )
					valid.SendMessage( "*The Virtuebane slams the ground with a thunderous blow!*" ); 
			}		
			
			// earthquake
			PlaySound( 0x2F3 );
				
			Timer.DelayCall( TimeSpan.FromSeconds( 30 ), new TimerCallback( delegate{ StopAffecting(); } ) );
						
			m_Stomp = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( 40, 50 ) );
		}
		
		public Mobile Validate( Mobile m )
		{			
			Mobile agro;
					
			if ( m is BaseCreature )
				agro = ( (BaseCreature) m ).ControlMaster;
			else
				agro = m;
			
			if ( !CanBeHarmful( agro, false ) || !agro.Player || Combatant == agro )
				return null;	
			
			return agro;
		}
		
		private static Dictionary<Mobile,bool> m_Affected;
		
		public static bool IsUnderInfluence( Mobile mobile )
		{
			if ( m_Affected != null )
			{
				if ( m_Affected.ContainsKey( mobile ) )
					return true;
			}
			
			return false;
		}
		
		public static bool Affect( Mobile mobile )
		{
			if ( m_Affected == null )
				m_Affected = new Dictionary<Mobile,bool>();
				
			if ( !m_Affected.ContainsKey( mobile ) )
			{
				m_Affected.Add( mobile, true );
				return true;
			}
			
			return false;
		}
		
		public static void StopAffecting()
		{
			if ( m_Affected != null )
				m_Affected.Clear();
		}
	}
}
