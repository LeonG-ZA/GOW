using System;
using Server;
using System.Text;
using System.Threading;
using Server.Gumps;
using Server.Network;
using System.IO;
using System.Xml;
using Server.Commands;

namespace Server.T2AConfiguration
{
    public static class T2AConfig
    {
        /// <summary>
        /// Has to be Changed before startup for map register.
        /// </summary>
        public static bool T2AMAPEnabled = false;

        private static bool s_T2AHouseGumpEnabled;
        private static bool s_T2AHouseDoorKeysEnabled;
        private static bool s_T2AItemIDEnabled;
        private static bool s_T2AClassicMoonGenEnabled;
        private static bool s_T2AFeluccaOnlyClassicEnabled;
        private static bool s_T2AClassicHarborMEnabled;
        private static bool s_T2AArchitectEnabled;

        public static bool T2AHouseGumpEnabled { get { return s_T2AHouseGumpEnabled; } set { s_T2AHouseGumpEnabled = value; } }
        public static bool T2AHouseDoorKeysEnabled { get { return s_T2AHouseDoorKeysEnabled; } set { s_T2AHouseDoorKeysEnabled = value; } }
        public static bool T2AItemIDEnabled { get { return s_T2AItemIDEnabled; } set { s_T2AItemIDEnabled = value; } }
        public static bool T2AClassicMoonGenEnabled { get { return s_T2AClassicMoonGenEnabled; } set { s_T2AClassicMoonGenEnabled = value; } }
        public static bool T2AFeluccaOnlyClassicEnabled { get { return s_T2AFeluccaOnlyClassicEnabled; } set { s_T2AFeluccaOnlyClassicEnabled = value; } }
        public static bool T2AClassicHarborMEnabled { get { return s_T2AClassicHarborMEnabled; } set { s_T2AClassicHarborMEnabled = value; } }
        public static bool T2AArchitectEnabled { get { return s_T2AArchitectEnabled; } set { s_T2AArchitectEnabled = value; } }

        public static void LoadSettings()
        {
            if (!Directory.Exists("Data/Settings"))
            {
                Directory.CreateDirectory("Data/Settings");
            }

            if (!File.Exists("Data/Settings/SettingsT2A.xml"))
            {
                File.Create("Data/Settings/SettingsT2A.xml");
            }

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Path.Combine(Core.BaseDirectory, "Data/Settings/SettingsT2A.xml"));

                XmlElement root = doc["SettingsT2A"];

                if (root == null)
                {
                    return;
                }

