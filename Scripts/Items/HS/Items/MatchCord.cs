
namespace Server.Items
{
    public class MatchCord : Item
    {
        public override int LabelNumber { get { return 1116304; } } // Match Cord
        [Constructable]
        public MatchCord()
            : this(1)
        { }

        [Constructable]
        public MatchCord(int amount)
            : base(0x1421)
        {
            Hue = 1171;
            Stackable = true;
            Weight = 1;
            Amount = amount;
        }

        public override void OnDoubleClick(Mobile from)
        {
            //TODO Target the Cannon (Does not need be in backpack to use)
            base.OnDoubleClick(from);
        }

        public MatchCord(Serial serial)
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