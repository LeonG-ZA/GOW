using System;

namespace Server.Items
{
    public class Cobia : BaseHighSeasFish
    {
        [Constructable]
        public Cobia()
            : base(0x4303)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public Cobia(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116400;
            }
        }//cobia

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