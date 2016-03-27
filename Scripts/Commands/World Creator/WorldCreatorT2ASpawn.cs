using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;

namespace Server.Gumps
{
    public class WorldCreatorT2ASpawn : Gump
    {
        public WorldCreatorT2ASpawn(Mobile from)
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
            AddAlphaRegion(120, 50, 567, 480);
            AddLabel(347, 66, 2728, @"World Creator");
            AddLabel(546, 509, 2728, @"@G.O.W, 2016");
            AddImage(295, 56, 52);
            AddImage(121, 50, 5609);
            AddImage(630, 50, 5609);
            AddLabel(340, 96, 2728, @"T2A Spawn");
            AddLabel(314, 127, 2728, @"Select Maps To Spawn");
            AddLabel(138, 148, 2728, @"Dungeons");
            AddCheck(136, 179, 210, 211, true, 1);
            AddCheck(136, 202, 210, 211, true, 2);
            AddCheck(136, 225, 210, 211, true, 3);
            AddCheck(136, 248, 210, 211, true, 4);
            AddCheck(136, 271, 210, 211, true, 5);
            AddCheck(136, 294, 210, 211, true, 6);
            AddCheck(136, 317, 210, 211, true, 7);
            AddCheck(136, 340, 210, 211, true, 8);
            AddCheck(136, 363, 210, 211, true, 9);
            AddCheck(136, 386, 210, 211, true, 10);
            AddCheck(136, 409, 210, 211, true, 11);
            AddCheck(136, 433, 210, 211, true, 12);
            AddLabel(160, 180, 2728, @"Britain Sewer");
            AddLabel(160, 204, 2728, @"Covetous");
            AddLabel(160, 227, 2728, @"Deceit");
            AddLabel(159, 250, 2728, @"Despise");
            AddLabel(160, 273, 2728, @"Destard");
            AddLabel(160, 296, 2728, @"Fire");
            AddLabel(160, 318, 2728, @"Hythloth");
            AddLabel(160, 340, 2728, @"Ice");
            AddLabel(160, 361, 2728, @"Shame");
            AddLabel(160, 386, 2728, @"Terathan Keep");
            AddLabel(160, 409, 2728, @"Trinsic Passage");
            AddLabel(160, 433, 2728, @"Wrong");
            AddLabel(275, 151, 2728, @"Graveyards");
            AddCheck(274, 180, 210, 211, true, 13);
            AddCheck(274, 202, 210, 211, true, 14);
            AddCheck(274, 225, 210, 211, true, 15);
            AddCheck(274, 248, 210, 211, true, 16);
            AddCheck(274, 271, 210, 211, true, 17);
            AddCheck(274, 294, 210, 211, true, 18);
            AddLabel(298, 180, 2728, @"Britain Graveyard");
            AddLabel(298, 204, 2728, @"Cove Graveyard");
            AddLabel(298, 227, 2728, @"Jhelom Graveyard");
            AddLabel(298, 250, 2728, @"Moonglow Graveyard");
            AddLabel(298, 273, 2728, @"Vesper Graveyard");
            AddLabel(298, 296, 2728, @"Yew Graveyard");
            AddLabel(276, 318, 2728, @"World Spawns");
            AddCheck(274, 340, 210, 211, true, 36);
            AddCheck(274, 363, 210, 211, true, 37);
            AddCheck(274, 386, 210, 211, true, 38);
            AddCheck(274, 409, 210, 211, true, 39);
            AddCheck(274, 433, 210, 211, true, 40);
            AddLabel(298, 340, 2728, @"Heavy");
            AddLabel(298, 361, 2728, @"Light");
            AddLabel(298, 386, 2728, @"Lost Lands");
            AddLabel(298, 409, 2728, @"Medium");
            AddLabel(298, 433, 2728, @"Miscellaneous");
            AddLabel(451, 151, 2728, @"Towns");
            AddCheck(451, 180, 210, 211, true, 19);
            AddCheck(452, 202, 210, 211, true, 20);
            AddCheck(452, 225, 210, 211, true, 21);
            AddCheck(452, 248, 210, 211, true, 22);
            AddCheck(452, 271, 210, 211, true, 23);
            AddCheck(452, 294, 210, 211, true, 24);
            AddCheck(452, 317, 210, 211, true, 25);
            AddCheck(452, 340, 210, 211, true, 26);
            AddCheck(452, 363, 210, 211, true, 27);
            AddCheck(452, 386, 210, 211, true, 28);
            AddCheck(452, 409, 210, 211, true, 29);
            AddCheck(452, 433, 210, 211, true, 30);
            AddCheck(582, 180, 210, 211, true, 31);
            AddCheck(582, 202, 210, 211, true, 32);
            AddCheck(582, 225, 210, 211, true, 33);
            AddCheck(582, 248, 210, 211, true, 34);
            AddCheck(582, 271, 210, 211, true, 35);
            AddLabel(476, 180, 2728, @"Britain");
            AddLabel(478, 204, 2728, @"Buccaneers Den");
            AddLabel(478, 227, 2728, @"Cove");
            AddLabel(478, 250, 2728, @"Delucia");
            AddLabel(478, 273, 2728, @"Jhelom");
            AddLabel(478, 296, 2728, @"Magincia");
            AddLabel(478, 318, 2728, @"Minoc");
            AddLabel(478, 340, 2728, @"Moonglow");
            AddLabel(478, 361, 2728, @"Nujel'm");
            AddLabel(478, 386, 2728, @"Ocllo");
            AddLabel(478, 409, 2728, @"Papua");
            AddLabel(478, 433, 2728, @"Serpents Hold");
            AddLabel(608, 180, 2728, @"Skara Brae");
            AddLabel(608, 204, 2728, @"Trinsic");
            AddLabel(608, 227, 2728, @"Vesper");
            AddLabel(608, 250, 2728, @"Wind");
            AddLabel(608, 273, 2728, @"Yew");
            AddButton(125, 501, 240, 239, 1, GumpButtonType.Reply, 0);
        }

