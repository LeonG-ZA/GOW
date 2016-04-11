namespace Server.Items
{
    public class EventSignalStoper : BaseToggler
    {
        private byte m_stoppedSignalID = 0;

        [CommandProperty(AccessLevel.GameMaster)]
        public byte StoppedSignalID
        {
            get { return m_stoppedSignalID; }
            set { m_stoppedSignalID = value; InvalidateProperties(); }
        }

        [Constructable]
        public EventSignalStoper() : this(0x9FA)
        {
        }

        public EventSignalStoper(int ItemID) : base(ItemID)
        {
            Name = "EventSignalStoper";
        }

        public override bool Toggle(byte state, Mobile who, int sid)
        {
            if (sid == lsid)
            {
                return false;
            }
            lsid = sid;

            if (m_stoppedSignalID == state)
            {
                return true;
            }

            bool ven = (Link == null || (Link as Item).Deleted) ? true : Link.Toggle(state, who, sid);
            return true & ven;
        }


        public EventSignalStoper(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
            writer.Write((byte)m_stoppedSignalID);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_stoppedSignalID = reader.ReadByte();
        }
    }
}