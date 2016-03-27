using System;

namespace Server.Items
{
    public class GiantSteps : GargishStoneLegs
    {
        public override int LabelNumber { get { return 1113537; } } // Giant Steps

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }
        public override int BasePhysicalResistance { get { return 18; } }
        public override int BaseFireResistance { get { return 16; } }
        public override int BaseEnergyResistance { get { return 12; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public GiantSteps()
            : base()
        {
            Hue = 556;

            Attributes.BonusStr = 5;
            Attributes.BonusDex = 5;
            Attributes.BonusHits = 5;
            Attributes.RegenHits = 2;
            Attributes.WeaponDamage = 10;
        }

        public GiantSteps(Serial serial)
            : base(serial)
        {
        }

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