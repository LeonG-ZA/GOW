using System;

namespace Server.Items
{
    public class AncientMeerEternalStaff : BlackStaff
    {
        [Constructable]
        public AncientMeerEternalStaff()
            : base()
        {
            Hue = 0x49;
        }

        public override int LabelNumber { get { return 1153858; } }// Ancient Meer Eternal Staff

        public AncientMeerEternalStaff(Serial serial)
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