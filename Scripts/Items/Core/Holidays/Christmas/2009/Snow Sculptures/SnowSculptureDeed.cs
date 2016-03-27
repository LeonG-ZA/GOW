using System;
using Server;
using Server.Gumps;

namespace Server.Items
{

	public class SnowSculptureDeed : Item
	{

		[Constructable]
		public SnowSculptureDeed() : base( 0x14EF )
		{
            Name = "A Snow Sculpture deed";
			LootType = LootType.Blessed;
            Hue = 1150;
			Weight = 5.0;
		}

		public SnowSculptureDeed( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				from.CloseGump( typeof( SnowSculptureGump ) );
				from.SendGump( new SnowSculptureGump( this ) );
			}
			else
				from.SendLocalizedMessage( 1062334 ); // This item must be in your backpack to be used.
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}
