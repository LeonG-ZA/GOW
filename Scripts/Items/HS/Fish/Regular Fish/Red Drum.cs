using System;

namespace Server.Items
{
    public class RedDrum : BaseHighSeasFish
    {
        [Constructable]
        public RedDrum()
            : base(0x4302)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public RedDrum(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116410;
            }
        }//red drum

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