namespace Server.Items
{
    public class CharredJukanHistoricalScroll  : Item
	{
		[Constructable]
        public CharredJukanHistoricalScroll ()
            : base(0x0E34)
        {
            Hue = 2019;
        }

        public override int LabelNumber { get { return 1153851; } }// Charred Jukan Historical Scroll 

        public CharredJukanHistoricalScroll(Serial serial)
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