using System;
using Server.Mobiles;

namespace Server.Items
{
    public class EventTeleportToggler : BaseToggler
    {
        private Point3D m_destination;
        private bool m_pets, m_players;
        private int m_state;
        private Map m_map;

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Destination
        {
            get { return m_destination; }
            set { m_destination = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool TeleportPets
        {
            get { return m_pets; }
            set { m_pets = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool TeleportPlayers
        {
            get { return m_players; }
            set { m_players = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TeleportOnState
        {
            get { return m_state; }
            set { m_state = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map MapDest
        {
            get { return m_map; }
            set { m_map = value; InvalidateProperties(); }
        }


        [Constructable]
        public EventTeleportToggler() : this(0x9D7)
        {
        }

        public EventTeleportToggler(int ItemID) : base(ItemID)
        {
            Name = "EventTeleportToggler";
            m_destination = new Point3D();
            m_state = 1;
            m_pets = true;
            m_players = true;
        }

        private void Delay_Callback(object state)
        {
            Map map = m_map;
            Mobile who;

            if (state is Mobile && !(state as Mobile).Deleted)
            {
                who = state as Mobile;
            }
            else
            {
                return;
            }

            if (map == null || map == Map.Internal)
            {
                map = this.Map;
            }

            Point3D loc = m_destination;
            if (loc == Point3D.Zero)
            {
                loc = this.Location;
            }

            who.MoveToWorld(loc, map);
            if (m_pets)
            {
                BaseCreature.TeleportPets(who, loc, map);
            }

        }

        public override bool Toggle(byte state, Mobile who, int sid)
        {
            if (sid == lsid)
            {
                return false;
            }

            lsid = sid;

            bool send = false;

            if (!(who == null || who.Deleted))
            {

                if (state == m_state)
                {
                    if (who.Player && m_players)
                    {
                        Timer.DelayCall(TimeSpan.FromSeconds(0.3), new TimerStateCallback(Delay_Callback), who);
                    }
                    else if (who is BaseCreature && m_pets)
                    {
                        if ((who as BaseCreature).Controlled)
                        {
                            Timer.DelayCall(TimeSpan.FromSeconds(0.3), new TimerStateCallback(Delay_Callback), who);
                        }
                    }
                }
                send = true;
            }

            bool ven = (Link == null || Link.Deleted) ? true : Link.Toggle(state, who, sid);
            return send & ven;
        }


        public EventTeleportToggler(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
            writer.Write(m_destination);
            writer.Write((bool)m_pets);
            writer.Write((bool)m_players);
            writer.Write((int)m_state);
            writer.Write((Map)m_map);

        }


        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_destination = reader.ReadPoint3D();
            m_pets = reader.ReadBool();
            m_players = reader.ReadBool();
            m_state = reader.ReadInt();
            m_map = reader.ReadMap();
        }
    }
}