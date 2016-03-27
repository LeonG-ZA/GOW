using System;
using Server.LogConsole;

namespace Server.Misc
{
    public class SCVersion
    {
        public static void Initialize()
        {
            Utility.PushColor(ConsoleColor.White);
            ConsoleLog.Write("G.O.W: Script Version");
            Utility.PushColor(ConsoleColor.DarkGray);
            ConsoleLog.Write(".................................................");
            Utility.PushColor(ConsoleColor.DarkCyan);
            ConsoleLog.WriteLine("[4.6.0.0]");// Release.BetaStage2.BetaStage1.Alpha
            Utility.PopColor();
        }
    }
}