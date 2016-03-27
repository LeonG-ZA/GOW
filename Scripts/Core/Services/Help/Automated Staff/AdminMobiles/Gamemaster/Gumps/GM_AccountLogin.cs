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
    public class AccountLogin : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register("AccountLogin", AccessLevel.Player, new CommandEventHandler(AccountLogin_OnCommand));
        }

        private static void AccountLogin_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new AccountLogin(e.Mobile));
        }

        #region AccountLogin Gump Configuration

        public AccountLogin(Mobile owner): base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);

            AddBackground(161, 153, 473, 289, 9200);
            AddItem(156, 415, 6914);
            AddItem(595, 416, 6913);
            AddItem(169, 355, 6920);
            AddItem(599, 153, 6917);
            AddItem(154, 154, 6916);
            AddBackground(196, 182, 412, 235, 9270);
            AddImage(111, 220, 10400);
            AddImage(602, 234, 10411);
            AddButton(612, 337, 22150, 22151, 0, GumpButtonType.Reply, 0);
            AddAlphaRegion(196, 181, 411, 237);
            AddBackground(210, 196, 385, 207, 9270);
            AddLabel(288, 217, 1160, @"SERVER AND ACCOUNT INFORMATION");
            AddImageTiled(334, 266, 212, 25, 9304);
            AddImageTiled(334, 307, 212, 25, 9304);
            AddLabel(252, 269, 195, @"Username:");
            AddLabel(252, 309, 195, @"Password:");
            AddButton(472, 366, 12010, 12009, 1, GumpButtonType.Reply, 1);
            AddImage(254, 366, 2092);
            AddTextEntry(334, 266, 212, 25, 0, 1, @"");
            AddTextEntry(334, 307, 212, 25, 0, 2, @"");         
        }

        #endregion Edited By: A.A.R

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (info.ButtonID == 1)
            {
                Mobile from = state.Mobile;
                Account acct = (Account)from.Account;

                string user = (string)info.GetTextEntry(1).Text;
                string pass = (string)info.GetTextEntry(2).Text;

                if (user == acct.Username && acct.CheckPassword(pass))
                {
                    from.SendMessage(64, "Login Confirmed.");
                    from.SendGump(new AccountInfo(from));
                }
                else
                {
                    from.SendMessage(38, "Either the username or password you entered was incorrect, Please recheck your spelling and remember that passwords and usernames are case sensitive. Please try again.");
                }
            }
        }
    }
}


