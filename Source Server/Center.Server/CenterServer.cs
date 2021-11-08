// Decompiled with JetBrains decompiler
// Type: Center.Server.CenterServer
// Assembly: Center.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4391F35A-9CAD-44A9-AFE0-208E0547A0DD
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Center\Center.Server.dll

using Bussiness;
using Bussiness.Protocol;
using Center.Server.Managers;
using Center.Server.Statics;
using Game.Base;
using Game.Base.Events;
using Game.Base.Packets;
using Game.Server.Managers;
using log4net;
using log4net.Config;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;

namespace Center.Server
{
  public class CenterServer : BaseServer
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private readonly int awardTime = 20;
    private bool boss = true;
    private bool close = true;
    private string Edition = "2612558";
    private int minute = 5;
    private Random rand = new Random();
    private bool _aSSState;
    private CenterServerConfig _config;
    private bool _dailyAwardState;
    private static CenterServer _instance;
    private Timer m_consortiaboss;
    private Timer m_loginLapseTimer;
    private Timer m_saveDBTimer;
    private Timer m_saveRecordTimer;
    private Timer m_scanAuction;
    private Timer m_scanConsortia;
    private Timer m_scanMail;
    private Timer m_sytemNotice;
    private Timer m_worldEvent;
    private DateTime minute1;

    public CenterServer(CenterServerConfig config)
    {
      this._config = config;
      this.LoadConfig();
    }

    public bool AvailTime(DateTime startTime, int min)
    {
      TimeSpan timeSpan = DateTime.Now - startTime;
      return min - (int) timeSpan.TotalMinutes > 0;
    }

    public static void CreateInstance(CenterServerConfig config)
    {
      if (CenterServer.Instance != null)
        return;
      FileInfo configFile = new FileInfo(config.LogConfigFile);
      if (!configFile.Exists)
        ResourceUtil.ExtractResource(configFile.Name, configFile.FullName, Assembly.GetAssembly(typeof (CenterServer)));
      XmlConfigurator.ConfigureAndWatch(configFile);
      CenterServer._instance = new CenterServer(config);
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      CenterServer.log.Fatal((object) ("Unhandled exception!\n" + e.ExceptionObject.ToString()));
      if (!e.IsTerminating)
        return;
      LogManager.Shutdown();
    }

    public void DisposeGlobalTimers()
    {
      if (this.m_saveDBTimer != null)
        this.m_saveDBTimer.Dispose();
      if (this.m_loginLapseTimer != null)
        this.m_loginLapseTimer.Dispose();
      if (this.m_saveRecordTimer != null)
        this.m_saveRecordTimer.Dispose();
      if (this.m_scanAuction != null)
        this.m_scanAuction.Dispose();
      if (this.m_scanMail != null)
        this.m_scanMail.Dispose();
      if (this.m_scanConsortia != null)
        this.m_scanConsortia.Dispose();
      if (this.m_worldEvent != null)
        this.m_worldEvent.Dispose();
      if (this.m_sytemNotice != null)
        this.m_sytemNotice.Dispose();
      if (this.m_consortiaboss == null)
        return;
      this.m_consortiaboss.Dispose();
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
      CenterServer.log.Info((object) (text + ": " + componentInitState.ToString()));
      if (!componentInitState)
        this.Stop();
      return componentInitState;
    }

