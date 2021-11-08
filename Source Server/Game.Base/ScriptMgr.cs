// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.ScriptMgr
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

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
        lock (ScriptMgr.m_scripts)
          return ScriptMgr.m_scripts.Values.ToArray<Assembly>();
      }
    }

    public static bool InsertAssembly(Assembly ass)
    {
      lock (ScriptMgr.m_scripts)
      {
        if (ScriptMgr.m_scripts.ContainsKey(ass.FullName))
          return false;
        ScriptMgr.m_scripts.Add(ass.FullName, ass);
        return true;
      }
    }

    public static bool RemoveAssembly(Assembly ass)
    {
      lock (ScriptMgr.m_scripts)
        return ScriptMgr.m_scripts.Remove(ass.FullName);
    }

    public static bool CompileScripts(
      bool compileVB,
      string path,
      string dllName,
      string[] asm_names)
    {
      if (!path.EndsWith("\\") && !path.EndsWith("/"))
        path += "/";
      ArrayList directory = ScriptMgr.ParseDirectory(new DirectoryInfo(path), compileVB ? "*.vb" : "*.cs", true);
      if (directory.Count == 0)
        return true;
      if (File.Exists(dllName))
        File.Delete(dllName);
      CompilerResults compilerResults = (CompilerResults) null;
      try
      {
        CodeDomProvider codeDomProvider = !compileVB ? (CodeDomProvider) new CSharpCodeProvider() : (CodeDomProvider) new VBCodeProvider();
        CompilerParameters options = new CompilerParameters(asm_names, dllName, true);
        options.GenerateExecutable = false;
        options.GenerateInMemory = false;
        options.WarningLevel = 2;
        options.CompilerOptions = "/lib:.";
        string[] strArray = new string[directory.Count];
        for (int index = 0; index < directory.Count; ++index)
          strArray[index] = ((FileSystemInfo) directory[index]).FullName;
        compilerResults = codeDomProvider.CompileAssemblyFromFile(options, strArray);
        GC.Collect();
        if (compilerResults.Errors.HasErrors)
        {
          foreach (CompilerError error in (CollectionBase) compilerResults.Errors)
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
              if (ScriptMgr.log.IsErrorEnabled)
              {
                ScriptMgr.log.Error((object) "Script compilation failed because: ");
                ScriptMgr.log.Error((object) error.ErrorText);
                ScriptMgr.log.Error((object) stringBuilder.ToString());
              }
            }
          }
          return false;
        }
      }
      catch (Exception ex)
      {
        if (ScriptMgr.log.IsErrorEnabled)
          ScriptMgr.log.Error((object) nameof (CompileScripts), ex);
      }
      if (compilerResults != null && !compilerResults.Errors.HasErrors)
        ScriptMgr.InsertAssembly(compilerResults.CompiledAssembly);
      return true;
    }

    private static ArrayList ParseDirectory(DirectoryInfo path, string filter, bool deep)
    {
      ArrayList arrayList = new ArrayList();
      if (!path.Exists)
        return arrayList;
      arrayList.AddRange((ICollection) path.GetFiles(filter));
      if (deep)
      {
        foreach (DirectoryInfo directory in path.GetDirectories())
          arrayList.AddRange((ICollection) ScriptMgr.ParseDirectory(directory, filter, deep));
      }
      return arrayList;
    }

    public static Type GetType(string name)
    {
      foreach (Assembly script in ScriptMgr.Scripts)
      {
        Type type = script.GetType(name);
        if (type != (Type) null)
          return type;
      }
      return (Type) null;
    }

    public static object CreateInstance(string name)
    {
      foreach (Assembly script in ScriptMgr.Scripts)
      {
        Type type = script.GetType(name);
        if (type != (Type) null && type.IsClass)
          return Activator.CreateInstance(type);
      }
      Console.WriteLine(name);
      return (object) null;
    }

    public static object CreateInstance(string name, Type baseType)
    {
      foreach (Assembly script in ScriptMgr.Scripts)
      {
        Type type = script.GetType(name);
        if (type != (Type) null && type.IsClass && baseType.IsAssignableFrom(type))
          return Activator.CreateInstance(type);
      }
      return (object) null;
    }

    public static Type[] GetDerivedClasses(Type baseType)
    {
      if (baseType == (Type) null)
        return new Type[0];
      ArrayList arrayList = new ArrayList();
      foreach (Assembly assembly in new ArrayList((ICollection) ScriptMgr.Scripts))
      {
        foreach (Type type in assembly.GetTypes())
        {
          if (type.IsClass && baseType.IsAssignableFrom(type))
            arrayList.Add((object) type);
        }
      }
      return (Type[]) arrayList.ToArray(typeof (Type));
    }

    public static Type[] GetImplementedClasses(string baseInterface)
    {
      ArrayList arrayList = new ArrayList();
      foreach (Assembly assembly in new ArrayList((ICollection) ScriptMgr.Scripts))
      {
        foreach (Type type in assembly.GetTypes())
        {
          if (type.IsClass && type.GetInterface(baseInterface) != (Type) null)
            arrayList.Add((object) type);
        }
      }
      return (Type[]) arrayList.ToArray(typeof (Type));
    }
  }
}
