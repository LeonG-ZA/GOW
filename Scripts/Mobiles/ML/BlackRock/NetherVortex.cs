using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server;
using Server.Misc;
using Server.Spells;
namespace Server.Mobiles
{
	[CorpseName( "a nether vortex corpse" )]
	public class NetherVortex : BaseCreature
	{
		private DateTime m_Delay = DateTime.UtcNow;

		[Constructable]
		public NetherVortex () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a nether vortex";
			Body = 13;
			Hue = 1175;
			BaseSoundID = 263;

			SetStr( 150 );
			SetDex( 185 );
			SetInt( 300 );

			SetStam( 185 );
			SetMana( 300 );

			SetHits( 2097 );

			SetDamage( 20 );

			SetDamageType( ResistanceType.Physical, 10 );
			SetDamageType( ResistanceType.Cold, 25 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Poison, 10 );
			SetDamageType( ResistanceType.Energy, 30 );

			SetResistance( ResistanceType.Physical, 71 );
			SetResistance( ResistanceType.Fire, 60 );
			SetResistance( ResistanceType.Cold, 55 );
			SetResistance( ResistanceType.Poison, 57 );
			SetResistance( ResistanceType.Energy, 50 );

			SetSkill( SkillName.EvalInt, 120.0 );
			SetSkill( SkillName.MagicResist, 120.0 );
			SetSkill( SkillName.Tactics, 96.0 );
			SetSkill( SkillName.Wrestling, 81.9 );
			SetSkill( SkillName.Meditation, 120.0 );

			//Fame = 4500;
			//Karma = -4500;

			switch ( Utility.Random( 1 ))
        	    		{
                            case 0: AddItem(new RandomBlackRock()); break;       	
				}
		}

		public override void GenerateLoot()
		{
		}

		private DateTime m_NextAbilityTime;

		private void DoAreaLeech()
		{
			m_NextAbilityTime += TimeSpan.FromSeconds( 2.5 );

			this.FixedParticles( 0x376A, 10, 10, 9537, 33, 0, EffectLayer.Waist );

			Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerCallback( DoAreaLeech_Finish ) );
		}

		private void DoAreaLeech_Finish()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 6 ) )
			{
				if ( this.CanBeHarmful( m ) && this.IsEnemy( m ) )
					list.Add( m );
			}

			{
				double scalar;

				if ( list.Count == 1 )
					scalar = 0.90;
				else if ( list.Count == 2 )
					scalar = 0.50;
				else
					scalar = 0.30;

				for ( int i = 0; i < list.Count; ++i )
				{
					Mobile m = (Mobile)list[i];

					int damage = (int)(m.Hits * scalar);

					damage += Utility.RandomMinMax( -5, 5 );

					if ( damage < 1 )
						damage = 1;

					m.MovingParticles( this, 0x36F4, 1, 0, false, false, 32, 0, 9535,    1, 0, (EffectLayer)255, 0x100 );
					m.MovingParticles( this, 0x0001, 1, 0, false,  true, 32, 0, 9535, 9536, 0, (EffectLayer)255, 0 );

					this.DoHarmful( m );
					this.Hits += ItemAttributes.Damage( m, this, damage, 100, 0, 0, 0, 0 );
				}
			}
		}

		public override void OnThink()
		{
			if ( DateTime.UtcNow >= m_NextAbilityTime )
			{
				Mobile combatant = this.Combatant;

				if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 12 ) )
				{
					m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( 10, 15 ) );

					int ability = Utility.Random( 4 );

					switch ( ability )
					{
						case 1: DoAreaLeech(); break;
					}
				}
			}

			if ( DateTime.UtcNow > m_Delay )
			{
				Ability.Aura( this, 10, 20, 2, 3, 0, "The radiating energy emitted from the creature is damaging you!" );
				m_Delay = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( 5, 10 ) );
			}

			base.OnThink();
		}

		public override bool BleedImmune{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 4; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override bool HasBreath{ get{ return true; } }
		public override int BreathEffectSpeed{ get{ return 1; } }
		public override int BreathEffectHue{ get{ return 1175; } }
		public override int BreathEffectSound{ get{ return 0x1CC; } }
        public override int BreathPhysicalDamage { get { return 10; } }
        public override int BreathFireDamage { get { return 25; } }
        public override int BreathColdDamage { get { return 25; } }
        public override int BreathPoisonDamage { get { return 10; } }
        public override int BreathEnergyDamage { get { return 30; } }

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from != null )
			{
				int hitback = damage;
				ItemAttributes.Damage( from, this, hitback, 100, 0, 0, 0, 0 );
			}
		}

		public NetherVortex( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( BaseSoundID == 263 )
				BaseSoundID = 655;
		}
	}
}
