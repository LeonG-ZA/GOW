using System;

namespace Server.Items
{
    public class EventEffectToggler : BaseToggler
    {
        private Point3D m_destination;
        private bool m_tomobile, m_withmobile;
        private TimeSpan m_duration;
        private int m_hue, m_speed;
        private EventEffectType m_type;
        private int m_animid;

        [CommandProperty(AccessLevel.GameMaster)]
        public int AnimationHue
        {
            get { return m_hue; }
            set { m_hue = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AnimationItemId
        {
            get { return m_animid; }
            set
            {
                if (m_type == EventEffectType.Advanced)
                {
                    m_animid = value;
                }
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AnimationSpeed
        {
            get { return m_speed; }
            set { m_speed = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public EventEffectType Animation
        {
            get { return m_type; }
            set
            {
                m_type = value;
                if (value != EventEffectType.Advanced)
                {
                    m_animid = (int)value;
                }
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Destination
        {
            get { return m_destination; }
            set { m_destination = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DisplayOverMobile
        {
            get { return m_tomobile; }
            set { m_tomobile = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool MovingWithMobile
        {
            get { return m_withmobile; }
            set { m_withmobile = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan EffectDuration
        {
            get { return m_duration; }
            set { m_duration = value; InvalidateProperties(); }
        }

        [Constructable]
        public EventEffectToggler() : this(0x1853)
        {
        }

        public EventEffectToggler(int ItemID) : base(ItemID)
        {
            Name = "EventEffectToggler";
            m_destination = new Point3D();
            m_tomobile = false;
            m_hue = 0;
            m_type = EventEffectType.Bees;
            m_duration = TimeSpan.FromSeconds(1);
        }

        private void SendEffect()
        {
            if (m_type == EventEffectType.Lighting)
            {
                Effects.SendBoltEffect(EffectItem.Create((m_destination == Point3D.Zero) ? this.Location : m_destination, this.Map, EffectItem.DefaultDuration), false, m_hue);
            }
            else if (m_type == EventEffectType.Advanced)
            {
                Effects.SendLocationParticles(EffectItem.Create((m_destination == Point3D.Zero) ? this.Location : m_destination, this.Map, EffectItem.DefaultDuration), (int)m_animid, m_speed, (int)m_duration.TotalSeconds * 5, m_hue, 0, 0, 0);
            }
            else
            {
                Effects.SendLocationParticles(EffectItem.Create((m_destination == Point3D.Zero) ? this.Location : m_destination, this.Map, EffectItem.DefaultDuration), (int)m_type, m_speed, (int)m_duration.TotalSeconds * 5, m_hue, 0, 0, 0);
            }
        }

        private void SendEffect(Mobile m)
        {
            if (!m_withmobile)
            {
                if (m_type == EventEffectType.Lighting)
                {
                    Effects.SendBoltEffect(m, false, m_hue);
                }
                else
                {
                    Effects.SendLocationParticles(m, (int)m_type, m_speed, (int)m_duration.TotalSeconds * 5, m_hue, 0, 0, 0);
                }
            }
            else
            {
                if (m_type == EventEffectType.Lighting)
                {
                    Effects.SendBoltEffect(m, false, m_hue);
                }
                else
                {
                    Effects.SendTargetParticles(m, (int)m_type, m_speed, (int)m_duration.TotalSeconds * 5, m_hue, 0, 0, EffectLayer.Head, 0);
                }
            }

        }

        public override bool Toggle(byte state, Mobile who, int sid)
        {
            if (sid == lsid)
            {
                return false;
            }
            lsid = sid;
            bool send = true;


            if (state != 0)
            {

                if (!m_tomobile)
                {
                    SendEffect();
                    send = true;
                }
                else if (who == null || who.Deleted)
                {
                    send = false;
                }
                else
                {
                    SendEffect(who);
                }
            }
            bool ven = (Link == null || Link.Deleted) ? true : Link.Toggle(state, who, sid);
            return send & ven;
        }


        public EventEffectToggler(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
            writer.Write(m_destination);
            writer.Write((bool)m_tomobile);
            writer.Write((bool)m_withmobile);
            writer.Write((TimeSpan)m_duration);
            writer.Write((int)m_hue);
            writer.Write((int)m_hue);
            writer.Write((int)m_type);
        }


        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_destination = reader.ReadPoint3D();
            m_tomobile = reader.ReadBool();
            m_withmobile = reader.ReadBool();
            m_duration = reader.ReadTimeSpan();
            m_hue = reader.ReadInt();
            m_speed = reader.ReadInt();
            m_type = (EventEffectType)reader.ReadInt();
        }
    }
}