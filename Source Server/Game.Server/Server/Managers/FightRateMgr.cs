// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.FightRateMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Rooms;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
  public class FightRateMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected static Dictionary<int, FightRateInfo> _fightRate;
    private static ReaderWriterLock m_lock;

    public static bool CanChangeStyle(BaseRoom game, GSPacketIn pkg)
    {
      FightRateInfo[] allFightRateInfo = FightRateMgr.GetAllFightRateInfo();
      try
      {
        foreach (FightRateInfo fightRateInfo in allFightRateInfo)
        {
          DateTime dateTime = fightRateInfo.BeginDay;
          int year1 = dateTime.Year;
          dateTime = DateTime.Now;
          int year2 = dateTime.Year;
          if (year1 <= year2)
          {
            dateTime = DateTime.Now;
            int year3 = dateTime.Year;
            dateTime = fightRateInfo.EndDay;
            int year4 = dateTime.Year;
            if (year3 <= year4)
            {
              dateTime = fightRateInfo.BeginDay;
              int dayOfYear1 = dateTime.DayOfYear;
              dateTime = DateTime.Now;
              int dayOfYear2 = dateTime.DayOfYear;
              if (dayOfYear1 <= dayOfYear2)
              {
                dateTime = DateTime.Now;
                int dayOfYear3 = dateTime.DayOfYear;
                dateTime = fightRateInfo.EndDay;
                int dayOfYear4 = dateTime.DayOfYear;
                if (dayOfYear3 <= dayOfYear4)
                {
                  dateTime = fightRateInfo.BeginTime;
                  TimeSpan timeOfDay1 = dateTime.TimeOfDay;
                  dateTime = DateTime.Now;
                  TimeSpan timeOfDay2 = dateTime.TimeOfDay;
                  if (timeOfDay1 <= timeOfDay2)
                  {
                    dateTime = DateTime.Now;
                    TimeSpan timeOfDay3 = dateTime.TimeOfDay;
                    dateTime = fightRateInfo.EndTime;
                    TimeSpan timeOfDay4 = dateTime.TimeOfDay;
                    if (timeOfDay3 <= timeOfDay4 && ThreadSafeRandom.NextStatic(1000000) < fightRateInfo.Rate)
                      return true;
                  }
                }
              }
            }
          }
        }
      }
      catch
      {
      }
      pkg.WriteBoolean(false);
      return false;
    }

    public static FightRateInfo[] GetAllFightRateInfo()
    {
      FightRateInfo[] fightRateInfoArray = (FightRateInfo[]) null;
      FightRateMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        fightRateInfoArray = FightRateMgr._fightRate.Values.ToArray<FightRateInfo>();
      }
      catch
      {
      }
      finally
      {
        FightRateMgr.m_lock.ReleaseReaderLock();
      }
      return fightRateInfoArray ?? new FightRateInfo[0];
    }

    public static bool Init()
    {
      try
      {
        FightRateMgr.m_lock = new ReaderWriterLock();
        FightRateMgr._fightRate = new Dictionary<int, FightRateInfo>();
        return FightRateMgr.LoadFightRate(FightRateMgr._fightRate);
      }
      catch (Exception ex)
      {
        if (FightRateMgr.log.IsErrorEnabled)
          FightRateMgr.log.Error((object) "AwardMgr", ex);
        return false;
      }
    }

    private static bool LoadFightRate(Dictionary<int, FightRateInfo> fighRate)
    {
      using (ServiceBussiness serviceBussiness = new ServiceBussiness())
      {
        foreach (FightRateInfo fightRateInfo in serviceBussiness.GetFightRate(GameServer.Instance.Configuration.ServerID))
        {
          if (!fighRate.ContainsKey(fightRateInfo.ID))
            fighRate.Add(fightRateInfo.ID, fightRateInfo);
        }
      }
      return true;
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, FightRateInfo> fighRate = new Dictionary<int, FightRateInfo>();
        if (FightRateMgr.LoadFightRate(fighRate))
        {
          FightRateMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            FightRateMgr._fightRate = fighRate;
            return true;
          }
          catch
          {
          }
          finally
          {
            FightRateMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (FightRateMgr.log.IsErrorEnabled)
          FightRateMgr.log.Error((object) "AwardMgr", ex);
      }
      return false;
    }
  }
}
