using System;

namespace Server.Items
{
    public class MetalKeg : BaseContainer
    {
        public override int LabelNumber { get { return 1150675; } } // Metal Keg

        [Constructable]
        public MetalKeg()
            : base(0xE7F)
        {
            this.Weight = 15.0;
            Hue = 443;
        }

        public MetalKeg(Serial serial)
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