﻿

namespace Server.Items
{
    public class Swab : Item
    {
        [Constructable]
        public Swab()
            : base(0x4248)
        { }

        public Swab(Serial serial)
            : base(serial)
        { }

        public override void OnDoubleClick(Mobile from)
        {
            //TODO Target the Cannon (Does not need be in backpack to use)
            base.OnDoubleClick(from);
        }

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