using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;

namespace Server.Gumps
{
    public class WorldCreatorFelSpawn : Gump
    {
        public WorldCreatorFelSpawn(Mobile from)
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
            AddBackground(108, 36, 600, 509, 9200);
            if (!ns.IsKRClient)
            {
                AddImageTiled(122, 50, 568, 479, 2624);
            }
            AddAlphaRegion(120, 49, 567, 480);
            AddLabel(347, 66, 2728, @"World Creator");
            AddLabel(546, 509, 2728, @"@G.O.W, 2016");
            AddLabel(340, 96, 2728, @"Felucca Spawn");
            AddLabel(314, 123, 2728, @"Select Maps To Spawn");
            AddImage(295, 56, 52);
            AddImage(121, 50, 5609);
            AddImage(630, 50, 5609);
            AddCheck(136, 158, 210, 211, true, 1);
            AddCheck(136, 180, 210, 211, true, 2);
            if (!Core.HS)
            {
                AddCheck(136, 202, 210, 211, true, 3);
            }
            AddCheck(136, 225, 210, 211, true, 4);
            AddCheck(136, 248, 210, 211, true, 5);
            AddCheck(136, 271, 210, 211, true, 6);
            AddCheck(136, 294, 210, 211, true, 7);
            AddCheck(136, 317, 210, 211, true, 8);
            AddCheck(136, 340, 210, 211, true, 9);
            AddCheck(136, 363, 210, 211, true, 10);
            AddCheck(136, 386, 210, 211, true, 1);
            AddCheck(136, 409, 210, 211, true, 12);
            AddCheck(136, 433, 210, 211, true, 13);
            AddCheck(136, 456, 210, 211, true, 14);
            AddCheck(136, 479, 210, 211, true, 15);
            if (!Core.HS)
            {
                AddCheck(274, 158, 210, 211, true, 16);
            }
            AddCheck(274, 180, 210, 211, true, 17);
            AddCheck(274, 202, 210, 211, true, 18);
            AddCheck(274, 225, 210, 211, true, 19);
            if (!Core.HS)
            {
                AddCheck(274, 248, 210, 211, true, 20);
            }
            AddLabel(140, 132, 2728, @"Dungeons");
            AddLabel(158, 158, 2728, @"Blighted Grove");
            AddLabel(160, 180, 2728, @"Britain Sewer");
            if (!Core.HS)
            {
                AddLabel(160, 204, 2728, @"Covetous");
            }
            AddLabel(160, 227, 2728, @"Deceit");
            AddLabel(159, 250, 2728, @"Despise");
            AddLabel(160, 273, 2728, @"Destard");
            AddLabel(160, 296, 2728, @"Fire");
            AddLabel(160, 318, 2728, @"Hythloth");
            AddLabel(160, 340, 2728, @"Ice");
            AddLabel(159, 362, 2728, @"Khaldun");
            AddLabel(159, 386, 2728, @"Orc Caves");
            AddLabel(159, 409, 2728, @"Painted Caves");
            AddLabel(159, 434, 2728, @"Palace Of Paroxy");
            AddLabel(159, 456, 2728, @"Prism Of Light");
            AddLabel(159, 478, 2728, @"Sanctuary");
            if (!Core.HS)
            {
                AddLabel(296, 158, 2728, @"Shame");
            }
            AddLabel(295, 178, 2728, @"Solen Hive");
            AddLabel(297, 201, 2728, @"Terathan Keep");
            AddLabel(296, 223, 2728, @"Trinsic Passage");
            if (!Core.HS)// Note add New Wrong
            {
                AddLabel(296, 248, 2728, @"Wrong");
            }
            AddCheck(274, 294, 210, 211, true, 21);
            AddCheck(274, 317, 210, 211, true, 22);
            AddCheck(274, 340, 210, 211, true, 23);
            AddCheck(274, 363, 210, 211, true, 24);
            AddCheck(274, 386, 210, 211, true, 25);
            AddCheck(274, 409, 210, 211, true, 26);
            //AddCheck(274, 433, 210, 211, true, 27);
            AddLabel(276, 271, 2728, @"Graveyards");
            AddLabel(296, 295, 2728, @"Britain Graveyard");
            AddLabel(296, 317, 2728, @"Cove Graveyard");
            AddLabel(296, 340, 2728, @"Jhelom Graveyard");
            AddLabel(296, 365, 2728, @"Moonglow Graveyard");
            AddLabel(296, 388, 2728, @"Vesper Graveyard");
            AddLabel(296, 410, 2728, @"Yew Graveyard");
            //AddLabel(296, 432, 2728, @"Haven Graveyard");
            //AddCheck(274, 479, 210, 211, true, 28);
            //AddCheck(452, 158, 210, 211, true, 29);
           // AddCheck(452, 180, 210, 211, true, 30);
            //AddCheck(452, 202, 210, 211, true, 31);
            //AddCheck(452, 225, 210, 211, true, 32);
            //AddLabel(277, 457, 2728, @"World Spawns");
            //AddLabel(296, 478, 2728, @"Heavy");
            //AddLabel(474, 157, 2728, @"Light");
            //AddLabel(473, 181, 2728, @"Lost Lands");
            //AddLabel(474, 203, 2728, @"Medium");
           // AddLabel(474, 224, 2728, @"Miscellaneous");
            AddButton(449, 163, 4005, 4007, 2, GumpButtonType.Reply, 0);
            AddLabel(483, 164, 2728, @"World Spawns");
            AddCheck(452, 271, 210, 211, true, 33);
            AddCheck(452, 294, 210, 211, true, 34);
            AddCheck(452, 317, 210, 211, true, 35);
            AddCheck(452, 340, 210, 211, true, 36);
            AddCheck(452, 363, 210, 211, true, 37);
            AddCheck(452, 386, 210, 211, true, 38);
            AddCheck(452, 409, 210, 211, true, 39);
            AddCheck(452, 433, 210, 211, true, 40);
            AddCheck(452, 457, 210, 211, true, 41);
            AddCheck(452, 480, 210, 211, true, 42);
            AddCheck(582, 158, 210, 211, true, 43);
            AddCheck(582, 180, 210, 211, true, 44);
            AddCheck(582, 202, 210, 211, true, 45);
            AddCheck(582, 225, 210, 211, true, 46);
            AddCheck(582, 248, 210, 211, true, 47);
            AddCheck(582, 271, 210, 211, true, 48);
            AddCheck(582, 293, 210, 211, true, 49);
            AddCheck(582, 317, 210, 211, true, 50);
            AddCheck(582, 340, 210, 211, true, 51);
            AddLabel(454, 249, 2728, @"Towns");
            AddLabel(475, 272, 2728, @"Britain");
            AddLabel(475, 294, 2728, @"Buccaneers Den");
            AddLabel(475, 319, 2728, @"Cove");
            AddLabel(474, 341, 2728, @"Delucia");
            AddLabel(474, 364, 2728, @"Jhelom");
            AddLabel(474, 385, 2728, @"Magincia");
            AddLabel(474, 409, 2728, @"Minoc");
            AddLabel(475, 432, 2728, @"Moonglow");
            AddLabel(474, 457, 2728, @"Nujel'm");
            AddLabel(473, 479, 2728, @"Serpents Hold");
            AddLabel(605, 159, 2728, @"Ocllo");
            AddLabel(605, 181, 2728, @"Papua");
            AddLabel(604, 203, 2728, @"Skara Brae");
            AddLabel(605, 272, 2728, @"Wind");
            AddLabel(605, 226, 2728, @"Trinsic");
            AddLabel(604, 249, 2728, @"Vesper");
            AddLabel(606, 292, 2728, @"Yew");
            AddLabel(605, 317, 2728, @"Heartwood");
            AddLabel(605, 341, 2728, @"Sea Market");
            AddButton(125, 501, 240, 239, 1, GumpButtonType.Reply, 0);
        }

