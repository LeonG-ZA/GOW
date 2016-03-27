using System;

namespace Server.Items
{
    [FlipableAttribute(0x50D8, 0x50D9)]
    public class GargishApron : BaseWaist
    {
        [Constructable]
        public GargishApron()
            : this(0)
        {
        }

        [Constructable]
        public GargishApron(int hue)
            : base(0x50D8, hue)
        {
            this.Weight = 2.0;
        }

        public GargishApron(Serial serial)
            : base(serial)
        {
        }

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