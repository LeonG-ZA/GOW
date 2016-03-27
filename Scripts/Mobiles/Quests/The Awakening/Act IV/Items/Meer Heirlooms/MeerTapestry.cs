using System;

namespace Server.Items
{
    public class MeerTapestry : Item
    {
        [Constructable]
        public MeerTapestry()
            : base(0x2FF3)
        {
            Hue = 2019;
        }

        public override int LabelNumber { get { return 1153857; } }// Meer Tapestry

        public MeerTapestry(Serial serial)
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