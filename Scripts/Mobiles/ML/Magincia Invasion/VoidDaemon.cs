using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a void daemon corpse" )]
	public class VoidDaemon : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.CrushingBlow;
		}

		[Constructable]
		public VoidDaemon() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a void daemon";
			Body = 792;
			BaseSoundID = 0x3E9;
			Hue = 1175;

			SetStr( 200, 210 );
			SetDex( 171, 200 );
			SetInt( 60, 80 );

			SetStam( 171, 200 );
			SetMana( 60, 80 );

			SetHits( 1145, 1150 );

			SetDamage( 30, 35 );

			SetDamageType( ResistanceType.Physical, 85 );
			SetDamageType( ResistanceType.Fire, 15 );

			SetResistance( ResistanceType.Physical, 50, 60 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 90, 100 );
			SetResistance( ResistanceType.Poison, 70, 80 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 70.1, 80.0 );
			SetSkill( SkillName.Wrestling, 95.1, 100.0 );

			Fame = 3000;
			Karma = -4000;

//todo barding difficulty 120

			VirtualArmor = 15;

			if ( 0.2 > Utility.RandomDouble() )
				PackItem( new VorpalBlade() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Meager );
		}

		public VoidDaemon( Serial serial ) : base( serial )
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