using System;

namespace Server.Items
{
    public class Bluefish : BaseHighSeasFish
    {
        [Constructable]
        public Bluefish()
            : base(0x09CC)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public Bluefish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116406;
            }
        }//bluefish

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