// Decompiled with JetBrains decompiler
// Type: Game.Server.Statics.LogMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Logic;
using log4net;
using System;
using System.Data;
using System.Reflection;

namespace Game.Server.Statics
{
  public class LogMgr
  {
    public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static int _areaId;
    private static int _gameType;
    private static int _serverId;
    private static object _syncStop;
    public static DataTable m_LogDropItem;
    public static DataTable m_LogFight;
    public static DataTable m_LogItem;
    public static DataTable m_LogMoney;

    public static void LogDropItemAdd(
      int userId,
      int itemId,
      int templateId,
      int dropId,
      int dropData)
    {
      try
      {
        object[] objArray = new object[9]
        {
          (object) LogMgr._gameType,
          (object) LogMgr._areaId,
          (object) LogMgr._serverId,
          (object) userId,
          (object) itemId,
          (object) templateId,
          (object) dropId,
          (object) dropData,
          (object) DateTime.Now
        };
        lock (LogMgr.m_LogDropItem)
          LogMgr.m_LogDropItem.Rows.Add(objArray);
      }
      catch (Exception ex)
      {
        if (!LogMgr.log.IsErrorEnabled)
          return;
        LogMgr.log.Error((object) ("LogMgr Error：LogDropItem @ " + (object) ex));
      }
    }

    public static void LogFightAdd(
      int roomId,
      eRoomType roomType,
      eGameType fightType,
      int changeTeam,
      DateTime playBegin,
      DateTime playEnd,
      int userCount,
      int mapId,
      string teamA,
      string teamB,
      string playResult,
      int winTeam,
      string BossWar)
    {
      try
      {
        object[] objArray = new object[16]
        {
          (object) LogMgr._gameType,
          (object) LogMgr._serverId,
          (object) LogMgr._areaId,
          (object) roomId,
          (object) (int) roomType,
          (object) (int) fightType,
          (object) changeTeam,
          (object) playBegin,
          (object) playEnd,
          (object) userCount,
          (object) mapId,
          (object) teamA,
          (object) teamB,
          (object) playResult,
          (object) winTeam,
          (object) BossWar
        };
        lock (LogMgr.m_LogFight)
          LogMgr.m_LogFight.Rows.Add(objArray);
      }
      catch (Exception ex)
      {
        if (!LogMgr.log.IsErrorEnabled)
          return;
        LogMgr.log.Error((object) ("LogMgr Error：Fight @ " + (object) ex));
      }
    }

    public static void LogItemAdd(
      int userId,
      LogItemType itemType,
      string beginProperty,
      SqlDataProvider.Data.ItemInfo item,
      string AddItem,
      int result)
    {
      try
      {
        string str = "";
        if (item != null)
          str = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", (object) item.StrengthenLevel, (object) item.Attack, (object) item.Defence, (object) item.Agility, (object) item.Luck, (object) item.AttackCompose, (object) item.DefendCompose, (object) item.AgilityCompose, (object) item.LuckCompose);
        object[] objArray = new object[12]
        {
          (object) LogMgr._gameType,
          (object) LogMgr._serverId,
          (object) LogMgr._areaId,
          (object) DateTime.Now,
          (object) userId,
          (object) (int) itemType,
          item == null ? (object) "" : (object) item.Template.Name,
          (object) (item == null ? 0 : item.ItemID),
          (object) AddItem,
          (object) beginProperty,
          (object) str,
          (object) result
        };
        lock (LogMgr.m_LogItem)
          LogMgr.m_LogItem.Rows.Add(objArray);
      }
      catch (Exception ex)
      {
        if (!LogMgr.log.IsErrorEnabled)
          return;
        LogMgr.log.Error((object) ("LogMgr Error：ItemAdd @ " + (object) ex));
      }
    }

    public static void LogMoneyAdd(
      LogMoneyType masterType,
      LogMoneyType sonType,
      int userId,
      int moneys,
      int SpareMoney,
      int gold,
      int giftToken,
      int offer,
      string otherPay,
      string goodId,
      string goodsType)
    {
      try
      {
        if (moneys == 0 || moneys > SpareMoney)
          return;
        if (sonType <= LogMoneyType.Shop_Present)
        {
          if (sonType != LogMoneyType.Auction_Update && (uint) (sonType - 202) > 1U && (uint) (sonType - 301) > 3U)
            goto label_9;
        }
        else
        {
          switch (sonType - 401)
          {
            case (LogMoneyType) 0:
            case LogMoneyType.Mail:
            case LogMoneyType.Marry:
            case LogMoneyType.Consortia:
            case LogMoneyType.Item:
            case LogMoneyType.Charge:
            case LogMoneyType.Box:
            case LogMoneyType.Shop | LogMoneyType.Award:
              break;
            case LogMoneyType.Auction:
            case LogMoneyType.Shop:
            case LogMoneyType.Award:
            case LogMoneyType.Game:
              goto label_9;
            default:
              if ((uint) (sonType - 601) > 1U && (uint) (sonType - 1001) > 2U)
                goto label_9;
              else
                goto case (LogMoneyType) 0;
          }
        }
        moneys *= -1;
label_9:
        object[] objArray = new object[15]
        {
          (object) LogMgr._gameType,
          (object) LogMgr._serverId,
          (object) LogMgr._areaId,
          (object) masterType,
          (object) sonType,
          (object) userId,
          (object) DateTime.Now,
          (object) moneys,
          (object) SpareMoney,
          (object) gold,
          (object) giftToken,
          (object) offer,
          (object) otherPay,
          (object) goodId,
          (object) goodsType
        };
        lock (LogMgr.m_LogMoney)
          ;
      }
      catch (Exception ex)
      {
        if (!LogMgr.log.IsErrorEnabled)
          return;
        LogMgr.log.Error((object) ("LogMgr Error：LogMoney @ " + (object) ex));
      }
    }

