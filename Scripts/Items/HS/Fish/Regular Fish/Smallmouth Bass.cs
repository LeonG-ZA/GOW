using System;

namespace Server.Items
{
    public class SmallmouthBass : BaseHighSeasFish
    {
        [Constructable]
        public SmallmouthBass()
            : base(0x09CD)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public SmallmouthBass(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116419;
            }
        }//smallmouth bass

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