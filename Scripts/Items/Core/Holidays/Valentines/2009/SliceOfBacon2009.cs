using System;

namespace Server.Items
{
    public class SliceOfBacon2009 : BaseFood
    {
        [Constructable]
        public SliceOfBacon2009()
            : this(1)
        {
        }

        [Constructable]
        public SliceOfBacon2009(int amount)
            : base(amount, 0x979)
        {
            Weight = 1.0;
            FillFactor = 1;
            LootType = LootType.Blessed;
        }

        public SliceOfBacon2009(Serial serial)
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