        public static void SpawnThis(Mobile from, List<int> ListSwitches, int switche, int map, string mapfile)
        {
            string folder = "";

            if (map == 1)
                folder = "Felucca";

            string prefix = Server.Commands.CommandSystem.Prefix;

            if (ListSwitches.Contains(switche) == true)
                CommandSystem.Handle(from, String.Format("{0}Spawngen {1}/{2}.map", prefix, folder, mapfile));
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0: //Closed or Cancel
                    {
                        from.SendGump(new WorldCreator(from));
                        break;
                    }
                case 2:
                    {
                        from.CloseGump(typeof(WorldCreatorFelSpawn));
                        from.SendGump(new WorldCreatorFelWorldSpawn(from));
                        break;
                    }
                default:
                    {
                        // Make sure that the APPLY button was pressed
                        if (info.ButtonID == 1)
                        {
                            // Get the array of switches selected
                            List<int> Selections = new List<int>(info.Switches);

                            SpawnThis(from, Selections, 1, 1, "Dungeons/BlightedGrove");
                            SpawnThis(from, Selections, 2, 1, "Dungeons/BritainSewer");
                            SpawnThis(from, Selections, 3, 1, "Dungeons/Covetous");
                            SpawnThis(from, Selections, 4, 1, "Dungeons/Deceit");
                            SpawnThis(from, Selections, 5, 1, "Dungeons/Despise");
                            SpawnThis(from, Selections, 6, 1, "Dungeons/Destard");
                            SpawnThis(from, Selections, 7, 1, "Dungeons/Fire");
                            SpawnThis(from, Selections, 8, 1, "Dungeons/Hythloth");
                            SpawnThis(from, Selections, 9, 1, "Dungeons/Ice");
                            SpawnThis(from, Selections, 10, 1, "Dungeons/Khaldun");
                            SpawnThis(from, Selections, 11, 1, "Dungeons/OrcCaves");
                            SpawnThis(from, Selections, 12, 1, "Dungeons/PaintedCaves");
                            SpawnThis(from, Selections, 13, 1, "Dungeons/PalaceOfParoxysmus");
                            SpawnThis(from, Selections, 14, 1, "Dungeons/PrismOfLight");
                            SpawnThis(from, Selections, 15, 1, "Dungeons/Sanctuary");
                            SpawnThis(from, Selections, 16, 1, "Dungeons/Shame");
                            SpawnThis(from, Selections, 17, 1, "Dungeons/SolenHive");
                            SpawnThis(from, Selections, 18, 1, "Dungeons/TerathanKeep");
                            SpawnThis(from, Selections, 19, 1, "Dungeons/TrinsicPassage");
                            SpawnThis(from, Selections, 20, 1, "Dungeons/Wrong");
                            SpawnThis(from, Selections, 21, 1, "Graveyards/BritainGraveyard");
                            SpawnThis(from, Selections, 22, 1, "Graveyards/CoveGraveyard");
                            SpawnThis(from, Selections, 23, 1, "Graveyards/JhelomGraveyard");
                            SpawnThis(from, Selections, 24, 1, "Graveyards/MoonglowGraveyard");
                            SpawnThis(from, Selections, 25, 1, "Graveyards/VesperGraveyard");
                            SpawnThis(from, Selections, 26, 1, "Graveyards/YewGraveyard");
                            SpawnThis(from, Selections, 27, 1, "Graveyards/HavenGraveyard");
                            SpawnThis(from, Selections, 28, 1, "Heavy");
                            SpawnThis(from, Selections, 29, 1, "Light");
                            SpawnThis(from, Selections, 30, 1, "LostLands");
                            SpawnThis(from, Selections, 31, 1, "Medium");
                            SpawnThis(from, Selections, 32, 1, "Miscellaneous");
                            SpawnThis(from, Selections, 33, 1, "Towns/Britain");
                            SpawnThis(from, Selections, 34, 1, "Towns/BuccaneersDen");
                            SpawnThis(from, Selections, 35, 1, "Towns/Cove");
                            SpawnThis(from, Selections, 36, 1, "Towns/Delucia");
                            SpawnThis(from, Selections, 37, 1, "Towns/Jhelom");
                            SpawnThis(from, Selections, 38, 1, "Towns/Magincia");
                            SpawnThis(from, Selections, 39, 1, "Towns/Minoc");
                            SpawnThis(from, Selections, 40, 1, "Towns/Moonglow");
                            SpawnThis(from, Selections, 41, 1, "Towns/Nujel'm");
                            SpawnThis(from, Selections, 42, 1, "Towns/SerpentsHold");
                            SpawnThis(from, Selections, 43, 1, "Towns/Ocllo");
                            SpawnThis(from, Selections, 44, 1, "Towns/Papua");
                            SpawnThis(from, Selections, 45, 1, "Towns/SkaraBrae");
                            SpawnThis(from, Selections, 46, 1, "Towns/Trinsic");
                            SpawnThis(from, Selections, 47, 1, "Towns/Vesper");
                            SpawnThis(from, Selections, 48, 1, "Towns/Wind");
                            SpawnThis(from, Selections, 49, 1, "Towns/Yew");
                            SpawnThis(from, Selections, 50, 1, "Towns/Heartwood");
                            SpawnThis(from, Selections, 51, 1, "Towns/SeaMarket");
                            SpawnThis(from, Selections, 100, 1, "World/BogofDesolation");
                            SpawnThis(from, Selections, 101, 1, "World/BrigandCamp");
                            SpawnThis(from, Selections, 102, 1, "World/BritainCrossroads");
                            SpawnThis(from, Selections, 103, 1, "World/CoveOrcFort");
                            SpawnThis(from, Selections, 105, 1, "World/DestardSwamps");
                            from.Say("SPAWN GENERATION COMPLETED");
                        }
                        break;
                    }

            }
        }
    }
}