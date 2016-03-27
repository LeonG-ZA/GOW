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
    public class ReportDefect : Gump
    {
        public static void Initialize()
        {
            Commands.CommandSystem.Register("ReportDefect", AccessLevel.GameMaster, new CommandEventHandler(ReportDefect_OnCommand));
        }

        [Usage("ReportDefect")]
        [Description("Makes A Call To Your Custom Gump.")]
        public static void ReportDefect_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new ReportDefect());
        }

        #region ReportDefect Gump Configuration

        public ReportDefect() : base( 0, 0 )
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
			AddLabel(343, 157, 695, @"REPORT A DEFECT");
			AddLabel(350, 422, 0, @"SHADOWS EDGE");
			AddButton(521, 422, 12009, 12010, (int)Buttons.Button2, GumpButtonType.Reply, 2);
			AddLabel(211, 441, 930, @"Defect Reports Help Us Fix Our Detail Oriented Game World");
			AddLabel(226, 458, 930, @"Without These Reports There Is Very Little We Can Do");
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
                        from.CloseGump(typeof(ReportDefect));
                        from.SendMessage("You Decide Not To File A Defect Report.");
                        break;
                    }

                case (int)Buttons.Button2:
                    {
                        string submit = (string)info.GetTextEntry((int)Buttons.TextEntry1).Text;

                        Console.WriteLine("");
                        Console.WriteLine("{0} From Account {1} Filed A Defect Report", from.Name, acct.Username);
                        Console.WriteLine("");

                        if (!Directory.Exists("Data/Reports/")) //create directory
                            Directory.CreateDirectory("Data/Reports/");

                        using (StreamWriter op = new StreamWriter("Data/Reports/Defect.txt", true))
                        {
                            op.WriteLine("");
                            op.WriteLine("Name Of Character: {0}, Account:{1}", from.Name, acct.Username);
                            op.WriteLine("Message: {0}", submit);
                            op.WriteLine("");
                        }

                        from.SendMessage("Your Defect Report Has Been Filed! Thank You.");
                        break;
                    }
            }
        }
    }
}