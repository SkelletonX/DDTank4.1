// Decompiled with JetBrains decompiler
// Type: Center.Server.ServerMgr
// Assembly: Center.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4391F35A-9CAD-44A9-AFE0-208E0547A0DD
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Center\Center.Server.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Center.Server
{
  public class ServerMgr
  {
    private static Dictionary<int, ServerInfo> _list = new Dictionary<int, ServerInfo>();
    private static object _syncStop = new object();
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static ServerInfo GetServerInfo(int id)
    {
      if (ServerMgr._list.ContainsKey(id))
        return ServerMgr._list[id];
      return (ServerInfo) null;
    }

    public static int GetState(int count, int total)
    {
      if (count >= total)
        return 5;
      return (double) count > (double) total * 0.5 ? 4 : 2;
    }

    public static bool ReLoadServerList()
    {
      try
      {
        using (ServiceBussiness serviceBussiness = new ServiceBussiness())
        {
          lock (ServerMgr._syncStop)
          {
            foreach (ServerInfo server in serviceBussiness.GetServerList())
            {
              if (ServerMgr._list.ContainsKey(server.ID))
              {
                ServerMgr._list[server.ID].IP = server.IP;
                ServerMgr._list[server.ID].Name = server.Name;
                ServerMgr._list[server.ID].Port = server.Port;
                ServerMgr._list[server.ID].Room = server.Room;
                ServerMgr._list[server.ID].Total = server.Total;
                ServerMgr._list[server.ID].MustLevel = server.MustLevel;
                ServerMgr._list[server.ID].LowestLevel = server.LowestLevel;
                ServerMgr._list[server.ID].Online = server.Online;
                ServerMgr._list[server.ID].State = server.State;
              }
              else
              {
                server.State = 1;
                server.Online = 0;
                ServerMgr._list.Add(server.ID, server);
              }
            }
          }
        }
        ServerMgr.log.Info((object) "ReLoad server list from db.");
        return true;
      }
      catch (Exception ex)
      {
        ServerMgr.log.ErrorFormat("ReLoad server list from db failed:{0}", (object) ex);
        return false;
      }
    }

    public static void SaveToDatabase()
    {
      try
      {
        using (ServiceBussiness serviceBussiness = new ServiceBussiness())
        {
          foreach (ServerInfo info in ServerMgr._list.Values)
            serviceBussiness.UpdateService(info);
        }
      }
      catch (Exception ex)
      {
        ServerMgr.log.Error((object) "Save server state", ex);
      }
    }

    public static bool Start()
    {
      try
      {
        using (ServiceBussiness serviceBussiness = new ServiceBussiness())
        {
          foreach (ServerInfo server in serviceBussiness.GetServerList())
          {
            server.State = 1;
            server.Online = 0;
            ServerMgr._list.Add(server.ID, server);
          }
        }
        ServerMgr.log.Info((object) "Load server list from db.");
        return true;
      }
      catch (Exception ex)
      {
        ServerMgr.log.ErrorFormat("Load server list from db failed:{0}", (object) ex);
        return false;
      }
    }

    public static ServerInfo[] Servers
    {
      get
      {
        return ServerMgr._list.Values.ToArray<ServerInfo>();
      }
    }
  }
}
