using System;

namespace Server.Items
{
    public class SeekerFish : BaseHighSeasFish
    {
        [Constructable]
        public SeekerFish()
            : base(0x4306)
        {
        	Hue = 2076;
        }

        public SeekerFish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116109;
            }
        }//seeker fish

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