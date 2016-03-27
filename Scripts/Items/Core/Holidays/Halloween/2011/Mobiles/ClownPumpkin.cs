using System;
using Server;
using Server.Items;  

namespace Server.Mobiles
{
	[CorpseName( "a killer pumpkin corpse" )]
	public class ClownPumpkin : BaseCreature
	{
		public override bool IsScaredOfScaryThings{ get{ return false; } }
		public override bool IsScaryToPets{ get{ return true; } }
		
		[Constructable]
		public ClownPumpkin () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a killer pumpkin";
			Body = 1247;
			
			BaseSoundID = 412;

			SetStr( 412, 512 );
			SetDex( 96, 115 );
			SetInt( 966, 1045 );

			SetHits( 666, 777 );

			SetDamage( 20, 27 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Cold, 100 );

			SetResistance( ResistanceType.Physical, 70, 85 );
			SetResistance( ResistanceType.Fire, 40, 55 );
			SetResistance( ResistanceType.Cold, 90, 100 );
			SetResistance( ResistanceType.Poison, 65, 70 );
			SetResistance( ResistanceType.Energy, 50, 65 );

			SetSkill( SkillName.EvalInt, 120.1, 130.0 );
			SetSkill( SkillName.Magery, 120.1, 130.0 );
			SetSkill( SkillName.Meditation, 100.1, 101.0 );
			SetSkill( SkillName.Poisoning, 100.1, 101.0 );
			SetSkill( SkillName.MagicResist, 175.2, 200.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.Wrestling, 75.1, 100.0 );

			Fame = 23000;
			Karma = -23000;

			AddItem( new LightSource() );

			PackItem( new Bone( 3 ) );
			
			if ( Paragon.ChestChance > Utility.RandomDouble() )
				PackItem( new ParagonChest( Name, TreasureMapLevel ) );

		}
		
        public override int GetIdleSound()
		{
			return 0x19D;
		}

		public override int GetAngerSound()
		{
			return 0x175;
		}

		public override int GetDeathSound()
		{
			return 0x108;
		}

		public override int GetAttackSound()
		{
			return 0xE2;
		}

		public override int GetHurtSound()
		{
			return 0x28B;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 3 );
			AddLoot( LootPack.MedScrolls, 2 );
		}
		
		public override void OnDeath( Container c )
		{
			base.OnDeath( c );		
			
			if ( Utility.RandomDouble() < 0.25 )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0: c.DropItem( new PlagueMask() );	break;//1		
					case 1: c.DropItem( new ClownMask() ); break;//1
					case 2: c.DropItem( new DaemonMask() ); break;//2					
				}
			}
		}
		
        public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool BleedImmune { get { return true; } }
		public override int TreasureMapLevel { get { return 5; } }
		

		public ClownPumpkin( Serial serial ) : base( serial )
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
