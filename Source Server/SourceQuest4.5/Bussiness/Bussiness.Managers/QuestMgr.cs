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

		public static List<AchievementConditionInfo> GetAchievementCondiction(AchievementInfo info)
		{
			if (dictionary_4.ContainsKey(info.ID))
			{
				return dictionary_4[info.ID];
			}
			return null;
		}

		public static List<AchievementGoodsInfo> GetAchievementGoods(AchievementInfo info)
		{
			if (dictionary_5.ContainsKey(info.ID))
			{
				return dictionary_5[info.ID];
			}
			return null;
		}

		public static List<AchievementInfo> GetAllAchievements()
		{
			return dictionary_3.Values.ToList();
		}

		public static int[] GetAllBuriedQuest()
		{
			List<int> list = new List<int>();
			foreach (QuestInfo info in m_questinfo.Values)
			{
				if (info.QuestID == 10)
				{
					list.Add(info.ID);
				}
			}
			return list.ToArray();
		}

		public static List<QuestConditionInfo> GetQuestCondiction(QuestInfo info)
		{
			if (m_questcondiction.ContainsKey(info.ID))
			{
				return m_questcondiction[info.ID];
			}
			return null;
		}

		public static List<QuestAwardInfo> GetQuestGoods(QuestInfo info)
		{
			if (m_questgoods.ContainsKey(info.ID))
			{
				return m_questgoods[info.ID];
			}
			return null;
		}

		public static AchievementInfo GetSingleAchievement(int id)
		{
			if (dictionary_3.ContainsKey(id))
			{
				return dictionary_3[id];
			}
			return null;
		}

		public static QuestInfo GetSingleQuest(int id)
		{
			if (m_questinfo.ContainsKey(id))
			{
				return m_questinfo[id];
			}
			return null;
		}

		public static bool Init()
		{
			return ReLoad();
		}

		public static Dictionary<int, List<AchievementConditionInfo>> LoadAchievementCondictionDb(Dictionary<int, AchievementInfo> achs)
		{
			Dictionary<int, List<AchievementConditionInfo>> dictionary = new Dictionary<int, List<AchievementConditionInfo>>();
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				AchievementConditionInfo[] allAchievementCondiction = bussiness.GetALlAchievementCondition();
				foreach (AchievementInfo ach in achs.Values)
				{
					IEnumerable<AchievementConditionInfo> source = allAchievementCondiction.Where((AchievementConditionInfo s) => s.AchievementID == ach.ID);
					dictionary.Add(ach.ID, source.ToList());
				}
				return dictionary;
			}
		}

		public static Dictionary<int, List<AchievementGoodsInfo>> LoadAchievementGoodDb(Dictionary<int, AchievementInfo> achs)
		{
			Dictionary<int, List<AchievementGoodsInfo>> dictionary = new Dictionary<int, List<AchievementGoodsInfo>>();
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				AchievementGoodsInfo[] allAchievementGoods = bussiness.GetAllAchievementGoods();
				foreach (AchievementInfo ach in achs.Values)
				{
					IEnumerable<AchievementGoodsInfo> source = allAchievementGoods.Where((AchievementGoodsInfo s) => s.AchievementID == ach.ID);
					dictionary.Add(ach.ID, source.ToList());
				}
				return dictionary;
			}
		}

		public static Dictionary<int, AchievementInfo> LoadAchievementInfoDb()
		{
			Dictionary<int, AchievementInfo> dictionary = new Dictionary<int, AchievementInfo>();
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				AchievementInfo[] allAchievement = bussiness.GetAllAchievement();
				foreach (AchievementInfo info in allAchievement)
				{
					if (!dictionary.ContainsKey(info.ID))
					{
						dictionary.Add(info.ID, info);
					}
				}
				return dictionary;
			}
		}

		public static Dictionary<int, List<QuestConditionInfo>> LoadQuestCondictionDb(Dictionary<int, QuestInfo> quests)
		{
			Dictionary<int, List<QuestConditionInfo>> dictionary = new Dictionary<int, List<QuestConditionInfo>>();
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				QuestConditionInfo[] allQuestCondiction = bussiness.GetAllQuestCondiction();
				foreach (QuestInfo quest in quests.Values)
				{
					IEnumerable<QuestConditionInfo> source = allQuestCondiction.Where((QuestConditionInfo s) => s.QuestID == quest.ID);
					dictionary.Add(quest.ID, source.ToList());
				}
				return dictionary;
			}
		}

		public static Dictionary<int, List<QuestAwardInfo>> LoadQuestGoodDb(Dictionary<int, QuestInfo> quests)
		{
			Dictionary<int, List<QuestAwardInfo>> dictionary = new Dictionary<int, List<QuestAwardInfo>>();
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				QuestAwardInfo[] allQuestGoods = bussiness.GetAllQuestGoods();
				foreach (QuestInfo quest in quests.Values)
				{
					IEnumerable<QuestAwardInfo> source = allQuestGoods.Where((QuestAwardInfo s) => s.QuestID == quest.ID);
					dictionary.Add(quest.ID, source.ToList());
				}
				return dictionary;
			}
		}

		public static Dictionary<int, QuestInfo> LoadQuestInfoDb()
		{
			Dictionary<int, QuestInfo> dictionary = new Dictionary<int, QuestInfo>();
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				QuestInfo[] aLlQuest = bussiness.GetALlQuest();
				foreach (QuestInfo info in aLlQuest)
				{
					if (!dictionary.ContainsKey(info.ID))
					{
						dictionary.Add(info.ID, info);
					}
				}
				return dictionary;
			}
		}

		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, QuestInfo> quests = LoadQuestInfoDb();
				Dictionary<int, List<QuestConditionInfo>> dictionary2 = LoadQuestCondictionDb(quests);
				Dictionary<int, List<QuestAwardInfo>> dictionary3 = LoadQuestGoodDb(quests);
				Dictionary<int, AchievementInfo> achs = LoadAchievementInfoDb();
				Dictionary<int, List<AchievementConditionInfo>> dictionary4 = LoadAchievementCondictionDb(achs);
				Dictionary<int, List<AchievementGoodsInfo>> dictionary5 = LoadAchievementGoodDb(achs);
				if (quests.Count > 0)
				{
					Interlocked.Exchange(ref m_questinfo, quests);
					Interlocked.Exchange(ref m_questcondiction, dictionary2);
					Interlocked.Exchange(ref m_questgoods, dictionary3);
				}
				if (achs.Count > 0)
				{
					Interlocked.Exchange(ref dictionary_4, dictionary4);
					Interlocked.Exchange(ref dictionary_3, achs);
					Interlocked.Exchange(ref dictionary_5, dictionary5);
				}
				return true;
			}
			catch (Exception exception)
			{
				log.Error("QuestMgr", exception);
			}
			return false;
		}
	}
}
