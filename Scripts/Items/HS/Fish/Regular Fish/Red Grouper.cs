using System;

namespace Server.Items
{
    public class RedGrouper : BaseHighSeasFish
    {
        [Constructable]
        public RedGrouper()
            : base(0x4307)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public RedGrouper(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116407;
            }
        }//red grouper

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