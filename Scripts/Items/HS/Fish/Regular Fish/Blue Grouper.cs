using System;

namespace Server.Items
{
    public class BlueGrouper : BaseHighSeasFish
    {
        [Constructable]
        public BlueGrouper()
            : base(0x4306)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public BlueGrouper(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116411;
            }
        }//blue grouper

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