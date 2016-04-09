using System;
using System.IO;
using Server.Commands;
using Server.Accounting;
using Server.Network;

namespace Server.Gumps
{
    public class SuggestionBox : Gump
    {
        public static void Initialize()
        {
            Commands.CommandSystem.Register("SuggestionBox", AccessLevel.GameMaster, new CommandEventHandler(SuggestionBox_OnCommand));
        }

        [Usage("SuggestionBox")]
        [Description("Makes A Call To Your Custom Gump.")]
        public static void SuggestionBox_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new SuggestionBox());
        }

        public SuggestionBox() : base( 0, 0 )
        {
            Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;

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
			AddLabel(345, 157, 695, @"SUGGESTION BOX");
			AddLabel(350, 422, 0, @"SHADOWS EDGE");
            AddButton(521, 422, 12009, 12010, (int)Buttons.Button2, GumpButtonType.Reply, 0);
			AddLabel(183, 441, 930, @"This Is Your Server And We Want To  Hear What You Have To Say!");
			AddLabel(228, 458, 930, @"Your Feedback Is Extremely Important To Us! Thank You.");
        }

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
                        from.SendMessage("You Decide Not To Submit A Suggestion.");
                        break;
                    }

                case (int)Buttons.Button2:
                    {
                        string submit = (string)info.GetTextEntry((int)Buttons.TextEntry1).Text;

                        Console.WriteLine("");
                        Console.WriteLine("{0} From Account {1} Submitted A Suggestion", from.Name, acct.Username);
                        Console.WriteLine("");

                        if (!Directory.Exists("Data/Suggestions")) //create directory
                            Directory.CreateDirectory("Data/Suggestions");

                        using (StreamWriter op = new StreamWriter("Data/Suggestions/Suggestion.txt", true))
                        {
                            op.WriteLine("");
                            op.WriteLine("Name Of Character: {0}, Account:{1}", from.Name, acct.Username);
                            op.WriteLine("Message: {0}", submit);
                            op.WriteLine("");
                        }

                        from.SendMessage("Your Suggestion Has Been Submitted! Thank You.");
                        break;
                    }
            }
        }
    }
}