namespace Server.Items
{
    public class RuinedJukanTome : Item
	{
		[Constructable]
        public RuinedJukanTome()
            : base(0x1E20)
        {
            Hue = 2724;
        }

        public override int LabelNumber { get { return 1153852; } }// Ruined Jukan Tome

        public RuinedJukanTome(Serial serial)
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