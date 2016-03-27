using System;
using Server;

namespace Server.Items
{
	[FlipableAttribute(0x48E0, 0x48E1)]
	public class ValentinePandaBear : Item /* TODO: when dye tub changes are implemented, furny dyable this */
	{
		public override int LabelNumber { get { return 1150294; } } // St. Valentine Bear

		[Constructable]
		public ValentinePandaBear()
			: base( 0x48E0 )
		{
			LootType = LootType.Blessed;
		}

		public ValentinePandaBear( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	[FlipableAttribute(0x48E2, 0x48E3)]
	public class ValentinePolarBear : Item /* TODO: when dye tub changes are implemented, furny dyable this */
	{
		public override int LabelNumber { get { return 1150294; } } // St. Valentine Bear

		[Constructable]
		public ValentinePolarBear()
			: base( 0x48E2 )
		{
			LootType = LootType.Blessed;
		}

		public ValentinePolarBear( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	[FlipableAttribute(0x4A9C, 0x4A9D)]
	public class ShackledHeart : Item /* TODO: when dye tub changes are implemented, furny dyable this */
	{
		public override int LabelNumber { get { return 1097972; } } // shackled heart

		[Constructable]
		public ShackledHeart()
			: base( 0x4A9C )
		{
			LootType = LootType.Blessed;
		}

		public ShackledHeart( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}