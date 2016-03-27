using System;

namespace Server.Items
{
    public class WinterDragonfish : BaseHighSeasFish
    {
        [Constructable]
        public WinterDragonfish()
            : base(0x44E6)
        {
        	Hue = 1150;
        }

        public WinterDragonfish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116105;
            }
        }//winter dragonfish

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