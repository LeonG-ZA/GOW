using System;

namespace Server.Items
{
    public class GiantSamuraiFish : BaseHighSeasFish
    {
        [Constructable]
        public GiantSamuraiFish()
            : base(0x4306)
        {
        	Hue = 2565;
        }

        public GiantSamuraiFish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116103;
            }
        }//giant samurai fish

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