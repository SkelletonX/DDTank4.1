// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.EventAwardMgr
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
  public class EventAwardMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ThreadSafeRandom rand = new ThreadSafeRandom();
    private static EventAwardInfo[] m_eventAward;
    private static Dictionary<int, List<EventAwardInfo>> m_EventAwards;

    public static void CreateEventAward(eEventType DateId)
    {
    }

    public static EventAwardInfo CreateSearchGoodsAward(eEventType DataId)
    {
      List<EventAwardInfo> eventAwardInfoList = new List<EventAwardInfo>();
      List<EventAwardInfo> eventAward = EventAwardMgr.FindEventAward(DataId);
      int num1 = 1;
      int maxRound = ThreadSafeRandom.NextStatic(eventAward.Select<EventAwardInfo, int>((Func<EventAwardInfo, int>) (s => s.Random)).Max());
      List<EventAwardInfo> list = eventAward.Where<EventAwardInfo>((Func<EventAwardInfo, bool>) (s => s.Random >= maxRound)).ToList<EventAwardInfo>();
      int num2 = list.Count<EventAwardInfo>();
      if (num2 > 0)
      {
        int count = num1 > num2 ? num2 : num1;
        foreach (int randomUnrepeat in EventAwardMgr.GetRandomUnrepeatArray(0, num2 - 1, count))
        {
          EventAwardInfo eventAwardInfo = list[randomUnrepeat];
          eventAwardInfoList.Add(eventAwardInfo);
        }
      }
      foreach (EventAwardInfo eventAwardInfo in eventAwardInfoList)
      {
        if (ItemMgr.FindItemTemplate(eventAwardInfo.TemplateID) != null)
          return eventAwardInfo;
      }
      return (EventAwardInfo) null;
    }

    public static List<EventAwardInfo> FindEventAward(eEventType DataId)
    {
      if (EventAwardMgr.m_EventAwards.ContainsKey((int) DataId))
        return EventAwardMgr.m_EventAwards[(int) DataId];
      return (List<EventAwardInfo>) null;
    }

    public static List<SqlDataProvider.Data.ItemInfo> GetAllEventAwardAward(
      eEventType DataId)
    {
      List<EventAwardInfo> eventAward = EventAwardMgr.FindEventAward(DataId);
      List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
      foreach (EventAwardInfo eventAwardInfo in eventAward)
      {
        SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(eventAwardInfo.TemplateID), eventAwardInfo.Count, 105);
        fromTemplate.IsBinds = eventAwardInfo.IsBinds;
        fromTemplate.ValidDate = eventAwardInfo.ValidDate;
        itemInfoList.Add(fromTemplate);
      }
      return itemInfoList;
    }

    public static List<EventAwardInfo> GetBoGuBoxAward(int type)
    {
      List<EventAwardInfo> eventAward = EventAwardMgr.FindEventAward(eEventType.BOGU_AVEDTURE);
      List<EventAwardInfo> eventAwardInfoList = new List<EventAwardInfo>();
      foreach (EventAwardInfo eventAwardInfo in eventAward)
      {
        if (eventAwardInfo.Random == type)
          eventAwardInfoList.Add(eventAwardInfo);
      }
      return eventAwardInfoList;
    }

    public static List<EventAwardInfo> GetDiceAward(eEventType DataId)
    {
      List<EventAwardInfo> eventAwardInfoList = new List<EventAwardInfo>();
      List<EventAwardInfo> eventAward = EventAwardMgr.FindEventAward(DataId);
      int num1 = 1;
      int maxRound = ThreadSafeRandom.NextStatic(eventAward.Select<EventAwardInfo, int>((Func<EventAwardInfo, int>) (s => s.Random)).Max());
      List<EventAwardInfo> list = eventAward.Where<EventAwardInfo>((Func<EventAwardInfo, bool>) (s => s.Random >= maxRound)).ToList<EventAwardInfo>();
      int num2 = list.Count<EventAwardInfo>();
      if (num2 > 0)
      {
        int count = num1 > num2 ? num2 : num1;
        foreach (int randomUnrepeat in EventAwardMgr.GetRandomUnrepeatArray(0, num2 - 1, count))
        {
          EventAwardInfo eventAwardInfo = list[randomUnrepeat];
          eventAwardInfoList.Add(eventAwardInfo);
        }
      }
      return eventAwardInfoList;
    }

    public static List<CardInfo> GetFightFootballTimeAward(eEventType DataId)
    {
      List<CardInfo> cardInfoList = new List<CardInfo>();
      List<EventAwardInfo> eventAwardInfoList = new List<EventAwardInfo>();
      List<EventAwardInfo> eventAward = EventAwardMgr.FindEventAward(DataId);
      int num1 = 1;
      int maxRound = ThreadSafeRandom.NextStatic(eventAward.Select<EventAwardInfo, int>((Func<EventAwardInfo, int>) (s => s.Random)).Max());
      List<EventAwardInfo> list = eventAward.Where<EventAwardInfo>((Func<EventAwardInfo, bool>) (s => s.Random >= maxRound)).ToList<EventAwardInfo>();
      int num2 = list.Count<EventAwardInfo>();
      if (num2 > 0)
      {
        int count = num1 > num2 ? num2 : num1;
        foreach (int randomUnrepeat in EventAwardMgr.GetRandomUnrepeatArray(0, num2 - 1, count))
        {
          EventAwardInfo eventAwardInfo = list[randomUnrepeat];
          eventAwardInfoList.Add(eventAwardInfo);
        }
      }
      foreach (EventAwardInfo eventAwardInfo in eventAwardInfoList)
      {
        CardInfo cardInfo = new CardInfo()
        {
          templateID = eventAwardInfo.TemplateID,
          count = eventAwardInfo.Count
        };
        cardInfoList.Add(cardInfo);
      }
      return cardInfoList;
    }

    public static int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
    {
      int[] numArray = new int[count];
      for (int index1 = 0; index1 < count; ++index1)
      {
        int num1 = EventAwardMgr.rand.Next(minValue, maxValue + 1);
        int num2 = 0;
        for (int index2 = 0; index2 < index1; ++index2)
        {
          if (numArray[index2] == num1)
            ++num2;
        }
        if (num2 == 0)
          numArray[index1] = num1;
        else
          --index1;
      }
      return numArray;
    }

    public static bool Init()
    {
      return EventAwardMgr.ReLoad();
    }

    public static EventAwardInfo[] LoadEventAwardDb()
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
        return produceBussiness.GetEventAwardInfos();
    }

    public static Dictionary<int, List<EventAwardInfo>> LoadEventAwards(
      EventAwardInfo[] EventAwards)
    {
      Dictionary<int, List<EventAwardInfo>> dictionary = new Dictionary<int, List<EventAwardInfo>>();
      for (int index = 0; index < EventAwards.Length; ++index)
      {
        EventAwardInfo info = EventAwards[index];
        if (!dictionary.Keys.Contains<int>(info.ActivityType))
        {
          IEnumerable<EventAwardInfo> source = ((IEnumerable<EventAwardInfo>) EventAwards).Where<EventAwardInfo>((Func<EventAwardInfo, bool>) (s => s.ActivityType == info.ActivityType));
          dictionary.Add(info.ActivityType, source.ToList<EventAwardInfo>());
        }
      }
      return dictionary;
    }

    public static bool ReLoad()
    {
      try
      {
        EventAwardInfo[] EventAwards = EventAwardMgr.LoadEventAwardDb();
        Dictionary<int, List<EventAwardInfo>> dictionary = EventAwardMgr.LoadEventAwards(EventAwards);
        if (EventAwards != null)
        {
          Interlocked.Exchange<EventAwardInfo[]>(ref EventAwardMgr.m_eventAward, EventAwards);
          Interlocked.Exchange<Dictionary<int, List<EventAwardInfo>>>(ref EventAwardMgr.m_EventAwards, dictionary);
        }
      }
      catch (Exception ex)
      {
        if (EventAwardMgr.log.IsErrorEnabled)
          EventAwardMgr.log.Error((object) nameof (ReLoad), ex);
        return false;
      }
      return true;
    }
  }
}
