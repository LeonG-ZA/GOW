using System;

namespace Server.Items
{
    [Flipable(0x279C, 0x27E7)]
    public class HakamaShita : BaseOuterTorso
    {
        [Constructable]
        public HakamaShita()
            : this(0)
        {
        }

        [Constructable]
        public HakamaShita(int hue)
            : base(0x279C, hue)
        {
            this.Weight = 3.0;
        }

        public HakamaShita(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}