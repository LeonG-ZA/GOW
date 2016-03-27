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
    public static class ClassicStartingQuestsQuestsGen
    {
        public static void Initialize()
        {
            if (QuestConfig.ClassicStartingQuestGenEnabled)
            {
                Generate("Data/World/Decoration/ClassicQuests/ClassicStartingQuests/Malas", Map.Malas);
                Generate("Data/World/Decoration/ClassicQuests/ClassicStartingQuests/Tokuno", Map.Tokuno);
                Generate("Data/World/Decoration/ClassicQuests/ClassicStartingQuests/Trammel", Map.Trammel);
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
                ArrayList list = Server.Gumps.WorldCreatorDecor.DecorationList.ReadAll(files[i]);

                m_List = list;

                for (int j = 0; j < list.Count; ++j)
                {
                    m_Count += ((Server.Gumps.WorldCreatorDecor.DecorationList)list[j]).Generate(maps);
                }
            }
        }
    }
}