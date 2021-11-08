using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
	public class ActiveMgr
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static Dictionary<int, ActiveAwardInfo> m_ActiveAwardInfo = new Dictionary<int, ActiveAwardInfo>();

		public static Dictionary<int, List<ActiveConditionInfo>> m_ActiveConditionInfo = new Dictionary<int, List<ActiveConditionInfo>>();

		public static List<ActiveAwardInfo> GetAwardInfo(DateTime lastDate, int playerGrade)
		{
			string awardId = null;
			int days = (DateTime.Now - lastDate).Days;
			if (DateTime.Now.DayOfYear > lastDate.DayOfYear)
			{
				days++;
			}
			List<ActiveAwardInfo> list = new List<ActiveAwardInfo>();
			foreach (List<ActiveConditionInfo> value in m_ActiveConditionInfo.Values)
			{
				foreach (ActiveConditionInfo info in value)
				{
					if (IsValid(info) && IsInGrade(info.LimitGrade, playerGrade) && info.Condition <= days)
					{
						awardId = info.AwardId;
						_ = info.ActiveID;
					}
				}
			}
			if (!string.IsNullOrEmpty(awardId))
			{
				string[] array = awardId.Split(',');
				foreach (string str2 in array)
				{
					if (!string.IsNullOrEmpty(str2) && m_ActiveAwardInfo.ContainsKey(Convert.ToInt32(str2)))
					{
						list.Add(m_ActiveAwardInfo[Convert.ToInt32(str2)]);
					}
				}
			}
			return list;
		}

		public static bool Init()
		{
			return ReLoad();
		}

		private static bool IsInGrade(string limitGrade, int playerGrade)
		{
			bool flag = false;
			int num = 0;
			int num2 = 0;
			if (limitGrade != null)
			{
				string[] strArray = limitGrade.Split('-');
				if (strArray.Length == 2)
				{
					num = Convert.ToInt32(strArray[0]);
					num2 = Convert.ToInt32(strArray[1]);
				}
				if (num <= playerGrade && num2 >= playerGrade)
				{
					flag = true;
				}
			}
			return flag;
		}

		public static bool IsValid(ActiveConditionInfo info)
		{
			_ = info.StartTime;
			_ = info.EndTime;
			if (info.StartTime.Ticks <= DateTime.Now.Ticks)
			{
				return info.EndTime.Ticks >= DateTime.Now.Ticks;
			}
			return false;
		}

		public static Dictionary<int, ActiveAwardInfo> LoadActiveAwardDb(Dictionary<int, List<ActiveConditionInfo>> conditions)
		{
			Dictionary<int, ActiveAwardInfo> dictionary = new Dictionary<int, ActiveAwardInfo>();
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				ActiveAwardInfo[] allActiveAwardInfo = bussiness.GetAllActiveAwardInfo();
				foreach (int num in conditions.Keys)
				{
					ActiveAwardInfo[] array = allActiveAwardInfo;
					foreach (ActiveAwardInfo info in array)
					{
						if (num == info.ActiveID && !dictionary.ContainsKey(info.ID))
						{
							dictionary.Add(info.ID, info);
						}
					}
				}
				return dictionary;
			}
		}

		public static Dictionary<int, List<ActiveConditionInfo>> LoadActiveConditionDb()
		{
			Dictionary<int, List<ActiveConditionInfo>> dictionary = new Dictionary<int, List<ActiveConditionInfo>>();
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				ActiveConditionInfo[] allActiveConditionInfo = bussiness.GetAllActiveConditionInfo();
				foreach (ActiveConditionInfo info in allActiveConditionInfo)
				{
					List<ActiveConditionInfo> list = new List<ActiveConditionInfo>();
					if (!dictionary.ContainsKey(info.ActiveID))
					{
						list.Add(info);
						dictionary.Add(info.ActiveID, list);
					}
					else
					{
						dictionary[info.ActiveID].Add(info);
					}
				}
				return dictionary;
			}
		}

		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, List<ActiveConditionInfo>> conditions = LoadActiveConditionDb();
				Dictionary<int, ActiveAwardInfo> dictionary2 = LoadActiveAwardDb(conditions);
				if (conditions.Count > 0)
				{
					Interlocked.Exchange(ref m_ActiveConditionInfo, conditions);
					Interlocked.Exchange(ref m_ActiveAwardInfo, dictionary2);
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
