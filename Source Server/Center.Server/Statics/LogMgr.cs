// Decompiled with JetBrains decompiler
// Type: Center.Server.Statics.LogMgr
// Assembly: Center.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4391F35A-9CAD-44A9-AFE0-208E0547A0DD
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Center\Center.Server.dll

using Bussiness;
using log4net;
using System;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace Center.Server.Statics
{
  public class LogMgr
  {
    private static object _syncStop = new object();
    public static object _sysObj = new object();
    public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static int _areaId;
    private static int _gameType;
    private static int _serverId;
    public static DataTable m_LogServer;
    private static int regCount;

    public static void AddRegCount()
    {
      lock (LogMgr._sysObj)
        ++LogMgr.regCount;
    }

    public static void Reset()
    {
      lock (LogMgr.m_LogServer)
        LogMgr.m_LogServer.Clear();
    }

    public static void Save()
    {
      LoginMgr.GetOnlineCount();
      int gameType = LogMgr._gameType;
      int serverId = LogMgr._serverId;
      DateTime now = DateTime.Now;
      int regCount = LogMgr.RegCount;
      LogMgr.RegCount = 0;
      int saveRecordSecond = LogMgr.SaveRecordSecond;
      using (ItemRecordBussiness itemRecordBussiness = new ItemRecordBussiness())
        itemRecordBussiness.LogServerDb(LogMgr.m_LogServer);
    }

    public static bool Setup()
    {
      return LogMgr.Setup(LogMgr.GameType, LogMgr.ServerID, LogMgr.AreaID);
    }

    public static bool Setup(int gametype, int serverid, int areaid)
    {
      LogMgr._gameType = gametype;
      LogMgr._serverId = serverid;
      LogMgr._areaId = areaid;
      LogMgr.m_LogServer = new DataTable("Log_Server");
      LogMgr.m_LogServer.Columns.Add("ApplicationId", typeof (int));
      LogMgr.m_LogServer.Columns.Add("SubId", typeof (int));
      LogMgr.m_LogServer.Columns.Add("EnterTime", typeof (DateTime));
      LogMgr.m_LogServer.Columns.Add("Online", typeof (int));
      LogMgr.m_LogServer.Columns.Add("Reg", typeof (int));
      return true;
    }

    public static int AreaID
    {
      get
      {
        return int.Parse(ConfigurationManager.AppSettings[nameof (AreaID)]);
      }
    }

    public static int GameType
    {
      get
      {
        return int.Parse(ConfigurationManager.AppSettings[nameof (GameType)]);
      }
    }

    public static int RegCount
    {
      get
      {
        lock (LogMgr._sysObj)
          return LogMgr.regCount;
      }
      set
      {
        lock (LogMgr._sysObj)
          LogMgr.regCount = value;
      }
    }

    public static int SaveRecordSecond
    {
      get
      {
        return int.Parse(ConfigurationManager.AppSettings["SaveRecordInterval"]) * 60;
      }
    }

    public static int ServerID
    {
      get
      {
        return int.Parse(ConfigurationManager.AppSettings[nameof (ServerID)]);
      }
    }
  }
}
