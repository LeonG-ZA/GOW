using System;
using Server;

namespace Server.Items
{
    [FlipableAttribute(0x18D9, 0x18DA)]
	public class FreshlyPickedRose : Item
	{
        private static string[] m_StaffNames = new string[]
			{
               "Cougar",
               "Dummergirl",
               "Iannan",
               "Jawbra",
               "Mesanna",
               "Misk",
               "MrsTroubleMaker",
               "Phoenix",
               "Rend",
               "Soulrais",
               "Supreem",
               "TheGrimmOmen",
               "Treaver",
               "Uriah",
               "Wasia",
               "Zoer",
               "Aeon",
               "Adris",
               "Agustus",
               "Aname",
               "Asiantam",
               "Autolycus",
               "Barnaby",
               "Bennu",
               "Borbarad",
               "DeAngelo",
               "Drosselmeyer",
               "Dudley",
               "Eira",
               "Elizabella",
               "Emile Layne",
               "Faine Morgan",
               "Fiorella",
               "Helios",
               "Infinity",
               "Isabella",
               "Malachi",
               "Miko",
               "Mystique",
               "Nekomata",
               "Promethium",
               "ThomasPyewacket",
               "Sangria",
               "Sarakan",
               "Seppo"
			};

        public static string[] Names { get { return m_StaffNames; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public string RoseName { get { return m_RoseName; } set { m_RoseName = value; } }

        private string m_RoseName;

		[Constructable]
        public FreshlyPickedRose(string roseName)
            : base(0x18D9)
		{
            m_RoseName = roseName;
            Weight = 1.0;
		}

        [Constructable]
        public FreshlyPickedRose()
            : this(m_StaffNames[Utility.Random(m_StaffNames.Length)])
        {
        }

        public FreshlyPickedRose(Serial serial)
            : base(serial)
		{
		}

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add(1114844, m_RoseName); // 
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, 1114844, m_RoseName); // 
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
            writer.Write(m_RoseName);
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
            m_RoseName = reader.ReadString();
            Utility.Intern(ref m_RoseName);
		}
	}
}
