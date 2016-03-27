using System;
using Server;

namespace Server.Items
{
    public class LilyPad : Item
    {
        public override int LabelNumber { get { return 1151218; } }

        [Constructable]
        public LilyPad():base(0x0DBC)
        {
            Weight = 1;
        }

        public LilyPad(Serial serial):base(serial)
        {

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }

    public class LilyPads : Item
    {
        public override int LabelNumber { get { return 1151219; } }

        [Constructable]
        public LilyPads()
            : base(0x0DBE)
        {
            Weight = 1;
        }

        public LilyPads(Serial serial)
            : base(serial)
        {

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}