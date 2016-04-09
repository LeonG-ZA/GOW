using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Server.Engines.Quests.Haven;
using Server.Engines.Quests.Necro;
using Server.Items;

namespace Server.Gumps
{
    public class WorldCreatorTools : Gump
    {
        public WorldCreatorTools(Mobile from)
            : base(0, 0)
        {
            NetState ns = from.NetState;
            if (ns == null)
            {
                return;
            }
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(1);
            AddBackground(110, 38, 600, 509, 9200);
            if (!ns.IsKRClient)
            {
                AddImageTiled(124, 52, 568, 479, 2624);
            }
            AddAlphaRegion(125, 52, 567, 480);
            AddLabel(347, 66, 2728, @"World Creator");
            AddLabel(555, 509, 2728, @"@G.O.W, 2016");
            AddLabel(350, 92, 2728, @"Tools");
            AddImage(297, 58, 52);
            AddImage(125, 51, 5609);
            AddImage(632, 51, 5609);
            AddButton(141, 177, 4005, 4007, 0, GumpButtonType.Page, 2);
            AddButton(141, 200, 4005, 4007, 2, GumpButtonType.Reply, 0);
            AddButton(141, 224, 4005, 4007, 3, GumpButtonType.Reply, 0);
            AddButton(141, 248, 4005, 4007, 4, GumpButtonType.Reply, 0);
            AddButton(141, 272, 4005, 4007, 5, GumpButtonType.Reply, 0);
            AddLabel(174, 179, 2728, @"Decorate Delete");
            AddLabel(175, 201, 2728, @"Door Generator Delete");
            AddLabel(175, 226, 2728, @"Public Moongate Delete");
            AddLabel(175, 250, 2728, @"Clear All");
            AddLabel(175, 274, 2728, @"Region Controller");
            AddHtml(140, 296, 200, 100, @"Add a Region Controller
            double-click the Region
            Controller to configure it's region.
            Every Controller, control's one
            region. Don't forget to do
            Command [Props.", (bool)true, (bool)true);
            AddButton(144, 406, 4005, 4007, 6, GumpButtonType.Reply, 0);
            AddButton(144, 432, 4005, 4007, 7, GumpButtonType.Reply, 0);
            AddButton(144, 457, 4005, 4007, 16, GumpButtonType.Reply, 0);
            AddLabel(178, 407, 2728, @"Spawners to Prime Spawner");
            AddLabel(178, 432, 2728, @"Spawn Editor");
            AddLabel(177, 457, 2728, @"Prime Spawner Counter");
            AddLabel(423, 152, 2728, @"Save Spawners");
            AddButton(421, 177, 4005, 4007, 8, GumpButtonType.Reply, 0);
            AddButton(421, 200, 4005, 4007, 9, GumpButtonType.Reply, 0);
            AddButton(421, 224, 4005, 4007, 10, GumpButtonType.Reply, 0);
            AddButton(421, 248, 4005, 4007, 11, GumpButtonType.Reply, 0);
            AddLabel(455, 178, 2728, @"All spawns (spawns.map)");
            AddLabel(455, 200, 2728, @"Spawns By Hand (byhand.map)");
            AddLabel(454, 224, 2728, @"Spawns inside region (region.map)");
            AddLabel(453, 248, 2728, @"Spawns inside coordinates");
            AddLabel(421, 276, 2728, @"Remove Spawners");
            AddButton(423, 298, 4005, 4007, 12, GumpButtonType.Reply, 0);
            AddButton(423, 321, 4005, 4007, 13, GumpButtonType.Reply, 0);
            AddButton(423, 344, 4005, 4007, 14, GumpButtonType.Reply, 0);
            AddButton(423, 367, 4005, 4007, 15, GumpButtonType.Reply, 0);
            AddButton(423, 390, 4005, 4007, 16, GumpButtonType.Reply, 0);
            AddLabel(456, 299, 2728, @"All Spawners in ALL facets");
            AddLabel(457, 322, 2728, @"All Spawners in THIS facet");
            AddLabel(457, 345, 2728, @"Remove Spawners by SpawnID");
            AddLabel(457, 367, 2728, @"Remove Spawners inside coordinates");
            AddLabel(456, 391, 2728, @"Remove Spawners inside region");

            AddPage(2);
            AddBackground(110, 38, 600, 509, 9200);
            //AddImageTiled(124, 53, 568, 479, 2624);
            AddAlphaRegion(126, 51, 567, 480);
            AddLabel(347, 67, 2728, @"World Creator");
            AddLabel(555, 509, 2728, @"@G.O.W, 2015");
            AddLabel(350, 92, 2728, @"Decorate Delete");
            AddImage(297, 59, 52);
            AddImage(125, 52, 5609);
            AddImage(632, 51, 5609);
            AddButton(141, 177, 4005, 4007, 17, GumpButtonType.Reply, 0);
            AddButton(141, 200, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddButton(141, 224, 4005, 4007, 18, GumpButtonType.Reply, 0);
            AddButton(141, 248, 4005, 4007, 19, GumpButtonType.Reply, 0);
            AddButton(141, 272, 4005, 4007, 20, GumpButtonType.Reply, 0);
            AddButton(141, 297, 4005, 4007, 21, GumpButtonType.Reply, 0);
            AddButton(141, 321, 4005, 4007, 22, GumpButtonType.Reply, 0);
            AddButton(141, 345, 4005, 4007, 23, GumpButtonType.Reply, 0);
            AddLabel(174, 179, 2728, @"Delete ALL");
            AddLabel(175, 201, 2728, @"Delete T2A");
            AddLabel(175, 226, 2728, @"Delete Felucca");
            AddLabel(175, 250, 2728, @"Delete Trammel");
            AddLabel(175, 274, 2728, @"Delete Ilshenar");
            AddLabel(173, 298, 2728, @"Delete Malas");
            AddLabel(174, 322, 2728, @"Delete Tokuno");
            AddLabel(175, 347, 2728, @"Delete TerMur");
        }

        public static void DoThis(Mobile from, string command)
        {
            string prefix = Server.Commands.CommandSystem.Prefix;
            CommandSystem.Handle(from, String.Format("{0}{1}", prefix, command));
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0:
                    {
                        from.CloseGump(typeof(WorldCreator));
                        break;
                    }
                case 1:
                    {
                        DoThis(from, "DecorateDeleteT2A");
                        break;
                    }
                case 2:
                    {
                        DoThis(from, "DoorGenDelete");
                        break;
                    }
                case 3:
                    {
                        DoThis(from, "MoonGenDelete");
                        break;
                    }
                case 4:
                    {
                        DoThis(from, "clearall");
                        break;
                    }
                case 5:
                    {
                        DoThis(from, "Add RegionControl");
                        break;
                    }
                case 6:
                    {
                        DoThis(from, "GOWSpawnerExporter");
                        break;
                    }
                case 7:
                    {
                        DoThis(from, "SpawnEditor");
                        break;
                    }
                case 8:
                    {
                        DoThis(from, "spawngen save");
                        break;
                    }
                case 9:
                    {
                        DoThis(from, "spawngen savebyhand");
                        break;
                    }
                case 10:
                    {
                        DoThis(from, "GumpSaveRegion");
                        break;
                    }
                case 11:
                    {
                        DoThis(from, "GumpSaveCoordinate");
                        break;
                    }
                case 12:
                    {
                        DoThis(from, "spawngen remove");
                        break;
                    }
                case 13:
                    {
                        DoThis(from, "spawngen cleanfacet");
                        break;
                    }
                case 14:
                    {
                        DoThis(from, "GumpRemoveID");
                        break;
                    }
                case 15:
                    {
                        DoThis(from, "GumpRemoveCoordinate");
                        break;
                    }
                case 16:
                    {
                        DoThis(from, "pscount");
                        break;
                    }
                case 17:
                    {
                        DoThis(from, "DecorateDeleteALL");
                        break;
                    }
                case 18:
                    {
                        DoThis(from, "DecorateDeleteFelucca");
                        break;
                    }
                case 19:
                    {
                        DoThis(from, "DecorateDeleteTrammel");
                        break;
                    }
                case 20:
                    {
                        DoThis(from, "DecorateDeleteIlshenar");
                        break;
                    }
                case 21:
                    {
                        DoThis(from, "DecorateDeleteMalas");
                        break;
                    }
                case 22:
                    {
                        DoThis(from, "DecorateDeleteTokuno");
                        break;
                    }
                case 23:
                    {
                        DoThis(from, "DecorateDeleteTerMur");
                        break;
                    }

            }
        }

        public class DecorateDelete
        {
            public static void Initialize()
            {
                CommandSystem.Register("DecorateDeleteALL", AccessLevel.Administrator, new CommandEventHandler(DecorateDeleteALL_OnCommand));
                CommandSystem.Register("DecoDelALL", AccessLevel.Administrator, new CommandEventHandler(DecorateDeleteALL_OnCommand));
                CommandSystem.Register("DecorateDeleteT2A", AccessLevel.Administrator, new CommandEventHandler(DecorateDeleteT2A_OnCommand));
                CommandSystem.Register("DecoDelT2A", AccessLevel.Administrator, new CommandEventHandler(DecorateDeleteT2A_OnCommand));
                CommandSystem.Register("DecorateDeleteFelucca", AccessLevel.Administrator, new CommandEventHandler(DecorateDeleteFelucca_OnCommand));
                CommandSystem.Register("DecoDelFel", AccessLevel.Administrator, new CommandEventHandler(DecorateDeleteFelucca_OnCommand));
                CommandSystem.Register("DecorateDeleteTrammel", AccessLevel.Administrator, new CommandEventHandler(DecorateDeleteTrammel_OnCommand));
                CommandSystem.Register("DecoDelTram", AccessLevel.Administrator, new CommandEventHandler(DecorateDeleteTrammel_OnCommand));
                CommandSystem.Register("DecorateDeleteIlshenar", AccessLevel.Administrator, new CommandEventHandler(DecorateDeleteIlshenar_OnCommand));
                CommandSystem.Register("DecoDelIlsh", AccessLevel.Administrator, new CommandEventHandler(DecorateDeleteIlshenar_OnCommand));
                CommandSystem.Register("DecorateDeleteMalas", AccessLevel.Administrator, new CommandEventHandler(DecorateDeleteMalas_OnCommand));
                CommandSystem.Register("DecoDelMaL", AccessLevel.Administrator, new CommandEventHandler(DecorateDeleteMalas_OnCommand));
                CommandSystem.Register("DecorateDeleteTokuno", AccessLevel.Administrator, new CommandEventHandler(DecorateDeleteTokuno_OnCommand));
                CommandSystem.Register("DecoDelTok", AccessLevel.Administrator, new CommandEventHandler(DecorateDeleteTokuno_OnCommand));
                CommandSystem.Register("DecorateDeleteTerMur", AccessLevel.Administrator, new CommandEventHandler(DecorateDeleteTerMur_OnCommand));
                CommandSystem.Register("DecoDelTer", AccessLevel.Administrator, new CommandEventHandler(DecorateDeleteTerMur_OnCommand));
            }

            [Usage("DecorateDeleteT2A")]
            [Aliases("DecoDelT2A")]
            [Description("Removes world decorations of T2A.")]
            private static void DecorateDeleteT2A_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;
                m_Count = 0;
                DateTime aTime = DateTime.UtcNow;

                m_Mobile.SendMessage("Deleting T2A world decoration, please wait.");

                Remove("Data/World/Decoration/UOT2A/Britannia", Map.Felucca);

                DateTime bTime = DateTime.UtcNow;

                m_Mobile.SendMessage("Deleting T2A complete. {0} items were deleted in {1} seconds.", m_Count, (bTime - aTime).TotalSeconds);
            }

            [Usage("DecorateDeleteFelucca")]
            [Aliases("DecoDelFel")]
            [Description("Deletes all Felucca world decoration.")]
            private static void DecorateDeleteFelucca_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;
                m_Count = 0;

                DateTime aTime = DateTime.UtcNow;

                m_Mobile.SendMessage("Deleting all world decoration, please wait.");

                Remove("Data/World/Decoration/All/Britannia", Map.Felucca);
                Remove("Data/World/Decoration/All/Felucca", Map.Felucca);

                DateTime bTime = DateTime.UtcNow;

                m_Mobile.SendMessage("Deleting all Felucca World complete. {0} items were deleted in {1} seconds.", m_Count, (bTime - aTime).TotalSeconds);
            }

            [Usage("DecorateDeleteTrammel")]
            [Aliases("DecoDelTram")]
            [Description("Deletes all Trammel world decoration.")]
            private static void DecorateDeleteTrammel_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;
                m_Count = 0;

                DateTime aTime = DateTime.UtcNow;

                m_Mobile.SendMessage("Deleting all Trammel world decoration, please wait.");

                Remove("Data/World/Decoration/All/Britannia", Map.Trammel);
                Remove("Data/World/Decoration/All/Trammel", Map.Trammel);

                DateTime bTime = DateTime.UtcNow;

                m_Mobile.SendMessage("Deleting all Trammel World complete. {0} items were deleted in {1} seconds.", m_Count, (bTime - aTime).TotalSeconds);
            }

