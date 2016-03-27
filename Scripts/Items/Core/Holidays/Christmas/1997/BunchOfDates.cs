using System; 
using Server.Items; 

namespace Server.Items 
{ 
	public class BunchOfDates : BaseFood
	{
		[Constructable]
		public BunchOfDates() : base( 0x1727 )
		{
            Name = "a bunch of dates";
			Stackable = true;
			Weight = 1.0;
			//Hue = 1866;
		}

		public BunchOfDates( Serial serial ) : base( serial )
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
