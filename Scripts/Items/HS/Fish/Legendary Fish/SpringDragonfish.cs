using System;

namespace Server.Items
{
    public class SpringDragonfish : BaseHighSeasFish
    {
        [Constructable]
        public SpringDragonfish()
            : base(0x44E6)
        {
        }

        public SpringDragonfish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116104;
            }
        }//spring dragonfish

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