using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Accounting;
using Server.Mobiles;

namespace Server.Gumps
{
	public class BodyPickerGump : Server.Gumps.Gump
	{
		public static void Initialize()
		{
			CommandSystem.Register( "BodyPicker", AccessLevel.GameMaster, new CommandEventHandler( ForceSpawnDailys_OnCommand ) );		
		}

		[Usage( "BodyPicker" )]
		[Description( "Allows you to scroll through Body Ids" )]
		private static void ForceSpawnDailys_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;
	        from.CloseGump(typeof(BodyPickerGump));
            from.SendGump(new BodyPickerGump(from));          
		}
		
		public TextRelay BodyID;			
		public BodyPickerGump(Mobile from) : base(0, 0)
		{
			int CurrentBody; 
			
			CurrentBody = from.Body; 
			
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			AddPage(0);
			
			AddBackground(31, 65, 300, 180, 9270);
			AddLabel(85, 75, 2025, @"Body Picker for 2d Clients:");

			AddLabel(95, 110, 1153, @"Body:");
			AddBackground(140, 110, 40, 20, 9300);
			AddTextEntry(140, 110, 40, 20, 1153, (int)Buttons.BodyTxt, "" + CurrentBody + "");
			
			AddLabel(130, 150, 1153, @"Previous");
			AddButton(140, 170, 5537, 5539, 1, GumpButtonType.Reply, 0); //Previous Body Type
			
			AddLabel(190, 150, 1153, @"Next");
			AddButton(200, 170, 5540, 5542, 2, GumpButtonType.Reply, 0); //Next Body Type
			
			AddButton(190, 110, 4023, 4024, 3, GumpButtonType.Reply, 0); // Okay to input Type
			
			AddButton(220, 110, 4020, 4021, 4, GumpButtonType.Reply, 0); // Cancel/Close
		}
		
			
		public enum Buttons
		{
			BodyTxt = 1,
			OkBtn,
			CancelBtn
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
		
			 Mobile from = sender.Mobile;
			
			switch( info.ButtonID )
			{
				case 1:
				{
					BodyID = info.GetTextEntry(1);
					int BodyVal;
				
					try
					{
						BodyVal = Convert.ToInt32(BodyID.Text);
					}
					catch
					{
						from.SendMessage( "Numbers only, please try again." );
						break;
					}
					
						from.Body = BodyVal - 1;
						from.SendGump(new BodyPickerGump(from));
						break;
				}
				
				case 2:
				{
					BodyID = info.GetTextEntry(1);
					int BodyVal;
				
					try
					{
						BodyVal = Convert.ToInt32(BodyID.Text);
					}
					catch
					{
						from.SendMessage( "Numbers only, please try again." );
						break;
					}
					
						from.Body = BodyVal + 1;
						from.SendGump(new BodyPickerGump(from));
						break;
				}
				case 3:
				{
					BodyID = info.GetTextEntry(1);
					int BodyVal;
				
					try
					{
						BodyVal = Convert.ToInt32(BodyID.Text);
					}
					catch
					{
						from.SendMessage( "Numbers only, please try again." );
						break;
					}
					
						from.Body = BodyVal;
						from.SendGump(new BodyPickerGump(from));
						break;
				}
				case 4:
				{
					from.CloseGump( typeof( BodyPickerGump) );
					break;
				}
			}			
		}
	}
}