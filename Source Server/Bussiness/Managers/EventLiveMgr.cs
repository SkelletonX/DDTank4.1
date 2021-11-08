// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.EventLiveMgr
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
  public class EventLiveMgr
  {
    private static Dictionary<int, EventLiveInfo> m_EventLiveInfo = new Dictionary<int, EventLiveInfo>();
    private static Dictionary<int, List<EventLiveGoods>> m_EventLiveGoods = new Dictionary<int, List<EventLiveGoods>>();
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static bool Init()
    {
      return EventLiveMgr.ReLoad();
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, EventLiveInfo> events = EventLiveMgr.LoadEventLiveInfoDb();
        Dictionary<int, List<EventLiveGoods>> dictionary = EventLiveMgr.LoadEventGoods(events);
        if (events.Count > 0)
        {
          Interlocked.Exchange<Dictionary<int, EventLiveInfo>>(ref EventLiveMgr.m_EventLiveInfo, events);
          Interlocked.Exchange<Dictionary<int, List<EventLiveGoods>>>(ref EventLiveMgr.m_EventLiveGoods, dictionary);
        }
        return true;
      }
      catch (Exception ex)
      {
        EventLiveMgr.log.Error((object) nameof (EventLiveMgr), ex);
      }
      return false;
    }

    public static Dictionary<int, EventLiveInfo> LoadEventLiveInfoDb()
    {
      Dictionary<int, EventLiveInfo> dictionary = new Dictionary<int, EventLiveInfo>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (EventLiveInfo eventLiveInfo in produceBussiness.GetAllEventLive())
        {
          if (!dictionary.ContainsKey(eventLiveInfo.EventID))
            dictionary.Add(eventLiveInfo.EventID, eventLiveInfo);
        }
      }
      return dictionary;
    }

    public static Dictionary<int, List<EventLiveGoods>> LoadEventGoods(
      Dictionary<int, EventLiveInfo> events)
    {
      Dictionary<int, List<EventLiveGoods>> dictionary = new Dictionary<int, List<EventLiveGoods>>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        EventLiveGoods[] allEventLiveGoods = produceBussiness.GetAllEventLiveGoods();
        foreach (EventLiveInfo eventLiveInfo in events.Values)
        {
          EventLiveInfo eventLive = eventLiveInfo;
          IEnumerable<EventLiveGoods> source = ((IEnumerable<EventLiveGoods>) allEventLiveGoods).Where<EventLiveGoods>((Func<EventLiveGoods, bool>) (s => s.EventID == eventLive.EventID));
          dictionary.Add(eventLive.EventID, source.ToList<EventLiveGoods>());
        }
      }
      return dictionary;
    }

    public static EventLiveInfo GetSingleEvent(int id)
    {
      return !EventLiveMgr.m_EventLiveInfo.ContainsKey(id) ? (EventLiveInfo) null : EventLiveMgr.m_EventLiveInfo[id];
    }

    public static List<EventLiveGoods> GetEventGoods(EventLiveInfo info)
    {
      return !EventLiveMgr.m_EventLiveGoods.ContainsKey(info.EventID) ? (List<EventLiveGoods>) null : EventLiveMgr.m_EventLiveGoods[info.EventID];
    }

    public static List<EventLiveInfo> GetAllEventInfo()
    {
      return EventLiveMgr.m_EventLiveInfo.Values.ToList<EventLiveInfo>();
    }
  }
}
