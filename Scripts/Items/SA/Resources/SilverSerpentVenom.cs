using System;

namespace Server.Items
{
    public class SilverSerpentVenom : Item
    {
        [Constructable]
        public SilverSerpentVenom()
            : this(1)
        {
        }

        [Constructable]
        public SilverSerpentVenom(int amount)
            : base(0xF82)
        {
            Weight = 1.0;
            Stackable = true;
            Amount = amount;
            Hue = 0x483;
        }

        public SilverSerpentVenom(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1112173;
            }
        }// silver serpent venom
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