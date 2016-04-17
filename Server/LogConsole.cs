using System;
using System.IO;

namespace Server.LogConsole
{
    public static class GOWLoggingConfig
    {
        public static bool GOWConsoleBeep = true;
        public static bool ConsoleLoggingEnabled = true;
        public static bool ErrorLoggingEnabled = true;
        public static bool ErrorConsoleEnabled = true;
        public static bool PlayerLoggingEnabled = true;
        public static bool PlayerConsoleEnabled = true;
        public static bool TeleportSystemEnabled = true;
        public static bool NewRegionSystemEnabled = true;
        public static bool VNCSupportEnabled = true;

        public static void Load()
        {
            string path = Path.Combine(Core.BaseDirectory, "Data/GOW.cfg");

            if (!File.Exists(path))
            {
                return;
            }

            using (StreamReader ip = new StreamReader(path))
            {
                string line;

                while ((line = ip.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.Length == 0 || line.StartsWith("#"))
                        continue;

                    try
                    {
                        string[] split = line.Split(' ');//\t

                        if (split.Length >= 2)
                        {
                            switch(split[0])
                            {
                                case "GOWConsoleBeep":
                                    {
                                        if (split[1] == "false")
                                        {
                                            GOWConsoleBeep = false;
                                        }
                                        else
                                        {
                                            GOWConsoleBeep = true;
                                        }
                                    }
                                    break;
                                case "ConsoleLoggingEnabled":
                                    {
                                        if (split[1] == "false") ConsoleLoggingEnabled = false;
                                        else ConsoleLoggingEnabled = true;
                                    }
                                    break;
                                case "ErrorLoggingEnabled":
                                    {
                                        if (split[1] == "false") ErrorLoggingEnabled = false;
                                        else ErrorLoggingEnabled = true;
                                    }
                                    break;
                                case "ErrorConsoleEnabled":
                                    {
                                        if (split[1] == "false") ErrorConsoleEnabled = false;
                                        else ErrorConsoleEnabled = true;
                                    }
                                    break;
                                case "PlayerLoggingEnabled":
                                    {
                                        if (split[1] == "false") PlayerLoggingEnabled = false;
                                        else PlayerLoggingEnabled = true;
                                    }
                                    break;
                                case "PlayerConsoleEnabled":
                                    {
                                        if (split[1] == "false") PlayerConsoleEnabled = false;
                                        else PlayerConsoleEnabled = true;
                                    }
                                    break;
                                case "TeleportSystemEnabled":
                                    {
                                        if (split[1] == "false") TeleportSystemEnabled = false;
                                        else TeleportSystemEnabled = true;
                                    }
                                    break;
                                case "NewRegionSystemEnabled":
                                    {
                                        if (split[1] == "false") TeleportSystemEnabled = false;
                                        else TeleportSystemEnabled = true;
                                    }
                                    break;
                                case "VNCSupportEnabled":
                                    {
                                        if (split[1] == "false") TeleportSystemEnabled = false;
                                        else TeleportSystemEnabled = true;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
    }

    public class ConsoleLog
    {
        private static StreamWriter m_Output;
        private static bool m_Enabled = GOWLoggingConfig.ConsoleLoggingEnabled;

        public static bool Enabled
        {
            get
            {
                return m_Enabled;
            }
            set
            {
                m_Enabled = value;
            }
        }
        public static StreamWriter Output
        {
            get
            {
                return m_Output;
            }
        }

        public static void Initialize()
        {
            if (!m_Enabled)
            {
                return;
            }
            CheckDirectory();
            try
            {
                m_Output = new StreamWriter("Logs/Console.log", true);
                m_Output.AutoFlush = true;
                m_Output.WriteLine("Log started on {0}", DateTime.UtcNow);
                m_Output.WriteLine();
            }
            catch
            {
                Utility.PushColor(ConsoleColor.Red);
                Console.WriteLine("GOW: ConsoleLogging failed to initialize LogWriter.");
                Console.WriteLine();
                Utility.PopColor();
                m_Enabled = false;
            }
        }

        private static void CheckDirectory()
        {
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
        }

        public static void Write(char ch)
        {
            Console.Write(ch);

            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write("{0}", ch);
                }
                catch
                {
                }
            }
        }

        public static void Write(object value)
        {
            Console.Write(value);
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write("{0}", value);
                }
                catch
                {
                }
            }
        }

        public static void Write(string str)
        {
            Console.Write(str);
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write("{0}", str);
                }
                catch
                {
                }
            }
        }

        public static void Write(string format, params object[] args)
        {
            Console.Write(format, args);
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write(string.Format(format, args));
                }
                catch
                {
                }
            }
        }

