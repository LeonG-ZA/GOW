using System;

namespace Server.Items
{
    public class Redherring : BaseHighSeasFish
    {
        [Constructable]
        public Redherring()
            : base(0x09CC)
        {
        }

        public Redherring(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1095046;
            }
        }//red herring

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