using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Multis;
using Server.Prompts;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Items;

namespace Server.Items
{
    public class PowerScrollBook : Item, ISecurable
    {
        private ArrayList m_Entries;
        private PowerScrollFilter m_Filter;
        private string m_BookName;
        private SecureLevel m_Level;
        private int m_BookCapacity;

        [CommandProperty(AccessLevel.GameMaster)]
        public string BookName
        {
            get { return m_BookName; }
            set { m_BookName = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SecureLevel Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }

        public ArrayList Entries
        {
            get { return m_Entries; }
        }

        public PowerScrollFilter Filter
        {
            get { return m_Filter; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BookCapacity
        {
            get { return m_BookCapacity; }
            set { m_BookCapacity = value; }
        }

        [Constructable]
        public PowerScrollBook()
            : base(0x9AA7)
        {
            Name = "power scroll book";//1155684 this the number needed for the book name out of the uo files
            Weight = 1.0;
            Hue = 1153;

            m_BookCapacity = 300;
            m_Entries = new ArrayList();
            m_Filter = new PowerScrollFilter();

            m_Level = SecureLevel.CoOwners;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
                from.LocalOverheadMessage(Network.MessageType.Regular, 0x3B2, 1019045);
            else if (m_Entries.Count == 0)
                from.SendLocalizedMessage(1062381);
            else if (from is PlayerMobile)
                from.SendGump(new PowerScrollBookGump((PlayerMobile)from, this));
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is PowerScroll)
            {
                if (!IsChildOf(from.Backpack))
                {
                    from.SendMessage("You must have the book in your backpack to add Power Scrolls to it.");
                    return false;
                }
                else if (m_Entries.Count < m_BookCapacity)
                {
                    m_Entries.Add(new PowerScrollBookEntry((PowerScroll)dropped));
                    InvalidateProperties();

                    from.SendLocalizedMessage(1155692); // You add the scroll to your Power Scroll book.

                    if (from is PlayerMobile)
                        from.SendGump(new PowerScrollBookGump((PlayerMobile)from, this));

                    dropped.Delete();
                    return true;
                }
                else
                {
                    from.SendMessage("That book is full of power scrolls.");
                    return false;
                }
            }
            else if (dropped is StatCapScroll)
            {
                from.SendLocalizedMessage(1155693);//This item is impermissible and can not be added to the book.
                return false;
            }

            from.SendLocalizedMessage(1155691); // This book only holds Power Scrolls.
            return false;
        }

        public PowerScrollBook(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            
            writer.Write((int)m_BookCapacity);
            writer.Write((int)m_Level);
            writer.Write(m_BookName);

            m_Filter.Serialize(writer);

            writer.WriteEncodedInt((int)m_Entries.Count);

            for (int i = 0; i < m_Entries.Count; ++i)
            {
                object obj = m_Entries[i];

                if (obj is PowerScrollBookEntry)
                {
                    writer.WriteEncodedInt(1);
                    ((PowerScrollBookEntry)obj).Serialize(writer);
                }
                else
                {
                    writer.WriteEncodedInt(-1);
                }
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    m_BookCapacity = reader.ReadInt();
                    m_Level = (SecureLevel)reader.ReadInt();
                    m_BookName = reader.ReadString();
                    m_Filter = new PowerScrollFilter(reader);

                    int count = reader.ReadEncodedInt();

                    m_Entries = new ArrayList(count);

                    for (int i = 0; i < count; ++i)
                    {
                        int v = reader.ReadEncodedInt();

                        switch (v)
                        {
                            case 1: m_Entries.Add(new PowerScrollBookEntry(reader)); break;
                        }
                    }

                    break;
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add("Power scrolls in book: {0}", m_Entries.Count.ToString());

            if (m_BookName != null && m_BookName.Length > 0)
                list.Add(1062481, m_BookName);

        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.CheckAlive() && IsChildOf(from.Backpack))
            {
                list.Add(new NameBookEntry(from, this));
            }

            SetSecureLevelEntry.AddTo(from, this, list);
        }

        private class NameBookEntry : ContextMenuEntry
        {
            private Mobile m_From;
            private PowerScrollBook m_Book;

            public NameBookEntry(Mobile from, PowerScrollBook book)
                : base(6216)
            {
                m_From = from;
                m_Book = book;
            }

            public override void OnClick()
            {
                if (m_From.CheckAlive() && m_Book.IsChildOf(m_From.Backpack))
                {
                    m_From.Prompt = new NameBookPrompt(m_Book);
                    m_From.SendLocalizedMessage(1062479);
                }
            }
        }

        private class NameBookPrompt : Prompt
        {
            private PowerScrollBook m_Book;

            public NameBookPrompt(PowerScrollBook book)
            {
                m_Book = book;
            }

            public override void OnResponse(Mobile from, string text)
            {
                if (text.Length > 40)
                    text = text.Substring(0, 40);

                if (from.CheckAlive() && m_Book.IsChildOf(from.Backpack))
                {
                    m_Book.BookName = Utility.FixHtml(text.Trim());

                    from.SendMessage("The power scroll book's name has been changed.");
                }
            }

            public override void OnCancel(Mobile from)
            {
            }
        }
    }
}