        public static void SpawnThis(Mobile from, List<int> ListSwitches, int switche, int map, string mapfile)
        {
            string folder = "";

            if (map == 1)
                folder = "T2A";

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
                default:
                    {
                        // Make sure that the APPLY button was pressed
                        if (info.ButtonID == 1)
                        {
                            // Get the array of switches selected
                            List<int> Selections = new List<int>(info.Switches);

                            from.Say("SPAWNING T2A Felucca...");
                            SpawnThis(from, Selections, 1, 1, "Dungeons/BritainSewer");
                            SpawnThis(from, Selections, 2, 1, "Dungeons/Covetous");
                            SpawnThis(from, Selections, 3, 1, "Dungeons/Deceit");
                            SpawnThis(from, Selections, 4, 1, "Dungeons/Despise");
                            SpawnThis(from, Selections, 5, 1, "Dungeons/Destard");
                            SpawnThis(from, Selections, 6, 1, "Dungeons/Fire");
                            SpawnThis(from, Selections, 7, 1, "Dungeons/Hythloth");
                            SpawnThis(from, Selections, 8, 1, "Dungeons/Ice");
                            SpawnThis(from, Selections, 9, 1, "Dungeons/Shame");
                            SpawnThis(from, Selections, 10, 1, "Dungeons/TerathanKeep");
                            SpawnThis(from, Selections, 11, 1, "Dungeons/TrinsicPassage");
                            SpawnThis(from, Selections, 12, 1, "Dungeons/Wrong");
                            SpawnThis(from, Selections, 13, 1, "Graveyards/BritainGraveyard");
                            SpawnThis(from, Selections, 14, 1, "Graveyards/CoveGraveyard");
                            SpawnThis(from, Selections, 15, 1, "Graveyards/JhelomGraveyard");
                            SpawnThis(from, Selections, 16, 1, "Graveyards/MoonglowGraveyard");
                            SpawnThis(from, Selections, 17, 1, "Graveyards/VesperGraveyard");
                            SpawnThis(from, Selections, 18, 1, "Graveyards/YewGraveyard");
                            SpawnThis(from, Selections, 19, 1, "Towns/Britain");
                            SpawnThis(from, Selections, 20, 1, "Towns/BuccaneersDen");
                            SpawnThis(from, Selections, 21, 1, "Towns/Cove");
                            SpawnThis(from, Selections, 22, 1, "Towns/Delucia");
                            SpawnThis(from, Selections, 23, 1, "Towns/Jhelom");
                            SpawnThis(from, Selections, 24, 1, "Towns/Magincia");
                            SpawnThis(from, Selections, 25, 1, "Towns/Minoc");
                            SpawnThis(from, Selections, 26, 1, "Towns/Moonglow");
                            SpawnThis(from, Selections, 27, 1, "Towns/Nujel'm");
                            SpawnThis(from, Selections, 28, 1, "Towns/Ocllo");
                            SpawnThis(from, Selections, 29, 1, "Towns/Papua");
                            SpawnThis(from, Selections, 30, 1, "Towns/SerpentsHold");
                            SpawnThis(from, Selections, 31, 1, "Towns/SkaraBrae");
                            SpawnThis(from, Selections, 32, 1, "Towns/Trinsic");
                            SpawnThis(from, Selections, 33, 1, "Towns/Vesper");
                            SpawnThis(from, Selections, 34, 1, "Towns/Wind");
                            SpawnThis(from, Selections, 35, 1, "Towns/Yew");
                            SpawnThis(from, Selections, 36, 1, "Heavy");
                            SpawnThis(from, Selections, 37, 1, "Light");
                            SpawnThis(from, Selections, 38, 1, "LostLands");
                            SpawnThis(from, Selections, 39, 1, "Medium");
                            SpawnThis(from, Selections, 40, 1, "Miscellaneous");
                            from.Say("SPAWN GENERATION COMPLETED");
                        }
                        break;
                    }

            }
        }
    }
}