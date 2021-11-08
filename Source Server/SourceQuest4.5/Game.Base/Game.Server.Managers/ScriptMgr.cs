using log4net;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Game.Server.Managers
{
	public class ScriptMgr
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static readonly Dictionary<string, Assembly> m_scripts = new Dictionary<string, Assembly>();

		public static Assembly[] Scripts
		{
			get
			{
				lock (m_scripts)
				{
					return m_scripts.Values.ToArray();
				}
			}
		}

		public static bool InsertAssembly(Assembly ass)
		{
			lock (m_scripts)
			{
				if (!m_scripts.ContainsKey(ass.FullName))
				{
					m_scripts.Add(ass.FullName, ass);
					return true;
				}
				return false;
			}
		}

		public static bool RemoveAssembly(Assembly ass)
		{
			lock (m_scripts)
			{
				return m_scripts.Remove(ass.FullName);
			}
		}

		public static bool CompileScripts(bool compileVB, string path, string dllName, string[] asm_names)
		{
			if (!path.EndsWith("\\") && !path.EndsWith("/"))
			{
				path += "/";
			}
			ArrayList arrayList = ParseDirectory(new DirectoryInfo(path), compileVB ? "*.vb" : "*.cs", deep: true);
			if (arrayList.Count == 0)
			{
				return true;
			}
			if (File.Exists(dllName))
			{
				File.Delete(dllName);
			}
			CompilerResults compilerResults = null;
			try
			{
				CodeDomProvider codeDomProvider = null;
				codeDomProvider = ((!compileVB) ? ((CodeDomProvider)new CSharpCodeProvider()) : ((CodeDomProvider)new VBCodeProvider()));
				CompilerParameters compilerParameters = new CompilerParameters(asm_names, dllName, includeDebugInformation: true);
				compilerParameters.GenerateExecutable = false;
				compilerParameters.GenerateInMemory = false;
				compilerParameters.WarningLevel = 2;
				compilerParameters.CompilerOptions = "/lib:.";
				string[] array = new string[arrayList.Count];
				for (int i = 0; i < arrayList.Count; i++)
				{
					array[i] = ((FileInfo)arrayList[i]).FullName;
				}
				compilerResults = codeDomProvider.CompileAssemblyFromFile(compilerParameters, array);
				GC.Collect();
				if (compilerResults.Errors.HasErrors)
				{
					foreach (CompilerError error in compilerResults.Errors)
					{
						if (!error.IsWarning)
						{
							StringBuilder stringBuilder = new StringBuilder();
							stringBuilder.Append("   ");
							stringBuilder.Append(error.FileName);
							stringBuilder.Append(" Line:");
							stringBuilder.Append(error.Line);
							stringBuilder.Append(" Col:");
							stringBuilder.Append(error.Column);
							if (log.IsErrorEnabled)
							{
								log.Error("Script compilation failed because: ");
								log.Error(error.ErrorText);
								log.Error(stringBuilder.ToString());
							}
						}
					}
					return false;
				}
			}
			catch (Exception exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("CompileScripts", exception);
				}
			}
			if (compilerResults != null && !compilerResults.Errors.HasErrors)
			{
				InsertAssembly(compilerResults.CompiledAssembly);
			}
			return true;
		}

		private static ArrayList ParseDirectory(DirectoryInfo path, string filter, bool deep)
		{
			ArrayList arrayList = new ArrayList();
			if (!path.Exists)
			{
				return arrayList;
			}
			arrayList.AddRange(path.GetFiles(filter));
			if (deep)
			{
				DirectoryInfo[] directories = path.GetDirectories();
				foreach (DirectoryInfo path2 in directories)
				{
					arrayList.AddRange(ParseDirectory(path2, filter, deep));
				}
			}
			return arrayList;
		}

		public static Type GetType(string name)
		{
			Assembly[] scripts = Scripts;
			for (int i = 0; i < scripts.Length; i++)
			{
				Type type = scripts[i].GetType(name);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}

		public static object CreateInstance(string name)
		{
			Assembly[] scripts = Scripts;
			for (int i = 0; i < scripts.Length; i++)
			{
				Type type = scripts[i].GetType(name);
				if (type != null && type.IsClass)
				{
					return Activator.CreateInstance(type);
				}
			}
			return null;
		}

		public static object CreateInstance(string name, Type baseType)
		{
			Assembly[] scripts = Scripts;
			for (int i = 0; i < scripts.Length; i++)
			{
				Type type = scripts[i].GetType(name);
				if (type != null && type.IsClass && baseType.IsAssignableFrom(type))
				{
					return Activator.CreateInstance(type);
				}
			}
			return null;
		}

		public static Type[] GetDerivedClasses(Type baseType)
		{
			if (baseType == null)
			{
				return new Type[0];
			}
			ArrayList arrayList = new ArrayList();
			foreach (Assembly item in new ArrayList(Scripts))
			{
				Type[] types = item.GetTypes();
				foreach (Type type in types)
				{
					if (type.IsClass && baseType.IsAssignableFrom(type))
					{
						arrayList.Add(type);
					}
				}
			}
			return (Type[])arrayList.ToArray(typeof(Type));
		}

		public static Type[] GetImplementedClasses(string baseInterface)
		{
			ArrayList arrayList = new ArrayList();
			foreach (Assembly item in new ArrayList(Scripts))
			{
				Type[] types = item.GetTypes();
				foreach (Type type in types)
				{
					if (type.IsClass && type.GetInterface(baseInterface) != null)
					{
						arrayList.Add(type);
					}
				}
			}
			return (Type[])arrayList.ToArray(typeof(Type));
		}
	}
}
