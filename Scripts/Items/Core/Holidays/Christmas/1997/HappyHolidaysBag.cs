using System;

namespace Server.Items
{
    public class HappyHolidaysBag : Bag
    {
        [Constructable]
        public HappyHolidaysBag()
            : this(1)
        {
            Movable = true;
            int[] HappyHolidaysBagHues = new int[] { 0x45, 0x27, 0x3d0 };
            Hue = HappyHolidaysBagHues[Utility.Random(HappyHolidaysBagHues.Length)];
        }

        [Constructable]
        public HappyHolidaysBag(int amount)
        {
            DropItem(new BottleOfChampagne());
            DropItem(new BunchOfDates());
            DropItem(new BottleOfEggnog());
            DropItem(new FireworksWand());
            DropItem(new Fruitcake());
            DropItem(new SeasonsGreetings());
            DropItem(new WristWatch());
        }

        public HappyHolidaysBag(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1007150;
            }
        }// Happy Holidays!

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