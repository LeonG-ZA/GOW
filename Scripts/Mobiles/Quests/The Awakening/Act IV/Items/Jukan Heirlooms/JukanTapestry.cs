using System;

namespace Server.Items
{
    public class JukanTapestry : Item
    {
        [Constructable]
        public JukanTapestry()
            : base(0x2FF2)
        {
            Hue = 2708;
        }

        public override int LabelNumber { get { return 1153847; } }// Jukan Tapestry

        public JukanTapestry(Serial serial)
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