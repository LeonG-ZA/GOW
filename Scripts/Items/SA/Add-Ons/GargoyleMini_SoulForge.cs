using System;
using Server;

namespace Server.Items
{
	public class GargoyleMini_SoulForge : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new GargoyleMiniSoulForgeDeed(); } }

		[Constructable]
		public GargoyleMini_SoulForge()
		{
			AddComponent( new AddonComponent( 17607 ), 0, 0, 0 );
		}

        public GargoyleMini_SoulForge(Serial serial)
            : base(serial)
		{
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
	public class GargoyleMiniSoulForgeDeed : BaseAddonDeed
	{
        public override BaseAddon Addon { get { return new GargoyleMini_SoulForge(); } }
		
		[Constructable]
		public GargoyleMiniSoulForgeDeed()
		{
			Name = "Gargoyle Soulforge";
		}

		public GargoyleMiniSoulForgeDeed( Serial serial ) : base( serial )
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