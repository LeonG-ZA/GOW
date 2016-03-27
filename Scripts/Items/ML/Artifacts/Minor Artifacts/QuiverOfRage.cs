using System;

namespace Server.Items
{
    public class QuiverOfRage : BaseQuiver
    {
        public override int LabelNumber { get { return 1075038; } } // Quiver of Rage

        public override int PhysicalDamage { get { return 20; } }
        public override int FireDamage { get { return 20; } }
        public override int ColdDamage { get { return 20; } }
        public override int PoisonDamage { get { return 20; } }
        public override int EnergyDamage { get { return 20; } }

        [Constructable]
        public QuiverOfRage()
            : base()
        {
            Hue = 588;

            WeightReduction = 25;
        }

        public QuiverOfRage(Serial serial)
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