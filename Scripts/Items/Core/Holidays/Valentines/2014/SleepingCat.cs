using System;

namespace Server.Items
{
    [Flipable]
    public class SleepingCat : Item
    {
        private static string[] m_StaffNames = new string[] { "Bleak", "Brutrin", "Kyronix", "Mesanna", "Misk", "MrsTroublemaker", "Myrmidon", "Onifrk", "Rakban", "Stethun" };

        public static string[] Names { get { return m_StaffNames; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public string CatName { get { return m_CatName; } set { m_CatName = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SoundID { get { return m_SoundID; } set { m_SoundID = value; InvalidateProperties(); } }

        private string m_CatName;
        private int m_SoundID;
        [Constructable]
        public SleepingCat(string catName)
            : base()
        {
            ItemID = Utility.RandomList(0x63DC, 0x63E2, 0x63D6);
            m_CatName = catName;
            Hue = Utility.RandomList(2500, 1105, 646, 0, 2725);
            SoundID = 0x676;
            Weight = 1.0;
        }

        [Constructable]
        public SleepingCat()
            : this(m_StaffNames[Utility.Random(m_StaffNames.Length)])
        {
        }

        public SleepingCat(Serial serial)
            : base(serial)
        {
        }

        public void Flip()
        {
            switch (ItemID)
            {
                case 0x63DC: ItemID = 0x63DF; break;
                case 0x63DF: ItemID = 0x63DC; break;
                case 0x63DD: ItemID = 0x63E0; break;
                case 0x63DE: ItemID = 0x63E1; break;
                case 0x63E2: ItemID = 0x63E5; break;
                case 0x63E5: ItemID = 0x63E2; break;
                case 0x63E3: ItemID = 0x63E6; break;
                case 0x63E4: ItemID = 0x63E7; break;
                case 0x63D6: ItemID = 0x63D9; break;
                case 0x63D9: ItemID = 0x63D6; break;
                case 0x63D7: ItemID = 0x63DA; break;
                case 0x63D8: ItemID = 0x63DB; break;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
            }
            else
                from.PlaySound(m_SoundID);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add(1154691, m_CatName); // A Kitten Raised by ~1_NAME~
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, 1154691, m_CatName); // A Kitten Raised by ~1_NAME~
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_CatName);
            writer.WriteEncodedInt((int)m_SoundID);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_CatName = reader.ReadString();
            m_SoundID = reader.ReadEncodedInt();
            Utility.Intern(ref m_CatName);
        }
    }
}