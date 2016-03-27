using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;

namespace Server.Gumps
{
    public class UnloadT2AGump : Gump
    {
        Mobile caller;

        public static void Initialize()
        {
            CommandSystem.Register("UnloadT2A", AccessLevel.Administrator, new CommandEventHandler(UnloadT2A_OnCommand));
        }

        [Usage("UnloadT2A")]
        [Description("Unload T2A with Gump.")]
        public static void UnloadT2A_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from.HasGump(typeof(UnloadT2AGump)))
                from.CloseGump(typeof(UnloadT2AGump));
            from.SendGump(new UnloadT2AGump(from));
        }

        public UnloadT2AGump(Mobile from) : this()
        {
            caller = from;
        }

		public void AddBlackAlpha( int x, int y, int width, int height )
		{
			AddImageTiled( x, y, width, height, 2624 );
			AddAlphaRegion( x, y, width, height );
		}

        public UnloadT2AGump() : base( 30, 30 )
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			AddPage(1);
			AddBackground(58, 22, 474, 540, 9200);
			AddImage(305, 306, 1418); // Castle
			AddBlackAlpha(66, 30, 458, 33);
			AddLabel(213, 37, 52, @"SELECT MAPS TO UNLOAD");
			AddBlackAlpha(66, 87, 239, 447);
			AddLabel(69, 67, 200, @"DUNGEONS");
			AddLabel(69, 112, 52, @"Britain Sewer");
			AddLabel(69, 133, 52, @"Covetous");
			AddLabel(69, 154, 52, @"Deceit");
			AddLabel(69, 174, 52, @"Despise");
			AddLabel(69, 196, 52, @"Destard");
			AddLabel(69, 217, 52, @"Fire");
			AddLabel(69, 237, 52, @"Graveyards");
			AddLabel(69, 258, 52, @"Hythloth");
			AddLabel(69, 280, 52, @"Ice");
			AddLabel(69, 310, 52, @"Shame");
			AddLabel(69, 340, 52, @"Terathan Keep");
			AddLabel(69, 360, 52, @"Trinsic Passage");
			AddLabel(69, 380, 52, @"Wrong");
			AddLabel(194, 66, 200, @"Felucca");
			AddCheck(210, 112, 210, 211, true, 202);
			AddCheck(210, 133, 210, 211, true, 203);
			AddCheck(210, 154, 210, 211, true, 204);
			AddCheck(210, 175, 210, 211, true, 205);
			AddCheck(210, 196, 210, 211, true, 206);
			AddCheck(210, 217, 210, 211, true, 207);
			AddCheck(210, 238, 210, 211, true, 208);
			AddCheck(210, 259, 210, 211, true, 209);
			AddCheck(210, 280, 210, 211, true, 210);
			AddCheck(210, 310, 210, 211, true, 219);
            AddCheck(210, 340, 210, 211, true, 221);
			AddCheck(210, 360, 210, 211, true, 224);
			AddCheck(210, 380, 210, 211, true, 227);
			AddBlackAlpha(311, 87, 213, 70);
			AddLabel(315, 67, 200, @"TOWNS");
			AddLabel(315, 91, 52, @"Animals");
			AddLabel(315, 112, 52, @"People (*)");
			AddLabel(315, 133, 52, @"Vendors");
			AddLabel(413, 66, 200, @"Felucca");
			AddCheck(429, 91, 210, 211, true, 222);
			AddCheck(429, 112, 210, 211, true, 223);
			AddCheck(429, 133, 210, 211, true, 225);
            AddBlackAlpha(311, 183, 213, 114);
			AddLabel(315, 162, 200, @"OUTDOORS");
			AddLabel(316, 187, 52, @"Animals");
			AddLabel(316, 207, 52, @"Lost Lands");
			AddLabel(316, 229, 52, @"Spawns");
			AddLabel(316, 249, 52, @"Reagents");
			AddLabel(316, 270, 52, @"Sea Life");
			AddLabel(413, 162, 200, @"Felucca");
			AddCheck(429, 187, 210, 211, true, 226);
			AddCheck(429, 208, 210, 211, true, 211);
			AddCheck(429, 229, 210, 211, true, 213);
			AddCheck(429, 250, 210, 211, true, 229);
			AddCheck(429, 271, 210, 211, true, 218);

			AddButton(320, 452, 240, 239, 1, GumpButtonType.Reply, 0); // Apply
        }

		public static void UnloadThis( Mobile from, List<int> ListSwitches, int switche )
		{
			string prefix = Server.Commands.CommandSystem.Prefix;

			if( ListSwitches.Contains( switche ) == true )
				CommandSystem.Handle( from, String.Format( "{0}Spawngen unload {1}", prefix, switche ) );
		}

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch(info.ButtonID)
            {
                case 0: //Closed or Cancel
				{
					break;
				}
				default:
				{
					// Make sure that the APPLY button was pressed
					if( info.ButtonID == 1 )
					{
						// Get the array of switches selected
						List<int> Selections = new List<int>( info.Switches );

						//FELUCCA
						// DUNGEONS
						UnloadThis(from, Selections, 201);
						UnloadThis(from, Selections, 202);
						UnloadThis(from, Selections, 203);
						UnloadThis(from, Selections, 204);
						UnloadThis(from, Selections, 205);
						UnloadThis(from, Selections, 206);
						UnloadThis(from, Selections, 207);
						UnloadThis(from, Selections, 208);
						UnloadThis(from, Selections, 209);
						UnloadThis(from, Selections, 210);
						UnloadThis(from, Selections, 228);
						UnloadThis(from, Selections, 212);
						UnloadThis(from, Selections, 214);
						UnloadThis(from, Selections, 215);
						UnloadThis(from, Selections, 216);
						UnloadThis(from, Selections, 217);
						UnloadThis(from, Selections, 219);
						UnloadThis(from, Selections, 220);
						UnloadThis(from, Selections, 221);
						UnloadThis(from, Selections, 224);
						UnloadThis(from, Selections, 227);
						//TOWNS
						UnloadThis(from, Selections, 222);
						UnloadThis(from, Selections, 223);
						UnloadThis(from, Selections, 225);
						//OUTDOORS
						UnloadThis(from, Selections, 226);
						UnloadThis(from, Selections, 211);
						UnloadThis(from, Selections, 213);
						UnloadThis(from, Selections, 229);
						UnloadThis(from, Selections, 218);
						
						from.Say( "SPAWN UNLOAD COMPLETED" );
					}
					break;
				}
            }
        }
    }
}