// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.RankMgr
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
  public class RankMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ReaderWriterLock m_lock = new ReaderWriterLock();
    private static Dictionary<int, UserMatchInfo> _matchs;
    protected static Timer _timer;

    public static void BeginTimer()
    {
      int num = 3600000;
      if (RankMgr._timer == null)
        RankMgr._timer = new Timer(new TimerCallback(RankMgr.TimeCheck), (object) null, num, num);
      else
        RankMgr._timer.Change(num, num);
    }

    public static UserMatchInfo FindRank(int UserID)
    {
      RankMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (RankMgr._matchs.ContainsKey(UserID))
          return RankMgr._matchs[UserID];
      }
      catch
      {
      }
      finally
      {
        RankMgr.m_lock.ReleaseReaderLock();
      }
      return (UserMatchInfo) null;
    }

    public static bool Init()
    {
      try
      {
        RankMgr.m_lock = new ReaderWriterLock();
        RankMgr._matchs = new Dictionary<int, UserMatchInfo>();
        RankMgr.BeginTimer();
        return RankMgr.ReLoad();
      }
      catch (Exception ex)
      {
        if (RankMgr.log.IsErrorEnabled)
          RankMgr.log.Error((object) nameof (RankMgr), ex);
        return false;
      }
    }

    private static bool LoadData(Dictionary<int, UserMatchInfo> Match)
    {
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        foreach (UserMatchInfo userMatchInfo in playerBussiness.GetAllUserMatchInfo())
        {
          if (!Match.ContainsKey(userMatchInfo.UserID))
            Match.Add(userMatchInfo.UserID, userMatchInfo);
        }
      }
      return true;
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, UserMatchInfo> Match = new Dictionary<int, UserMatchInfo>();
        if (RankMgr.LoadData(Match))
        {
          RankMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            RankMgr._matchs = Match;
            return true;
          }
          catch
          {
          }
          finally
          {
            RankMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (RankMgr.log.IsErrorEnabled)
          RankMgr.log.Error((object) nameof (RankMgr), ex);
      }
      return false;
    }

    public void StopTimer()
    {
      if (RankMgr._timer == null)
        return;
      RankMgr._timer.Dispose();
      RankMgr._timer = (Timer) null;
    }

    protected static void TimeCheck(object sender)
    {
      try
      {
        int tickCount = Environment.TickCount;
        ThreadPriority priority = Thread.CurrentThread.Priority;
        Thread.CurrentThread.Priority = ThreadPriority.Lowest;
        RankMgr.ReLoad();
        Thread.CurrentThread.Priority = priority;
        int num = Environment.TickCount - tickCount;
      }
      catch (Exception ex)
      {
        Console.WriteLine("TimeCheck Rank: " + (object) ex);
      }
    }
  }
}
