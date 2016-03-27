using System;
using Server.Accounting;
using Server.LogConsole;

namespace Server.Misc
{
    public class AccountInit
    {
        public static void Initialize()
        {
            Utility.PushColor(ConsoleColor.White);
            ConsoleLog.Write("G.O.W: Administrator Account");
            Utility.PushColor(ConsoleColor.DarkGray);
            ConsoleLog.Write("..........................................");

            if (Accounts.Count == 0 && !Core.Service)
            {
                Utility.PushColor(ConsoleColor.DarkCyan);
                Console.WriteLine("No Account");
                Utility.PushColor(ConsoleColor.White);
                Console.Write("Do you want to create the Owner Username now? (y/n)");
                Utility.PopColor();

                if (Console.ReadKey(true).Key == ConsoleKey.Y)
                {
                    Console.WriteLine(System.Environment.NewLine);

                    Utility.PushColor(ConsoleColor.White);
                    Console.Write("Username: ");
                    string username = Console.ReadLine();

                    string password = "admin";
                    Account a = new Account(username, password);
                    a.AccessLevel = AccessLevel.Owner;
                    Utility.PushColor(ConsoleColor.Cyan);
                    ConsoleLog.WriteLine("[Created]");
                    Utility.PopColor();
                }
            }
            else
            {
                Utility.PushColor(ConsoleColor.DarkCyan);
                ConsoleLog.WriteLine("[Success]");
                Utility.PopColor();
            }
        }
    }
}