using System;
using Server;

namespace Server.Items
{
	public class Planeshield : MetalKiteShield
	{

		public override int BaseEnergyResistance{ get{ return 10; } }

		[Constructable]
		public Planeshield()
		{
			Hue = 0x47E;
			Attributes.SpellChanneling = 1;

		}

		public Planeshield( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}