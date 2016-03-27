using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;

namespace Server.Gumps
{
    public class WorldCreatorMalasSpawn : Gump
    {
        public WorldCreatorMalasSpawn(Mobile from)
            : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(108, 36, 600, 509, 9200);
            //AddImageTiled(122, 50, 568, 479, 2624);
            AddAlphaRegion(120, 49, 567, 480);
            AddLabel(347, 66, 2728, @"World Creator");
            AddLabel(546, 509, 2728, @"@GOW, 2016");
            AddLabel(314, 123, 2728, @"Select Maps To Spawn");
            AddLabel(340, 95, 2728, @"Malas Spawn");
            AddImage(295, 56, 52);
            AddImage(121, 50, 5609);
            AddImage(630, 50, 5609);
            AddCheck(136, 158, 210, 211, true, 1);
            AddCheck(136, 180, 210, 211, true, 2);
            AddCheck(136, 202, 210, 211, true, 3);
            AddCheck(136, 225, 210, 211, true, 4);
            AddCheck(136, 248, 210, 211, true, 5);
            AddCheck(136, 271, 210, 211, true, 6);
            AddCheck(136, 294, 210, 211, true, 7);
            AddCheck(136, 317, 210, 211, true, 8);
            AddLabel(158, 158, 2728, @"Bedlam");
            AddLabel(160, 180, 2728, @"Citadel");
            AddLabel(160, 204, 2728, @"Doom");
            AddLabel(160, 227, 2728, @"Labyrinth");
            AddLabel(159, 249, 2728, @"North");
            AddLabel(160, 273, 2728, @"OrcForts");
            AddLabel(160, 296, 2728, @"South");
            AddLabel(160, 317, 2728, @"Vendors");
            AddButton(125, 501, 240, 239, 1, GumpButtonType.Reply, 0);
        }

        public static void SpawnThis(Mobile from, List<int> ListSwitches, int switche, int map, string mapfile)
        {
            string folder = "";

            if (map == 4)
                folder = "Malas";

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

                            from.Say("SPAWNING Ilshenar...");
                            SpawnThis(from, Selections, 1, 4, "Bedlam");
                            SpawnThis(from, Selections, 2, 4, "Citadel");
                            SpawnThis(from, Selections, 3, 4, "Doom");
                            SpawnThis(from, Selections, 4, 4, "Labyrinth");
                            SpawnThis(from, Selections, 5, 4, "North");
                            SpawnThis(from, Selections, 6, 4, "OrcForts");
                            SpawnThis(from, Selections, 7, 4, "South");
                            SpawnThis(from, Selections, 8, 4, "Vendors");
                            from.Say("SPAWN GENERATION COMPLETED");
                        }
                        break;
                    }

            }
        }
    }
}