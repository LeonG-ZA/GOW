using System;
using Server;

namespace Server.Items
{
	[FlipableAttribute(0x4f7c, 0x4f7d)]
	public class CupidStatue : Item
	{
		public override int LabelNumber { get { return 1099220; } } // cupid statue

		[Constructable]
		public CupidStatue( )
			: base( 0x4F7C )
		{
			LootType = LootType.Blessed;
		}

		public CupidStatue( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}