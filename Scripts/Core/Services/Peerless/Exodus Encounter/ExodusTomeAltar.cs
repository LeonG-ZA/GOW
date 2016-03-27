using System;
using Server;
using System.Collections;

namespace Server.Items
{
    public class ExodusTomeAltar : BaseActivation
    {
		[Constructable]
		public ExodusTomeAltar()
			: base( 0xEFE )
		{
			Hue = 1150;
			Mapa = Map.Ilshenar;
			EnterPoint = new Point3D( 2154, 1253, -60 );
		}

        public ExodusTomeAltar(Serial serial)
			: base( serial )
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

			/*int version = */
			reader.ReadInt();
		}
    }
}
           
