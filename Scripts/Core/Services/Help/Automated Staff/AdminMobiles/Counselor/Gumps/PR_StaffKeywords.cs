using Server.Commands;
using Server.Network;

namespace Server.Gumps
{
    public class PR_StaffKeywords : Gump
    {
        public static void Initialize()
        {
            Commands.CommandSystem.Register("PR", AccessLevel.GameMaster, new CommandEventHandler(PR_OnCommand));
        }

        [Usage("PR")]
        [Description("Makes A Call To Your Custom Gump.")]
        public static void PR_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new PR_StaffKeywords());
        }

        public PR_StaffKeywords() : base( 0, 0 )
        {
            Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;

			AddPage(0);
			AddBackground(279, 111, 231, 377, 9200);
			AddItem(475, 110, 6917);
			AddItem(471, 461, 6913);
			AddImage(229, 215, 10400);
			AddButton(488, 337, 22150, 22151, 0, GumpButtonType.Reply, 0);
			AddItem(274, 460, 6914);
			AddItem(272, 111, 6916);
			AddItem(288, 357, 6920);
			AddBackground(299, 124, 199, 352, 9390);
			AddAlphaRegion(329, 164, 140, 272);
			AddHtml( 335, 171, 129, 259, @"> serverinfo
> tosagreement
> serverrules 
> meetourstaff
> showcredits
  ------------
> livesupport
  ------------
> skillcap
> skills
> statcap
> playerguide
> bestiary
  ------------
> events
> eventrequest
> hiring
> suggestion
> donations", (bool)true, (bool)true);
			AddLabel(339, 127, 695, @"STAFF KEYWORDS");
			AddLabel(347, 454, 0, @"SHADOWS EDGE");
			AddImage(478, 232, 10411);     
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0:
                    {
                        from.CloseGump(typeof(PR_StaffKeywords));
                        break;
                    }

            }
        }
    }
}