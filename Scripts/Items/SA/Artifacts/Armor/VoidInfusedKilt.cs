using System;

namespace Server.Items
{
    public class VoidInfusedKilt : GargishPlateKilt
    {
        public override int LabelNumber { get { return 1113868; } } // Void Infused Kilt

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }
        public override int BasePhysicalResistance { get { return 5; } }
        public override int BaseFireResistance { get { return 6; } }
        public override int BaseColdResistance { get { return 3; } }
        public override int BasePoisonResistance { get { return 3; } }
        public override int BaseEnergyResistance { get { return 4; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public VoidInfusedKilt()
            : base()
        {
            Hue = 1482;

            AbsorptionAttributes.EaterDamage = 10;
            Attributes.BonusStr = 5;
            Attributes.BonusDex = 5;
            Attributes.RegenStam = 1;
            Attributes.RegenMana = 1;
            Attributes.AttackChance = 5;
        }

        public VoidInfusedKilt(Serial serial)
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