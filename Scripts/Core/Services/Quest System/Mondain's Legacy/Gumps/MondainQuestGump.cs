using System;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class MondainQuestGump : BaseQuestGump
    {
        private const int ButtonOffset = 10;
        private readonly object m_Quester;
        private readonly PlayerMobile m_From;
        private readonly BaseQuest m_Quest;
        private readonly bool m_Offer;
        private readonly bool m_Completed;
        private Section m_Section;

        public MondainQuestGump(PlayerMobile from)
            : this(from, null, Section.Main, false, false)
        {
        }

        public MondainQuestGump(BaseQuest quest)
            : this(quest, Section.Description, true)
        {
        }

        public MondainQuestGump(BaseQuest quest, Section section, bool offer)
            : this(null, quest, section, offer, false)
        {
        }

        public MondainQuestGump(BaseQuest quest, Section section, bool offer, bool completed)
            : this(null, quest, section, offer, completed)
        {
        }

        public MondainQuestGump(PlayerMobile owner, BaseQuest quest, Section section, bool offer, bool completed)
            : this(owner, quest, section, offer, completed, null)
        {
        }

        public MondainQuestGump(PlayerMobile owner, BaseQuest quest, Section section, bool offer, bool completed, object quester)
            : base(75, 25)
        {
            m_Quester = quester;
            m_Quest = quest;
            m_Section = section;
            m_Offer = offer;
            m_Completed = completed;

            if (quest != null)
            {
                m_From = quest.Owner;
            }
            else
            {
                m_From = owner;
            }
		
            Closable = false;
            Disposable = true;
            Dragable = true;
            Resizable = false;
			
            AddPage(0);
			
            AddImageTiled(50, 20, 400, 400, 0x1404);
            AddImageTiled(50, 29, 30, 390, 0x28DC);
            AddImageTiled(34, 140, 17, 279, 0x242F);			
            AddImage(48, 135, 0x28AB);
            AddImage(-16, 285, 0x28A2);
            AddImage(0, 10, 0x28B5);
            AddImage(25, 0, 0x28B4);
            AddImageTiled(83, 15, 350, 15, 0x280A);
            AddImage(34, 419, 0x2842);
            AddImage(442, 419, 0x2840);
            AddImageTiled(51, 419, 392, 17, 0x2775);
            AddImageTiled(415, 29, 44, 390, 0xA2D);
            AddImageTiled(415, 29, 30, 390, 0x28DC);
            AddImage(370, 50, 0x589);

            if ((int)m_From.AccessLevel > (int)AccessLevel.Counselor && quest != null)
            {
                AddButton(379, 60, 0x15A9, 0x15A9, (int)Buttons.CompleteQuest, GumpButtonType.Reply, 0);
            }
            else
            {
                AddImage(379, 60, 0x15A9);
            }
						
            AddImage(425, 0, 0x28C9);
            AddImage(90, 33, 0x232D);
            AddImageTiled(130, 65, 175, 1, 0x238D);
			
            switch ( m_Section )
            {
                case Section.Main:
                    SecMain();
                    break;
                case Section.Description:
                    SecDescription();
                    break;
                case Section.Objectives:
                    SecObjectives();
                    break; 
                case Section.Rewards:
                    SecRewards();
                    break;
                case Section.Refuse:
                    SecRefuse();
                    break;
                case Section.Complete:
                    SecComplete();
                    break;
                case Section.InProgress:
                    SecInProgress();
                    break;
            }
        }

        public enum Section
        {
            Main,
            Description,
            Objectives,
            Rewards,
            Refuse,
            Complete,
            InProgress,
        }

        private enum Buttons
        {
            Close,
            CloseQuest,
            RefuseQuest,
            ResignQuest,
            AcceptQuest,
            AcceptReward,
            PreviousPage,
            NextPage,
            Complete,
            CompleteQuest,
        }
        public virtual void SecMain()
        {
            if (m_From == null)
            {
                return;
            }
		
            AddHtmlLocalized(130, 45, 270, 16, 1046026, 0xFFFFFF, false, false); // Quest Log
				
            int offset = 140;
			
            for (int i = m_From.Quests.Count - 1; i >= 0; i--)
            {
                BaseQuest quest = m_From.Quests[i];
				
                AddHtmlObject(98, offset, 270, 21, quest.Title, quest.Failed ? 0x3C00 : White, false, false);					
                AddButton(368, offset, 0x26B0, 0x26B1, ButtonOffset + i, GumpButtonType.Reply, 0);	
				
                offset += 21;			
            }
				
            AddButton(313, 395, 0x2EEC, 0x2EEE, (int)Buttons.Close, GumpButtonType.Reply, 0);
        }

        public virtual void SecDescription()
        {
            if (m_Quest == null)
            {
                return;
            }

            if (m_Offer)
            {
                AddHtmlLocalized(130, 45, 270, 16, 1049010, 0xFFFFFF, false, false); // Quest Offer
            }
            else
            {
                AddHtmlLocalized(130, 45, 270, 16, 1046026, 0xFFFFFF, false, false); // Quest Log
            }

            if (m_Quest.Failed)
            {
                AddHtmlLocalized(160, 80, 250, 16, 500039, 0x3C00, false, false); // Failed!
            }
			
            AddHtmlObject(160, 108, 250, 16, m_Quest.Title, DarkGreen, false, false);

            if (m_Quest.ChainID != QuestChain.None)
            {
                AddHtmlLocalized(98, 140, 312, 16, 1075024, 0x2710, false, false); // Description (quest chain)
            }
            else
            {
                AddHtmlLocalized(98, 140, 312, 16, 1072202, 0x2710, false, false); // Description
            }
			
            AddHtmlObject(98, 156, 312, 180, m_Quest.Description, LightGreen, false, true);
				
            if (m_Offer)
            {
                AddButton(95, 395, 0x2EE0, 0x2EE2, (int)Buttons.AcceptQuest, GumpButtonType.Reply, 0);
                AddButton(313, 395, 0x2EF2, 0x2EF4, (int)Buttons.RefuseQuest, GumpButtonType.Reply, 0);
            }
            else
            {
                AddButton(95, 395, 0x2EF5, 0x2EF7, (int)Buttons.ResignQuest, GumpButtonType.Reply, 0);
                AddButton(313, 395, 0x2EEC, 0x2EEE, (int)Buttons.CloseQuest, GumpButtonType.Reply, 0);
            }
			
            AddButton(275, 370, 0x2EE9, 0x2EEB, (int)Buttons.NextPage, GumpButtonType.Reply, 0);
        }

        public virtual void SecObjectives()
        {
            if (m_Quest == null)
            {
                return;
            }

            if (m_Offer)
            {
                AddHtmlLocalized(130, 45, 270, 16, 1049010, 0xFFFFFF, false, false); // Quest Offer
            }
            else
            {
                AddHtmlLocalized(130, 45, 270, 16, 1046026, 0xFFFFFF, false, false); // Quest Log
            }
			
            AddHtmlObject(160, 108, 250, 16, m_Quest.Title, DarkGreen, false, false);
            AddHtmlLocalized(98, 140, 312, 16, 1049073, 0x2710, false, false); // Objective:

            if (m_Quest.AllObjectives)
            {
                AddHtmlLocalized(98, 156, 312, 16, 1072208, 0x2710, false, false); // All of the following	
            }
            else
            {
                AddHtmlLocalized(98, 156, 312, 16, 1072209, 0x2710, false, false); // Only one of the following
            }
			
            int offset = 172;

            for (int i = 0; i < m_Quest.Objectives.Count; i++)
            {
                BaseObjective objective = m_Quest.Objectives[i];

                if (objective is SlayObjective)
                {
                    SlayObjective slay = (SlayObjective)objective;

                    AddHtmlLocalized(98, offset, 30, 16, 1072204, 0x15F90, false, false); // Slay	
                    AddLabel(133, offset, 0x481, slay.MaxProgress + " " + slay.Name); // %count% + %name%

                    offset += 16;

                    if (m_Offer)
                    {
                        if (slay.Timed)
                        {
                            AddHtmlLocalized(103, offset, 120, 16, 1062379, 0x15F90, false, false); // Est. time remaining:
                            AddLabel(223, offset, 0x481, FormatSeconds(slay.Seconds)); // %est. time remaining%

                            offset += 16;
                        }
                        continue;
                    }

                    if (slay.Region != null)
                    {
                        AddHtmlLocalized(103, offset, 312, 20, 1018327, 0x15F90, false, false); // Location
                        AddHtmlObject(223, offset, 312, 20, slay.Region.Name, White, false, false); // %location%

                        offset += 16;
                    }

                    AddHtmlLocalized(103, offset, 120, 16, 3000087, 0x15F90, false, false); // Total			
                    AddLabel(223, offset, 0x481, slay.CurProgress.ToString());	 // %current progress%

                    offset += 16;

                    if (ReturnTo() != null)
                    {
                        AddHtmlLocalized(103, offset, 120, 16, 1074782, 0x15F90, false, false); // Return to	
                        AddLabel(223, offset, 0x481, ReturnTo());	 // %return to%		

                        offset += 16;
                    }

                    if (slay.Timed)
                    {
                        AddHtmlLocalized(103, offset, 120, 16, 1062379, 0x15F90, false, false); // Est. time remaining:
                        AddLabel(223, offset, 0x481, FormatSeconds(slay.Seconds)); // %est. time remaining%

                        offset += 16;
                    }
                }
                else if (objective is ObtainObjective)
                {
                    ObtainObjective obtain = (ObtainObjective)objective;

                    AddHtmlLocalized(98, offset, 40, 16, 1072205, 0x15F90, false, false); // Obtain						
                    AddLabel(143, offset, 0x481, obtain.MaxProgress + " " + obtain.Name); // %count% + %name%

                    if (obtain.Image > 0)
                    {
                        AddItem(350, offset, obtain.Image); // Image
                    }

                    offset += 16;

                    if (m_Offer)
                    {
                        if (obtain.Timed)
                        {
                            AddHtmlLocalized(103, offset, 120, 16, 1062379, 0x15F90, false, false); // Est. time remaining:
                            AddLabel(223, offset, 0x481, FormatSeconds(obtain.Seconds)); // %est. time remaining%

                            offset += 16;
                        }
                        else if (obtain.Image > 0)
                            offset += 16;

                        continue;
                    }
                    AddHtmlLocalized(103, offset, 120, 16, 3000087, 0x15F90, false, false); // Total			
                    AddLabel(223, offset, 0x481, obtain.CurProgress.ToString());	 // %current progress%

                    offset += 16;

                    if (ReturnTo() != null)
                    {
                        AddHtmlLocalized(103, offset, 120, 16, 1074782, 0x15F90, false, false); // Return to	
                        AddLabel(223, offset, 0x481, ReturnTo());	 // %return to%

                        offset += 16;
                    }

                    if (obtain.Timed)
                    {
                        AddHtmlLocalized(103, offset, 120, 16, 1062379, 0x15F90, false, false); // Est. time remaining:
                        AddLabel(223, offset, 0x481, FormatSeconds(obtain.Seconds)); // %est. time remaining%

                        offset += 16;
                    }
                }
                else if (objective is GuildCraftObjective)
                {
                    GuildCraftObjective craft = (GuildCraftObjective)objective;

                    AddHtmlLocalized(98, offset, 40, 16, 1072205, 0x15F90, false, false); // Obtain

                    string resource = null;

                    switch (craft.Resource)
                    {
                        case CraftResource.None:
                            resource = "none";
                            break;
                        case CraftResource.Iron:
                            resource = "iron";
                            break;
                        case CraftResource.DullCopper:
                            resource = "dull copper";
                            break;
                        case CraftResource.ShadowIron:
                            resource = "shadow iron";
                            break;
                        case CraftResource.Copper:
                            resource = "copper";
                            break;
                        case CraftResource.Bronze:
                            resource = "bronze";
                            break;
                        case CraftResource.Gold:
                            resource = "gold";
                            break;
                        case CraftResource.Agapite:
                            resource = "agapite";
                            break;
                        case CraftResource.Verite:
                            resource = "verite";
                            break;
                        case CraftResource.Valorite:
                            resource = "valorite";
                            break;
                        case CraftResource.RegularLeather:
                            resource = "leather";
                            break;
                        case CraftResource.SpinedLeather:
                            resource = "spined";
                            break;
                        case CraftResource.HornedLeather:
                            resource = "horned";
                            break;
                        case CraftResource.BarbedLeather:
                            resource = "barbed";
                            break;
                        case CraftResource.RedScales:
                            resource = "red scale";
                            break;
                        case CraftResource.YellowScales:
                            resource = "yellow scale";
                            break;
                        case CraftResource.BlackScales:
                            resource = "black scale";
                            break;
                        case CraftResource.WhiteScales:
                            resource = "white scale";
                            break;
                        case CraftResource.GreenScales:
                            resource = "green scale";
                            break;
                        case CraftResource.BlueScales:
                            resource = "blue scale";
                            break;
                        case CraftResource.RegularWood:
                            resource = "wooden";
                            break;
                        case CraftResource.OakWood:
                            resource = "oak";
                            break;
                        case CraftResource.AshWood:
                            resource = "ash";
                            break;
                        case CraftResource.YewWood:
                            resource = "yew";
                            break;
                        case CraftResource.Heartwood:
                            resource = "heartwood";
                            break;
                        case CraftResource.Bloodwood:
                            resource = "bloodwood";
                            break;
                        case CraftResource.Frostwood:
                            resource = "frostwood";
                            break;
                    }
                    if (craft.Exceptional && craft.Resource != CraftResource.None)
                    {
                        AddLabel(143, offset, 0x481, craft.MaxProgress + " exceptional " + resource + " " + craft.Name); // %count% + %name%
                    }
                    else if (craft.Exceptional && craft.Resource == CraftResource.None)
                    {
                        AddLabel(143, offset, 0x481, craft.MaxProgress + " exceptional " + craft.Name); // %count% + %name%
                    }
                    else if (!craft.Exceptional && craft.Resource != CraftResource.None)
                    {
                        AddLabel(143, offset, 0x481, craft.MaxProgress + " " + resource + " " + craft.Name); // %count% + %name%
                    }
                    else
                    {
                        AddLabel(143, offset, 0x481, craft.MaxProgress + " " + craft.Name); // %count% + %name%
                    }

                    if (craft.Image > 0)
                        AddItem(350, offset, craft.Image); // Image

                    offset += 16;

                    if (m_Offer)
                    {
                        if (craft.Timed)
                        {
                            AddHtmlLocalized(103, offset, 120, 16, 1062379, 0x15F90, false, false); // Est. time remaining:
                            AddLabel(223, offset, 0x481, FormatSeconds(craft.Seconds)); // %est. time remaining%

                            offset += 16;
                        }
                        else if (craft.Image > 0)
                        {
                            offset += 16;
                        }

                        continue;
                    }
                    AddHtmlLocalized(103, offset, 120, 16, 3000087, 0x15F90, false, false); // Total			
                    AddLabel(223, offset, 0x481, craft.CurProgress.ToString());	 // %current progress%

                    offset += 16;

                    if (m_Quest is BaseGuildQuest)
                    {
                        AddHtmlLocalized(103, offset, 120, 16, 1074782, 0x15F90, false, false); // Return to
                        AddLabel(223, offset, 0x481, String.Format("{0} Guild", ((BaseGuildQuest)m_Quest).GuildType));

                        offset += 16;
                    }
                    else if (ReturnTo() != null)
                    {
                        AddHtmlLocalized(103, offset, 120, 16, 1074782, 0x15F90, false, false); // Return to
                        AddLabel(223, offset, 0x481, ReturnTo());	 // %return to%

                        offset += 16;
                    }

                    if (craft.Timed)
                    {
                        AddHtmlLocalized(103, offset, 120, 16, 1062379, 0x15F90, false, false); // Est. time remaining:
                        AddLabel(223, offset, 0x481, FormatSeconds(craft.Seconds)); // %est. time remaining%

                        offset += 16;
                    }
                }
                else if (objective is DeliverObjective)
                {
                    DeliverObjective deliver = (DeliverObjective)objective;

                    AddKRHtmlLocalized(0, 0, 0, 0, -5, false, false);

                    AddHtmlLocalized(98, offset, 40, 16, 1072207, 0x15F90, false, false); // Deliver						
                    AddLabel(143, offset, 0x481, deliver.MaxProgress + " " + deliver.DeliveryName);	 // %name%

                    offset += 16;

                    AddHtmlLocalized(103, offset, 120, 16, 1072379, 0x15F90, false, false); // Deliver to						
                    AddLabel(223, offset, 0x481, deliver.DestName); // %deliver to%

                    offset += 16;

                    if (deliver.Timed)
                    {
                        AddHtmlLocalized(103, offset, 120, 16, 1062379, 0x15F90, false, false); // Est. time remaining:
                        AddLabel(223, offset, 0x481, FormatSeconds(deliver.Seconds)); // %est. time remaining%

                        offset += 16;
                    }
                }
                else if (objective is EscortObjective)
                {
                    EscortObjective escort = (EscortObjective)objective;

                    AddKRHtmlLocalized(0, 0, 0, 0, -4, false, false);

                    if (escort != null)
                    {
                        AddHtmlLocalized(98, offset, 50, 16, 1072206, 0x15F90, false, false); // Escort to
                        AddHtmlObject(153, offset, 200, 16, escort.Region.Name, White, false, false);

                        offset += 16;

                        if (escort.Timed)
                        {
                            AddHtmlLocalized(103, offset, 120, 16, 1062379, 0x15F90, false, false); // Est. time remaining:
                            AddLabel(223, offset, 0x481, FormatSeconds(escort.Seconds)); // %est. time remaining%

                            offset += 16;
                        }
                    }
                }
                else if (objective is ApprenticeObjective)
                {
                    ApprenticeObjective apprentice = (ApprenticeObjective)objective;

                    AddKRHtmlLocalized(0, 0, 0, 0, -9, false, false);

                    AddHtmlLocalized(98, offset, 200, 16, 1077485, "#" + (1044060 + (int)apprentice.Skill) + "\t" + apprentice.MaxProgress, 0x15F90, false, false); // Increase ~1_SKILL~ to ~2_VALUE~

                    offset += 16;
                }
                else if (objective is DestroyObjectObjective)
                {
                    DestroyObjectObjective obtain = (DestroyObjectObjective)objective;

                    //AddHtmlLocalized(98, offset, 40, 16, 1072205, 0x15F90, false, false); // Obtain	
                    //was 143
                    AddLabel(98, offset, 0x481, "Destroy " + obtain.MaxProgress + " " + obtain.Name); // %count% + %name%

                    offset += 16;

                    if (m_Offer)
                    {
                        if (obtain.Timed)
                        {
                            AddHtmlLocalized(103, offset, 120, 16, 1062379, 0x15F90, false, false); // Est. time remaining:
                            AddLabel(223, offset, 0x481, FormatSeconds(obtain.Seconds)); // %est. time remaining%

                            offset += 16;
                        }
                        else if (obtain.Image > 0)
                            offset += 16;

                        continue;
                    }
                    AddHtmlLocalized(103, offset, 120, 16, 3000087, 0x15F90, false, false); // Total			
                    AddLabel(223, offset, 0x481, obtain.CurProgress.ToString());	 // %current progress%

                    offset += 16;

                    if (ReturnTo() != null)
                    {
                        AddHtmlLocalized(103, offset, 120, 16, 1074782, 0x15F90, false, false); // Return to	
                        AddLabel(223, offset, 0x481, ReturnTo());	 // %return to%

                        offset += 16;
                    }

                    if (obtain.Timed)
                    {
                        AddHtmlLocalized(103, offset, 120, 16, 1062379, 0x15F90, false, false); // Est. time remaining:
                        AddLabel(223, offset, 0x481, FormatSeconds(obtain.Seconds)); // %est. time remaining%

                        offset += 16;
                    }
                }
                else if (objective is TamingObjective)
                {
                    TamingObjective obtain = (TamingObjective)objective;

                    //AddHtmlLocalized(98, offset, 40, 16, 1072205, 0x15F90, false, false); // Obtain	
                    //was 143
                    AddLabel(98, offset, 0x481, "Tame " + obtain.MaxProgress + " " + obtain.Name); // %count% + %name%

                    offset += 16;

                    if (m_Offer)
                    {
                        if (obtain.Timed)
                        {
                            AddHtmlLocalized(103, offset, 120, 16, 1062379, 0x15F90, false, false); // Est. time remaining:
                            AddLabel(223, offset, 0x481, FormatSeconds(obtain.Seconds)); // %est. time remaining%

                            offset += 16;
                        }
                        else if (obtain.Image > 0)
                            offset += 16;

                        continue;
                    }
                    AddHtmlLocalized(103, offset, 120, 16, 3000087, 0x15F90, false, false); // Total			
                    AddLabel(223, offset, 0x481, obtain.CurProgress.ToString());	 // %current progress%

                    offset += 16;

                    if (ReturnTo() != null)
                    {
                        AddHtmlLocalized(103, offset, 120, 16, 1074782, 0x15F90, false, false); // Return to	
                        AddLabel(223, offset, 0x481, ReturnTo());	 // %return to%

                        offset += 16;
                    }

                    if (obtain.Timed)
                    {
                        AddHtmlLocalized(103, offset, 120, 16, 1062379, 0x15F90, false, false); // Est. time remaining:
                        AddLabel(223, offset, 0x481, FormatSeconds(obtain.Seconds)); // %est. time remaining%

                        offset += 16;
                    }
                }
                else if (objective is BaseBardObjective)
                {
                    BaseBardObjective bardObjective = (BaseBardObjective)objective;

                    AddHtmlLocalized(98, offset, 200, 16, bardObjective.Cliloc, 0x15F90, false, false);

                    offset += 16;
                }
                    /*
                else if (objective is InciteObjective)
                {
                    if (m_Quest.GetType() == typeof(IndoctrinationBattleRouserQuest))
                    {
                        InciteObjective incite = (InciteObjective)objective;

                        AddHtmlLocalized(98, offset, 350, 16, 1115658, 0x15F90, false, false);

                        offset += 16;
                    }
                }
                else if (objective is CalmObjective)
                {
                    if (m_Quest.GetType() == typeof(BeaconOfHarmony))
                    {
                        CalmObjective calm = (CalmObjective)objective;

                        AddHtmlLocalized(98, offset, 350, 16, 1115678, 0x15F90, false, false); //Calm five mongbats.  

                        offset += 16;
                    }
                }
                else if (objective is DiscordObjective)
                {
                    if (m_Quest.GetType() == typeof(WieldingSonicBlade))
                    {
                        DiscordObjective discord = (DiscordObjective)objective;

                        AddHtmlLocalized(98, offset, 350, 16, 1115698, 0x15F90, false, false); // Discord five goats.  

                        offset += 16;
                    }
                }
                     */
            }
			
            if (m_Offer)
            {
                AddButton(95, 395, 0x2EE0, 0x2EE2, (int)Buttons.AcceptQuest, GumpButtonType.Reply, 0);
                AddButton(313, 395, 0x2EF2, 0x2EF4, (int)Buttons.RefuseQuest, GumpButtonType.Reply, 0);
            }
            else
            {
                AddButton(95, 395, 0x2EF5, 0x2EF7, (int)Buttons.ResignQuest, GumpButtonType.Reply, 0);
                AddButton(313, 395, 0x2EEC, 0x2EEE, (int)Buttons.CloseQuest, GumpButtonType.Reply, 0);
            }
			
            AddButton(130, 370, 0x2EEF, 0x2EF1, (int)Buttons.PreviousPage, GumpButtonType.Reply, 0);
            AddButton(275, 370, 0x2EE9, 0x2EEB, (int)Buttons.NextPage, GumpButtonType.Reply, 0);
        }

        public virtual void SecRewards()
        {
            if (m_Quest == null)
                return;
		
            if (m_Offer)
                AddHtmlLocalized(130, 45, 270, 16, 1049010, 0xFFFFFF, false, false); // Quest Offer
            else
                AddHtmlLocalized(130, 45, 270, 16, 1046026, 0xFFFFFF, false, false); // Quest Log	
			
            AddHtmlObject(160, 108, 250, 16, m_Quest.Title, DarkGreen, false, false);
            AddHtmlLocalized(98, 140, 312, 16, 1072201, 0x2710, false, false); // Reward	
			
            int offset = 163;
			
            for (int i = 0; i < m_Quest.Rewards.Count; i++)
            {
                BaseReward reward = m_Quest.Rewards[i];
				
                AddImage(105, offset, 0x4B9);
                AddHtmlObject(133, offset, 280, 16, reward.Name, LightGreen, false, false);
				
                offset += 16;
            }
			
            if (m_Completed)
            {
                AddButton(95, 395, 0x2EE0, 0x2EE2, (int)Buttons.AcceptReward, GumpButtonType.Reply, 0);
            }
            else if (m_Offer)
            {
                AddButton(95, 395, 0x2EE0, 0x2EE2, (int)Buttons.AcceptQuest, GumpButtonType.Reply, 0);
                AddButton(313, 395, 0x2EF2, 0x2EF4, (int)Buttons.RefuseQuest, GumpButtonType.Reply, 0);
                AddButton(130, 370, 0x2EEF, 0x2EF1, (int)Buttons.PreviousPage, GumpButtonType.Reply, 0);
            }
            else
            {
                AddButton(95, 395, 0x2EF5, 0x2EF7, (int)Buttons.ResignQuest, GumpButtonType.Reply, 0);
                AddButton(313, 395, 0x2EEC, 0x2EEE, (int)Buttons.CloseQuest, GumpButtonType.Reply, 0);
                AddButton(130, 370, 0x2EEF, 0x2EF1, (int)Buttons.PreviousPage, GumpButtonType.Reply, 0);
            }
        }

        public virtual void SecRefuse()
        {
            if (m_Quest == null)
                return;
		
            if (m_Offer)
            {
                AddHtmlLocalized(130, 45, 270, 16, 3006156, 0xFFFFFF, false, false); // Quest Conversation
                AddImage(140, 110, 0x4B9);
                AddHtmlObject(160, 108, 250, 16, m_Quest.Title, DarkGreen, false, false);
                AddHtmlObject(98, 140, 312, 180, m_Quest.Refuse, LightGreen, false, true);
				
                AddButton(313, 395, 0x2EE6, 0x2EE8, (int)Buttons.Close, GumpButtonType.Reply, 0);
            }
        }

        public virtual void SecInProgress()
        {
            if (m_Quest == null)
                return;
				
            AddHtmlLocalized(130, 45, 270, 16, 3006156, 0xFFFFFF, false, false); // Quest Conversation				
            AddImage(140, 110, 0x4B9);
            AddHtmlObject(160, 108, 250, 16, m_Quest.Title, DarkGreen, false, false);	
            AddHtmlObject(98, 140, 312, 180, m_Quest.Uncomplete, LightGreen, false, true);
							
            AddButton(313, 395, 0x2EE6, 0x2EE8, (int)Buttons.Close, GumpButtonType.Reply, 0);
        }

        public virtual void SecComplete()
        {
            if (m_Quest == null)
                return;
				
            if (m_Quest.Complete == null)
            {
                if (QuestHelper.TryDeleteItems(m_Quest))
                {
                    if (QuestHelper.AnyRewards(m_Quest))
                    {
                        m_Section = Section.Rewards;
                        SecRewards();
                    }
                    else
                        m_Quest.GiveRewards();
                }
					
                return;
            }
				
            AddHtmlLocalized(130, 45, 270, 16, 3006156, 0xFFFFFF, false, false); // Quest Conversation
            AddImage(140, 110, 0x4B9);
            AddHtmlObject(160, 108, 250, 16, m_Quest.Title, DarkGreen, false, false);	
            AddHtmlObject(98, 140, 312, 180, m_Quest.Complete, LightGreen, false, true);
				
            AddButton(313, 395, 0x2EE6, 0x2EE8, (int)Buttons.Close, GumpButtonType.Reply, 0);
            AddButton(95, 395, 0x2EE9, 0x2EEB, (int)Buttons.Complete, GumpButtonType.Reply, 0);
        }

        public virtual string FormatSeconds(int seconds)
        {
            int hours = seconds / 3600;
			
            seconds -= hours * 3600;
			
            int minutes = seconds / 60;
			
            seconds -= minutes * 60;
			
            if (hours > 0 && minutes > 0)
                return hours + ":" + minutes + ":" + seconds;
            else if (minutes > 0)
                return minutes + ":" + seconds;
            else
                return seconds.ToString();
        }

        public virtual string ReturnTo()
        {
            if (m_Quest == null)
                return null;
				
            if (m_Quest.StartingMobile != null)
            {
                string returnTo = m_Quest.StartingMobile.Name;				
				
                if (m_Quest.StartingMobile.Region != null)
                    returnTo = String.Format("{0} ({1})", returnTo, m_Quest.StartingMobile.Region.Name);
                else
                    returnTo = String.Format("{0}", returnTo);
					
                return returnTo;
            }
			
            return null;
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (m_From != null)
            {
                m_From.CloseGump(typeof(MondainQuestGump));
            }
				
            switch ( info.ButtonID )
            { 
                // close quest list
                case (int)Buttons.Close:
                    break;
                    // close quest
                case (int)Buttons.CloseQuest:
                    m_From.SendGump(new MondainQuestGump(m_From));
                    break;
                    // accept quest
                case (int)Buttons.AcceptQuest:
                    if (m_Offer)
                        m_Quest.OnAccept();						
                    break;
                    // refuse quest
                case (int)Buttons.RefuseQuest:
                    if (m_Offer)
                    {
                        m_Quest.OnRefuse();
                        m_From.SendGump(new MondainQuestGump(m_Quest, Section.Refuse, true));
                    }
                    break;
                    // resign quest
                case (int)Buttons.ResignQuest:
                    if (!m_Offer)
                    {
                        m_From.SendGump(new MondainResignGump(m_Quest));
                    }
                    break;
                    // accept reward
                case (int)Buttons.AcceptReward:
                    if (!m_Offer && m_Section == Section.Rewards && m_Completed)
                    {
                        m_Quest.GiveRewards();
                    }
                    break;
                    // previous page
                case (int)Buttons.PreviousPage:
                    if (m_Section == Section.Objectives || (m_Section == Section.Rewards && !m_Completed))
                    {
                        m_Section = (Section)((int)m_Section - 1);
                        m_From.SendGump(new MondainQuestGump(m_Quest, m_Section, m_Offer));						
                    }
                    break;
                    // next page
                case (int)Buttons.NextPage:
                    if (m_Section == Section.Description || m_Section == Section.Objectives)
                    {
                        m_Section = (Section)((int)m_Section + 1);
                        m_From.SendGump(new MondainQuestGump(m_Quest, m_Section, m_Offer));						
                    }
                    break;
                    // player complete quest
                case (int)Buttons.Complete:
                    if (!m_Offer && m_Section == Section.Complete)
                    {
                        if (!m_Quest.Completed)
                        {
                            m_From.SendLocalizedMessage(1074861); // You do not have everything you need!
                        }
                        else
                        {
                            QuestHelper.DeleteItems(m_Quest);

                            if (m_Quester != null)
                            {
                                m_Quest.Quester = m_Quester;
                            }

                            if (!QuestHelper.AnyRewards(m_Quest))
                            {
                                m_Quest.GiveRewards();
                            }
                            else
                            {
                                m_From.SendGump(new MondainQuestGump(m_Quest, Section.Rewards, false, true));
                            }
                        }
                    }
                    break;
                    // admin complete quest
                case (int)Buttons.CompleteQuest:
                    if ((int)m_From.AccessLevel > (int)AccessLevel.Counselor && m_Quest != null)
                    {
                        QuestHelper.CompleteQuest(m_From, m_Quest);
                    }
                    break;
                    // show quest
                default:
                    if (m_Section != Section.Main || info.ButtonID >= m_From.Quests.Count + ButtonOffset || info.ButtonID < ButtonOffset)
                    {
                        break;
                    }
					
                    m_From.SendGump(new MondainQuestGump(m_From.Quests[(int)info.ButtonID - ButtonOffset], Section.Description, false));										
                    break;
            }
        }
    }
}