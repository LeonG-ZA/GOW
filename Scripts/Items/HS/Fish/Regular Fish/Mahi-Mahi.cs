using System;

namespace Server.Items
{
    public class MahiMahi : BaseHighSeasFish
    {
        [Constructable]
        public MahiMahi()
            : base(0x44C5)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public MahiMahi(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116401;
            }
        }//mahi-mahi

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