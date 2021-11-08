// Decompiled with JetBrains decompiler
// Type: Game.Service.actions.ServiceStop
// Assembly: Road.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 39736CC6-5447-4D8B-81FB-29999A4887F8
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Road.Service.exe

using System;
using System.Collections;
using System.ServiceProcess;

namespace Game.Service.actions
{
  public class ServiceStop : IAction
  {
    public string Name
    {
      get
      {
        return "--servicestop";
      }
    }

    public string Syntax
    {
      get
      {
        return "--servicestop";
      }
    }

    public string Description
    {
      get
      {
        return "Stops the DOL system service";
      }
    }

    public void OnAction(Hashtable parameters)
    {
      ServiceController dolService = GameServerService.GetDOLService();
      if (dolService == null)
        Console.WriteLine("You have to install the service first!");
      else if (dolService.Status == ServiceControllerStatus.StartPending)
        Console.WriteLine("Server is still starting, please check the logfile for progress information!");
      else if (dolService.Status != ServiceControllerStatus.Running)
      {
        Console.WriteLine("The DOL service is not running");
      }
      else
      {
        try
        {
          Console.WriteLine("Stopping the DOL service...");
          dolService.Stop();
          dolService.WaitForStatus(ServiceControllerStatus.Stopped);
          Console.WriteLine("Finished!");
        }
        catch (InvalidOperationException ex)
        {
          Console.WriteLine("Could not stop the DOL service!");
          Console.WriteLine(ex.Message);
        }
      }
    }
  }
}
