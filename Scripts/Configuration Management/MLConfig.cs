using System;
using Server;
using System.Text;
using System.Threading;
using Server.Gumps;
using Server.Network;
using System.IO;
using System.Xml;
using Server.Commands;

namespace Server.MLConfiguration
{
    public static class MLConfig
    {
        private static bool m_PalaceOfParoxysmus;
        private static bool m_TwistedWeald;
        private static bool m_BlightedGrove;
        private static bool m_Bedlam;
        private static bool m_PrismOfLight;
        private static bool m_Citadel;
        private static bool m_PaintedCaves;
        private static bool m_Labyrinth;
        private static bool m_Sanctuary;
        private static bool m_StygianDragonLair;
        private static bool m_MedusasLair;
        private static bool m_Spellweaving;
        private static bool m_PublicDonations;

        public static bool PalaceOfParoxysmus { get { return m_PalaceOfParoxysmus; } set { m_PalaceOfParoxysmus = value; } }
        public static bool TwistedWeald { get { return m_TwistedWeald; } set { m_TwistedWeald = value; } }
        public static bool BlightedGrove { get { return m_BlightedGrove; } set { m_BlightedGrove = value; } }
        public static bool Bedlam { get { return m_Bedlam; } set { m_Bedlam = value; } }
        public static bool PrismOfLight { get { return m_PrismOfLight; } set { m_PrismOfLight = value;  } }
        public static bool Citadel { get { return m_Citadel; } set { m_Citadel = value; } }
        public static bool PaintedCaves { get { return m_PaintedCaves; } set { m_PaintedCaves = value; } }
        public static bool Labyrinth { get { return m_Labyrinth; } set { m_Labyrinth = value; } }
        public static bool Sanctuary { get { return m_Sanctuary; } set { m_Sanctuary = value; } }
        public static bool StygianDragonLair { get { return m_StygianDragonLair; } set { m_StygianDragonLair = value; } }
        public static bool MedusasLair { get { return m_MedusasLair; } set { m_MedusasLair = value; } }
        public static bool Spellweaving { get { return m_Spellweaving; } set {  m_Spellweaving = value; } }
        public static bool PublicDonations { get { return m_PublicDonations; } set { m_PublicDonations = value; } }

        public static void LoadSettings()
        {
            if (!Directory.Exists("Data/Settings"))
                Directory.CreateDirectory("Data/Settings");

            if (!File.Exists("Data/Settings/SettingsML.xml"))
                File.Create("Data/Settings/SettingsML.xml");

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Path.Combine(Core.BaseDirectory, "Data/Settings/SettingsML.xml"));

                XmlElement root = doc["SettingsML"];

                if (root == null)
                    return;

