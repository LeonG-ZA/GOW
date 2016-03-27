using System;

namespace Server.Items
{
    public class QuiverOfBlight : ElvenQuiver
    {
        public override int LabelNumber { get { return 1073111; } } // Quiver Of Blight

        public override int ColdDamage { get { return 50; } }
        public override int PoisonDamage { get { return 50; } }

        [Constructable]
        public QuiverOfBlight()
            : base()
        {
            Hue = 0x4F3;
            WeightReduction = 30;
        }

        public QuiverOfBlight(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}