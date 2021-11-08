// Decompiled with JetBrains decompiler
// Type: Fighting.Server.FightServer
// Assembly: Fighting.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8F1EB855-F1B7-44B3-B212-6508DAF33CC5
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Fight\Fighting.Server.dll

using Bussiness;
using Bussiness.Managers;
using Fighting.Server.Games;
using Fighting.Server.Rooms;
using Game.Base;
using Game.Base.Events;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Managers;
using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;

namespace Fighting.Server
{
  public class FightServer : BaseServer
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static bool m_compiled = false;
    private FightServerConfig m_config;
    private static FightServer m_instance;
    private bool m_running;

    private FightServer(FightServerConfig config)
    {
      this.m_config = config;
    }

    public static void CreateInstance(FightServerConfig config)
    {
      if (FightServer.m_instance != null)
        return;
      FileInfo configFile = new FileInfo(config.LogConfigFile);
      if (!configFile.Exists)
        ResourceUtil.ExtractResource(configFile.Name, configFile.FullName, Assembly.GetAssembly(typeof (FightServer)));
      XmlConfigurator.ConfigureAndWatch(configFile);
      FightServer.m_instance = new FightServer(config);
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      FightServer.log.Fatal((object) ("Unhandled exception!\n" + e.ExceptionObject.ToString()));
      if (!e.IsTerminating)
        return;
      LogManager.Shutdown();
    }

    public ServerClient[] GetAllClients()
    {
      ServerClient[] serverClientArray = (ServerClient[]) null;
      lock (this._clients.SyncRoot)
      {
        serverClientArray = new ServerClient[this._clients.Count];
        this._clients.Keys.CopyTo((Array) serverClientArray, 0);
      }
      return serverClientArray;
    }

    protected override BaseClient GetNewClient()
    {
      return (BaseClient) new ServerClient(this);
    }

    protected bool InitComponent(bool componentInitState, string text)
    {
      FightServer.log.Info((object) (text + ": " + componentInitState.ToString()));
      if (!componentInitState)
        this.Stop();
      return componentInitState;
    }

    public bool RecompileScripts()
    {
      string str = "Game.Base.dll,Game.Logic.dll,SqlDataProvider.dll,Bussiness.dll,System.Drawing.dll";
      if (!FightServer.m_compiled)
      {
        string path = this.Configuration.RootDirectory + Path.DirectorySeparatorChar.ToString() + "scripts";
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        string[] asm_names = str.Split(',');
        FightServer.m_compiled = ScriptMgr.CompileScripts(false, path, this.Configuration.ScriptCompilationTarget, asm_names);
      }
      return FightServer.m_compiled;
    }

    public void SendToALL(GSPacketIn pkg)
    {
      this.SendToALL(pkg, (ServerClient) null);
    }

    public void SendToALL(GSPacketIn pkg, ServerClient except)
    {
      ServerClient[] allClients = this.GetAllClients();
      if (allClients == null)
        return;
      foreach (ServerClient serverClient in allClients)
      {
        if (serverClient != except)
          serverClient.SendTCP(pkg);
      }
    }

    public override bool Start()
    {
      if (this.m_running)
        return false;
      bool flag;
      try
      {
        this.m_running = true;
        Thread.CurrentThread.Priority = ThreadPriority.Normal;
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);
        if (!this.InitComponent(this.InitSocket(IPAddress.Parse(m_config.Ip), this.m_config.Port), "InitSocket Port:" + (object) this.m_config.Port) || !this.InitComponent(this.RecompileScripts(), "Recompile Scripts") || (!this.InitComponent(this.StartScriptComponents(), "Script components") || !this.InitComponent(ProxyRoomMgr.Setup(), "RoomMgr.Setup")) || (!this.InitComponent(GameMgr.Setup(0, 4), "GameMgr.Setup") || !this.InitComponent(MapMgr.Init(), "MapMgr Init") || (!this.InitComponent(ItemMgr.Init(), "ItemMgr Init") || !this.InitComponent(PropItemMgr.Init(), "PropItemMgr Init"))) || (!this.InitComponent(BallMgr.Init(), "BallMgr Init") || !this.InitComponent(BallConfigMgr.Init(), "BallConfigMgr Init") || (!this.InitComponent(DropMgr.Init(), "DropMgr Init") || !this.InitComponent(NPCInfoMgr.Init(), "NPCInfoMgr Init")) || (!this.InitComponent(WindMgr.Init(), "WindMgr Init") || !this.InitComponent(LanguageMgr.Setup(""), "LanguageMgr Init"))))
          return false;
        GameEventMgr.Notify((RoadEvent) ScriptEvent.Loaded);
        if (!this.InitComponent(base.Start(), "base.Start()"))
        {
          flag = false;
        }
        else
        {
          ProxyRoomMgr.Start();
          GameMgr.Start();
          GameEventMgr.Notify((RoadEvent) GameServerEvent.Started, (object) this);
          GC.Collect(GC.MaxGeneration);
          FightServer.log.Info((object) "GameServer is now open for connections!");
          flag = true;
        }
      }
      catch (Exception ex)
      {
        FightServer.log.Error((object) "Failed to start the server", ex);
        flag = false;
      }
      return flag;
    }

    protected bool StartScriptComponents()
    {
      try
      {
        ScriptMgr.InsertAssembly(typeof (FightServer).Assembly);
        ScriptMgr.InsertAssembly(typeof (BaseGame).Assembly);
        foreach (Assembly script in ScriptMgr.Scripts)
        {
          GameEventMgr.RegisterGlobalEvents(script, typeof (GameServerStartedEventAttribute), (RoadEvent) GameServerEvent.Started);
          GameEventMgr.RegisterGlobalEvents(script, typeof (GameServerStoppedEventAttribute), (RoadEvent) GameServerEvent.Stopped);
          GameEventMgr.RegisterGlobalEvents(script, typeof (ScriptLoadedEventAttribute), (RoadEvent) ScriptEvent.Loaded);
          GameEventMgr.RegisterGlobalEvents(script, typeof (ScriptUnloadedEventAttribute), (RoadEvent) ScriptEvent.Unloaded);
        }
        FightServer.log.Info((object) "Registering global event handlers: true");
        return true;
      }
      catch (Exception ex)
      {
        FightServer.log.Error((object) nameof (StartScriptComponents), ex);
        return false;
      }
    }

    public override void Stop()
    {
      if (!this.m_running)
        return;
      try
      {
        this.m_running = false;
        GameMgr.Stop();
        ProxyRoomMgr.Stop();
      }
      catch (Exception ex)
      {
        FightServer.log.Error((object) "Server stopp error:", ex);
      }
      finally
      {
        base.Stop();
      }
    }

    public FightServerConfig Configuration
    {
      get
      {
        return this.m_config;
      }
    }

    public static FightServer Instance
    {
      get
      {
        return FightServer.m_instance;
      }
    }
  }
}