            [Usage("DecorateDeleteIlshenar")]
            [Aliases("DecoDelIlsh")]
            [Description("Deletes all Ilshenar world decoration.")]
            private static void DecorateDeleteIlshenar_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;
                m_Count = 0;

                DateTime aTime = DateTime.UtcNow;

                m_Mobile.SendMessage("Deleting all Ilshenar world decoration, please wait.");

                Remove("Data/World/Decoration/All/Ilshenar", Map.Ilshenar);

                DateTime bTime = DateTime.UtcNow;

                m_Mobile.SendMessage("Deleting all Ilshenar World complete. {0} items were deleted in {1} seconds.", m_Count, (bTime - aTime).TotalSeconds);
            }

            [Usage("DecorateDeleteMalas")]
            [Aliases("DecoDelMaL")]
            [Description("Deletes all Malas world decoration.")]
            private static void DecorateDeleteMalas_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;
                m_Count = 0;

                DateTime aTime = DateTime.UtcNow;

                m_Mobile.SendMessage("Deleting all Malas world decoration, please wait.");

                Remove("Data/World/Decoration/All/Malas", Map.Malas);

                DateTime bTime = DateTime.UtcNow;

                m_Mobile.SendMessage("Deleting all Malas World complete. {0} items were deleted in {1} seconds.", m_Count, (bTime - aTime).TotalSeconds);
            }

            [Usage("DecorateDeleteTokuno")]
            [Aliases("DecoDelTok")]
            [Description("Deletes all Tokuno world decoration.")]
            private static void DecorateDeleteTokuno_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;
                m_Count = 0;

                DateTime aTime = DateTime.UtcNow;

                m_Mobile.SendMessage("Deleting all Tokuno world decoration, please wait.");

                Remove("Data/World/Decoration/All/Tokuno", Map.Tokuno);

                DateTime bTime = DateTime.UtcNow;

                m_Mobile.SendMessage("Deleting all Tokuno World complete. {0} items were deleted in {1} seconds.", m_Count, (bTime - aTime).TotalSeconds);
            }

            [Usage("DecorateDeleteTerMur")]
            [Aliases("DecoDelTer")]
            [Description("Deletes all Ter Mur world decoration.")]
            private static void DecorateDeleteTerMur_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;
                m_Count = 0;

                DateTime aTime = DateTime.UtcNow;

                m_Mobile.SendMessage("Deleting all Ter Mur world decoration, please wait.");

                Remove("Data/World/Decoration/All/Ter Mur", Map.TerMur);

                DateTime bTime = DateTime.UtcNow;

                m_Mobile.SendMessage("Deleting all Ter Mur World complete. {0} items were deleted in {1} seconds.", m_Count, (bTime - aTime).TotalSeconds);
            }

            [Usage("DecorateDeleteALL")]
            [Aliases("DecoDelALL")]
            [Description("Deletes all world decoration.")]
            private static void DecorateDeleteALL_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;
                m_Count = 0;

                DateTime aTime = DateTime.UtcNow;

                m_Mobile.SendMessage("Deleting all world decoration, please wait.");

                Remove("Data/World/Decoration/All/Britannia", Map.Trammel, Map.Felucca);
                Remove("Data/World/Decoration/All/Trammel", Map.Trammel);
                Remove("Data/World/Decoration/All/Felucca", Map.Felucca);
                Remove("Data/World/Decoration/All/Ilshenar", Map.Ilshenar);
                Remove("Data/World/Decoration/All/Malas", Map.Malas);
                Remove("Data/World/Decoration/All/Tokuno", Map.Tokuno);
                Remove("Data/World/Decoration/All/Ter Mur", Map.TerMur);

                DateTime bTime = DateTime.UtcNow;

                m_Mobile.SendMessage("Deleting all World complete. {0} items were deleted in {1} seconds.", m_Count, (bTime - aTime).TotalSeconds);
            }

            public static void Remove(string folder, params Map[] maps)
            {
                if (!Directory.Exists(folder))
                    return;

                string[] files = Directory.GetFiles(folder, "*.cfg");

                RemoveGeneric(files, maps);
            }

            public static void RemoveOne(string folder, string filename, params Map[] maps)
            {
                if (!Directory.Exists(folder))
                    return;

                string[] files = Directory.GetFiles(folder, filename);

                RemoveGeneric(files, maps);
            }

            private static void RemoveGeneric(string[] files, params Map[] maps)
            {
                for (int i = 0; i < files.Length; ++i)
                {
                    ArrayList list = DecorationListDelete.ReadAll(files[i]);

                    m_List = list;

                    for (int j = 0; j < list.Count; ++j)
                        m_Count += ((DecorationListDelete)list[j]).Remove(maps);
                }
            }

            public static Item FindByID(int id)
            {
                if (m_List == null)
                    return null;

                for (int j = 0; j < m_List.Count; ++j)
                {
                    DecorationListDelete list = (DecorationListDelete)m_List[j];

                    if (list.ID == id)
                        return list.Constructed;
                }

                return null;
            }

            private static ArrayList m_List;
            private static Mobile m_Mobile;
            private static int m_Count;
        }

        public class DecorationListDelete
        {
            private Type m_Type;
            private int m_ItemID;
            private string[] m_Params;
            private ArrayList m_Entries;
            private Item m_Constructed;

            public Item Constructed { get { return m_Constructed; } }

            public int ID
            {
                get
                {
                    for (int i = 0; i < m_Params.Length; ++i)
                    {
                        if (m_Params[i].StartsWith("ID"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                return Utility.ToInt32(m_Params[i].Substring(++indexOf));
                        }
                    }

                    return 0;
                }
            }

            public DecorationListDelete()
            {
            }

            private static Type typeofStatic = typeof(Static);
            private static Type typeofLocalizedStatic = typeof(LocalizedStatic);
            private static Type typeofBaseDoor = typeof(BaseDoor);
            private static Type typeofAnkhWest = typeof(AnkhWest);
            private static Type typeofAnkhNorth = typeof(AnkhNorth);
            private static Type typeofBeverage = typeof(BaseBeverage);
            private static Type typeofLocalizedSign = typeof(LocalizedSign);
            private static Type typeofMarkContainer = typeof(MarkContainer);
            private static Type typeofWarningItem = typeof(WarningItem);
            private static Type typeofHintItem = typeof(HintItem);
            private static Type typeofCannon = typeof(Cannon);
            private static Type typeofSerpentPillar = typeof(SerpentPillar);

            public Item Construct()
            {
                Item item;

                try
                {
                    if (m_Type == typeofStatic)
                    {
                        item = new Static(m_ItemID);
                    }
                    else if (m_Type == typeofLocalizedStatic)
                    {
                        int labelNumber = 0;

                        for (int i = 0; i < m_Params.Length; ++i)
                        {
                            if (m_Params[i].StartsWith("LabelNumber"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                {
                                    labelNumber = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                                    break;
                                }
                            }
                        }

                        item = new LocalizedStatic(m_ItemID, labelNumber);
                    }
                    else if (m_Type == typeofLocalizedSign)
                    {
                        int labelNumber = 0;

                        for (int i = 0; i < m_Params.Length; ++i)
                        {
                            if (m_Params[i].StartsWith("LabelNumber"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                {
                                    labelNumber = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                                    break;
                                }
                            }
                        }

                        item = new LocalizedSign(m_ItemID, labelNumber);
                    }
                    else if (m_Type == typeofAnkhWest || m_Type == typeofAnkhNorth)
                    {
                        bool bloodied = false;

                        for (int i = 0; !bloodied && i < m_Params.Length; ++i)
                            bloodied = (m_Params[i] == "Bloodied");

                        if (m_Type == typeofAnkhWest)
                            item = new AnkhWest(bloodied);
                        else
                            item = new AnkhNorth(bloodied);
                    }
                    else if (m_Type == typeofMarkContainer)
                    {
                        bool bone = false;
                        bool locked = false;
                        Map map = Map.Malas;

                        for (int i = 0; i < m_Params.Length; ++i)
                        {
                            if (m_Params[i] == "Bone")
                            {
                                bone = true;
                            }
                            else if (m_Params[i] == "Locked")
                            {
                                locked = true;
                            }
                            else if (m_Params[i].StartsWith("TargetMap"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    map = Map.Parse(m_Params[i].Substring(++indexOf));
                            }
                        }

                        MarkContainer mc = new MarkContainer(bone, locked);

                        mc.TargetMap = map;
                        mc.Description = "strange location";

                        item = mc;
                    }
                    else if (m_Type == typeofHintItem)
                    {
                        int range = 0;
                        int messageNumber = 0;
                        string messageString = null;
                        int hintNumber = 0;
                        string hintString = null;
                        TimeSpan resetDelay = TimeSpan.Zero;

                        for (int i = 0; i < m_Params.Length; ++i)
                        {
                            if (m_Params[i].StartsWith("Range"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    range = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                            }
                            else if (m_Params[i].StartsWith("WarningString"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    messageString = m_Params[i].Substring(++indexOf);
                            }
                            else if (m_Params[i].StartsWith("WarningNumber"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    messageNumber = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                            }
                            else if (m_Params[i].StartsWith("HintString"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    hintString = m_Params[i].Substring(++indexOf);
                            }
                            else if (m_Params[i].StartsWith("HintNumber"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    hintNumber = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                            }
                            else if (m_Params[i].StartsWith("ResetDelay"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    resetDelay = TimeSpan.Parse(m_Params[i].Substring(++indexOf));
                            }
                        }

                        HintItem hi = new HintItem(m_ItemID, range, messageNumber, hintNumber);

                        hi.WarningString = messageString;
                        hi.HintString = hintString;
                        hi.ResetDelay = resetDelay;

                        item = hi;
                    }
                    else if (m_Type == typeofWarningItem)
                    {
                        int range = 0;
                        int messageNumber = 0;
                        string messageString = null;
                        TimeSpan resetDelay = TimeSpan.Zero;

                        for (int i = 0; i < m_Params.Length; ++i)
                        {
                            if (m_Params[i].StartsWith("Range"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    range = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                            }
                            else if (m_Params[i].StartsWith("WarningString"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    messageString = m_Params[i].Substring(++indexOf);
                            }
                            else if (m_Params[i].StartsWith("WarningNumber"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    messageNumber = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                            }
                            else if (m_Params[i].StartsWith("ResetDelay"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    resetDelay = TimeSpan.Parse(m_Params[i].Substring(++indexOf));
                            }
                        }

                        WarningItem wi = new WarningItem(m_ItemID, range, messageNumber);

                        wi.WarningString = messageString;
                        wi.ResetDelay = resetDelay;

                        item = wi;
                    }
                    else if (m_Type == typeofCannon)
                    {
                        CannonDirection direction = CannonDirection.North;

                        for (int i = 0; i < m_Params.Length; ++i)
                        {
                            if (m_Params[i].StartsWith("CannonDirection"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    direction = (CannonDirection)Enum.Parse(typeof(CannonDirection), m_Params[i].Substring(++indexOf), true);
                            }
                        }

                        item = new Cannon(direction);
                    }
                    else if (m_Type == typeofSerpentPillar)
                    {
                        string word = null;
                        Rectangle2D destination = new Rectangle2D();

                        for (int i = 0; i < m_Params.Length; ++i)
                        {
                            if (m_Params[i].StartsWith("Word"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    word = m_Params[i].Substring(++indexOf);
                            }
                            else if (m_Params[i].StartsWith("DestStart"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    destination.Start = Point2D.Parse(m_Params[i].Substring(++indexOf));
                            }
                            else if (m_Params[i].StartsWith("DestEnd"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    destination.End = Point2D.Parse(m_Params[i].Substring(++indexOf));
                            }
                        }

                        item = new SerpentPillar(word, destination);
                    }
                    else if (m_Type.IsSubclassOf(typeofBeverage))
                    {
                        BeverageType content = BeverageType.Liquor;
                        bool fill = false;

                        for (int i = 0; !fill && i < m_Params.Length; ++i)
                        {
                            if (m_Params[i].StartsWith("Content"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                {
                                    content = (BeverageType)Enum.Parse(typeof(BeverageType), m_Params[i].Substring(++indexOf), true);
                                    fill = true;
                                }
                            }
                        }

                        if (fill)
                            item = (Item)Activator.CreateInstance(m_Type, new object[] { content });
                        else
                            item = (Item)Activator.CreateInstance(m_Type);
                    }
                    else if (m_Type.IsSubclassOf(typeofBaseDoor))
                    {
                        DoorFacing facing = DoorFacing.WestCW;

                        for (int i = 0; i < m_Params.Length; ++i)
                        {
                            if (m_Params[i].StartsWith("Facing"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                {
                                    facing = (DoorFacing)Enum.Parse(typeof(DoorFacing), m_Params[i].Substring(++indexOf), true);
                                    break;
                                }
                            }
                        }

                        item = (Item)Activator.CreateInstance(m_Type, new object[] { facing });
                    }
                    else
                    {
                        item = (Item)Activator.CreateInstance(m_Type);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(String.Format("Bad type: {0}", m_Type), e);
                }

                if (item is BaseAddon)
                {
                    if (item is MaabusCoffin)
                    {
                        MaabusCoffin coffin = (MaabusCoffin)item;

                        for (int i = 0; i < m_Params.Length; ++i)
                        {
                            if (m_Params[i].StartsWith("SpawnLocation"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    coffin.SpawnLocation = Point3D.Parse(m_Params[i].Substring(++indexOf));
                            }
                        }
                    }
                    else if (m_ItemID > 0)
                    {
                        List<AddonComponent> comps = ((BaseAddon)item).Components;

                        for (int i = 0; i < comps.Count; ++i)
                        {
                            AddonComponent comp = (AddonComponent)comps[i];

                            if (comp.Offset == Point3D.Zero)
                                comp.ItemID = m_ItemID;
                        }
                    }
                }
                else if (item is BaseLight)
                {
                    bool unlit = false, unprotected = false;

                    for (int i = 0; i < m_Params.Length; ++i)
                    {
                        if (!unlit && m_Params[i] == "Unlit")
                            unlit = true;
                        else if (!unprotected && m_Params[i] == "Unprotected")
                            unprotected = true;

                        if (unlit && unprotected)
                            break;
                    }

                    if (!unlit)
                        ((BaseLight)item).Ignite();
                    if (!unprotected)
                        ((BaseLight)item).Protected = true;

                    if (m_ItemID > 0)
                        item.ItemID = m_ItemID;
                }
                else if (item is Server.Mobiles.Spawner)
                {
                    Server.Mobiles.Spawner sp = (Server.Mobiles.Spawner)item;

                    sp.NextSpawn = TimeSpan.Zero;

                    for (int i = 0; i < m_Params.Length; ++i)
                    {
                        if (m_Params[i].StartsWith("Spawn"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.SpawnNames.Add(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("MinDelay"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.MinDelay = TimeSpan.Parse(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("MaxDelay"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.MaxDelay = TimeSpan.Parse(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("NextSpawn"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.NextSpawn = TimeSpan.Parse(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("Count"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.SpawnMax = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("Team"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.Team = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("HomeRange"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.HomeRange = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("Running"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.Running = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("Group"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.Group = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
                        }
                    }
                }
                else if (item is RecallRune)
                {
                    RecallRune rune = (RecallRune)item;

                    for (int i = 0; i < m_Params.Length; ++i)
                    {
                        if (m_Params[i].StartsWith("Description"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                rune.Description = m_Params[i].Substring(++indexOf);
                        }
                        else if (m_Params[i].StartsWith("Marked"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                rune.Marked = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("TargetMap"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                rune.TargetMap = Map.Parse(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("Target"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                rune.Target = Point3D.Parse(m_Params[i].Substring(++indexOf));
                        }
                    }
                }
                else if (item is SkillTeleporter)
                {
                    SkillTeleporter tp = (SkillTeleporter)item;

                    for (int i = 0; i < m_Params.Length; ++i)
                    {
                        if (m_Params[i].StartsWith("Skill"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Skill = (SkillName)Enum.Parse(typeof(SkillName), m_Params[i].Substring(++indexOf), true);
                        }
                        else if (m_Params[i].StartsWith("RequiredFixedPoint"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Required = Utility.ToInt32(m_Params[i].Substring(++indexOf)) * 0.01;
                        }
                        else if (m_Params[i].StartsWith("Required"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Required = Utility.ToDouble(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("MessageString"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.MessageString = m_Params[i].Substring(++indexOf);
                        }
                        else if (m_Params[i].StartsWith("MessageNumber"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.MessageNumber = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("PointDest"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.PointDest = Point3D.Parse(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("MapDest"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.MapDest = Map.Parse(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("Creatures"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Creatures = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("SourceEffect"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.SourceEffect = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("DestEffect"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.DestEffect = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("SoundID"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.SoundID = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("Delay"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Delay = TimeSpan.Parse(m_Params[i].Substring(++indexOf));
                        }
                    }

                    if (m_ItemID > 0)
                        item.ItemID = m_ItemID;
                }
                else if (item is KeywordTeleporter)
                {
                    KeywordTeleporter tp = (KeywordTeleporter)item;

                    for (int i = 0; i < m_Params.Length; ++i)
                    {
                        if (m_Params[i].StartsWith("Substring"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Substring = m_Params[i].Substring(++indexOf);
                        }
                        else if (m_Params[i].StartsWith("Keyword"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Keyword = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("Range"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Range = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("PointDest"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.PointDest = Point3D.Parse(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("MapDest"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.MapDest = Map.Parse(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("Creatures"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Creatures = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("SourceEffect"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.SourceEffect = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("DestEffect"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.DestEffect = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("SoundID"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.SoundID = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("Delay"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Delay = TimeSpan.Parse(m_Params[i].Substring(++indexOf));
                        }
                    }

                    if (m_ItemID > 0)
                        item.ItemID = m_ItemID;
                }
                else if (item is Teleporter)
                {
                    Teleporter tp = (Teleporter)item;

                    for (int i = 0; i < m_Params.Length; ++i)
                    {
                        if (m_Params[i].StartsWith("PointDest"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.PointDest = Point3D.Parse(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("MapDest"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.MapDest = Map.Parse(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("Creatures"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Creatures = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("SourceEffect"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.SourceEffect = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("DestEffect"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.DestEffect = Utility.ToBoolean(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("SoundID"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.SoundID = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                        }
                        else if (m_Params[i].StartsWith("Delay"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Delay = TimeSpan.Parse(m_Params[i].Substring(++indexOf));
                        }
                    }

                    if (m_ItemID > 0)
                        item.ItemID = m_ItemID;
                }
                else if (item is FillableContainer)
                {
                    FillableContainer cont = (FillableContainer)item;

                    for (int i = 0; i < m_Params.Length; ++i)
                    {
                        if (m_Params[i].StartsWith("ContentType"))
                        {
                            int indexOf = m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                cont.ContentType = (FillableContentType)Enum.Parse(typeof(FillableContentType), m_Params[i].Substring(++indexOf), true);
                        }
                    }

                    if (m_ItemID > 0)
                        item.ItemID = m_ItemID;
                }
                else if (m_ItemID > 0)
                {
                    item.ItemID = m_ItemID;
                }

                item.Movable = false;

                for (int i = 0; i < m_Params.Length; ++i)
                {
                    if (m_Params[i].StartsWith("Light"))
                    {
                        int indexOf = m_Params[i].IndexOf('=');

                        if (indexOf >= 0)
                            item.Light = (LightType)Enum.Parse(typeof(LightType), m_Params[i].Substring(++indexOf), true);
                    }
                    else if (m_Params[i].StartsWith("Hue"))
                    {
                        int indexOf = m_Params[i].IndexOf('=');

                        if (indexOf >= 0)
                        {
                            int hue = Utility.ToInt32(m_Params[i].Substring(++indexOf));

                            if (item is DyeTub)
                                ((DyeTub)item).DyedHue = hue;
                            else
                                item.Hue = hue;
                        }
                    }
                    else if (m_Params[i].StartsWith("Name"))
                    {
                        int indexOf = m_Params[i].IndexOf('=');

                        if (indexOf >= 0)
                            item.Name = m_Params[i].Substring(++indexOf);
                    }
                    else if (m_Params[i].StartsWith("Amount"))
                    {
                        int indexOf = m_Params[i].IndexOf('=');

                        if (indexOf >= 0)
                        {
                            // Must supress stackable warnings

                            bool wasStackable = item.Stackable;

                            item.Stackable = true;
                            item.Amount = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                            item.Stackable = wasStackable;
                        }
                    }
                }

                return item;
            }

            private static Queue<Item> m_DeleteQueue = new Queue<Item>();

            private static bool FindItemDelete(int x, int y, int z, Map map, Item srcItem)
            {
                int itemID = srcItem.ItemID;

                bool res = false;

                IPooledEnumerable eable;

                if (srcItem is BaseDoor)
                {
                    eable = map.GetItemsInRange(new Point3D(x, y, z), 1);

                    foreach (Item item in eable)
                    {
                        if (!(item is BaseDoor))
                            continue;

                        BaseDoor bd = (BaseDoor)item;
                        Point3D p;
                        int bdItemID;

                        if (bd.Open)
                        {
                            p = new Point3D(bd.X - bd.Offset.X, bd.Y - bd.Offset.Y, bd.Z - bd.Offset.Z);
                            bdItemID = bd.ClosedID;
                        }
                        else
                        {
                            p = bd.Location;
                            bdItemID = bd.ItemID;
                        }

                        if (p.X != x || p.Y != y)
                            continue;

                        if (item.Z == z && bdItemID == itemID)
                        {
                            m_DeleteQueue.Enqueue(item);
                            res = true;
                        }
                        /*else if (Math.Abs(item.Z - z) < 8)
                            m_DeleteQueue.Enqueue(item);*/
                    }
                }
                else if ((TileData.ItemTable[itemID & 0x7FFF].Flags & TileFlag.LightSource) != 0)
                {
                    eable = map.GetItemsInRange(new Point3D(x, y, z), 0);

                    LightType lt = srcItem.Light;
                    string srcName = srcItem.ItemData.Name;

                    foreach (Item item in eable)
                    {
                        if (item.Z == z)
                        {
                            if (item.ItemID == itemID)
                            {
                                /*if ( item.Light != lt )
                                    m_DeleteQueue.Enqueue( item );
                                else*/
                                res = true;
                                m_DeleteQueue.Enqueue(item);
                            }
                            else if ((item.ItemData.Flags & TileFlag.LightSource) != 0 && item.ItemData.Name == srcName)
                            {
                                //m_DeleteQueue.Enqueue( item );
                            }
                        }
                    }
                }
                else if (srcItem is Teleporter || srcItem is FillableContainer || srcItem is BaseBook)
                {
                    eable = map.GetItemsInRange(new Point3D(x, y, z), 0);

                    Type type = srcItem.GetType();

                    foreach (Item item in eable)
                    {
                        if (item.Z == z && item.ItemID == itemID)
                        {
                            if (item.GetType() != type)
                            {
                                //m_DeleteQueue.Enqueue(item);
                            }
                            else
                            {
                                m_DeleteQueue.Enqueue(item);
                                res = true;
                            }
                        }
                    }
                }
                else
                {
                    eable = map.GetItemsInRange(new Point3D(x, y, z), 0);

                    foreach (Item item in eable)
                    {
                        if (item.Z == z && item.ItemID == itemID)
                        {
                            m_DeleteQueue.Enqueue(item);
                            res = true;
                            //eable.Free();
                            //return true;
                        }
                    }
                }

                eable.Free();

                while (m_DeleteQueue.Count > 0)
                    m_DeleteQueue.Dequeue().Delete();

                return res;
            }

            public int Remove(Map[] maps)
            {
                int count = 0;

                Item item = null;

                for (int i = 0; i < m_Entries.Count; ++i)
                {
                    WorldCreatorDecor.DecorationEntry entry = (WorldCreatorDecor.DecorationEntry)m_Entries[i];
                    Point3D loc = entry.Location;
                    string extra = entry.Extra;

                    for (int j = 0; j < maps.Length; ++j)
                    {
                        if (item == null)
                            item = Construct();

                        m_Constructed = item;

                        if (item == null)
                            continue;

                        if (FindItemDelete(loc.X, loc.Y, loc.Z, maps[j], item))
                        {
                            ++count;
                        }
                    }
                }

                if (item != null)
                    item.Delete();

                return count;
            }

            public static ArrayList ReadAll(string path)
            {
                using (StreamReader ip = new StreamReader(path))
                {
                    ArrayList list = new ArrayList();

                    for (DecorationListDelete v = Read(ip); v != null; v = Read(ip))
                        list.Add(v);

                    return list;
                }
            }

            private static string[] m_EmptyParams = new string[0];

            public static DecorationListDelete Read(StreamReader ip)
            {
                string line;

                while ((line = ip.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.Length > 0 && !line.StartsWith("#"))
                        break;
                }

                if (string.IsNullOrEmpty(line))
                    return null;

                DecorationListDelete list = new DecorationListDelete();

                int indexOf = line.IndexOf(' ');

                list.m_Type = ScriptCompiler.FindTypeByName(line.Substring(0, indexOf++), true);

                if (list.m_Type == null)
                    throw new ArgumentException(String.Format("Type not found for header: '{0}'", line));

                line = line.Substring(indexOf);
                indexOf = line.IndexOf('(');
                if (indexOf >= 0)
                {
                    list.m_ItemID = Utility.ToInt32(line.Substring(0, indexOf - 1));

                    string parms = line.Substring(++indexOf);

                    if (line.EndsWith(")"))
                        parms = parms.Substring(0, parms.Length - 1);

                    list.m_Params = parms.Split(';');

                    for (int i = 0; i < list.m_Params.Length; ++i)
                        list.m_Params[i] = list.m_Params[i].Trim();
                }
                else
                {
                    list.m_ItemID = Utility.ToInt32(line);
                    list.m_Params = m_EmptyParams;
                }

                list.m_Entries = new ArrayList();

                while ((line = ip.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.Length == 0)
                        break;

                    if (line.StartsWith("#"))
                        continue;

                    list.m_Entries.Add(new WorldCreatorDecor.DecorationEntry(line));
                }

                return list;
            }
        }
    }
}