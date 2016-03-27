using System;

namespace Server.Items
{
    [FlipableAttribute(0x236E, 0x2371)]
    public class LightOfTheWinterSolstice : Item
    {
        private static readonly string[] HolidayCandleNames = new string[]
        {
           "Ando",
           "Axiom",
           "Binky",
           "Cadillac",
           "Carbon",
           "Cheap Book",
           "Cyrus",
           "Deko",
           "Draconis Rex",
           "Echa Firwood",
           "Ender",
           "Eva",
           "EvilMantis",
           "Excrom",
           "Farmer Farley",
           "Fenris",
           "Fertbert",
           "Foster",
           "Galess",
           "GM Blatry",
           "GM Comforl",
           "GM Licatia",
           "GM Marby",
           "GM Prume",
           "GM Rend",
           "GM Snark",
           "GM Sowl",
           "GM Unicta",
           "GM Zoer",
           "GunGix",
           "Hanse",
           "Hugo",
           "Hyacinth",
           "Imirian",
           "Inoia",
           "JB",
           "Jinsol",
           "Kalag",
           "L.Lantz",
           "LadyLu",
           "Leurocian",
           "LongBow",
           "M.Cory",
           "Malachite",
           "Mantisa",
           "Maul",
           "Meatshield",
           "MrsTroublemaker",
           "MrTact",
           "Mung",
           "Neojonez",
           "Niobe",
           "Oaks",
           "Ogel",
           "Orbeus",
           "Platinum",
           "Purple",
           "Rugen",
           "Ryujin",
           "Saralah",
           "SharkBait",
           "Shepherd",
           "Silvani",
           "Skunky",
           "Spada",
           "Sparkle",
           "Speedman",
           "Stormwind",
           "Sunsword",
           "The Intern",
           "Torikichi",
           "TTSO",
           "ValQor",
           "Vex",
           "Wasia",
           "Wildcat",
           "Wilki",
           "Willow",
           "Wraith",
           "Ya-Ssan",
           "Yeti",
           "Zilo"
        };
        private string m_Dipper;
        [Constructable]
        public LightOfTheWinterSolstice()
            : this(HolidayCandleNames[Utility.Random(HolidayCandleNames.Length)])
        {
        }

        [Constructable]
        public LightOfTheWinterSolstice(string dipper)
            : base(0x236E)
        {
            m_Dipper = dipper;

            Weight = 1.0;
            LootType = LootType.Blessed;
            Light = LightType.Circle300;
            Hue = Utility.RandomDyedHue();
        }

        public LightOfTheWinterSolstice(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Dipper
        {
            get
            {
                return this.m_Dipper;
            }
            set
            {
                this.m_Dipper = value;
            }
        }
        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            this.LabelTo(from, 1070881, this.m_Dipper); // Hand Dipped by ~1_name~
            this.LabelTo(from, 1070880); // Winter 2004
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1070881, this.m_Dipper); // Hand Dipped by ~1_name~
            list.Add(1070880); // Winter 2004
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((string)this.m_Dipper);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 1:
                    {
                        this.m_Dipper = reader.ReadString();
                        break;
                    }
                case 0:
                    {
                        this.m_Dipper = HolidayCandleNames[Utility.Random(HolidayCandleNames.Length)];
                        break;
                    }
            }

            if (this.m_Dipper != null)
                this.m_Dipper = String.Intern(this.m_Dipper);
        }
    }
}