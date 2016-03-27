using System;
using Server;

namespace Server.Items
{
	public class BottleOfEggnog : BeverageBottle
	{
		[Constructable]
		public BottleOfEggnog() : base( BeverageType.Champagne )
		{
            Name = "a Bottle Of Eggnog";
            ItemID = 0x99F;
			LootType = LootType.Blessed;
		}

        public BottleOfEggnog(Serial serial)
            : base(serial)
		{
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
