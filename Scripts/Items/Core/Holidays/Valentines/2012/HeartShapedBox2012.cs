using System;
using Server;

namespace Server.Items
{
	[FlipableAttribute( 0x49CA, 0x49CB )]
    public class HeartShapedBox2012 : Container
	{
        public override int LabelNumber { get { return 1097771; } } // heart shaped box

		[Constructable]
		public HeartShapedBox2012()
			: base( 0x49CA )
		{
            Weight = 1;
            GumpID = 0x120;
            AddToBox(new CupidsArrow2012(), new Point3D(90, 85, 0));
            AddToBox(new CupidStatue(), new Point3D(130, 55, 0));
		}

        public HeartShapedBox2012(Serial serial)
			: base( serial )
		{
		}

        public virtual void AddToBox(Item item, Point3D loc)
        {
            DropItem(item);
            item.Location = loc;
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
