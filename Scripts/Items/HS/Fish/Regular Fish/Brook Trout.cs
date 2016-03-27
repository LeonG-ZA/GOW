using System;

namespace Server.Items
{
    public class BrookTrout : BaseHighSeasFish
    {
        [Constructable]
        public BrookTrout()
            : base(0x09CC)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public BrookTrout(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116415;
            }
        }//brook trout

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