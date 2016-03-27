namespace Server.Items
{
    public class CharredMeerHistoricalScroll  : Item
	{
		[Constructable]
        public CharredMeerHistoricalScroll ()
            : base(0x0E34)
        {
            Hue = 2019;
        }

        public override int LabelNumber { get { return 1153861; } }// Charred Meer Historical Scroll

        public CharredMeerHistoricalScroll(Serial serial)
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