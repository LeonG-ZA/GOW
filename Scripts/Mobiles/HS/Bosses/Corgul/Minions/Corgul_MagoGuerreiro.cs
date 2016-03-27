using System;
using Server.Items;

namespace Server.Mobiles
{
    public class Corgul_MagoGuerreiro : BaseCreature
    {
        [Constructable]
        public Corgul_MagoGuerreiro()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
			Name = "Soulbound Battle Mage";
            Body = 124;

            SetStr(156);
            SetDex(101);
            SetInt(181);
			
			SetHits( 419 );
			SetStam( 101 );
			SetMana( 619 );

            SetDamage(12, 17);

            SetSkill(SkillName.MagicResist, 83.6);
			SetSkill(SkillName.Magery, 91.7);
			SetSkill(SkillName.EvalInt, 81.9);
            SetSkill(SkillName.Tactics, 77.1);
            SetSkill(SkillName.Wrestling, 45.7);

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20);
			SetDamageType( ResistanceType.Poison, 20);
			SetDamageType( ResistanceType.Energy, 20);
			

			SetResistance( ResistanceType.Physical, 55 );
			SetResistance( ResistanceType.Fire, 54 );
			SetResistance( ResistanceType.Cold, 51 );
			SetResistance( ResistanceType.Poison, 50 );
			SetResistance( ResistanceType.Energy, 52 );
			
            Fame = 125;
            Karma = -1000;

        }

        public Corgul_MagoGuerreiro(Serial serial)
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