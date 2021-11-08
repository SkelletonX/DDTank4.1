// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.QuestMgr
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
  public class QuestMgr
  {
    private static Dictionary<int, AchievementInfo> dictionary_3 = new Dictionary<int, AchievementInfo>();
    private static Dictionary<int, List<AchievementConditionInfo>> dictionary_4 = new Dictionary<int, List<AchievementConditionInfo>>();
    private static Dictionary<int, List<AchievementGoodsInfo>> dictionary_5 = new Dictionary<int, List<AchievementGoodsInfo>>();
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<int, List<QuestConditionInfo>> m_questcondiction = new Dictionary<int, List<QuestConditionInfo>>();
    private static Dictionary<int, List<QuestAwardInfo>> m_questgoods = new Dictionary<int, List<QuestAwardInfo>>();
    private static Dictionary<int, QuestInfo> m_questinfo = new Dictionary<int, QuestInfo>();

    public static List<AchievementConditionInfo> GetAchievementCondiction(
      AchievementInfo info)
    {
      if (QuestMgr.dictionary_4.ContainsKey(info.ID))
        return QuestMgr.dictionary_4[info.ID];
      return (List<AchievementConditionInfo>) null;
    }

    public static List<AchievementGoodsInfo> GetAchievementGoods(
      AchievementInfo info)
    {
      if (QuestMgr.dictionary_5.ContainsKey(info.ID))
        return QuestMgr.dictionary_5[info.ID];
      return (List<AchievementGoodsInfo>) null;
    }

    public static List<AchievementInfo> GetAllAchievements()
    {
      return QuestMgr.dictionary_3.Values.ToList<AchievementInfo>();
    }

    public static int[] GetAllBuriedQuest()
    {
      List<int> intList = new List<int>();
      foreach (QuestInfo questInfo in QuestMgr.m_questinfo.Values)
      {
        if (questInfo.QuestID == 10)
          intList.Add(questInfo.ID);
      }
      return intList.ToArray();
    }

    public static List<QuestConditionInfo> GetQuestCondiction(QuestInfo info)
    {
      if (QuestMgr.m_questcondiction.ContainsKey(info.ID))
        return QuestMgr.m_questcondiction[info.ID];
      return (List<QuestConditionInfo>) null;
    }

    public static List<QuestAwardInfo> GetQuestGoods(QuestInfo info)
    {
      if (QuestMgr.m_questgoods.ContainsKey(info.ID))
        return QuestMgr.m_questgoods[info.ID];
      return (List<QuestAwardInfo>) null;
    }

    public static AchievementInfo GetSingleAchievement(int id)
    {
      if (QuestMgr.dictionary_3.ContainsKey(id))
        return QuestMgr.dictionary_3[id];
      return (AchievementInfo) null;
    }

    public static QuestInfo GetSingleQuest(int id)
    {
      if (QuestMgr.m_questinfo.ContainsKey(id))
        return QuestMgr.m_questinfo[id];
      return (QuestInfo) null;
    }

    public static bool Init()
    {
      return QuestMgr.ReLoad();
    }

    public static Dictionary<int, List<AchievementConditionInfo>> LoadAchievementCondictionDb(
      Dictionary<int, AchievementInfo> achs)
    {
      Dictionary<int, List<AchievementConditionInfo>> dictionary = new Dictionary<int, List<AchievementConditionInfo>>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        AchievementConditionInfo[] achievementCondition = produceBussiness.GetALlAchievementCondition();
        using (Dictionary<int, AchievementInfo>.ValueCollection.Enumerator enumerator = achs.Values.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AchievementInfo ach = enumerator.Current;
            IEnumerable<AchievementConditionInfo> source = ((IEnumerable<AchievementConditionInfo>) achievementCondition).Where<AchievementConditionInfo>((Func<AchievementConditionInfo, bool>) (s => s.AchievementID == ach.ID));
            dictionary.Add(ach.ID, source.ToList<AchievementConditionInfo>());
          }
          return dictionary;
        }
      }
    }

    public static Dictionary<int, List<AchievementGoodsInfo>> LoadAchievementGoodDb(
      Dictionary<int, AchievementInfo> achs)
    {
      Dictionary<int, List<AchievementGoodsInfo>> dictionary = new Dictionary<int, List<AchievementGoodsInfo>>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        AchievementGoodsInfo[] achievementGoods = produceBussiness.GetAllAchievementGoods();
        using (Dictionary<int, AchievementInfo>.ValueCollection.Enumerator enumerator = achs.Values.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AchievementInfo ach = enumerator.Current;
            IEnumerable<AchievementGoodsInfo> source = ((IEnumerable<AchievementGoodsInfo>) achievementGoods).Where<AchievementGoodsInfo>((Func<AchievementGoodsInfo, bool>) (s => s.AchievementID == ach.ID));
            dictionary.Add(ach.ID, source.ToList<AchievementGoodsInfo>());
          }
          return dictionary;
        }
      }
    }

    public static Dictionary<int, AchievementInfo> LoadAchievementInfoDb()
    {
      Dictionary<int, AchievementInfo> dictionary = new Dictionary<int, AchievementInfo>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (AchievementInfo achievementInfo in produceBussiness.GetAllAchievement())
        {
          if (!dictionary.ContainsKey(achievementInfo.ID))
            dictionary.Add(achievementInfo.ID, achievementInfo);
        }
      }
      return dictionary;
    }

    public static Dictionary<int, List<QuestConditionInfo>> LoadQuestCondictionDb(
      Dictionary<int, QuestInfo> quests)
    {
      Dictionary<int, List<QuestConditionInfo>> dictionary = new Dictionary<int, List<QuestConditionInfo>>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        QuestConditionInfo[] allQuestCondiction = produceBussiness.GetAllQuestCondiction();
        foreach (QuestInfo questInfo in quests.Values)
        {
          QuestInfo quest = questInfo;
          IEnumerable<QuestConditionInfo> source = ((IEnumerable<QuestConditionInfo>) allQuestCondiction).Where<QuestConditionInfo>((Func<QuestConditionInfo, bool>) (s => s.QuestID == quest.ID));
          dictionary.Add(quest.ID, source.ToList<QuestConditionInfo>());
        }
      }
      return dictionary;
    }

    public static Dictionary<int, List<QuestAwardInfo>> LoadQuestGoodDb(
      Dictionary<int, QuestInfo> quests)
    {
      Dictionary<int, List<QuestAwardInfo>> dictionary = new Dictionary<int, List<QuestAwardInfo>>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        QuestAwardInfo[] allQuestGoods = produceBussiness.GetAllQuestGoods();
        foreach (QuestInfo questInfo in quests.Values)
        {
          QuestInfo quest = questInfo;
          IEnumerable<QuestAwardInfo> source = ((IEnumerable<QuestAwardInfo>) allQuestGoods).Where<QuestAwardInfo>((Func<QuestAwardInfo, bool>) (s => s.QuestID == quest.ID));
          dictionary.Add(quest.ID, source.ToList<QuestAwardInfo>());
        }
      }
      return dictionary;
    }

    public static Dictionary<int, QuestInfo> LoadQuestInfoDb()
    {
      Dictionary<int, QuestInfo> dictionary = new Dictionary<int, QuestInfo>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (QuestInfo questInfo in produceBussiness.GetALlQuest())
        {
          if (!dictionary.ContainsKey(questInfo.ID))
            dictionary.Add(questInfo.ID, questInfo);
        }
      }
      return dictionary;
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, QuestInfo> quests = QuestMgr.LoadQuestInfoDb();
        Dictionary<int, List<QuestConditionInfo>> dictionary1 = QuestMgr.LoadQuestCondictionDb(quests);
        Dictionary<int, List<QuestAwardInfo>> dictionary2 = QuestMgr.LoadQuestGoodDb(quests);
        Dictionary<int, AchievementInfo> achs = QuestMgr.LoadAchievementInfoDb();
        Dictionary<int, List<AchievementConditionInfo>> dictionary3 = QuestMgr.LoadAchievementCondictionDb(achs);
        Dictionary<int, List<AchievementGoodsInfo>> dictionary4 = QuestMgr.LoadAchievementGoodDb(achs);
        if (quests.Count > 0)
        {
          Interlocked.Exchange<Dictionary<int, QuestInfo>>(ref QuestMgr.m_questinfo, quests);
          Interlocked.Exchange<Dictionary<int, List<QuestConditionInfo>>>(ref QuestMgr.m_questcondiction, dictionary1);
          Interlocked.Exchange<Dictionary<int, List<QuestAwardInfo>>>(ref QuestMgr.m_questgoods, dictionary2);
        }
        if (achs.Count > 0)
        {
          Interlocked.Exchange<Dictionary<int, List<AchievementConditionInfo>>>(ref QuestMgr.dictionary_4, dictionary3);
          Interlocked.Exchange<Dictionary<int, AchievementInfo>>(ref QuestMgr.dictionary_3, achs);
          Interlocked.Exchange<Dictionary<int, List<AchievementGoodsInfo>>>(ref QuestMgr.dictionary_5, dictionary4);
        }
        return true;
      }
      catch (Exception ex)
      {
        QuestMgr.log.Error((object) nameof (QuestMgr), ex);
      }
      return false;
    }
  }
}
