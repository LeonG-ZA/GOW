using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CustomsFramework;
using Server.Network;
//using System.Collections;
using Server.LogConsole;
/*
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Server;
using Server.Accounting;
using Server.Gumps;
using Server.Network;
using System.Runtime;
using CustomsFramework;
 */

namespace Server
{
    public delegate void Slice();

    public static class Core
    {
        static Core()
        {
            DataDirectories = new List<string>();

            GlobalMaxUpdateRange = 24;
            GlobalUpdateRange = 18;
        }

        private static bool _Crashed;
        private static Thread _TimerThread;
        private static string _BaseDirectory;
        private static string _ExePath;

        private static bool _Cache = true;

        private static bool _Profiling;
        private static DateTime _ProfileStart;
        private static TimeSpan _ProfileTime;

        public static MessagePump MessagePump { get; set; }

        public static Slice Slice;

        public static bool Profiling
        {
            get { return _Profiling; }
            set
            {
                if (_Profiling == value)
                    return;

                _Profiling = value;

                if (_ProfileStart > DateTime.MinValue)
                    _ProfileTime += DateTime.UtcNow - _ProfileStart;

                _ProfileStart = (_Profiling ? DateTime.UtcNow : DateTime.MinValue);
            }
        }

        public static TimeSpan ProfileTime
        {
            get
            {
                if (_ProfileStart > DateTime.MinValue)
                    return _ProfileTime + (DateTime.UtcNow - _ProfileStart);

                return _ProfileTime;
            }
        }

        public static bool Service { get; private set; }
        public static bool Debug { get; private set; }

        public static bool HaltOnWarning { get; private set; }
        public static bool VBdotNet { get; private set; }

        public static List<string> DataDirectories { get; private set; }

        public static Assembly Assembly { get; set; }

        public static Version Version { get { return Assembly.GetName().Version; } }

        public static Process Process { get; private set; }
        public static Thread Thread { get; private set; }

        public static MultiTextWriter MultiConsoleOut { get; private set; }

        /* DateTime.Now and DateTime.UtcNow are based on actual system clock time.
         * The resolution is acceptable but large clock jumps are possible and cause issues.
         * GetTickCount and GetTickCount64 have poor resolution.
         * GetTickCount64 is unavailable on Windows XP and Windows Server 2003.
         * Stopwatch.GetTimestamp() (QueryPerformanceCounter) is high resolution, but
         * somewhat expensive to call and unreliable with certain system configurations.
         */

        /* The following implementation contains an effective substitute for GetTickCount64 that
         * is reliable as long as it is retrieved once every 2^32 ms (~49 days).
         */

        /* We don't really need this, but it may be useful in the future.
        private static ThreadLocal<long> _HighOrder = new ThreadLocal<long>();
        private static ThreadLocal<uint> _LastTickCount = new ThreadLocal<uint>();
        */

        private static readonly bool _HighRes = Stopwatch.IsHighResolution;

        private static readonly double _HighFrequency = 1000.0 / Stopwatch.Frequency;
        private const double _LowFrequency = 1000.0 / TimeSpan.TicksPerSecond;

        private static bool _UseHRT;

        public static bool UsingHighResolutionTiming { get { return _UseHRT && _HighRes && !Unix; } }

        public static long TickCount { get { return (long)Ticks; } }

        public static double Ticks
        {
            get
            {
                if (_UseHRT && _HighRes && !Unix)
                {
                    return Stopwatch.GetTimestamp() * _HighFrequency;
                }

                return DateTime.UtcNow.Ticks * _LowFrequency;
            }
        }

        public static readonly bool Is64Bit = Environment.Is64BitProcess;

        public static bool MultiProcessor { get; private set; }
        public static int ProcessorCount { get; private set; }

        public static bool Unix { get; private set; }

        public static string FindDataFile(string path)
        {
            if (DataDirectories.Count == 0)
            {
                throw new InvalidOperationException("Attempted to FindDataFile before DataDirectories list has been filled.");
            }

            string fullPath = null;

            foreach (string p in DataDirectories)
            {
                fullPath = Path.Combine(p, path);

                if (File.Exists(fullPath))
                {
                    break;
                }

                fullPath = null;
            }

            return fullPath;
        }

