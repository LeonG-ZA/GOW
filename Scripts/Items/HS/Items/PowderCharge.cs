
namespace Server.Items
{
    public class LightPowderCharge : Item
    {
        [Constructable]
        public LightPowderCharge()
            : this(1)
        { }

        [Constructable]
        public LightPowderCharge(int amount)
            : base(0xE73)
        {
            Hue = 1150;
            Stackable = true;
            Amount = amount;
            Weight = 1;
        }

        public LightPowderCharge(Serial serial)
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
    public class HeavyPowderCharge : Item
    {
        [Constructable]
        public HeavyPowderCharge()
            : this(1)
        { }

        [Constructable]
        public HeavyPowderCharge(int amount)
            : base(0xE73)
        {
            Hue = 1150;
            Stackable = true;
            Amount = amount;
            Weight = 1;
        }

        public HeavyPowderCharge(Serial serial)
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