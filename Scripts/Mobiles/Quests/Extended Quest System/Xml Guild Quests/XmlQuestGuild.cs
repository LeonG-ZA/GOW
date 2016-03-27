using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Commands;
using Server.Commands.Generic;

namespace Server.Engines.XmlSpawner2
{
    public class XmlQuestGuild : XmlAttachment
    {
        public static new void Initialize()
        {
            CommandSystem.Register("ResetDailys", AccessLevel.GameMaster, new CommandEventHandler(ResetDailys_OnCommand));
        }

        [Usage("ResetDailys")]
        [Description("Resets daily quests")]
        public static void ResetDailys_OnCommand(CommandEventArgs e)
        {
            List<XmlAttachment> aList = XmlAttach.FindAttachments(e.Mobile, typeof(XmlQuestGuild));
            //ArrayList aList = XmlAttach.FindAttachments(e.Mobile, typeof(XmlQuestGuild));
            List<XmlQuestGuild> list = new List<XmlQuestGuild>();

            if (aList.Count > 0)
            {
                foreach (XmlQuestGuild questGuild in aList)
                    list.Add(questGuild);
            }

            for (int i = 0; i < list.Count; i++)
            {
                list[i].QuestsDone = 0;
                list[i].QuestsTaken = 0;
                list[i].Reset = false;
                list[i].ResetTime = DateTime.UtcNow;
            }

            e.Mobile.SendMessage("All daily quests reset");
        }

        private string m_GuildType;
        private string m_TokenName;
        private int m_Rank = 1;
        private int m_Rep;
        private int m_Tokens;
        private int m_QuestsDone;
        private int m_QuestsTaken;
        private bool m_Reset;
        private DateTime m_ResetTime;

        [CommandProperty(AccessLevel.GameMaster)]
        //public string GuildType { get { return m_GuildType; } set { m_GuildType = value; CheckDuplicate(this); } }
        public string GuildType { get { return m_GuildType; } set { m_GuildType = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public string TokenName { get { return m_TokenName; } set { m_TokenName = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Rank { get { return m_Rank; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Rep
        {
            get { return m_Rep; }
            set
            {
                if (value > 226250)
                {
                    m_Rank = 6;
                    m_Rep = 226250;
                }
                else if (value > 98250)
                {
                    m_Rank = 5;
                    m_Rep = value;
                }
                else if (value > 41250)
                {
                    m_Rank = 4;
                    m_Rep = value;
                }
                else if (value > 16250)
                {
                    m_Rank = 3;
                    m_Rep = value;
                }
                else if (value > 5000)
                {
                    m_Rank = 2;
                    m_Rep = value;
                }
                else
                {
                    m_Rank = 1;
                    m_Rep = value;
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Tokens { get { return m_Tokens; } set { m_Tokens = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int QuestsDone { get { return m_QuestsDone; } set { m_QuestsDone = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int QuestsTaken { get { return m_QuestsTaken; } set { m_QuestsTaken = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Reset { get { return m_Reset; } set { m_Reset = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime ResetTime { get { return m_ResetTime; } set { m_ResetTime = value; } }

        public XmlQuestGuild(ASerial serial) : base(serial)
		{
		}

		[Attachable]
        public XmlQuestGuild(string name)
		{
			Name = name;
            GuildType = name;
		}

        [Attachable]
        public XmlQuestGuild(string name, string tokenName)
        {
            Name = name;
            GuildType = name;
            TokenName = tokenName;
        }

        public override void OnAttach()
        {
            base.OnAttach();

            //CheckDuplicate(this);
        }

        public static string GetRepTitle(int rank)
        {
            switch (rank)
            {
                default:
                    return "";
                case 1:
                    return "Novice";
                case 2:
                    return "Apprentice";
                case 3:
                    return "Journeyman";
                case 4:
                    return "Expert";
                case 5:
                    return "Master";
                case 6:
                    return "Grandmaster";
            }
        }

        public static bool CheckGuildType(Mobile from, string guildType)
        {
            List<XmlAttachment> aList = XmlAttach.FindAttachments(from, typeof(XmlQuestGuild));
            //ArrayList aList = XmlAttach.FindAttachments(from, typeof(XmlQuestGuild));
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
                    if (guildType == list[i].GuildType)
                        return true;
                }
                return false;
            }
            return false;
        }

        private void CheckDuplicate(XmlQuestGuild guild)
        {
            List<XmlAttachment> aList = XmlAttach.FindAttachments(guild.AttachedTo, typeof(XmlQuestGuild));
            //ArrayList aList = XmlAttach.FindAttachments(guild.AttachedTo, typeof(XmlQuestGuild));
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
                    if (guild.GuildType == list[i].GuildType && list[i] != guild)
                        guild.Delete();
                }
            }
        }

        public static void CheckResetQuests(XmlQuestGuild guild)
        {
            if (!guild.Reset)
            {
                guild.Reset = true;

                if (DateTime.UtcNow.Hour < 6)
                    guild.ResetTime = DateTime.UtcNow;
                else
                    guild.ResetTime = DateTime.UtcNow + TimeSpan.FromDays(1);
            }

            if (DateTime.UtcNow.Hour >= 6 && DateTime.UtcNow.Day >= guild.ResetTime.Day)
            {
                guild.Reset = false;
                guild.QuestsTaken -= guild.QuestsDone;
                guild.QuestsDone = 0;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            // version 0
            writer.Write((string)m_GuildType);
            writer.Write((string)m_TokenName);
            writer.Write((int)m_Rank);
            writer.Write((int)m_Rep);
            writer.Write((int)m_Tokens);
            writer.Write((int)m_QuestsDone);
            writer.Write((int)m_QuestsTaken);
            writer.Write((bool)m_Reset);
            writer.Write((DateTime)m_ResetTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            // version 0
            m_GuildType = reader.ReadString();
            m_TokenName = reader.ReadString();
            m_Rank = reader.ReadInt();
            m_Rep = reader.ReadInt();
            m_Tokens = reader.ReadInt();
            m_QuestsDone = reader.ReadInt();
            m_QuestsTaken = reader.ReadInt();
            m_Reset = reader.ReadBool();
            m_ResetTime = reader.ReadDateTime();
        }
    }
}
