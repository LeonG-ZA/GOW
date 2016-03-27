using System;

namespace Server.Mobiles
{
    [CorpseName("a turkey corpse")]
    public class Turkey : BaseCreature
    {
        [Constructable]
        public Turkey()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            this.Name = "a turkey";
            this.Body = 95;
            this.BaseSoundID = 1642;

            this.SetStr(13, 14);
            this.SetDex(13, 20);
            this.SetInt(5, 15);

            this.SetHits(13, 14);
            this.SetMana(5, 15);
            this.SetStam(13, 20);

            this.SetDamage(1);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 5, 7);
            this.SetResistance(ResistanceType.Fire, 7, 9);
            this.SetResistance(ResistanceType.Cold, 3, 4);
            this.SetResistance(ResistanceType.Poison, 4, 5);
            this.SetResistance(ResistanceType.Energy, 4, 5);

            this.SetSkill(SkillName.MagicResist, 8.7);
            this.SetSkill(SkillName.Tactics, 10.1);
            this.SetSkill(SkillName.Wrestling, 11.1);
            this.SetSkill(SkillName.Anatomy, 69.4);

            //this.Fame = 150;
            this.Karma = 0;

            this.VirtualArmor = 2;

            this.Tamable = true;
            this.ControlSlots = 1;
            this.MinTameSkill = 0.0;
        }

        public Turkey(Serial serial)
            : base(serial)
        {
        }

        public override int Meat
        {
            get
            {
                return 2;
            }
        }
        public override MeatType MeatType
        {
            get
            {
                return MeatType.Bird;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.GrainsAndHay;
            }
        }
        public override bool CanFly
        {
            get
            {
                return true;
            }
        }
        public override int Feathers
        {
            get
            {
                return 75;
            }
        }
        public override int GetAngerSound()
        {
            return 0x66B;
        }

        public override int GetIdleSound()
        {
            return 0x66A;
        }

        public override int GetAttackSound()
        {
            return 0x66B;
        }

        public override int GetHurtSound()
        {
            return 0x66B;
        }

        public override int GetDeathSound()
        {
            return 0x072;
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

    [CorpseName("a turkey corpse")]
    public class MisterGobbles : BaseCreature
    {
        [Constructable]
        public MisterGobbles()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            this.Name = "Mister Gobbles";
            this.Body = 95;
            this.BaseSoundID = 1642;

            this.SetStr(15, 20);
            this.SetDex(15, 25);
            this.SetInt(7, 17);

            this.SetHits(15, 20);
            this.SetMana(7, 17);
            this.SetStam(15, 25);

            this.SetDamage(3);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 5, 7);
            this.SetResistance(ResistanceType.Fire, 7, 9);
            this.SetResistance(ResistanceType.Cold, 3, 4);
            this.SetResistance(ResistanceType.Poison, 4, 5);
            this.SetResistance(ResistanceType.Energy, 4, 5);

            this.SetSkill(SkillName.MagicResist, 8.7);
            this.SetSkill(SkillName.Tactics, 10.1);
            this.SetSkill(SkillName.Wrestling, 11.1);
            this.SetSkill(SkillName.Anatomy, 69.4);

            //this.Fame = 150;
            this.Karma = 0;

            this.VirtualArmor = 2;

            this.Tamable = true;
            this.ControlSlots = 1;
            this.MinTameSkill = 0.0;
        }

        public MisterGobbles(Serial serial)
            : base(serial)
        {
        }

        public override int Meat
        {
            get
            {
                return 2;
            }
        }
        public override MeatType MeatType
        {
            get
            {
                return MeatType.Bird;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.GrainsAndHay;
            }
        }
        public override bool CanFly
        {
            get
            {
                return true;
            }
        }
        public override int Feathers
        {
            get
            {
                return 75;
            }
        }
        public override int GetAngerSound()
        {
            return 1643;
        }

        public override int GetIdleSound()
        {
            return 1642;
        }

        public override int GetAttackSound()
        {
            return 1643;
        }

        public override int GetHurtSound()
        {
            return 1643;
        }

        public override int GetDeathSound()
        {
            return 0x072;
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