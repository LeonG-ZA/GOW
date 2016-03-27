using System;

namespace Server.Items
{
    public class Darkfish : BaseHighSeasFish
    {
        [Constructable]
        public Darkfish()
            : base(0x4307)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public Darkfish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116429;
            }
        }//darkfish

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