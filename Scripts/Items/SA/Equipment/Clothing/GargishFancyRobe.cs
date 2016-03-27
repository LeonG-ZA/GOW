using System;

namespace Server.Items
{
    [Flipable(0x4002, 0x4003)]
    public class GargishFancyRobe : BaseClothing
    {
        public override Race RequiredRace
        {
            get
            {
                return Race.Gargoyle;
            }
        }
        public override bool CanBeWornByGargoyles
        {
            get
            {
                return true;
            }
        }

        [Constructable]
        public GargishFancyRobe()
            : this(0)
        {
        }

        [Constructable]
        public GargishFancyRobe(int hue)
            : base(0x4002, Layer.OuterTorso, hue)
        {
            this.Weight = 1.0;
        }

        public GargishFancyRobe(Serial serial)
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