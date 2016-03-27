using System;
using System.IO;
using Microsoft.Win32;
using System.Collections.Generic;
using Server.MainConfiguration;

namespace Server.Misc
{
    public class DataPath
    {
        /* If you have not installed Ultima Online,
        * or wish the server to use a separate set of datafiles,
        * change the 'CustomPath' value.
        * Example:
        *  private static string CustomPath = @"C:\Program Files\Ultima Online";
        */
        private static readonly string CustomPath = MainConfig.MainMulPath;
        /* The following is a list of files which a required for proper execution:
        * 
        * Multi.idx
        * Multi.mul
        * VerData.mul
        * TileData.mul
        * Map*.mul or Map*LegacyMUL.uop
        * StaIdx*.mul
        * Statics*.mul
        * MapDif*.mul
        * MapDifL*.mul
        * StaDif*.mul
        * StaDifL*.mul
        * StaDifI*.mul
        */
        public static void Configure()
        {
            string pathUO = GetPath(@"Origin Worlds Online\Ultima Online\1.0", "ExePath");
            string pathTD = GetPath(@"Origin Worlds Online\Ultima Online Third Dawn\1.0", "ExePath"); //These refer to 2D & 3D, not the Third Dawn expansion
            string pathKR = GetPath(@"Origin Worlds Online\Ultima Online\KR Legacy Beta", "ExePath"); //After KR, This is the new registry key for the 2D client
            string pathSA = GetPath(@"Electronic Arts\Ultima Online Stygian Abyss Classic", "InstallDir");
            string pathHS = GetPath(@"Electronic Arts\Ultima Online Classic", "InstallDir");

            if (CustomPath != null) 
                Core.DataDirectories.Add(CustomPath); 

            if (pathUO != null) 
                Core.DataDirectories.Add(pathUO); 

            if (pathTD != null) 
                Core.DataDirectories.Add(pathTD);

            if (pathKR != null)
                Core.DataDirectories.Add(pathKR);

            if (pathSA != null)
                Core.DataDirectories.Add(pathSA);

            if (pathHS != null)
                Core.DataDirectories.Add(pathHS);

            if (Core.DataDirectories.Count == 0 && !Core.Service)
            {
                Console.WriteLine("Enter the Ultima Online directory:");
                Console.Write("> ");

                Core.DataDirectories.Add(Console.ReadLine());
            }
        }

