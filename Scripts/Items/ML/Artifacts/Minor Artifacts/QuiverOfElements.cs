using System;

namespace Server.Items
{
    public class QuiverOfElements : BaseQuiver
    {
        public override int LabelNumber { get { return 1075040; } } // Quiver of the Elements

        public override int ChaosDamage { get { return 100; } }
        [Constructable]
        public QuiverOfElements()
            : base()
        {
            Hue = 235;
            WeightReduction = 50;
        }

        public QuiverOfElements(Serial serial)
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