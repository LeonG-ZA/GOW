namespace Server.Items
{
    public class ReadingGlassesOfTheArts : ElvenGlasses, ICollectionItem
    {
        public override int LabelNumber { get { return 1073363; } } // Reading Glasses of the Arts

        public override int BasePhysicalResistance { get { return 8; } }
        public override int BaseFireResistance { get { return 4; } }
        public override int BaseColdResistance { get { return 5; } }
        public override int BasePoisonResistance { get { return 1; } }
        public override int BaseEnergyResistance { get { return 7; } }

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public ReadingGlassesOfTheArts()
        {
            Hue = 115;

            Attributes.BonusInt = 5;
            Attributes.BonusStr = 5;
            Attributes.BonusHits = 15;
        }

        public ReadingGlassesOfTheArts(Serial serial)
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