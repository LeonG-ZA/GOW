
namespace Server.Items
{
    public class Saltpeter : Item
    {
        [Constructable]
        public Saltpeter()
            : this(1)
        { }

        [Constructable]
        public Saltpeter(int amount)
            : base(0x423A)
        {
            Amount = amount;
            Stackable = true;
            Hue = 1150;
        }

        public Saltpeter(Serial serial)
            : base(serial)
        { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

    }
}