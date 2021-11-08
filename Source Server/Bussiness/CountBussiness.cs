// Decompiled with JetBrains decompiler
// Type: Bussiness.CountBussiness
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using DAL;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bussiness
{
  public class CountBussiness
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static int _appID;
    private static string _connectionString;
    private static bool _conutRecord;
    private static int _serverID;
    private static int _subID;

    public static void InsertContentCount(Dictionary<string, string> clientInfos)
    {
      try
      {
        if (!CountBussiness.CountRecord)
          return;
        SqlHelper.BeginExecuteNonQuery(CountBussiness.ConnectionString, "Modify_Count_Content", (object) clientInfos["Application_Id"], (object) clientInfos["Cpu"], (object) clientInfos["OperSystem"], (object) clientInfos["IP"], (object) clientInfos["IPAddress"], (object) clientInfos["NETCLR"], (object) clientInfos["Browser"], (object) clientInfos["ActiveX"], (object) clientInfos["Cookies"], (object) clientInfos["CSS"], (object) clientInfos["Language"], (object) clientInfos["Computer"], (object) clientInfos["Platform"], (object) clientInfos["Win16"], (object) clientInfos["Win32"], (object) clientInfos["Referry"], (object) clientInfos["Redirect"], (object) clientInfos["TimeSpan"], (object) (clientInfos["ScreenWidth"] + clientInfos["ScreenHeight"]), (object) clientInfos["Color"], (object) clientInfos["Flash"], (object) "Insert");
      }
      catch (Exception ex)
      {
        CountBussiness.log.Error((object) "Insert Log Error!!!!", ex);
      }
    }

    public static void InsertGameInfo(
      DateTime begin,
      int mapID,
      int money,
      int gold,
      string users)
    {
      CountBussiness.InsertGameInfo(CountBussiness.AppID, CountBussiness.SubID, CountBussiness.ServerID, begin, DateTime.Now, users.Split(',').Length, mapID, money, gold, users);
    }

    public static void InsertGameInfo(
      int appid,
      int subid,
      int serverid,
      DateTime begin,
      DateTime end,
      int usercount,
      int mapID,
      int money,
      int gold,
      string users)
    {
      try
      {
        if (!CountBussiness.CountRecord)
          return;
        SqlHelper.BeginExecuteNonQuery(CountBussiness.ConnectionString, "SP_Insert_Count_FightInfo", (object) appid, (object) subid, (object) serverid, (object) begin, (object) end, (object) usercount, (object) mapID, (object) money, (object) gold, (object) users);
      }
      catch (Exception ex)
      {
        CountBussiness.log.Error((object) "Insert Log Error!", ex);
      }
    }

    public static void InsertServerInfo(int usercount, int gamecount)
    {
      CountBussiness.InsertServerInfo(CountBussiness.AppID, CountBussiness.SubID, CountBussiness.ServerID, usercount, gamecount, DateTime.Now);
    }

    public static void InsertServerInfo(
      int appid,
      int subid,
      int serverid,
      int usercount,
      int gamecount,
      DateTime time)
    {
      try
      {
        if (!CountBussiness.CountRecord)
          return;
        SqlHelper.BeginExecuteNonQuery(CountBussiness.ConnectionString, "SP_Insert_Count_Server", (object) appid, (object) subid, (object) serverid, (object) usercount, (object) gamecount, (object) time);
      }
      catch (Exception ex)
      {
        CountBussiness.log.Error((object) "Insert Log Error!!", ex);
      }
    }

    public static void InsertSystemPayCount(
      int consumerid,
      int money,
      int gold,
      int consumertype,
      int subconsumertype)
    {
      CountBussiness.InsertSystemPayCount(CountBussiness.AppID, CountBussiness.SubID, consumerid, money, gold, consumertype, subconsumertype, DateTime.Now);
    }

    public static void InsertSystemPayCount(
      int appid,
      int subid,
      int consumerid,
      int money,
      int gold,
      int consumertype,
      int subconsumertype,
      DateTime datime)
    {
      try
      {
        if (!CountBussiness.CountRecord)
          return;
        SqlHelper.BeginExecuteNonQuery(CountBussiness.ConnectionString, "SP_Insert_Count_SystemPay", (object) appid, (object) subid, (object) consumerid, (object) money, (object) gold, (object) consumertype, (object) subconsumertype, (object) datime);
      }
      catch (Exception ex)
      {
        CountBussiness.log.Error((object) "InsertSystemPayCount Log Error!!!", ex);
      }
    }

    public static void SetConfig(
      string connectionString,
      int appID,
      int subID,
      int serverID,
      bool countRecord)
    {
      CountBussiness._connectionString = connectionString;
      CountBussiness._appID = appID;
      CountBussiness._subID = subID;
      CountBussiness._serverID = serverID;
      CountBussiness._conutRecord = countRecord;
    }

    public static int AppID
    {
      get
      {
        return CountBussiness._appID;
      }
    }

    public static string ConnectionString
    {
      get
      {
        return CountBussiness._connectionString;
      }
    }

    public static bool CountRecord
    {
      get
      {
        return CountBussiness._conutRecord;
      }
    }

    public static int ServerID
    {
      get
      {
        return CountBussiness._serverID;
      }
    }

    public static int SubID
    {
      get
      {
        return CountBussiness._subID;
      }
    }
  }
}
