using System;
using System.Collections.Generic;

namespace Server.Items
{
    public class EventBackpackSeeker : BaseToggler
    {
        private Item m_CountLink;
        private bool m_CountStackAmount = false;
        private bool m_UseCountAsSignalValue = false;
        private bool m_SendSignalForEachItem = false;
        private bool m_RemoveSeeked = false;

        private string m_SeekedTypeName = "";
        private string m_SeekedName = "";
        private int m_SeekedItemID = -1;
        private int m_SeekedHue = -1;
        private int m_SeekUpTo = 1;

        private Container m_seekIn;


        private bool m_stopDungSignal = false;

        private Type SeekedType = null;

        #region CP
        [CommandProperty(AccessLevel.GameMaster)]
        public Container SeekIn
        {
            get { return m_seekIn; }
            set { m_seekIn = value; }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool RemoveSeeked
        {
            get { return m_RemoveSeeked; }
            set { m_RemoveSeeked = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int SeekUpTo
        {
            get { return m_SeekUpTo; }
            set { m_SeekUpTo = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool StopDungSignal
        {
            get { return m_stopDungSignal; }
            set { m_stopDungSignal = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool CountStackAmount
        {
            get { return m_CountStackAmount; }
            set { m_CountStackAmount = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool UseCountAsSignalValue
        {
            get { return m_UseCountAsSignalValue; }
            set { m_UseCountAsSignalValue = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool SendSignalForEachItem
        {
            get { return m_SendSignalForEachItem; }
            set { m_SendSignalForEachItem = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public string SeekedTypeName
        {
            get { return m_SeekedTypeName; }
            set
            {
                if (value == null)
                    value = "";
                m_SeekedTypeName = value;

                if (m_SeekedTypeName != "")
                {
                    SeekedType = ScriptCompiler.FindTypeByName(m_SeekedTypeName, true);
                    m_SeekedTypeName = (SeekedType != null) ? SeekedType.ToString() : "";
                }
                else
                    SeekedType = null;

                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public string SeekedName
        {
            get { return m_SeekedName; }
            set { if (value == null) value = ""; m_SeekedName = value.ToLower(); InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int SeekedItemID
        {
            get { return m_SeekedItemID; }
            set { m_SeekedItemID = value; InvalidateProperties(); }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int SeekedHue
        {
            get { return m_SeekedHue; }
            set { m_SeekedHue = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Item CountLink
        {
            get { return m_CountLink; }
            set
            {
                if (value is IToggler)
                {
                    m_CountLink = value;
                }
                else
                    m_CountLink = null;
                InvalidateProperties();
            }
        }
        #endregion

        [Constructable]
        public EventBackpackSeeker() : this(8483)
        {
        }
        public EventBackpackSeeker(int ItemID) : base(ItemID)
        {
            Name = "EventBackpackSeeker";
        }

        protected override int Rnd()
        {
            lsidc = base.Rnd();
            return lsidc;
        }
        private int lsidc = 0;//nastavovano v rnd
        public override bool Toggle(byte state, Mobile who, int sid)
        {
            if (sid == lsid || lsidc == sid)
            {
                return false;
            }
            lsid = sid;

            bool ven2 = true;
            if (state > 0)
            {
                if ((who != null && who.Player && who.Backpack != null) || m_seekIn != null)
                {
                    List<Item> items = new List<Item>();
                    if (m_seekIn != null)
                    {
                        FindItems(m_seekIn, items);
                    }
                    else
                    {
                        FindItems(who.Backpack, items);
                    }

                    if (items.Count > 0)
                    {
                        int cnt = GetCnt(items);
                        if (m_SeekUpTo > 0)//hledat urèitý poèet
                        {
                            if (cnt >= m_SeekUpTo)
                            {
                                if (m_CountLink != null && !m_CountLink.Deleted)
                                {
                                    if (m_SendSignalForEachItem)
                                    {
                                        for (int i = 0; i < cnt; i++)
                                        {
                                            ((IToggler)CountLink).Toggle((byte)((m_UseCountAsSignalValue) ? m_SeekUpTo : 1), who, Rnd());
                                        }
                                    }
                                    else
                                    {
                                        ((IToggler)CountLink).Toggle((byte)((m_UseCountAsSignalValue) ? m_SeekUpTo : 1), who, Rnd());
                                    }
                                }
                                int curcnt = m_SeekUpTo;
                                if (m_RemoveSeeked)
                                {
                                    for (int i = items.Count - 1; i >= 0 && curcnt > 0; i--)
                                    {
                                        if (items[i].Amount <= curcnt)
                                        {
                                            curcnt -= items[i].Amount;
                                            items[i].Delete();
                                        }
                                        else
                                        {
                                            items[i].Amount -= curcnt;
                                            curcnt = 0;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ven2 = false;
                            }
                        }
                        else//nehledame urcity pocet
                        {
                            if (m_CountLink != null && !m_CountLink.Deleted)
                            {
                                if (m_SendSignalForEachItem)
                                {
                                    for (int i = 0; i < cnt; i++)
                                    {
                                        ((IToggler)CountLink).Toggle((byte)((m_UseCountAsSignalValue) ? cnt : 1), who, Rnd());
                                    }
                                }
                                else
                                {
                                    ((IToggler)CountLink).Toggle((byte)((m_UseCountAsSignalValue) ? cnt : 1), who, Rnd());
                                }
                            }

                            if (m_RemoveSeeked)
                            {
                                for (int i = items.Count - 1; i >= 0; i--)
                                {
                                    items[i].Delete();
                                }
                            }
                        }
                    }
                    else
                        ven2 = false;
                }
            }
            bool ven = true;
            if (!m_stopDungSignal)
            {
                ven = (Link == null || Link.Deleted) ? true : Link.Toggle(state, who, sid);
            }
            return true & ven & ven2;
        }

        public int GetCnt(List<Item> l)
        {

            if (m_CountStackAmount)
            {
                int c = 0;
                for (int i = 0; i < l.Count; i++)
                {
                    if (l[i].Stackable)
                    {
                        c += l[i].Amount;
                    }
                    else
                    {
                        c++;
                    }
                }
                return c;
            }
            else
            {
                return l.Count;
            }
        }
        public EventBackpackSeeker(Serial serial) : base(serial)
        {
        }
        public bool MatchType(Item i)
        {
            if (SeekedType == null || SeekedType.IsInstanceOfType(i))
            {
                return true;
            }
            return false;
        }

        public bool MatchItemID(Item i)
        {
            if (m_SeekedItemID < 0 || i.ItemID == m_SeekedItemID)
            {
                return true;
            }
            return false;
        }

        public bool MatchHue(Item i)
        {
            if (m_SeekedHue < 0 || i.Hue == m_SeekedHue)
            {
                return true;
            }
            return false;
        }
        public bool MatchName(Item i)
        {
            if (m_SeekedName == "" || (i.Name != null && i.Name.ToLower() == m_SeekedName))
            {
                return true;
            }

            return false;
        }

        public void FindItems(Container c, List<Item> founded)
        {
            for (int i = 0; i < c.Items.Count; i++)
            {
                Item x = c.Items[i];
                if (MatchHue(x) && MatchItemID(x) && MatchName(x) && MatchType(x))
                {
                    founded.Add(x);
                }
                if (x is Container && (x is LockableContainer && !((LockableContainer)x).Locked))
                {
                    FindItems((Container)x, founded);
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);//version
            writer.Write((Item)m_CountLink);
            writer.Write((bool)m_CountStackAmount);
            writer.Write((bool)m_UseCountAsSignalValue);
            writer.Write((bool)m_SendSignalForEachItem);
            writer.Write((bool)m_RemoveSeeked);
            writer.Write((string)m_SeekedTypeName);
            writer.Write((string)m_SeekedName);
            writer.Write((int)m_SeekedItemID);
            writer.Write((int)m_SeekedHue);
            writer.Write((int)m_SeekUpTo);
            writer.Write((bool)m_stopDungSignal);
            writer.Write((Item)m_seekIn);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            CountLink = reader.ReadItem();
            m_CountStackAmount = reader.ReadBool();
            m_UseCountAsSignalValue = reader.ReadBool();
            m_SendSignalForEachItem = reader.ReadBool();
            m_RemoveSeeked = reader.ReadBool();
            m_SeekedTypeName = reader.ReadString();
            SeekedType = ScriptCompiler.FindTypeByFullName(m_SeekedTypeName);
            m_SeekedName = reader.ReadString();
            m_SeekedItemID = reader.ReadInt();
            m_SeekedHue = reader.ReadInt();
            m_SeekUpTo = reader.ReadInt();
            m_stopDungSignal = reader.ReadBool();

            if (version > 0)
            {
                m_seekIn = reader.ReadItem() as Container;
            }
        }
    }
}
