using System;

namespace Server.Items
{
    public class BlackSeabass : BaseHighSeasFish
    {
        [Constructable]
        public BlackSeabass()
            : base(0x09CE)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public BlackSeabass(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116396;
            }
        }//black seabass

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