        public static string FindDataFile(string format, params object[] args)
        {
            return FindDataFile(String.Format(format, args));
        }

        #region Expansions
        public static Expansion Expansion { get; set; }
        public static bool T2A { get { return Expansion >= Expansion.T2A; } }
        public static bool UOR { get { return Expansion >= Expansion.UOR; } }
        public static bool UOTD { get { return Expansion >= Expansion.UOTD; } }
        public static bool LBR { get { return Expansion >= Expansion.LBR; } }
        public static bool AOS { get { return Expansion >= Expansion.AOS; } }
        public static bool SE { get { return Expansion >= Expansion.SE; } }
        public static bool ML { get { return Expansion >= Expansion.ML; } }
        public static bool SA { get { return Expansion >= Expansion.SA; } }
        public static bool HS { get { return Expansion >= Expansion.HS; } }
        public static bool TOL { get { return Expansion >= Expansion.TOL; } }
        #endregion

        public static string ExePath { get { return _ExePath ?? (_ExePath = Assembly.Location); } }

        public static string BaseDirectory
        {
            get
            {
                if (_BaseDirectory == null)
                {
                    try
                    {
                        _BaseDirectory = ExePath;

                        if (_BaseDirectory.Length > 0)
                            _BaseDirectory = Path.GetDirectoryName(_BaseDirectory);
                    }
                    catch
                    {
                        _BaseDirectory = "";
                    }
                }

                return _BaseDirectory;
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.IsTerminating ? "Error:" : "Warning:");
            Console.WriteLine(e.ExceptionObject);

            if (e.IsTerminating)
            {
                _Crashed = true;

                bool close = false;

                try
                {
                    CrashedEventArgs args = new CrashedEventArgs(e.ExceptionObject as Exception);

                    EventSink.InvokeCrashed(args);

                    close = args.Close;
                }
                catch
                { }

                if (!close && !Service)
                {
                    try
                    {
                        foreach (Listener l in MessagePump.Listeners)
                        {
                            l.Dispose();
                        }
                    }
                    catch
                    { }

                    Console.WriteLine("This exception is fatal, press return to exit");
                    Console.ReadLine();
                }

                Kill();
            }
        }

        internal enum ConsoleEventType
        {
            CTRL_C_EVENT,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        internal delegate bool ConsoleEventHandler(ConsoleEventType type);
        internal static ConsoleEventHandler _ConsoleEventHandler;

        internal class SafeNativeMethods
        {
            [DllImport("kernel32")]
            internal static extern bool QueryPerformanceCounter(out long value);
        }

        internal class UnsafeNativeMethods
        {
            [DllImport("Kernel32")]
            internal static extern bool SetConsoleCtrlHandler(ConsoleEventHandler callback, bool add);
        }

        private static bool OnConsoleEvent(ConsoleEventType type)
        {
            if (World.Saving || (Service && type == ConsoleEventType.CTRL_LOGOFF_EVENT))
                return true;

            Kill();	//Kill -> HandleClosed will handle waiting for the completion of flushing to disk

            return true;
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            HandleClosed();
        }

        private static bool _Closing;
        public static bool Closing { get { return _Closing; } }

        private static int _CycleIndex = 1;
        private static float[] _CyclesPerSecond = new float[100];

        public static float CyclesPerSecond
        {
            get { return _CyclesPerSecond[(_CycleIndex - 1) % _CyclesPerSecond.Length]; }
        }

        public static float AverageCPS { get { return _CyclesPerSecond.Take(_CycleIndex).Average(); } }

        public static void Kill()
        {
            Kill(false);
        }

        public static void Kill(bool restart)
        {
            HandleClosed();

            if (restart)
                Process.Start(ExePath, Arguments);

            Process.Kill();
        }

        private static void HandleClosed()
        {
            if (_Closing)
                return;

            _Closing = true;

            Console.Write("Exiting...");

            World.WaitForWriteCompletion();

            if (!_Crashed)
                EventSink.InvokeShutdown(new ShutdownEventArgs());

            Timer.TimerThread.Set();

            Console.WriteLine("done");
        }

