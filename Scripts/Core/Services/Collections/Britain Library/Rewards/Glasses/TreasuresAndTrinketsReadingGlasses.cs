using System;
using Server;

namespace Server.Items
{
	public class TreasuresAndTrinketsReadingGlasses : ElvenGlasses, ICollectionItem
	{
		public override int LabelNumber { get { return 1073373; } } // Treasures And Trinkets Reading Glasses

        public override int BasePhysicalResistance { get { return 8; } }
        public override int BaseFireResistance { get { return 6; } }
        public override int BaseColdResistance { get { return 7; } }
        public override int BasePoisonResistance { get { return 7; } }
        public override int BaseEnergyResistance { get { return 7; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public TreasuresAndTrinketsReadingGlasses()
		{
			Hue = 450;
			Attributes.BonusInt = 10;
			Attributes.BonusHits = 5;
			Attributes.SpellDamage = 10;
		}

		public TreasuresAndTrinketsReadingGlasses( Serial serial )
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