    public bool InitGlobalTimers()
    {
      int num1 = this._config.SaveIntervalInterval * 60 * 1000;
      if (this.m_saveDBTimer == null)
        this.m_saveDBTimer = new Timer(new TimerCallback(this.SaveTimerProc), (object) null, num1, num1);
      else
        this.m_saveDBTimer.Change(num1, num1);
      int num2 = this._config.SystemNoticeInterval * 60 * 1000;
      if (this.m_sytemNotice == null)
        this.m_sytemNotice = new Timer(new TimerCallback(this.SystemNoticeTimerProc), (object) null, num2, num2);
      else
        this.m_sytemNotice.Change(num2, num2);
      int num3 = this._config.LoginLapseInterval * 60 * 1000;
      if (this.m_loginLapseTimer == null)
        this.m_loginLapseTimer = new Timer(new TimerCallback(this.LoginLapseTimerProc), (object) null, num3, num3);
      else
        this.m_loginLapseTimer.Change(num3, num3);
      int num4 = this._config.SaveRecordInterval * 60 * 1000;
      if (this.m_saveRecordTimer == null)
        this.m_saveRecordTimer = new Timer(new TimerCallback(this.SaveRecordProc), (object) null, num4, num4);
      else
        this.m_saveRecordTimer.Change(num4, num4);
      int num5 = this._config.ScanAuctionInterval * 60 * 1000;
      if (this.m_scanAuction == null)
        this.m_scanAuction = new Timer(new TimerCallback(this.ScanAuctionProc), (object) null, num5, num5);
      else
        this.m_scanAuction.Change(num5, num5);
      int num6 = this._config.ScanMailInterval * 60 * 1000;
      if (this.m_scanMail == null)
        this.m_scanMail = new Timer(new TimerCallback(this.ScanMailProc), (object) null, num6, num6);
      else
        this.m_scanMail.Change(num6, num6);
      int num7 = this._config.ScanConsortiaInterval * 60 * 1000;
      if (this.m_scanConsortia == null)
        this.m_scanConsortia = new Timer(new TimerCallback(this.ScanConsortiaProc), (object) null, num7, num7);
      else
        this.m_scanConsortia.Change(num7, num7);
      int num8 = 60000;
      if (this.m_worldEvent == null)
        this.m_worldEvent = new Timer(new TimerCallback(this.ScanWorldEventProc), (object) null, num8, num8);
      else
        this.m_worldEvent.Change(num8, num8);
      int num9 = 60000;
      if (this.m_consortiaboss == null)
        this.m_consortiaboss = new Timer(new TimerCallback(this.ScanConsortiabossProc), (object) null, num9, num9);
      else
        this.m_consortiaboss.Change(num9, num9);
      return true;
    }

    public void LoadConfig()
    {
      this._aSSState = bool.Parse(ConfigurationManager.AppSettings["AAS"]);
      this._dailyAwardState = bool.Parse(ConfigurationManager.AppSettings["DailyAwardState"]);
    }

    protected void LoginLapseTimerProc(object sender)
    {
      try
      {
        Player[] allPlayer = LoginMgr.GetAllPlayer();
        long ticks = DateTime.Now.Ticks;
        long num = (long) this._config.LoginLapseInterval * 10L * 1000L;
        foreach (Player player in allPlayer)
        {
          if (player.State == ePlayerState.NotLogin)
          {
            if (player.LastTime + num < ticks)
              LoginMgr.RemovePlayer(player.Id);
          }
          else
            player.LastTime = ticks;
        }
      }
      catch (Exception ex)
      {
        if (!CenterServer.log.IsErrorEnabled)
          return;
        CenterServer.log.Error((object) "LoginLapseTimer callback", ex);
      }
    }

    public int NoticeServerUpdate(int serverId, int type)
    {
      ServerClient[] allClients = this.GetAllClients();
      if (allClients != null)
      {
        foreach (ServerClient serverClient in allClients)
        {
          if (serverClient.Info.ID == serverId)
          {
            GSPacketIn pkg = new GSPacketIn((short) 11);
            pkg.WriteInt(type);
            serverClient.SendTCP(pkg);
            return 0;
          }
        }
      }
      return 1;
    }

    public int RateUpdate(int serverId)
    {
      ServerClient[] allClients = this.GetAllClients();
      if (allClients != null)
      {
        foreach (ServerClient serverClient in allClients)
        {
          if (serverClient.Info.ID == serverId)
          {
            GSPacketIn pkg = new GSPacketIn((short) 177);
            pkg.WriteInt(serverId);
            serverClient.SendTCP(pkg);
            return 0;
          }
        }
      }
      return 1;
    }

    public bool RecompileScripts()
    {
      string path = this._config.RootDirectory + Path.DirectorySeparatorChar.ToString() + "scripts";
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      string[] asm_names = this._config.ScriptAssemblies.Split(',');
      return ScriptMgr.CompileScripts(false, path, this._config.ScriptCompilationTarget, asm_names);
    }