        private static AutoResetEvent _Signal = new AutoResetEvent(true);
        public static void Set() { _Signal.Set(); }

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            foreach (string a in args)
            {
                if (Insensitive.Equals(a, "-debug"))
                {
                    Debug = true;
                }
                else if (Insensitive.Equals(a, "-service"))
                {
                    Service = true;
                }
                else if (Insensitive.Equals(a, "-profile"))
                {
                    Profiling = true;
                }
                else if (Insensitive.Equals(a, "-nocache"))
                {
                    _Cache = false;
                }
                else if (Insensitive.Equals(a, "-haltonwarning"))
                {
                    HaltOnWarning = true;
                }
                else if (Insensitive.Equals(a, "-vb"))
                {
                    VBdotNet = true;
                }
                else if (Insensitive.Equals(a, "-usehrt"))
                {
                    _UseHRT = true;
                }
            }

            try
            {
                if (Service)
                {
                    Console.SetOut(MultiConsoleOut = new MultiTextWriter(new TextWriter[]
					{
						new FileLogger("Logs/Console.log")
					}));
                }
                else
                {
                    Console.SetOut(MultiConsoleOut = new MultiTextWriter(new TextWriter[]
					{
						Console.Out
					}));
                    GOWLoggingConfig.Load();
                    if (GOWLoggingConfig.ConsoleLoggingEnabled)
                    {
                        ConsoleLog.Initialize();
                    }
                    if (GOWLoggingConfig.ErrorLoggingEnabled)
                    {
                        ErrorLog.Initialize();
                    }
                    if (GOWLoggingConfig.PlayerLoggingEnabled)
                    {
                        PlayerLog.Initialize();
                    }
                }
            }
            catch
            {
            }

            Thread = Thread.CurrentThread;
            Process = Process.GetCurrentProcess();
            Assembly = Assembly.GetEntryAssembly();

            if (Thread != null)
            {
                Thread.Name = "Core Thread";
            }

            if (BaseDirectory.Length > 0)
            {
                Directory.SetCurrentDirectory(BaseDirectory);
            }

            Timer.TimerThread ttObj = new Timer.TimerThread();

            _TimerThread = new Thread(ttObj.TimerMain)
            {
                Name = "Timer Thread"
            };


            Version ver = Assembly.GetName().Version;
            Utility.PushColor(ConsoleColor.Blue);
            ConsoleLog.WriteLine("________________________________________________________________________________");
            Utility.PushColor(ConsoleColor.White);
            Utility.PopColor();

            Utility.PushColor(ConsoleColor.Magenta);
            ConsoleLog.WriteLine("                                Generation Of Worlds                        ");
            Utility.PushColor(ConsoleColor.White);