                ReadNode(root, "T2AHouseGumpEnabled", ref s_T2AHouseGumpEnabled);
                ReadNode(root, "T2AHouseDoorKeysEnabled", ref s_T2AHouseDoorKeysEnabled);
                ReadNode(root, "T2AItemIDEnabled", ref s_T2AItemIDEnabled);
                ReadNode(root, "T2AClassicMoonGenEnabled", ref s_T2AClassicMoonGenEnabled);
                ReadNode(root, "T2AFeluccaOnlyClassicEnabled", ref s_T2AFeluccaOnlyClassicEnabled);
                ReadNode(root, "T2AClassicHarborMEnabled", ref s_T2AClassicHarborMEnabled);
                ReadNode(root, "T2AArchitectEnabled", ref s_T2AArchitectEnabled);
            }
            catch
            {
            }
        }

        public static void SaveSettings()
        {
            if (!Directory.Exists("Data/Settings"))
            {
                Directory.CreateDirectory("Data/Settings");
            }

            if (!File.Exists("Data/Settings/SettingsT2A.xml"))
            {
                File.Create("Data/Settings/SettingsT2A.xml");
            }

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Path.Combine(Core.BaseDirectory, "Data/Settings/SettingsT2A.xml"));

                XmlElement root = doc["SettingsT2A"];

                if (root == null)
                    return;

                UpdateNode(root, "T2AHouseGumpEnabled", s_T2AHouseGumpEnabled);
                UpdateNode(root, "T2AHouseDoorKeysEnabled", s_T2AHouseDoorKeysEnabled);
                UpdateNode(root, "T2AItemIDEnabled", s_T2AItemIDEnabled);
                UpdateNode(root, "T2AClassicMoonGenEnabled", s_T2AClassicMoonGenEnabled);
                UpdateNode(root, "T2AFeluccaOnlyClassicEnabled", s_T2AFeluccaOnlyClassicEnabled);
                UpdateNode(root, "T2AClassicHarborMEnabled", s_T2AClassicHarborMEnabled);
                UpdateNode(root, "T2AArchitectEnabled", s_T2AArchitectEnabled);

                doc.Save("Data/Settings/SettingsT2A.xml");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while updating 'SettingsT2A.xml': {0}", e);
            }
        }

        public static void ReadNode(XmlElement root, string dungeon, ref bool val)
        {
            if (root == null)
            {
                return;
            }

            foreach (XmlElement element in root.SelectNodes(dungeon))
            {
                if (element.HasAttribute("active"))
                {
                    val = XmlConvert.ToBoolean(element.GetAttribute("active"));
                }
            }
        }

        public static void UpdateNode(XmlElement root, string dungeon, bool val)
        {
            if (root == null)
            {
                return;
            }

            foreach (XmlElement element in root.SelectNodes(dungeon))
            {
                if (element.HasAttribute("active"))
                {
                    element.SetAttribute("active", XmlConvert.ToString(val));
                }
            }
        }

        public static void Initialize()
        {
            CommandSystem.Register("SettingsT2A", AccessLevel.Administrator, new CommandEventHandler(SettingsT2A_OnCommand));
            LoadSettings();
        }

        [Usage("SettingsT2A")]
        [Description("T2A Settings.")]
        private static void SettingsT2A_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile.HasGump(typeof(T2ASettingsGump)))
            {
                e.Mobile.CloseGump(typeof(T2ASettingsGump));
            }
            e.Mobile.SendGump(new T2ASettingsGump());
        }
    }

    public class T2ASettingsGump : Gump
    {
        public T2ASettingsGump()
            : base(50, 50)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(110, 38, 600, 509, 9200);
            //AddImageTiled(124, 52, 568, 479, 2624);
            AddAlphaRegion(124, 51, 567, 480);
            AddLabel(347, 66, 2728, @"World Creator");
            AddLabel(555, 509, 2728, @"@GOW, 2015");
            AddImage(297, 58, 52);
            AddImage(126, 52, 5609);
            AddImage(632, 52, 5609);
            AddLabel(342, 98, 2728, @"T2A Settings");

            AddButton(171, 178, T2AConfig.T2AHouseGumpEnabled ? 0x939 : 0x938, T2AConfig.T2AHouseGumpEnabled ? 0x939 : 0x938, 1, GumpButtonType.Reply, 0);
            AddButton(171, 202, T2AConfig.T2AHouseDoorKeysEnabled ? 0x939 : 0x938, T2AConfig.T2AHouseDoorKeysEnabled ? 0x939 : 0x938, 2, GumpButtonType.Reply, 0);
            AddButton(171, 229, T2AConfig.T2AItemIDEnabled ? 0x939 : 0x938, T2AConfig.T2AItemIDEnabled ? 0x939 : 0x938, 3, GumpButtonType.Reply, 0);
            AddButton(171, 253, T2AConfig.T2AClassicMoonGenEnabled ? 0x939 : 0x938, T2AConfig.T2AClassicMoonGenEnabled ? 0x939 : 0x938, 4, GumpButtonType.Reply, 0);
            AddButton(171, 277, T2AConfig.T2AFeluccaOnlyClassicEnabled ? 0x939 : 0x938, T2AConfig.T2AFeluccaOnlyClassicEnabled ? 0x939 : 0x938, 5, GumpButtonType.Reply, 0);
            AddButton(171, 301, T2AConfig.T2AClassicHarborMEnabled ? 0x939 : 0x938, T2AConfig.T2AClassicHarborMEnabled ? 0x939 : 0x938, 6, GumpButtonType.Reply, 0);
            AddButton(171, 325, T2AConfig.T2AArchitectEnabled ? 0x939 : 0x938, T2AConfig.T2AArchitectEnabled ? 0x939 : 0x938, 7, GumpButtonType.Reply, 0);

            AddLabel(196, 173, 2728, @"T2A House Gumps");
            AddLabel(196, 197, 2728, @"T2A House Keys");
            AddLabel(196, 224, 2728, @"T2A ItemID Skill");
            AddLabel(196, 248, 2728, @"T2A Public Moongate Decorate");
            AddLabel(196, 271, 2728, @"T2A Moongate Teleportion");
            AddLabel(196, 295, 2728, @"T2A Harbor Master Docking");
            AddLabel(196, 318, 2728, @"T2A Architect");

            AddLabel(587, 398, 2728, @"Legend:");
            AddLabel(618, 427, 2728, @"Disabled");
            AddLabel(618, 454, 2728, @"Enabled");
            AddButton(593, 431, 2360, 2360, 0, GumpButtonType.Reply, 0);
            AddButton(593, 459, 2361, 2361, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case 0:
                    sender.Mobile.SendGump(new WorldCreator(sender.Mobile));
                    T2AConfig.SaveSettings();
                    break;
                case 1:
                    T2AConfig.T2AHouseGumpEnabled = !T2AConfig.T2AHouseGumpEnabled;
                    break;
                case 2:
                    T2AConfig.T2AHouseDoorKeysEnabled = !T2AConfig.T2AHouseDoorKeysEnabled;
                    break;
                case 3:
                    T2AConfig.T2AItemIDEnabled = !T2AConfig.T2AItemIDEnabled;
                    break;
                case 4:
                    T2AConfig.T2AClassicMoonGenEnabled = !T2AConfig.T2AClassicMoonGenEnabled;
                    break;
                case 5:
                    T2AConfig.T2AFeluccaOnlyClassicEnabled = !T2AConfig.T2AFeluccaOnlyClassicEnabled;
                    break;
                case 6:
                    T2AConfig.T2AClassicHarborMEnabled = !T2AConfig.T2AClassicHarborMEnabled;
                    break;
                case 7:
                    T2AConfig.T2AArchitectEnabled = !T2AConfig.T2AArchitectEnabled;
                    break;
            }
            if (info.ButtonID > 0)
            {
                sender.Mobile.SendGump(new T2ASettingsGump());
            }
        }
    }
}