    public static void LogMoneyAdd(
      LogMoneyType masterType,
      LogMoneyType sonType,
      int userId,
      int moneys,
      int SpareMoney,
      int gold,
      int giftToken,
      int offer,
      int medal,
      string otherPay,
      string goodId,
      string goodsType)
    {
      try
      {
        if (moneys == 0 || moneys > SpareMoney)
          return;
        if (sonType <= LogMoneyType.Shop_Present)
        {
          if (sonType != LogMoneyType.Auction_Update && (uint) (sonType - 202) > 1U && (uint) (sonType - 301) > 3U)
            goto label_9;
        }
        else
        {
          switch (sonType - 401)
          {
            case (LogMoneyType) 0:
            case LogMoneyType.Mail:
            case LogMoneyType.Marry:
            case LogMoneyType.Consortia:
            case LogMoneyType.Item:
            case LogMoneyType.Charge:
            case LogMoneyType.Box:
            case LogMoneyType.Shop | LogMoneyType.Award:
              break;
            case LogMoneyType.Auction:
            case LogMoneyType.Shop:
            case LogMoneyType.Award:
            case LogMoneyType.Game:
              goto label_9;
            default:
              if ((uint) (sonType - 601) > 1U && (uint) (sonType - 1001) > 2U)
                goto label_9;
              else
                goto case (LogMoneyType) 0;
          }
        }
        moneys *= -1;
label_9:
        object[] objArray = new object[16]
        {
          (object) LogMgr._gameType,
          (object) LogMgr._serverId,
          (object) LogMgr._areaId,
          (object) masterType,
          (object) sonType,
          (object) userId,
          (object) DateTime.Now,
          (object) moneys,
          (object) SpareMoney,
          (object) gold,
          (object) giftToken,
          (object) offer,
          (object) medal,
          (object) otherPay,
          (object) goodId,
          (object) goodsType
        };
        lock (LogMgr.m_LogMoney)
          LogMgr.m_LogMoney.Rows.Add(objArray);
      }
      catch (Exception ex)
      {
        if (!LogMgr.log.IsErrorEnabled)
          return;
        LogMgr.log.Error((object) ("LogMgr Error：LogMoney @ " + (object) ex));
      }
    }

    public static void Reset()
    {
      lock (LogMgr.m_LogItem)
        LogMgr.m_LogItem.Clear();
      lock (LogMgr.m_LogMoney)
        LogMgr.m_LogMoney.Clear();
      lock (LogMgr.m_LogFight)
        LogMgr.m_LogFight.Clear();
    }

    public static void Save()
    {
      if (LogMgr._syncStop == null)
        return;
      lock (LogMgr._syncStop)
      {
        using (ItemRecordBussiness db = new ItemRecordBussiness())
        {
          LogMgr.SaveLogItem(db);
          LogMgr.SaveLogMoney(db);
          LogMgr.SaveLogFight(db);
          LogMgr.SaveLogDropItem(db);
        }
      }
    }

    public static void SaveLogDropItem(ItemRecordBussiness db)
    {
      lock (LogMgr.m_LogDropItem)
        db.LogDropItemDb(LogMgr.m_LogDropItem);
    }

    public static void SaveLogFight(ItemRecordBussiness db)
    {
      lock (LogMgr.m_LogFight)
        db.LogFightDb(LogMgr.m_LogFight);
    }

    public static void SaveLogItem(ItemRecordBussiness db)
    {
      lock (LogMgr.m_LogItem)
        db.LogItemDb(LogMgr.m_LogItem);
    }

    public static void SaveLogMoney(ItemRecordBussiness db)
    {
      lock (LogMgr.m_LogMoney)
        db.LogMoneyDb(LogMgr.m_LogMoney);
    }

