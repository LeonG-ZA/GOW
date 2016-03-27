namespace Server.Items
{
    public class JukanUrn  : Item
	{
		[Constructable]
        public JukanUrn ()
            : base(0x0B48)
        {
            Hue = 2720;
        }

        public override int LabelNumber { get { return 1153849; } }// Jukan Urn

        public JukanUrn(Serial serial)
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