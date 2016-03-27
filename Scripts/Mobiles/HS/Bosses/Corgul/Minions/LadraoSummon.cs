using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an Brigand corpse" )]
	public class SpawnedBrigand : Brigand
	{
		[Constructable]
		public SpawnedBrigand()
		{
			Container pack = Backpack;

			if ( pack != null )
				pack.Delete();

			NoKillAwards = true;
		}

		public SpawnedBrigand( Serial serial ) : base( serial )
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
			NoKillAwards = true;
		}
	}
}