using System;

namespace Server.Mobiles
{
    [CorpseName("an Archaeosaurus corpse")]
    public class Archaeosaurus : BaseCreature
    {
        [Constructable]
        public Archaeosaurus()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Archaeosaurus";
            Body = 1287;

            SetStr(400, 420);
            SetDex(280, 300);
            SetInt(203, 210);

            SetHits(200, 210);

            SetDamage(10, 14);

            SetDamageType(ResistanceType.Fire, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 25, 30);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 20, 25);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 35, 40);

            SetSkill(SkillName.MagicResist, 25.8, 30.0);
            SetSkill(SkillName.Tactics, 45.0, 50.0);
            SetSkill(SkillName.Wrestling, 34.3, 47.7);

            Fame = 18000;
            Karma = -18000;
        }

        public Archaeosaurus(Serial serial)
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