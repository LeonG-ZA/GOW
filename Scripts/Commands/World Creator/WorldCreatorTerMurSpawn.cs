using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;

namespace Server.Gumps
{
    public class WorldCreatorTerMurSpawn : Gump
    {
        public WorldCreatorTerMurSpawn(Mobile from)
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
            AddLabel(340, 96, 2728, @"TerMur Spawn");
            AddLabel(314, 123, 2728, @"Select Maps To Spawn");
            AddImage(295, 56, 52);
            AddImage(121, 50, 5609);
            AddImage(630, 50, 5609);
            AddCheck(136, 158, 210, 211, true, 1);
            AddCheck(136, 180, 210, 211, true, 2);
            AddCheck(136, 202, 210, 211, true, 3);
            AddCheck(136, 225, 210, 211, true, 4);
            AddLabel(158, 158, 2728, @"Abyss");
            AddLabel(158, 180, 2728, @"TerMur");
            AddLabel(158, 204, 2728, @"Underworld");
            AddLabel(158, 227, 2728, @"Vendors");
            AddButton(125, 501, 240, 239, 0, GumpButtonType.Reply, 0);
        }

        public static void SpawnThis(Mobile from, List<int> ListSwitches, int switche, int map, string mapfile)
        {
            string folder = "";

            if (map == 6)
                folder = "TerMur";

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

                            from.Say("SPAWNING TERMUR...");
                            SpawnThis(from, Selections, 1, 6, "FanDancersDojo");
                            SpawnThis(from, Selections, 2, 6, "Outdoors");
                            SpawnThis(from, Selections, 3, 6, "TownsLife");
                            SpawnThis(from, Selections, 4, 6, "Vendors");
                            from.Say("SPAWN GENERATION COMPLETED");
                        }
                        break;
                    }

            }
        }
    }
}