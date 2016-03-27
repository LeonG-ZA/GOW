using System;

namespace Server.Items
{
    public class Mudpuppy : BaseHighSeasFish
    {
        [Constructable]
        public Mudpuppy()
            : base(0x09CC)
        {
        }

        public Mudpuppy(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1095117;
            }
        }//mud puppy

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