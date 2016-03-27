using System;

namespace Server.Items
{
    public class FairySalmon : BaseHighSeasFish
    {
        [Constructable]
        public FairySalmon()
            : base(0x4302)
        {
        	Hue = 2121;
        }

        public FairySalmon(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116089;
            }
        }// fairy salmon

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