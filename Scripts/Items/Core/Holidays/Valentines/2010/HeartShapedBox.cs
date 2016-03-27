﻿using System;

namespace Server.Items
{
	public class HeartShapedBox : Container
	{
		public override int LabelNumber { get { return 1097762; } } // heart shaped box

		[Constructable]
		public HeartShapedBox()
			: base( Utility.RandomList( 0x49CC, 0x49D0 ) )
		{
			Weight = 1.0;
			GumpID = 0x120;

			for ( int i = 0; i < 6; i++ )
			{
				for ( int j = 0; j < 4; j++ )
				{
                    PlaceItemIn(this, 60 + (10 * i), 35 + (j * 13), new ValentineChocolate());
				}
			}
		}

        private static void PlaceItemIn(Container parent, int x, int y, Item item)
        {
            parent.AddItem(item);
            item.Location = new Point3D(x, y, 0);
        }

		public HeartShapedBox( Serial serial )
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