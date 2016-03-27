using System;

namespace Server.Items
{
    public class ElvenShirt : BaseShirt
    {
        [Constructable]
        public ElvenShirt()
            : this(0)
        {
        }

        [Constructable]
        public ElvenShirt(int hue)
            : base(0x3175, hue)
        {
            this.Weight = 2.0;
        }

        public ElvenShirt(Serial serial)
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