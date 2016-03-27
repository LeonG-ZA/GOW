using System;
using Server.Items;

namespace Server.Mobiles
{
    public class CharybdisTentacles : BaseCreature
    {
        [Constructable]
        public CharybdisTentacles()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
			Name = "Giant Tentacle";
            Body = 1245;
			BaseSoundID = 0x669;

            SetStr(127, 155);
            SetDex(65, 85);
            SetInt(102, 123);
			
			SetHits( 105, 113 );
			SetStam( 66, 85 );
			SetMana( 102, 123 );

            SetDamage(10, 15);

            SetSkill(SkillName.MagicResist, 100.4, 113.5);
			SetSkill(SkillName.Magery, 60.2, 72.4);
			SetSkill( SkillName.EvalInt, 60.1, 73.4 );
            SetSkill(SkillName.Wrestling, 52.1, 70.0);

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Cold, 50);	

			SetResistance( ResistanceType.Physical, 32, 45 );
			SetResistance( ResistanceType.Fire, 10, 25 );
			SetResistance( ResistanceType.Cold, 10, 25 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 5, 10 );
			
            Fame = 2000;
            Karma = -1000;

        }

        public CharybdisTentacles(Serial serial)
            : base(serial)
        {
        }
		
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}