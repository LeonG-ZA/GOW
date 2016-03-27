﻿using System;
using Server;

namespace Server.Items
{
	public class BrassRing : GoldRing
	{
		public override int LabelNumber { get { return 1075778; } } // Brass Ring

		[Constructable]
		public BrassRing()
		{
			LootType = LootType.Blessed;
		}

		public override bool Nontransferable { get { return true; } }

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			list.Add( 1072351 ); // Quest Item
		}

		public BrassRing( Serial serial )
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