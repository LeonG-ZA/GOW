using System;

namespace Server.Items
{
    public class YellowPerch : BaseHighSeasFish
    {
        [Constructable]
        public YellowPerch()
            : base(0x4303)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public YellowPerch(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116413;
            }
        }//yellow perch

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