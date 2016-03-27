using System;

namespace Server.Items
{
    [FlipableAttribute(0x9BCC, 0x9BCD)]
    public class BlackTigerPelt : BaseLeather
    {
        [Constructable]
        public BlackTigerPelt()
            : this(1)
        {
        }

        [Constructable]
        public BlackTigerPelt(int amount)
            : base(CraftResource.RegularLeather, amount)
        {
        }

        public BlackTigerPelt(Serial serial)
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