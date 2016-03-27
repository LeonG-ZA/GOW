using System;

namespace Server.Items
{
    public class Walleye
    	: BaseHighSeasFish
    {
        [Constructable]
        public Walleye()
            : base(0x09CF)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public Walleye(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116422;
            }
        }//walleye

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