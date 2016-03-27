using System;

namespace Server.Items
{
    public class UnicornFish : BaseHighSeasFish
    {
        [Constructable]
        public UnicornFish()
            : base(0x4304)
        {
        	Hue = 1154;
        }

        public UnicornFish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116086;
            }
        }//unicorn fish

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