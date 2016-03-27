using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;

namespace Server.Gumps
{
    public class WorldCreatorFelWorldSpawn : Gump
    {
        public WorldCreatorFelWorldSpawn(Mobile from)
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
            AddLabel(340, 96, 2728, @"World Spawns");
            AddLabel(314, 123, 2728, @"Select Maps To Spawn");
            AddImage(295, 56, 52);
            AddImage(121, 50, 5609);
            AddImage(630, 50, 5609);
            AddCheck(136, 157, 210, 211, true, 1);
            AddCheck(136, 179, 210, 211, true, 2);
            AddCheck(136, 201, 210, 211, true, 3);
            AddCheck(136, 224, 210, 211, true, 4);
            AddCheck(136, 247, 210, 211, true, 5);
            AddCheck(136, 271, 210, 211, true, 6);
            AddCheck(136, 294, 210, 211, true, 7);
            AddCheck(136, 317, 210, 211, true, 8);
            AddCheck(136, 340, 210, 211, true, 9);
            AddCheck(136, 363, 210, 211, true, 10);
            AddCheck(136, 386, 210, 211, true, 11);
            AddCheck(136, 409, 210, 211, true, 12);
            AddCheck(136, 433, 210, 211, true, 13);
            AddCheck(136, 456, 210, 211, true, 14);
            AddCheck(136, 479, 210, 211, true, 15);
            AddLabel(158, 157, 2728, @"Bog of Desolation");
            AddLabel(160, 179, 2728, @"Brigand Camp");
            AddLabel(160, 203, 2728, @"Britain Crossroads");
            AddLabel(160, 226, 2728, @"Cove Orc Fort");
            AddLabel(159, 249, 2728, @"Desert of Compassion");
            AddLabel(160, 273, 2728, @"Destard Swamps");
            AddLabel(160, 296, 2728, @"Fens of the Dead");
            AddLabel(160, 318, 2728, @"Fire Temple");
            AddLabel(159, 339, 2728, @"Mountains of Avarice");
            AddLabel(159, 362, 2728, @"Rat Valley");
            AddLabel(159, 386, 2728, @"Rock Ridge");
            AddLabel(159, 409, 2728, @"Trinsic Jungles");
            AddLabel(159, 431, 2728, @"Trinsic Jungles Altar Ruin");
            AddLabel(159, 456, 2728, @"Trinsic Jungles Cemetery");
            AddLabel(159, 477, 2728, @"Trinsic Jungles Ruin");
            AddCheck(301, 160, 210, 211, true, 16);
            AddCheck(301, 181, 210, 211, true, 17);
            AddCheck(301, 203, 210, 211, true, 18);
            AddCheck(301, 224, 210, 211, true, 19);
            AddCheck(301, 245, 210, 211, true, 20);
            AddCheck(301, 266, 210, 211, true, 21);
            AddLabel(322, 160, 2728, @"Windemere Woods");
            AddLabel(323, 178, 2728, @"Wind Entrance");
            AddLabel(322, 200, 2728, @"Wisp Circle");
            AddLabel(321, 221, 2728, @"Yew Crypts");
            AddLabel(322, 243, 2728, @"Yew Lich Ruin");
            AddLabel(323, 265, 2728, @"Yew Orc Fort");
            AddCheck(301, 287, 210, 211, true, 22);
            AddCheck(301, 308, 210, 211, true, 23);
            AddCheck(301, 329, 210, 211, true, 24);
            AddCheck(301, 350, 210, 211, true, 25);
            AddCheck(301, 371, 210, 211, true, 26);
            AddLabel(323, 287, 2728, @"Light");
            AddLabel(322, 308, 2728, @"Medium");
            AddLabel(323, 329, 2728, @"Heavy");
            AddLabel(324, 350, 2728, @"Miscellaneous");
            AddLabel(323, 371, 2728, @"LostLands");
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
                default:
                    {
                        // Make sure that the APPLY button was pressed
                        if (info.ButtonID == 1)
                        {
                            // Get the array of switches selected
                            List<int> Selections = new List<int>(info.Switches);

                            SpawnThis(from, Selections, 1, 1, "World/BogofDesolation");
                            SpawnThis(from, Selections, 2, 1, "World/BrigandCamp");
                            SpawnThis(from, Selections, 3, 1, "World/BritainCrossroads");
                            SpawnThis(from, Selections, 4, 1, "World/CoveOrcFort");
                            SpawnThis(from, Selections, 5, 1, "World/DestardSwamps");
                            SpawnThis(from, Selections, 6, 1, "World/FensoftheDead");
                            SpawnThis(from, Selections, 7, 1, "World/FireTemple");
                            SpawnThis(from, Selections, 8, 1, "World/MountainsofAvarice");
                            SpawnThis(from, Selections, 9, 1, "World/RatValley");
                            SpawnThis(from, Selections, 10, 1, "World/RockRidge");
                            SpawnThis(from, Selections, 22, 1, "World/Light");
                            SpawnThis(from, Selections, 23, 1, "World/Medium");
                            SpawnThis(from, Selections, 24, 1, "World/Heavy");
                            SpawnThis(from, Selections, 25, 1, "World/Miscellaneous");
                            SpawnThis(from, Selections, 26, 1, "World/LostLands");
                            from.Say("SPAWN GENERATION COMPLETED");
                        }
                        break;
                    }

            }
        }
    }
}