using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.Quests
{
    public class BaseGuildQuester : MondainQuester
    {
        private string m_GuildType = "Unknown";
        private int m_Rank = 1;

        [CommandProperty(AccessLevel.GameMaster)]
        public string GuildType { get { return m_GuildType; } set { m_GuildType = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Rank { get { return m_Rank; } set { m_Rank = value; InvalidateProperties(); } }

        public BaseGuildQuester(string guildType, int rank)
        {
            m_GuildType = guildType;
            m_Rank = rank;

            Title = "the " + m_GuildType + " Guild Representative";
        }

        public BaseGuildQuester(Serial serial)
            : base(serial)
        {
        }

        public override Type[] Quests
        {
            get
            {
                 return null;
            }
        }

        public override void OnTalk(PlayerMobile player)
        {
            if (QuestHelper.DeliveryArrived(player, this))
                return;

            if (QuestHelper.InProgress(player, Quests))
                return;

            if (QuestHelper.QuestLimitReached(player))
                return;

            // check if this quester can offer any quest chain (already started)
            foreach (KeyValuePair<QuestChain, BaseChain> pair in player.Chains)
            {
                BaseChain chain = pair.Value;

                if (chain != null && chain.Quester != null && chain.Quester == GetType())
                {
                    BaseQuest quest = QuestHelper.RandomQuest(player, new Type[] { chain.CurrentQuest }, this);

                    if (quest != null)
                    {
                        player.CloseGump(typeof(MondainQuestGump));
                        player.SendGump(new MondainQuestGump(quest));
                        return;
                    }
                }
            }

            //BaseQuest questt = BaseGuildQuest.GetGuildQuest(player, Quests, this) as BaseQuest;
            BaseQuest questt = QuestHelper.RandomQuest(player, Quests, this);

            if (questt != null)
            {
                player.CloseGump(typeof(MondainQuestGump));
                player.SendGump(new MondainQuestGump(questt));
            }
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (m.Alive && m is PlayerMobile)
            {
                List<XmlAttachment> aList = XmlAttach.FindAttachments(m, typeof(XmlQuestGuild));
                //ArrayList aList = XmlAttach.FindAttachments(m, typeof(XmlQuestGuild));
                List<XmlQuestGuild> list = new List<XmlQuestGuild>();

                if (aList.Count > 0)
                {
                    foreach (XmlQuestGuild questGuild in aList)
                        list.Add(questGuild);
                }

                if (list.Count > 0)
                {
                    for(int i = 0; i < list.Count; i++)
                    {
                        if (m_GuildType == list[i].GuildType)
                        {
                            XmlQuestGuild.CheckResetQuests(list[i]);

                            if (list[i].QuestsTaken >= 3)
                            {
                                if (list[i].QuestsDone < 3 && QuestHelper.InProgress((PlayerMobile)m, this.Quests))
                                    return;
                                else
                                    Say("I cannot offer you any more work today.");
                            }
                            else
                            {
                                if (list[i].Rank >= m_Rank)
                                    OnTalk((PlayerMobile)m);
                                else
                                    Say("I'm sorry but you are not of high enough rank for me to offer you work.");
                            }
                        }
                    }
                }
            }
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            SpeechHue = Utility.RandomDyedHue();
            Hue = Utility.RandomSkinHue();

            if (IsInvulnerable && !Core.AOS)
                NameHue = 0x35;

            if (Female = GetGender())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
            }
        }

        public override void InitOutfit()
        {
            base.InitOutfit();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            switch(Rank)
            {
                case 1:
                    list.Add(1077475); //Novice
                    break;
                case 2:
                    list.Add(1077476); //Apprentice
                    break;
                case 3:
                    list.Add(1077477); //Journeyman
                    break;
                case 4:
                    list.Add(1077478); //Expert
                    break;
                case 5:
                    list.Add(1077480); //Master
                    break;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((int)m_Rank);
            writer.Write((string)m_GuildType);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Rank = reader.ReadInt();
            m_GuildType = reader.ReadString();
        }
    }
}
