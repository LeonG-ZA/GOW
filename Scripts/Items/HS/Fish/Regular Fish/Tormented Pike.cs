using System;

namespace Server.Items
{
    public class TormentedPike : BaseHighSeasFish
    {
        [Constructable]
        public TormentedPike()
            : base(0x44C3)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public TormentedPike(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116434;
            }
        }//tormented pike

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