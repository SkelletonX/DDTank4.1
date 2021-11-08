// Decompiled with JetBrains decompiler
// Type: Game.Logic.PveInfoMgr
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Game.Logic
{
  public static class PveInfoMgr
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<int, PveInfo> dictionary_0 = new Dictionary<int, PveInfo>();
    private static ReaderWriterLock readerWriterLock_0 = new ReaderWriterLock();
    private static ThreadSafeRandom threadSafeRandom_0 = new ThreadSafeRandom();

    public static bool Init()
    {
      return PveInfoMgr.ReLoad();
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, PveInfo> dictionary = PveInfoMgr.LoadFromDatabase();
        if (dictionary.Count > 0)
          Interlocked.Exchange<Dictionary<int, PveInfo>>(ref PveInfoMgr.dictionary_0, dictionary);
        return true;
      }
      catch (Exception ex)
      {
        PveInfoMgr.ilog_0.Error((object) nameof (PveInfoMgr), ex);
      }
      return false;
    }

    public static Dictionary<int, PveInfo> LoadFromDatabase()
    {
      Dictionary<int, PveInfo> dictionary = new Dictionary<int, PveInfo>();
      using (PveBussiness pveBussiness = new PveBussiness())
      {
        foreach (PveInfo allPveInfo in pveBussiness.GetAllPveInfos())
        {
          if (!dictionary.ContainsKey(allPveInfo.ID))
            dictionary.Add(allPveInfo.ID, allPveInfo);
        }
      }
      return dictionary;
    }

    public static PveInfo GetPveInfoById(int id)
    {
      if (PveInfoMgr.dictionary_0.ContainsKey(id))
        return PveInfoMgr.dictionary_0[id];
      return (PveInfo) null;
    }

    public static PveInfo[] GetPveInfo()
    {
      if (PveInfoMgr.dictionary_0 == null)
        PveInfoMgr.ReLoad();
      return PveInfoMgr.dictionary_0.Values.ToArray<PveInfo>();
    }

    public static PveInfo GetPveInfoByType(eRoomType roomType, int levelLimits)
    {
      switch (roomType)
      {
        case eRoomType.Exploration:
          using (Dictionary<int, PveInfo>.ValueCollection.Enumerator enumerator = PveInfoMgr.dictionary_0.Values.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              PveInfo current = enumerator.Current;
              if ((eRoomType) current.Type == roomType && current.LevelLimits == levelLimits)
                return current;
            }
            break;
          }
        case eRoomType.Boss:
        case eRoomType.Dungeon:
        case eRoomType.FightLab:
        case eRoomType.Freshman:
        case eRoomType.Academy:
        case eRoomType.Labyrinth:
        case eRoomType.CoupleBoss:
          using (Dictionary<int, PveInfo>.ValueCollection.Enumerator enumerator = PveInfoMgr.dictionary_0.Values.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              PveInfo current = enumerator.Current;
              if ((eRoomType) current.Type == roomType)
                return current;
            }
            break;
          }
      }
      return (PveInfo) null;
    }
  }
}
