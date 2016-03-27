using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Regions;
using Server.Targeting;
using Server.Gumps;

namespace Server.Engines.Quests
{
    public class ExtendedQuestHelper
    {
        public static bool CheckDestroyObject(PlayerMobile player, Item item)
        {
            for (int i = player.Quests.Count - 1; i >= 0; i --)
            {
                BaseQuest quest = player.Quests[i];
				
                for (int j = quest.Objectives.Count - 1; j >= 0; j --)
                {
                    BaseObjective objective = quest.Objectives[j];

                    if (objective is DestroyObjectObjective)
                    {
                        DestroyObjectObjective obtain = (DestroyObjectObjective)objective;
						
                        if (obtain.Update(item))
                        {
                            if (quest.Completed)
                                quest.OnCompleted();
                            else if (obtain.Completed)
                                player.PlaySound(quest.UpdateSound);
									
                            return true;
                        }
                    }
                }
            }	
            return false;
        }

        public static bool CheckTamedObject(PlayerMobile player, BaseCreature creature)
        {
            for (int i = player.Quests.Count - 1; i >= 0; i--)
            {
                BaseQuest quest = player.Quests[i];

                for (int j = quest.Objectives.Count - 1; j >= 0; j--)
                {
                    if (quest.Objectives[j] is TamingObjective)
                    {
                        TamingObjective tame = (TamingObjective)quest.Objectives[j];

                        if (tame.Update(creature))
                        {
                            if (quest.Completed)
                                quest.OnCompleted();
                            else if (tame.Completed)
                                player.PlaySound(quest.UpdateSound);

                            return true;
                        }
                    }
                }
            }

            return false;
        }
	}
}