            ConsoleLog.WriteLine("                               Running Version {0}.{1}.{2}", new object[]
			{
				ver.Major,
				ver.Minor,
				ver.MinorRevision
			});
            ConsoleLog.WriteLine("                         .NET Framework Version {0}.{1}.{2}", new object[]
			{
				Environment.Version.Major,
				Environment.Version.Minor,
				Environment.Version.Build
			});
            Utility.PushColor(ConsoleColor.Blue);
            ConsoleLog.WriteLine("________________________________________________________________________________");
            int platform = (int)Environment.OSVersion.Platform;
            Utility.PushColor(ConsoleColor.White);
            ConsoleLog.Write("G.O.W: Detecting Operating System");
            Utility.PushColor(ConsoleColor.DarkGray);
            ConsoleLog.Write(".....................................");
            Utility.PushColor(ConsoleColor.White);
            if (platform == 4 || platform == 128)
            {
                Unix = true;
                ConsoleLog.WriteLine("[Unix]");
                Utility.PopColor();
            }
            else
            {
                _ConsoleEventHandler = new ConsoleEventHandler(OnConsoleEvent);
                UnsafeNativeMethods.SetConsoleCtrlHandler(_ConsoleEventHandler, true);

                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Win32S:
                        ConsoleLog.WriteLine("[Win3.1]");
                        Utility.PopColor();
                        break;
                    case PlatformID.Win32Windows:
                        {
                            int minor = Environment.OSVersion.Version.Minor;
                            if (minor != 0)
                            {
                                if (minor != 10)
                                {
                                    if (minor == 90)
                                    {
                                        ConsoleLog.WriteLine("[WinME]");
                                        Utility.PopColor();
                                    }
                                }
                                else
                                {
                                    ConsoleLog.WriteLine("[Win98]");
                                    Utility.PopColor();
                                }
                            }
                            else
                            {
                                ConsoleLog.WriteLine("[Win95]");
                                Utility.PopColor();
                            }
                            break;
                        }
                    case PlatformID.Win32NT:
                        switch (Environment.OSVersion.Version.Major)
                        {
                            case 3:
                                ConsoleLog.WriteLine("[NT 3.51]");
                                Utility.PopColor();
                                break;
                            case 4:
                                Console.WriteLine("[NT 4.0]");
                                Utility.PopColor();
                                break;
                            case 5:
                                switch (Environment.OSVersion.Version.Minor)
                                {
                                    case 0:
                                        ConsoleLog.WriteLine("[Win2000]");
                                        Utility.PopColor();
                                        break;
                                    case 1:
                                        ConsoleLog.WriteLine("[WinXP]");
                                        Utility.PopColor();
                                        break;
                                    case 2:
                                        ConsoleLog.WriteLine("[Win2003]");
                                        Utility.PopColor();
                                        break;
                                }
                                break;
                            case 6:
                                switch (Environment.OSVersion.Version.Minor)
                                {
                                    case 0:
                                        ConsoleLog.WriteLine("[Vista]");
                                        Utility.PopColor();
                                        break;
                                    case 1:
                                        ConsoleLog.WriteLine("[Win7]");
                                        Utility.PopColor();
                                        break;
                                    case 2:
                                        ConsoleLog.WriteLine("[Win8]");
                                        Utility.PopColor();
                                        break;
                                    case 3:
                                        ConsoleLog.WriteLine("[Win8.1]");
                                        Utility.PopColor();
                                        break;
                                    case 4:
                                        ConsoleLog.WriteLine("[Win10]");
                                        Utility.PopColor();
                                        break;
                                }
                                break;
                        }
                        break;
                    case PlatformID.WinCE:
                        ConsoleLog.WriteLine("[WinCE]");
                        Utility.PopColor();
                        break;
                    default:
                        _ConsoleEventHandler = new ConsoleEventHandler(OnConsoleEvent);
                        UnsafeNativeMethods.SetConsoleCtrlHandler(_ConsoleEventHandler, true);
                        ConsoleLog.WriteLine("[Unknown]");
                        Utility.PopColor();
                        break;
                }
            }
            Utility.PushColor(ConsoleColor.White);
            ConsoleLog.Write("G.O.W: Processor Configuration");
            Utility.PushColor(ConsoleColor.DarkGray);
            ConsoleLog.Write("........................................");
            Utility.PushColor(ConsoleColor.White);
            if (Is64Bit)
            {
                ConsoleLog.WriteLine("[64-bit]");
                Utility.PopColor();
            }
            else
            {
                ConsoleLog.WriteLine("[32-bit]");
                Utility.PopColor();
            }
            Utility.PushColor(ConsoleColor.White);
            ConsoleLog.Write("G.O.W: Processor Count");
            Utility.PushColor(ConsoleColor.DarkGray);
            ConsoleLog.Write("................................................");
            Utility.PushColor(ConsoleColor.White);
            ProcessorCount = Environment.ProcessorCount;
            if (ProcessorCount > 1)
            {
                MultiProcessor = true;
            }
            ConsoleLog.WriteLine("[{0}]", new object[]
			{
				ProcessorCount
			});
            Utility.PopColor();
            string arguments = Arguments;
            Utility.PushColor(ConsoleColor.White);
            ConsoleLog.Write("G.O.W: Operating Mode");
            Utility.PushColor(ConsoleColor.DarkGray);
            ConsoleLog.Write(".................................................");
            if (arguments.Length > 0)
            {
                if (arguments.Length > 8)
                {
                    Console.SetCursorPosition(63, Console.CursorTop);
                }
                else if (arguments.Length == 8)
                {
                    Console.SetCursorPosition(69, Console.CursorTop);
                }
                Utility.PushColor(ConsoleColor.Yellow);
                ConsoleLog.WriteLine("[{0}]", new object[]
				{
					arguments
				});
                Utility.PopColor();
            }
            else
            {
                Utility.PushColor(ConsoleColor.DarkGray);
                ConsoleLog.WriteLine("[default]");
                Utility.PopColor();
            }
            Utility.PushColor(ConsoleColor.White);
            ConsoleLog.Write("G.O.W: Garbage Collection Mode");
            Utility.PushColor(ConsoleColor.DarkGray);
            ConsoleLog.Write("........................................");
            if (GCSettings.IsServerGC)
            {
                Utility.PushColor(ConsoleColor.White);
                ConsoleLog.WriteLine("[Enabled]");
                Utility.PopColor();
            }
            else
            {
                Utility.PushColor(ConsoleColor.Gray);
                Console.SetCursorPosition(69, Console.CursorTop);
                ConsoleLog.WriteLine("[Disabled]");
                Utility.PopColor();
            }
            string text = "....................................................";
            int num = 70 - (RandomImpl.Type.Name.Length + 18);
            if (num < 0)
            {
                num = 0;
            }
            Utility.PushColor(ConsoleColor.White);
            ConsoleLog.Write("G.O.W: RandomImpl {0}", new object[]
			{
				RandomImpl.Type.Name
			});
            Utility.PushColor(ConsoleColor.DarkGray);
            ConsoleLog.WriteConsole("...........................................");
            Utility.PushColor(ConsoleColor.White);
            Console.SetCursorPosition(69, Console.CursorTop);
            ConsoleLog.WriteLineConsole("[{0}]", new object[]
			{
				RandomImpl.IsHardwareRNG ? "Hardware" : "Software"
			});
            ConsoleLog.WriteLog(text.Substring(0, num));
            ConsoleLog.WriteLineLog("[{0}]", new object[]
			{
				RandomImpl.IsHardwareRNG ? "Hardware" : "Software"
			});
            Utility.PopColor();
            if (_UseHRT)
            {
                Utility.PushColor(ConsoleColor.White);
                ConsoleLog.Write("G.O.W: High Resolution Timing requested");
                Utility.PushColor(ConsoleColor.DarkGray);
                ConsoleLog.Write("...............................");
                if (UsingHighResolutionTiming)
                {
                    Utility.PushColor(ConsoleColor.Green);
                    ConsoleLog.WriteLine("[Success]");
                    Utility.PopColor();
                }
                else
                {
                    Utility.PushColor(ConsoleColor.Red);
                    ConsoleLog.WriteLine("[Failure]");
                    Utility.PopColor();
                }
            }
            else
            {
                Utility.PushColor(ConsoleColor.White);
                ConsoleLog.Write("G.O.W: Standard Timing requested");
                Utility.PushColor(ConsoleColor.DarkGray);
                ConsoleLog.Write("......................................");
                Utility.PushColor(ConsoleColor.Green);
                ConsoleLog.WriteLine("[Success]");
                Utility.PopColor();
            }
            Utility.PushColor(ConsoleColor.White);
            ConsoleLog.Write("G.O.W: Compiling C# scripts");
            Utility.PushColor(ConsoleColor.DarkGray);
            ConsoleLog.Write("...........................................");
            Utility.PopColor();
            while (!ScriptCompiler.Compile(Debug, _Cache))
            {
                if (Service)
                {
                    return;
                }
                Utility.PushColor(ConsoleColor.Yellow);
                ConsoleLog.WriteLine("");
                ConsoleLog.Write("G.O.W: Press <return> to exit, or <R> to try again.                   ");
                if (Console.ReadKey(true).Key != ConsoleKey.R)
                {
                    return;
                }
            }
            ScriptCompiler.Invoke("Configure");
            Region.Load();
            World.Load();
            Teleporters.Initialize();
            ScriptCompiler.Invoke("Initialize");
            Utility.PushColor(ConsoleColor.White);
            ConsoleLog.Write("G.O.W: Initializing Network");
            Utility.PushColor(ConsoleColor.DarkGray);
            ConsoleLog.Write("...(F6 for a list of available IP's).....");
            Utility.PushColor(ConsoleColor.White);
            if (GOWLoggingConfig.GOWConsoleBeep)
            {
                ConsoleLog.WriteLineConsole("\a[Listening]");
            }
            else
            {
                ConsoleLog.WriteLineConsole("[Listening]");
            }
            ConsoleLog.WriteLineLog("..[Listening]");
            Utility.PopColor();
            MessagePump messagePump = MessagePump = new MessagePump();
            _TimerThread.Start();

