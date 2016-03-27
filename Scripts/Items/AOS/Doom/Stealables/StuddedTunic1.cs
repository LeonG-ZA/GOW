using System;

namespace Server.Items
{
	public class StuddedTunic1 : StealableArtifact
	{
		public override int ArtifactRarity { get { return 7; } }

		[Constructable]
		public StuddedTunic1()
			: base( 0x13D9 )
		{
		}

		public StuddedTunic1( Serial serial )
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
