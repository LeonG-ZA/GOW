using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Targeting;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
    [CorpseName("a Clan Chitter Assistant corpse")]
	public class ClanChitterAssistant : BaseCreature
	{
		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Ratman; } }

		[Constructable]
		public ClanChitterAssistant() : base( AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Clan Chitter Assistant";
			Body = 0x8E;
			BaseSoundID = 437;

			SetStr( 146, 180 );
			SetDex( 101, 130 );
			SetInt( 116, 140 );

			SetHits( 120, 150 );

			SetDamage( 4, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 23, 34 );
			SetResistance( ResistanceType.Fire, 22, 27 );
			SetResistance( ResistanceType.Cold, 33, 49 );
			SetResistance( ResistanceType.Poison, 15, 19 );
			SetResistance( ResistanceType.Energy, 10, 19 );

            SetSkill(SkillName.Anatomy, 0.0, 0.0);
			SetSkill( SkillName.MagicResist, 81.1, 90.0 );
			SetSkill( SkillName.Tactics, 53.1, 75.0 );
			SetSkill( SkillName.Wrestling, 62.1, 75.0 );

			Fame = 6500;
			Karma = -6500;

			VirtualArmor = 56;

			AddItem( new Bow() );
			PackItem( new Arrow( Utility.RandomMinMax( 50, 70 ) ) );
		}

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);

            if (0.1 > Utility.RandomDouble())
                AddLoot(LootPack.CavernIngredients);
        }

        public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
        public override int LoyaltyPointsAward { get { return 2; } }

        public override bool CanRummageCorpses { get { return true; } }
        public override int Meat { get { return 1; } }
        public override int Hides { get { return 8; } }
        public override int TreasureMapLevel { get { return 2; } }

		public ClanChitterAssistant( Serial serial ) : base( serial )
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

			if ( Body == 42 )
			{
				Body = 0x8E;
				Hue = 0;
			}
		}
	}
}
