using System;

namespace Server.Items
{
    public class BouraTailShield : WoodenKiteShield
    {
        public override int LabelNumber { get { return 1112361; } } // boura tail shield

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public BouraTailShield()
        {
            Hue = 356;
            Attributes.ReflectPhysical = 10;
            ArmorAttributes.ReactiveParalyze = 1;
        }

        public BouraTailShield(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance { get { return 8; } }

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