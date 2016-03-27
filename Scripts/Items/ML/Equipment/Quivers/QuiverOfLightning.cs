using System;

namespace Server.Items
{
    public class QuiverOfLightning : ElvenQuiver
    {
        public override int LabelNumber { get { return 1073112; } } // Quiver of Lightning 

        public override int PhysicalDamage { get { return 50; } }
        public override int EnergyDamage { get { return 50; } }

        [Constructable]
        public QuiverOfLightning()
            : base()
        {
            Hue = 1273;
            WeightReduction = 30;
        }

        public QuiverOfLightning(Serial serial)
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