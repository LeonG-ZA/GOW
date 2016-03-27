using System;

namespace Server.Items
{
    public class ThrobbingHeart : Item
    {
        [Constructable]
        public ThrobbingHeart()
            : base(0x1CED)
        {
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public override int LabelNumber { get { return 1111842; } } // a throbbing heart

        public ThrobbingHeart(Serial serial)
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