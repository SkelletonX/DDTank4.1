// Decompiled with JetBrains decompiler
// Type: Center.Server.WorldMgr
// Assembly: Center.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4391F35A-9CAD-44A9-AFE0-208E0547A0DD
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Center\Center.Server.dll

using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Center.Server
{
  public class WorldMgr
  {
    private static object _syncStop = new object();
    public static string[] bossResourceId = new string[4]
    {
      "1",
      "2",
      "2",
      "4"
    };
    public static long current_blood = 0;
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    public static readonly long MAX_BLOOD = 2100000000;
    public static string[] name = new string[4]
    {
      "Chefão",
      "Chefe do Mundo",
      "WorldBoss",
      "Capitão do Futebol"
    };
    public static List<string> NotceList = new List<string>();
    public static int[] Pve_Id = new int[4]
    {
      1243,
      30001,
      30002,
      30004
    };
    private static readonly int worldbossTime = 60;
    public static DateTime begin_time;
    public static int currentPVE_ID;
    public static DateTime end_time;
    public static int fight_time;
    public static bool fightOver;
    public static bool IsLeagueOpen;
    public static DateTime LeagueOpenTime;
    private static Dictionary<string, RankingPersonInfo> m_rankList;
    public static bool roomClose;
    public static bool worldOpen;

    public static bool CheckName(string NickName)
    {
      return WorldMgr.m_rankList.Keys.Contains<string>(NickName);
    }

    public static RankingPersonInfo GetSingleRank(string name)
    {
      return WorldMgr.m_rankList[name];
    }

    public static bool LoadNotice(string path)
    {
      string str1 = path + WorldMgr.SystemNoticeFile;
      if (!File.Exists(str1))
      {
        WorldMgr.log.Error((object) ("SystemNotice file : " + str1 + " not found !"));
      }
      else
      {
        try
        {
          foreach (XElement node in XDocument.Load(str1).Root.Nodes())
          {
            try
            {
              int.Parse(node.Attribute((XName) "id").Value);
              string str2 = node.Attribute((XName) "notice").Value;
              WorldMgr.NotceList.Add(str2);
            }
            catch (Exception ex)
            {
              WorldMgr.log.Error((object) "BattleMgr setup error:", ex);
            }
          }
        }
        catch (Exception ex)
        {
          WorldMgr.log.Error((object) "BattleMgr setup error:", ex);
        }
      }
      WorldMgr.log.InfoFormat("Total {0} syterm notice loaded.", (object) WorldMgr.NotceList.Count);
      return true;
    }

    public static void ReduceBlood(int value)
    {
      if (WorldMgr.current_blood <= 0L)
        return;
      WorldMgr.current_blood -= (long) value;
    }

    public static List<RankingPersonInfo> SelectTopTen()
    {
      List<RankingPersonInfo> rankingPersonInfoList = new List<RankingPersonInfo>();
      foreach (KeyValuePair<string, RankingPersonInfo> keyValuePair in (IEnumerable<KeyValuePair<string, RankingPersonInfo>>) WorldMgr.m_rankList.OrderByDescending<KeyValuePair<string, RankingPersonInfo>, int>((Func<KeyValuePair<string, RankingPersonInfo>, int>) (pair => pair.Value.Damage)))
      {
        if (rankingPersonInfoList.Count == 10)
          return rankingPersonInfoList;
        rankingPersonInfoList.Add(keyValuePair.Value);
      }
      return rankingPersonInfoList;
    }

    public static void SetupWorldBoss(int id)
    {
      WorldMgr.current_blood = WorldMgr.MAX_BLOOD;
      WorldMgr.begin_time = DateTime.Now;
      WorldMgr.end_time = WorldMgr.begin_time.AddDays(1.0);
      WorldMgr.fight_time = WorldMgr.worldbossTime - WorldMgr.begin_time.Minute;
      WorldMgr.fightOver = false;
      WorldMgr.roomClose = false;
      WorldMgr.currentPVE_ID = id;
      WorldMgr.worldOpen = true;
    }

    public static bool Start()
    {
      try
      {
        WorldMgr.m_rankList = new Dictionary<string, RankingPersonInfo>();
        WorldMgr.current_blood = WorldMgr.MAX_BLOOD;
        WorldMgr.begin_time = DateTime.Now;
        WorldMgr.LeagueOpenTime = DateTime.Now;
        WorldMgr.end_time = WorldMgr.begin_time.AddDays(1.0);
        WorldMgr.fightOver = true;
        WorldMgr.roomClose = true;
        WorldMgr.worldOpen = false;
        WorldMgr.IsLeagueOpen = false;
        return WorldMgr.LoadNotice(AppDomain.CurrentDomain.BaseDirectory);
      }
      catch (Exception ex)
      {
        WorldMgr.log.ErrorFormat("Load server list from db failed:{0}", (object) ex);
        return false;
      }
    }

    public static void UpdateFightTime()
    {
      if (WorldMgr.fightOver)
        return;
      WorldMgr.fight_time = WorldMgr.worldbossTime - WorldMgr.begin_time.Minute;
    }

    public static void UpdateRank(int damage, int honor, string nickName)
    {
      if (WorldMgr.m_rankList.Keys.Contains<string>(nickName))
      {
        WorldMgr.m_rankList[nickName].Damage += damage;
        WorldMgr.m_rankList[nickName].Honor += honor;
      }
      else
      {
        RankingPersonInfo rankingPersonInfo = new RankingPersonInfo()
        {
          ID = WorldMgr.m_rankList.Count + 1,
          Name = nickName,
          Damage = damage,
          Honor = honor
        };
        WorldMgr.m_rankList.Add(nickName, rankingPersonInfo);
      }
    }

    public static void WorldBossClearRank()
    {
      WorldMgr.m_rankList.Clear();
    }

    public static void WorldBossClose()
    {
      WorldMgr.worldOpen = false;
    }

    public static void WorldBossFightOver()
    {
      WorldMgr.fightOver = true;
    }

    public static void WorldBossRoomClose()
    {
      WorldMgr.roomClose = true;
    }

    private static string SystemNoticeFile
    {
      get
      {
        return ConfigurationManager.AppSettings["SystemNoticePath"];
      }
    }
  }
}