                ReadNode(root, "PalaceOfParoxysmus", ref m_PalaceOfParoxysmus);
                ReadNode(root, "TwistedWeald", ref m_TwistedWeald);
                ReadNode(root, "BlightedGrove", ref m_BlightedGrove);
                ReadNode(root, "Bedlam", ref m_Bedlam);
                ReadNode(root, "PrismOfLight", ref m_PrismOfLight);
                ReadNode(root, "Citadel", ref m_Citadel);
                ReadNode(root, "PaintedCaves", ref m_PaintedCaves);
                ReadNode(root, "Labyrinth", ref m_Labyrinth);
                ReadNode(root, "Sanctuary", ref m_Sanctuary);
                ReadNode(root, "StygianDragonLair", ref m_StygianDragonLair);
                ReadNode(root, "MedusasLair", ref m_MedusasLair);
                ReadNode(root, "Spellweaving", ref m_Spellweaving);
                ReadNode(root, "PublicDonations", ref m_PublicDonations);
            }
            catch
            {
            }
        }

        public static void SaveSettings()
        {
            if (!Directory.Exists("Data/Settings"))
                Directory.CreateDirectory("Data/Settings");

            if (!File.Exists("Data/Settings/SettingsML.xml"))
                File.Create("Data/Settings/SettingsML.xml");

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Path.Combine(Core.BaseDirectory, "Data/Settings/SettingsML.xml"));

                XmlElement root = doc["SettingsML"];

                if (root == null)
                    return;

                UpdateNode(root, "PalaceOfParoxysmus", m_PalaceOfParoxysmus);
                UpdateNode(root, "TwistedWeald", m_TwistedWeald);
                UpdateNode(root, "BlightedGrove", m_BlightedGrove);
                UpdateNode(root, "Bedlam", m_Bedlam);
                UpdateNode(root, "PrismOfLight", m_PrismOfLight);
                UpdateNode(root, "Citadel", m_Citadel);
                UpdateNode(root, "PaintedCaves", m_PaintedCaves);
                UpdateNode(root, "Labyrinth", m_Labyrinth);
                UpdateNode(root, "Sanctuary", m_Sanctuary);
                UpdateNode(root, "StygianDragonLair", m_StygianDragonLair);
                UpdateNode(root, "MedusasLair", m_MedusasLair);
                UpdateNode(root, "Spellweaving", m_Spellweaving);
                UpdateNode(root, "PublicDonations", m_PublicDonations);

                doc.Save("Data/Settings/SettingsML.xml");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while updating 'SettingsML.xml': {0}", e);
            }
        }

        public static void ReadNode(XmlElement root, string dungeon, ref bool val)
        {
            if (root == null)
                return;

            foreach (XmlElement element in root.SelectNodes(dungeon))
            {
                if (element.HasAttribute("active"))
                    val = XmlConvert.ToBoolean(element.GetAttribute("active"));
            }
        }

        public static void UpdateNode(XmlElement root, string dungeon, bool val)
        {
            if (root == null)
                return;

            foreach (XmlElement element in root.SelectNodes(dungeon))
            {
                if (element.HasAttribute("active"))
                    element.SetAttribute("active", XmlConvert.ToString(val));
            }
        }

        public static void Initialize()
        {
            CommandSystem.Register("SettingsML", AccessLevel.Administrator, new CommandEventHandler(SettingsML_OnCommand));
            LoadSettings();
        }

        [Usage("SettingsML")]
        [Description("ML Settings.")]
        private static void SettingsML_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile.HasGump(typeof(MLSettingsGump)))
                e.Mobile.CloseGump(typeof(MLSettingsGump));
            e.Mobile.SendGump(new MLSettingsGump());
        }
    }

    public class MLSettingsGump : Gump
    {
        public MLSettingsGump()
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
            AddLabel(342, 98, 2728, @"ML Settings");

            AddButton(171, 178, MLConfig.PalaceOfParoxysmus ? 0x939 : 0x938, MLConfig.PalaceOfParoxysmus ? 0x939 : 0x938, 1, GumpButtonType.Reply, 0);
            AddButton(171, 202, MLConfig.TwistedWeald ? 0x939 : 0x938, MLConfig.TwistedWeald ? 0x939 : 0x938, 2, GumpButtonType.Reply, 0);
            AddButton(171, 229, MLConfig.BlightedGrove ? 0x939 : 0x938, MLConfig.BlightedGrove ? 0x939 : 0x938, 3, GumpButtonType.Reply, 0);
            AddButton(171, 253, MLConfig.Bedlam ? 0x939 : 0x938, MLConfig.Bedlam ? 0x939 : 0x938, 4, GumpButtonType.Reply, 0);
            AddButton(171, 277, MLConfig.PrismOfLight ? 0x939 : 0x938, MLConfig.PrismOfLight ? 0x939 : 0x938, 5, GumpButtonType.Reply, 0);
            AddButton(171, 301, MLConfig.Citadel ? 0x939 : 0x938, MLConfig.Citadel ? 0x939 : 0x938, 6, GumpButtonType.Reply, 0);
            AddButton(171, 325, MLConfig.PaintedCaves ? 0x939 : 0x938, MLConfig.PaintedCaves ? 0x939 : 0x938, 7, GumpButtonType.Reply, 0);
            AddButton(409, 178, MLConfig.Labyrinth ? 0x939 : 0x938, MLConfig.Labyrinth ? 0x939 : 0x938, 8, GumpButtonType.Reply, 0);
            AddButton(409, 202, MLConfig.Sanctuary ? 0x939 : 0x938, MLConfig.Sanctuary ? 0x939 : 0x938, 9, GumpButtonType.Reply, 0);
            AddButton(409, 229, MLConfig.StygianDragonLair ? 0x939 : 0x938, MLConfig.StygianDragonLair ? 0x939 : 0x938, 10, GumpButtonType.Reply, 0);
            AddButton(409, 253, MLConfig.MedusasLair ? 0x939 : 0x938, MLConfig.MedusasLair ? 0x939 : 0x938, 11, GumpButtonType.Reply, 0);
            AddButton(409, 275, MLConfig.Spellweaving ? 0x939 : 0x938, MLConfig.Spellweaving ? 0x939 : 0x938, 12, GumpButtonType.Reply, 0);
            AddButton(409, 301, MLConfig.PublicDonations ? 0x939 : 0x938, MLConfig.PublicDonations ? 0x939 : 0x938, 13, GumpButtonType.Reply, 0);

            AddLabel(196, 173, 2728, @"Palace of Paroxysmus");
            AddLabel(196, 197, 2728, @"Twisted Weald");
            AddLabel(196, 224, 2728, @"Blighted Grove");
            AddLabel(196, 248, 2728, @"Bedlam");
            AddLabel(196, 271, 2728, @"Prism of Light");
            AddLabel(196, 295, 2728, @"The Citadel");
            AddLabel(196, 318, 2728, @"Painted Caves");
            AddLabel(426, 173, 2728, @"Labyrinth");
            AddLabel(427, 200, 2728, @"Sanctuary");
            AddLabel(428, 224, 2728, @"Stygian Dragon Lair");
            AddLabel(427, 249, 2728, @"Medusas Lair");
            AddLabel(428, 273, 2728, @"Spellweaving");
            AddLabel(429, 298, 2728, @"Public Donations");

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
                    MLConfig.SaveSettings();
                    break;
                case 1:
                    MLConfig.PalaceOfParoxysmus = !MLConfig.PalaceOfParoxysmus;
                    break;
                case 2:
                    MLConfig.TwistedWeald = !MLConfig.TwistedWeald;
                    break;
                case 3:
                    MLConfig.BlightedGrove = !MLConfig.BlightedGrove;
                    break;
                case 4:
                    MLConfig.Bedlam = !MLConfig.Bedlam;
                    break;
                case 5:
                    MLConfig.PrismOfLight = !MLConfig.PrismOfLight;
                    break;
                case 6:
                    MLConfig.Citadel = !MLConfig.Citadel;
                    break;
                case 7:
                    MLConfig.PaintedCaves = !MLConfig.PaintedCaves;
                    break;
                case 8:
                    MLConfig.Labyrinth = !MLConfig.Labyrinth;
                    break;
                case 9:
                    MLConfig.Sanctuary = !MLConfig.Sanctuary;
                    break;
                case 10:
                    MLConfig.StygianDragonLair = !MLConfig.StygianDragonLair;
                    break;
                case 11:
                    MLConfig.MedusasLair = !MLConfig.MedusasLair;
                    break;
                case 12:
                    MLConfig.Spellweaving = !MLConfig.Spellweaving;
                    break;
                case 13:
                    MLConfig.PublicDonations = !MLConfig.PublicDonations;
                    break;
            }
            if (info.ButtonID > 0)
                sender.Mobile.SendGump(new MLSettingsGump());
        }
    }
}