    protected void SaveRecordProc(object sender)
    {
      try
      {
        int tickCount = Environment.TickCount;
        if (CenterServer.log.IsInfoEnabled)
        {
          CenterServer.log.Info((object) "Saving Record...");
          CenterServer.log.Debug((object) ("Save ThreadId=" + (object) Thread.CurrentThread.ManagedThreadId));
        }
        ThreadPriority priority = Thread.CurrentThread.Priority;
        Thread.CurrentThread.Priority = ThreadPriority.Lowest;
        LogMgr.Save();
        Thread.CurrentThread.Priority = priority;
        int num = Environment.TickCount - tickCount;
        if (CenterServer.log.IsInfoEnabled)
          CenterServer.log.Info((object) "Saving Record complete!");
        if (num <= 120000)
          return;
        CenterServer.log.WarnFormat("Saved all Record  in {0} ms!", (object) num);
      }
      catch (Exception ex)
      {
        if (!CenterServer.log.IsErrorEnabled)
          return;
        CenterServer.log.Error((object) nameof (SaveRecordProc), ex);
      }
    }

    protected void SaveTimerProc(object state)
    {
      try
      {
        int tickCount = Environment.TickCount;
        if (CenterServer.log.IsInfoEnabled)
        {
          CenterServer.log.Info((object) "Saving database...");
          CenterServer.log.Debug((object) ("Save ThreadId=" + (object) Thread.CurrentThread.ManagedThreadId));
        }
        ThreadPriority priority = Thread.CurrentThread.Priority;
        Thread.CurrentThread.Priority = ThreadPriority.Lowest;
        ServerMgr.SaveToDatabase();
        Thread.CurrentThread.Priority = priority;
        int num = Environment.TickCount - tickCount;
        if (!CenterServer.log.IsInfoEnabled)
          return;
        CenterServer.log.Info((object) "Saving database complete!");
        CenterServer.log.Info((object) ("Saved all databases " + (object) num + "ms"));
      }
      catch (Exception ex)
      {
        if (!CenterServer.log.IsErrorEnabled)
          return;
        CenterServer.log.Error((object) nameof (SaveTimerProc), ex);
      }
    }

    protected void ScanAuctionProc(object sender)
    {
      try
      {
        int tickCount = Environment.TickCount;
        if (CenterServer.log.IsInfoEnabled)
        {
          CenterServer.log.Info((object) "Saving Record...");
          CenterServer.log.Debug((object) ("Save ThreadId=" + (object) Thread.CurrentThread.ManagedThreadId));
        }
        ThreadPriority priority = Thread.CurrentThread.Priority;
        Thread.CurrentThread.Priority = ThreadPriority.Lowest;
        string noticeUserID = "";
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
          playerBussiness.ScanAuction(ref noticeUserID, GameProperties.Cess);
        string str = noticeUserID;
        char[] chArray = new char[1]{ ',' };
        foreach (string s in str.Split(chArray))
        {
          if (!string.IsNullOrEmpty(s))
          {
            GSPacketIn pkg = new GSPacketIn((short) 117);
            pkg.WriteInt(int.Parse(s));
            pkg.WriteInt(1);
            this.SendToALL(pkg);
          }
        }
        Thread.CurrentThread.Priority = priority;
        int num = Environment.TickCount - tickCount;
        if (CenterServer.log.IsInfoEnabled)
          CenterServer.log.Info((object) "Scan Auction complete!");
        if (num <= 120000)
          return;
        CenterServer.log.WarnFormat("Scan all Auction  in {0} ms", (object) num);
      }
      catch (Exception ex)
      {
        if (!CenterServer.log.IsErrorEnabled)
          return;
        CenterServer.log.Error((object) nameof (ScanAuctionProc), ex);
      }
    }

