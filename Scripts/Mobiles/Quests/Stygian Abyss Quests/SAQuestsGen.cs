using Server;
using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Gumps;
using Server.Engines.Quests;
using Server.QuestConfiguration;
using Server.Engines.Collections;

namespace Server.Items
{
    public static class SAQuestsGen
    {
        public static void Initialize()
        {
            if (QuestConfig.SAQuestGenEnabled)
            {
                Generate();
            }
        }

        public static void Generate()
        {
            // TerMur
            // UnderWorld
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Sutek"), new Point3D(911, 586, -14), Map.TerMur);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "ElderDugan"), new Point3D(1104, 1127, -52), Map.TerMur);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "FiddlingTobin"), new Point3D(1117, 1124, -42), Map.TerMur);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Gretchen"), new Point3D(1088, 1128, -42), Map.TerMur);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Jaacar"), new Point3D(1204, 1041, -42), Map.TerMur);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Neville"), new Point3D(1155, 964, -42), Map.TerMur);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "QuartermasterFlint"), new Point3D(1135, 1132, -42), Map.TerMur);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Vernix"), new Point3D(1015, 976, -27), Map.TerMur);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Xenrr"), new Point3D(1014, 1000, -43), Map.TerMur);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Barreraak"), new Point3D(1189, 989, -27), Map.TerMur);
            // Royal City
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Laifem"), new Point3D(806, 3397, 0), Map.TerMur);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "QueenZhah"), new Point3D(748, 3347, 61), Map.TerMur);
            // Vesper
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Dermott"), new Point3D(2842, 883, 0), Map.Trammel);

            // Quest Decor and Teleporters
            //PutDeco(new BlightedGroveTeleFel(), new Point3D(587, 1638, 0), Map.Felucca);
        }

        public static void PutSpawner(Spawner s, Point3D loc, Map map)
        {
            string name = String.Format("SAQS-{0}", s.Name);

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