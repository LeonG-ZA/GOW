#region References
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using Server.LogConsole;
#endregion

namespace Server
{
	public static class ScriptCompiler
	{
		private static Assembly[] m_Assemblies;

		public static Assembly[] Assemblies { get { return m_Assemblies; } set { m_Assemblies = value; } }

		private static readonly List<string> m_AdditionalReferences = new List<string>();

		public static string[] GetReferenceAssemblies()
		{
			var list = new List<string>();

			string path = Path.Combine(Core.BaseDirectory, "Data/Assemblies.cfg");

			if (File.Exists(path))
			{
				using (var ip = new StreamReader(path))
				{
					string line;

					while ((line = ip.ReadLine()) != null)
					{
						if (line.Length > 0 && !line.StartsWith("#"))
						{
							list.Add(line);
						}
					}
				}
			}

			list.Add(Core.ExePath);
 
			list.AddRange(m_AdditionalReferences);

			return list.ToArray();
		}

		public static string GetCompilerOptions(bool debug)
		{
			StringBuilder sb = null;

			if (!debug)
			{
				AppendCompilerOption(ref sb, "/optimize");
			}

#if MONO
			AppendCompilerOption( ref sb, "/d:MONO" );
            #endif

			if (Core.Is64Bit)
			{
				AppendCompilerOption(ref sb, "/d:x64"); // Legacy Support
			}

			AppendCompilerOption(ref sb, "/d:Framework_4_0"); // Legacy Support

            //AppendCompilerOption(ref sb, "/d:NEWTIMERS");
            //AppendCompilerOption(ref sb, "/d:NEWPARENT");

			return (sb == null ? null : sb.ToString());
		}

		private static void AppendCompilerOption(ref StringBuilder sb, string define)
		{
			if (sb == null)
			{
				sb = new StringBuilder();
			}
			else
			{
				sb.Append(' ');
			}

			sb.Append(define);
		}

		private static byte[] GetHashCode(string compiledFile, string[] scriptFiles, bool debug)
		{
			using (var ms = new MemoryStream())
			{
				using (var bin = new BinaryWriter(ms))
				{
					var fileInfo = new FileInfo(compiledFile);

					bin.Write(fileInfo.LastWriteTimeUtc.Ticks);

					foreach (string scriptFile in scriptFiles)
					{
						fileInfo = new FileInfo(scriptFile);

						bin.Write(fileInfo.LastWriteTimeUtc.Ticks);
					}

					bin.Write(debug);
					bin.Write(Core.Version.ToString());

					ms.Position = 0;

					using (SHA1 sha1 = SHA1.Create())
					{
						return sha1.ComputeHash(ms);
					}
				}
			}
		}

		public static bool CompileCSScripts(out Assembly assembly)
		{
			return CompileCSScripts(false, true, out assembly);
		}

        public static bool CompileCSScripts(bool debug, out Assembly assembly)
		{
			return CompileCSScripts(debug, true, out assembly);
		}

