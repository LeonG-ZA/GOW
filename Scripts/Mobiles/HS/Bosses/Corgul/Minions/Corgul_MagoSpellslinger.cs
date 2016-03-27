using System;
using Server.Items;

namespace Server.Mobiles
{
    public class Corgul_MagoSpellslinger : BaseCreature
    {
        [Constructable]
        public Corgul_MagoSpellslinger()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
			Name = "Soulbound Spellslinger";
            Body = 124;

            SetStr(129);
            SetDex(96);
            SetInt(127);
			
			SetHits( 192 );
			SetStam( 96 );
			SetMana( 440 );

            SetDamage(8, 12);

            SetSkill(SkillName.MagicResist, 97.0);
			SetSkill(SkillName.Magery, 100.3);
			SetSkill(SkillName.EvalInt, 83.3);
            SetSkill(SkillName.Tactics, 86.3);
            SetSkill(SkillName.Wrestling, 95.6);

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 32 );
			SetResistance( ResistanceType.Fire, 34 );
			SetResistance( ResistanceType.Cold, 32 );
			SetResistance( ResistanceType.Poison, 32 );
			SetResistance( ResistanceType.Energy, 32 );
			
            Fame = 2540;
            Karma = -1000;

        }

        public Corgul_MagoSpellslinger(Serial serial)
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