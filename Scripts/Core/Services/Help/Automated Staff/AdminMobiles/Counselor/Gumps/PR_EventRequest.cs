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
    public class PR_EventRequest : Gump
    {
        #region Suggestion Box Gump Configuration

        public PR_EventRequest() : base( 0, 0 )
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);
			AddBackground(161, 139, 473, 343, 9200);
			AddItem(156, 454, 6914);
			AddItem(595, 455, 6913);
			AddItem(169, 355, 6920);
			AddItem(599, 138, 6917);
			AddItem(154, 139, 6916);
			AddBackground(180, 154, 442, 289, 9390);
			AddAlphaRegion(206, 194, 390, 209);
            AddTextEntry(213, 203, 377, 192, 0, (int)Buttons.TextEntry1, @"");
			AddImage(111, 220, 10400);
			AddImage(602, 234, 10411);
            AddButton(612, 337, 22150, 22151, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			AddLabel(349, 157, 695, @"EVENT REQUEST");
			AddLabel(350, 422, 0, @"SHADOWS EDGE");
            AddButton(521, 422, 12009, 12010, (int)Buttons.Button2, GumpButtonType.Reply, 0);
			AddLabel(183, 441, 930, @"This Is Your Server And We Want To  Hear What You Have To Say!");
			AddLabel(227, 458, 930, @"Your Feedback Is Extremely Important To Us! Thank You.");
        }

        #endregion Edited By: A.A.R

        public enum Buttons
        {
            Button1,
            Button2,
            TextEntry1,
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            Account acct = (Account)from.Account;

            switch (info.ButtonID)
            {
                case (int)Buttons.Button1:
                    {
                        from.CloseGump(typeof(SuggestionBox));
                        from.SendMessage("You Decide Not To Submit An Event Request.");
                        break;
                    }

                case (int)Buttons.Button2:
                    {
                        string submit = (string)info.GetTextEntry((int)Buttons.TextEntry1).Text;

                        Console.WriteLine("");
                        Console.WriteLine("{0} From Account {1} Submitted An Event Request", from.Name, acct.Username);
                        Console.WriteLine("");

                        if (!Directory.Exists("Data/Requests")) //create directory
                            Directory.CreateDirectory("Data/Requests");

                        using (StreamWriter op = new StreamWriter("Data/Requests/Event.txt", true))
                        {
                            op.WriteLine("");
                            op.WriteLine("Name Of Character: {0}, Account:{1}", from.Name, acct.Username);
                            op.WriteLine("Message: {0}", submit);
                            op.WriteLine("");
                        }

                        from.SendMessage("Your Event Request Has Been Submitted! Thank You.");
                        break;
                    }
            }
        }
    }
}