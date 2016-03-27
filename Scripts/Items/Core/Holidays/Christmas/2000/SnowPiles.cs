namespace Server.Items
{
    public class SnowPileDeco : Item
    {
        private static readonly int[] SnowIds = new int[] { 0x8E2, 0x8E0, 0x8E6, 0x8E5, 0x8E3 };
        [Constructable]
        public SnowPileDeco()
            : this(SnowIds[Utility.Random(SnowIds.Length)])
        {
        }

        [Constructable]
        public SnowPileDeco(int itemid)
            : base(itemid)
        {
            Hue = 0x481;
        }

        public SnowPileDeco(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "Snow Pile";
            }
        }
        public override double DefaultWeight
        {
            get
            {
                return 2.0;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}