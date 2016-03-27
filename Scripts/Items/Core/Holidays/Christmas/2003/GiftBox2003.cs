using System;

namespace Server.Items
{
    [Flipable(0x232A, 0x232B)]
    public class GiftBox2003 : GiftBox
    {
        [Constructable]
        public GiftBox2003()
        {
            DropItem(new Snowman());
            DropItem(new WreathDeed());
            DropItem(new BlueSnowflake());
            DropItem(new RedPoinsettia());
        }

        public GiftBox2003(Serial serial)
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