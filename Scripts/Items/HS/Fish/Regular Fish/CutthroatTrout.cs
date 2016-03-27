using System;

namespace Server.Items
{
    public class CutthroatTrout : BaseHighSeasFish
    {
        [Constructable]
        public CutthroatTrout()
            : base(0x4303)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public CutthroatTrout(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116427;
            }
        }//cutthroat trout

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