using System;

namespace Server.Items
{
    public class SeasonsGreetings : Item
    {
        [Constructable]
        public SeasonsGreetings()
            : base(0x14ef)
        {
            int[] SeasonsGreetingsHues = new int[] { 0x45, 0x27, 0x3d0 };
            Hue = SeasonsGreetingsHues[Utility.Random(SeasonsGreetingsHues.Length)];
            Stackable = false;
            LootType = LootType.Blessed;
            Movable = true;
        }

        public SeasonsGreetings(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041425;
            }
        }// Season's Greetings
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