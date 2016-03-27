using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.Engines.XmlSpawner2
{
    public class XmlAddRewardToken : XmlAttachment
    {
        private int m_DataValue;
        private string m_GuildType;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Value { get { return m_DataValue; } set { m_DataValue = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public string GuildType { get { return m_GuildType; } set { m_GuildType = value; } }

        // These are the various ways in which the message attachment can be constructed.  
        // These can be called via the [addatt interface, via scripts, via the spawner ATTACH keyword.
        // Other overloads could be defined to handle other types of arguments

        // a serial constructor is REQUIRED
        public XmlAddRewardToken(ASerial serial)
            : base(serial)
        {
        }

        [Attachable]
        public XmlAddRewardToken(int value, string guildType)
        {
            Value = value;
            GuildType = guildType;
        }


        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            // version 0
            writer.Write((int)m_DataValue);
            writer.Write((string)m_GuildType);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            // version 0
            m_DataValue = reader.ReadInt();
            m_GuildType = reader.ReadString();
        }

        public override void OnAttach()
        {
            base.OnAttach();

            if (AttachedTo is PlayerMobile)
            {
                List<XmlAttachment> aList = XmlAttach.FindAttachments(AttachedTo, typeof(XmlQuestGuild));
                //ArrayList aList = XmlAttach.FindAttachments(AttachedTo, typeof(XmlQuestGuild));
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
                            list[i].Tokens += Value;
                            ((Mobile)AttachedTo).SendMessage("You have recieved {0} {1}.", Value, list[i].TokenName);
                            break;
                        }
                    }
                }
                Delete();
            }
            else
                Delete();
        }
    }
}
