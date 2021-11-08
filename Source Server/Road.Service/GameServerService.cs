// Decompiled with JetBrains decompiler
// Type: Game.Service.GameServerService
// Assembly: Road.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 39736CC6-5447-4D8B-81FB-29999A4887F8
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Road.Service.exe

using Game.Server;
using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;

namespace Game.Service
{
  public class GameServerService : ServiceBase
  {
    public GameServerService()
    {
      this.ServiceName = "ROAD";
      this.AutoLog = false;
      this.CanHandlePowerEvent = false;
      this.CanPauseAndContinue = false;
      this.CanShutdown = true;
      this.CanStop = true;
    }

    private static bool StartServer()
    {
      Directory.SetCurrentDirectory(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName);
      FileInfo fileInfo = new FileInfo("./config/serverconfig.xml");
      GameServer.CreateInstance(new GameServerConfig());
      return GameServer.Instance.Start();
    }

    private static void StopServer()
    {
      GameServer.Instance.Stop();
    }

    protected override void OnStart(string[] args)
    {
      if (!GameServerService.StartServer())
        throw new ApplicationException("Failed to start server!");
    }

    protected override void OnStop()
    {
      GameServerService.StopServer();
    }

    protected override void OnShutdown()
    {
      GameServerService.StopServer();
    }

    public static ServiceController GetDOLService()
    {
      foreach (ServiceController service in ServiceController.GetServices())
      {
        if (service.ServiceName.ToLower().Equals("ROAD"))
          return service;
      }
      return (ServiceController) null;
    }
  }
}
