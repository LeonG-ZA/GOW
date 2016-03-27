namespace Server.Items
{
    public class KegofJukanWine  : Item
	{
		[Constructable]
        public KegofJukanWine ()
            : base(0x1AD6)
        {
            Hue = 1460;
        }

        public override int LabelNumber { get { return 1153846; } }// Keg of Jukan Wine

        public KegofJukanWine(Serial serial)
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