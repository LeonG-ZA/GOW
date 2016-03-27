using System;
using System.Data;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using System.Collections;
using System.Collections.Generic;
using Server.Commands.Generic;
using Server.Commands;
using Server.Gumps;
using System.Text;
using Server.Engines.Quests;
using Server.FeaturesConfiguration;

namespace Server.Engines.XmlSpawner2
{
    public class XmlGuildsGump : BaseQuestGump
    {
        public static void Initialize()
        {
            if (FeaturesConfig.FeatXMLCheckGuildsEnabled)
            CommandSystem.Register("CheckGuilds", AccessLevel.Player, new CommandEventHandler(CheckRewardTokens_OnCommand));
        }

        [Usage("CheckGuilds")]
        [Description("Reports guild statuses")]
        public static void CheckRewardTokens_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile is PlayerMobile)
                e.Mobile.SendGump(new XmlGuildsGump((PlayerMobile)e.Mobile));
        }

        private enum Buttons
        {
            Close,
            Main,
            Guild,
        }

        public enum Page
        {
            Main,
            Guild,
        }

        private PlayerMobile m_From;
        private Page m_Page;
        private string m_GuildType;
        private int m_GuildsCount;
        private List<XmlQuestGuild> m_List;

        public XmlGuildsGump(PlayerMobile from)
            : this(from, Page.Main, "None")
        {
        }

        public XmlGuildsGump(PlayerMobile from, Page page, string guildType)
            : base(75, 25)
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
                m_List = list;

            m_GuildType = guildType;
            m_Page = page;
            m_From = from;

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

            AddImage(379, 60, 0x15A9);

            AddImage(425, 0, 0x28C9);
            AddImage(90, 33, 0x232D);
            AddImageTiled(130, 65, 175, 1, 0x238D);

            switch (m_Page)
            {
                case Page.Main: PageMain(); break;
                case Page.Guild: PageGuild(); break;
            }
        }

        public void AddGuildButton(XmlGuildsGump guildsGump)
        {
            int offset = 140;

            if (m_List.Count > 0)
            {
                for (int i = 0; i < m_List.Count; i++)
                {
                    guildsGump.AddHtmlObject(98, offset, 270, 21, m_List[i].GuildType + " Guild", White, false, false);
                    AddButton(368, offset, 0x26B0, 0x26B1, 100 + i, GumpButtonType.Reply, 0);

                    offset += 21;
                    m_GuildsCount += 1;
                }
            }
        }

        public void AddGuildInfo(XmlGuildsGump guildsGump, string guildType)
        {
            //int offset = 140;

            if (m_List.Count > 0)
            {
                for (int i = 0; i < m_List.Count; i++)
                {
                    if (guildType == m_List[i].GuildType)
                    {
                        AddHtmlObject(130, 45, 270, 16, m_List[i].GuildType + " Guild", White, false, false);

                        guildsGump.AddHtmlObject(98, 161, 270, 21, "Rank : " + XmlQuestGuild.GetRepTitle(m_List[i].Rank), White, false, false);
                        guildsGump.AddHtmlObject(98, 182, 270, 21, "Standing : " + m_List[i].Rep, White, false, false);
                        guildsGump.AddHtmlObject(98, 203, 270, 21, m_List[i].TokenName + " : " + m_List[i].Tokens, White, false, false);
                    }
                }
            }
        }

        public void PageMain()
        {
            if (m_From == null)
                return;

            AddHtmlObject(130, 45, 270, 16, "Guilds", White, false, false);

            if(m_List != null)
                AddGuildButton(this);

            AddButton(313, 395, 0x2EEC, 0x2EEE, (int)Buttons.Close, GumpButtonType.Reply, 0);
        }

        public void PageGuild()
        {
            if (m_From == null)
                return;

            AddGuildInfo(this, m_GuildType);

            AddButton(98, 395, 0x2EEF, 0x2EF1, (int)Buttons.Main, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (m_From != null)
                m_From.CloseGump(typeof(XmlGuildsGump));

            switch (info.ButtonID)
            {
                // close quest list
                case (int)Buttons.Close:
                    break;
                case (int)Buttons.Main:
                    m_From.SendGump(new XmlGuildsGump(m_From, Page.Main, "None"));
                    break;
                default:
                    for (int i = 0; i < m_List.Count; i++)
                    {
                        if (info.ButtonID == 100 + i)
                        {
                            m_From.SendGump(new XmlGuildsGump(m_From, Page.Guild, m_List[i].GuildType));
                            break;
                        }
                    }

                    //m_From.SendGump(new XmlGuildsGump(m_From, Page.Main, "None"));
                    break;
            }
        }
    }
}
