﻿using System;
using Server;

namespace Server.Items
{
	public class DraconicOrb : TransientItem
	{
		public override int LabelNumber { get { return 1113515; } } // Draconic Orb (Lesser)

		[Constructable]
		public DraconicOrb()
			: base( 0x573E, TimeSpan.FromHours( 12.0 ) )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
			Hue = 0x80F;
		}

		public DraconicOrb( Serial serial )
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