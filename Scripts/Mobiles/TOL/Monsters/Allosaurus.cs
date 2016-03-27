using System;

namespace Server.Mobiles
{
    [CorpseName("an allosaurus corpse")]
    public class Allosaurus : BaseCreature
    {
        [Constructable]
        public Allosaurus()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an allosaurus";
            Body = 1290;

            SetStr(780, 800);
            SetDex(71, 80);
            SetInt(100, 130);

            SetHits(300, 325);

            SetDamage(18, 22);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 25, 30);
            SetResistance(ResistanceType.Cold, 35, 40);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 40, 45);

            SetSkill(SkillName.MagicResist, 67.8, 70.0);
            SetSkill(SkillName.Tactics, 100.0, 115.0);
            SetSkill(SkillName.Wrestling, 100.0, 115.0);
            SetSkill(SkillName.Poisoning, 67.8, 70.0);

            Fame = 18000;// not sure yet
            Karma = -18000;
        }

        public Allosaurus(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

        }
    }
}