    protected void ScanConsortiabossProc(object sender)
    {
      try
      {
        int tickCount = Environment.TickCount;
        if (CenterServer.log.IsInfoEnabled)
        {
          CenterServer.log.Info((object) "Scan Consortiaboss...");
          CenterServer.log.Debug((object) ("Scan ThreadId=" + (object) Thread.CurrentThread.ManagedThreadId));
        }
        ThreadPriority priority = Thread.CurrentThread.Priority;
        Thread.CurrentThread.Priority = ThreadPriority.Lowest;
        ConsortiaBossMgr.UpdateTime();
        ++ConsortiaBossMgr.TimeCheckingAward;
        if (ConsortiaBossMgr.TimeCheckingAward > 5)
        {
          List<int> consortiaGetAward = ConsortiaBossMgr.GetAllConsortiaGetAward();
          GSPacketIn pkg = new GSPacketIn((short) 185);
          pkg.WriteInt(consortiaGetAward.Count);
          foreach (int val in consortiaGetAward)
            pkg.WriteInt(val);
          this.SendToALL(pkg);
          ConsortiaBossMgr.TimeCheckingAward = 0;
          CenterServer.log.Info((object) "Scan Consortiaboss award complete!");
        }
        Thread.CurrentThread.Priority = priority;
        int num = Environment.TickCount - tickCount;
        if (!CenterServer.log.IsInfoEnabled)
          return;
        CenterServer.log.Info((object) "Scan Consortiaboss complete!");
      }
      catch (Exception ex)
      {
        if (!CenterServer.log.IsErrorEnabled)
          return;
        CenterServer.log.Error((object) nameof (ScanConsortiabossProc), ex);
      }
    }

    protected void ScanConsortiaProc(object sender)
    {
      try
      {
        int tickCount = Environment.TickCount;
        if (CenterServer.log.IsInfoEnabled)
        {
          CenterServer.log.Info((object) "Saving Record...");
          CenterServer.log.Debug((object) ("Save ThreadId=" + (object) Thread.CurrentThread.ManagedThreadId));
        }
        ThreadPriority priority = Thread.CurrentThread.Priority;
        Thread.CurrentThread.Priority = ThreadPriority.Lowest;
        string noticeID = "";
        using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
          consortiaBussiness.ScanConsortia(ref noticeID);
        string str = noticeID;
        char[] chArray = new char[1]{ ',' };
        foreach (string s in str.Split(chArray))
        {
          if (!string.IsNullOrEmpty(s))
          {
            GSPacketIn pkg = new GSPacketIn((short) 128);
            pkg.WriteByte((byte) 2);
            pkg.WriteInt(int.Parse(s));
            this.SendToALL(pkg);
          }
        }
        Thread.CurrentThread.Priority = priority;
        int num = Environment.TickCount - tickCount;
        if (CenterServer.log.IsInfoEnabled)
          CenterServer.log.Info((object) "Scan Consortia complete!");
        if (num <= 120000)
          return;
        CenterServer.log.WarnFormat("Scan all Consortia in {0} ms", (object) num);
      }
      catch (Exception ex)
      {
        if (!CenterServer.log.IsErrorEnabled)
          return;
        CenterServer.log.Error((object) nameof (ScanConsortiaProc), ex);
      }
    }

    protected void ScanMailProc(object sender)
    {
      try
      {
        int tickCount = Environment.TickCount;
        if (CenterServer.log.IsInfoEnabled)
        {
          CenterServer.log.Info((object) "Saving Record...");
          CenterServer.log.Debug((object) ("Save ThreadId=" + (object) Thread.CurrentThread.ManagedThreadId));
        }
        ThreadPriority priority = Thread.CurrentThread.Priority;
        Thread.CurrentThread.Priority = ThreadPriority.Lowest;
        string noticeUserID = "";
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
          playerBussiness.ScanMail(ref noticeUserID);
        string str = noticeUserID;
        char[] chArray = new char[1]{ ',' };
        foreach (string s in str.Split(chArray))
        {
          if (!string.IsNullOrEmpty(s))
          {
            GSPacketIn pkg = new GSPacketIn((short) 117);
            pkg.WriteInt(int.Parse(s));
            pkg.WriteInt(1);
            this.SendToALL(pkg);
          }
        }
        Thread.CurrentThread.Priority = priority;
        int num = Environment.TickCount - tickCount;
        if (CenterServer.log.IsInfoEnabled)
          CenterServer.log.Info((object) "Scan Mail complete!");
        if (num <= 120000)
          return;
        CenterServer.log.WarnFormat("Scan all Mail in {0} ms", (object) num);
      }
      catch (Exception ex)
      {
        if (!CenterServer.log.IsErrorEnabled)
          return;
        CenterServer.log.Error((object) nameof (ScanMailProc), ex);
      }
    }

