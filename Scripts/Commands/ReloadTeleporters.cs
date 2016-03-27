using System; 
using System.IO; 
using Server; 
using System.Text; 
using System.Collections.Generic; 
using System.Net; 
using Server.Mobiles; 
using Server.Network;
using Server.Commands;

namespace Server.Commands 
{ 
	public class Reloadtelepoters 
	{ 
		public static void Initialize() 
		{
            CommandSystem.Register("Reload Teleporters", AccessLevel.Administrator, new CommandEventHandler(ReloadTeleporters_OnCommand));
            CommandSystem.Register("RT", AccessLevel.Administrator, new CommandEventHandler(ReloadTeleporters_OnCommand));
		}

        [Usage("Reload Teleporters")]
        [Aliases("RT")]
        [Description("Reloading teleporter configuration paths and files")]
        private static void ReloadTeleporters_OnCommand(CommandEventArgs e) 
		{
            Mobile from = e.Mobile;

            Teleporters.Initialize();

			from.SendMessage( "Reloading teleporter configuration paths and files Done." );
		} 
	} 
}