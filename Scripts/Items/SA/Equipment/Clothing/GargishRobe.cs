using System;

namespace Server.Items
{
    [Flipable(0x4000, 0x4001)]
    public class GargishRobe : BaseClothing
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
        public GargishRobe()
            : this(0)
        {
        }

        [Constructable]
        public GargishRobe(int hue)
            : base(0x4000, Layer.OuterTorso, hue)
        {
            this.Weight = 1.0;
        }

        public GargishRobe(Serial serial)
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