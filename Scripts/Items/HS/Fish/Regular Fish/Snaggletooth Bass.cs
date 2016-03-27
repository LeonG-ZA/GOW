using System;

namespace Server.Items
{
    public class SnaggletoothBass : BaseHighSeasFish
    {
        [Constructable]
        public SnaggletoothBass()
            : base(0x09CF)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public SnaggletoothBass(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116426;
            }
        }//snaggletooth bass

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