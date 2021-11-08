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

		private static EventAwardInfo[] m_eventAward;

		private static Dictionary<int, List<EventAwardInfo>> m_EventAwards;

		private static ThreadSafeRandom rand = new ThreadSafeRandom();

		public static void CreateEventAward(eEventType DateId)
		{
		}

		public static EventAwardInfo CreateSearchGoodsAward(eEventType DataId)
		{
			List<EventAwardInfo> list = new List<EventAwardInfo>();
			List<EventAwardInfo> list2 = FindEventAward(DataId);
			int count2 = 1;
			int maxRound = ThreadSafeRandom.NextStatic(list2.Select((EventAwardInfo s) => s.Random).Max());
			List<EventAwardInfo> source = list2.Where((EventAwardInfo s) => s.Random >= maxRound).ToList();
			int num2 = source.Count();
			if (num2 > 0)
			{
				count2 = ((count2 > num2) ? num2 : count2);
				int[] randomUnrepeatArray = GetRandomUnrepeatArray(0, num2 - 1, count2);
				foreach (int num3 in randomUnrepeatArray)
				{
					EventAwardInfo item = source[num3];
					list.Add(item);
				}
			}
			foreach (EventAwardInfo info2 in list)
			{
				if (ItemMgr.FindItemTemplate(info2.TemplateID) != null)
				{
					return info2;
				}
			}
			return null;
		}

		public static List<EventAwardInfo> FindEventAward(eEventType DataId)
		{
			if (m_EventAwards.ContainsKey((int)DataId))
			{
				return m_EventAwards[(int)DataId];
			}
			return null;
		}

		public static List<ItemInfo> GetAllEventAwardAward(eEventType DataId)
		{
			List<EventAwardInfo> list3 = FindEventAward(DataId);
			List<ItemInfo> list2 = new List<ItemInfo>();
			foreach (EventAwardInfo info in list3)
			{
				ItemInfo item = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(info.TemplateID), info.Count, 105);
				item.IsBinds = info.IsBinds;
				item.ValidDate = info.ValidDate;
				list2.Add(item);
			}
			return list2;
		}

		public static List<EventAwardInfo> GetBoGuBoxAward(int type)
		{
			List<EventAwardInfo> list3 = FindEventAward(eEventType.BOGU_AVEDTURE);
			List<EventAwardInfo> list2 = new List<EventAwardInfo>();
			foreach (EventAwardInfo info in list3)
			{
				if (info.Random == type)
				{
					list2.Add(info);
				}
			}
			return list2;
		}

		public static List<EventAwardInfo> GetDiceAward(eEventType DataId)
		{
			List<EventAwardInfo> list = new List<EventAwardInfo>();
			List<EventAwardInfo> list2 = FindEventAward(DataId);
			int count2 = 1;
			int maxRound = ThreadSafeRandom.NextStatic(list2.Select((EventAwardInfo s) => s.Random).Max());
			List<EventAwardInfo> source = list2.Where((EventAwardInfo s) => s.Random >= maxRound).ToList();
			int num2 = source.Count();
			if (num2 > 0)
			{
				count2 = ((count2 > num2) ? num2 : count2);
				int[] randomUnrepeatArray = GetRandomUnrepeatArray(0, num2 - 1, count2);
				foreach (int num3 in randomUnrepeatArray)
				{
					EventAwardInfo item = source[num3];
					list.Add(item);
				}
			}
			return list;
		}

		public static List<CardInfo> GetFightFootballTimeAward(eEventType DataId)
		{
			List<CardInfo> list = new List<CardInfo>();
			List<EventAwardInfo> list2 = new List<EventAwardInfo>();
			List<EventAwardInfo> list3 = FindEventAward(DataId);
			int count2 = 1;
			int maxRound = ThreadSafeRandom.NextStatic(list3.Select((EventAwardInfo s) => s.Random).Max());
			List<EventAwardInfo> source = list3.Where((EventAwardInfo s) => s.Random >= maxRound).ToList();
			int num2 = source.Count();
			if (num2 > 0)
			{
				count2 = ((count2 > num2) ? num2 : count2);
				int[] randomUnrepeatArray = GetRandomUnrepeatArray(0, num2 - 1, count2);
				foreach (int num3 in randomUnrepeatArray)
				{
					EventAwardInfo item = source[num3];
					list2.Add(item);
				}
			}
			foreach (EventAwardInfo info2 in list2)
			{
				CardInfo info3 = new CardInfo
				{
					templateID = info2.TemplateID,
					count = info2.Count
				};
				list.Add(info3);
			}
			return list;
		}

		public static int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
		{
			int[] numArray = new int[count];
			for (int i = 0; i < count; i++)
			{
				int num2 = rand.Next(minValue, maxValue + 1);
				int num3 = 0;
				for (int j = 0; j < i; j++)
				{
					if (numArray[j] == num2)
					{
						num3++;
					}
				}
				if (num3 == 0)
				{
					numArray[i] = num2;
				}
				else
				{
					i--;
				}
			}
			return numArray;
		}

		public static bool Init()
		{
			return ReLoad();
		}

		public static EventAwardInfo[] LoadEventAwardDb()
		{
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				return bussiness.GetEventAwardInfos();
			}
		}

		public static Dictionary<int, List<EventAwardInfo>> LoadEventAwards(EventAwardInfo[] EventAwards)
		{
			Dictionary<int, List<EventAwardInfo>> dictionary = new Dictionary<int, List<EventAwardInfo>>();
			foreach (EventAwardInfo info in EventAwards)
			{
				if (!dictionary.Keys.Contains(info.ActivityType))
				{
					IEnumerable<EventAwardInfo> source = EventAwards.Where((EventAwardInfo s) => s.ActivityType == info.ActivityType);
					dictionary.Add(info.ActivityType, source.ToList());
				}
			}
			return dictionary;
		}

		public static bool ReLoad()
		{
			try
			{
				EventAwardInfo[] eventAwards = LoadEventAwardDb();
				Dictionary<int, List<EventAwardInfo>> dictionary = LoadEventAwards(eventAwards);
				if (eventAwards != null)
				{
					Interlocked.Exchange(ref m_eventAward, eventAwards);
					Interlocked.Exchange(ref m_EventAwards, dictionary);
				}
			}
			catch (Exception exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("ReLoad", exception);
				}
				return false;
			}
			return true;
		}
	}
}
