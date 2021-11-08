using Bussiness;
using Bussiness.Managers;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Game.Server.Managers
{
	public class FusionMgr
	{
		private static Dictionary<string, FusionInfo> dictionary_0;

		private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static Items_Fusion_List_Info[] m_itemsfusionlist = null;

		private static Random random_0;

		private static ReaderWriterLock readerWriterLock_0;

		public static ItemTemplateInfo Fusion(List<ItemInfo> Items, List<ItemInfo> AppendItems, ref bool isBind, ref bool result)
		{
			List<int> list = new List<int>();
			int num = 0;
			int TotalRate = 0;
			int num2 = 0;
			if (Items != null)
			{
				ItemTemplateInfo itemTemplateInfo = null;
				foreach (ItemInfo Item in Items)
				{
					if (Item != null)
					{
						list.Add(Item.Template.FusionType);
						if (Item.Template.Level > num)
						{
							num = Item.Template.Level;
						}
						TotalRate += Item.Template.FusionRate;
						num2 += Item.Template.FusionNeedRate;
						if (Item.IsBinds)
						{
							isBind = true;
						}
					}
				}
				foreach (ItemInfo AppendItem in AppendItems)
				{
					TotalRate += AppendItem.Template.FusionRate / 2;
					num2 += AppendItem.Template.FusionNeedRate / 2;
					if (AppendItem.IsBinds)
					{
						isBind = true;
					}
				}
				list.Sort();
				StringBuilder stringBuilder = new StringBuilder();
				foreach (int item in list)
				{
					stringBuilder.Append(item);
				}
				string key = stringBuilder.ToString();
				readerWriterLock_0.AcquireReaderLock(-1);
				try
				{
					if (dictionary_0.ContainsKey(key))
					{
						FusionInfo fusionInfo = dictionary_0[key];
						ItemTemplateInfo goodsbyFusionTypeandLevel = ItemMgr.GetGoodsbyFusionTypeandLevel(fusionInfo.Reward, num);
						ItemTemplateInfo goodsbyFusionTypeandLevel2 = ItemMgr.GetGoodsbyFusionTypeandLevel(fusionInfo.Reward, num + 1);
						ItemTemplateInfo goodsbyFusionTypeandLevel3 = ItemMgr.GetGoodsbyFusionTypeandLevel(fusionInfo.Reward, num + 2);
						List<ItemTemplateInfo> list2 = new List<ItemTemplateInfo>();
						if (goodsbyFusionTypeandLevel3 != null)
						{
							list2.Add(goodsbyFusionTypeandLevel3);
						}
						if (goodsbyFusionTypeandLevel2 != null)
						{
							list2.Add(goodsbyFusionTypeandLevel2);
						}
						if (goodsbyFusionTypeandLevel != null)
						{
							list2.Add(goodsbyFusionTypeandLevel);
						}
						Func<ItemTemplateInfo, bool> predicate = (ItemTemplateInfo s) => (double)TotalRate / (double)s.FusionNeedRate <= 1.1;
						ItemTemplateInfo itemTemplateInfo2 = Enumerable.OrderByDescending<ItemTemplateInfo, double>(keySelector: (ItemTemplateInfo s) => (double)TotalRate / (double)s.FusionNeedRate, source: list2.Where(predicate)).FirstOrDefault();
						Func<ItemTemplateInfo, bool> predicate2 = (ItemTemplateInfo s) => (double)TotalRate / (double)s.FusionNeedRate > 1.1;
						ItemTemplateInfo itemTemplateInfo3 = Enumerable.OrderBy<ItemTemplateInfo, double>(keySelector: (ItemTemplateInfo s) => (double)TotalRate / (double)s.FusionNeedRate, source: list2.Where(predicate2)).FirstOrDefault();
						if (itemTemplateInfo2 != null && itemTemplateInfo3 == null)
						{
							itemTemplateInfo = itemTemplateInfo2;
							Items_Fusion_List_Info items_Fusion_List_Info = null;
							for (int i = 0; i < m_itemsfusionlist.Count(); i++)
							{
								if (m_itemsfusionlist[i].TemplateID == itemTemplateInfo2.TemplateID)
								{
									items_Fusion_List_Info = m_itemsfusionlist[i];
									break;
								}
							}
							if (items_Fusion_List_Info != null)
							{
								if (new Random().Next(1, 100) < items_Fusion_List_Info.Real)
								{
									result = true;
								}
								else
								{
									result = false;
								}
							}
							else
							{
								result = true;
							}
						}
						if (itemTemplateInfo2 != null && itemTemplateInfo3 != null)
						{
							if (itemTemplateInfo2.Level - itemTemplateInfo3.Level == 2)
							{
								double num3 = (double)(100 * TotalRate) * 0.6 / (double)itemTemplateInfo2.FusionNeedRate;
							}
							else
							{
								double num4 = (double)(100 * TotalRate) / (double)itemTemplateInfo2.FusionNeedRate;
							}
							if ((double)(100 * TotalRate) / (double)itemTemplateInfo2.FusionNeedRate > (double)random_0.Next(100))
							{
								itemTemplateInfo = itemTemplateInfo2;
								result = true;
							}
							else
							{
								itemTemplateInfo = itemTemplateInfo3;
								result = true;
							}
						}
						if (itemTemplateInfo2 == null && itemTemplateInfo3 != null)
						{
							itemTemplateInfo = itemTemplateInfo3;
							if (random_0.Next(num2) < TotalRate)
							{
								result = true;
							}
						}
						if (result)
						{
							using (List<ItemInfo>.Enumerator enumerator3 = Items.GetEnumerator())
							{
								do
								{
									if (!enumerator3.MoveNext())
									{
										return itemTemplateInfo;
									}
								}
								while (enumerator3.Current.Template.TemplateID != itemTemplateInfo.TemplateID);
								result = false;
							}
						}
						return itemTemplateInfo;
					}
				}
				catch
				{
				}
				finally
				{
					readerWriterLock_0.ReleaseReaderLock();
				}
			}
			return null;
		}

		public static Dictionary<int, double> FusionPreview(List<ItemInfo> Items, List<ItemInfo> AppendItems, ref bool isBind)
		{
			List<int> list = new List<int>();
			int num = 0;
			int TotalRate = 0;
			int num2 = 0;
			Dictionary<int, double> dictionary = new Dictionary<int, double>();
			dictionary.Clear();
			foreach (ItemInfo Item in Items)
			{
				list.Add(Item.Template.FusionType);
				if (Item.Template.Level > num)
				{
					num = Item.Template.Level;
				}
				TotalRate += Item.Template.FusionRate;
				num2 += Item.Template.FusionNeedRate;
				if (Item.IsBinds)
				{
					isBind = true;
				}
			}
			foreach (ItemInfo AppendItem in AppendItems)
			{
				TotalRate += AppendItem.Template.FusionRate / 2;
				num2 += AppendItem.Template.FusionRate / 2;
				if (AppendItem.IsBinds)
				{
					isBind = true;
				}
			}
			list.Sort();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (int item in list)
			{
				stringBuilder.Append(item);
			}
			string key = stringBuilder.ToString().Trim();
			readerWriterLock_0.AcquireReaderLock(-1);
			try
			{
				if (dictionary_0.ContainsKey(key))
				{

					FusionInfo fusionInfo = dictionary_0[key];
					double num3 = 0.0;
					double num4 = 0.0;
					ItemTemplateInfo goodsbyFusionTypeandLevel = ItemMgr.GetGoodsbyFusionTypeandLevel(fusionInfo.Reward, num);
					ItemTemplateInfo goodsbyFusionTypeandLevel2 = ItemMgr.GetGoodsbyFusionTypeandLevel(fusionInfo.Reward, num + 1);
					ItemTemplateInfo goodsbyFusionTypeandLevel3 = ItemMgr.GetGoodsbyFusionTypeandLevel(fusionInfo.Reward, num + 2);
					List<ItemTemplateInfo> list2 = new List<ItemTemplateInfo>();
					if (goodsbyFusionTypeandLevel3 != null)
					{
						list2.Add(goodsbyFusionTypeandLevel3);
					}
					if (goodsbyFusionTypeandLevel2 != null)
					{
						list2.Add(goodsbyFusionTypeandLevel2);
					}
					if (goodsbyFusionTypeandLevel != null)
					{
						list2.Add(goodsbyFusionTypeandLevel);
					}

					ItemTemplateInfo itemTemplateInfo = (from s in list2
														 where (double)TotalRate / (double)s.FusionNeedRate <= 1.1
														 orderby (double)TotalRate / (double)s.FusionNeedRate descending
														 select s).FirstOrDefault();
					ItemTemplateInfo itemTemplateInfo2 = (from s in list2
														  where (double)TotalRate / (double)s.FusionNeedRate > 1.1
														  orderby (double)TotalRate / (double)s.FusionNeedRate
														  select s).FirstOrDefault();

					if (itemTemplateInfo != null && itemTemplateInfo2 == null)
					{
						num3 = (double)(100 * TotalRate) / (double)num2;
						dictionary.Add(itemTemplateInfo.TemplateID, num3);
					}
					if (itemTemplateInfo != null && itemTemplateInfo2 != null)
					{
						if (itemTemplateInfo.Level - itemTemplateInfo2.Level == 2)
						{
							num3 = (double)(100 * TotalRate) * 0.6 / (double)itemTemplateInfo.FusionNeedRate;
							num4 = 100.0 - num3;
						}
						else
						{
							num3 = (double)(100 * TotalRate) / (double)itemTemplateInfo.FusionNeedRate;
							num4 = 100.0 - num3;
						}
						dictionary.Add(itemTemplateInfo.TemplateID, num3);
						dictionary.Add(itemTemplateInfo2.TemplateID, num4);
					}
					if (itemTemplateInfo == null && itemTemplateInfo2 != null)
					{
						num4 = (double)(100 * TotalRate) / (double)num2;
						dictionary.Add(itemTemplateInfo2.TemplateID, num4);
					}
					int[] array = dictionary.Keys.ToArray();
					int[] array2 = array;
					foreach (int num5 in array2)
					{
						foreach (ItemInfo Item2 in Items)
						{
							if (num5 == Item2.Template.TemplateID && dictionary.ContainsKey(num5))
							{
								dictionary.Remove(num5);
							}
						}
					}
					return dictionary;
				}
				return dictionary;
			}
			catch
			{
			}
			finally
			{
				readerWriterLock_0.ReleaseReaderLock();
			}
			return null;
		}
		public static bool Init()
		{
			try
			{
				readerWriterLock_0 = new ReaderWriterLock();
				dictionary_0 = new Dictionary<string, FusionInfo>();
				random_0 = new Random();
				return smethod_0(dictionary_0);
			}
			catch (Exception exception)
			{
				if (ilog_0.IsErrorEnabled)
				{
					ilog_0.Error("FusionMgr", exception);
				}
				return false;
			}
		}

		public static bool ReLoad()
		{
			try
			{
				Dictionary<string, FusionInfo> pVQImtJIaVCSTenHlR = new Dictionary<string, FusionInfo>();
				if (smethod_0(pVQImtJIaVCSTenHlR))
				{
					readerWriterLock_0.AcquireWriterLock(-1);
					try
					{
						dictionary_0 = pVQImtJIaVCSTenHlR;
						return true;
					}
					catch
					{
					}
					finally
					{
						readerWriterLock_0.ReleaseWriterLock();
					}
				}
			}
			catch (Exception exception)
			{
				if (ilog_0.IsErrorEnabled)
				{
					ilog_0.Error("FusionMgr", exception);
				}
			}
			return false;
		}

		private static bool smethod_0(Dictionary<string, FusionInfo> PVQImtJIaVCSTenHlR1)
		{
			using (ProduceBussiness produceBussiness = new ProduceBussiness())
			{
				FusionInfo[] allFusion = produceBussiness.GetAllFusion();
				foreach (FusionInfo fusionInfo in allFusion)
				{
					List<int> list = new List<int>();
					list.Add(fusionInfo.Item1);
					list.Add(fusionInfo.Item2);
					list.Add(fusionInfo.Item3);
					list.Add(fusionInfo.Item4);
					list.Sort();
					StringBuilder stringBuilder = new StringBuilder();
					foreach (int item in list)
					{
						if (item != 0)
						{
							stringBuilder.Append(item);
						}
					}
					string key = stringBuilder.ToString();
					if (!PVQImtJIaVCSTenHlR1.ContainsKey(key))
					{
						PVQImtJIaVCSTenHlR1.Add(key, fusionInfo);
					}
				}
				m_itemsfusionlist = produceBussiness.GetAllFusionList();
			}
			return true;
		}
	}
}
