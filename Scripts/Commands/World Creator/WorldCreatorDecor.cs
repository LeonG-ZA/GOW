using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using System.IO;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;
using Server.Engines.Quests.Haven;
using Server.Engines.Quests.Necro;
using Server.Items;
using Server.WebConfiguration;
using Server.TestCenterConfiguration;

namespace Server.Gumps
{
    public class WorldCreatorDecor : Gump
    {
        public WorldCreatorDecor(Mobile from)
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

            AddPage(0);
            AddBackground(110, 38, 600, 509, 9200);
            if (!ns.IsKRClient)
            {
                AddImageTiled(124, 52, 568, 479, 2624);
            }
            AddAlphaRegion(125, 52, 567, 480);
            AddLabel(347, 66, 2728, @"World Creator");
            AddLabel(555, 509, 2728, @"@G.O.W, 2016");
            AddLabel(307, 125, 2728, @"Decoration");
            AddImage(297, 58, 52);
            AddImage(125, 51, 5609);
            AddImage(632, 51, 5609);
            AddButton(141, 180, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddButton(141, 206, 4005, 4007, 2, GumpButtonType.Reply, 0);
            AddButton(141, 232, 4005, 4007, 3, GumpButtonType.Reply, 0);
            AddButton(141, 258, 4005, 4007, 4, GumpButtonType.Reply, 0);
            AddButton(141, 284, 4005, 4007, 5, GumpButtonType.Reply, 0);
            AddButton(141, 310, 4005, 4007, 6, GumpButtonType.Reply, 0);
            AddButton(141, 336, 4005, 4007, 7, GumpButtonType.Reply, 0);
            AddButton(338, 180, 4005, 4007, 8, GumpButtonType.Reply, 0);
            AddButton(338, 206, 4005, 4007, 9, GumpButtonType.Reply, 0);
            AddButton(338, 232, 4005, 4007, 10, GumpButtonType.Reply, 0);
            AddButton(338, 258, 4005, 4007, 11, GumpButtonType.Reply, 0);
            AddButton(499, 180, 4005, 4007, 12, GumpButtonType.Reply, 0);
            AddButton(499, 206, 4005, 4007, 13, GumpButtonType.Reply, 0);
            //AddButton(499, 209, 4005, 4007, 13, GumpButtonType.Reply, 0);
            AddButton(499, 232, 4005, 4007, 14, GumpButtonType.Reply, 0);
            AddButton(499, 258, 4005, 4007, 15, GumpButtonType.Reply, 0);
            AddButton(499, 284, 4005, 4007, 16, GumpButtonType.Reply, 0);
            AddButton(499, 310, 4005, 4007, 17, GumpButtonType.Reply, 0);
            AddButton(499, 337, 4005, 4007, 18, GumpButtonType.Reply, 0);
            AddButton(499, 362, 4005, 4007, 19, GumpButtonType.Reply, 0);
            AddButton(499, 387, 4005, 4007, 20, GumpButtonType.Reply, 0);
            AddButton(499, 412, 4005, 4007, 21, GumpButtonType.Reply, 0);
            AddButton(499, 438, 4005, 4007, 22, GumpButtonType.Reply, 0);
            AddButton(338, 284, 4005, 4007, 23, GumpButtonType.Reply, 0);
            AddButton(338, 310, 4005, 4007, 24, GumpButtonType.Reply, 0);
            AddButton(338, 336, 4005, 4007, 25, GumpButtonType.Reply, 0);
            AddButton(339, 363, 4005, 4007, 26, GumpButtonType.Reply, 0);
            AddButton(339, 388, 4005, 4007, 27, GumpButtonType.Reply, 0);
            AddButton(339, 413, 4005, 4007, 28, GumpButtonType.Reply, 0);
            AddLabel(174, 181, 2728, @"T2A Decoration");
            AddLabel(174, 208, 2728, @"Felucca Decoration");
            AddLabel(174, 235, 2728, @"Trammel Decoration");
            AddLabel(174, 260, 2728, @"Ilshenar Decoration");
            AddLabel(174, 286, 2728, @"Malas Decoration");
            AddLabel(174, 312, 2728, @"Tokuno Decoration");
            AddLabel(174, 338, 2728, @"TerMur Decoration");
            AddLabel(372, 181, 2728, @"Decorate All Maps");
            AddLabel(372, 206, 2728, @"Decorate ML");
            AddLabel(372, 234, 2728, @"Decorate SA");
            AddLabel(372, 260, 2728, @"Decorate HS");
            AddLabel(532, 180, 2728, @"Door Generator");
            AddLabel(532, 209, 2728, @"Sign Generator");
            AddLabel(532, 233, 2728, @"Navery Stone Generator");
            AddLabel(532, 259, 2728, @"Solen Hive Generator");
            AddLabel(532, 285, 2728, @"Secret Loc Generator");
            AddLabel(532, 312, 2728, @"Primeval Lich Lever Gen");
            AddLabel(532, 336, 2728, @"Factions Generator");
            AddLabel(532, 363, 2728, @"Doom Generator");
            AddLabel(532, 389, 2728, @"Stealable Artifacts Gen");
            AddLabel(532, 414, 2728, @"Khaldun Generator");
            AddLabel(532, 439, 2728, @"Moongate Generator");
            AddLabel(373, 286, 2728, @"Tomb of Kings Gen");
            AddLabel(372, 311, 2728, @"Arisen Gen");
            AddLabel(372, 337, 2728, @"Despise Revamp Gen");
            AddLabel(372, 365, 2728, @"Dungeon Chest Gen");
            AddLabel(372, 389, 2728, @"Mad Scientist Gen");
            AddLabel(372, 416, 2728, @"Goblin Trap Gen");
        }

        public static void DoThis(Mobile from, string command)
        {
            string prefix = Server.Commands.CommandSystem.Prefix;
            CommandSystem.Handle(from, String.Format("{0}{1}", prefix, command));
        }

