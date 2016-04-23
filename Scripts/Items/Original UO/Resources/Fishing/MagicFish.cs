namespace Server.Items
{
    public class PrizedFish : BaseMagicFish
    {
        [Constructable]
        public PrizedFish()
            : base(51)
        {
        }

        public PrizedFish(Serial serial)
            : base(serial)
        {
        }

        public override int Bonus
        {
            get
            {
                return 5;
            }
        }
        public override StatType Type
        {
            get
            {
                return StatType.Int;
            }
        }
        public override int LabelNumber
        {
            get
            {
                return 1041073;
            }
        }// prized fish
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (Hue == 151)
            {
                Hue = 51;
            }
        }
    }

    public class WondrousFish : BaseMagicFish
    {
        [Constructable]
        public WondrousFish()
            : base(86)
        {
        }

        public WondrousFish(Serial serial)
            : base(serial)
        {
        }

        public override int Bonus
        {
            get
            {
                return 5;
            }
        }
        public override StatType Type
        {
            get
            {
                return StatType.Dex;
            }
        }
        public override int LabelNumber
        {
            get
            {
                return 1041074;
            }
        }// wondrous fish
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (Hue == 286)
            {
                Hue = 86;
            }
        }
    }

    public class TrulyRareFish : BaseMagicFish
    {
        [Constructable]
        public TrulyRareFish()
            : base(76)
        {
        }

        public TrulyRareFish(Serial serial)
            : base(serial)
        {
        }

        public override int Bonus
        {
            get
            {
                return 5;
            }
        }
        public override StatType Type
        {
            get
            {
                return StatType.Str;
            }
        }
        public override int LabelNumber
        {
            get
            {
                return 1041075;
            }
        }// truly rare fish
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (Hue == 376)
            {
                Hue = 76;
            }
        }
    }

    public class PeculiarFish : BaseMagicFish
    {
        [Constructable]
        public PeculiarFish()
            : base(66)
        {
        }

        public PeculiarFish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041076;
            }
        }// highly peculiar fish
        public override bool Apply(Mobile from)
        {
            from.Stam += 10;
            return true;
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

            if (Hue == 266)
            {
                Hue = 66;
            }
        }
    }
}