using System;

namespace Server.Items
{
    public class BlackMarlin : BaseHighSeasFish
    {
        [Constructable]
        public BlackMarlin()
            : base(0x4304)
        {
        	Hue = 2301;
        }

        public BlackMarlin(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116099;
            }
        }//black marlin

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