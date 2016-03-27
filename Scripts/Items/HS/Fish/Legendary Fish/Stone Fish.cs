using System;

namespace Server.Items
{
    public class StoneFish : BaseHighSeasFish
    {
        [Constructable]
        public StoneFish()
            : base(0x44C5)
        {
        	Hue = 2009;
        }

        public StoneFish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116110;
            }
        }//stone fish

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