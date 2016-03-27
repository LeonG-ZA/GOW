using System;
using Server;

namespace Server.Items
{
	public class ZooRewardWolfStatue : Item
	{
		public override int LabelNumber { get { return 1073190; } } // A Wolf Contribution Statue from the Britannia Royal Zoo.

		[Constructable]
		public ZooRewardWolfStatue()
			: base( 0x2122 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public ZooRewardWolfStatue( Serial serial )
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