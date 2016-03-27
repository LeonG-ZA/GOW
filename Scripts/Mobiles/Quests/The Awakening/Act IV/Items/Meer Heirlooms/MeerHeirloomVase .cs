namespace Server.Items
{
    public class MeerHeirloomVase  : Item
	{
		[Constructable]
        public MeerHeirloomVase ()
            : base(0x42B2)
        {
            Hue = 2724;
        }

        public override int LabelNumber { get { return 1153854; } }// Meer Heirloom Vase 

        public MeerHeirloomVase(Serial serial)
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