    protected void ScanWorldEventProc(object sender)
    {
      try
      {
        int tickCount = Environment.TickCount;
        if (CenterServer.log.IsInfoEnabled)
        {
          CenterServer.log.Info((object) "Scan  WorldEvent ...");
          CenterServer.log.Debug((object) ("Scan ThreadId=" + (object) Thread.CurrentThread.ManagedThreadId));
        }
        ThreadPriority priority = Thread.CurrentThread.Priority;
        Thread.CurrentThread.Priority = ThreadPriority.Lowest;
        this.SendUpdateWorldEvent();
        Thread.CurrentThread.Priority = priority;
        int num = Environment.TickCount - tickCount;
        if (!CenterServer.log.IsInfoEnabled)
          return;
        CenterServer.log.Info((object) "Scan WorldEvent complete!");
      }
      catch (Exception ex)
      {
        if (!CenterServer.log.IsErrorEnabled)
          return;
        CenterServer.log.Error((object) "Scan WorldEvent Proc", ex);
      }
    }

    public bool SendAAS(bool state)
    {
      if (!StaticFunction.UpdateConfig("Center.Service.exe.config", "AAS", state.ToString()))
        return false;
      this.ASSState = state;
      GSPacketIn pkg = new GSPacketIn((short) 7);
      pkg.WriteBoolean(state);
      this.SendToALL(pkg);
      return true;
    }

    public void SendBattleGoundOpenClose(bool value)
    {
      GSPacketIn pkg = new GSPacketIn((short) 88);
      pkg.WriteBoolean(value);
      this.SendToALL(pkg);
    }

    public void SendConfigState()
    {
      GSPacketIn pkg = new GSPacketIn((short) 8);
      pkg.WriteBoolean(this.ASSState);
      pkg.WriteBoolean(this.DailyAwardState);
      this.SendToALL(pkg);
    }

    public bool SendConfigState(int type, bool state)
    {
      string empty = string.Empty;
      string name;
      switch (type)
      {
        case 1:
          name = "AAS";
          break;
        case 2:
          name = "DailyAwardState";
          break;
        default:
          return false;
      }
      if (!StaticFunction.UpdateConfig("Center.Service.exe.config", name, state.ToString()))
        return false;
      switch (type)
      {
        case 1:
          this.ASSState = state;
          break;
        case 2:
          this.DailyAwardState = state;
          break;
      }
      this.SendConfigState();
      return true;
    }

    public void SendConsortiaDelete(int consortiaID)
    {
      GSPacketIn pkg = new GSPacketIn((short) 128);
      pkg.WriteByte((byte) 5);
      pkg.WriteInt(consortiaID);
      this.SendToALL(pkg);
    }

    public void SendFightFootballTime(bool value)
    {
      GSPacketIn pkg = new GSPacketIn((short) 89);
      pkg.WriteBoolean(value);
      this.SendToALL(pkg);
    }

    public void SendLeagueOpenClose(bool value)
    {
      GSPacketIn pkg = new GSPacketIn((short) 87);
      pkg.WriteBoolean(value);
      this.SendToALL(pkg);
    }

    public void SendPrivateInfo()
    {
      int currentPveId = WorldMgr.currentPVE_ID;
      GSPacketIn pkg = new GSPacketIn((short) 80);
      pkg.WriteLong(WorldMgr.MAX_BLOOD);
      pkg.WriteLong(WorldMgr.current_blood);
      pkg.WriteString(WorldMgr.name[currentPveId]);
      pkg.WriteString(WorldMgr.bossResourceId[currentPveId]);
      pkg.WriteInt(WorldMgr.Pve_Id[currentPveId]);
      pkg.WriteBoolean(WorldMgr.fightOver);
      pkg.WriteBoolean(WorldMgr.roomClose);
      pkg.WriteDateTime(WorldMgr.begin_time);
      pkg.WriteDateTime(WorldMgr.end_time);
      pkg.WriteInt(WorldMgr.fight_time);
      pkg.WriteBoolean(WorldMgr.worldOpen);
      this.SendToALL(pkg);
    }

