using System;

namespace Server.Items
{
    public class GargishBracelet : BaseBracelet
    {
        [Constructable]
        public GargishBracelet()
            : base(0x4211)
        {
            Weight = 0.1;
        }

        public GargishBracelet(Serial serial)
            : base(serial)
        {
        }

        public override Race RequiredRace { get { return Race.Gargoyle; } }

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