using System;
using Server;

namespace Server.Items
{
	public class ChampagneGlass : BeverageBottle
	{
		[Constructable]
		public ChampagneGlass() : base( BeverageType.Champagne )
		{
            ItemID = 0x99A;
            int[] ChampagneGlassHues = new int[] { 0x45, 0x27 };
            Hue = ChampagneGlassHues[Utility.Random(ChampagneGlassHues.Length)];
			LootType = LootType.Blessed;
		}

        public ChampagneGlass(Serial serial)
            : base(serial)
		{
		}

        public override int LabelNumber
        {
            get
            {
                return 1041423;
            }
        }// a champagne glass

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
