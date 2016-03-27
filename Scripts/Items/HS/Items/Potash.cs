
namespace Server.Items
{
    public class Potash : Item
    {
        [Constructable]
        public Potash()
            : this(1)
        { }

        [Constructable]
        public Potash(int amount)
            : base(0x423A)
        {
            Amount = amount;
            Stackable = true;
            Hue = 2410;
        }

        public Potash(Serial serial)
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