using System;

namespace Server.Items
{
    public class LanternFish : BaseHighSeasFish
    {
        [Constructable]
        public LanternFish()
            : base(0x44C5)
        {
        	Hue = 2003;
        }

        public LanternFish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116106;
            }
        }//lantern fish

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