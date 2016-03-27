namespace Server.Items
{
    public class MeerUrn  : Item
	{
		[Constructable]
        public MeerUrn ()
            : base(0x0B47)
        {
            Hue = 2720;
        }

        public override int LabelNumber { get { return 1153860; } }// Meer Urn

        public MeerUrn(Serial serial)
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