using System;

namespace Server.Items 
{
    public class BaseStatue : Item, IEngravable
    {
        private string m_EngravedText;

        [CommandProperty(AccessLevel.GameMaster)]
        public string EngravedText
        {
            get { return m_EngravedText; }
            set
            {
                m_EngravedText = value;
                InvalidateProperties();
            }
        }

        public BaseStatue(int itemID) : this(1, itemID)
        {
        }

        public BaseStatue(int amount, int itemID) : base(itemID)
        {
            this.Weight = 10;
        }

        public BaseStatue(Serial serial) : base(serial)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (this.Name == null)
            {
                list.Add(this.LabelNumber);
            }
            else
            {
                list.Add(this.Name);
            }

            if (!String.IsNullOrEmpty(m_EngravedText))
            {
                list.Add(1062613, m_EngravedText);
            }
        }

        public override bool ForceShowProperties
        {
            get
            {
                return ObjectPropertyList.Enabled;
            }
        }

        private static void SetSaveFlag(ref SaveFlag flags, SaveFlag toSet, bool setIf)
        {
            if (setIf)
            {
                flags |= toSet;
            }
        }

        private static bool GetSaveFlag(SaveFlag flags, SaveFlag toGet)
        {
            return ((flags & toGet) != 0);
        }

        [Flags]
        private enum SaveFlag : uint
        {
            None = 0x00000000,
            EngravedText = 0x00000001
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            // Version 1
            writer.Write(m_EngravedText);

            // Version 0
            SaveFlag flags = SaveFlag.None;

            SetSaveFlag(ref flags, SaveFlag.EngravedText, !String.IsNullOrEmpty(m_EngravedText));

            writer.Write((int)flags);

            if (GetSaveFlag(flags, SaveFlag.EngravedText))
            {
                writer.Write(m_EngravedText);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_EngravedText = reader.ReadString();
                        goto case 0;
                    }
                case 0:
                    {
                        SaveFlag flags = (SaveFlag)reader.ReadInt();

                        if (GetSaveFlag(flags, SaveFlag.EngravedText))
                        {
                            m_EngravedText = reader.ReadString();
                        }
                        break;
                    }
            }
        }
    }
}