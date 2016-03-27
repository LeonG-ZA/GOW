using System;

namespace Server.Items
{
    public class DarkChocolate2009 : BaseSweet
    {
        public override int LabelNumber { get { return 1079994; } } // Dark chocolate
        public override double DefaultWeight { get { return 1.0; } }

        [Constructable]
        public DarkChocolate2009()
            : base(0xF10)
        {
            Hue = 1125;
            LootType = LootType.Blessed;
        }

        public DarkChocolate2009(Serial serial)
            : base(serial)
        { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();
        }
    }
}