using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a dryad corpse" )]
	public class DryadA : BaseCreature
	{
		public override bool InitialInnocent{ get{ return true; } }

		[Constructable]
		public DryadA() : base( AIType.AI_Mage, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Name = "a dryad";
			Body = 266;

			SetStr( 130, 150 ); // TODO: All Values are taken from Stratics and will change along with the info taken from stratics/OSI.
			SetDex( 150, 170 );
			SetInt( 250, 275 );

			SetHits( 310, 320 );

			SetDamage( 11, 20 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 40, 45 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 25, 35 );

			SetSkill( SkillName.Meditation, 80, 90 );
			SetSkill( SkillName.EvalInt, 70, 80 );
			SetSkill( SkillName.Magery, 70, 80 );
			SetSkill( SkillName.MagicResist, 110, 120 );
			SetSkill( SkillName.Tactics, 70, 80 );
			SetSkill( SkillName.Wrestling, 70, 80 );

			Fame = 1250;
			Karma = 1250;

			//PackSpellweavingScroll();

			if ( Utility.RandomDouble() < .33 )
                PackItem(Engines.Plants.Seed.RandomPeculiarSeed(1));
		}

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }
		
		public override int Meat{ get{ return 1; } }	
        public override int GetDeathSound() { return 0x57A; }
        public override int GetAttackSound() { return 0x57B; }
        public override int GetIdleSound() { return 0x57C; }
        public override int GetAngerSound() { return 0x57D; }
        public override int GetHurtSound() { return 0x57E; }
		
		public DryadA( Serial serial ) : base( serial )
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
		}
	}
}
