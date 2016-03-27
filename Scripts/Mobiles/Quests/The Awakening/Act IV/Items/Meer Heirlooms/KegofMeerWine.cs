namespace Server.Items
{
    public class KegofMeerWine  : Item
	{
		[Constructable]
        public KegofMeerWine ()
            : base(0x1AD6)
        {
            Hue = 1460;
        }

        public override int LabelNumber { get { return 1153856; } }// Keg of Meer Wine

        public KegofMeerWine(Serial serial)
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