        public static void DoT2A(Mobile from)
        {
            DoThis(from, "ClassicMoongen");
            DoThis(from, "DoorGenT2A");
            DoThis(from, "SignPutT2A");
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
                        DoT2A(from);
                        DoThis(from, "DecorateT2A");
                        break;
                    }
                case 2:
                    {
                        DoThis(from, "DecorateFelucca");
                        break;
                    }
                case 3:
                    {
                        DoThis(from, "DecorateTrammel");
                        break;
                    }
                case 4:
                    {
                        DoThis(from, "DecorateIlshenar");
                        break;
                    }
                case 5:
                    {
                        DoThis(from, "DecorateMalas");
                        break;
                    }
                case 6:
                    {
                        DoThis(from, "DecorateTokuno");
                        break;
                    }
                case 7:
                    {
                        DoThis(from, "DecorateTerMur");
                        break;
                    }
                case 8:
                    {
                        DoThis(from, "DecorateALL");
                        //DoThis(from, "PeerlessML");
                        break;
                    }
                case 9:
                    {
                        DoThis(from, "DecorateML");
                        //DoThis(from, "PeerlessML");
                        break;
                    }
                case 10:
                    {
                        DoThis(from, "DecorateSA");
                        //DoThis(from, "PeerlessML");
                        break;
                    }
                case 11:
                    {
                        DoThis(from, "DecorateHS");
                        //DoThis(from, "PeerlessML");
                        break;
                    }
                case 12:
                    {
                        DoThis(from, "DoorGen");
                        break;
                    }
                case 13:
                    {
                        DoThis(from, "SignGen");
                        break;
                    }
                case 14:
                    {
                        DoThis(from, "GenNavrey");
                        break;
                    }
                case 15:
                    {
                        DoThis(from, "SHTelGen");
                        break;
                    }
                case 16:
                    {
                        DoThis(from, "SecretLocGen");
                        break;
                    }
                case 17:
                    {
                        DoThis(from, "GenLichPuzzle");
                        break;
                    }
                case 18:
                    {
                        DoThis(from, "GenerateFactions");
                        break;
                    }
                case 19:
                    {
                        DoThis(from, "GenGauntlet");
                        break;
                    }
                case 20:
                    {
                        DoThis(from, "GenStealArties");
                        break;
                    }
                case 21:
                    {
                        DoThis(from, "GenKhaldun");
                        break;
                    }
                case 22:
                    {
                        DoThis(from, "Moongen");
                        break;
                    }
                case 23:
                    {
                        DoThis(from, "GenToK");
                        break;
                    }
                case 24:
                    {
                        DoThis(from, "ArisenGenerate");
                        break;
                    }
                case 25:
                    {
                        DoThis(from, "SetupDespise");
                        break;
                    }
                case 26:
                    {
                        DoThis(from, "ChestsGenerate");
                        break;
                    }
                case 27:
                    {
                        DoThis(from, "GenSutek");
                        break;
                    }
                case 28:
                    {
                        DoThis(from, "GoblinTrapsGenerate");
                        break;
                    }
            }
        }

        public class Decorate
        {
            private static string m_Key;
            public static string Key
            {
                get { return m_Key; }
            }

            public static void Initialize()
            {
                CommandSystem.Register("DecorateALL", AccessLevel.Administrator, new CommandEventHandler(DecorateALL_OnCommand));
                CommandSystem.Register("DecorateT2A", AccessLevel.Administrator, new CommandEventHandler(DecorateT2A_OnCommand));
                CommandSystem.Register("DecorateFelucca", AccessLevel.Administrator, new CommandEventHandler(DecorateFelucca_OnCommand));
                CommandSystem.Register("DecorateTrammel", AccessLevel.Administrator, new CommandEventHandler(DecorateTrammel_OnCommand));
                CommandSystem.Register("DecorateIlshenar", AccessLevel.Administrator, new CommandEventHandler(DecorateIlshenar_OnCommand));
                CommandSystem.Register("DecorateMalas", AccessLevel.Administrator, new CommandEventHandler(DecorateMalas_OnCommand));
                CommandSystem.Register("DecorateTokuno", AccessLevel.Administrator, new CommandEventHandler(DecorateTokuno_OnCommand));
                CommandSystem.Register("DecorateTerMur", AccessLevel.Administrator, new CommandEventHandler(DecorateTerMur_OnCommand));
                CommandSystem.Register("DecorateML", AccessLevel.Administrator, new CommandEventHandler(DecorateML_OnCommand));
                CommandSystem.Register("DecorateSA", AccessLevel.Administrator, new CommandEventHandler(DecorateSA_OnCommand));
                CommandSystem.Register("DecorateHS", AccessLevel.Administrator, new CommandEventHandler(DecorateHS_OnCommand));
                //CommandSystem.Register("PeerlessML", AccessLevel.Administrator, new CommandEventHandler(PeerlessML_OnCommand));
            }

            [Usage("DecorateT2A")]
            [Description("Generates world decorations for a T2A shard.")]
            private static void DecorateT2A_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;
                m_Count = 0;
                m_Mobile.SendMessage("Generating T2A decoration, please wait.");

                Generate("decoT2A", "Data/World/Decoration/UOT2A/Britannia", Map.Felucca);

                m_Mobile.SendMessage("T2A generating complete. {0} items were generated.", m_Count);
            }

            [Usage("DecorateFelucca")]
            [Description("Generates felucca decorations for your shard.")]
            private static void DecorateFelucca_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;
                m_Count = 0;
                m_Mobile.SendMessage("Generating Felucca decoration, please wait.");

                Generate("decoFel", "Data/World/Decoration/All/Britannia", Map.Felucca);
                Generate("decoFel", "Data/World/Decoration/All/Felucca", Map.Felucca);

                m_Mobile.SendMessage("Felucca generating complete. {0} items were generated.", m_Count);
            }

            [Usage("DecorateTrammel")]
            [Description("Generates Trammel decorations for your shard.")]
            private static void DecorateTrammel_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;
                m_Count = 0;
                m_Mobile.SendMessage("Generating Trammel decoration, please wait.");

                Generate("decoTram", "Data/World/Decoration/All/Britannia", Map.Trammel);
                Generate("decoTram", "Data/World/Decoration/All/Trammel", Map.Trammel);

                m_Mobile.SendMessage("Trammel generating complete. {0} items were generated.", m_Count);
            }

            [Usage("DecorateIlshenar")]
            [Description("Generates Ilshenar decorations for your shard.")]
            private static void DecorateIlshenar_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;
                m_Count = 0;
                m_Mobile.SendMessage("Generating Ilshenar decoration, please wait.");

                Generate("decoIlsh", "Data/World/Decoration/All/Ilshenar", Map.Ilshenar);

                m_Mobile.SendMessage("Ilshenar generating complete. {0} items were generated.", m_Count);
            }

            [Usage("DecorateMalas")]
            [Description("Generates Malas decorations for your shard.")]
            private static void DecorateMalas_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;
                m_Count = 0;
                m_Mobile.SendMessage("Generating Malas decoration, please wait.");
                Generate("decoMal", "Data/World/Decoration/All/Malas", Map.Malas);
                m_Mobile.SendMessage("Malas generating complete. {0} items were generated.", m_Count);
            }

            [Usage("DecorateTokuno")]
            [Description("Generates Tokuno decorations for your shard.")]
            private static void DecorateTokuno_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;
                m_Count = 0;
                m_Mobile.SendMessage("Generating Tokuno decoration, please wait.");

                Generate("decoTok", "Data/World/Decoration/All/Tokuno", Map.Tokuno);

                m_Mobile.SendMessage("Tokuno generating complete. {0} items were generated.", m_Count);
            }

            [Usage("DecorateTerMur")]
            [Description("Generates Stygian Abyss world decoration.")]
            private static void DecorateTerMur_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;

                m_Mobile.SendMessage("Generating Ter Mur world decoration, please wait.");

                Generate("decoTerM", "Data/World/Decoration/All/Ter Mur", Map.TerMur);

                m_Mobile.SendMessage("Ter Mur world generation complete.");
            }

            [Usage("DecorateALL")]
            [Description("Generates all world decoration.")]
            private static void DecorateALL_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;
                m_Count = 0;

                m_Mobile.SendMessage("Generating world decoration, please wait.");

                Generate("decoAll", "Data/World/Decoration/All/Britannia", Map.Trammel, Map.Felucca);
                Generate("decoAll", "Data/World/Decoration/All/Trammel", Map.Trammel);
                Generate("decoALl", "Data/World/Decoration/All/Felucca", Map.Felucca);
                Generate("decoAll", "Data/World/Decoration/All/Ilshenar", Map.Ilshenar);
                Generate("decoAll", "Data/World/Decoration/All/Malas", Map.Malas);
                Generate("decoAll", "Data/World/Decoration/All/Tokuno", Map.Tokuno);
                Generate("decoAll", "Data/World/Decoration/All/Ter Mur", Map.TerMur);

                if (TestCenterConfig.TestCenterEnabled)
                {
                    Generate("decoAll", "Data/World/Decoration/All/TC/Felucca", Map.Felucca);
                    Generate("decoAll", "Data/World/Decoration/All/TC/Trammel", Map.Trammel);
                    Generate("decoAll", "Data/World/Decoration/All/TC/Tokuno", Map.Tokuno);
                }

                m_Mobile.SendMessage("World generating complete. {0} items were generated.", m_Count);
            }

            //[Usage("PeerlessML")]
            //[Description("Generates Peerless ML world decoration.")]
            //private static void PeerlessML_OnCommand(CommandEventArgs e)
            //{
                /*
                PeerlessAltar altar;
                PeerlessTeleporter tele;
                PrismOfLightPillar pillar;

                altar = new BedlamAltar();

                if (!FindItem(86, 1627, 0, Map.Malas, altar))
                {
                    altar = new BedlamAltar();
                    altar.MoveToWorld(new Point3D(86, 1627, 0), Map.Malas);
                    tele = new PeerlessTeleporter(altar);
                    tele.PointDest = altar.ExitDest;
                    tele.MoveToWorld(new Point3D(99, 1617, 50), Map.Malas);
                }

                // Blighted Grove - Trammel
                altar = new BlightedGroveAltar();

                if (!FindItem(6502, 875, 0, Map.Trammel, altar))
                {
                    altar.MoveToWorld(new Point3D(6502, 875, 0), Map.Trammel);
                    tele = new PeerlessTeleporter(altar);
                    tele.PointDest = altar.ExitDest;
                    tele.MoveToWorld(new Point3D(6511, 949, 26), Map.Trammel);
                }

                // Blighted Grove - Felucca
                altar = new BlightedGroveAltar();

                if (!FindItem(6502, 875, 0, Map.Felucca, altar))
                {
                    altar.MoveToWorld(new Point3D(6502, 875, 0), Map.Felucca);
                    tele = new PeerlessTeleporter(altar);
                    tele.PointDest = altar.ExitDest;
                    tele.MoveToWorld(new Point3D(6511, 949, 26), Map.Felucca);
                }

                // Palace of Paroxysmus - Trammel
                altar = new ParoxysmusAltar();

                if (!FindItem(6511, 506, -34, Map.Trammel, altar))
                {
                    altar.MoveToWorld(new Point3D(6511, 506, -34), Map.Trammel);
                    tele = new PeerlessTeleporter(altar);
                    tele.PointDest = altar.ExitDest;
                    tele.MoveToWorld(new Point3D(6518, 365, 46), Map.Trammel);
                }

                // Palace of Paroxysmus - Felucca
                altar = new ParoxysmusAltar();

                if (!FindItem(6511, 506, -34, Map.Felucca, altar))
                {
                    altar.MoveToWorld(new Point3D(6511, 506, -34), Map.Felucca);
                    tele = new PeerlessTeleporter(altar);
                    tele.PointDest = altar.ExitDest;
                    tele.MoveToWorld(new Point3D(6518, 365, 46), Map.Felucca);
                }

                // Prism of Light - Trammel
                altar = new PrismOfLightAltar();

                if (!FindItem(6509, 167, 6, Map.Trammel, altar))
                {
                    altar.MoveToWorld(new Point3D(6509, 167, 6), Map.Trammel);
                    tele = new PeerlessTeleporter(altar);
                    tele.PointDest = altar.ExitDest;
                    tele.Visible = true;
                    tele.ItemID = 0xDDA;
                    tele.MoveToWorld(new Point3D(6501, 137, -20), Map.Trammel);

                    pillar = new PrismOfLightPillar((PrismOfLightAltar)altar, 0x581);
                    pillar.MoveToWorld(new Point3D(6506, 167, 0), Map.Trammel);

                    pillar = new PrismOfLightPillar((PrismOfLightAltar)altar, 0x581);
                    pillar.MoveToWorld(new Point3D(6509, 164, 0), Map.Trammel);

                    pillar = new PrismOfLightPillar((PrismOfLightAltar)altar, 0x581);
                    pillar.MoveToWorld(new Point3D(6506, 164, 0), Map.Trammel);

                    pillar = new PrismOfLightPillar((PrismOfLightAltar)altar, 0x481);
                    pillar.MoveToWorld(new Point3D(6512, 167, 0), Map.Trammel);

                    pillar = new PrismOfLightPillar((PrismOfLightAltar)altar, 0x481);
                    pillar.MoveToWorld(new Point3D(6509, 170, 0), Map.Trammel);

                    pillar = new PrismOfLightPillar((PrismOfLightAltar)altar, 0x481);
                    pillar.MoveToWorld(new Point3D(6512, 170, 0), Map.Trammel);
                }

                // Prism of Light - Felucca
                altar = new PrismOfLightAltar();

                if (!FindItem(6509, 167, 6, Map.Felucca, altar))
                {
                    altar.MoveToWorld(new Point3D(6509, 167, 6), Map.Felucca);
                    tele = new PeerlessTeleporter(altar);
                    tele.PointDest = altar.ExitDest;
                    tele.Visible = true;
                    tele.ItemID = 0xDDA;
                    tele.MoveToWorld(new Point3D(6501, 137, -20), Map.Felucca);

                    pillar = new PrismOfLightPillar((PrismOfLightAltar)altar, 0x581);
                    pillar.MoveToWorld(new Point3D(6506, 167, 0), Map.Felucca);

                    pillar = new PrismOfLightPillar((PrismOfLightAltar)altar, 0x581);
                    pillar.MoveToWorld(new Point3D(6509, 164, 0), Map.Felucca);

                    pillar = new PrismOfLightPillar((PrismOfLightAltar)altar, 0x581);
                    pillar.MoveToWorld(new Point3D(6506, 164, 0), Map.Felucca);

                    pillar = new PrismOfLightPillar((PrismOfLightAltar)altar, 0x481);
                    pillar.MoveToWorld(new Point3D(6512, 167, 0), Map.Felucca);

                    pillar = new PrismOfLightPillar((PrismOfLightAltar)altar, 0x481);
                    pillar.MoveToWorld(new Point3D(6509, 170, 0), Map.Felucca);

                    pillar = new PrismOfLightPillar((PrismOfLightAltar)altar, 0x481);
                    pillar.MoveToWorld(new Point3D(6512, 170, 0), Map.Felucca);
                }

                // The Citadel - Malas
                altar = new CitadelAltar();

                if (!FindItem(89, 1885, 0, Map.Malas, altar))
                {
                    altar.MoveToWorld(new Point3D(89, 1885, 0), Map.Malas);
                    tele = new PeerlessTeleporter(altar);
                    tele.PointDest = altar.ExitDest;
                    tele.MoveToWorld(new Point3D(111, 1955, 0), Map.Malas);
                }

                // Twisted Weald - Ilshenar
                altar = new TwistedWealdAltar();

                if (!FindItem(2170, 1255, -60, Map.Ilshenar, altar))
                {
                    altar.MoveToWorld(new Point3D(2170, 1255, -60), Map.Ilshenar);
                    tele = new PeerlessTeleporter(altar);
                    tele.PointDest = altar.ExitDest;
                    tele.MoveToWorld(new Point3D(2139, 1271, -57), Map.Ilshenar);
                }

                /*
                // Stygian Dragon Lair - Abyss
                altar = new StygianDragonAltar();

                if (!FindItem(363, 157, 5, Map.TerMur, altar))
                {
                    altar.MoveToWorld(new Point3D(363, 157, 0), Map.TerMur);
                    tele = new PeerlessTeleporter(altar);
                    tele.PointDest = altar.ExitDest;

                    tele.MoveToWorld(new Point3D(305, 159, 105), Map.TerMur);

                    brazier = new StygianDragonBrazier((StygianDragonAltar)altar, 0x207B);
                    brazier.MoveToWorld(new Point3D(362, 156, 5), Map.TerMur);

                    brazier = new StygianDragonBrazier((StygianDragonAltar)altar, 0x207B);
                    brazier.MoveToWorld(new Point3D(364, 156, 7), Map.TerMur);

                    brazier = new StygianDragonBrazier((StygianDragonAltar)altar, 0x207B);
                    brazier.MoveToWorld(new Point3D(364, 158, 7), Map.TerMur);

                    brazier = new StygianDragonBrazier((StygianDragonAltar)altar, 0x207B);
                    brazier.MoveToWorld(new Point3D(362, 158, 7), Map.TerMur);
                }

                //Medusa Lair - Abyss
                altar = new MedusaAltar();

                if (!FindItem(822, 756, 56, Map.TerMur, altar))
                {
                    altar.MoveToWorld(new Point3D(822, 756, 56), Map.TerMur);
                    tele = new PeerlessTeleporter(altar);
                    tele.PointDest = altar.ExitDest;

                    tele.MoveToWorld(new Point3D(840, 926, -5), Map.TerMur);

                    nest = new MedusaNest((MedusaAltar)altar, 0x207B);
                    nest.MoveToWorld(new Point3D(821, 755, 56), Map.TerMur);

                    nest = new MedusaNest((MedusaAltar)altar, 0x207B);
                    nest.MoveToWorld(new Point3D(823, 755, 56), Map.TerMur);

                    nest = new MedusaNest((MedusaAltar)altar, 0x207B);
                    nest.MoveToWorld(new Point3D(821, 757, 56), Map.TerMur);

                    nest = new MedusaNest((MedusaAltar)altar, 0x207B);
                    nest.MoveToWorld(new Point3D(823, 757, 56), Map.TerMur);
                }
                 */
            //}

            [Usage("DecorateML")]
            [Description("Generates Mondain's Legacy world decoration.")]
            private static void DecorateML_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;

                m_Mobile.SendMessage("Generating Mondain's Legacy world decoration, please wait.");

                Generate("DecoML", "Data/World/Decoration/UOML/Trammel", Map.Trammel);
                Generate("DecoML", "Data/World/Decoration/UOML/Felucca", Map.Felucca);
                Generate("DecoML", "Data/World/Decoration/UOML/Malas", Map.Malas);
                Generate("DecoML", "Data/World/Decoration/UOML/Tokuno", Map.Tokuno);

                m_Mobile.SendMessage("Mondain's Legacy world generating complete.");
            }

            [Usage("DecorateSA")]
            [Description("Generates Stygian Abyss world decoration.")]
            private static void DecorateSA_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;

                m_Mobile.SendMessage("Generating Stygian Abyss world decoration, please wait.");

                Generate("DecoSA", "Data/World/Decoration/UOSA/Britannia", Map.Trammel, Map.Felucca);
                Generate("DecoSA", "Data/World/Decoration/UOSA/Trammel", Map.Trammel);
                Generate("DecoSA", "Data/World/Decoration/UOSA/Felucca", Map.Felucca);
                Generate("DecoSA", "Data/World/Decoration/UOSA/Ilshenar", Map.Ilshenar);
                Generate("DecoSA", "Data/World/Decoration/UOSA/Malas", Map.Malas);
                Generate("DecoSA", "Data/World/Decoration/UOSA/Tokuno", Map.Tokuno);
                Generate("DecoSA", "Data/World/Decoration/UOSA/Ter Mur", Map.TerMur);

                m_Mobile.SendMessage("Stygian Abyss world generation complete.");
            }

            [Usage("DecorateHS")]
            [Description("Generates High Seas decorations for your shard.")]
            private static void DecorateHS_OnCommand(CommandEventArgs e)
            {
                m_Mobile = e.Mobile;
                m_Count = 0;
                m_Mobile.SendMessage("Generating High Seas decoration, please wait.");

                Generate("DecoHS", "Data/World/Decoration/UOHS/Britannia", Map.Trammel, Map.Felucca);
                Generate("DecoHS", "Data/World/Decoration/UOHS/Trammel", Map.Trammel);
                Generate("DecoHS", "Data/World/Decoration/UOHS/Felucca", Map.Felucca);
                Generate("DecoHS", "Data/World/Decoration/UOHS/Ilshenar", Map.Ilshenar);
                Generate("DecoHS", "Data/World/Decoration/UOHS/Malas", Map.Malas);
                Generate("DecoHS", "Data/World/Decoration/UOHS/Tokuno", Map.Tokuno);
                Generate("DecoHS", "Data/World/Decoration/UOHS/Ter Mur", Map.TerMur);

                m_Mobile.SendMessage("High Seas generating complete. {0} items were generated.", m_Count);
            }

            public static bool FindItem(int x, int y, int z, Map map, Item test)
            {
                return FindItem(new Point3D(x, y, z), map, test);
            }

            public static bool FindItem(Point3D p, Map map, Item test)
            {
                IPooledEnumerable eable = map.GetItemsInRange(p);

                foreach (Item item in eable)
                {
                    if (item.Z == p.Z && item.ItemID == test.ItemID)
                    {
                        eable.Free();
                        return true;
                    }
                }

                eable.Free();
                return false;
            }

            public static void Generate(string keyName, string folder, params Map[] maps)
            {
                m_Key = keyName;

                if (!Directory.Exists(folder))
                    return;

                string[] files = Directory.GetFiles(folder, "*.cfg");

                for (int i = 0; i < files.Length; ++i)
                {
                    ArrayList list = DecorationList.ReadAll(files[i]);

                    m_List = list;

                    for (int j = 0; j < list.Count; ++j)
                        m_Count += ((DecorationList)list[j]).Generate(maps);
                }
            }

            public static Item FindByID(int id)
            {
                if (m_List == null)
                    return null;

                for (int j = 0; j < m_List.Count; ++j)
                {
                    DecorationList list = (DecorationList)m_List[j];

                    if (list.ID == id)
                        return list.Constructed;
                }

                return null;
            }

            private static ArrayList m_List;
            private static Mobile m_Mobile;
            private static int m_Count;
        }

        public class DecorationList
        {
            private Type m_Type;
            private int m_ItemID;
            private string[] m_Params;
            private ArrayList m_Entries;
            private Item m_Constructed;

            public Item Constructed
            {
                get
                {
                    return this.m_Constructed;
                }
            }

            public int ID
            {
                get
                {
                    for (int i = 0; i < this.m_Params.Length; ++i)
                    {
                        if (this.m_Params[i].StartsWith("ID"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                return Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                        }
                    }

                    return 0;
                }
            }

            public DecorationList()
            {
            }

            private static readonly Type typeofStatic = typeof(Static);
            private static readonly Type typeofLocalizedStatic = typeof(LocalizedStatic);
            private static readonly Type typeofBaseDoor = typeof(BaseDoor);
            private static readonly Type typeofAnkhWest = typeof(AnkhWest);
            private static readonly Type typeofAnkhNorth = typeof(AnkhNorth);
            private static readonly Type typeofBeverage = typeof(BaseBeverage);
            private static readonly Type typeofLocalizedSign = typeof(LocalizedSign);
            private static readonly Type typeofMarkContainer = typeof(MarkContainer);
            private static readonly Type typeofWarningItem = typeof(WarningItem);
            private static readonly Type typeofHintItem = typeof(HintItem);
            private static readonly Type typeofCannon = typeof(Cannon);
            private static readonly Type typeofSerpentPillar = typeof(SerpentPillar);
            private static readonly Type typeofResurrectItem = typeof(ResurrectItem);
            private static readonly Type typeofUnderworldSecretDoor = typeof(UnderworldSecretDoor);
            private static readonly Type typeofVinesLockedSecretDoor = typeof(VinesLockedSecretDoor);
            private static readonly Type typeofTombOfKingsSecretDoor = typeof(TombOfKingsSecretDoor);

            public Item Construct()
            {
                if (this.m_Type == null)
                    return null;

                Item item;

                try
                {
                    if (this.m_Type == typeofStatic)
                    {
                        item = new Static(this.m_ItemID);
                    }
                    else if (this.m_Type == typeofLocalizedStatic)
                    {
                        int labelNumber = 0;

                        for (int i = 0; i < this.m_Params.Length; ++i)
                        {
                            if (this.m_Params[i].StartsWith("LabelNumber"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                {
                                    labelNumber = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                                    break;
                                }
                            }
                        }

                        item = new LocalizedStatic(this.m_ItemID, labelNumber);
                    }
                    else if (this.m_Type == typeofLocalizedSign)
                    {
                        int labelNumber = 0;

                        for (int i = 0; i < this.m_Params.Length; ++i)
                        {
                            if (this.m_Params[i].StartsWith("LabelNumber"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                {
                                    labelNumber = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                                    break;
                                }
                            }
                        }

                        item = new LocalizedSign(this.m_ItemID, labelNumber);
                    }
                    else if (this.m_Type == typeofAnkhWest || this.m_Type == typeofAnkhNorth)
                    {
                        bool bloodied = false;

                        for (int i = 0; !bloodied && i < this.m_Params.Length; ++i)
                            bloodied = (this.m_Params[i] == "Bloodied");

                        if (this.m_Type == typeofAnkhWest)
                            item = new AnkhWest(bloodied);
                        else
                            item = new AnkhNorth(bloodied);
                    }
                    else if (this.m_Type == typeofMarkContainer)
                    {
                        bool bone = false;
                        bool locked = false;
                        Map map = Map.Malas;

                        for (int i = 0; i < this.m_Params.Length; ++i)
                        {
                            if (this.m_Params[i] == "Bone")
                            {
                                bone = true;
                            }
                            else if (this.m_Params[i] == "Locked")
                            {
                                locked = true;
                            }
                            else if (this.m_Params[i].StartsWith("TargetMap"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    map = Map.Parse(this.m_Params[i].Substring(++indexOf));
                            }
                        }

                        MarkContainer mc = new MarkContainer(bone, locked);

                        mc.TargetMap = map;
                        mc.Description = "strange location";

                        item = mc;
                    }
                    else if (this.m_Type == typeofHintItem)
                    {
                        int range = 0;
                        int messageNumber = 0;
                        string messageString = null;
                        int hintNumber = 0;
                        string hintString = null;
                        TimeSpan resetDelay = TimeSpan.Zero;

                        for (int i = 0; i < this.m_Params.Length; ++i)
                        {
                            if (this.m_Params[i].StartsWith("Range"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    range = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                            }
                            else if (this.m_Params[i].StartsWith("WarningString"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    messageString = this.m_Params[i].Substring(++indexOf);
                            }
                            else if (this.m_Params[i].StartsWith("WarningNumber"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    messageNumber = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                            }
                            else if (this.m_Params[i].StartsWith("HintString"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    hintString = this.m_Params[i].Substring(++indexOf);
                            }
                            else if (this.m_Params[i].StartsWith("HintNumber"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    hintNumber = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                            }
                            else if (this.m_Params[i].StartsWith("ResetDelay"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    resetDelay = TimeSpan.Parse(this.m_Params[i].Substring(++indexOf));
                            }
                        }

                        HintItem hi = new HintItem(this.m_ItemID, range, messageNumber, hintNumber);

                        hi.WarningString = messageString;
                        hi.HintString = hintString;
                        hi.ResetDelay = resetDelay;

                        item = hi;
                    }
                    else if (this.m_Type == typeofWarningItem)
                    {
                        int range = 0;
                        int messageNumber = 0;
                        string messageString = null;
                        TimeSpan resetDelay = TimeSpan.Zero;

                        for (int i = 0; i < this.m_Params.Length; ++i)
                        {
                            if (this.m_Params[i].StartsWith("Range"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    range = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                            }
                            else if (this.m_Params[i].StartsWith("WarningString"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    messageString = this.m_Params[i].Substring(++indexOf);
                            }
                            else if (this.m_Params[i].StartsWith("WarningNumber"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    messageNumber = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                            }
                            else if (this.m_Params[i].StartsWith("ResetDelay"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    resetDelay = TimeSpan.Parse(this.m_Params[i].Substring(++indexOf));
                            }
                        }

                        WarningItem wi = new WarningItem(this.m_ItemID, range, messageNumber);

                        wi.WarningString = messageString;
                        wi.ResetDelay = resetDelay;

                        item = wi;
                    }
                    else if (this.m_Type == typeofCannon)
                    {
                        CannonDirection direction = CannonDirection.North;

                        for (int i = 0; i < this.m_Params.Length; ++i)
                        {
                            if (this.m_Params[i].StartsWith("CannonDirection"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    direction = (CannonDirection)Enum.Parse(typeof(CannonDirection), this.m_Params[i].Substring(++indexOf), true);
                            }
                        }

                        item = new Cannon(direction);
                    }
                    else if (this.m_Type == typeofSerpentPillar)
                    {
                        string word = null;
                        Rectangle2D destination = new Rectangle2D();

                        for (int i = 0; i < this.m_Params.Length; ++i)
                        {
                            if (this.m_Params[i].StartsWith("Word"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    word = this.m_Params[i].Substring(++indexOf);
                            }
                            else if (this.m_Params[i].StartsWith("DestStart"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    destination.Start = Point2D.Parse(this.m_Params[i].Substring(++indexOf));
                            }
                            else if (this.m_Params[i].StartsWith("DestEnd"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    destination.End = Point2D.Parse(this.m_Params[i].Substring(++indexOf));
                            }
                        }

                        item = new SerpentPillar(word, destination);
                    }
                    else if (this.m_Type.IsSubclassOf(typeofBeverage))
                    {
                        BeverageType content = BeverageType.Liquor;
                        bool fill = false;

                        for (int i = 0; !fill && i < this.m_Params.Length; ++i)
                        {
                            if (this.m_Params[i].StartsWith("Content"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                {
                                    content = (BeverageType)Enum.Parse(typeof(BeverageType), this.m_Params[i].Substring(++indexOf), true);
                                    fill = true;
                                }
                            }
                        }

                        if (fill)
                            item = (Item)Activator.CreateInstance(this.m_Type, new object[] { content });
                        else
                            item = (Item)Activator.CreateInstance(this.m_Type);
                    }
                    else if (m_Type == typeofResurrectItem)
                    {
                        ResurrectMessage message = ResurrectMessage.Generic;

                        for (int i = 0; i < m_Params.Length; ++i)
                        {
                            if (m_Params[i].StartsWith("ResurrectMessage"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                {
                                    message = (ResurrectMessage)Enum.Parse(typeof(ResurrectMessage), m_Params[i].Substring(++indexOf), true);
                                }
                            }
                        }

                        item = new ResurrectItem(message);
                    }
                    else if (m_Type == typeofUnderworldSecretDoor)
                    {
                        int closedId = 1, mediumId = 1;

                        for (int i = 0; i < m_Params.Length; ++i)
                        {
                            if (m_Params[i].StartsWith("ClosedId"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                {
                                    closedId = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                                }
                            }
                            else if (m_Params[i].StartsWith("MediumId"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                {
                                    mediumId = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                                }
                            }
                        }

                        item = new UnderworldSecretDoor(closedId, mediumId);
                    }
                    else if (m_Type == typeofTombOfKingsSecretDoor)
                    {
                        int closedId = 1;

                        for (int i = 0; i < m_Params.Length; ++i)
                        {
                            if (m_Params[i].StartsWith("ClosedId"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                {
                                    closedId = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                                }
                            }
                        }

                        item = new TombOfKingsSecretDoor(closedId);
                    }
                    else if (m_Type == typeofVinesLockedSecretDoor)
                    {
                        int closedId = 1, mediumId = 1;

                        for (int i = 0; i < m_Params.Length; ++i)
                        {
                            if (m_Params[i].StartsWith("ClosedId"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                {
                                    closedId = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                                }
                            }
                            else if (m_Params[i].StartsWith("MediumId"))
                            {
                                int indexOf = m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                {
                                    mediumId = Utility.ToInt32(m_Params[i].Substring(++indexOf));
                                }
                            }
                        }

                        item = new VinesLockedSecretDoor(closedId, mediumId);
                    }
                    else if (this.m_Type.IsSubclassOf(typeofBaseDoor))
                    {
                        DoorFacing facing = DoorFacing.WestCW;

                        for (int i = 0; i < this.m_Params.Length; ++i)
                        {
                            if (this.m_Params[i].StartsWith("Facing"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                {
                                    facing = (DoorFacing)Enum.Parse(typeof(DoorFacing), this.m_Params[i].Substring(++indexOf), true);
                                    break;
                                }
                            }
                        }

                        item = (Item)Activator.CreateInstance(this.m_Type, new object[] { facing });
                    }
                    else
                    {
                        item = (Item)Activator.CreateInstance(this.m_Type);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(String.Format("Bad type: {0}", this.m_Type), e);
                }

                if (item is BaseAddon)
                {
                    if (item is MaabusCoffin)
                    {
                        MaabusCoffin coffin = (MaabusCoffin)item;

                        for (int i = 0; i < this.m_Params.Length; ++i)
                        {
                            if (this.m_Params[i].StartsWith("SpawnLocation"))
                            {
                                int indexOf = this.m_Params[i].IndexOf('=');

                                if (indexOf >= 0)
                                    coffin.SpawnLocation = Point3D.Parse(this.m_Params[i].Substring(++indexOf));
                            }
                        }
                    }
                    else if (this.m_ItemID > 0)
                    {
                        List<AddonComponent> comps = ((BaseAddon)item).Components;

                        for (int i = 0; i < comps.Count; ++i)
                        {
                            AddonComponent comp = (AddonComponent)comps[i];

                            if (comp.Offset == Point3D.Zero)
                                comp.ItemID = this.m_ItemID;
                        }
                    }
                }
                else if (item is BaseLight)
                {
                    bool unlit = false, unprotected = false;

                    for (int i = 0; i < this.m_Params.Length; ++i)
                    {
                        if (!unlit && this.m_Params[i] == "Unlit")
                            unlit = true;
                        else if (!unprotected && this.m_Params[i] == "Unprotected")
                            unprotected = true;

                        if (unlit && unprotected)
                            break;
                    }

                    if (!unlit)
                        ((BaseLight)item).Ignite();
                    if (!unprotected)
                        ((BaseLight)item).Protected = true;

                    if (this.m_ItemID > 0)
                        item.ItemID = this.m_ItemID;
                }
                else if (item is Server.Mobiles.Spawner)
                {
                    Server.Mobiles.Spawner sp = (Server.Mobiles.Spawner)item;

                    sp.NextSpawn = TimeSpan.Zero;

                    for (int i = 0; i < this.m_Params.Length; ++i)
                    {
                        if (this.m_Params[i].StartsWith("Spawn"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.SpawnNames.Add(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("MinDelay"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.MinDelay = TimeSpan.Parse(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("MaxDelay"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.MaxDelay = TimeSpan.Parse(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("NextSpawn"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.NextSpawn = TimeSpan.Parse(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("Count"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.SpawnMax = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("Team"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.Team = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("HomeRange"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.HomeRange = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("Running"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.Running = Utility.ToBoolean(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("Group"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                sp.Group = Utility.ToBoolean(this.m_Params[i].Substring(++indexOf));
                        }
                    }
                }
                else if (item is RecallRune)
                {
                    RecallRune rune = (RecallRune)item;

                    for (int i = 0; i < this.m_Params.Length; ++i)
                    {
                        if (this.m_Params[i].StartsWith("Description"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                rune.Description = this.m_Params[i].Substring(++indexOf);
                        }
                        else if (this.m_Params[i].StartsWith("Marked"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                rune.Marked = Utility.ToBoolean(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("TargetMap"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                rune.TargetMap = Map.Parse(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("Target"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                rune.Target = Point3D.Parse(this.m_Params[i].Substring(++indexOf));
                        }
                    }
                }
                else if (item is SkillTeleporter)
                {
                    SkillTeleporter tp = (SkillTeleporter)item;

                    for (int i = 0; i < this.m_Params.Length; ++i)
                    {
                        if (this.m_Params[i].StartsWith("Skill"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Skill = (SkillName)Enum.Parse(typeof(SkillName), this.m_Params[i].Substring(++indexOf), true);
                        }
                        else if (this.m_Params[i].StartsWith("RequiredFixedPoint"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Required = Utility.ToInt32(this.m_Params[i].Substring(++indexOf)) * 0.1;
                        }
                        else if (this.m_Params[i].StartsWith("Required"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Required = Utility.ToDouble(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("MessageString"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.MessageString = this.m_Params[i].Substring(++indexOf);
                        }
                        else if (this.m_Params[i].StartsWith("MessageNumber"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.MessageNumber = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("PointDest"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.PointDest = Point3D.Parse(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("MapDest"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.MapDest = Map.Parse(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("Creatures"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Creatures = Utility.ToBoolean(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("SourceEffect"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.SourceEffect = Utility.ToBoolean(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("DestEffect"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.DestEffect = Utility.ToBoolean(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("SoundID"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.SoundID = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("Delay"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Delay = TimeSpan.Parse(this.m_Params[i].Substring(++indexOf));
                        }
                    }

                    if (this.m_ItemID > 0)
                        item.ItemID = this.m_ItemID;
                }
                else if (item is KeywordTeleporter)
                {
                    KeywordTeleporter tp = (KeywordTeleporter)item;

                    for (int i = 0; i < this.m_Params.Length; ++i)
                    {
                        if (this.m_Params[i].StartsWith("Substring"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Substring = this.m_Params[i].Substring(++indexOf);
                        }
                        else if (this.m_Params[i].StartsWith("Keyword"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Keyword = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("Range"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Range = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("PointDest"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.PointDest = Point3D.Parse(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("MapDest"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.MapDest = Map.Parse(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("Creatures"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Creatures = Utility.ToBoolean(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("SourceEffect"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.SourceEffect = Utility.ToBoolean(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("DestEffect"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.DestEffect = Utility.ToBoolean(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("SoundID"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.SoundID = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("Delay"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Delay = TimeSpan.Parse(this.m_Params[i].Substring(++indexOf));
                        }
                    }

                    if (this.m_ItemID > 0)
                        item.ItemID = this.m_ItemID;
                }
                else if (item is Teleporter)
                {
                    Teleporter tp = (Teleporter)item;

                    for (int i = 0; i < this.m_Params.Length; ++i)
                    {
                        if (this.m_Params[i].StartsWith("PointDest"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.PointDest = Point3D.Parse(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("MapDest"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.MapDest = Map.Parse(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("Creatures"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Creatures = Utility.ToBoolean(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("SourceEffect"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.SourceEffect = Utility.ToBoolean(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("DestEffect"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.DestEffect = Utility.ToBoolean(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("SoundID"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.SoundID = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                        }
                        else if (this.m_Params[i].StartsWith("Delay"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                tp.Delay = TimeSpan.Parse(this.m_Params[i].Substring(++indexOf));
                        }
                    }

                    if (this.m_ItemID > 0)
                        item.ItemID = this.m_ItemID;
                }
                else if (item is FillableContainer)
                {
                    FillableContainer cont = (FillableContainer)item;

                    for (int i = 0; i < this.m_Params.Length; ++i)
                    {
                        if (this.m_Params[i].StartsWith("ContentType"))
                        {
                            int indexOf = this.m_Params[i].IndexOf('=');

                            if (indexOf >= 0)
                                cont.ContentType = (FillableContentType)Enum.Parse(typeof(FillableContentType), this.m_Params[i].Substring(++indexOf), true);
                        }
                    }

                    if (this.m_ItemID > 0)
                        item.ItemID = this.m_ItemID;
                }
                else if (this.m_ItemID > 0)
                {
                    item.ItemID = this.m_ItemID;
                }

                item.Movable = false;

                for (int i = 0; i < this.m_Params.Length; ++i)
                {
                    if (this.m_Params[i].StartsWith("Light"))
                    {
                        int indexOf = this.m_Params[i].IndexOf('=');

                        if (indexOf >= 0)
                            item.Light = (LightType)Enum.Parse(typeof(LightType), this.m_Params[i].Substring(++indexOf), true);
                    }
                    else if (this.m_Params[i].StartsWith("Hue"))
                    {
                        int indexOf = this.m_Params[i].IndexOf('=');

                        if (indexOf >= 0)
                        {
                            int hue = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));

                            if (item is DyeTub)
                                ((DyeTub)item).DyedHue = hue;
                            else
                                item.Hue = hue;
                        }
                    }
                    else if (this.m_Params[i].StartsWith("Name"))
                    {
                        int indexOf = this.m_Params[i].IndexOf('=');

                        if (indexOf >= 0)
                            item.Name = this.m_Params[i].Substring(++indexOf);
                    }
                    else if (this.m_Params[i].StartsWith("Amount"))
                    {
                        int indexOf = this.m_Params[i].IndexOf('=');

                        if (indexOf >= 0)
                        {
                            // Must supress stackable warnings
                            bool wasStackable = item.Stackable;

                            item.Stackable = true;
                            item.Amount = Utility.ToInt32(this.m_Params[i].Substring(++indexOf));
                            item.Stackable = wasStackable;
                        }
                    }
                }

                return item;
            }

            private static readonly Queue m_DeleteQueue = new Queue();

            private static bool FindItem(int x, int y, int z, Map map, Item srcItem)
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
                            res = true;
                        else if (Math.Abs(item.Z - z) < 8)
                            m_DeleteQueue.Enqueue(item);
                    }
                }
                else if ((TileData.ItemTable[itemID & TileData.MaxItemValue].Flags & TileFlag.LightSource) != 0)
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
                                if (item.Light != lt)
                                    m_DeleteQueue.Enqueue(item);
                                else
                                    res = true;
                            }
                            else if ((item.ItemData.Flags & TileFlag.LightSource) != 0 && item.ItemData.Name == srcName)
                            {
                                m_DeleteQueue.Enqueue(item);
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
                                m_DeleteQueue.Enqueue(item);
                            else
                                res = true;
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
                            eable.Free();
                            return true;
                        }
                    }
                }

                eable.Free();

                while (m_DeleteQueue.Count > 0)
                    ((Item)m_DeleteQueue.Dequeue()).Delete();

                return res;
            }

            public int Generate(Map[] maps)
            {
                int count = 0;

                Item item = null;

                for (int i = 0; i < this.m_Entries.Count; ++i)
                {
                    DecorationEntry entry = (DecorationEntry)this.m_Entries[i];
                    Point3D loc = entry.Location;
                    string extra = entry.Extra;

                    for (int j = 0; j < maps.Length; ++j)
                    {
                        if (item == null)
                            item = this.Construct();

                        this.m_Constructed = item;

                        if (item == null)
                            continue;

                        if (FindItem(loc.X, loc.Y, loc.Z, maps[j], item))
                        {
                        }
                        else
                        {
                            item.MoveToWorld(loc, maps[j]);
                            ++count;

                            if (item is BaseDoor)
                            {
                                IPooledEnumerable eable = maps[j].GetItemsInRange(loc, 1);

                                Type itemType = item.GetType();

                                foreach (Item link in eable)
                                {
                                    if (link != item && link.Z == item.Z && link.GetType() == itemType)
                                    {
                                        ((BaseDoor)item).Link = (BaseDoor)link;
                                        ((BaseDoor)link).Link = (BaseDoor)item;
                                        break;
                                    }
                                }

                                eable.Free();
                            }
                            else if (item is MarkContainer)
                            {
                                try
                                {
                                    ((MarkContainer)item).Target = Point3D.Parse(extra);
                                }
                                catch
                                {
                                }
                            }

                            item = null;
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

                    for (DecorationList v = Read(ip); v != null; v = Read(ip))
                        list.Add(v);

                    return list;
                }
            }

            private static readonly string[] m_EmptyParams = new string[0];

            public static DecorationList Read(StreamReader ip)
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

                DecorationList list = new DecorationList();

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

                    list.m_Entries.Add(new DecorationEntry(line));
                }

                return list;
            }
        }

        public class DecorationEntry
        {
            private readonly Point3D m_Location;
            private readonly string m_Extra;

            public Point3D Location
            {
                get
                {
                    return this.m_Location;
                }
            }
            public string Extra
            {
                get
                {
                    return this.m_Extra;
                }
            }

            public DecorationEntry(string line)
            {
                string x, y, z;

                this.Pop(out x, ref line);
                this.Pop(out y, ref line);
                this.Pop(out z, ref line);

                this.m_Location = new Point3D(Utility.ToInt32(x), Utility.ToInt32(y), Utility.ToInt32(z));
                this.m_Extra = line;
            }

            public void Pop(out string v, ref string line)
            {
                int space = line.IndexOf(' ');

                if (space >= 0)
                {
                    v = line.Substring(0, space++);
                    line = line.Substring(space);
                }
                else
                {
                    v = line;
                    line = "";
                }
            }
        }
    }

    public class DoorGenerator
    {
        private static readonly Rectangle2D[] m_BritRegions = new Rectangle2D[]
        {
            new Rectangle2D(new Point2D(250, 750), new Point2D(775, 1330)),
            new Rectangle2D(new Point2D(525, 2095), new Point2D(925, 2430)),
            new Rectangle2D(new Point2D(1025, 2155), new Point2D(1265, 2310)),
            new Rectangle2D(new Point2D(1635, 2430), new Point2D(1705, 2508)),
            new Rectangle2D(new Point2D(1775, 2605), new Point2D(2165, 2975)),
            new Rectangle2D(new Point2D(1055, 3520), new Point2D(1570, 4075)),
            new Rectangle2D(new Point2D(2860, 3310), new Point2D(3120, 3630)),
            new Rectangle2D(new Point2D(2470, 1855), new Point2D(3950, 3045)),
            new Rectangle2D(new Point2D(3425, 990), new Point2D(3900, 1455)),
            new Rectangle2D(new Point2D(4175, 735), new Point2D(4840, 1600)),
            new Rectangle2D(new Point2D(2375, 330), new Point2D(3100, 1045)),
            new Rectangle2D(new Point2D(2100, 1090), new Point2D(2310, 1450)),
            new Rectangle2D(new Point2D(1495, 1400), new Point2D(1550, 1475)),
            new Rectangle2D(new Point2D(1085, 1520), new Point2D(1415, 1910)),
            new Rectangle2D(new Point2D(1410, 1500), new Point2D(1745, 1795)),
            new Rectangle2D(new Point2D(5120, 2300), new Point2D(6143, 4095))
        };
        private static readonly Rectangle2D[] m_IlshRegions = new Rectangle2D[]
        {
            new Rectangle2D(new Point2D(0, 0), new Point2D(288 * 8, 200 * 8))
        };
        private static readonly Rectangle2D[] m_MalasRegions = new Rectangle2D[]
        {
            new Rectangle2D(new Point2D(0, 0), new Point2D(320 * 8, 256 * 8))
        };
        private static readonly int[] m_SouthFrames = new int[]
        {
            0x0006,
            0x0008,
            0x000B,
            0x001A,
            0x001B,
            0x001F,
            0x0038,
            0x0057,
            0x0059,
            0x005B,
            0x005D,
            0x0080,
            0x0081,
            0x0082,
            0x0084,
            0x0090,
            0x0091,
            0x0094,
            0x0096,
            0x0099,
            0x00A6,
            0x00A7,
            0x00AA,
            0x00AE,
            0x00B0,
            0x00B3,
            0x00C7,
            0x00C9,
            0x00F8,
            0x00FA,
            0x00FD,
            0x00FE,
            0x0100,
            0x0103,
            0x0104,
            0x0106,
            0x0109,
            0x0127,
            0x0129,
            0x012B,
            0x012D,
            0x012F,
            0x0131,
            0x0132,
            0x0134,
            0x0135,
            0x0137,
            0x0139,
            0x013B,
            0x014C,
            0x014E,
            0x014F,
            0x0151,
            0x0153,
            0x0155,
            0x0157,
            0x0158,
            0x015A,
            0x015D,
            0x015E,
            0x015F,
            0x0162,
            0x01CF,
            0x01D1,
            0x01D4,
            0x01FF,
            0x0204,
            0x0206,
            0x0208,
            0x020A
        };
        private static readonly int[] m_NorthFrames = new int[]
        {
            0x0006,
            0x0008,
            0x000D,
            0x001A,
            0x001B,
            0x0020,
            0x003A,
            0x0057,
            0x0059,
            0x005B,
            0x005D,
            0x0080,
            0x0081,
            0x0082,
            0x0084,
            0x0090,
            0x0091,
            0x0094,
            0x0096,
            0x0099,
            0x00A6,
            0x00A7,
            0x00AC,
            0x00AE,
            0x00B0,
            0x00C7,
            0x00C9,
            0x00F8,
            0x00FA,
            0x00FD,
            0x00FE,
            0x0100,
            0x0103,
            0x0104,
            0x0106,
            0x0109,
            0x0127,
            0x0129,
            0x012B,
            0x012D,
            0x012F,
            0x0131,
            0x0132,
            0x0134,
            0x0135,
            0x0137,
            0x0139,
            0x013B,
            0x014C,
            0x014E,
            0x014F,
            0x0151,
            0x0153,
            0x0155,
            0x0157,
            0x0158,
            0x015A,
            0x015D,
            0x015E,
            0x015F,
            0x0162,
            0x01CF,
            0x01D1,
            0x01D4,
            0x01FF,
            0x0201,
            0x0204,
            0x0208,
            0x020A
        };
        private static readonly int[] m_EastFrames = new int[]
        {
            0x0007,
            0x000A,
            0x001A,
            0x001C,
            0x001E,
            0x0037,
            0x0058,
            0x0059,
            0x005C,
            0x005E,
            0x0080,
            0x0081,
            0x0082,
            0x0084,
            0x0090,
            0x0092,
            0x0095,
            0x0097,
            0x0098,
            0x00A6,
            0x00A8,
            0x00AB,
            0x00AE,
            0x00AF,
            0x00B2,
            0x00C7,
            0x00C8,
            0x00EA,
            0x00F8,
            0x00F9,
            0x00FC,
            0x00FE,
            0x00FF,
            0x0102,
            0x0104,
            0x0105,
            0x0108,
            0x0127,
            0x0128,
            0x012B,
            0x012C,
            0x012E,
            0x0130,
            0x0132,
            0x0133,
            0x0135,
            0x0136,
            0x0138,
            0x013A,
            0x014C,
            0x014D,
            0x014F,
            0x0150,
            0x0152,
            0x0154,
            0x0156,
            0x0158,
            0x0159,
            0x015C,
            0x015E,
            0x0160,
            0x0163,
            0x01CF,
            0x01D0,
            0x01D3,
            0x01FF,
            0x0203,
            0x0205,
            0x0207,
            0x0209
        };
        private static readonly int[] m_WestFrames = new int[]
        {
            0x0007,
            0x000C,
            0x001A,
            0x001C,
            0x0021,
            0x0039,
            0x0058,
            0x0059,
            0x005C,
            0x005E,
            0x0080,
            0x0081,
            0x0082,
            0x0084,
            0x0090,
            0x0092,
            0x0095,
            0x0097,
            0x0098,
            0x00A6,
            0x00A8,
            0x00AD,
            0x00AE,
            0x00AF,
            0x00B5,
            0x00C7,
            0x00C8,
            0x00EA,
            0x00F8,
            0x00F9,
            0x00FC,
            0x00FE,
            0x00FF,
            0x0102,
            0x0104,
            0x0105,
            0x0108,
            0x0127,
            0x0128,
            0x012C,
            0x012E,
            0x0130,
            0x0132,
            0x0133,
            0x0135,
            0x0136,
            0x0138,
            0x013A,
            0x014C,
            0x014D,
            0x014F,
            0x0150,
            0x0152,
            0x0154,
            0x0156,
            0x0158,
            0x0159,
            0x015C,
            0x015E,
            0x0160,
            0x0163,
            0x01CF,
            0x01D0,
            0x01D3,
            0x01FF,
            0x0200,
            0x0203,
            0x0207,
            0x0209
        };
        private static Map m_Map;
        private static int m_Count;
        public static void Initialize()
        {
            CommandSystem.Register("DoorGen", AccessLevel.Administrator, new CommandEventHandler(DoorGen_OnCommand));
        }

        [Usage("DoorGen")]
        [Description("Generates doors by analyzing the map. Slow.")]
        public static void DoorGen_OnCommand(CommandEventArgs e)
        {
            Generate();
        }

        public static void Generate()
        {
            World.Broadcast(0x35, true, "Generating doors, please wait.");

            Network.NetState.FlushAll();
            Network.NetState.Pause();

            m_Map = Map.Trammel;
            m_Count = 0;

            for (int i = 0; i < m_BritRegions.Length; ++i)
                Generate(m_BritRegions[i]);

            int trammelCount = m_Count;

            m_Map = Map.Felucca;
            m_Count = 0;

            for (int i = 0; i < m_BritRegions.Length; ++i)
                Generate(m_BritRegions[i]);

            int feluccaCount = m_Count;

            m_Map = Map.Ilshenar;
            m_Count = 0;

            for (int i = 0; i < m_IlshRegions.Length; ++i)
                Generate(m_IlshRegions[i]);

            int ilshenarCount = m_Count;

            m_Map = Map.Malas;
            m_Count = 0;

            for (int i = 0; i < m_MalasRegions.Length; ++i)
                Generate(m_MalasRegions[i]);

            int malasCount = m_Count;

            Network.NetState.Resume();

            World.Broadcast(0x35, true, "Door generation complete. Trammel: {0}; Felucca: {1}; Ilshenar: {2}; Malas: {3};", trammelCount, feluccaCount, ilshenarCount, malasCount);
        }

        public static bool IsFrame(int id, int[] list)
        {
            if (id > list[list.Length - 1])
                return false;

            for (int i = 0; i < list.Length; ++i)
            {
                int delta = id - list[i];

                if (delta < 0)
                    return false;
                else if (delta == 0)
                    return true;
            }

            return false;
        }

        public static bool IsNorthFrame(int id)
        {
            return IsFrame(id, m_NorthFrames);
        }

        public static bool IsSouthFrame(int id)
        {
            return IsFrame(id, m_SouthFrames);
        }

        public static bool IsWestFrame(int id)
        {
            return IsFrame(id, m_WestFrames);
        }

        public static bool IsEastFrame(int id)
        {
            return IsFrame(id, m_EastFrames);
        }

        public static bool IsEastFrame(int x, int y, int z)
        {
            StaticTile[] tiles = m_Map.Tiles.GetStaticTiles(x, y);

            for (int i = 0; i < tiles.Length; ++i)
            {
                StaticTile tile = tiles[i];

                if (tile.Z == z && IsEastFrame(tile.ID))
                    return true;
            }

            return false;
        }

        public static bool IsSouthFrame(int x, int y, int z)
        {
            StaticTile[] tiles = m_Map.Tiles.GetStaticTiles(x, y);

            for (int i = 0; i < tiles.Length; ++i)
            {
                StaticTile tile = tiles[i];

                if (tile.Z == z && IsSouthFrame(tile.ID))
                    return true;
            }

            return false;
        }

        public static BaseDoor AddDoor(int x, int y, int z, DoorFacing facing)
        {
            int doorZ = z;
            int doorTop = doorZ + 20;

            if (!m_Map.CanFit(x, y, z, 16, false, false))
                return null;

            if (y == 1743 && x >= 1343 && x <= 1344)
                return null;

            if (y == 1679 && x >= 1392 && x <= 1393)
                return null;

            if (x == 1320 && y >= 1618 && y <= 1640)
                return null;

            if (x == 1383 && y >= 1642 && y <= 1643)
                return null;

            BaseDoor door = new DarkWoodDoor(facing);
            door.MoveToWorld(new Point3D(x, y, z), m_Map);

            ++m_Count;

            return door;
        }

        public static void Generate(Rectangle2D region)
        {
            for (int rx = 0; rx < region.Width; ++rx)
            {
                for (int ry = 0; ry < region.Height; ++ry)
                {
                    int vx = rx + region.X;
                    int vy = ry + region.Y;

                    StaticTile[] tiles = m_Map.Tiles.GetStaticTiles(vx, vy);

                    for (int i = 0; i < tiles.Length; ++i)
                    {
                        StaticTile tile = tiles[i];

                        int id = tile.ID;
                        int z = tile.Z;

                        if (IsWestFrame(id))
                        {
                            if (IsEastFrame(vx + 2, vy, z))
                            {
                                AddDoor(vx + 1, vy, z, DoorFacing.WestCW);
                            }
                            else if (IsEastFrame(vx + 3, vy, z))
                            {
                                BaseDoor first = AddDoor(vx + 1, vy, z, DoorFacing.WestCW);
                                BaseDoor second = AddDoor(vx + 2, vy, z, DoorFacing.EastCCW);

                                if (first != null && second != null)
                                {
                                    first.Link = second;
                                    second.Link = first;
                                }
                                else
                                {
                                    if (first != null)
                                        first.Delete();

                                    if (second != null)
                                        second.Delete();
                                }
                            }
                        }
                        else if (IsNorthFrame(id))
                        {
                            if (IsSouthFrame(vx, vy + 2, z))
                            {
                                AddDoor(vx, vy + 1, z, DoorFacing.SouthCW);
                            }
                            else if (IsSouthFrame(vx, vy + 3, z))
                            {
                                BaseDoor first = AddDoor(vx, vy + 1, z, DoorFacing.NorthCCW);
                                BaseDoor second = AddDoor(vx, vy + 2, z, DoorFacing.SouthCW);

                                if (first != null && second != null)
                                {
                                    first.Link = second;
                                    second.Link = first;
                                }
                                else
                                {
                                    if (first != null)
                                        first.Delete();

                                    if (second != null)
                                        second.Delete();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}