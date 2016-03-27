using System;

namespace Server.Items
{
    public class UncommonShiner : BaseHighSeasFish
    {
        [Constructable]
        public UncommonShiner()
            : base(0x09CE)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public UncommonShiner(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116420;
            }
        }//uncommon shiner

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