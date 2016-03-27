using System;

namespace Server.Items
{
    [FlipableAttribute(0x9BCC, 0x9BCD)]
    public class OrangeTigerPelt : BaseLeather
    {
        [Constructable]
        public OrangeTigerPelt()
            : this(1)
        {
        }

        [Constructable]
        public OrangeTigerPelt(int amount)
            : base(CraftResource.RegularLeather, amount)
        {
        }

        public OrangeTigerPelt(Serial serial)
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