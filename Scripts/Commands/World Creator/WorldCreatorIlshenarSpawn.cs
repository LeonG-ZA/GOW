using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;

namespace Server.Gumps
{
    public class WorldCreatorIlshenarSpawn : Gump
    {
        public WorldCreatorIlshenarSpawn(Mobile from)
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
            AddLabel(314, 123, 2728, @"Select Maps To Spawn");
            AddLabel(340, 95, 2728, @"Ilshenar Spawn");
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
            AddCheck(136, 340, 210, 211, true, 9);
            AddCheck(136, 363, 210, 211, true, 10);
            AddCheck(136, 386, 210, 211, true, 11);
            AddCheck(136, 409, 210, 211, true, 12);
            AddCheck(136, 433, 210, 211, true, 13);
            AddCheck(136, 456, 210, 211, true, 14);
            AddLabel(158, 158, 2728, @"Ancientlair");
            AddLabel(160, 180, 2728, @"Ankh");
            AddLabel(160, 204, 2728, @"Blood");
            AddLabel(160, 227, 2728, @"Exodus");
            AddLabel(159, 249, 2728, @"Mushroom");
            AddLabel(160, 273, 2728, @"Outdoors");
            AddLabel(160, 296, 2728, @"Ratmancave");
            AddLabel(160, 317, 2728, @"Rock");
            AddLabel(160, 340, 2728, @"Sorcerers");
            AddLabel(159, 362, 2728, @"Spectre");
            AddLabel(159, 386, 2728, @"Towns");
            AddLabel(159, 409, 2728, @"TwistedWeald");
            AddLabel(159, 434, 2728, @"Vendors");
            AddLabel(159, 456, 2728, @"Wisp");
            AddButton(125, 501, 240, 239, 1, GumpButtonType.Reply, 0);
        }

        public static void SpawnThis(Mobile from, List<int> ListSwitches, int switche, int map, string mapfile)
        {
            string folder = "";

            if (map == 3)
                folder = "Ilshenar";

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
                            SpawnThis(from, Selections, 1, 3, "Ancientlair");
                            SpawnThis(from, Selections, 2, 3, "Ankh");
                            SpawnThis(from, Selections, 3, 3, "Blood");
                            SpawnThis(from, Selections, 4, 3, "Exodus");
                            SpawnThis(from, Selections, 5, 3, "Mushroom");
                            SpawnThis(from, Selections, 6, 3, "Outdoors");
                            SpawnThis(from, Selections, 7, 3, "Ratmancave");
                            SpawnThis(from, Selections, 8, 3, "Rock");
                            SpawnThis(from, Selections, 9, 3, "Sorcerers");
                            SpawnThis(from, Selections, 10, 3, "Spectre");
                            SpawnThis(from, Selections, 11, 3, "Towns");
                            SpawnThis(from, Selections, 12, 3, "TwistedWeald");
                            SpawnThis(from, Selections, 13, 3, "Vendors");
                            SpawnThis(from, Selections, 14, 3, "Wisp");
                            from.Say("SPAWN GENERATION COMPLETED");
                        }
                        break;
                    }

            }
        }
    }
}