// Decompiled with JetBrains decompiler
// Type: Game.Logic.MissionInfoMgr
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Logic
{
  public static class MissionInfoMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ReaderWriterLock m_lock = new ReaderWriterLock();
    private static Dictionary<int, MissionInfo> m_missionInfos = new Dictionary<int, MissionInfo>();
    private static ThreadSafeRandom m_rand = new ThreadSafeRandom();

    public static MissionInfo GetMissionInfo(int id)
    {
      if (MissionInfoMgr.m_missionInfos.ContainsKey(id))
        return MissionInfoMgr.m_missionInfos[id];
      return (MissionInfo) null;
    }

    public static bool Init()
    {
      return MissionInfoMgr.Reload();
    }

    private static Dictionary<int, MissionInfo> LoadFromDatabase()
    {
      Dictionary<int, MissionInfo> dictionary = new Dictionary<int, MissionInfo>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (MissionInfo missionInfo in produceBussiness.GetAllMissionInfo())
        {
          if (!dictionary.ContainsKey(missionInfo.Id))
            dictionary.Add(missionInfo.Id, missionInfo);
        }
      }
      return dictionary;
    }

    public static bool Reload()
    {
      try
      {
        Dictionary<int, MissionInfo> dictionary = MissionInfoMgr.LoadFromDatabase();
        if (dictionary.Count > 0)
          Interlocked.Exchange<Dictionary<int, MissionInfo>>(ref MissionInfoMgr.m_missionInfos, dictionary);
        return true;
      }
      catch (Exception ex)
      {
        MissionInfoMgr.log.Error((object) nameof (MissionInfoMgr), ex);
      }
      return false;
    }
  }
}
