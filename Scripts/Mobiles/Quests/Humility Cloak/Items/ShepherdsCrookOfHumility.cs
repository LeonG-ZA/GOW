﻿using System;
using Server;

namespace Server.Items
{
	public class ShepherdsCrookOfHumility : ShepherdsCrook
	{
		public override int LabelNumber { get { return 1075856; } } // Shepherd's Crook of Humility (Replica)

		[Constructable]
		public ShepherdsCrookOfHumility()
		{
			Hue = 0x386;
		}

		public ShepherdsCrookOfHumility( Serial serial )
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