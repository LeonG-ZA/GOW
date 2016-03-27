using System;

namespace Server.Items
{
    public class GreenCatfish : BaseHighSeasFish
    {
        [Constructable]
        public GreenCatfish()
            : base(0x44C6)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public GreenCatfish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116421;
            }
        }//green catfish

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