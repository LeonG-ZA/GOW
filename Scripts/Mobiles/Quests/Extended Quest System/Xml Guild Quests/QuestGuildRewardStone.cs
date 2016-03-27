using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Misc;
using Server.Engines.BulkOrders;
using Server.Regions;
using Server.Factions;
using Server.Engines.XmlSpawner2;
using System.CustomizableVendor;

namespace Server.Engines.Quests
{
    public class QuestGuildRewardStone : StoneRewardVendor
    {
        private string m_GuildType;
        private int m_Rank;

        [CommandProperty(AccessLevel.GameMaster)]
        public string GuildType { get { return m_GuildType; } set { m_GuildType = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Rank { get { return m_Rank; } set { m_Rank = value; InvalidateProperties(); } }

        [Constructable]
        public QuestGuildRewardStone()
            : base()
        {
        }

        public QuestGuildRewardStone( Serial serial ) : base( serial )
		{
		}

        public override void OnDoubleClick(Mobile m)
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
                for (int i = 0; i < list.Count; i++)
                {
                    if (m_GuildType == list[i].GuildType)
                    {
                        if (list[i].Rank >= m_Rank)
                            base.OnDoubleClick(m);
                        else
                            m.SendMessage("You require to be at least a {0} in the {1} Guild to purhcase any of these rewards.", XmlQuestGuild.GetRepTitle(m_Rank), m_GuildType);
                    }
                }
            }
            else
                m.SendMessage("You require to be at least a {0} in the {1} Guild to purhcase any of these rewards.", XmlQuestGuild.GetRepTitle(m_Rank), m_GuildType);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1070722, "Requires: " + XmlQuestGuild.GetRepTitle(m_Rank) + " rank with " + m_GuildType + " Guild.");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            // version 0
            writer.Write((string)m_GuildType);
            writer.Write((int)m_Rank);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            // version 0
            m_GuildType = reader.ReadString();
            m_Rank = reader.ReadInt();
        }
    }
}
