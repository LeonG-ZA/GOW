namespace Server.Items
{
    public class MaceAndShieldReadingGlasses : ElvenGlasses, ICollectionItem
    {
        public override int LabelNumber { get { return 1073381; } } // Mace And Shield Reading Glasses

        public override int BasePhysicalResistance { get { return 23; } }
        public override int BaseFireResistance { get { return 6; } }
        public override int BaseColdResistance { get { return 7; } }
        public override int BasePoisonResistance { get { return 7; } }
        public override int BaseEnergyResistance { get { return 7; } }

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public MaceAndShieldReadingGlasses()
        {
            Hue = 477;

            Attributes.BonusStr = 10;
            Attributes.BonusDex = 5;
            HitLowerDefend = 30;
        }

        public MaceAndShieldReadingGlasses(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}