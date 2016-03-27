using System;

namespace Server.Items
{
    public class Tarpon : BaseHighSeasFish
    {
        [Constructable]
        public Tarpon()
            : base(0x44C3)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public Tarpon(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116397;
            }
        }//tarpon

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