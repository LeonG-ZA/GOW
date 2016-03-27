using System;

namespace Server.Items
{
    public class SweetPotatoPie : Item
    {
        int m_charges;

        [Constructable]
        public SweetPotatoPie()
            : base(0x4C0D)
        {
            this.Name = "sweet potato pie";
            this.Weight = 1.0;
            this.Stackable = false;
            this.m_charges = 6;
        }

        public SweetPotatoPie(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (m_charges > 0)
            {
                switch (Utility.Random(2))
                {
                    case 0: from.AddToBackpack(new SweetPotatoPieSliceS()); break;
                    case 1: from.AddToBackpack(new SweetPotatoPieSliceE()); break;
                }
                this.ItemID = 0x4C02;
                m_charges -= 1;
                this.InvalidateProperties();
                if (m_charges == 0)
                    this.Delete();
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1153151, this.m_charges.ToString()); // charges: ~1_val~
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            this.LabelTo(from, 1153151, this.m_charges.ToString()); // charges: ~1_val~
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

    public class SweetPotatoPieSliceS : BaseFood
    {
        [Constructable]
        public SweetPotatoPieSliceS()
            : base(1, 0x4C03)
        {
            this.Name = "slice of pie";
            this.Weight = 1.0;
            this.FillFactor = 1;
            this.Stackable = false;
            //this.Hue = 32;
        }

        public SweetPotatoPieSliceS(Serial serial)
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

    public class SweetPotatoPieSliceE : BaseFood
    {
        [Constructable]
        public SweetPotatoPieSliceE()
            : base(1, 0x4C04)
        {
            this.Name = "slice of pie";
            this.Weight = 1.0;
            this.FillFactor = 1;
            this.Stackable = false;
            //this.Hue = 32;
        }

        public SweetPotatoPieSliceE(Serial serial)
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