        public static bool CompileCSScripts(bool debug, bool cache, out Assembly assembly)
		{
			string[] files = GetScripts("*.cs");

			if (files.Length == 0)
			{
				Utility.PushColor(ConsoleColor.Red);
				Console.WriteLine("[Failed]");
				Utility.PopColor();
                assembly = null;
				return true;
			}

			if (File.Exists("Scripts/Output/Scripts.CS.dll"))
			{
				if (cache && File.Exists("Scripts/Output/Scripts.CS.hash"))
				{
					try
					{
						byte[] hashCode = GetHashCode("Scripts/Output/Scripts.CS.dll", files, debug);

						using (var fs = new FileStream("Scripts/Output/Scripts.CS.hash", FileMode.Open, FileAccess.Read, FileShare.Read))
						{
							using (var bin = new BinaryReader(fs))
							{
								byte[] bytes = bin.ReadBytes(hashCode.Length);

								if (bytes.Length == hashCode.Length)
								{
									bool valid = true;

									for (int i = 0; i < bytes.Length; ++i)
									{
										if (bytes[i] != hashCode[i])
										{
											valid = false;
											break;
										}
									}

									if (valid)
									{
										assembly = Assembly.LoadFrom("Scripts/Output/Scripts.CS.dll");

										if (!m_AdditionalReferences.Contains(assembly.Location))
										{
											m_AdditionalReferences.Add(assembly.Location);
										}

                                        Utility.PushColor(ConsoleColor.Green);
                                        ConsoleLog.WriteLine("[Success]");
                                        Utility.PopColor();

										return true;
									}
								}
							}
						}
					}
					catch
					{ }
				}
			}

			DeleteFiles("Scripts.CS*.dll");

			using (var provider = new CSharpCodeProvider())
			{
				string path = GetUnusedPath("Scripts.CS");

				var parms = new CompilerParameters(GetReferenceAssemblies(), path, debug);

				string options = GetCompilerOptions(debug);

				if (options != null)
				{
					parms.CompilerOptions = options;
				}

				if (Core.HaltOnWarning)
				{
					parms.WarningLevel = 4;
				}

#if !MONO
				CompilerResults results = provider.CompileAssemblyFromFile(parms, files);
#else
				parms.CompilerOptions = String.Format( "{0} /nowarn:169,219,414 /recurse:Scripts/*.cs", parms.CompilerOptions );
				CompilerResults results = provider.CompileAssemblyFromFile( parms, "" );
                #endif
				m_AdditionalReferences.Add(path);

				Display(results);

#if !MONO
				if (results.Errors.Count > 0)
				{
					assembly = null;
					return false;
				}
#else
				if( results.Errors.Count > 0 ) {
					foreach( CompilerError err in results.Errors ) {
						if ( !err.IsWarning ) {
							assembly = null;
							return false;
						}
					}
				}
                #endif

				if (cache && Path.GetFileName(path) == "Scripts.CS.dll")
				{
					try
					{
						byte[] hashCode = GetHashCode(path, files, debug);

						using (
							var fs = new FileStream("Scripts/Output/Scripts.CS.hash", FileMode.Create, FileAccess.Write, FileShare.None))
						{
							using (var bin = new BinaryWriter(fs))
							{
								bin.Write(hashCode, 0, hashCode.Length);
							}
						}
					}
					catch
					{ }
				}

				assembly = results.CompiledAssembly;
				return true;
			}
		}

		public static bool CompileVBScripts(out Assembly assembly)
		{
			return CompileVBScripts(false, out assembly);
		}

		public static bool CompileVBScripts(bool debug, out Assembly assembly)
		{
			return CompileVBScripts(debug, true, out assembly);
		}

		public static bool CompileVBScripts(bool debug, bool cache, out Assembly assembly)
		{
            Utility.PushColor(ConsoleColor.White);
            ConsoleLog.Write("G.O.W: Compiling VB.NET scripts");
            Utility.PushColor(ConsoleColor.DarkGray);
            ConsoleLog.Write(".......................................");
			string[] files = GetScripts("*.vb");

			if (files.Length == 0)
			{
                Utility.PushColor(ConsoleColor.Red);
                ConsoleLog.WriteLine("[Failed]");
                Utility.PopColor();
				assembly = null;
				return true;
			}

			if (File.Exists("Scripts/Output/Scripts.VB.dll"))
			{
				if (cache && File.Exists("Scripts/Output/Scripts.VB.hash"))
				{
					byte[] hashCode = GetHashCode("Scripts/Output/Scripts.VB.dll", files, debug);

					try
					{
						using (var fs = new FileStream("Scripts/Output/Scripts.VB.hash", FileMode.Open, FileAccess.Read, FileShare.Read))
						{
							using (var bin = new BinaryReader(fs))
							{
								byte[] bytes = bin.ReadBytes(hashCode.Length);

								if (bytes.Length == hashCode.Length)
								{
									bool valid = true;

									for (int i = 0; i < bytes.Length; ++i)
									{
										if (bytes[i] != hashCode[i])
										{
											valid = false;
											break;
										}
									}

									if (valid)
									{
										assembly = Assembly.LoadFrom("Scripts/Output/Scripts.VB.dll");

										if (!m_AdditionalReferences.Contains(assembly.Location))
										{
											m_AdditionalReferences.Add(assembly.Location);
										}

									Utility.PushColor(ConsoleColor.Green);
									ConsoleLog.WriteLine("[Success]");
									Utility.PopColor();

										return true;
									}
								}
							}
						}
					}
					catch
					{ }
				}
			}

			DeleteFiles("Scripts.VB*.dll");

			using (var provider = new VBCodeProvider())
			{
				string path = GetUnusedPath("Scripts.VB");

				var parms = new CompilerParameters(GetReferenceAssemblies(), path, debug);

				string options = GetCompilerOptions(debug);

				if (options != null)
				{
					parms.CompilerOptions = options;
				}

				if (Core.HaltOnWarning)
				{
					parms.WarningLevel = 4;
				}

				CompilerResults results = provider.CompileAssemblyFromFile(parms, files);
				m_AdditionalReferences.Add(path);

				Display(results);

				if (results.Errors.Count > 0)
				{
					assembly = null;
					return false;
				}

				if (cache && Path.GetFileName(path) == "Scripts.VB.dll")
				{
					try
					{
						byte[] hashCode = GetHashCode(path, files, debug);

						using (
							var fs = new FileStream("Scripts/Output/Scripts.VB.hash", FileMode.Create, FileAccess.Write, FileShare.None))
						{
							using (var bin = new BinaryWriter(fs))
							{
								bin.Write(hashCode, 0, hashCode.Length);
							}
						}
					}
					catch
					{ }
				}

				assembly = results.CompiledAssembly;
				return true;
			}
		}

