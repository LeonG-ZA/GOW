using System;

namespace Server.Items
{
    public class CaptainSnook : BaseHighSeasFish
    {
        [Constructable]
        public CaptainSnook()
            : base(0x44C5)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public CaptainSnook(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116408;
            }
        }//captain snook

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