using System;
using Server.Gumps;

namespace Server.Items
{
    public class DecoratedCommemorativePlate : Item
    {
        [Constructable]
        public DecoratedCommemorativePlate()
            : base(0x9BC8)
        {
            LootType = LootType.Blessed;
        }

        public DecoratedCommemorativePlate(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber { get { return 1156149; } }//An Ornately Decorated Commemorative Plate

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