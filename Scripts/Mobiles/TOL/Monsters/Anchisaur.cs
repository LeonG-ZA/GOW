using System;

namespace Server.Mobiles
{
    [CorpseName("an Anchisaur corpse")]
    public class Anchisaur : BaseCreature
    {
        [Constructable]
        public Anchisaur()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Anchisaur";
            Body = 1292;

            SetStr(203, 260);
            SetDex(100, 110);
            SetInt(90, 100);

            SetHits(100, 150);

            SetDamage(6, 24);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 30, 35);
            SetResistance(ResistanceType.Cold, 15, 20);
            SetResistance(ResistanceType.Poison, 25, 30);
            SetResistance(ResistanceType.Energy, 20, 25);

            SetSkill(SkillName.MagicResist, 67.8, 70.0);
            SetSkill(SkillName.Tactics, 70.3, 75.7);
            SetSkill(SkillName.Wrestling, 70.3, 75.7);

            Fame = 18000;// Not Sure
            Karma = -18000;
        }

        public Anchisaur(Serial serial)
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