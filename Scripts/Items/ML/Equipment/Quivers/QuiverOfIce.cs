using System;

namespace Server.Items
{
    public class QuiverOfIce : ElvenQuiver
    {
        public override int LabelNumber { get { return 1073110; } } // Quiver of Ice 

        public override int PhysicalDamage { get { return 50; } }
        public override int ColdDamage { get { return 50; } }

        [Constructable]
        public QuiverOfIce()
            : base()
        {
            Hue = 1261;
            WeightReduction = 30;
        }

        public QuiverOfIce(Serial serial)
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