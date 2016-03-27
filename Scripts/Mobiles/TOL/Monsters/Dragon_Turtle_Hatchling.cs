using System;

namespace Server.Mobiles
{
    [CorpseName("a dragon turtle hatchling corpse")]
    public class Dragon_Turtle_Hatchling : BaseCreature
    {
        [Constructable]
        public Dragon_Turtle_Hatchling()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a dragon turtle hatchling";
            Body = 1294;

            SetStr(1025, 1425);
            SetDex(81, 148);
            SetInt(475, 675);

            SetHits(1000, 2000);
            SetStam(120, 135);

            SetDamage(24, 33);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 85);
            SetResistance(ResistanceType.Fire, 65, 90);
            SetResistance(ResistanceType.Cold, 40, 55);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 50, 75);

            SetSkill(SkillName.Meditation, 0);
            SetSkill(SkillName.EvalInt, 110.0, 140.0);
            SetSkill(SkillName.Magery, 110.0, 140.0);
            SetSkill(SkillName.Poisoning, 0);
            SetSkill(SkillName.Anatomy, 0);
            SetSkill(SkillName.MagicResist, 110.0, 140.0);
            SetSkill(SkillName.Tactics, 110.0, 140.0);
            SetSkill(SkillName.Wrestling, 115.0, 145.0);

            Fame = 22000;
            Karma = -15000;

            VirtualArmor = 60;

            Tamable = true;
            ControlSlots = 5;
            MinTameSkill = 104.7;
        }

        public Dragon_Turtle_Hatchling(Serial serial)
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