using System;

namespace Server.Items
{
    public class DrakeFish : BaseHighSeasFish
    {
        [Constructable]
        public DrakeFish()
            : base(0x44C5)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public DrakeFish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116429;
            }
        }//drake fish

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