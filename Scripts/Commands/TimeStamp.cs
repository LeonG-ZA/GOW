using Server.FeaturesConfiguration;
using System;

namespace Server.Commands
{
    public class TimeStamp
    {
        public static void Initialize()
        {
            if (FeaturesConfig.FeatPlayerTimeStampEnabled)
            {
                CommandSystem.Register("TimeStamp", AccessLevel.Player, new CommandEventHandler(CheckTime_OnCommand));
            }
            else
            {
                CommandSystem.Register("TimeStamp", AccessLevel.Counselor, new CommandEventHandler(CheckTime_OnCommand));
            }
        }

        [Usage("TimeStamp")]
        [Description("Check's Your Servers Current Date And Time")]
        public static void CheckTime_OnCommand(CommandEventArgs e)
        {
            Mobile m = e.Mobile;
            DateTime now = DateTime.UtcNow;
            m.SendMessage("The Current Date And Time Is " + now + "(EST)");         
        }
    }
}