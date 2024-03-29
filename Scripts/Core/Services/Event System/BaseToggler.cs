using System;
using System.Reflection;
using Server.Gumps;

namespace Server.Items
{
    public class BaseToggler : Item, IToggler, IToggle
    {
        private Item m_togler;
        protected int lsid; //last session id

        public BaseToggler() : this(0x970)
        {
        }

        public BaseToggler(int ItemID) : base(ItemID)
        {
            Visible = false;
            Movable = false;
        }

        public virtual bool Toggle(byte state, Mobile who, int sid)
        {
            return true;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Item EventLink
        {
            get { return m_togler; }
            set
            {
                if (value is IToggler || value == null)
                {
                    m_togler = value;
                }
                InvalidateProperties();
            }
        }

        public IToggler Link
        {
            get { return m_togler as IToggler; }
            set
            {
                m_togler = value as Item;
            }
        }

        public BaseToggler(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
            writer.Write((Item)m_togler);
        }


        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_togler = reader.ReadItem();
        }

        protected virtual int Rnd()
        {
            return (Utility.Random(int.MaxValue));
        }

        #region vypis
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
                if (m.GetCustomAttributes(typeof(Server.CommandPropertyAttribute), false).Length <= 0)
                {
                    continue;
                }
                if (m.DeclaringType == typeof(Item))
                {
                    break;
                }
                o = m.GetValue(this, null);
                par += ins + string.Format("{0}: {1}", m.Name, ((o == null) ? "null" : ((o is Item) ? ((Item)o).Name : ((o is Mobile) ? ((Mobile)o).Name : o))));
                ins = "\n";
            }
            list.Add(1049644, par);
        }
        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel < AccessLevel.GameMaster)
            {
                return;
            }
            InvalidateProperties();
            from.SendGump(new PropertiesGump(from, this));
        }

        #endregion

    }
}