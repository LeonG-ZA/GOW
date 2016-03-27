using System;
using Server;

namespace Server.Items
{
	public class ValentineBubbles : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new ValentineBubblesDeed(); } }
		
		#region Mondain's Legacy
		public override bool RetainDeedHue{ get{ return true; }	}
		#endregion

		[Constructable]
		public ValentineBubbles()
		{

			AddComponent( new AddonComponent( 0x4AA4 ),  0,  0, 0 );

		}

		public ValentineBubbles( Serial serial ) : base( serial )
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

	public class ValentineBubblesDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new ValentineBubbles(); } }
		public override int LabelNumber{ get{ return 1152280; } } // Valentine's Day 2012

		[Constructable]
		public ValentineBubblesDeed()
		{
		}

		public ValentineBubblesDeed( Serial serial ) : base( serial )
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