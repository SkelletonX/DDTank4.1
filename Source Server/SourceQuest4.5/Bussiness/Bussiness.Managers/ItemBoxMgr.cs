using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
	public class ItemBoxMgr
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static ItemBoxInfo[] m_itemBox;

		private static Dictionary<int, List<ItemBoxInfo>> m_itemBoxs;

		private static ThreadSafeRandom random = new ThreadSafeRandom();

		public static bool ReLoad()
		{
			try
			{
				ItemBoxInfo[] tempItemBox = LoadItemBoxDb();
				Dictionary<int, List<ItemBoxInfo>> tempItemBoxs = LoadItemBoxs(tempItemBox);
				if (tempItemBox != null)
				{
					Interlocked.Exchange(ref m_itemBox, tempItemBox);
					Interlocked.Exchange(ref m_itemBoxs, tempItemBoxs);
				}
			}
			catch (Exception e)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("ReLoad", e);
				}
				return false;
			}
			return true;
		}

		public static bool Init()
		{
			return ReLoad();
		}

		public static ItemBoxInfo[] LoadItemBoxDb()
		{
			using (ProduceBussiness db = new ProduceBussiness())
			{
				return db.GetItemBoxInfos();
			}
		}

		public static Dictionary<int, List<ItemBoxInfo>> LoadItemBoxs(ItemBoxInfo[] itemBoxs)
		{
			Dictionary<int, List<ItemBoxInfo>> infos = new Dictionary<int, List<ItemBoxInfo>>();
			foreach (ItemBoxInfo info in itemBoxs)
			{
				if (!infos.Keys.Contains(info.ID))
				{
					IEnumerable<ItemBoxInfo> temp = itemBoxs.Where((ItemBoxInfo s) => s.ID == info.ID);
					infos.Add(info.ID, temp.ToList());
				}
			}
			return infos;
		}

		public static List<ItemBoxInfo> FindItemBox(int DataId)
		{
			if (m_itemBoxs.ContainsKey(DataId))
			{
				return m_itemBoxs[DataId];
			}
			return null;
		}

		public static List<ItemInfo> GetAllItemBoxAward(int DataId)
		{
			List<ItemBoxInfo> list = FindItemBox(DataId);
			List<ItemInfo> infos = new List<ItemInfo>();
			foreach (ItemBoxInfo info in list)
			{
				ItemInfo item = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(info.TemplateId), info.ItemCount, 105);
				item.IsBinds = info.IsBind;
				item.ValidDate = info.ItemValid;
				infos.Add(item);
			}
			return infos;
		}

		public static ItemBoxInfo FindSpecialItemBox(int DataId)
		{
			ItemBoxInfo item = new ItemBoxInfo();
			switch (DataId)
			{
			case -300:
				item.TemplateId = 11420;
				item.ItemCount = 1;
				break;
			case -1100:
				item.TemplateId = 11213;
				item.ItemCount = 1;
				break;
			case 11408:
				item.TemplateId = 11420;
				item.ItemCount = 1;
				break;
			case -100:
				item.TemplateId = 11233;
				item.ItemCount = 1;
				break;
			case -200:
				item.TemplateId = 112244;
				item.ItemCount = 1;
				break;
			}
			return item;
		}

		public static bool CreateItemBox(int DateId, List<ItemInfo> itemInfos, SpecialItemDataInfo specialInfo)
		{
			return CreateItemBox(DateId, null, itemInfos, specialInfo);
		}

		public static bool CreateItemBox(int DateId, List<ItemBoxInfo> tempBox, List<ItemInfo> itemInfos, SpecialItemDataInfo specialInfo)
		{
			new List<ItemBoxInfo>();
			List<ItemBoxInfo> source = FindItemBox(DateId);
			if (tempBox != null && tempBox.Count > 0)
			{
				source = tempBox;
			}
			if (source == null)
			{
				return false;
			}
			List<ItemBoxInfo> itemBoxInfoList2 = source.Where((ItemBoxInfo s) => s.IsSelect).ToList();
			int num1 = 1;
			int maxRound = 0;
			if (itemBoxInfoList2.Count < source.Count)
			{
				maxRound = ThreadSafeRandom.NextStatic((from s in source
					where !s.IsSelect
					select s.Random).Max());
				if (maxRound <= 0)
				{
					log.Error("ItemBoxMgr Random Error: " + maxRound + " | " + DateId);
					maxRound = (from s in source
						where !s.IsSelect
						select s.Random).Max();
				}
			}
			List<ItemBoxInfo> list = source.Where((ItemBoxInfo s) => !s.IsSelect && s.Random >= maxRound).ToList();
			int num2 = list.Count();
			if (num2 > 0)
			{
				int count = (num1 > num2) ? num2 : num1;
				int[] randomUnrepeatArray = GetRandomUnrepeatArray(0, num2 - 1, count);
				foreach (int randomUnrepeat in randomUnrepeatArray)
				{
					ItemBoxInfo itemBoxInfo2 = list[randomUnrepeat];
					if (itemBoxInfoList2 == null)
					{
						itemBoxInfoList2 = new List<ItemBoxInfo>();
					}
					itemBoxInfoList2.Add(itemBoxInfo2);
				}
			}
			foreach (ItemBoxInfo itemBoxInfo in itemBoxInfoList2)
			{
				if (itemBoxInfo == null)
				{
					return false;
				}
				switch (itemBoxInfo.TemplateId)
				{
				case -300:
					specialInfo.GiftToken += itemBoxInfo.ItemCount;
					break;
				case -200:
					specialInfo.Money += itemBoxInfo.ItemCount;
					break;
				case -100:
					specialInfo.Gold += itemBoxInfo.ItemCount;
					break;
				case 11107:
					specialInfo.GP += itemBoxInfo.ItemCount;
					break;
				default:
				{
					ItemInfo fromTemplate = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(itemBoxInfo.TemplateId), itemBoxInfo.ItemCount, 101);
					if (fromTemplate != null)
					{
						fromTemplate.IsBinds = itemBoxInfo.IsBind;
						fromTemplate.ValidDate = itemBoxInfo.ItemValid;
						fromTemplate.StrengthenLevel = itemBoxInfo.StrengthenLevel;
						fromTemplate.AttackCompose = itemBoxInfo.AttackCompose;
						fromTemplate.DefendCompose = itemBoxInfo.DefendCompose;
						fromTemplate.AgilityCompose = itemBoxInfo.AgilityCompose;
						fromTemplate.LuckCompose = itemBoxInfo.LuckCompose;
						if (itemInfos == null)
						{
							itemInfos = new List<ItemInfo>();
						}
						itemInfos.Add(fromTemplate);
					}
					break;
				}
				}
			}
			return true;
		}

		public static bool CreateItemBox(int DateId, List<ItemInfo> itemInfos, ref int gold, ref int point, ref int giftToken, ref int medal, ref int exp)
		{
			List<ItemBoxInfo> FiltInfos2 = new List<ItemBoxInfo>();
			List<ItemBoxInfo> unFiltInfos = FindItemBox(DateId);
			if (unFiltInfos == null)
			{
				return false;
			}
			FiltInfos2 = unFiltInfos.Where((ItemBoxInfo s) => s.IsSelect).ToList();
			int dropItemCount2 = 1;
			int maxRound = 0;
			if (FiltInfos2.Count < unFiltInfos.Count)
			{
				maxRound = ThreadSafeRandom.NextStatic((from s in unFiltInfos
					where !s.IsSelect
					select s.Random).Max());
			}
			List<ItemBoxInfo> RoundInfos = unFiltInfos.Where((ItemBoxInfo s) => !s.IsSelect && s.Random >= maxRound).ToList();
			int maxItems = RoundInfos.Count();
			if (maxItems > 0)
			{
				dropItemCount2 = ((dropItemCount2 > maxItems) ? maxItems : dropItemCount2);
				int[] array = GetRandomUnrepeatArray(0, maxItems - 1, dropItemCount2);
				foreach (int i in array)
				{
					ItemBoxInfo item = RoundInfos[i];
					if (FiltInfos2 == null)
					{
						FiltInfos2 = new List<ItemBoxInfo>();
					}
					FiltInfos2.Add(item);
				}
			}
			foreach (ItemBoxInfo info in FiltInfos2)
			{
				if (info == null)
				{
					return false;
				}
				switch (info.TemplateId)
				{
				case -1100:
					giftToken += info.ItemCount;
					break;
				case -300:
					medal += info.ItemCount;
					break;
				case -200:
					point += info.ItemCount;
					break;
				case -100:
					gold += info.ItemCount;
					break;
				case 11107:
					exp += info.ItemCount;
					break;
				default:
				{
					ItemInfo item2 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(info.TemplateId), info.ItemCount, 101);
					if (item2 != null)
					{
						item2.Count = info.ItemCount;
						item2.IsBinds = info.IsBind;
						item2.ValidDate = info.ItemValid;
						item2.StrengthenLevel = info.StrengthenLevel;
						item2.AttackCompose = info.AttackCompose;
						item2.DefendCompose = info.DefendCompose;
						item2.AgilityCompose = info.AgilityCompose;
						item2.LuckCompose = info.LuckCompose;
						item2.IsTips = (info.IsTips != 0);
						item2.IsLogs = info.IsLogs;
						if (itemInfos == null)
						{
							itemInfos = new List<ItemInfo>();
						}
						itemInfos.Add(item2);
					}
					break;
				}
				}
			}
			return true;
		}

		public static int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
		{
			int[] resultRound = new int[count];
			for (int i = 0; i < count; i++)
			{
				int j = random.Next(minValue, maxValue + 1);
				int num = 0;
				for (int k = 0; k < i; k++)
				{
					if (resultRound[k] == j)
					{
						num++;
					}
				}
				if (num == 0)
				{
					resultRound[i] = j;
				}
				else
				{
					i--;
				}
			}
			return resultRound;
		}

		public static bool CreateItemBox(int DateId, List<ItemInfo> itemInfos, ref int gold, ref int point, ref int giftToken, ref int exp)
		{
			return CreateItemBox(DateId, null, itemInfos, ref gold, ref point, ref giftToken, ref exp);
		}

		public static bool CreateItemBox(int DateId, List<ItemBoxInfo> tempBox, List<ItemInfo> itemInfos, ref int gold, ref int point, ref int giftToken, ref int exp)
		{
			List<ItemBoxInfo> list2 = new List<ItemBoxInfo>();
			List<ItemBoxInfo> list3 = FindItemBox(DateId);
			if (tempBox != null && tempBox.Count > 0)
			{
				list3 = tempBox;
			}
			if (list3 == null)
			{
				return false;
			}
			list2 = list3.Where((ItemBoxInfo s) => s.IsSelect).ToList();
			int count2 = 1;
			int maxRound = 0;
			if (list2.Count < list3.Count)
			{
				maxRound = ThreadSafeRandom.NextStatic((from s in list3
					where !s.IsSelect
					select s.Random).Max());
			}
			List<ItemBoxInfo> source = list3.Where((ItemBoxInfo s) => !s.IsSelect && s.Random >= maxRound).ToList();
			int num2 = source.Count();
			if (num2 > 0)
			{
				count2 = ((count2 > num2) ? num2 : count2);
				int[] numArray = GetRandomUnrepeatArray(0, num2 - 1, count2);
				foreach (int num3 in numArray)
				{
					ItemBoxInfo item = source[num3];
					if (list2 == null)
					{
						list2 = new List<ItemBoxInfo>();
					}
					list2.Add(item);
				}
			}
			foreach (ItemBoxInfo info2 in list2)
			{
				if (info2 == null)
				{
					return false;
				}
				switch (info2.TemplateId)
				{
				case -200:
					point += info2.ItemCount;
					break;
				case -300:
					giftToken += info2.ItemCount;
					break;
				case -100:
					gold += info2.ItemCount;
					break;
				case 11107:
					exp += info2.ItemCount;
					break;
				default:
				{
					ItemInfo info3 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(info2.TemplateId), info2.ItemCount, 101);
					if (info3 != null)
					{
						info3.IsBinds = info2.IsBind;
						info3.ValidDate = info2.ItemValid;
						info3.StrengthenLevel = info2.StrengthenLevel;
						info3.AttackCompose = info2.AttackCompose;
						info3.DefendCompose = info2.DefendCompose;
						info3.AgilityCompose = info2.AgilityCompose;
						info3.LuckCompose = info2.LuckCompose;
						if (itemInfos == null)
						{
							itemInfos = new List<ItemInfo>();
						}
						itemInfos.Add(info3);
					}
					break;
				}
				}
			}
			return true;
		}

		public static List<ItemBoxInfo> FindLotteryItemBoxByRand(int DateId, int countSelect)
		{
			List<ItemBoxInfo> list = FindLotteryItemBox(DateId);
			List<ItemBoxInfo> list2 = new List<ItemBoxInfo>();
			for (int i = 0; i < countSelect; i++)
			{
				int num2 = ThreadSafeRandom.NextStatic(0, list.Count);
				if (num2 < list.Count)
				{
					list2.Add(list[num2]);
					list.Remove(list[num2]);
				}
			}
			return list2;
		}

		public static List<ItemBoxInfo> FindLotteryItemBox(int DataId)
		{
			if (!m_itemBoxs.ContainsKey(DataId))
			{
				return null;
			}
			List<ItemBoxInfo> list = new List<ItemBoxInfo>();
			foreach (ItemBoxInfo current in m_itemBoxs[DataId])
			{
				bool flag = true;
				foreach (ItemBoxInfo info2 in list)
				{
					if (info2.TemplateId == current.TemplateId && info2.ItemCount == current.ItemCount)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(current);
				}
			}
			return list;
		}
	}
}
