// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.ConsortiaLevelMgr
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
  public class ConsortiaLevelMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<int, ConsortiaLevelInfo> _consortiaLevel;
    private static ReaderWriterLock m_lock;
    private static ThreadSafeRandom rand;

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, ConsortiaLevelInfo> consortiaLevel = new Dictionary<int, ConsortiaLevelInfo>();
        if (ConsortiaLevelMgr.Load(consortiaLevel))
        {
          ConsortiaLevelMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            ConsortiaLevelMgr._consortiaLevel = consortiaLevel;
            return true;
          }
          catch
          {
          }
          finally
          {
            ConsortiaLevelMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (ConsortiaLevelMgr.log.IsErrorEnabled)
          ConsortiaLevelMgr.log.Error((object) nameof (ConsortiaLevelMgr), ex);
      }
      return false;
    }

    public static bool Init()
    {
      try
      {
        ConsortiaLevelMgr.m_lock = new ReaderWriterLock();
        ConsortiaLevelMgr._consortiaLevel = new Dictionary<int, ConsortiaLevelInfo>();
        ConsortiaLevelMgr.rand = new ThreadSafeRandom();
        return ConsortiaLevelMgr.Load(ConsortiaLevelMgr._consortiaLevel);
      }
      catch (Exception ex)
      {
        if (ConsortiaLevelMgr.log.IsErrorEnabled)
          ConsortiaLevelMgr.log.Error((object) nameof (ConsortiaLevelMgr), ex);
        return false;
      }
    }

    private static bool Load(Dictionary<int, ConsortiaLevelInfo> consortiaLevel)
    {
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        foreach (ConsortiaLevelInfo consortiaLevelInfo in consortiaBussiness.GetAllConsortiaLevel())
        {
          if (!consortiaLevel.ContainsKey(consortiaLevelInfo.Level))
            consortiaLevel.Add(consortiaLevelInfo.Level, consortiaLevelInfo);
        }
      }
      return true;
    }

    public static ConsortiaLevelInfo FindConsortiaLevelInfo(int level)
    {
      ConsortiaLevelMgr.m_lock.AcquireReaderLock(-1);
      try
      {
        if (ConsortiaLevelMgr._consortiaLevel.ContainsKey(level))
          return ConsortiaLevelMgr._consortiaLevel[level];
      }
      catch
      {
      }
      finally
      {
        ConsortiaLevelMgr.m_lock.ReleaseReaderLock();
      }
      return (ConsortiaLevelInfo) null;
    }
  }
}
