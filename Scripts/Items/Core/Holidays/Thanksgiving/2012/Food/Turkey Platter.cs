using System;

namespace Server.Items
{
    [FlipableAttribute(0x4988, 0x498F)]
    public class TurkeyPlatter : Item
    {
        int m_charges;

        [Constructable]
        public TurkeyPlatter()
            : base(0x4988)
        {
            this.Name = "turkey platter";
            this.Weight = 10.0;
            this.Stackable = false;
            this.m_charges = 8;
            this.Hue = Utility.RandomList(0x135, 0xcd, 0x38, 0x3b, 0x42, 0x4f, 0x11e, 0x60, 0x317, 0x10, 0x136, 0x1f9, 0x1a, 0xeb, 0x86, 0x2e);
        }

        public TurkeyPlatter(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            switch (Utility.Random(4))
            {
                case 0:
                    switch (Utility.Random(3))
                    {
                        case 0: from.AddToBackpack(new RoastChicken()); break;
                        case 1: from.AddToBackpack(new RoastDuck()); break;
                        case 2: from.AddToBackpack(new RoastTurkey()); break;
                    }
                    break;
                case 1: from.AddToBackpack(new GibletGravy()); break;
                case 2: from.AddToBackpack(new TurkeyDinner()); break;
                case 3: from.AddToBackpack(new TurkeyLeg()); break;
            }
            m_charges -= 1;
            this.InvalidateProperties();
            if (m_charges < 1)
                this.Delete();
        }


        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1060741, this.m_charges.ToString()); // charges: ~1_val~
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            this.LabelTo(from, 1060741, this.m_charges.ToString()); // charges: ~1_val~
        }
        
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(m_charges);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_charges = reader.ReadInt();
        }
    }

    public class TurkeyDinner : BaseFood
    {
        [Constructable]
        public TurkeyDinner()
            : base(1, 0x09AF)
        {
            this.Name = "turkey dinner";
            this.Weight = 1.0;
            this.FillFactor = 3;
            this.Stackable = false;
        }

        public TurkeyDinner(Serial serial)
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

    public class TurkeyLeg : BaseFood
    {
        [Constructable]
        public TurkeyLeg()
            : base(1, 0x1608)
        {
            this.Name = "turkey leg";
            this.Weight = 1.0;
            this.FillFactor = 1;
            this.Stackable = true;
        }

        public TurkeyLeg(Serial serial)
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

    public class GibletGravy : BaseFood
    {
        [Constructable]
        public GibletGravy()
            : base(1, 0x1602)
        {
            this.Name = "giblet gravy";
            this.Weight = 1.0;
            this.FillFactor = 2;
            this.Stackable = false;
        }

        public GibletGravy(Serial serial)
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

    public class RoastChicken : BaseFood
    {
        [Constructable]
        public RoastChicken()
            : this(1)
        {
        }

        [Constructable]
        public RoastChicken(int amount)
            : base(amount, 0x09B7)
        {
            this.Weight = 1.0;
            this.FillFactor = 3;
            this.Name = "roast chicken";
        }

        public RoastChicken(Serial serial)
            : base(serial)
        {
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

    public class RoastDuck : BaseFood
    {
        [Constructable]
        public RoastDuck()
            : this(1)
        {
        }

        [Constructable]
        public RoastDuck(int amount)
            : base(amount, 0x09B7)
        {
            this.Weight = 1.0;
            this.FillFactor = 3;
            this.Name = "roast duck";
        }

        public RoastDuck(Serial serial)
            : base(serial)
        {
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

    public class RoastTurkey : BaseFood
    {
        [Constructable]
        public RoastTurkey()
            : this(1)
        {
        }

        [Constructable]
        public RoastTurkey(int amount)
            : base(amount, 0x09B7)
        {
            this.Weight = 1.0;
            this.FillFactor = 3;
            this.Name = "roast turkey";
        }

        public RoastTurkey(Serial serial)
            : base(serial)
        {
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