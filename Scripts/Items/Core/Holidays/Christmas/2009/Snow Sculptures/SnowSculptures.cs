using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0x4578, 0x4579 )]
	public class SeahorseSnowSculpture : Item
	{
		[Constructable]
		public SeahorseSnowSculpture() : base( 0x4578 )
		{
			Weight = 10.0;
			LootType = LootType.Blessed;
		}

		public SeahorseSnowSculpture( Serial serial ) : base( serial )
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
	
	[Flipable( 0x456E, 0x456F )]
	public class PegasusSnowSculpture : Item
	{
		[Constructable]
		public PegasusSnowSculpture() : base( 0x456E )
		{
			Weight = 10.0;
			LootType = LootType.Blessed;
		}

		public PegasusSnowSculpture( Serial serial ) : base( serial )
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
	
	[Flipable( 0x457A, 0x457B )]
	public class MermaidSnowSculpture : Item
	{
		[Constructable]
		public MermaidSnowSculpture() : base( 0x457A )
		{
			Weight = 10.0;
			LootType = LootType.Blessed;
		}

		public MermaidSnowSculpture( Serial serial ) : base( serial )
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
	
	[Flipable( 0x457C, 0x457D )]
	public class GryphonSnowSculpture : Item
	{
		[Constructable]
		public GryphonSnowSculpture() : base( 0x457C )
		{
			Weight = 10.0;
			LootType = LootType.Blessed;
		}

		public GryphonSnowSculpture( Serial serial ) : base( serial )
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