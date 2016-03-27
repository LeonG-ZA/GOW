using System;

namespace Server.Items
{
	public class Switches : Item
	{
		[Constructable]
		public Switches() : base( 0xDE1 )
		{
			this.Weight = 0.1;
			Name = "switches";
		}

		public Switches( Serial serial ) : base( serial )
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

			int version = reader.ReadInt();
		}
	}
}