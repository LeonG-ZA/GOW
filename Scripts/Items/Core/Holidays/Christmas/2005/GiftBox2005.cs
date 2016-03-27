using System;

namespace Server.Items
{
    [Flipable(0x232A, 0x232B)]
    public class GiftBox2005 : GiftBox
    {
        [Constructable]
        public GiftBox2005()
        {
            int random = Utility.Random(100);

            if (random < 70)
                DropItem(new SantasSleighDeed());
            else if (random < 84)
                DropItem(new SantasReindeer2());
            else if (random < 50)
                DropItem(new SantasReindeer1());
            else
                DropItem(new SantasReindeer3());
        }

        public GiftBox2005(Serial serial)
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