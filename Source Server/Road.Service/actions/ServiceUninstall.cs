// Decompiled with JetBrains decompiler
// Type: Game.Service.actions.ServiceUninstall
// Assembly: Road.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 39736CC6-5447-4D8B-81FB-29999A4887F8
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Road.Service.exe

using System;
using System.Collections;
using System.Configuration.Install;
using System.Reflection;

namespace Game.Service.actions
{
  public class ServiceUninstall : IAction
  {
    public string Name
    {
      get
      {
        return "--serviceuninstall";
      }
    }

    public string Syntax
    {
      get
      {
        return "--serviceuninstall";
      }
    }

    public string Description
    {
      get
      {
        return "Uninstalls the DOL system service";
      }
    }

    public void OnAction(Hashtable parameters)
    {
      AssemblyInstaller assemblyInstaller = new AssemblyInstaller(Assembly.GetExecutingAssembly(), new string[1]
      {
        "/LogToConsole=false"
      });
      Hashtable hashtable = new Hashtable();
      if (GameServerService.GetDOLService() == null)
      {
        Console.WriteLine("No service named \"DOL\" found!");
      }
      else
      {
        Console.WriteLine("Uninstalling DOL system service...");
        try
        {
          assemblyInstaller.Uninstall((IDictionary) hashtable);
        }
        catch (Exception ex)
        {
          Console.WriteLine("Error uninstalling system service");
          Console.WriteLine(ex.Message);
          return;
        }
        Console.WriteLine("Finished!");
      }
    }
  }
}
