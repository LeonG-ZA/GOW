using System;
using System.Reflection;
using Server.Gumps;

namespace Server.Items
{
    public class TVController : Item
    {

        private TVController m_link;
        private string m_message;
        private byte m_invokes;
        private int m_lsid, m_id;

        #region command propertys

        [CommandProperty(AccessLevel.GameMaster)]
        public string Message
        {
            get { return m_message; }
            set
            {
                m_message = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TVController SpeechLink
        {
            get { return m_link; }
            set
            {
                m_link = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public byte INvokesEvent
        {
            get { return m_invokes; }
            set
            {
                m_invokes = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ReactsON
        {
            get { return m_id; }
            set
            {
                m_id = value;
                InvalidateProperties();

            }
        }
        #endregion

        [Constructable]
        public TVController() : base(0xEBF)
        {
            m_invokes = 1;
            Name = "TVController";
            Visible = false;
        }

        public TVController(Serial serial) : base(serial)
        {
        }


        public TVController GetController(int sid, int wantedid)
        {
            if (sid == m_lsid)
            {
                return null;
            }

            m_lsid = sid;

            if (wantedid == m_id)
            {
                return this;
            }
            else if (m_link == null || m_link.Deleted)
            {
                return null;
            }
            else
            {
                return m_link.GetController(sid, wantedid);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((Item)m_link);
            writer.Write((string)m_message);
            writer.Write((byte)m_invokes);
            writer.Write((int)m_id);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_link = reader.ReadItem() as TVController;
            m_message = reader.ReadString();
            m_invokes = reader.ReadByte();
            m_id = reader.ReadInt();
        }

        #region tootip
        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            Type x = this.GetType();
            PropertyInfo[] mem = x.GetProperties();
            PropertyInfo m;
            string par = "";
            string ins = "";
            object o = null;
            for (int i = 0; i < mem.Length; i++)
            {
                m = mem[i];
                if (m.DeclaringType == typeof(Item))
                    break;
                o = m.GetValue(this, null);
                par += ins + string.Format("{0}: {1}", m.Name, ((o == null) ? "null" : ((o is Item) ? ((Item)o).Name : ((o is Mobile) ? ((Mobile)o).Name : o))));
                ins = "\n";
            }
            list.Add(1049644, par);
        }
        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel < AccessLevel.GameMaster)
                return;
            InvalidateProperties();
            from.SendGump(new PropertiesGump(from, this));
        }

        #endregion
    }
}