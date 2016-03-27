using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.Quests
{
    public class BaseGuildQuest : BaseQuest
    {
        private string m_GuildType = "Unknown";
        private int m_Rank = 1;
        private int m_TokenAmount = 1;
        private int m_RepGain = 200;

        [CommandProperty(AccessLevel.GameMaster)]
        public string GuildType { get { return m_GuildType; } set { m_GuildType = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Rank { get { return m_Rank; } set { m_Rank = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TokenAmount { get { return m_TokenAmount; } set { m_TokenAmount = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RepGain { get { return m_RepGain; } set { m_RepGain = value; } }

        public BaseGuildQuest(string guildType)
            : base()
        {
            //string rewardString = null;

            m_GuildType = guildType;
        }

        public BaseGuildQuest(string guildType, int rank)
            : base()
        {
            //string rewardString = null;

            m_GuildType = guildType;
            m_Rank = rank;
        }

        private static void UpdateQuestRewards(BaseGuildQuest quest)
        {
            int tokens;
            bool give_rep = true;

                if (GetGuildRank(quest) >= quest.Rank)
                {
                    //Scale tokens
                    tokens = (quest.TokenAmount + quest.Rank) - GetGuildRank(quest);

                    if (GetGuildRank(quest) == 6)
                        tokens += 1;

                    //Scale rep
                    if (GetGuildRep(quest) >= GetQuestRepBounds(quest.Rank))
                        give_rep = false;

                    for (int i = 0; i < quest.Rewards.Count; i++)
                    {
                        BaseReward reward = quest.Rewards[i];

                        if (reward.Name.ToString() == GetGuildTokenName(quest))
                        {
                            reward.Name = tokens + " " + GetGuildTokenName(quest);
                            quest.TokenAmount = tokens;
                        }

                        if (reward.Name.ToString() == "Guild Standing")
                        {
                            if (give_rep)
                                reward.Name = quest.RepGain + " Guild Standing";
                            else
                            {
                                reward.Name = "Your rank has surpassed this level of quest.";
                                quest.RepGain = 0;
                            }
                        }
                    }
                }
            //}
        }

        //public static BaseGuildQuest GetGuildQuest(PlayerMobile from, Type[] quests, object quester)
        //{
        //    if (quests == null || (quests != null && quests.Length == 0))
        //        return null;

        //    BaseGuildQuest quest = ConstructGuildQuest(quests[0], quester) as BaseGuildQuest;

        //    if (quest != null)
        //    {
        //        quest.Owner = from;
        //        quest.Quester = quester;

        //        if (CanOffer(from, quest, quests.Length == 1))
        //            return quest;
        //        else if (quest.StartingMobile != null && !quest.DoneOnce && quests.Length == 1)
        //            quest.StartingMobile.OnOfferFailed();
        //    }
            
        //    return null;
        //}

        //public static object ConstructGuildQuest(Type type, object quester)
        //{
        //    if (type == null)
        //        return null;

        //    try
        //    {
        //        if (CustomUtilities.IsSubclassOfRawGeneric(typeof(BaseGuildQuest), type))
        //        {
        //            if (quester is BaseGuildQuester)
        //                Activator.CreateInstance(type, new Object[] { ((BaseGuildQuester)quester).GuildType, ((BaseGuildQuester)quester).Rank });
        //            else if (quester is BaseGuildQuestBoard)
        //                Activator.CreateInstance(type, new Object[] { ((BaseGuildQuestBoard)quester).GuildType, ((BaseGuildQuestBoard)quester).Rank });
        //            else
        //                return null;
        //        }
        //        else
        //            return null;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public static int GetQuestRepBounds(int rank)
        {
            switch (rank)
            {
                default:
                    return 0;
                case 1:
                    return 5000;
                case 2:
                    return 16250;
                case 3:
                    return 41250;
                case 4:
                    return 98250;
                case 5:
                    return 226250;
            }
        }

        public static string GetGuildTokenName(BaseGuildQuest quest)
        {
            List<XmlAttachment> aList = XmlAttach.FindAttachments(quest.Owner, typeof(XmlQuestGuild));
            List<XmlQuestGuild> list = new List<XmlQuestGuild>();

            if (aList.Count > 0)
            {
                foreach (XmlQuestGuild questGuild in aList)
                    list.Add(questGuild);
            }

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (quest.GuildType == list[i].GuildType)
                        return list[i].TokenName;
                }
                return "Unknown";
            }
            else
                return "Unknown";
        }

        public static int GetGuildRank(BaseGuildQuest quest)
        {
            List<XmlAttachment> aList = XmlAttach.FindAttachments(quest.Owner, typeof(XmlQuestGuild));
            List<XmlQuestGuild> list = new List<XmlQuestGuild>();

            if (aList.Count > 0)
            {
                foreach (XmlQuestGuild questGuild in aList)
                    list.Add(questGuild);
            }

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (quest.GuildType == list[i].GuildType)
                        return list[i].Rank;
                }
                return 1;
            }
            else
                return 1;
        }

        public static int GetGuildRep(BaseGuildQuest quest)
        {
            List<XmlAttachment> aList = XmlAttach.FindAttachments(quest.Owner, typeof(XmlQuestGuild));
            List<XmlQuestGuild> list = new List<XmlQuestGuild>();

            if (aList.Count > 0)
            {
                foreach (XmlQuestGuild questGuild in aList)
                    list.Add(questGuild);
            }

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (quest.GuildType == list[i].GuildType)
                        return list[i].Rep;
                }
                return 0;
            }
            else
                return 0;
        }

        public static int GetDailyTaken(BaseGuildQuest quest)
        {
            List<XmlAttachment> aList = XmlAttach.FindAttachments(quest.Owner, typeof(XmlQuestGuild));
            List<XmlQuestGuild> list = new List<XmlQuestGuild>();

            if (aList.Count > 0)
            {
                foreach (XmlQuestGuild questGuild in aList)
                    list.Add(questGuild);
            }

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (quest.GuildType == list[i].GuildType)
                        return list[i].QuestsTaken;
                }
                return 0;
            }
            else
                return 0;
        }

        public static void SetDailyTaken(BaseGuildQuest quest, int taken)
        {
            List<XmlAttachment> aList = XmlAttach.FindAttachments(quest.Owner, typeof(XmlQuestGuild));
            List<XmlQuestGuild> list = new List<XmlQuestGuild>();

            if (aList.Count > 0)
            {
                foreach (XmlQuestGuild questGuild in aList)
                    list.Add(questGuild);
            }

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (quest.GuildType == list[i].GuildType)
                        list[i].QuestsTaken += taken;
                }
            }
        }

        public static int GetDailyDone(BaseGuildQuest quest)
        {
            List<XmlAttachment> aList = XmlAttach.FindAttachments(quest.Owner, typeof(XmlQuestGuild));
            List<XmlQuestGuild> list = new List<XmlQuestGuild>();

            if (aList.Count > 0)
            {
                foreach (XmlQuestGuild questGuild in aList)
                    list.Add(questGuild);
            }

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (quest.GuildType == list[i].GuildType)
                        return list[i].QuestsDone;
                }
                return 0;
            }
            else
                return 0;
        }

        public static void SetDailyDone(BaseGuildQuest quest, int done)
        {
            List<XmlAttachment> aList = XmlAttach.FindAttachments(quest.Owner, typeof(XmlQuestGuild));
            List<XmlQuestGuild> list = new List<XmlQuestGuild>();

            if (aList.Count > 0)
            {
                foreach (XmlQuestGuild questGuild in aList)
                    list.Add(questGuild);
            }

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (quest.GuildType == list[i].GuildType)
                        list[i].QuestsDone += done;
                }
            }
        }

        /*
        public override bool CanOffer()
        {
            for (int i = 0; i < Owner.Quests.Count; i++)
            {
                BaseQuest quest = Owner.Quests[i];

                if (quest != null)
                {
                    if (quest.Quester == Quester)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            return true;
        }
        */

        public override void GiveRewards()
        {
            XmlAddRewardToken rewardTokens = new XmlAddRewardToken(m_TokenAmount, m_GuildType);
            XmlAttach.AttachTo(Owner, rewardTokens);

            XmlAddGuildRep rewardRep = new XmlAddGuildRep(m_RepGain, m_GuildType);
            XmlAttach.AttachTo(Owner, rewardRep);

            SetDailyDone(this, 1);

            base.GiveRewards();
        }

        public override void OnAccept()
        {
            SetDailyTaken(this, 1);

            UpdateQuestRewards(this);

            base.OnAccept();
        }

        public override void OnRefuse()
        {
            SetDailyTaken(this, 1);
            SetDailyDone(this, 1);

            base.OnRefuse();
        }

        public override void OnResign(bool resignChain)
        {
            SetDailyDone(this, 1);

            base.OnResign(resignChain);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((string)m_GuildType);
            writer.Write((int)m_Rank);
            writer.Write((int)m_RepGain);
            writer.Write((int)m_TokenAmount);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_GuildType = reader.ReadString();
            m_Rank = reader.ReadInt();
            m_RepGain = reader.ReadInt();
            m_TokenAmount = reader.ReadInt();
        }
    }
}