    public void SendPrivateInfo(string name)
    {
      if (!WorldMgr.CheckName(name))
        return;
      GSPacketIn pkg = new GSPacketIn((short) 85);
      RankingPersonInfo singleRank = WorldMgr.GetSingleRank(name);
      pkg.WriteString(name);
      pkg.WriteInt(singleRank.Damage);
      pkg.WriteInt(singleRank.Honor);
      this.SendToALL(pkg);
    }

    public bool SendReload(eReloadType type)
    {
      return this.SendReload(type.ToString());
    }

    public bool SendReload(string str)
    {
      try
      {
        eReloadType eReloadType = (eReloadType) Enum.Parse(typeof (eReloadType), str, true);
        if (eReloadType == eReloadType.server)
        {
          this._config.Refresh();
          this.InitGlobalTimers();
          this.LoadConfig();
          ServerMgr.ReLoadServerList();
          this.SendConfigState();
        }
        GSPacketIn pkg = new GSPacketIn((short) 11);
        pkg.WriteInt((int) eReloadType);
        this.SendToALL(pkg, (ServerClient) null);
        return true;
      }
      catch (Exception ex)
      {
        CenterServer.log.Error((object) "Order is not Exist!", ex);
      }
      return false;
    }

    public void SendRoomClose(byte type)
    {
      GSPacketIn pkg = new GSPacketIn((short) 83);
      pkg.WriteByte(type);
      this.SendToALL(pkg);
    }

    public void SendShutdown()
    {
      this.SendToALL(new GSPacketIn((short) 15));
    }

    public void SendSystemNotice(string msg)
    {
      GSPacketIn pkg = new GSPacketIn((short) 10);
      pkg.WriteInt(0);
      pkg.WriteString(msg);
      this.SendToALL(pkg, (ServerClient) null);
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

    public void SendUpdateRank(bool type)
    {
      List<RankingPersonInfo> rankingPersonInfoList = WorldMgr.SelectTopTen();
      if ((uint) rankingPersonInfoList.Count <= 0U)
        return;
      GSPacketIn pkg = new GSPacketIn((short) 81);
      pkg.WriteBoolean(type);
      pkg.WriteInt(rankingPersonInfoList.Count);
      foreach (RankingPersonInfo rankingPersonInfo in rankingPersonInfoList)
      {
        pkg.WriteInt(rankingPersonInfo.ID);
        pkg.WriteString(rankingPersonInfo.Name);
        pkg.WriteInt(rankingPersonInfo.Damage);
      }
      this.SendToALL(pkg);
    }

    public void SendUpdateWorldBlood()
    {
      GSPacketIn pkg = new GSPacketIn((short) 79);
      pkg.WriteLong(WorldMgr.MAX_BLOOD);
      pkg.WriteLong(WorldMgr.current_blood);
      this.SendToALL(pkg);
    }

    public void SendUpdateWorldEvent()
    {
      int hour = DateTime.Now.Hour;
      string[] strArray = GameProperties.TimeForLeague.Split('|');
      DateTime now = DateTime.Now;
      if (now >= Convert.ToDateTime(strArray[0]) && now <= Convert.ToDateTime(strArray[1]))
      {
        if (WorldMgr.IsLeagueOpen)
          return;
        this.SendLeagueOpenClose(true);
        WorldMgr.IsLeagueOpen = true;
        WorldMgr.LeagueOpenTime = DateTime.Now;
        //#todo:Fazer Torneio
        //this.SendSystemNotice("O torneio 2x2 já foi aberto, chame seu parceiro e entre para as lutas");
      }
      else
      {
        if (!WorldMgr.IsLeagueOpen)
          return;
        this.SendLeagueOpenClose(false);
        WorldMgr.IsLeagueOpen = false;
        //#todo:Fazer Torneio
        //this.SendSystemNotice("O torneio foi fechado");
      }
    }

    public void SendWorldBossFightOver()
    {
      this.SendToALL(new GSPacketIn((short) 82));
    }

    public override bool Start()
    {
      try
      {
        Thread.CurrentThread.Priority = ThreadPriority.Normal;
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);
        GameProperties.Refresh();
        if (!this.InitComponent(this.RecompileScripts(), "Recompile Scripts") || !this.InitComponent(this.StartScriptComponents(), "Script components") || (!this.InitComponent(GameProperties.EDITION == this.Edition, "Check Server Edition:" + this.Edition) || !this.InitComponent(this.InitSocket(IPAddress.Parse(this._config.Ip), this._config.Port), "InitSocket Port:" + (object) this._config.Port)) || (!this.InitComponent(CenterService.Start(), "Center Service") || !this.InitComponent(ServerMgr.Start(), "Load serverlist") || (!this.InitComponent(MacroDropMgr.Init(), "Init MacroDropMgr") || !this.InitComponent(LanguageMgr.Setup(""), "LanguageMgr Init"))) || (!this.InitComponent(WorldMgr.Start(), "WorldMgr Init") || !this.InitComponent(this.InitGlobalTimers(), "Init Global Timers")))
          return false;
        GameEventMgr.Notify((RoadEvent) ScriptEvent.Loaded);
        MacroDropMgr.Start();
        if (!this.InitComponent(base.Start(), "base.Start()"))
          return false;
        GameEventMgr.Notify((RoadEvent) GameServerEvent.Started, (object) this);
        GC.Collect(GC.MaxGeneration);
        CenterServer.log.Info((object) "GameServer is now open for connections!");
        GameProperties.Save();
        return true;
      }
      catch (Exception ex)
      {
        CenterServer.log.Error((object) "Failed to start the server", ex);
        return false;
      }
    }

