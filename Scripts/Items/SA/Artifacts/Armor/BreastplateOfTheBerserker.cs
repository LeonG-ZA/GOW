using System;

namespace Server.Items
{
    public class BreastplateOfTheBerserker : GargishPlateChest
    {
        public override int LabelNumber { get { return 1113539; } } // Breastplate of the Berserker

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }
        public override int BasePhysicalResistance { get { return 10; } }
        public override int BaseFireResistance { get { return 10; } }
        public override int BasePoisonResistance { get { return 5; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public BreastplateOfTheBerserker() 
        {
            Hue = 0x21;

            Attributes.BonusHits = 5;
            Attributes.WeaponSpeed = 10;
            Attributes.WeaponDamage = 15;
            Attributes.LowerManaCost = 4;
        }

        public BreastplateOfTheBerserker(Serial serial)
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