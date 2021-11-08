// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.FairBattleRewardMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
  public class FairBattleRewardMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<int, FairBattleRewardInfo> _fairBattleRewards;
    private static ReaderWriterLock m_lock;
    private static ThreadSafeRandom rand;

    public static FairBattleRewardInfo FindLevel(int Level)
    {
      FairBattleRewardMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (FairBattleRewardMgr._fairBattleRewards.ContainsKey(Level))
          return FairBattleRewardMgr._fairBattleRewards[Level];
      }
      catch
      {
      }
      finally
      {
        FairBattleRewardMgr.m_lock.ReleaseReaderLock();
      }
      return (FairBattleRewardInfo) null;
    }

    public static FairBattleRewardInfo GetBattleDataByPrestige(int Prestige)
    {
      for (int index = FairBattleRewardMgr._fairBattleRewards.Values.Count - 1; index >= 0; --index)
      {
        if (Prestige >= FairBattleRewardMgr._fairBattleRewards[index].Prestige)
          return FairBattleRewardMgr._fairBattleRewards[index];
      }
      return (FairBattleRewardInfo) null;
    }

    public static int GetGP(int level)
    {
      if (FairBattleRewardMgr.MaxLevel() > level && level > 0)
        return FairBattleRewardMgr.FindLevel(level - 1).Prestige;
      return 0;
    }

    public static int GetLevel(int GP)
    {
      if (GP >= FairBattleRewardMgr.FindLevel(FairBattleRewardMgr.MaxLevel()).Prestige)
        return FairBattleRewardMgr.MaxLevel();
      for (int Level = 1; Level <= FairBattleRewardMgr.MaxLevel(); ++Level)
      {
        if (GP < FairBattleRewardMgr.FindLevel(Level).Prestige)
        {
          if ((uint) (Level - 1) > 0U)
            return Level - 1;
          return 1;
        }
      }
      return 1;
    }

    public static bool Init()
    {
      try
      {
        FairBattleRewardMgr.m_lock = new ReaderWriterLock();
        FairBattleRewardMgr._fairBattleRewards = new Dictionary<int, FairBattleRewardInfo>();
        FairBattleRewardMgr.rand = new ThreadSafeRandom();
        return FairBattleRewardMgr.LoadFairBattleReward(FairBattleRewardMgr._fairBattleRewards);
      }
      catch (Exception ex)
      {
        if (FairBattleRewardMgr.log.IsErrorEnabled)
          FairBattleRewardMgr.log.Error((object) nameof (FairBattleRewardMgr), ex);
        return false;
      }
    }

    private static bool LoadFairBattleReward(Dictionary<int, FairBattleRewardInfo> Level)
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (FairBattleRewardInfo battleRewardInfo in produceBussiness.GetAllFairBattleReward())
        {
          if (!Level.ContainsKey(battleRewardInfo.Level))
            Level.Add(battleRewardInfo.Level, battleRewardInfo);
        }
      }
      return true;
    }

    public static int MaxLevel()
    {
      if (FairBattleRewardMgr._fairBattleRewards == null)
        FairBattleRewardMgr.Init();
      return FairBattleRewardMgr._fairBattleRewards.Values.Count;
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, FairBattleRewardInfo> Level = new Dictionary<int, FairBattleRewardInfo>();
        if (FairBattleRewardMgr.LoadFairBattleReward(Level))
        {
          FairBattleRewardMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            FairBattleRewardMgr._fairBattleRewards = Level;
            return true;
          }
          catch
          {
          }
          finally
          {
            FairBattleRewardMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (FairBattleRewardMgr.log.IsErrorEnabled)
          FairBattleRewardMgr.log.Error((object) "FairBattleMgr", ex);
      }
      return false;
    }
  }
}
