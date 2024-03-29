using System;

namespace Server.Items
{
    public class LavaFish : BaseHighSeasFish
    {
        [Constructable]
        public LavaFish()
            : base(0x4304)
        {
        	Hue = 2075;
        }

        public LavaFish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116096;
            }
        }//lava fish

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