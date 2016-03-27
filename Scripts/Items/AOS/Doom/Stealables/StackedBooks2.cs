using System;

namespace Server.Items
{
	public class StackedBooks2 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		[Constructable]
		public StackedBooks2()
			: base( 0x1E25 )
		{
		}

		public StackedBooks2( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
