using System;
using Server;

namespace Server.Items
{
	public class ObsidianChaosShield : ChaosShield
	{

		public override int BasePhysicalResistance{ get{ return 15; } }

		[Constructable]
		public ObsidianChaosShield()
		{
			Hue = 1175;
			Attributes.ReflectPhysical = 10;
			Attributes.DefendChance = 10;

			//TODO:  Add other resists randomly set between 1% - 3%
		}

		public override bool Validate( Mobile m )
		{
			return true;
		}

		public ObsidianChaosShield( Serial serial ) : base( serial )
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