using Game.Server.Rooms;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace Game.Server.Battle
{
  public class BattleMgr
  {
    public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    public static List<BattleServer> m_list = new List<BattleServer>();
    public static bool AutoReconnect = true;

    public static bool Setup()
    {
      if (File.Exists("battle.xml"))
      {
        try
        {
          foreach (XElement node in XDocument.Load("battle.xml").Root.Nodes())
          {
            try
            {
              int serverId = int.Parse(node.Attribute((XName) "id").Value);
              string str1 = node.Attribute((XName) "ip").Value;
              int num = int.Parse(node.Attribute((XName) "port").Value);
              string str2 = node.Attribute((XName) "key").Value;
              string ip = str1;
              int port = num;
              string loginKey = str2;
              BattleMgr.AddBattleServer(new BattleServer(serverId, ip, port, loginKey));
              BattleMgr.log.InfoFormat("Battle server {0}:{1} loaded...", (object) str1, (object) num);
            }
            catch (Exception ex)
            {
              BattleMgr.log.Error((object) "BattleMgr setup error:", ex);
            }
          }
        }
        catch (Exception ex)
        {
          BattleMgr.log.Error((object) "BattleMgr setup error:", ex);
        }
      }
      BattleMgr.log.InfoFormat("Total {0} battle server loaded.", (object) BattleMgr.m_list.Count);
      return true;
    }

    public static BattleServer GetServer(int id)
    {
      foreach (BattleServer battleServer in BattleMgr.m_list)
      {
        if (battleServer.ServerId == id)
          return battleServer;
      }
      return (BattleServer) null;
    }

    public static void AddBattleServer(BattleServer battle)
    {
      if (battle == null)
        return;
      BattleMgr.m_list.Add(battle);
      battle.Disconnected += new EventHandler(BattleMgr.smethod_0);
    }

    private static void smethod_0(object object_0, object object_1)
    {
      BattleServer server = object_0 as BattleServer;
      BattleMgr.log.WarnFormat("Disconnect from battle server {0}:{1}", (object) server.Ip, (object) server.Port);
      if (server == null || !BattleMgr.AutoReconnect || !BattleMgr.m_list.Contains(server))
        return;
      BattleMgr.RemoveServer(server);
      if ((DateTime.Now - server.LastRetryTime).TotalMinutes > 3.0)
        server.RetryCount = 0;
      if (server.RetryCount >= 3)
        return;
      BattleServer battle = server.Clone();
      BattleMgr.AddBattleServer(battle);
      ++battle.RetryCount;
      battle.LastRetryTime = DateTime.Now;
      try
      {
        battle.Start();
      }
      catch (Exception ex)
      {
        BattleMgr.log.ErrorFormat("Batter server {0}:{1} can't connected!", (object) server.Ip, (object) server.Port);
        BattleMgr.log.Error((object) ex.Message);
        server.RetryCount = 0;
      }
    }

    public static void ConnectTo(int id, string ip, int port, string key)
    {
      BattleServer battle = new BattleServer(id, ip, port, key);
      BattleMgr.AddBattleServer(battle);
      try
      {
        battle.Start();
      }
      catch (Exception ex)
      {
        BattleMgr.log.ErrorFormat("Batter server {0}:{1} can't connected!", (object) battle.Ip, (object) battle.Port);
        BattleMgr.log.Error((object) ex.Message);
      }
    }

    public static void Disconnet(int id)
    {
      BattleServer server = BattleMgr.GetServer(id);
      if (server == null || !server.IsActive)
        return;
      server.LastRetryTime = DateTime.Now;
      server.RetryCount = 4;
      server.Server.Disconnect();
    }

    public static void RemoveServer(BattleServer server)
    {
      if (server == null)
        return;
      BattleMgr.m_list.Remove(server);
      server.Disconnected += new EventHandler(BattleMgr.smethod_0);
    }

    public static void Start()
    {
      foreach (BattleServer battleServer in BattleMgr.m_list)
      {
        try
        {
          battleServer.Start();
        }
        catch (Exception ex)
        {
          BattleMgr.log.ErrorFormat("Batter server {0}:{1} can't connected!", (object) battleServer.Ip, (object) battleServer.Port);
          BattleMgr.log.Error((object) ex.Message);
        }
      }
    }

    public static BattleServer FindActiveServer()
    {
      lock (BattleMgr.m_list)
      {
        foreach (BattleServer battleServer in BattleMgr.m_list)
        {
          if (battleServer.IsActive)
            return battleServer;
        }
      }
      return (BattleServer) null;
    }

    public static BattleServer FindActiveServer(bool isCrosszone)
    {
      lock (BattleMgr.m_list)
      {
        foreach (BattleServer battleServer in BattleMgr.m_list)
        {
          BattleMgr.log.WarnFormat("ServerId {0}", (object) battleServer.ServerId);
          if (isCrosszone && battleServer.ServerId == 2 && battleServer.IsActive || !isCrosszone && battleServer.IsActive)
            return battleServer;
        }
      }
      return (BattleServer) null;
    }

    public static BattleServer AddRoom(BaseRoom room)
    {
      BattleServer activeServer = BattleMgr.FindActiveServer(room.isCrosszone);
      if (activeServer != null && activeServer.AddRoom(room))
        return activeServer;
      return (BattleServer) null;
    }

    public static List<BattleServer> GetAllBattles()
    {
      return BattleMgr.m_list;
    }
  }
}
