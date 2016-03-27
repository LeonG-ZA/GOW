using System;
using Server.Items;

namespace Server.Mobiles
{
    public class Corgul_MagoAprendiz : BaseCreature
    {
        [Constructable]
        public Corgul_MagoAprendiz()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
			Name = "Soulbound Apprentice Mage";
            Body = 124;

            SetStr(172);
            SetDex(136);
            SetInt(73);
			
			SetHits( 320 );
			SetStam( 101 );
			SetMana( 619 );

            SetDamage(10, 15);

            SetSkill(SkillName.MagicResist, 83.6);
			SetSkill(SkillName.Magery, 91.7);
			SetSkill(SkillName.EvalInt, 81.9);
            SetSkill(SkillName.Tactics, 77.1);
            SetSkill(SkillName.Wrestling, 45.7);

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

        public Corgul_MagoAprendiz(Serial serial)
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