// Decompiled with JetBrains decompiler
// Type: Center.Server.ConsortiaBossMgr
// Assembly: Center.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4391F35A-9CAD-44A9-AFE0-208E0547A0DD
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Center\Center.Server.dll

using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Center.Server
{
  public sealed class ConsortiaBossMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ReaderWriterLock m_clientLocker = new ReaderWriterLock();
    private static Dictionary<int, ConsortiaInfo> m_consortias = new Dictionary<int, ConsortiaInfo>();
    public static int TimeCheckingAward = 0;

    public static bool AddConsortia(int consortiaId, ConsortiaInfo consortia)
    {
      ConsortiaBossMgr.m_clientLocker.AcquireWriterLock(-1);
      try
      {
        if (ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId))
          return false;
        ConsortiaBossMgr.m_consortias.Add(consortiaId, consortia);
      }
      finally
      {
        ConsortiaBossMgr.m_clientLocker.ReleaseWriterLock();
      }
      return true;
    }

    public static void CloseConsortia(int consortiaId, bool IsBossDie)
    {
      if (!ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId) || ConsortiaBossMgr.m_consortias[consortiaId].bossState != 1)
        return;
      ConsortiaBossMgr.m_consortias[consortiaId].bossState = 2;
      ConsortiaBossMgr.m_consortias[consortiaId].IsSendAward = true;
      ConsortiaBossMgr.m_consortias[consortiaId].IsBossDie = IsBossDie;
      ConsortiaBossMgr.m_consortias[consortiaId].SendToClient = true;
    }

    public static bool ExtendAvailable(int consortiaId, int riches)
    {
      ConsortiaBossMgr.m_clientLocker.AcquireWriterLock(-1);
      try
      {
        if (ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId))
        {
          if (ConsortiaBossMgr.m_consortias[consortiaId].extendAvailableNum <= 0)
            return false;
          --ConsortiaBossMgr.m_consortias[consortiaId].extendAvailableNum;
          ConsortiaBossMgr.m_consortias[consortiaId].endTime = ConsortiaBossMgr.m_consortias[consortiaId].endTime.AddMinutes(10.0);
          ConsortiaBossMgr.m_consortias[consortiaId].Riches = riches;
        }
      }
      finally
      {
        ConsortiaBossMgr.m_clientLocker.ReleaseWriterLock();
      }
      return true;
    }

    public static List<int> GetAllConsortiaGetAward()
    {
      List<int> intList = new List<int>();
      ConsortiaBossMgr.m_clientLocker.AcquireReaderLock(-1);
      try
      {
        foreach (ConsortiaInfo consortiaInfo in ConsortiaBossMgr.m_consortias.Values)
        {
          int consortiaId = consortiaInfo.ConsortiaID;
          if (consortiaInfo.IsSendAward)
          {
            intList.Add(consortiaId);
            if (ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId))
              ConsortiaBossMgr.m_consortias[consortiaId].IsSendAward = false;
          }
        }
      }
      finally
      {
        ConsortiaBossMgr.m_clientLocker.ReleaseReaderLock();
      }
      return intList;
    }

    public static ConsortiaInfo GetConsortiaById(int consortiaId)
    {
      ConsortiaInfo consortiaInfo = (ConsortiaInfo) null;
      ConsortiaBossMgr.m_clientLocker.AcquireReaderLock(-1);
      try
      {
        if (ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId))
          consortiaInfo = ConsortiaBossMgr.m_consortias[consortiaId];
      }
      finally
      {
        ConsortiaBossMgr.m_clientLocker.ReleaseReaderLock();
      }
      return consortiaInfo;
    }

    public static bool Init()
    {
      bool flag = false;
      try
      {
        ConsortiaBossMgr.m_consortias.Clear();
        flag = true;
      }
      catch (Exception ex)
      {
        ConsortiaBossMgr.log.Error((object) "ConsortiaBossMgr Init", ex);
      }
      return flag;
    }

    public static void ResetConsortia(int consortiaId)
    {
      if (!ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId) || ConsortiaBossMgr.m_consortias[consortiaId].bossState != 2)
        return;
      ConsortiaBossMgr.m_consortias[consortiaId].bossState = 0;
      ConsortiaBossMgr.m_consortias[consortiaId].IsBossDie = false;
    }

    public static List<RankingPersonInfo> SelectRank(int consortiaId)
    {
      List<RankingPersonInfo> rankingPersonInfoList = new List<RankingPersonInfo>();
      ConsortiaBossMgr.m_clientLocker.AcquireWriterLock(-1);
      try
      {
        if (!ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId) || ConsortiaBossMgr.m_consortias[consortiaId].RankList == null)
          return rankingPersonInfoList;
        foreach (KeyValuePair<string, RankingPersonInfo> keyValuePair in (IEnumerable<KeyValuePair<string, RankingPersonInfo>>) ConsortiaBossMgr.m_consortias[consortiaId].RankList.OrderByDescending<KeyValuePair<string, RankingPersonInfo>, int>((Func<KeyValuePair<string, RankingPersonInfo>, int>) (pair => pair.Value.TotalDamage)))
          rankingPersonInfoList.Add(keyValuePair.Value);
      }
      finally
      {
        ConsortiaBossMgr.m_clientLocker.ReleaseWriterLock();
      }
      return rankingPersonInfoList;
    }

    public static void UpdateBlood(int consortiaId, int damage)
    {
      ConsortiaBossMgr.m_clientLocker.AcquireWriterLock(-1);
      try
      {
        if (!ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId))
          return;
        ConsortiaBossMgr.m_consortias[consortiaId].TotalAllMemberDame += (long) damage;
        if (ConsortiaBossMgr.m_consortias[consortiaId].TotalAllMemberDame >= ConsortiaBossMgr.m_consortias[consortiaId].MaxBlood)
          ConsortiaBossMgr.CloseConsortia(consortiaId, true);
      }
      finally
      {
        ConsortiaBossMgr.m_clientLocker.ReleaseWriterLock();
      }
    }

    public static bool UpdateConsortia(
      int consortiaId,
      int bossState,
      DateTime endTime,
      DateTime LastOpenBoss,
      long MaxBlood)
    {
      ConsortiaBossMgr.m_clientLocker.AcquireWriterLock(-1);
      try
      {
        if (ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId))
        {
          ConsortiaBossMgr.m_consortias[consortiaId].bossState = bossState;
          ConsortiaBossMgr.m_consortias[consortiaId].endTime = endTime;
          ConsortiaBossMgr.m_consortias[consortiaId].LastOpenBoss = LastOpenBoss;
          ConsortiaBossMgr.m_consortias[consortiaId].MaxBlood = MaxBlood;
          ConsortiaBossMgr.m_consortias[consortiaId].TotalAllMemberDame = 0L;
        }
      }
      finally
      {
        ConsortiaBossMgr.m_clientLocker.ReleaseWriterLock();
      }
      return true;
    }

    public static void UpdateRank(
      int consortiaId,
      int damage,
      int richer,
      int honor,
      string nickName,
      int UserID)
    {
      ConsortiaBossMgr.m_clientLocker.AcquireWriterLock(-1);
      try
      {
        if (!ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId))
          return;
        if (ConsortiaBossMgr.m_consortias[consortiaId].RankList == null)
          ConsortiaBossMgr.m_consortias[consortiaId].RankList = new Dictionary<string, RankingPersonInfo>();
        if (ConsortiaBossMgr.m_consortias[consortiaId].RankList.ContainsKey(nickName))
        {
          ConsortiaBossMgr.m_consortias[consortiaId].RankList[nickName].TotalDamage += damage;
          ConsortiaBossMgr.m_consortias[consortiaId].RankList[nickName].Damage += richer;
          ConsortiaBossMgr.m_consortias[consortiaId].RankList[nickName].Honor += honor;
        }
        else
        {
          RankingPersonInfo rankingPersonInfo = new RankingPersonInfo()
          {
            ID = ConsortiaBossMgr.m_consortias[consortiaId].RankList.Count + 1,
            Name = nickName,
            UserID = UserID,
            TotalDamage = damage,
            Damage = richer,
            Honor = honor
          };
          ConsortiaBossMgr.m_consortias[consortiaId].RankList.Add(nickName, rankingPersonInfo);
        }
      }
      finally
      {
        ConsortiaBossMgr.m_clientLocker.ReleaseWriterLock();
      }
    }

    public static void UpdateSendToClient(int consortiaId)
    {
      ConsortiaBossMgr.m_clientLocker.AcquireWriterLock(-1);
      try
      {
        if (!ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId))
          return;
        ConsortiaBossMgr.m_consortias[consortiaId].SendToClient = false;
      }
      finally
      {
        ConsortiaBossMgr.m_clientLocker.ReleaseWriterLock();
      }
    }

    public static void UpdateTime()
    {
      foreach (ConsortiaInfo consortiaInfo in ConsortiaBossMgr.m_consortias.Values)
      {
        if (consortiaInfo.endTime < DateTime.Now)
          ConsortiaBossMgr.CloseConsortia(consortiaInfo.ConsortiaID, false);
        if (consortiaInfo.LastOpenBoss.Date < DateTime.Now.Date)
          ConsortiaBossMgr.ResetConsortia(consortiaInfo.ConsortiaID);
      }
    }
  }
}
