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
    public class GM_StaffKeywords : Gump
    {
        public static void Initialize()
        {
            Commands.CommandSystem.Register("GM", AccessLevel.GameMaster, new CommandEventHandler(GM_OnCommand));
        }

        [Usage("GM")]
        [Description("Makes A Call To Your Custom Gump.")]
        public static void GM_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new GM_StaffKeywords());
        }

        #region StaffKeywords Gump Configuration
       
        public GM_StaffKeywords() : base( 0, 0 )
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

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
> reportplayer
> reportlag
> reportguild
> reportdefect
> reportadmin
> livesupport
  ------------
> teleportme
> relocateme
> retrievebody
> retrievepets
> accounthelp", (bool)true, (bool)true);
			AddLabel(339, 127, 695, @"STAFF KEYWORDS");
			AddLabel(347, 454, 0, @"SHADOWS EDGE");
			AddImage(478, 232, 10411);       
        }

        #endregion Edited By: A.A.R

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 0:
                    {
                        from.CloseGump(typeof(GM_StaffKeywords));
                        break;
                    }

            }
        }
    }
}