            foreach (Map m in Map.AllMaps)
            {
                m.Tiles.Force();
            }
            NetState.Initialize();
            Utility.PushColor(ConsoleColor.Blue);
            ConsoleLog.WriteLine("________________________________________________________________________________");
            Utility.PopColor();
            EventSink.InvokeServerStarted();
            try
            {
                long now, last = TickCount;

                const int sampleInterval = 100;
                const float ticksPerSecond = 1000.0f * sampleInterval;

                long sample = 0;

                while (!_Closing)
                {
                    _Signal.WaitOne();

                    Mobile.ProcessDeltaQueue();
                    Item.ProcessDeltaQueue();

                    Timer.Slice();
                    messagePump.Slice();

                    NetState.FlushAll();
                    NetState.ProcessDisposedQueue();

                    if (Slice != null)
                    {
                        Slice();
                    }

                    if (sample++ % sampleInterval != 0)
                    {
                        continue;
                    }

                    now = TickCount;
                    _CyclesPerSecond[_CycleIndex++ % _CyclesPerSecond.Length] = ticksPerSecond / (now - last);
                    last = now;
                }
            }
            catch (Exception e)
            {
                CurrentDomain_UnhandledException(null, new UnhandledExceptionEventArgs(e, true));
            }
        }

        public static string Arguments
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (Debug)
                {
                    Utility.Separate(sb, "-debug", " ");
                }