        private static string GetPath(string subName, string keyName)
        {
            try
            {
                string keyString;

                if (Core.Is64Bit)
                    keyString = @"SOFTWARE\Wow6432Node\{0}";
                else
                    keyString = @"SOFTWARE\{0}";

                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(String.Format(keyString, subName)))
                {
                    if (key == null)
                        return null;

                    string v = key.GetValue(keyName) as string;

                    if (String.IsNullOrEmpty(v))
                        return null;

                    if (keyName == "InstallDir")
                        v = v + @"\";

                    v = Path.GetDirectoryName(v);

                    if (String.IsNullOrEmpty(v))
                        return null;

                    return v;
                }
            }
            catch
            {
                return null;
            }
        }
    }

    public static class DataPathExtended
    {
        public static bool Is64Bit { get; private set; }
        public static List<string> KnownInstallationRegistryKeys { get; private set; }
        public static List<string> KnownRegistryKeyValueNames { get; private set; }
        public static List<string> DetectedPaths { get; private set; }

        [CallPriority(-1)]
        public static void Configure()
        {
            Is64Bit = (IntPtr.Size == 8);

            KnownInstallationRegistryKeys = new List<string> 
            {
                @"Electronic Arts\EA Games\Ultima Online Stygian Abyss Classic",     
                @"Origin Worlds Online\Ultima Online\KR Legacy Beta", 
                @"EA Games\Ultima Online: Mondain's Legacy\1.00.0000", 
                @"Origin Worlds Online\Ultima Online\1.0", 
                @"Origin Worlds Online\Ultima Online Third Dawn\1.0",
                @"EA GAMES\Ultima Online Samurai Empire", 
                @"EA Games\Ultima Online: Mondain's Legacy", 
                @"EA GAMES\Ultima Online Samurai Empire\1.0", 
                @"EA GAMES\Ultima Online Samurai Empire\1.00.0000", 
                @"EA GAMES\Ultima Online: Samurai Empire\1.0", 
                @"EA GAMES\Ultima Online: Samurai Empire\1.00.0000", 
                @"EA Games\Ultima Online: Mondain's Legacy\1.0", 
                @"EA Games\Ultima Online: Mondain's Legacy\1.00.0000", 
                @"Origin Worlds Online\Ultima Online Samurai Empire BETA\2d\1.0", 
                @"Origin Worlds Online\Ultima Online Samurai Empire BETA\3d\1.0", 
                @"Origin Worlds Online\Ultima Online Samurai Empire\2d\1.0", 
                @"Origin Worlds Online\Ultima Online Samurai Empire\3d\1.0",
                @"Electronic Arts\EA Games\Ultima Online Classic"
            };

            KnownRegistryKeyValueNames = new List<string> 
            {
                @"ExePath",
                @"InstallDir",
                @"Install Dir",
                @"GameExecutionPath"
            };

            Init();
        }

        private static void Init()
        {
            if (MainConfig.MainDataPathExtendedEnabled)
            {
                Console.WriteLine("Searching for Ultima Online installations...");
                DetectedPaths = new List<string>(Locate());

                if (DetectedPaths.Count > 0)
                {
                    Console.WriteLine("Found {0:#,#} Ultima Online installations:", DetectedPaths.Count);
                    DetectedPaths.ForEach(Console.WriteLine);
                    Core.DataDirectories.AddRange(DetectedPaths);
                }
                else
                {
                    Console.WriteLine("Could not find any Ultima Online installations.");

                    if (!Core.Service)
                    {
                        Console.WriteLine("Enter the Ultima Online directory:");
                        Console.Write("> ");

                        Core.DataDirectories.Add(Console.ReadLine());
                    }
                }
            }
        }

        public static IEnumerable<string> Locate()
        {
            string prefix = Is64Bit ? @"SOFTWARE\Wow6432Node\" : @"SOFTWARE\";

            foreach (string knownKeyName in KnownInstallationRegistryKeys)
            {
                if (!String.IsNullOrWhiteSpace(knownKeyName))
                {
                    string exePath;
                    TryGetExePath(prefix + knownKeyName, out exePath);

                    if (!String.IsNullOrWhiteSpace(exePath))
                    { yield return exePath; }
                }
            }
        }

        private static bool TryGetExePath(string regPath, out string exePath)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regPath) ?? Registry.CurrentUser.OpenSubKey(regPath))
                {
                    if (key == null)
                    {
                        exePath = null;
                        return false;
                    }

                    string dir = null, file = null;

                    foreach (string knownKeyValueName in KnownRegistryKeyValueNames)
                    {
                        string pathStub = key.GetValue(knownKeyValueName) as string;

                        if (String.IsNullOrWhiteSpace(pathStub))
                        { continue; }

                        if (String.IsNullOrWhiteSpace(file) && Path.HasExtension(pathStub))
                        {
                            file = Path.GetFileName(pathStub);

                            if (String.IsNullOrWhiteSpace(dir) && Path.IsPathRooted(pathStub))
                            { dir = Path.GetDirectoryName(pathStub); }
                        }

                        if (String.IsNullOrWhiteSpace(dir) && Path.IsPathRooted(pathStub))
                        { dir = pathStub; }

                        if (!String.IsNullOrWhiteSpace(dir) && !String.IsNullOrWhiteSpace(file))
                        {
                            string fullPath = dir.Replace('/', '\\');

                            if (fullPath[fullPath.Length - 1] != '\\')
                                fullPath += '\\';

                            fullPath += file;

                            if (File.Exists(fullPath))
                            {
                                exePath = dir;
                                return true;
                            }
                        }
                    }
                }
            }
            catch
            { }
            exePath = null;
            return false;
        }
    }
}