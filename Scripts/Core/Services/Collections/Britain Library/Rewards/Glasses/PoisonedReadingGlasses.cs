namespace Server.Items
{
    public class PoisonedReadingGlasses : ElvenGlasses, ICollectionItem
    {
        public override int LabelNumber { get { return 1073376; } } // Poisoned Reading Glasses

        public override int BasePhysicalResistance { get { return 8; } }
        public override int BaseFireResistance { get { return 6; } }
        public override int BaseColdResistance { get { return 7; } }
        public override int BasePoisonResistance { get { return 27; } }
        public override int BaseEnergyResistance { get { return 7; } }

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public PoisonedReadingGlasses()
        {
            Hue = 275;

            Attributes.BonusStam = 3;
            Attributes.RegenStam = 4;
        }

        public PoisonedReadingGlasses(Serial serial)
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