using System;
using Server.Network;

namespace Server.Items
{
	public class ValCandy : BaseFood
	{

		[Constructable]
		public ValCandy() : base( 0x09EA )
		{
            if (Utility.Random(100) < 100) 
		    switch ( Utility.Random( 10 ))  
            {
                case 0: Name = "URA QT"; break;
                case 1: Name = "No.1 Fan"; break;
                case 2: Name = "Kiss Me"; break;
                case 3: Name = "2 COOL"; break;
                case 4: Name = "Be Mine"; break;
                case 5: Name = "Dream Team"; break;
                case 6: Name = "U RULE"; break;
                case 7: Name = "URA KILLA"; break;
                case 8: Name = "UR GREAT"; break;
                case 9: Name = "Heart Of Gold"; break;
            }
			
			Hue = Utility.RandomList( 1060, 1165, 1166, 1167, 1167, 1168, 1169,
			                        1155, 1156, 1157, 1158, 2077, 2078, 2079, 2097, 2981,
                                    1170, 1174, 1969, 1158 );
            Weight = 0.1;
			FillFactor = 10;
			Stackable = false;
            
		}

		public ValCandy( Serial serial ) : base( serial )
		{
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060662, "Valentines Day\t2007" );
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
