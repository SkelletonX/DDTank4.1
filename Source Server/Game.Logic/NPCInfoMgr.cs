// Decompiled with JetBrains decompiler
// Type: Game.Logic.NPCInfoMgr
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
  public static class NPCInfoMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ReaderWriterLock m_lock = new ReaderWriterLock();
    private static Dictionary<int, NpcInfo> m_npcs = new Dictionary<int, NpcInfo>();
    private static ThreadSafeRandom m_rand = new ThreadSafeRandom();

    public static NpcInfo GetNpcInfoById(int id)
    {
      if (NPCInfoMgr.m_npcs.ContainsKey(id))
        return NPCInfoMgr.m_npcs[id];
      return (NpcInfo) null;
    }

    public static bool Init()
    {
      return NPCInfoMgr.ReLoad();
    }

    private static Dictionary<int, NpcInfo> LoadFromDatabase()
    {
      Dictionary<int, NpcInfo> dictionary = new Dictionary<int, NpcInfo>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (NpcInfo npcInfo in produceBussiness.GetAllNPCInfo())
        {
          if (!dictionary.ContainsKey(npcInfo.ID))
            dictionary.Add(npcInfo.ID, npcInfo);
        }
      }
      return dictionary;
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, NpcInfo> dictionary = NPCInfoMgr.LoadFromDatabase();
        if (dictionary != null && dictionary.Count > 0)
          Interlocked.Exchange<Dictionary<int, NpcInfo>>(ref NPCInfoMgr.m_npcs, dictionary);
        return true;
      }
      catch (Exception ex)
      {
        NPCInfoMgr.log.Error((object) nameof (NPCInfoMgr), ex);
      }
      return false;
    }
  }
}
