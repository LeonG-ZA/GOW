using System;

namespace Server.Items
{
    [FlipableAttribute(0x4BAB, 0x4BAC)]
    public class BasketOfRolls : Item
    {
        int m_charges;

        [Constructable]
        public BasketOfRolls()
            : base(0x4BAB)
        {
            Weight = 3.0;
            Stackable = false;
            m_charges = 13;
        }

        public BasketOfRolls(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber { get { return 1153518; } }//basket of rolls

        public override void OnDoubleClick(Mobile from)
        {
            if (m_charges > 0)
            {
                from.AddToBackpack(new DinnerRoll());
                m_charges -= 1;
                InvalidateProperties();
                if (m_charges == 0)
                    Delete();
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1060741, m_charges.ToString()); // charges: ~1_val~
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            LabelTo(from, 1060741, m_charges.ToString()); // charges: ~1_val~
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

    public class DinnerRoll : BaseFood
    {
        [Constructable]
        public DinnerRoll()
            : base(1, 0x09EA)
        {
            Weight = 1.0;
            FillFactor = 1;
            Stackable = true;
        }

        public DinnerRoll(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber { get { return 1153520; } }//dinner roll

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