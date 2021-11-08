// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.ConsortiaExtraMgr
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
  public class ConsortiaExtraMgr
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<int, ConsortiaBadgeConfigInfo> dictionary_3 = new Dictionary<int, ConsortiaBadgeConfigInfo>();
    private static ReaderWriterLock readerWriterLock_0 = new ReaderWriterLock();
    private static int int_0 = 10000;
    private static Dictionary<int, ConsortiaLevelInfo> dictionary_0;
    private static Dictionary<int, ConsortiaBuffTempInfo> dictionary_1;
    private static Dictionary<int, ConsortiaBossConfigInfo> dictionary_2;
    private static ThreadSafeRandom threadSafeRandom_0;

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, ConsortiaLevelInfo> VDAEVuh8JIiPH5u2QO = new Dictionary<int, ConsortiaLevelInfo>();
        Dictionary<int, ConsortiaBuffTempInfo> uVEcJgIYrOMpJAq3U5 = new Dictionary<int, ConsortiaBuffTempInfo>();
        Dictionary<int, ConsortiaBadgeConfigInfo> dictionary = ConsortiaExtraMgr.smethod_0();
        if (ConsortiaExtraMgr.OvbRxniGbs(VDAEVuh8JIiPH5u2QO, uVEcJgIYrOMpJAq3U5))
        {
          ConsortiaExtraMgr.readerWriterLock_0.AcquireWriterLock(-1);
          try
          {
            ConsortiaExtraMgr.dictionary_0 = VDAEVuh8JIiPH5u2QO;
            ConsortiaExtraMgr.dictionary_1 = uVEcJgIYrOMpJAq3U5;
            if (dictionary.Values.Count > 0)
              Interlocked.Exchange<Dictionary<int, ConsortiaBadgeConfigInfo>>(ref ConsortiaExtraMgr.dictionary_3, dictionary);
            return true;
          }
          catch
          {
          }
          finally
          {
            ConsortiaExtraMgr.readerWriterLock_0.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (ConsortiaExtraMgr.ilog_0.IsErrorEnabled)
          ConsortiaExtraMgr.ilog_0.Error((object) nameof (ConsortiaExtraMgr), ex);
      }
      return false;
    }

    public static bool Init()
    {
      try
      {
        return ConsortiaExtraMgr.ReLoad();
      }
      catch (Exception ex)
      {
        if (ConsortiaExtraMgr.ilog_0.IsErrorEnabled)
          ConsortiaExtraMgr.ilog_0.Error((object) nameof (ConsortiaExtraMgr), ex);
        return false;
      }
    }

    private static Dictionary<int, ConsortiaBadgeConfigInfo> smethod_0()
    {
      Dictionary<int, ConsortiaBadgeConfigInfo> dictionary = new Dictionary<int, ConsortiaBadgeConfigInfo>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (ConsortiaBadgeConfigInfo consortiaBadgeConfigInfo in produceBussiness.GetAllConsortiaBadgeConfig())
        {
          if (!dictionary.ContainsKey(consortiaBadgeConfigInfo.BadgeID))
            dictionary.Add(consortiaBadgeConfigInfo.BadgeID, consortiaBadgeConfigInfo);
        }
      }
      return dictionary;
    }

    private static bool OvbRxniGbs(
      Dictionary<int, ConsortiaLevelInfo> VDAEVuh8JIiPH5u2QO,
      Dictionary<int, ConsortiaBuffTempInfo> uVEcJgIYrOMpJAq3U5)
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (ConsortiaLevelInfo consortiaLevelInfo in produceBussiness.GetAllConsortiaLevel())
        {
          if (!VDAEVuh8JIiPH5u2QO.ContainsKey(consortiaLevelInfo.Level))
            VDAEVuh8JIiPH5u2QO.Add(consortiaLevelInfo.Level, consortiaLevelInfo);
        }
        foreach (ConsortiaBuffTempInfo consortiaBuffTempInfo in produceBussiness.GetAllConsortiaBuffTemp())
        {
          if (!uVEcJgIYrOMpJAq3U5.ContainsKey(consortiaBuffTempInfo.id))
            uVEcJgIYrOMpJAq3U5.Add(consortiaBuffTempInfo.id, consortiaBuffTempInfo);
        }
      }
      return true;
    }

    public static ConsortiaBadgeConfigInfo FindConsortiaBadgeConfig(int id)
    {
      if (ConsortiaExtraMgr.dictionary_3.ContainsKey(id))
        return ConsortiaExtraMgr.dictionary_3[id];
      return (ConsortiaBadgeConfigInfo) null;
    }

    public static ConsortiaBossConfigInfo FindConsortiaBossInfo(int level)
    {
      ConsortiaExtraMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        if (ConsortiaExtraMgr.dictionary_2.ContainsKey(level))
          return ConsortiaExtraMgr.dictionary_2[level];
      }
      catch
      {
      }
      finally
      {
        ConsortiaExtraMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return (ConsortiaBossConfigInfo) null;
    }

    public static ConsortiaLevelInfo FindConsortiaLevelInfo(int level)
    {
      ConsortiaExtraMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        if (ConsortiaExtraMgr.dictionary_0.ContainsKey(level))
          return ConsortiaExtraMgr.dictionary_0[level];
      }
      catch
      {
      }
      finally
      {
        ConsortiaExtraMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return (ConsortiaLevelInfo) null;
    }

    public static ConsortiaBuffTempInfo FindConsortiaBuffInfo(int id)
    {
      ConsortiaExtraMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        if (ConsortiaExtraMgr.dictionary_1.ContainsKey(id))
          return ConsortiaExtraMgr.dictionary_1[id];
      }
      catch
      {
      }
      finally
      {
        ConsortiaExtraMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return (ConsortiaBuffTempInfo) null;
    }

    public static List<ConsortiaBuffTempInfo> GetAllConsortiaBuff()
    {
      ConsortiaExtraMgr.readerWriterLock_0.AcquireReaderLock(-1);
      List<ConsortiaBuffTempInfo> consortiaBuffTempInfoList = new List<ConsortiaBuffTempInfo>();
      try
      {
        foreach (ConsortiaBuffTempInfo consortiaBuffTempInfo in ConsortiaExtraMgr.dictionary_1.Values)
          consortiaBuffTempInfoList.Add(consortiaBuffTempInfo);
      }
      catch
      {
      }
      finally
      {
        ConsortiaExtraMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return consortiaBuffTempInfoList;
    }

    public static List<ConsortiaBuffTempInfo> GetAllConsortiaBuff(
      int level,
      int type)
    {
      ConsortiaExtraMgr.readerWriterLock_0.AcquireReaderLock(-1);
      List<ConsortiaBuffTempInfo> consortiaBuffTempInfoList = new List<ConsortiaBuffTempInfo>();
      try
      {
        foreach (ConsortiaBuffTempInfo consortiaBuffTempInfo in ConsortiaExtraMgr.dictionary_1.Values)
        {
          if (consortiaBuffTempInfo.level == level && consortiaBuffTempInfo.type == type)
            consortiaBuffTempInfoList.Add(consortiaBuffTempInfo);
        }
      }
      catch
      {
      }
      finally
      {
        ConsortiaExtraMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return consortiaBuffTempInfoList;
    }
  }
}
