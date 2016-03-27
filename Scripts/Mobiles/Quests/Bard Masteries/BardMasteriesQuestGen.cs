using Server;
using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Gumps;
using Server.Engines.Quests;
using Server.QuestConfiguration;

namespace Server.Items
{
    public static class BardMasteriesQuestGen
    {
        public static void Initialize()
        {
            if (QuestConfig.BardMasteriesQuestGenEnabled)
            {
                Generate();
            }
        }

        public static void Generate()
        {
            //Felucca
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "SirBerran"), new Point3D(1460, 1565, 30), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "SirFalean"), new Point3D(1466, 1551, 30), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "SirHareus"), new Point3D(1466, 1560, 30), Map.Felucca);
            //Trammel
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "SirBerran"), new Point3D(1460, 1565, 30), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "SirFalean"), new Point3D(1466, 1551, 30), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "SirHareus"), new Point3D(1466, 1560, 30), Map.Trammel);
        }

        public static void PutSpawner(Spawner s, Point3D loc, Map map)
        {
            string name = String.Format("BMQS-{0}", s.Name);

            // Auto cleanup on regeneration
            List<Item> toDelete = new List<Item>();

            foreach (Item item in map.GetItemsInRange(loc, 0))
            {
                if (item is Spawner && item.Name == name)
                {
                    toDelete.Add(item);
                }
            }

            foreach (Item item in toDelete)
            {
                item.Delete();
            }

            s.Name = name;
            s.MoveToWorld(loc, map);
        }

        public static void PutDeco(Item deco, Point3D loc, Map map)
        {
            // Auto cleanup on regeneration
            List<Item> toDelete = new List<Item>();

            foreach (Item item in map.GetItemsInRange(loc, 0))
            {
                if (item.ItemID == deco.ItemID && item.Z == loc.Z)
                {
                    toDelete.Add(item);
                }
            }

            foreach (Item item in toDelete)
            {
                item.Delete();
            }

            deco.MoveToWorld(loc, map);
        }
    }
}