        public static void Display(CompilerResults results)
        {
            if (results.Errors.Count > 0)
            {
                Dictionary<string, List<CompilerError>> dictionary = new Dictionary<string, List<CompilerError>>(results.Errors.Count, StringComparer.OrdinalIgnoreCase);
                Dictionary<string, List<CompilerError>> dictionary2 = new Dictionary<string, List<CompilerError>>(results.Errors.Count, StringComparer.OrdinalIgnoreCase);
                bool flag = true;
                foreach (CompilerError compilerError in results.Errors)
                {
                    string fileName = compilerError.FileName;
                    if (string.IsNullOrEmpty(fileName))
                    {
                        if (flag)
                        {
                            Utility.PushColor(ConsoleColor.Red);
                            ConsoleLog.WriteLine("[Error]");
                            flag = false;
                        }
                        ConsoleLog.WriteLine("ScriptCompiler: {0}: {1}", new object[]
						{
							compilerError.ErrorNumber,
							compilerError.ErrorText
						});
                        Utility.PopColor();
                    }
                    else
                    {
                        Dictionary<string, List<CompilerError>> dictionary3 = compilerError.IsWarning ? dictionary2 : dictionary;
                        List<CompilerError> list = null;
                        dictionary3.TryGetValue(fileName, out list);
                        if (list == null)
                        {
                            list = (dictionary3[fileName] = new List<CompilerError>());
                        }
                        list.Add(compilerError);
                    }
                }
                if (dictionary.Count > 0)
                {
                    if (flag)
                    {
                        Utility.PushColor(ConsoleColor.Red);
                        ConsoleLog.WriteLine("[Error]");
                        Utility.PopColor();
                    }
                }
                else if (flag)
                {
                    Utility.PushColor(ConsoleColor.Yellow);
                    ConsoleLog.WriteLine("[Warning]");
                    Utility.PopColor();
                }
                string fullPath = Path.GetFullPath(Path.Combine(Core.BaseDirectory, "Scripts" + Path.DirectorySeparatorChar));
                Uri uri = new Uri(fullPath);
                Utility.PushColor(ConsoleColor.Yellow);
                if (dictionary2.Count > 0)
                {
                    Utility.PushColor(ConsoleColor.Yellow);
                    ConsoleLog.WriteLine("Warnings:");
                }
                foreach (KeyValuePair<string, List<CompilerError>> current in dictionary2)
                {
                    string key = current.Key;
                    List<CompilerError> value = current.Value;
                    string fullPath2 = Path.GetFullPath(key);
                    string text = Uri.UnescapeDataString(uri.MakeRelativeUri(new Uri(fullPath2)).OriginalString);
                    ConsoleLog.WriteLine(" + {0}:", new object[]
					{
						text
					});
                    Utility.PushColor(ConsoleColor.DarkYellow);
                    foreach (CompilerError current2 in value)
                    {
                        ConsoleLog.WriteLine("Line {0}: {1}", new object[]
						{
							current2.Line,
							current2.ErrorText
						});
                    }
                    Utility.PopColor();
                }
                Utility.PopColor();
                if (dictionary.Count > 0)
                {
                    Utility.PushColor(ConsoleColor.Red);
                    ConsoleLog.WriteLine("Errors:");
                }
                foreach (KeyValuePair<string, List<CompilerError>> current3 in dictionary)
                {
                    string key2 = current3.Key;
                    List<CompilerError> value2 = current3.Value;
                    string fullPath3 = Path.GetFullPath(key2);
                    string text2 = Uri.UnescapeDataString(uri.MakeRelativeUri(new Uri(fullPath3)).OriginalString);
                    ConsoleLog.WriteLine(" + {0}:", new object[]
					{
						text2
					});
                    Utility.PushColor(ConsoleColor.DarkRed);
                    foreach (CompilerError current4 in value2)
                    {
                        ConsoleLog.WriteLine("Line {0}: {1}", new object[]
						{
							current4.Line,
							current4.ErrorText
						});
                    }
                    Utility.PopColor();
                }
                Utility.PopColor();
                return;
            }
            Utility.PushColor(ConsoleColor.Green);
            ConsoleLog.WriteLine("[Success]");
            Utility.PopColor();
        }

