namespace Server.Items
{
    public class EventManualToggle : BaseToggle
    {
        private byte m_signalValue = 1;

        private Mobile m_SendedMobile = null;


        [CommandProperty(AccessLevel.GameMaster)]
        public byte SignalValue
        {
            get { return m_signalValue; }
            set { m_signalValue = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile SendedMobile
        {
            get { return m_SendedMobile; }
            set { m_SendedMobile = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DoToggle
        {
            get { return false; }
            set
            {
                if (Link != null)
                {
                    if (value)
                    {
                        Link.Toggle(m_signalValue, m_SendedMobile, Rnd());
                    }
                    else
                    {
                        Link.Toggle(0, m_SendedMobile, Rnd());
                    }
                }
            }
        }

        [Constructable]
        public EventManualToggle() : this(2502)
        {
        }

        [Constructable]
        public EventManualToggle(int ItemID) : base(ItemID)
        {
            Name = "EventManualToggle";

        }

        public EventManualToggle(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((byte)m_signalValue);
            writer.Write(m_SendedMobile);


        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_signalValue = reader.ReadByte();
            m_SendedMobile = reader.ReadMobile();
        }
    }
}