        public static void WriteLine(string text)
        {
            Console.WriteLine(text);
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.WriteLine("{0}", text);
                }
                catch
                {
                }
            }
        }

        public static void WriteLine(object value)
        {
            Console.WriteLine(value);
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.WriteLine("{0}", value);
                }
                catch
                {
                }
            }
        }

        public static void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.WriteLine(string.Format(format, args));
                }
                catch
                {
                }
            }
        }

        public static void WriteConsole(char ch)
        {
            Console.Write(ch);
        }

        public static void WriteConsole(object value)
        {
            Console.Write(value);
        }

        public static void WriteConsole(string str)
        {
            Console.Write(str);
        }

        public static void WriteConsole(string format, params object[] args)
        {
            Console.Write(format, args);
        }

        public static void WriteLineConsole(string text)
        {
            Console.WriteLine(text);
        }

        public static void WriteLineConsole(object value)
        {
            Console.WriteLine(value);
        }

        public static void WriteLineConsole(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public static void WriteLog(char ch)
        {
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write("{0}", ch);
                }
                catch
                {
                }
            }
        }

        public static void WriteLog(object value)
        {
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write("{0}", value);
                }
                catch
                {
                }
            }
        }

        public static void WriteLog(string str)
        {
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write("{0}", str);
                }
                catch
                {
                }
            }
        }

        public static void WriteLog(string format, params object[] args)
        {
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write(String.Format(format, args));
                }
                catch
                {
                }
            }
        }

        public static void WriteLineLog(string text)
        {
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.WriteLine("{0}", text);
                }
                catch
                {
                }
            }
        }

        public static void WriteLineLog(object value)
        {
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.WriteLine("{0}", value);
                }
                catch
                {
                }
            }
        }

        public static void WriteLineLog(string format, params object[] args)
        {
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.WriteLine(String.Format(format, args));
                }
                catch
                {
                }
            }
        }
    }

    public class ErrorLog
    {
        private static StreamWriter m_Output;
        private static bool m_Enabled = GOWLoggingConfig.ErrorLoggingEnabled;

        public static bool Enabled
        {
            get
            {
                return m_Enabled;
            }
            set
            {
                m_Enabled = value;
            }
        }
        public static StreamWriter Output
        {
            get
            {
                return m_Output;
            }
        }

        public static void Initialize()
        {
            if (!m_Enabled)
            {
                return;
            }
            CheckDirectory();
            try
            {
                m_Output = new StreamWriter("Logs/Error.log", true);
                m_Output.AutoFlush = true;
                m_Output.WriteLine("Log started on {0}", DateTime.UtcNow);
                m_Output.WriteLine();
            }
            catch
            {
                Utility.PushColor(ConsoleColor.Red);
                Console.WriteLine("G.O.W: Error Logging failed to initialize LogWriter.");
                Console.WriteLine();
                Utility.PopColor();
                m_Enabled = false;
            }
        }

        private string Timestamp
        {
            get
            {
                return String.Format("{0:D2}:{1:D2}:{2:D2} ERROR: ", DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second);
            }
        }

        private static void CheckDirectory()
        {
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
        }

        //    value = this.Timestamp + value;

        public static void Write(char ch)
        {
            if (GOWLoggingConfig.ErrorConsoleEnabled)
            {
                Utility.PushColor(ConsoleColor.Red);
                Console.Write(ch);
                Utility.PopColor();
            }
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write("{0}", ch);
                }
                catch
                {
                }
            }
        }

        public static void Write(object value)
        {
            if (GOWLoggingConfig.ErrorConsoleEnabled)
            {
                Utility.PushColor(ConsoleColor.Red);
                Console.Write(value);
                Utility.PopColor();
            }
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write("{0}", value);
                }
                catch
                {
                }
            }
        }

        public static void Write(string str)
        {
            if (GOWLoggingConfig.ErrorConsoleEnabled)
            {
                Utility.PushColor(ConsoleColor.Red);
                Console.Write(str);
                Utility.PopColor();
            }
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write("{0}", str);
                }
                catch
                {
                }
            }
        }

        public static void Write(string format, params object[] args)
        {
            if (GOWLoggingConfig.ErrorConsoleEnabled)
            {
                Utility.PushColor(ConsoleColor.Red);
                Console.Write(format, args);
                Utility.PopColor();
            }
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write(String.Format(format, args));
                }
                catch
                {
                }
            }
        }

        public static void WriteLine(string text)
        {
            if (GOWLoggingConfig.ErrorConsoleEnabled)
            {
                Utility.PushColor(ConsoleColor.Red);
                Console.WriteLine(text);
                Utility.PopColor();
            }
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.WriteLine("{0}", text);
                }
                catch
                {
                }
            }
        }

        public static void WriteLine(object value)
        {
            if (GOWLoggingConfig.ErrorConsoleEnabled)
            {
                Utility.PushColor(ConsoleColor.Red);
                Console.WriteLine(value);
                Utility.PopColor();
            }
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.WriteLine("{0}", value);
                }
                catch
                {
                }
            }
        }

        public static void WriteLine(string format, params object[] args)
        {
            if (GOWLoggingConfig.ErrorConsoleEnabled)
            {
                Utility.PushColor(ConsoleColor.Red);
                Console.WriteLine(format, args);
                Utility.PopColor();
            }
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.WriteLine(String.Format(format, args));
                }
                catch
                {
                }
            }
        }
        
        public static void WriteWarning(char ch)
        {
            if (GOWLoggingConfig.ErrorConsoleEnabled)
            {
                Utility.PushColor(ConsoleColor.Yellow);
                Console.Write(ch);
                Utility.PopColor();
            }
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write("{0}", ch);
                }
                catch
                {
                }
            }
        }

        public static void WriteWarning(object value)
        {
            if (GOWLoggingConfig.ErrorConsoleEnabled)
            {
                Utility.PushColor(ConsoleColor.Yellow);
                Console.Write(value);
                Utility.PopColor();
            }
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write("{0}", value);
                }
                catch
                {
                }
            }
        }

        public static void WriteWarning(string str)
        {
            if (GOWLoggingConfig.ErrorConsoleEnabled)
            {
                Utility.PushColor(ConsoleColor.Yellow);
                Console.Write(str);
                Utility.PopColor();
            }
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write("{0}", str);
                }
                catch
                {
                }
            }
        }

        public static void WriteWarning(string format, params object[] args)
        {
            if (GOWLoggingConfig.ErrorConsoleEnabled)
            {
                Utility.PushColor(ConsoleColor.Yellow);
                Console.Write(format, args);
                Utility.PopColor();
            }
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write(String.Format(format, args));
                }
                catch
                {
                }
            }
        }

        public static void WriteLineWarning(string text)
        {
            if (GOWLoggingConfig.ErrorConsoleEnabled)
            {
                Utility.PushColor(ConsoleColor.Yellow);
                Console.WriteLine(text);
                Utility.PopColor();
            }
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.WriteLine("{0}", text);
                }
                catch
                {
                }
            }
        }

        public static void WriteLineWarning(object value)
        {
            if (GOWLoggingConfig.ErrorConsoleEnabled)
            {
                Utility.PushColor(ConsoleColor.Yellow);
                Console.WriteLine(value);
                Utility.PopColor();
            }
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.WriteLine("{0}", value);
                }
                catch
                {
                }
            }
        }

        public static void WriteLineWarning(string format, params object[] args)
        {
            if (GOWLoggingConfig.ErrorConsoleEnabled)
            {
                Utility.PushColor(ConsoleColor.Yellow);
                Console.WriteLine(format, args);
                Utility.PopColor();
            }
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.WriteLine(String.Format(format, args));
                }
                catch
                {
                }
            }
        }
    }    

    // ADD IF PLAYER TO CONSOLE THEN CONSOLE MESSAGE INCLUDED
    public class PlayerLog
    {
        private static StreamWriter m_Output;

        private static bool m_Enabled = GOWLoggingConfig.PlayerLoggingEnabled;

        public static bool Enabled
        {
            get
            {
                return m_Enabled;
            }
            set
            {
                m_Enabled = value;
            }
        }
        public static StreamWriter Output
        {
            get
            {
                return m_Output;
            }
        }

        public static void Initialize()
        {
            if (!m_Enabled)
            {
                return;
            }
            CheckDirectory();
            try
            {
                m_Output = new StreamWriter("Logs/Player.log", true);
                m_Output.AutoFlush = true;
                m_Output.WriteLine("Log started on {0}", DateTime.UtcNow);
                m_Output.WriteLine();
            }
            catch
            {
                Utility.PushColor(ConsoleColor.Red);
                Console.WriteLine("GOW: Player Logging failed to initialize LogWriter.");
                Console.WriteLine();
                Utility.PopColor();
                m_Enabled = false;
            }
        }

        private static void CheckDirectory()
        {
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
        }

        public static void Write(char ch)
        {
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write("{0}", ch);
                }
                catch
                {
                }
            }
        }

        public static void Write(object value)
        {
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write("{0}", value);
                }
                catch
                {
                }
            }
        }

        public static void Write(string str)
        {
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write("{0}", str);
                }
                catch
                {
                }
            }
        }

        public static void Write(string format, params object[] args)
        {
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.Write(String.Format(format, args));
                }
                catch
                {
                }
            }
        }

        public static void WriteLine(string text)
        {
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.WriteLine("{0}", text);
                }
                catch
                {
                }
            }
        }

        public static void WriteLine(object value)
        {
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.WriteLine("{0}", value);
                }
                catch
                {
                }
            }
        }

        public static void WriteLine(string format, params object[] args)
        {
            if (m_Enabled)
            {
                CheckDirectory();
                try
                {
                    m_Output.WriteLine(String.Format(format, args));
                }
                catch
                {
                }
            }
        }
    }
}