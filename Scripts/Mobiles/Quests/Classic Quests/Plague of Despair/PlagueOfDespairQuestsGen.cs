using Server;
using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Gumps;
using Server.Engines.Quests;
using Server.QuestConfiguration;
using Server.Engines.Collections;
using Server.Engines.Quests.Necro;
using System.IO;
using System.Collections;

namespace Server.Items
{
    public static class PlagueOfDespairQuestsGen
    {
        public static void Initialize()
        {
            if (QuestConfig.PlagueOfDespairQuestGenEnabled)
            {
                Generate("Data/World/Decoration/ClassicQuests/PlagueOfDespair/Britannia", Map.Trammel, Map.Felucca);
                Generate("Data/World/Decoration/ClassicQuests/PlagueOfDespair/Trammel", Map.Trammel);
            }
        }
        private static ArrayList m_List;
        private static int m_Count;

        public static void Generate(string folder, params Map[] maps)
        {
            if (!Directory.Exists(folder))
            {
                return;
            }

            string[] files = Directory.GetFiles(folder, "*.cfg");

            for (int i = 0; i < files.Length; ++i)
            {
                ArrayList list = WorldCreatorDecor.DecorationList.ReadAll(files[i]);

                m_List = list;

                for (int j = 0; j < list.Count; ++j)
                {
                    m_Count += ((WorldCreatorDecor.DecorationList)list[j]).Generate(maps);
                }
            }
        }
    }
}