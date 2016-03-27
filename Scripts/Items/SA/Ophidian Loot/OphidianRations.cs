using System;
using System.Collections;
using Server.Network;
using Server.AllHues;

namespace Server.Items
{
    public class OphidianRation1 : BaseFood
    {
        [Constructable]
        public OphidianRation1() : this(1) { }

        [Constructable]
        public OphidianRation1(int amount)
            : base(amount, 0x9D1)
        {
            Name = "Ophidian Ration";
            Weight = 1.0;
            FillFactor = 1;
            Hue = AllHuesInfo.OphidianRatOrd;
        }

        public OphidianRation1(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class OphidianRation2 : BaseFood
    {
        [Constructable]
        public OphidianRation2() : this(1) { }

        [Constructable]
        public OphidianRation2(int amount)
            : base(amount, 0x9D2)
        {
            Name = "Ophidian Ration";
            Weight = 1.0;
            FillFactor = 1;
            Hue = AllHuesInfo.OphidianRatOrd;
        }

        public OphidianRation2(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class OphidianRation3 : BaseFood
    {
        [Constructable]
        public OphidianRation3() : this(1) { }

        [Constructable]
        public OphidianRation3(int amount)
            : base(amount, 0x9B7)
        {
            Name = "Ophidian Ration";
            Weight = 1.0;
            FillFactor = 5;
            Hue = AllHuesInfo.OphidianRatOrd;
        }

        public OphidianRation3(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }

        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class OphidianRation4 : BaseFood
    {
        [Constructable]
        public OphidianRation4() : this(1) { }

        [Constructable]
        public OphidianRation4(int amount)
            : base(amount, 0x9F2)
        {
            Name = "Ophidian Ration";
            Weight = 1.0;
            FillFactor = 5;
            Hue = AllHuesInfo.OphidianRatOrd;
        }

        public OphidianRation4(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }

        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class OphidianRation5 : BaseFood
    {
        [Constructable]
        public OphidianRation5()
            : this(1)
        {
        }

        [Constructable]
        public OphidianRation5(int amount)
            : base(amount, 0x9C9)
        {
            Name = "Ophidian Ration";
            Weight = 1.0;
            FillFactor = 5;
            Stackable = true;
            Amount = amount;
            Hue = AllHuesInfo.OphidianRatOrd;
        }

        public OphidianRation5(Serial serial)
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

    public class OphidianRation6 : BaseFood
    {
        [Constructable]
        public OphidianRation6()
            : this(1)
        {
        }

        [Constructable]
        public OphidianRation6(int amount)
            : base(amount, 0x979)
        {
            Name = "Ophidian Ration";
            Weight = 0.2;
            FillFactor = 1;
            Stackable = true;
            Amount = amount;
            Hue = AllHuesInfo.OphidianRatOrd;
        }

        public OphidianRation6(Serial serial)
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

    public class OphidianRation7 : BaseFood
    {
        [Constructable]
        public OphidianRation7() : this(1) { }
        [Constructable]
        public OphidianRation7(int amount)
            : base(amount, 0x9EB)
        {
            Weight = 1.0;
            FillFactor = 3;
            Hue = AllHuesInfo.OphidianRatOrd;
            Name = "Ophidian Ration";
        }
        public OphidianRation7(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }

    }

    public class OphidianRation8 : BaseFood
    {
        [Constructable]
        public OphidianRation8() : this(1) { }
        [Constructable]
        public OphidianRation8(int amount)
            : base(amount, 0x97D)
        {
            Name = "Ophidian Ration";
            Weight = 0.3;
            FillFactor = 9;
            Hue = AllHuesInfo.OphidianRatOrd;
        }
        public OphidianRation8(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }

    }

    public class OphidianRation9 : BaseFood
    {
        [Constructable]
        public OphidianRation9()
            : this(1)
        {
        }

        [Constructable]
        public OphidianRation9(int amount)
            : base(amount, 0x9D0)
        {
            FillFactor = 2;
            Hue = AllHuesInfo.OphidianRatOrd;
            Name = "Ophidian Ration";
        }

        public OphidianRation9(Serial serial)
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