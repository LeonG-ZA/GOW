using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a bane dragon corpse" )]
	public class BaneDragon : BaseMount
	{
		[Constructable]
		public BaneDragon() : this( "a bane dragon" )
		{
		}
		public override bool SubdueBeforeTame{ get{ return true; } } // Must be beaten into submission

		[Constructable]
		public BaneDragon( string name ) : base( name, 0x31F, 0x3EBE, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Hue = 1175;
			
			SetStr( 550, 552 );
			SetDex( 88, 125 );
			SetInt( 88, 165 );

			SetHits( 558, 648 );

			SetDamage( 20, 26 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Cold, 25 );
			SetDamageType( ResistanceType.Poison, 25 );

			SetResistance( ResistanceType.Physical, 60, 70 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 20, 40 );

			SetSkill( SkillName.Magery, 27.3, 53.3 );
			SetSkill(SkillName.EvalInt, 26.3, 50.4);
			SetSkill( SkillName.MagicResist, 87.3, 97.9 );
			SetSkill( SkillName.Tactics, 107.6, 110.0 );
			SetSkill( SkillName.Wrestling, 92.6, 97.7 );

			Fame = 18000;
			Karma = -18000;

            PackItem(new CrystallineBlackrock(Utility.RandomMinMax(2, 5)));

			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 107.1;
		}

		public override double GetControlChance( Mobile m, bool useBaseSkill )
		{
			return 1.0;
		}
		
		public override int GetIdleSound()
		{
			return 0x2CE;
		}

		public override int GetDeathSound()
		{
			return 0x2CC;
		}

		public override int GetHurtSound()
		{
			return 0x2D1;
		}

		public override int GetAttackSound()
		{
			return 0x2C8;
		}

		public override bool AutoDispel{ get{ return !Controlled; } }
		public override bool HasBreath{ get{ return true; } } // fire breath enabled
        public override int BreathPoisonDamage{ get{ return 100; } }
		public override int BreathEffectHue{ get{ return 70; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }//Blackrock Stew
		public override int Meat{ get{ return 5; } }

		public BaneDragon( Serial serial ) : base( serial )
		{
		}
        /*
        public override bool IsEnemy(Mobile m) //override the is enemy, calling the mobile m being considered 
        {
            PlayerMobile pm = m as PlayerMobile;

            if (pm is PlayerMobile)
            {
                if (pm.BExp < 0)
                    return base.IsEnemy(m);
                else if (pm.BExp == 0)
                    return false; // false = not enemy
                else if (pm.BExp > 0)
                    return false; // false = not enemy
            }

            return base.IsEnemy(m);
        }
         */

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}