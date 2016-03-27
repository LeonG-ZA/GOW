using System;
using Server;

namespace Server.Items
{
	public class VileTentacles : Item
	{
		public override int LabelNumber { get { return 1113333; } } // vile tentacles

		[Constructable]
		public VileTentacles()
			: this( 1 )
		{
		}

		[Constructable]
		public VileTentacles( int amount )
			: base( 0x5727 )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public VileTentacles( Serial serial )
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
