namespace Server.Items
{
    public class LightOfWayReadingGlasses : ElvenGlasses, ICollectionItem
    {
        public override int LabelNumber { get { return 1073378; } } // Light Of Way Reading Glasses

        public override int BasePhysicalResistance { get { return 8; } }
        public override int BaseFireResistance { get { return 6; } }
        public override int BaseColdResistance { get { return 7; } }
        public override int BasePoisonResistance { get { return 7; } }
        public override int BaseEnergyResistance { get { return 7; } }

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public LightOfWayReadingGlasses()
        {
            Hue = 598;
            Attributes.BonusStr = 7;
            Attributes.BonusInt = 5;
            Attributes.WeaponDamage = 30;
        }

        public LightOfWayReadingGlasses(Serial serial)
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