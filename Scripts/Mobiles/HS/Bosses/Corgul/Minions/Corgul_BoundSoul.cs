using System;
using Server.Items;

namespace Server.Mobiles
{
    public class Corgul_BoundSoul : BaseCreature
    {
        [Constructable]
        public Corgul_BoundSoul()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
			Name = "Bound Soul";
            Hue = 0x4001;
            Body = 970;

            SetStr(172);
            SetDex(136);
            SetInt(73);
			
			SetHits( 419 );
			SetStam( 101 );
			SetMana( 100 );

            SetDamage(17, 22);

            SetSkill(SkillName.MagicResist, 102.0);
            SetSkill(SkillName.Tactics, 110.0);
            SetSkill(SkillName.Wrestling, 101.7);

			SetDamageType( ResistanceType.Physical, 10 );
			SetDamageType( ResistanceType.Cold, 30 );
			SetDamageType( ResistanceType.Poison, 30);
			SetDamageType( ResistanceType.Energy, 30);
			

			SetResistance( ResistanceType.Physical, 94 );
			SetResistance( ResistanceType.Cold, 30 );
			SetResistance( ResistanceType.Poison, 30 );
			SetResistance( ResistanceType.Energy, 30 );
			
            Fame = 1000;
            Karma = -1000;

        }

        public Corgul_BoundSoul(Serial serial)
            : base(serial)
        {
        }

        public override bool ClickTitle
        {
            get
            {
                return false;
            }
        }
        public override bool AlwaysMurderer
        {
            get
            {
                return true;
            }
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