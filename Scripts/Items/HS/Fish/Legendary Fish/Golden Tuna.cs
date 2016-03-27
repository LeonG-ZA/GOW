using System;

namespace Server.Items
{
    public class GoldenTuna : BaseHighSeasFish
    {
        [Constructable]
        public GoldenTuna()
            : base(0x4302)
        {
        	Hue = 1161;
        }

        public GoldenTuna(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116102;
            }
        }//golden tuna

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