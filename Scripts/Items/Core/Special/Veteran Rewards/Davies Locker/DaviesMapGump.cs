using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Prompts;
using Server.Targeting;

namespace Server.Gumps
{
	public class DaviesMapGump : Gump
	{
		public string GetName( string name )
		{
			if ( name == null || (name = name.Trim()).Length <= 0 )
				return "(indescript)";

			return name;
		}

		public DaviesMapGump( Mobile from, int xx, int yy ) : base( 25, 25 )
		{

			double dx = xx/13.06;
			double dy = yy/10.9;
			int x = (int)dx + 50;
			int y = (int)dy + 65;

			Resizable = true;
			Resizable=false;
			AddPage(0);
			AddBackground(52, 25, 393, 430, 5120);
			AddImage(59, 63, 5528);

			AddItem(x, y, 575);
			AddImage(60, 65, 2360);
			AddImage(430, 65, 2360);
			AddImage(430, 435, 2360);
			AddImage(60, 435, 2360);
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			return;
		}
	}
}