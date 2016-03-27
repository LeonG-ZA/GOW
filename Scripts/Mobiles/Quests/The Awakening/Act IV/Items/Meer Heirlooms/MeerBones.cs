namespace Server.Items
{
    public class MeerBones  : Item
	{
		[Constructable]
        public MeerBones ()
            : base(0x1B10)
        {
            Hue = 2709;
        }

        public override int LabelNumber { get { return 1153849; } }//Meer Bones

        public MeerBones(Serial serial)
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