		public static string GetUnusedPath(string name)
		{
			string path = Path.Combine(Core.BaseDirectory, String.Format("Scripts/Output/{0}.dll", name));

			for (int i = 2; File.Exists(path) && i <= 1000; ++i)
			{
				path = Path.Combine(Core.BaseDirectory, String.Format("Scripts/Output/{0}.{1}.dll", name, i));
			}

			return path;
		}

		public static void DeleteFiles(string mask)
		{
			try
			{
				string[] files = Directory.GetFiles(Path.Combine(Core.BaseDirectory, "Scripts/Output"), mask);

				foreach (string file in files)
				{
					try
					{
						File.Delete(file);
					}
					catch
					{ }
				}
			}
			catch
			{ }
		}

		private delegate CompilerResults Compiler(bool debug);

		public static bool Compile()
		{
			return Compile(false);
		}

		public static bool Compile(bool debug)
		{
			return Compile(debug, true);
		}

		public static bool Compile(bool debug, bool cache)
		{
			EnsureDirectory("Scripts/");
			EnsureDirectory("Scripts/Output/");

			if (m_AdditionalReferences.Count > 0)
			{
				m_AdditionalReferences.Clear();
			}

			var assemblies = new List<Assembly>();

			Assembly assembly;

			if (CompileCSScripts(debug, cache, out assembly))
			{
				if (assembly != null)
				{
					assemblies.Add(assembly);
				}
			}
			else
			{
				return false;
			}

			if (Core.VBdotNet)
			{
				if (CompileVBScripts(debug, cache, out assembly))
				{
					if (assembly != null)
					{
						assemblies.Add(assembly);
					}
				}
				else
				{
					return false;
				}
			}
            else
            {
                Utility.PushColor(ConsoleColor.White);
                ConsoleLog.Write("G.O.W: Skipping VB.NET Scripts");
                Utility.PushColor(ConsoleColor.DarkGray);
                ConsoleLog.Write("...(use -vb to enable)..................");
                Utility.PushColor(ConsoleColor.Green);
                ConsoleLog.WriteLine("[Success]");
                Utility.PopColor();
            }

			if (assemblies.Count == 0)
			{
				return false;
			}

			m_Assemblies = assemblies.ToArray();

            Utility.PushColor(ConsoleColor.White);
            ConsoleLog.Write("G.O.W: Verify Scripts");
            Utility.PushColor(ConsoleColor.DarkGray);
            ConsoleLog.WriteLog(".");
            ConsoleLog.WriteConsole(".................................................");
            Utility.PopColor();

            Core.VerifySerialization();
            string text = "...................";
            string text2 = string.Format("({0} items, {1} mobiles, {2} custom)", Core.ScriptItems, Core.ScriptMobiles, Core.ScriptCustoms);
            int num = text2.Length - 30;
            if (num < 0)
            {
                num = 0;
            }
            num = 18 - num;
            Utility.PushColor(ConsoleColor.Cyan);
            Console.SetCursorPosition(22, Console.CursorTop);
            ConsoleLog.Write(text2);
            Console.SetCursorPosition(70, Console.CursorTop);
            Utility.PushColor(ConsoleColor.Green);
            ConsoleLog.WriteLineConsole("[Success]");
            ConsoleLog.WriteLog(text.Substring(0, num));
            ConsoleLog.WriteLineLog("[Success]");
            Utility.PopColor();
            return true;
		}

		public static void Invoke(string method)
		{
			var invoke = new List<MethodInfo>();

			for (int a = 0; a < m_Assemblies.Length; ++a)
			{
				Type[] types = m_Assemblies[a].GetTypes();

				for (int i = 0; i < types.Length; ++i)
				{
					MethodInfo m = types[i].GetMethod(method, BindingFlags.Static | BindingFlags.Public);

					if (m != null)
					{
						invoke.Add(m);
					}
				}
			}

			invoke.Sort(new CallPriorityComparer());

			for (int i = 0; i < invoke.Count; ++i)
			{
				invoke[i].Invoke(null, null);
			}
		}

		private static readonly Dictionary<Assembly, TypeCache> m_TypeCaches = new Dictionary<Assembly, TypeCache>();
		private static TypeCache m_NullCache;

