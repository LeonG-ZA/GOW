using System;

namespace Server.Items
{
    public class EventMoveToggler : BaseToggler
    {
        private Item m_item;
        private Point3D m_location;
        private Point3D m_destination;
        private TimeSpan m_delay;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Delay
        {
            get { return m_delay; }
            set { m_delay = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Item OffsetedItem
        {
            get { return m_item; }
            set
            {
                m_item = value;
                m_location = new Point3D(m_item.Location);
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Destination
        {
            get { return m_destination; }
            set { m_destination = value; InvalidateProperties(); }
        }

        private void Delay_Callback(object state)
        {
            if (Link != null && !(Link as Item).Deleted)
            {
                if (state is Mobile)
                {
                    Link.Toggle(0, state as Mobile, Rnd());
                }
                else
                {
                    Link.Toggle(0, null, Rnd());
                }
            }
            this.Toggle(0, null, Rnd());
        }

        [Constructable]
        public EventMoveToggler() : this(0xE7A)
        {
        }

        public EventMoveToggler(int ItemID) : base(ItemID)
        {
            Name = "EventMoveToggler";
            m_location = new Point3D();
            m_destination = new Point3D();
        }
        public override bool Toggle(byte state, Mobile who, int sid)
        {
            if (sid == lsid)
                return false;
            lsid = sid;

            bool send = false;


            if (!(m_item == null || m_item.Deleted))
            {

                if (state == 0)
                {
                    m_item.Location = m_location;
                }
                else
                {
                    m_item.Location = m_destination;
                    if (m_delay != TimeSpan.Zero)
                        Timer.DelayCall(m_delay, new TimerStateCallback(Delay_Callback), who);
                }
                send = true;
            }
            bool ven = (Link == null || Link.Deleted) ? true : Link.Toggle(state, who, sid);
            return send & ven;
        }


        public EventMoveToggler(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version 
            writer.Write(m_location);
            writer.Write(m_destination);
            writer.Write(m_item);
            writer.Write((TimeSpan)m_delay);
        }


        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_location = reader.ReadPoint3D();
            m_destination = reader.ReadPoint3D();
            m_item = reader.ReadItem();
            m_delay = reader.ReadTimeSpan();
        }
    }
}