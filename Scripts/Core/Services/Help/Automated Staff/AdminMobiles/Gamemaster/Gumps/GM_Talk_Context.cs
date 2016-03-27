using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Items;
using Server.ContextMenus;
using Server.Multis;
using Server.Spells;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Accounting;
using Server.Misc;
using Server.Network;

namespace Server.Gumps
{
    public class GameMaster_Talk : Gump
    {
        private TimeSpan m_UseDelay = TimeSpan.FromDays(30); //Set To 30 Days

        public static void Initialize()
        {
            Commands.CommandSystem.Register("GMTalk", AccessLevel.GameMaster, new CommandEventHandler(GMTalk_OnCommand));
        }

        [Usage("GMTalk")]
        [Description("Makes A Call To Your Custom Gump.")]
        public static void GMTalk_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new GameMaster_Talk());
        }

        #region GM_Talk_Context Gump Configuration

        public GameMaster_Talk(): base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(161, 154, 473, 289, 9200);
            AddItem(156, 415, 6914);
            AddItem(595, 416, 6913);
            AddItem(169, 355, 6920);
            AddItem(599, 153, 6917);
            AddItem(154, 154, 6916);
            AddBackground(196, 182, 412, 235, 9270);
            AddImage(111, 220, 10400);
            AddImage(602, 234, 10411);
            AddButton(612, 337, 22150, 22151, (int)Buttons.Button1, GumpButtonType.Reply, 1);
            AddAlphaRegion(196, 181, 411, 237);
            AddBackground(210, 196, 385, 207, 9270);
            AddLabel(302, 218, 195, @"SHADOWS EDGE ( 2000 - 2011 )");
            AddLabel(239, 245, 694, @"We're Very Excited That You've Chosen To Be A Part");
            AddLabel(242, 265, 694, @"Of Our Community! As A Token Of Our Appreciation");
            AddLabel(239, 285, 694, @"We'd Like To Offer You A Monthly Promotional Gift!");
            AddButton(386, 340, 4037, 4036, (int)Buttons.Button2, GumpButtonType.Reply, 2);
            AddLabel(320, 312, 695, @"Double Click The Grab Bag!");
            AddItem(210, 339, 2567);
            AddItem(551, 339, 2573);
        }

        #endregion Edited By: A.A.R

        public enum Buttons
        {
            Button1,
            Button2,
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            Account acct = (Account)from.Account;

            switch (info.ButtonID)
            {
                case (int)Buttons.Button1:
                    {
                        from.CloseGump(typeof(GameMaster_Talk));
                        break;
                    }

                case (int)Buttons.Button2:
                    {
                        TimeSpan m_UseDelay = TimeSpan.FromDays(30); //Set To 30 Days

                        if (from.Backpack != null)
                        {
                            PlayerMobile pm = (PlayerMobile)from;

                            if (pm.AccessLevel == AccessLevel.Player)
                            {
                                from.SendMessage("Sorry Only Staff Can Recieve Rewards Here");
                            }

                            if (pm.AccessLevel >= AccessLevel.Counselor)

                                if (DateTime.UtcNow >= pm.PromoGiftLast + m_UseDelay)
                                {
                                    from.AddToBackpack(new PromotionalDeed_GM());
                                    from.CloseGump(typeof(GameMaster_Talk));

                                    Console.WriteLine("");
                                    Console.WriteLine("{0} From Account {1} Has Just Redeemed A Monthly Staff Server Gift", from.Name, acct.Username);
                                    Console.WriteLine("");

                                    from.SendMessage("Check Back Every Thirty (30) Days To Obtain New Promotional Offers");

                                    pm.PromoGiftLast = DateTime.UtcNow;
                                }
                                else
                                {
                                    from.SendMessage("You May Only Obtain One (1) Promotional Gift Every Thirty (30) Days");
                                }
                        }
                        break;
                    }
            }
        }
    }
}