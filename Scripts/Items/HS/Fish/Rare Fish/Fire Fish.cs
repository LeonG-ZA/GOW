using System;

namespace Server.Items
{
    public class FireFish : BaseHighSeasFish
    {
        [Constructable]
        public FireFish()
            : base(0x4306)
        {
        	Hue = 2118;
        }

        public FireFish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116093;
            }
        }// fire fish

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