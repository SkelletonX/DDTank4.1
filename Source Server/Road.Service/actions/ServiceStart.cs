// Decompiled with JetBrains decompiler
// Type: Game.Service.actions.ServiceStart
// Assembly: Road.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 39736CC6-5447-4D8B-81FB-29999A4887F8
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Road.Service.exe

using System;
using System.Collections;
using System.ServiceProcess;

namespace Game.Service.actions
{
  public class ServiceStart : IAction
  {
    public string Name
    {
      get
      {
        return "--servicestart";
      }
    }

    public string Syntax
    {
      get
      {
        return "--servicestart";
      }
    }

    public string Description
    {
      get
      {
        return "Starts the DOL system service";
      }
    }

    public void OnAction(Hashtable parameters)
    {
      ServiceController dolService = GameServerService.GetDOLService();
      if (dolService == null)
        Console.WriteLine("You have to install the service first!");
      else if (dolService.Status == ServiceControllerStatus.StartPending)
        Console.WriteLine("Server is still starting, please check the logfile for progress information!");
      else if (dolService.Status != ServiceControllerStatus.Stopped)
      {
        Console.WriteLine("The DOL service is not stopped");
      }
      else
      {
        try
        {
          Console.WriteLine("Starting the DOL service...");
          dolService.Start();
          dolService.WaitForStatus(ServiceControllerStatus.StartPending, TimeSpan.FromSeconds(10.0));
          Console.WriteLine("Starting can take some time, please check the logfile for progress information!");
          Console.WriteLine("Finished!");
        }
        catch (InvalidOperationException ex)
        {
          Console.WriteLine("Could not start the DOL service!");
          Console.WriteLine(ex.Message);
        }
        catch (System.ServiceProcess.TimeoutException ex)
        {
          Console.WriteLine("Error starting the service, please check the logfile for further info!");
        }
      }
    }
  }
}
