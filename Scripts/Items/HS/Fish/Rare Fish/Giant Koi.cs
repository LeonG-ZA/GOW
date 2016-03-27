using System;

namespace Server.Items
{
    public class GiantKoi : BaseHighSeasFish
    {
        [Constructable]
        public GiantKoi()
            : base(0x44C5)
        {
        	Hue = 2114;
        }

        public GiantKoi(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116088;
            }
        }// giant koi

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