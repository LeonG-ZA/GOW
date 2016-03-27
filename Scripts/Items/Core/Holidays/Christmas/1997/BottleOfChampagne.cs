using System;
using Server;

namespace Server.Items
{
	public class BottleOfChampagne : BeverageBottle
	{
		[Constructable]
		public BottleOfChampagne() : base( BeverageType.Champagne )
		{
			LootType = LootType.Blessed;
		}

        public BottleOfChampagne(Serial serial)
            : base(serial)
		{
		}

        public override int LabelNumber
        {
            get
            {
                return 1123621;
            }
        }// Bottle of Champagne

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
