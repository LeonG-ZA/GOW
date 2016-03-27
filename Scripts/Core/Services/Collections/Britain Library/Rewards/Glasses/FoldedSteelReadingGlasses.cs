using System;
using Server;

namespace Server.Items
{
	public class FoldedSteelReadingGlasses : ElvenGlasses, ICollectionItem
	{
		public override int LabelNumber { get { return 1073380; } } // Folded Steel Reading Glasses

        public override int BasePhysicalResistance { get { return 18; } }
        public override int BaseFireResistance { get { return 6; } }
        public override int BaseColdResistance { get { return 7; } }
        public override int BasePoisonResistance { get { return 7; } }
        public override int BaseEnergyResistance { get { return 7; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public FoldedSteelReadingGlasses()
		{
			Hue = 1150;
			Attributes.DefendChance = 15;
			Attributes.NightSight = 1;
			Attributes.BonusStr = 8;
		}

		public FoldedSteelReadingGlasses( Serial serial )
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
