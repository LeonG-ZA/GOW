
namespace Server.Items
{
    public class FuseCord : Item
    {
        public override int LabelNumber { get { return 1116305; } } // Fuse Cord
        [Constructable]
        public FuseCord()
            : this(1)
        { }

        [Constructable]
        public FuseCord(int amount)
            : base(0x1420)
        {
            Hue = 1164;
            Stackable = true;
            Weight = 1;
            Amount = amount;
        }

        public override void OnDoubleClick(Mobile from)
        {
            //TODO Target the Cannon (Does not need be in backpack to use)
            base.OnDoubleClick(from);
        }

        public FuseCord(Serial serial)
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