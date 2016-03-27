using System;

namespace Server.Items
{
    public class ElvenDarkShirt : BaseShirt
    {
        [Constructable]
        public ElvenDarkShirt()
            : this(0)
        {
        }

        [Constructable]
        public ElvenDarkShirt(int hue)
            : base(0x3176, hue)
        {
            this.Weight = 2.0;
        }

        public ElvenDarkShirt(Serial serial)
            : base(serial)
        {
        }

        public override Race RequiredRace
        {
            get
            {
                return Race.Elf;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}