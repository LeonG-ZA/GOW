using System;

namespace Server.Items
{
    public class LurkerFish : BaseHighSeasFish
    {
        [Constructable]
        public LurkerFish()
            : base(0x09CE)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public LurkerFish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116435;
            }
        }//lurker fish

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