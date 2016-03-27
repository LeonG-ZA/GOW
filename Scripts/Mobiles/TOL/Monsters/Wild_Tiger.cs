using System;

namespace Server.Mobiles
{
    [CorpseName("a wild tiger corpse")]
    public class Wild_Tiger : BaseCreature
    {
        [Constructable]
        public Wild_Tiger()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a wild tiger";

            switch (Utility.Random(2))
            {
                case 0:
                    {
                        BodyValue = 1254;
                        break;
                    }
                case 1:
                    {
                        BodyValue = 1255;
                        break;
                    }

            }

            //Add a low chance of the tiger being a white tiger
            int hueValue = Utility.Random(500);

            if (hueValue <= 1)
            {
                Hue = 0x481;
            }

            SetStr(500, 555);
            SetDex(89, 123);
            SetInt(100, 159);

            SetHits(555, 650);

            SetDamage(20, 26);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Poison, 20);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire, 20, 40);
            SetResistance(ResistanceType.Cold, 20, 40);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.EvalInt, 15.2, 19.3);
            SetSkill(SkillName.Magery, 39.5, 49.5);
            SetSkill(SkillName.MagicResist, 91.4, 93.4);
            SetSkill(SkillName.Tactics, 108.1, 110.0);
            SetSkill(SkillName.Wrestling, 97.3, 98.2);

            Fame = 14000;
            Karma = -14000;

            VirtualArmor = 60;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 107.1;
        }

        public Wild_Tiger(Serial serial)
            : base(serial)
        {
        }

        public override int GetAngerSound()
        {
            return 0x518;
        }

        public override int GetIdleSound()
        {
            return 0x517;
        }

        public override int GetAttackSound()
        {
            return 0x516;
        }

        public override int GetHurtSound()
        {
            return 0x519;
        }

        public override int GetDeathSound()
        {
            return 0x515;
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

            if (this.BaseSoundID == 357)
                this.BaseSoundID = 0x451;
        }
    }
}