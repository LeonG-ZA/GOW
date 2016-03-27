using System;

namespace Server.Items
{
    [Flipable(0x232A, 0x232B)]
    public class GiftBox2004 : GiftBox
    {
        [Constructable]
        public GiftBox2004()
        {
            DropItem(new MistletoeDeed());
            DropItem(new PileOfGlacialSnow());
            DropItem(new LightOfTheWinterSolstice());

            int random = Utility.Random(100);

            if (random < 60)
                DropItem(new DecorativeTopiary());
            else if (random < 84)
                DropItem(new FestiveCactus());
            else
                DropItem(new SnowyTree());
        }

        public GiftBox2004(Serial serial)
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