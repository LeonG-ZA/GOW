using System;

namespace Server.Items
{
    public class YellowfinTuna : BaseHighSeasFish
    {
        [Constructable]
        public YellowfinTuna()
            : base(0x44C4)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public YellowfinTuna(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116404;
            }
        }//yellowfin tuna

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