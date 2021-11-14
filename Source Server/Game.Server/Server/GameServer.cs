using Bussiness;
using Bussiness.Managers;
using Game.Base;
using Game.Base.Events;
using Game.Base.Manager;
using Game.Logic;
using Game.Server.Battle;
using Game.Server.ConsortiaTask;
using Game.Server.GameObjects;
using Game.Server.Games;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.RingStation;
using Game.Server.Rooms;
using Game.Server.Statics;
using log4net;
using log4net.Config;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Game.Server
{
    public class GameServer : BaseServer
    {
        public static readonly string Edition = "2612558";
        public static bool KeepRunning = false;
        public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static bool m_compiled = false;
        private static GameServer m_instance = (GameServer)null;
        private static int m_tryCount = 4;
        private int _shutdownCount = 6;
        private LoginServerConnector _loginServer;
        private Timer _shutdownTimer;
        private const int BUF_SIZE = 8192;
        protected Timer m_bagMailScanTimer;
        protected Timer m_buffScanTimer;
        private GameServerConfig m_config;
        private bool m_debugMenory;
        private bool m_isRunning;
        private Queue m_packetBufPool;
        protected Timer m_pingCheckTimer;
        protected Timer m_qqTipScanTimer;
        protected Timer m_saveDbTimer;
        protected Timer m_saveRecordTimer;
        Update_Celeb Celeb = new Update_Celeb();

        protected GameServer(GameServerConfig config)
        {
            this.m_config = config;
            if (GameServer.log.IsDebugEnabled)
            {
                GameServer.log.Debug((object)("Current directory is: " + Directory.GetCurrentDirectory()));
                GameServer.log.Debug((object)("Gameserver root directory is: " + this.Configuration.RootDirectory));
                GameServer.log.Debug((object)"Changing directory to root directory");
            }
            Directory.SetCurrentDirectory(this.Configuration.RootDirectory);
        }

        public byte[] AcquirePacketBuffer()
        {
            lock (this.m_packetBufPool.SyncRoot)
            {
                if (this.m_packetBufPool.Count > 0)
                    return (byte[])this.m_packetBufPool.Dequeue();
            }
            GameServer.log.Warn((object)"packet buffer pool is empty!");
            return new byte[8192];
        }

        private bool AllocatePacketBuffers()
        {
            int capacity = this.Configuration.MaxClientCount * 3;
            this.m_packetBufPool = new Queue(capacity);
            for (int index = 0; index < capacity; ++index)
                this.m_packetBufPool.Enqueue((object)new byte[8192]);
            if (GameServer.log.IsDebugEnabled)
                GameServer.log.DebugFormat("allocated packet buffers: {0}", (object)capacity.ToString());
            return true;
        }

        protected void BuffScanTimerProc(object sender)
        {
            try
            {
                Celeb.UpdateCeleb();
                int tickCount = Environment.TickCount;
                if (GameServer.log.IsInfoEnabled)
                {
                    GameServer.log.Info((object)"Buff Scaning ...");
                    GameServer.log.Debug((object)("BuffScan ThreadId=" + (object)Thread.CurrentThread.ManagedThreadId));
                }
                int num1 = 0;
                ThreadPriority priority = Thread.CurrentThread.Priority;
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                {
                    if (allPlayer.BufferList != null)
                    {
                        allPlayer.BufferList.Update();
                        ++num1;
                    }
                }
                Thread.CurrentThread.Priority = priority;
                int num2 = Environment.TickCount - tickCount;
                if (GameServer.log.IsInfoEnabled)
                {
                    GameServer.log.Info((object)"Buff Scan complete!");
                    GameServer.log.Info((object)("Buff all " + (object)num1 + " players in " + (object)num2 + "ms"));
                }
                if (num2 <= 120000)
                    return;
                GameServer.log.WarnFormat("Scan all Buff and {0} players in {1} ms", (object)num1, (object)num2);
            }
            catch (Exception ex)
            {
                if (!GameServer.log.IsErrorEnabled)
                    return;
                GameServer.log.Error((object)nameof(BuffScanTimerProc), ex);
            }
        }

        public static void CreateInstance(GameServerConfig config)
        {
            if (GameServer.m_instance != null)
                return;
            FileInfo configFile = new FileInfo(config.LogConfigFile);
            if (!configFile.Exists)
                ResourceUtil.ExtractResource(configFile.Name, configFile.FullName, Assembly.GetAssembly(typeof(GameServer)));
            XmlConfigurator.ConfigureAndWatch(configFile);
            GameServer.m_instance = new GameServer(config);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                GameServer.log.Fatal((object)("Unhandled exception!\n" + e.ExceptionObject.ToString()));
                if (!e.IsTerminating)
                    return;
                this.Stop();
            }
            catch
            {
                try
                {
                    using (FileStream fileStream = new FileStream("c:\\testme.log", FileMode.Append, FileAccess.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream, Encoding.UTF8))
                            streamWriter.WriteLine(e.ExceptionObject);
                    }
                }
                catch
                {
                }
            }
        }

        public GameClient[] GetAllClients()
        {
            GameClient[] gameClientArray = (GameClient[])null;
            lock (this._clients.SyncRoot)
            {
                gameClientArray = new GameClient[this._clients.Count];
                this._clients.Keys.CopyTo((Array)gameClientArray, 0);
            }
            return gameClientArray;
        }

        protected override BaseClient GetNewClient()
        {
            return (BaseClient)new GameClient(this, this.AcquirePacketBuffer(), this.AcquirePacketBuffer());
        }

        protected bool InitComponent(bool componentInitState, string text)
        {
            if (this.m_debugMenory)
                GameServer.log.Debug((object)("Start Memory " + text + ": " + (object)(GC.GetTotalMemory(false) / 1024L / 1024L)));
            if (GameServer.log.IsInfoEnabled)
                GameServer.log.Info((object)(text + ": " + componentInitState.ToString()));
            if (!componentInitState)
                this.Stop();
            if (this.m_debugMenory)
                GameServer.log.Debug((object)("Finish Memory " + text + ": " + (object)(GC.GetTotalMemory(false) / 1024L / 1024L)));
            return componentInitState;
        }

        public bool InitGlobalTimer()
        {
            int num1 = this.Configuration.DBSaveInterval * 60 * 1000;
            if (this.m_saveDbTimer == null)
                this.m_saveDbTimer = new Timer(new TimerCallback(this.SaveTimerProc), (object)null, num1, num1);
            else
                this.m_saveDbTimer.Change(num1, num1);
            int num2 = this.Configuration.PingCheckInterval * 60 * 1000;
            if (this.m_pingCheckTimer == null)
                this.m_pingCheckTimer = new Timer(new TimerCallback(this.PingCheck), (object)null, num2, num2);
            else
                this.m_pingCheckTimer.Change(num2, num2);
            int num3 = this.Configuration.SaveRecordInterval * 60 * 1000;
            if (this.m_saveRecordTimer != null)
                this.m_saveRecordTimer.Change(num3, num3);
            int num4 = 60000;
            if (this.m_buffScanTimer == null)
                this.m_buffScanTimer = new Timer(new TimerCallback(this.BuffScanTimerProc), (object)null, num4, num4);
            else
                this.m_buffScanTimer.Change(num4, num4);
            return true;
        }

        private bool InitLoginServer()
        {
            this._loginServer = new LoginServerConnector(this.m_config.LoginServerIp, this.m_config.LoginServerPort, this.m_config.ServerID, this.m_config.ServerName, this.AcquirePacketBuffer(), this.AcquirePacketBuffer());
            this._loginServer.Disconnected += new ClientEventHandle(this.loginServer_Disconnected);
            return this._loginServer.Connect();
        }

        private void loginServer_Disconnected(BaseClient client)
        {
            int num = this.m_isRunning ? 1 : 0;
            this.Stop();
            if (num != 0 && GameServer.m_tryCount > 0)
            {
                --GameServer.m_tryCount;
                GameServer.log.Error((object)"Center Server Disconnect! Stopping Server");
                GameServer.log.ErrorFormat("Start the game server again after 1 second,and left try times:{0}", (object)GameServer.m_tryCount);
                Thread.Sleep(1000);
                if (!this.Start())
                    return;
                GameServer.log.Error((object)"Restart the game server success!");
            }
            else
            {
                if (GameServer.m_tryCount == 0)
                {
                    GameServer.log.ErrorFormat("Restart the game server failed after {0} times.", (object)4);
                    GameServer.log.Error((object)"Server Stopped!");
                }
                LogManager.Shutdown();
            }
        }

        protected void PingCheck(object sender)
        {
            try
            {
                GameServer.log.Info((object)"Begin ping check....");
                long num = (long)this.Configuration.PingCheckInterval * 60L * 1000L * 1000L * 10L;
                GameClient[] allClients = this.GetAllClients();
                if (allClients != null)
                {
                    foreach (GameClient gameClient in allClients)
                    {
                        if (gameClient != null)
                        {
                            if (gameClient.IsConnected)
                            {
                                if (gameClient.Player != null)
                                {
                                    gameClient.Out.SendPingTime(gameClient.Player);
                                    if (AntiAddictionMgr.ISASSon && AntiAddictionMgr.count == 0)
                                        ++AntiAddictionMgr.count;
                                }
                                else if (gameClient.PingTime + num < DateTime.Now.Ticks)
                                    gameClient.Disconnect();
                            }
                            else
                                gameClient.Disconnect();
                        }
                    }
                }
                GameServer.log.Info((object)"End ping check....");
            }
            catch (Exception ex)
            {
                if (GameServer.log.IsErrorEnabled)
                    GameServer.log.Error((object)"PingCheck callback", ex);
            }
            try
            {
                GameServer.log.Info((object)"Begin ping center check....");
                GameServer.Instance.LoginServer.SendPingCenter();
                GameServer.log.Info((object)"End ping center check....");
            }
            catch (Exception ex)
            {
                if (!GameServer.log.IsErrorEnabled)
                    return;
                GameServer.log.Error((object)"PingCheck center callback", ex);
            }
        }

        public bool RecompileScripts()
        {
            if (!GameServer.m_compiled)
            {
                string path = this.Configuration.RootDirectory + Path.DirectorySeparatorChar.ToString() + "scripts";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string[] asm_names = this.Configuration.ScriptAssemblies.Split(',');
                GameServer.m_compiled = ScriptMgr.CompileScripts(false, path, this.Configuration.ScriptCompilationTarget, asm_names);
            }
            return GameServer.m_compiled;
        }

        public void ReleasePacketBuffer(byte[] buf)
        {
            if (buf == null || GC.GetGeneration((object)buf) < GC.MaxGeneration)
                return;
            lock (this.m_packetBufPool.SyncRoot)
                this.m_packetBufPool.Enqueue((object)buf);
        }

        protected void SaveRecordProc(object sender)
        {
            try
            {
                int tickCount = Environment.TickCount;
                if (GameServer.log.IsInfoEnabled)
                {
                    GameServer.log.Info((object)"Saving Record...");
                    GameServer.log.Debug((object)("Save ThreadId=" + (object)Thread.CurrentThread.ManagedThreadId));
                }
                ThreadPriority priority = Thread.CurrentThread.Priority;
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                LogMgr.Save();
                Thread.CurrentThread.Priority = priority;
                int num = Environment.TickCount - tickCount;
                if (GameServer.log.IsInfoEnabled)
                    GameServer.log.Info((object)"Saving Record complete!");
                if (num <= 120000)
                    return;
                GameServer.log.WarnFormat("Saved all Record  in {0} ms", (object)num);
            }
            catch (Exception ex)
            {
                if (!GameServer.log.IsErrorEnabled)
                    return;
                GameServer.log.Error((object)nameof(SaveRecordProc), ex);
            }
        }

        protected void SaveTimerProc(object sender)
        {
            try
            {
                int tickCount = Environment.TickCount;
                if (GameServer.log.IsInfoEnabled)
                {
                    GameServer.log.Info((object)"Saving database...");
                    GameServer.log.Debug((object)("Save ThreadId=" + (object)Thread.CurrentThread.ManagedThreadId));
                }
                int num1 = 0;
                ThreadPriority priority = Thread.CurrentThread.Priority;
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                {
                    allPlayer.SaveNewsItemIntoDatabase();
                    ++num1;
                }
                WorldMgr.UpdateCaddyRank();
                WorldMgr.ScanShopFreeVaildDate();
                ConsortiaTaskMgr.ScanConsortiaTask();
                AcademyMgr.RemoveOldRequest();
                Thread.CurrentThread.Priority = priority;
                int num2 = Environment.TickCount - tickCount;
                if (GameServer.log.IsInfoEnabled)
                {
                    GameServer.log.Info((object)"Saving New Item to database complete!");
                    GameServer.log.Info((object)("Saved all databases and " + (object)num1 + " players in " + (object)num2 + "ms"));
                }
                if (num2 <= 120000)
                    return;
                GameServer.log.WarnFormat("Saved all databases and {0} players in {1} ms", (object)num1, (object)num2);
            }
            catch (Exception ex)
            {
                if (!GameServer.log.IsErrorEnabled)
                    return;
                GameServer.log.Error((object)nameof(SaveTimerProc), ex);
            }
            finally
            {
                GameEventMgr.Notify((RoadEvent)GameServerEvent.WorldSave);
            }
        }

        public void Shutdown()
        {
            GameServer.Instance.LoginServer.SendShutdown(true);
            this._shutdownTimer = new Timer(new TimerCallback(this.ShutDownCallBack), (object)null, 0, 60000);
        }

        private void ShutDownCallBack(object state)
        {
            try
            {
                --this._shutdownCount;
                Console.WriteLine(string.Format("Server will shutdown after {0} mins!", (object)this._shutdownCount));
                foreach (GameClient allClient in GameServer.Instance.GetAllClients())
                {
                    if (allClient.Out != null)
                        allClient.Out.SendMessage(eMessageType.GM_NOTICE, string.Format("{0}{1}{2}", (object)LanguageMgr.GetTranslation("Game.Service.actions.ShutDown1"), (object)this._shutdownCount, (object)LanguageMgr.GetTranslation("Game.Service.actions.ShutDown2")));
                }
                if ((uint)this._shutdownCount > 0U)
                    return;
                Console.WriteLine("Server has stopped!");
                GameServer.Instance.LoginServer.SendShutdown(false);
                this._shutdownTimer.Dispose();
                this._shutdownTimer = (Timer)null;
                GameServer.Instance.Stop();
            }
            catch (Exception ex)
            {
                GameServer.log.Error((object)ex);
            }
        }

        public override bool Start()
        {
            if (this.m_isRunning)
                return false;
            bool flag;
            try
            {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);
                Thread.CurrentThread.Priority = ThreadPriority.Normal;
                GameProperties.Refresh();
                if (!this.InitComponent(this.RecompileScripts(), "Recompile Scripts"))
                    ;
                if (!this.InitComponent(ConsortiaLevelMgr.Init(), "ConsortiaLevelMgr Init") || !this.InitComponent(this.StartScriptComponents(), "Script components") || (!this.InitComponent(GameProperties.EDITION == GameServer.Edition, "Edition: " + GameServer.Edition) || !this.InitComponent(this.InitSocket(IPAddress.Parse(this.Configuration.Ip), this.Configuration.Port), "InitSocket Port: " + (object)this.Configuration.Port)) || (!this.InitComponent(this.AllocatePacketBuffers(), "AllocatePacketBuffers()") || !this.InitComponent(LogMgr.Setup(this.Configuration.GAME_TYPE, this.Configuration.ServerID, this.Configuration.AreaID), "LogMgr Init") || (!this.InitComponent(WorldMgr.Init(), "WorldMgr Init") || !this.InitComponent(MapMgr.Init(), "MapMgr Init"))) || (!this.InitComponent(ItemMgr.Init(), "ItemMgr Init") || !this.InitComponent(ItemBoxMgr.Init(), "ItemBox Init") || (!this.InitComponent(BallMgr.Init(), "BallMgr Init") || !this.InitComponent(TangCapVipMgr.Init(), "Tiem Nang Trang Bi")) || (!this.InitComponent(ExerciseMgr.Init(), "ExerciseMgr Init") || !this.InitComponent(LevelMgr.Init(), "levelMgr Init") || (!this.InitComponent(BallConfigMgr.Init(), "BallConfigMgr Init") || !this.InitComponent(FusionMgr.Init(), "FusionMgr Init")))) || (!this.InitComponent(UserBoxMgr.Init(), "UserBoxMgr Init") || !this.InitComponent(AwardMgr.Init(), "AwardMgr Init") || (!this.InitComponent(AchievementMgr.Init(), "AchievementMgr Init") || (!this.InitComponent(PetMoePropertyMgr.Init(), "PetMoePropertyMgr Init")) || (!this.InitComponent(FightSpiritTemplateMgr.Init(), "FightSpiritTemplateMgr Init")) || (!this.InitComponent(TotemMgr.Init(), "TotemMgr Init")) || (!this.InitComponent(TotemHonorMgr.Init(), "TotemHonorMgr Init")) || !this.InitComponent(NPCInfoMgr.Init(), "NPCInfoMgr Init")) || (!this.InitComponent(MissionInfoMgr.Init(), "MissionInfoMgr Init") || !this.InitComponent(PveInfoMgr.Init(), "PveInfoMgr Init") || (!this.InitComponent(DropMgr.Init(), "Drop Init") || !this.InitComponent(FightRateMgr.Init(), "FightRateMgr Init"))) || (!this.InitComponent(RefineryMgr.Init(), "RefineryMgr Init") || !this.InitComponent(StrengthenMgr.Init(), "StrengthenMgr Init") || (!this.InitComponent(PropItemMgr.Init(), "PropItemMgr Init") || !this.InitComponent(ShopMgr.Init(), "ShopMgr Init")) || (!this.InitComponent(QuestMgr.Init(), "QuestMgr Init") || !this.InitComponent(RoomMgr.Setup(this.Configuration.MaxRoomCount), "RoomMgr.Setup") || (!this.InitComponent(GameMgr.Setup(this.Configuration.ServerID, GameProperties.BOX_APPEAR_CONDITION), "GameMgr.Start()") || !this.InitComponent(ConsortiaMgr.Init(), "ConsortiaMgr Init"))))) || !this.InitComponent(ConsortiaExtraMgr.Init(), "ConsortiaExtraMgr Init") || (!this.InitComponent(LanguageMgr.Setup(""), "LanguageMgr Init") || !this.InitComponent(RateMgr.Init(this.Configuration), "ExperienceRateMgr Init") || (!this.InitComponent(WindMgr.Init(), "WindMgr Init") || !this.InitComponent(CardMgr.Init(), "CardMgr Init")) || (!this.InitComponent(FairBattleRewardMgr.Init(), "FairBattleRewardMgr Init") || !this.InitComponent(ConsortiaTaskMgr.Init(), "ConsortiaTaskMgr Setup") || (!this.InitComponent(PetMgr.Init(), "PetMgr Setup") || !this.InitComponent(MacroDropMgr.Init(), "MacroDropMgr Init"))) || (!this.InitComponent(MarryRoomMgr.Init(), "MarryRoomMgr Init") || !this.InitComponent(RankMgr.Init(), "RankMgr Init") || (!this.InitComponent(CommunalActiveMgr.Init(), "CommunalActiveMgr Setup") || !this.InitComponent(QQTipsMgr.Init(), "QQTipsMgr Init")) || (!this.InitComponent(SubActiveMgr.Init(), "SubActiveMgr Setup") || !this.InitComponent(EventAwardMgr.Init(), "EventAwardMgr Setup") || (!this.InitComponent(EventLiveMgr.Init(), "EventLiveMgr Setup") || !this.InitComponent(CommandsMgr.Init(), "CommandsMgr Setup")))) || (!this.InitComponent(AcademyMgr.Init(), "AcademyMgr Setup") || !this.InitComponent(BattleMgr.Setup(), "BattleMgr Setup") || (!this.InitComponent(this.InitGlobalTimer(), "Init Global Timers") || !this.InitComponent(LogMgr.Setup(1, this.Configuration.ServerID, this.Configuration.AreaID), "LogMgr Setup")))))
                    return false;
                if (!this.InitComponent(RobotManager.Start(), "RobotManager Start"))
                {
                    return false;
                }
                GameEventMgr.Notify((RoadEvent)ScriptEvent.Loaded);
                if (!this.InitComponent(this.InitLoginServer(), "Login To CenterServer") || !this.InitComponent(HotSpringMgr.Init(), "HotSpringMgr Init"))
                    return false;
                this.InitComponent(RingStationMgr.Init(), "AutoBot Init");
                RoomMgr.Start();
                GameMgr.Start();
                BattleMgr.Start();
                MacroDropMgr.Start();
                if (!this.InitComponent(base.Start(), "base.Start()"))
                {
                    flag = false;
                }
                else
                {
                    Whitelisting.Init();
                    GameEventMgr.Notify((RoadEvent)GameServerEvent.Started, (object)this);
                    GC.Collect(GC.MaxGeneration);
                    if (GameServer.log.IsInfoEnabled)
                        GameServer.log.Info((object)"GameServer is now open for connections!");
                    this.m_isRunning = true;
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                if (GameServer.log.IsErrorEnabled)
                    GameServer.log.Error((object)"Failed to start the server", ex);
                flag = false;
            }
            return flag;
        }

        protected bool StartScriptComponents()
        {
            try
            {
                if (GameServer.log.IsInfoEnabled)
                    GameServer.log.Info((object)"Server rules: true");
                ScriptMgr.InsertAssembly(typeof(GameServer).Assembly);
                ScriptMgr.InsertAssembly(typeof(BaseGame).Assembly);
                ScriptMgr.InsertAssembly(typeof(BaseServer).Assembly);
                foreach (Assembly asm in new ArrayList((ICollection)ScriptMgr.Scripts))
                {
                    GameEventMgr.RegisterGlobalEvents(asm, typeof(GameServerStartedEventAttribute), (RoadEvent)GameServerEvent.Started);
                    GameEventMgr.RegisterGlobalEvents(asm, typeof(GameServerStoppedEventAttribute), (RoadEvent)GameServerEvent.Stopped);
                    GameEventMgr.RegisterGlobalEvents(asm, typeof(ScriptLoadedEventAttribute), (RoadEvent)ScriptEvent.Loaded);
                    GameEventMgr.RegisterGlobalEvents(asm, typeof(ScriptUnloadedEventAttribute), (RoadEvent)ScriptEvent.Unloaded);
                }
                if (GameServer.log.IsInfoEnabled)
                    GameServer.log.Info((object)"Registering global event handlers: true");
            }
            catch (Exception ex)
            {
                if (GameServer.log.IsErrorEnabled)
                    GameServer.log.Error((object)nameof(StartScriptComponents), ex);
                return false;
            }
            return true;
        }

        public override void Stop()
        {
            if (!this.m_isRunning)
                return;
            this.m_isRunning = false;
            if (!MarryRoomMgr.UpdateBreakTimeWhereServerStop())
                Console.WriteLine("Update BreakTime failed");
            RoomMgr.Stop();
            GameMgr.Stop();
            WorldMgr.Stop();
            ConsortiaTaskMgr.Stop();
            if (this._loginServer != null)
            {
                this._loginServer.Disconnected -= new ClientEventHandle(this.loginServer_Disconnected);
                this._loginServer.Disconnect();
            }
            if (this.m_pingCheckTimer != null)
            {
                this.m_pingCheckTimer.Change(-1, -1);
                this.m_pingCheckTimer.Dispose();
                this.m_pingCheckTimer = (Timer)null;
            }
            if (this.m_saveDbTimer != null)
            {
                this.m_saveDbTimer.Change(-1, -1);
                this.m_saveDbTimer.Dispose();
                this.m_saveDbTimer = (Timer)null;
            }
            if (this.m_saveRecordTimer != null)
            {
                this.m_saveRecordTimer.Change(-1, -1);
                this.m_saveRecordTimer.Dispose();
                this.m_saveRecordTimer = (Timer)null;
            }
            if (this.m_buffScanTimer != null)
            {
                this.m_buffScanTimer.Change(-1, -1);
                this.m_buffScanTimer.Dispose();
                this.m_buffScanTimer = (Timer)null;
            }
            if (this.m_qqTipScanTimer != null)
            {
                this.m_qqTipScanTimer.Change(-1, -1);
                this.m_qqTipScanTimer.Dispose();
                this.m_qqTipScanTimer = (Timer)null;
            }
            if (this.m_bagMailScanTimer != null)
            {
                this.m_bagMailScanTimer.Change(-1, -1);
                this.m_bagMailScanTimer.Dispose();
                this.m_bagMailScanTimer = (Timer)null;
            }
            base.Stop();
            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
            GameServer.log.Info((object)"Server Stopped!");
            Console.WriteLine("Server Stopped!");
        }

        public GameServerConfig Configuration
        {
            get
            {
                return this.m_config;
            }
        }

        public static GameServer Instance
        {
            get
            {
                return GameServer.m_instance;
            }
        }

        public LoginServerConnector LoginServer
        {
            get
            {
                return this._loginServer;
            }
        }

        public int PacketPoolSize
        {
            get
            {
                return this.m_packetBufPool.Count;
            }
        }
    }
}
