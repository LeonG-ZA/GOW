using System;
using Server;

namespace Server.Items
{
	public class WizardsCrystalReadingGlasses : ElvenGlasses, ICollectionItem
	{
		public override int LabelNumber { get { return 1073374; } } // Wizard's Crystal Reading Glasses

        public override int BasePhysicalResistance { get { return 3; } }
        public override int BaseFireResistance { get { return 1; } }
        public override int BaseColdResistance { get { return 2; } }
        public override int BasePoisonResistance { get { return 2; } }
        public override int BaseEnergyResistance { get { return 2; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public WizardsCrystalReadingGlasses()
		{
			Hue = 557;
			Attributes.BonusMana = 10;
			Attributes.RegenMana = 3;
			Attributes.SpellDamage = 15;
		}

		public WizardsCrystalReadingGlasses( Serial serial )
			: base( serial )
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
