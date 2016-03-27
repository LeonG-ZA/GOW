using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
    [Flipable(0x992D, 0x992E)]
	public class PumpkinCarvingKit : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefPumpkinCarve.CraftSystem; } }

		[Constructable]
		public PumpkinCarvingKit() : base( 0x992D )
		{
		}

		[Constructable]
        public PumpkinCarvingKit(int uses)
            : base(uses, 0x992D)
		{
            Weight = 1.0;
            Hue = 0x3CC;
		}

        public PumpkinCarvingKit(Serial serial)
            : base(serial)
		{
		}

        public override int LabelNumber { get { return 1154271; } }// Jack O' Lantern Carving Kit

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
