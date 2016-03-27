using System;

namespace Server.Items
{
    public class Bonito : BaseHighSeasFish
    {
        [Constructable]
        public Bonito()
            : base(0x4303)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public Bonito(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116405;
            }
        }//bonito

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