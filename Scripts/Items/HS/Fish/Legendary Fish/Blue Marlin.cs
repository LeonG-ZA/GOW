using System;

namespace Server.Items
{
    public class BlueMarlin : BaseHighSeasFish
    {
        [Constructable]
        public BlueMarlin()
            : base(0x4304)
        {
        	Hue = 1927;
        }

        public BlueMarlin(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116097;
            }
        }//blue marlin

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