		public static TypeCache GetTypeCache(Assembly asm)
		{
			if (asm == null)
			{
				if (m_NullCache == null)
				{
					m_NullCache = new TypeCache(null);
				}

				return m_NullCache;
			}

			TypeCache c = null;
			m_TypeCaches.TryGetValue(asm, out c);

			if (c == null)
			{
				m_TypeCaches[asm] = c = new TypeCache(asm);
			}

			return c;
		}

		public static Type FindTypeByFullName(string fullName)
		{
			return FindTypeByFullName(fullName, true);
		}

		public static Type FindTypeByFullName(string fullName, bool ignoreCase)
		{
			Type type = null;

			for (int i = 0; type == null && i < m_Assemblies.Length; ++i)
			{
				type = GetTypeCache(m_Assemblies[i]).GetTypeByFullName(fullName, ignoreCase);
			}

			if (type == null)
			{
				type = GetTypeCache(Core.Assembly).GetTypeByFullName(fullName, ignoreCase);
			}

			return type;
		}

		public static Type FindTypeByName(string name)
		{
			return FindTypeByName(name, true);
		}

		public static Type FindTypeByName(string name, bool ignoreCase)
		{
			Type type = null;

			for (int i = 0; type == null && i < m_Assemblies.Length; ++i)
			{
				type = GetTypeCache(m_Assemblies[i]).GetTypeByName(name, ignoreCase);
			}

			if (type == null)
			{
				type = GetTypeCache(Core.Assembly).GetTypeByName(name, ignoreCase);
			}

			return type;
		}

		public static void EnsureDirectory(string dir)
		{
			string path = Path.Combine(Core.BaseDirectory, dir);

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		public static string[] GetScripts(string filter)
		{
			var list = new List<string>();

			GetScripts(list, Path.Combine(Core.BaseDirectory, "Scripts"), filter);

			return list.ToArray();
		}

		public static void GetScripts(List<string> list, string path, string filter)
		{
			foreach (string dir in Directory.GetDirectories(path))
			{
				GetScripts(list, dir, filter);
			}

			list.AddRange(Directory.GetFiles(path, filter));
		}
	}

	public class TypeCache
	{
		private readonly Type[] m_Types;
		private readonly TypeTable m_Names;
		private readonly TypeTable m_FullNames;

		public Type[] Types { get { return m_Types; } }
		public TypeTable Names { get { return m_Names; } }
		public TypeTable FullNames { get { return m_FullNames; } }

		public Type GetTypeByName(string name, bool ignoreCase)
		{
			return m_Names.Get(name, ignoreCase);
		}

		public Type GetTypeByFullName(string fullName, bool ignoreCase)
		{
			return m_FullNames.Get(fullName, ignoreCase);
		}

		public TypeCache(Assembly asm)
		{
			if (asm == null)
			{
				m_Types = Type.EmptyTypes;
			}
			else
			{
				m_Types = asm.GetTypes();
			}

			m_Names = new TypeTable(m_Types.Length);
			m_FullNames = new TypeTable(m_Types.Length);

			Type typeofTypeAliasAttribute = typeof(TypeAliasAttribute);

			for (int i = 0; i < m_Types.Length; ++i)
			{
				Type type = m_Types[i];

				m_Names.Add(type.Name, type);
				m_FullNames.Add(type.FullName, type);

				if (type.IsDefined(typeofTypeAliasAttribute, false))
				{
					object[] attrs = type.GetCustomAttributes(typeofTypeAliasAttribute, false);

					if (attrs != null && attrs.Length > 0)
					{
						var attr = attrs[0] as TypeAliasAttribute;

						if (attr != null)
						{
							for (int j = 0; j < attr.Aliases.Length; ++j)
							{
								m_FullNames.Add(attr.Aliases[j], type);
							}
						}
					}
				}
			}
		}
	}

	public class TypeTable
	{
		private readonly Dictionary<string, Type> m_Sensitive;
		private readonly Dictionary<string, Type> m_Insensitive;

		public void Add(string key, Type type)
		{
			m_Sensitive[key] = type;
			m_Insensitive[key] = type;
		}

		public Type Get(string key, bool ignoreCase)
		{
			Type t = null;

			if (ignoreCase)
			{
				m_Insensitive.TryGetValue(key, out t);
			}
			else
			{
				m_Sensitive.TryGetValue(key, out t);
			}

			return t;
		}

		public TypeTable(int capacity)
		{
			m_Sensitive = new Dictionary<string, Type>(capacity);
			m_Insensitive = new Dictionary<string, Type>(capacity, StringComparer.OrdinalIgnoreCase);
		}
	}
}