namespace Server.Items
{
    public class JukanBones  : Item
	{
		[Constructable]
        public JukanBones ()
            : base(0x1B0F)
        {
            Hue = 2673;
        }

        public override int LabelNumber { get { return 1153849; } }// Jukan Bones

        public JukanBones(Serial serial)
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