namespace Server.Items
{
    public class TVInvoker : Item
    {
        private int m_id;
        private int m_vendorid;

        [CommandProperty(AccessLevel.GameMaster)]
        public int ReplyID
        {
            get { return m_id; }
            set { m_id = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int VendorID
        {
            get { return m_vendorid; }
            set { m_vendorid = value; }
        }

        [Constructable]
        public TVInvoker() : base(0x227A)
        {
            Name = "QuestScroll";
            m_id = 1;
        }

        public TVInvoker(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write((int)m_id);
            writer.Write((int)m_vendorid);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_id = reader.ReadInt();
            m_vendorid = reader.ReadInt();
        }
    }
}