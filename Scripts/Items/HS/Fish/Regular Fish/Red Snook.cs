using System;

namespace Server.Items
{
    public class RedSnook : BaseHighSeasFish
    {
        [Constructable]
        public RedSnook()
            : base(0x09CD)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public RedSnook(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116398;
            }
        }//red snook

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