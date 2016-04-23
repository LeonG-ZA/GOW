namespace Server.Items
{
    public class Fish : Item, ICarvable
    {
        [Constructable]
        public Fish()
            : this(1)
        {
        }

        [Constructable]
        public Fish(int amount)
            : base(Utility.Random(0x09CC, 4))
        {
            Stackable = true;
            if (Core.HS)
            {
                Weight = 10.0;
            }
            else
            {
                Weight = 1.0;
            }
            Amount = amount;
        }

        public Fish(Serial serial)
            : base(serial)
        {
        }

        public void Carve(Mobile from, Item item)
        {
            base.ScissorHelper(from, new RawFishSteak(), 4);
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