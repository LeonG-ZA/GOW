using System;
using Server;

namespace Server.Items
{
	[FlipableAttribute( 0x4A8E, 0x4A8F )]
	public class PlagueMask : Item
	{
		[Constructable]
		public PlagueMask() : base( 0x4A8E )
		{
			
			Name = "Plague Mask";
			Weight = 2.0;
			LootType = LootType.Blessed;
		}

		public PlagueMask( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060662, "Halloween \t 2011" );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	[FlipableAttribute( 0x4A90, 0x4A91 )]
	public class ClownMask : Item
	{
		[Constructable]
		public ClownMask() : base( 0x4A90 )
		{
			
			Name = "Evil Clown Mask";
			Weight = 2.0;
			LootType = LootType.Blessed;
		}

		public ClownMask( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060662, "Halloween \t 2011" );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	[FlipableAttribute( 0x4A92, 0x4A93 )]
	public class DaemonMask : Item
	{
		[Constructable]
		public DaemonMask() : base( 0x4A92 )
		{
			
			Name = "Daemon Mask";
			Weight = 2.0;
			LootType = LootType.Blessed;
		}

		public DaemonMask( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060662, "Halloween \t 2011" );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
