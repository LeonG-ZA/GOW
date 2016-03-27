namespace Server.Items
{
    public class KegofJukanMead  : Item
	{
		[Constructable]
        public KegofJukanMead ()
            : base(0x1AD6)
        {
            Hue = 2724;
        }

        public override int LabelNumber { get { return 1153845; } }// Keg of Jukan Mead
        public KegofJukanMead(Serial serial)
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