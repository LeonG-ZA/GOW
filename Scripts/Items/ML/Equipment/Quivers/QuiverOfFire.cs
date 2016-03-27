using System;

namespace Server.Items
{
    public class QuiverOfFire : ElvenQuiver
    {
        public override int LabelNumber { get { return 1073109; } } // Quiver of Fire 

        public override int PhysicalDamage { get { return 50; } }
        public override int FireDamage { get { return 50; } }

        [Constructable]
        public QuiverOfFire()
            : base()
        {
            Hue = 1255;
            WeightReduction = 30;
        }

        public QuiverOfFire(Serial serial)
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