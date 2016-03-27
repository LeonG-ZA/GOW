using System;

namespace Server.Items
{
	public class VirtueCodexRugAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new VirtueCodexRugDeed(); } }

		[Constructable]
		public VirtueCodexRugAddon() : base()
		{
			AddComponent( new LocalizedAddonComponent( 0x40DA, 1095474 ), 0, 0, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40DB, 1095474 ), 1, 0, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40DC, 1095474 ), 2, 0, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40DD, 1095474 ), 3, 0, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40DE, 1095474 ), 4, 0, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40DF, 1095474 ), 5, 0, 0 );
			
			AddComponent( new LocalizedAddonComponent( 0x40E0, 1095474 ), 0, 1, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40E1, 1095474 ), 1, 1, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40E2, 1095474 ), 2, 1, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40E3, 1095474 ), 3, 1, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40E4, 1095474 ), 4, 1, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40E5, 1095474 ), 5, 1, 0 );
			
			AddComponent( new LocalizedAddonComponent( 0x40E6, 1095474 ), 0, 2, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40E7, 1095474 ), 1, 2, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40E8, 1095474 ), 2, 2, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40E9, 1095474 ), 3, 2, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40EA, 1095474 ), 4, 2, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40EB, 1095474 ), 5, 2, 0 );
			
			AddComponent( new LocalizedAddonComponent( 0x40EC, 1095474 ), 0, 3, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40ED, 1095474 ), 1, 3, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40EE, 1095474 ), 2, 3, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40EF, 1095474 ), 3, 3, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40F0, 1095474 ), 4, 3, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40F1, 1095474 ), 5, 3, 0 );
			
			AddComponent( new LocalizedAddonComponent( 0x40F2, 1095474 ), 0, 4, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40F3, 1095474 ), 1, 4, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40F4, 1095474 ), 2, 4, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40F5, 1095474 ), 3, 4, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40F6, 1095474 ), 4, 4, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40F7, 1095474 ), 5, 4, 0 );
			
			AddComponent( new LocalizedAddonComponent( 0x40F8, 1095474 ), 0, 5, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40F9, 1095474 ), 1, 5, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40FA, 1095474 ), 2, 5, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40FB, 1095474 ), 3, 5, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40FC, 1095474 ), 4, 5, 0 );
			AddComponent( new LocalizedAddonComponent( 0x40FD, 1095474 ), 5, 5, 0 );
		}

		public VirtueCodexRugAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	public class VirtueCodexRugDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new VirtueCodexRugAddon(); } }
        public override int LabelNumber { get { return 1113919; } } // a Codex of Virtue deed

		[Constructable]
		public VirtueCodexRugDeed() : base()
		{
			LootType = LootType.Blessed;
		}

		public VirtueCodexRugDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}
