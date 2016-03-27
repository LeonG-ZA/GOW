namespace Server.Items
{
    public class RuinedMeerTomes : Item
	{
		[Constructable]
        public RuinedMeerTomes()
            : base(0x1E21)
        {
            Hue = 2724;
        }

        public override int LabelNumber { get { return 1153862; } }// Ruined Meer Tomes

        public RuinedMeerTomes(Serial serial)
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