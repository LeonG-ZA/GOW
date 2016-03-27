using System;
using Server;

namespace Server.Items
{
	public class FrostguardTalisman : BaseTalisman
	{
		public override int LabelNumber { get { return 1115516; } } // Frostguard Talisman

        //public override int BaseColdResistance { get { return 3; } }

		[Constructable]
		public FrostguardTalisman()
			: base( 0x2F5B )
		{
			Hue = 0x556;

			Weight = 1.0;
			//SAbsorptionAttributes.ColdEater = 5; // TODO: enable eaters in talismans
			Attributes.RegenMana = 1;
			Attributes.LowerManaCost = 5;
		}

		public FrostguardTalisman( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}