using System;
using Server;
using Server.AllHues;

namespace Server.Items
{
    [Flipable(0x14F0, 0x14EF)]
	public class OphidianOrders : Item
    {
		[Constructable]
        public OphidianOrders()
            : base(0x14F0)
		{
			Name = "Ophidian Orders";
            Hue = AllHuesInfo.OphidianRatOrd;
			Weight = 1.0;
		}

        public OphidianOrders(Serial serial)
            : base(serial)
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