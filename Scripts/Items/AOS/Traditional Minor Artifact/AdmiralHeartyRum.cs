using System;
using Server;

namespace Server.Items
{
    public class AdmiralHeartyRum : BeverageBottle
    {
        public override int LabelNumber { get { return 1063477; } } // The Admiral's Hearty Rum

        [Constructable]
        public AdmiralHeartyRum()
            : base(BeverageType.Wine)
        {
            Hue = 0x66C;
        }

        public AdmiralHeartyRum(Serial serial)
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
