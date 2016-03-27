namespace Server.Items
{
    public class KegofMeerAle  : Item
	{
		[Constructable]
        public KegofMeerAle ()
            : base(0x1AD6)
        {
            Hue = 2724;
        }

        public override int LabelNumber { get { return 1153855; } }// Keg of Meer Ale

        public KegofMeerAle(Serial serial)
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