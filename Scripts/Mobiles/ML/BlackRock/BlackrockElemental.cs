using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server;
using Server.Misc;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName( "an blackrock elemental corpse" )]
    public class BlackrockElemental : AuraCreature
	{
		private DateTime m_Delay = DateTime.UtcNow;

		[Constructable]
		public BlackrockElemental() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.2 )
		{
			Name = "a blackrock elemental";
			Body = 14;
			BaseSoundID = 268;
			Hue = 1175;

			SetStr( 300 );
			SetDex( 185 );
			SetInt( 150 );

			SetStam( 185 );
			SetMana( 300 );

			SetHits( 2457 );

            //AuraMessage = "The intense cold is damaging you!";
            AuraType = ResistanceType.Energy;
            MinAuraDelay = 5;
            MaxAuraDelay = 15;
            MinAuraDamage = 15;
            MaxAuraDamage = 25;
            AuraRange = 5;

			SetDamage( 28 );

			SetDamageType( ResistanceType.Physical, 25 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Cold, 25 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 71 );
			SetResistance( ResistanceType.Fire, 54 );
			SetResistance( ResistanceType.Cold, 60 );
			SetResistance( ResistanceType.Poison, 56 );
			SetResistance( ResistanceType.Energy, 40 );

			SetSkill( SkillName.MagicResist, 120.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.Wrestling, 120.0 );
			SetSkill( SkillName.Anatomy, 120.0 );

			//Fame = 3500;
			//Karma = -3500;

			VirtualArmor = 34;

            switch (Utility.Random(1))
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

		public override bool BleedImmune{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 4; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from != null )
			{
				int hitback = damage;
				ItemAttributes.Damage( from, this, hitback, 100, 0, 0, 0, 0 );
			}
		}

		public BlackrockElemental( Serial serial ) : base( serial )
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
		}
	}
}