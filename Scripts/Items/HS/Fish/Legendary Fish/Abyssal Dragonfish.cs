using System;

namespace Server.Items
{
    public class AbyssalDragonfish : BaseHighSeasFish
    {
        [Constructable]
        public AbyssalDragonfish()
            : base(0x44E6)
        {
        	Hue = 1908;
        }

        public AbyssalDragonfish(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116118;
            }
        }//abyssal dragonfish

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