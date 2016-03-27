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

namespace Server.Engines.Help
{
    public class QualityAssurance : Gump
    {

        public QualityAssurance(Mobile from) : base( 0, 0 )
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
			AddButton(612, 337, 22150, 22151, (int)Buttons.Button0, GumpButtonType.Reply, 0);//cancel
			AddLabel(316, 157, 695, @"PLAYER FEEDBACK REPORT");
			AddLabel(350, 422, 0, @"SHADOWS EDGE");
			AddButton(521, 422, 12009, 12010, (int)Buttons.Button1, GumpButtonType.Reply, 1);//submit
			AddLabel(239, 441, 930, @"Thank You For Using Our Player Help Paging System!");
			AddLabel(198, 458, 930, @"Please Tell Us About Your Experience And Rate Our Performance");
        }

        public enum Buttons
		{
			Button0,
			Button1,
            TextEntry1,
		}

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            Account acct = (Account)from.Account;

            switch (info.ButtonID)
            {
                case (int)Buttons.Button0:
                    {
                        from.CloseGump(typeof(ReportGuild));
                        from.SendMessage("You Decide Not To Provide Player Feedback.");
                        break;
                    }

                case (int)Buttons.Button1:
                    {
                        string submit = (string)info.GetTextEntry((int)Buttons.TextEntry1).Text;

                        Console.WriteLine("");
                        Console.WriteLine("{0} From Account {1} Filed A Player Feedback Report", from.Name, acct.Username);
                        Console.WriteLine("");

                        if (!Directory.Exists("Export/Reports/")) //create directory
                            Directory.CreateDirectory("Export/Reports/");

                        using (StreamWriter op = new StreamWriter("Export/Reports/Feedback.txt", true))
                        {
                            op.WriteLine("");
                            op.WriteLine("Name Of Character: {0}, Account:{1}", from.Name, acct.Username);
                            op.WriteLine("Message: {0}", submit);
                            op.WriteLine("");
                        }

                        from.SendMessage("Your Player Feedback Report Has Been Filed! Thank You.");
                        break;
                    }
            }
        }
    }
}