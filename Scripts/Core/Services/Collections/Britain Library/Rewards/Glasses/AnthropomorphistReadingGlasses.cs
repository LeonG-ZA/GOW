using System;
using Server;

namespace Server.Items
{
	public class AnthropomorphistReadingGlasses : ElvenGlasses, ICollectionItem
	{
		public override int LabelNumber { get { return 1073379; } } // Anthropomorphist Reading Glasses

        public override int BasePhysicalResistance { get { return 3; } }
        public override int BaseFireResistance { get { return 1; } }
        public override int BaseColdResistance { get { return 7; } }
        public override int BasePoisonResistance { get { return 17; } }
        public override int BaseEnergyResistance { get { return 17; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public AnthropomorphistReadingGlasses()
		{
			Hue = 128;
			Attributes.RegenMana = 3;
			Attributes.BonusHits = 5;
		}

		public AnthropomorphistReadingGlasses( Serial serial )
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
