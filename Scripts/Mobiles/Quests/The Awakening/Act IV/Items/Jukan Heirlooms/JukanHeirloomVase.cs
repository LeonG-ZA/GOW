namespace Server.Items
{
    public class JukanHeirloomVase : Item
	{
		[Constructable]
        public JukanHeirloomVase()
            : base(0x42B3)
        {
            Hue = 2707;
        }

        public override int LabelNumber { get { return 1153842; } }// Jukan Heirloom Vase

        public JukanHeirloomVase(Serial serial)
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