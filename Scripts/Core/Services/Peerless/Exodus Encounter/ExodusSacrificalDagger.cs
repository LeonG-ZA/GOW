using System;
using Server;

namespace Server.Items
{
    public class ExodusSacrificalDagger : TransientItem
    {
        [Constructable]
        public ExodusSacrificalDagger()
            : base(0x2D2D, TimeSpan.FromSeconds(21600.0))
        {
        }

        public ExodusSacrificalDagger(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber { get { return 1153500; } }// Exodus Sacrifical Dagger

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
 