    public static bool Setup(int gametype, int serverid, int areaid)
    {
      LogMgr._gameType = gametype;
      LogMgr._serverId = serverid;
      LogMgr._areaId = areaid;
      LogMgr._syncStop = new object();
      LogMgr.m_LogItem = new DataTable("Log_Item");
      LogMgr.m_LogItem.Columns.Add("ApplicationId", Type.GetType("System.Int32"));
      LogMgr.m_LogItem.Columns.Add("SubId", typeof (int));
      LogMgr.m_LogItem.Columns.Add("LineId", typeof (int));
      LogMgr.m_LogItem.Columns.Add("EnterTime", Type.GetType("System.DateTime"));
      LogMgr.m_LogItem.Columns.Add("UserId", typeof (int));
      LogMgr.m_LogItem.Columns.Add("Operation", typeof (int));
      LogMgr.m_LogItem.Columns.Add("ItemName", typeof (string));
      LogMgr.m_LogItem.Columns.Add("ItemID", typeof (int));
      LogMgr.m_LogItem.Columns.Add("AddItem", typeof (string));
      LogMgr.m_LogItem.Columns.Add("BeginProperty", typeof (string));
      LogMgr.m_LogItem.Columns.Add("EndProperty", typeof (string));
      LogMgr.m_LogItem.Columns.Add("Result", typeof (int));
      LogMgr.m_LogMoney = new DataTable("Log_Money");
      LogMgr.m_LogMoney.Columns.Add("ApplicationId", typeof (int));
      LogMgr.m_LogMoney.Columns.Add("SubId", typeof (int));
      LogMgr.m_LogMoney.Columns.Add("LineId", typeof (int));
      LogMgr.m_LogMoney.Columns.Add("MastType", typeof (int));
      LogMgr.m_LogMoney.Columns.Add("SonType", typeof (int));
      LogMgr.m_LogMoney.Columns.Add("UserId", typeof (int));
      LogMgr.m_LogMoney.Columns.Add("EnterTime", Type.GetType("System.DateTime"));
      LogMgr.m_LogMoney.Columns.Add("Moneys", typeof (int));
      LogMgr.m_LogMoney.Columns.Add("SpareMoney", typeof (int));
      LogMgr.m_LogMoney.Columns.Add("Gold", typeof (int));
      LogMgr.m_LogMoney.Columns.Add("GiftToken", typeof (int));
      LogMgr.m_LogMoney.Columns.Add("Medal", typeof (int));
      LogMgr.m_LogMoney.Columns.Add("Offer", typeof (int));
      LogMgr.m_LogMoney.Columns.Add("OtherPay", typeof (string));
      LogMgr.m_LogMoney.Columns.Add("GoodId", typeof (string));
      LogMgr.m_LogMoney.Columns.Add("GoodsType", typeof (string));
      LogMgr.m_LogFight = new DataTable("Log_Fight");
      LogMgr.m_LogFight.Columns.Add("ApplicationId", typeof (int));
      LogMgr.m_LogFight.Columns.Add("SubId", typeof (int));
      LogMgr.m_LogFight.Columns.Add("LineId", typeof (int));
      LogMgr.m_LogFight.Columns.Add("RoomId", typeof (int));
      LogMgr.m_LogFight.Columns.Add("RoomType", typeof (int));
      LogMgr.m_LogFight.Columns.Add("FightType", typeof (int));
      LogMgr.m_LogFight.Columns.Add("ChangeTeam", typeof (int));
      LogMgr.m_LogFight.Columns.Add("PlayBegin", Type.GetType("System.DateTime"));
      LogMgr.m_LogFight.Columns.Add("PlayEnd", Type.GetType("System.DateTime"));
      LogMgr.m_LogFight.Columns.Add("UserCount", typeof (int));
      LogMgr.m_LogFight.Columns.Add("MapId", typeof (int));
      LogMgr.m_LogFight.Columns.Add("TeamA", typeof (string));
      LogMgr.m_LogFight.Columns.Add("TeamB", typeof (string));
      LogMgr.m_LogFight.Columns.Add("PlayResult", typeof (string));
      LogMgr.m_LogFight.Columns.Add("WinTeam", typeof (int));
      LogMgr.m_LogFight.Columns.Add("Detail", typeof (string));
      LogMgr.m_LogDropItem = new DataTable("Log_DropItem");
      LogMgr.m_LogDropItem.Columns.Add("ApplicationId", typeof (int));
      LogMgr.m_LogDropItem.Columns.Add("SubId", typeof (int));
      LogMgr.m_LogDropItem.Columns.Add("LineId", typeof (int));
      LogMgr.m_LogDropItem.Columns.Add("UserId", typeof (int));
      LogMgr.m_LogDropItem.Columns.Add("ItemId", typeof (int));
      LogMgr.m_LogDropItem.Columns.Add("TemplateID", typeof (int));
      LogMgr.m_LogDropItem.Columns.Add("DropId", typeof (int));
      LogMgr.m_LogDropItem.Columns.Add("DropData", typeof (int));
      LogMgr.m_LogDropItem.Columns.Add("EnterTime", Type.GetType("System.DateTime"));
      return true;
    }
  }
}