    protected bool StartScriptComponents()
    {
      try
      {
        ScriptMgr.InsertAssembly(typeof (CenterServer).Assembly);
        ScriptMgr.InsertAssembly(typeof (BaseServer).Assembly);
        foreach (Assembly script in ScriptMgr.Scripts)
        {
          GameEventMgr.RegisterGlobalEvents(script, typeof (GameServerStartedEventAttribute), (RoadEvent) GameServerEvent.Started);
          GameEventMgr.RegisterGlobalEvents(script, typeof (GameServerStoppedEventAttribute), (RoadEvent) GameServerEvent.Stopped);
          GameEventMgr.RegisterGlobalEvents(script, typeof (ScriptLoadedEventAttribute), (RoadEvent) ScriptEvent.Loaded);
          GameEventMgr.RegisterGlobalEvents(script, typeof (ScriptUnloadedEventAttribute), (RoadEvent) ScriptEvent.Unloaded);
        }
        CenterServer.log.Info((object) "Registering global event handlers: true");
        return true;
      }
      catch (Exception ex)
      {
        CenterServer.log.Error((object) nameof (StartScriptComponents), ex);
        return false;
      }
    }

    public override void Stop()
    {
      this.DisposeGlobalTimers();
      this.SaveTimerProc((object) null);
      this.SaveRecordProc((object) null);
      CenterService.Stop();
      base.Stop();
    }

    protected void SystemNoticeTimerProc(object state)
    {
      try
      {
        int tickCount = Environment.TickCount;
        if (CenterServer.log.IsInfoEnabled)
        {
          CenterServer.log.Info((object) "System Notice ...");
          CenterServer.log.Debug((object) ("Save ThreadId=" + (object) Thread.CurrentThread.ManagedThreadId));
        }
        ThreadPriority priority = Thread.CurrentThread.Priority;
        Thread.CurrentThread.Priority = ThreadPriority.Lowest;
        List<string> notceList = WorldMgr.NotceList;
        if (notceList.Count > 0)
        {
          int index = this.rand.Next(notceList.Count);
          CenterServer.Instance.SendSystemNotice(notceList[index]);
        }
        Thread.CurrentThread.Priority = priority;
        int num = Environment.TickCount - tickCount;
        if (!CenterServer.log.IsInfoEnabled)
          return;
        CenterServer.log.Info((object) "System Notice complete!");
      }
      catch (Exception ex)
      {
        if (!CenterServer.log.IsErrorEnabled)
          return;
        CenterServer.log.Error((object) nameof (SystemNoticeTimerProc), ex);
      }
    }

    public bool ASSState
    {
      get
      {
        return this._aSSState;
      }
      set
      {
        this._aSSState = value;
      }
    }

    public bool DailyAwardState
    {
      get
      {
        return this._dailyAwardState;
      }
      set
      {
        this._dailyAwardState = value;
      }
    }

    public static CenterServer Instance
    {
      get
      {
        return CenterServer._instance;
      }
    }
  }
}
