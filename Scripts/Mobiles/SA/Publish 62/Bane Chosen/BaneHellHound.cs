using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a bane hell hound corpse" )]
	public class BaneHellHound : BaseCreature
	{
		[Constructable]
		public BaneHellHound() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a hell hound";
			Body = 98;
			Hue = 1175;
			BaseSoundID = 229;

			SetStr( 202, 350 );
			SetDex( 181, 205 );
			SetInt( 36, 60 );

			SetHits( 266, 325 );

			SetDamage( 15, 20 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 80 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );
			
			SetSkill( SkillName.MagicResist, 85.1, 105.0 );
			SetSkill( SkillName.Tactics, 65.1, 80.0 );
			SetSkill( SkillName.Wrestling, 65.1, 80.0 );

			Fame = 7000;
			Karma = -7000;

			VirtualArmor = 30;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 95.5;

			PackItem( new SulfurousAsh( 5 ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.Meager );
		}

		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override int Meat{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Canine; } }

		public BaneHellHound( Serial serial ) : base( serial )
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
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
