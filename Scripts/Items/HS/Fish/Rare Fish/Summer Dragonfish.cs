using System;

namespace Server.Items
{
    public class SummerDragonfish : BaseHighSeasFish
    {
        [Constructable]
        public SummerDragonfish()
            : base(0x44E6)
        {
        	Hue = 2725;
        }

        public SummerDragonfish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116091;
            }
        }//summer dragonfish

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