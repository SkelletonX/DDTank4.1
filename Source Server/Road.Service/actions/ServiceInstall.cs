// Decompiled with JetBrains decompiler
// Type: Game.Service.actions.ServiceInstall
// Assembly: Road.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 39736CC6-5447-4D8B-81FB-29999A4887F8
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Road.Service.exe

using System;
using System.Collections;
using System.Configuration.Install;
using System.Reflection;
using System.Text;

namespace Game.Service.actions
{
  public class ServiceInstall : IAction
  {
    public string Name
    {
      get
      {
        return "--serviceinstall";
      }
    }

    public string Syntax
    {
      get
      {
        return "--serviceinstall";
      }
    }

    public string Description
    {
      get
      {
        return "Installs DOL as system service with he given parameters";
      }
    }

    public void OnAction(Hashtable parameters)
    {
      ArrayList arrayList = new ArrayList();
      arrayList.Add((object) "/LogToConsole=false");
      StringBuilder stringBuilder = new StringBuilder();
      foreach (DictionaryEntry parameter in parameters)
      {
        if (stringBuilder.Length > 0)
          stringBuilder.Append(" ");
        stringBuilder.Append(parameter.Key);
        stringBuilder.Append("=");
        stringBuilder.Append(parameter.Value);
      }
      arrayList.Add((object) ("commandline=" + stringBuilder.ToString()));
      AssemblyInstaller assemblyInstaller = new AssemblyInstaller(Assembly.GetExecutingAssembly(), (string[]) arrayList.ToArray(typeof (string)));
      Hashtable hashtable = new Hashtable();
      if (GameServerService.GetDOLService() != null)
      {
        Console.WriteLine("DOL service is already installed!");
      }
      else
      {
        Console.WriteLine("Installing Road as system service...");
        try
        {
          assemblyInstaller.Install((IDictionary) hashtable);
          assemblyInstaller.Commit((IDictionary) hashtable);
        }
        catch (Exception ex)
        {
          assemblyInstaller.Rollback((IDictionary) hashtable);
          Console.WriteLine("Error installing as system service");
          Console.WriteLine(ex.Message);
          return;
        }
        Console.WriteLine("Finished!");
      }
    }
  }
}
