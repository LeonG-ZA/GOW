using System;

namespace Server.Items
{
    public class YellowtailBarracuda : BaseHighSeasFish
    {
        [Constructable]
        public YellowtailBarracuda()
            : base(0x44C3)
        {
        	Hue = 1191;
        }

        public YellowtailBarracuda(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116098;
            }
        }//yellowtail barracuda

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