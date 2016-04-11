using System;
using Server.Mobiles;

namespace Server.Items
{
    public class EventStepToggle : BaseToggle
    {
        private bool m_active, m_onlyplayer, m_delayable;
        private TimeSpan m_delay;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Delay
        {
            get { return m_delay; }
            set { m_delay = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Active
        {
            get { return m_active; }
            set { m_active = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool PlayerOnly
        {
            get { return m_onlyplayer; }
            set { m_onlyplayer = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool Delayable
        {
            get { return m_delayable; }
            set { m_delayable = value; InvalidateProperties(); }
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

        }

        private void Controll_Callback(object state)
        {
            OnMoveOff(null);
        }

        [Constructable]
        public EventStepToggle() : this(0x1B7A)
        {
        }

        [Constructable]
        public EventStepToggle(int ItemID) : base(ItemID)
        {
            Name = "EventStepToggle";
            m_delayable = false;
            m_active = true;
            m_onlyplayer = false;
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m == null || m.Deleted)
            {
                return false;
            }

            if (m_active)
            {
                if ((m_onlyplayer && (m is PlayerMobile)) || !m_onlyplayer)
                {
                    if (m_delayable)
                    {
                        Timer.DelayCall(m_delay, new TimerStateCallback(Delay_Callback), m);
                    }
                    if (Link != null && !Link.Deleted)
                    {
                        Link.Toggle(1, m, Rnd());
                    }
                }
            }

            return true;
        }

        public override bool OnMoveOff(Mobile m)
        {
            Timer.DelayCall(TimeSpan.FromSeconds(2), new TimerStateCallback(Controll_Callback), null);
            if (m == null || m.Deleted)
            {
                return false;
            }
            if (m_active)
            {
                if (!m_delayable)
                {
                    if ((m_onlyplayer && (m is PlayerMobile)) || !m_onlyplayer)
                    {
                        IPooledEnumerable eable = GetMobilesInRange(0);
                        foreach (object o in eable)
                        {
                            if (o is Mobile)
                            {
                                if (!(o == m))
                                {
                                    return true;
                                }
                            }
                        }

                        if (Link != null && !Link.Deleted)
                        {
                            Link.Toggle(0, m, Rnd());
                        }
                    }
                }
            }
            return true;
        }


        public EventStepToggle(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_active);
            writer.Write((bool)m_onlyplayer);
            writer.Write((bool)m_delayable);
            writer.Write((TimeSpan)m_delay);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_active = reader.ReadBool();
            m_onlyplayer = reader.ReadBool();
            m_delayable = reader.ReadBool();
            m_delay = reader.ReadTimeSpan();

            if (Link != null && !Link.Deleted)
                Timer.DelayCall(TimeSpan.FromSeconds(20), new TimerStateCallback(Delay_Callback), null);
        }
    }
}