                if (Service)
                {
                    Utility.Separate(sb, "-service", " ");
                }

                if (Profiling)
                {
                    Utility.Separate(sb, "-profile", " ");
                }

                if (!_Cache)
                {
                    Utility.Separate(sb, "-nocache", " ");
                }

                if (HaltOnWarning)
                {
                    Utility.Separate(sb, "-haltonwarning", " ");
                }

                if (VBdotNet)
                {
                    Utility.Separate(sb, "-vb", " ");
                }

                if (_UseHRT)
                {
                    Utility.Separate(sb, "-usehrt", " ");
                }

                return sb.ToString();
            }
        }

        public static int GlobalUpdateRange { get; set; }
        public static int GlobalMaxUpdateRange { get; set; }

        private static int m_ItemCount, m_MobileCount, m_CustomsCount;

        public static int ScriptItems { get { return m_ItemCount; } }
        public static int ScriptMobiles { get { return m_MobileCount; } }
        public static int ScriptCustoms { get { return m_CustomsCount; } }

        public static void VerifySerialization()
        {
            m_ItemCount = 0;
            m_MobileCount = 0;
            m_CustomsCount = 0;

            VerifySerialization(Assembly.GetCallingAssembly());

            foreach (Assembly a in ScriptCompiler.Assemblies)
            {
                VerifySerialization(a);
            }
        }

        private static readonly Type[] _SerialTypeArray = { typeof(Serial) };
        private static readonly Type[] _CustomsSerialTypeArray = { typeof(CustomSerial) };

        private static void VerifyType(Type t)
        {
            bool isItem = t.IsSubclassOf(typeof(Item));

            if (isItem || t.IsSubclassOf(typeof(Mobile)))
            {
                if (isItem)
                {
                    Interlocked.Increment(ref m_ItemCount);
                }
                else
                {
                    Interlocked.Increment(ref m_MobileCount);
                }

                StringBuilder warningSb = null;

                try
                {
                    if (t.GetConstructor(_SerialTypeArray) == null)
                    {
                        warningSb = new StringBuilder();

                        warningSb.AppendLine("       - No serialization constructor");
                    }

                    if (t.GetMethod("Serialize", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly) == null)
                    {
                        if (warningSb == null)
                        {
                            warningSb = new StringBuilder();
                        }

                        warningSb.AppendLine("       - No Serialize() method");
                    }

                    if (t.GetMethod("Deserialize",BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly) == null)
                    {
                        if (warningSb == null)
                        {
                            warningSb = new StringBuilder();
                        }

                        warningSb.AppendLine("       - No Deserialize() method");
                    }

                    if (warningSb != null && warningSb.Length > 0)
                    {
                        Utility.PushColor(ConsoleColor.Yellow);
                        Console.WriteLine("Warning: {0}\n{1}", t, warningSb);
                        Utility.PopColor();
                    }
                }
                catch
                {
                    Utility.PushColor(ConsoleColor.Yellow);
                    ConsoleLog.WriteLine("G.O.W: Exception in serialization verification of type {0}", new object[]
					{
						t
					});
                    Utility.PopColor();
                    return;
                }
            }
            else if (t.IsSubclassOf(typeof(SaveData)))
            {
                Interlocked.Increment(ref m_CustomsCount);

                StringBuilder warningSb = null;

                try
                {
                    if (t.GetConstructor(_CustomsSerialTypeArray) == null)
                    {
                        warningSb = new StringBuilder();

                        warningSb.AppendLine("       - No serialization constructor");
                    }

                    if (t.GetMethod("Serialize", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly) == null)
                    {
                        if (warningSb == null)
                        {
                            warningSb = new StringBuilder();
                        }

                        warningSb.AppendLine("       - No Serialize() method");
                    }

                    if (t.GetMethod("Deserialize", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly) == null)
                    {
                        if (warningSb == null)
                        {
                            warningSb = new StringBuilder();
                        }

                        warningSb.AppendLine("       - No Deserialize() method");
                    }

                    if (warningSb != null && warningSb.Length > 0)
                    {
                        Utility.PushColor(ConsoleColor.White);
                        Console.WriteLine("G.O.W: Warnings {0}\n{1}", t, warningSb);
                        Utility.PopColor();
                    }
                }
                catch
                {
                    Utility.PushColor(ConsoleColor.Yellow);
                    Console.WriteLine("G.O.W: Exception in serialization verification of type {0}", t);
                    Utility.PopColor();
                }
            }
        }

        private static void VerifySerialization(Assembly a)
        {
            if (a != null)
            {
                Parallel.ForEach(a.GetTypes(), VerifyType);
            }
        }
    }

    public class FileLogger : TextWriter
    {
        public const string DateFormat = "[MMMM dd hh:mm:ss.f tt]: ";
        private bool _NewLine;

        public string FileName { get; private set; }

        public FileLogger(string file)
            : this(file, false)
        {
        }

        public FileLogger(string file, bool append)
        {
            FileName = file;

            using (
                var writer =
                    new StreamWriter(
                        new FileStream(FileName, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Read)))
            {
                writer.WriteLine(">>>Logging started on {0}.", DateTime.UtcNow.ToString("f"));
                //f = Tuesday, April 10, 2001 3:51 PM 
            }

            _NewLine = true;
        }

        public override void Write(char ch)
        {
            using (var writer = new StreamWriter(new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.Read)))
            {
                if (_NewLine)
                {
                    writer.Write(DateTime.UtcNow.ToString(DateFormat));
                    _NewLine = false;
                }

                writer.Write(ch);
            }
        }

        public override void Write(string str)
        {
            using (var writer = new StreamWriter(new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.Read)))
            {
                if (_NewLine)
                {
                    writer.Write(DateTime.UtcNow.ToString(DateFormat));
                    _NewLine = false;
                }

                writer.Write(str);
            }
        }

        public override void WriteLine(string line)
        {
            using (var writer = new StreamWriter(new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.Read)))
            {
                if (_NewLine)
                {
                    writer.Write(DateTime.UtcNow.ToString(DateFormat));
                }

                writer.WriteLine(line);
                _NewLine = true;
            }
        }

        public override Encoding Encoding { get { return Encoding.Default; } }
    }

    public class MultiTextWriter : TextWriter
    {
        private readonly List<TextWriter> _Streams;

        public MultiTextWriter(params TextWriter[] streams)
        {
            _Streams = new List<TextWriter>(streams);

            if (_Streams.Count < 0)
            {
                throw new ArgumentException("You must specify at least one stream.");
            }
        }

        public void Add(TextWriter tw)
        {
            _Streams.Add(tw);
        }

        public void Remove(TextWriter tw)
        {
            _Streams.Remove(tw);
        }

        public override void Write(char ch)
        {
            foreach (var t in _Streams)
            {
                t.Write(ch);
            }
        }

        public override void WriteLine(string line)
        {
            foreach (var t in _Streams)
            {
                t.WriteLine(line);
            }
        }

        public override void WriteLine(string line, params object[] args)
        {
            WriteLine(String.Format(line, args));
        }

        public override Encoding Encoding { get { return Encoding.Default; } }
    }
}