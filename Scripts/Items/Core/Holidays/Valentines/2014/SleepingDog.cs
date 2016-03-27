using System;

namespace Server.Items
{
    [Flipable]
    public class SleepingDog : Item
    {
        private static string[] m_StaffNames = new string[] { "Bleak", "Brutrin", "Kyronix", "Mesanna", "Misk", "MrsTroublemaker", "Myrmidon", "Onifrk", "Rakban", "Stethun" };

        public static string[] Names { get { return m_StaffNames; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public string DogName { get { return m_DogName; } set { m_DogName = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SoundID { get { return m_SoundID; } set { m_SoundID = value; InvalidateProperties(); } }

        private string m_DogName;
        private int m_SoundID;
        [Constructable]
        public SleepingDog(string dogName)
            : base()
        {
            ItemID = Utility.RandomList(0x63EE, 0x63F4, 0x63E8);
            m_DogName = dogName;
            Hue = Utility.RandomList(49, 2724, 1109, 0);
            SoundID = 0x675;
            Weight = 1.0;
        }

        [Constructable]
        public SleepingDog()
            : this(m_StaffNames[Utility.Random(m_StaffNames.Length)])
        {
        }

        public SleepingDog(Serial serial)
            : base(serial)
        {
        }

        public void Flip()
        {
            switch (ItemID)
            {
                case 0x63EE: ItemID = 0x63F1; break;
                case 0x63F1: ItemID = 0x63EE; break;
                case 0x63EF: ItemID = 0x63F2; break;
                case 0x63F0: ItemID = 0x63F3; break;
                case 0x63F4: ItemID = 0x63F7; break;
                case 0x63F7: ItemID = 0x63F4; break;
                case 0x63F5: ItemID = 0x63F8; break;
                case 0x63F6: ItemID = 0x63F9; break;
                case 0x63E8: ItemID = 0x63EB; break;
                case 0x63EB: ItemID = 0x63E8; break;
                case 0x63E9: ItemID = 0x63EC; break;
                case 0x63EA: ItemID = 0x63ED; break;
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
            list.Add(1154692, m_DogName); // A Puppy Raised by ~1_NAME~
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, 1154692, m_DogName); // A Puppy Raised by ~1_NAME~
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_DogName);
            writer.WriteEncodedInt((int)m_SoundID);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_DogName = reader.ReadString();
            m_SoundID = reader.ReadEncodedInt();
            Utility.Intern(ref m_DogName);
        }
    }
}