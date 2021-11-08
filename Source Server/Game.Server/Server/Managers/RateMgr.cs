// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.RateMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
  public class RateMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ReaderWriterLock m_lock = new ReaderWriterLock();
    private static ArrayList m_RateInfos = new ArrayList();

    public static float GetRate(eRateType eType)
    {
      float num = 1f;
      RateMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        RateInfo rateInfoWithType = RateMgr.GetRateInfoWithType((int) eType);
        if (rateInfoWithType == null)
          return num;
        if ((double) rateInfoWithType.Rate == 0.0)
          return 1f;
        if (RateMgr.IsValid(rateInfoWithType))
          num = rateInfoWithType.Rate;
      }
      catch
      {
      }
      finally
      {
        RateMgr.m_lock.ReleaseReaderLock();
      }
      return num;
    }

    private static RateInfo GetRateInfoWithType(int type)
    {
      foreach (RateInfo rateInfo in RateMgr.m_RateInfos)
      {
        if (rateInfo.Type == type)
          return rateInfo;
      }
      return (RateInfo) null;
    }

    public static bool Init(GameServerConfig config)
    {
      RateMgr.m_lock.AcquireWriterLock(-1);
      try
      {
        using (ServiceBussiness serviceBussiness = new ServiceBussiness())
          RateMgr.m_RateInfos = serviceBussiness.GetRate(config.ServerID);
        return true;
      }
      catch (Exception ex)
      {
        if (RateMgr.log.IsErrorEnabled)
          RateMgr.log.Error((object) nameof (RateMgr), ex);
        return false;
      }
      finally
      {
        RateMgr.m_lock.ReleaseWriterLock();
      }
    }

    private static bool IsValid(RateInfo _RateInfo)
    {
      DateTime beginDay = _RateInfo.BeginDay;
      DateTime endDay = _RateInfo.EndDay;
      if (_RateInfo.BeginDay.Year <= DateTime.Now.Year)
      {
        DateTime dateTime = DateTime.Now;
        int year1 = dateTime.Year;
        dateTime = _RateInfo.EndDay;
        int year2 = dateTime.Year;
        if (year1 <= year2)
        {
          dateTime = _RateInfo.BeginDay;
          int dayOfYear1 = dateTime.DayOfYear;
          dateTime = DateTime.Now;
          int dayOfYear2 = dateTime.DayOfYear;
          if (dayOfYear1 <= dayOfYear2)
          {
            dateTime = DateTime.Now;
            int dayOfYear3 = dateTime.DayOfYear;
            dateTime = _RateInfo.EndDay;
            int dayOfYear4 = dateTime.DayOfYear;
            if (dayOfYear3 <= dayOfYear4)
            {
              dateTime = _RateInfo.BeginTime;
              TimeSpan timeOfDay1 = dateTime.TimeOfDay;
              dateTime = DateTime.Now;
              TimeSpan timeOfDay2 = dateTime.TimeOfDay;
              if (timeOfDay1 <= timeOfDay2)
              {
                dateTime = DateTime.Now;
                TimeSpan timeOfDay3 = dateTime.TimeOfDay;
                dateTime = _RateInfo.EndTime;
                TimeSpan timeOfDay4 = dateTime.TimeOfDay;
                return timeOfDay3 <= timeOfDay4;
              }
            }
          }
        }
      }
      return false;
    }

    public static bool ReLoad()
    {
      return RateMgr.Init(GameServer.Instance.Configuration);
    }
  }
}
