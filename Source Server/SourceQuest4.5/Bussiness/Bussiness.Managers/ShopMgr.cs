using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
	public static class ShopMgr
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static ReaderWriterLock m_lock = new ReaderWriterLock();

		private static Dictionary<int, ShopItemInfo> m_shop = new Dictionary<int, ShopItemInfo>();

		private static Dictionary<int, ShopGoodsShowListInfo> m_ShopGoodsCanBuy = new Dictionary<int, ShopGoodsShowListInfo>();

		private static Dictionary<int, ShopGoodsShowListInfo> m_shopGoodsShowLists = new Dictionary<int, ShopGoodsShowListInfo>();

		public static bool CanBuy(int shopID, int consortiaShopLevel, ref bool isBinds, int cousortiaID, int playerRiches)
		{
			bool flag11 = false;
			using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
			{
				switch (shopID)
				{
				case 1:
					flag11 = true;
					isBinds = false;
					return flag11;
				case 2:
					flag11 = true;
					isBinds = false;
					return flag11;
				case 3:
					flag11 = true;
					isBinds = false;
					return flag11;
				case 4:
					flag11 = true;
					isBinds = false;
					return flag11;
				case 11:
				{
					ConsortiaEquipControlInfo consortiaEuqipRiches1 = consortiaBussiness.GetConsortiaEuqipRiches(cousortiaID, 1, 1);
					if (consortiaShopLevel < consortiaEuqipRiches1.Level)
					{
						return flag11;
					}
					if (playerRiches < consortiaEuqipRiches1.Riches)
					{
						return flag11;
					}
					flag11 = true;
					isBinds = true;
					return flag11;
				}
				case 12:
				{
					ConsortiaEquipControlInfo consortiaEuqipRiches2 = consortiaBussiness.GetConsortiaEuqipRiches(cousortiaID, 2, 1);
					if (consortiaShopLevel < consortiaEuqipRiches2.Level)
					{
						return flag11;
					}
					if (playerRiches < consortiaEuqipRiches2.Riches)
					{
						return flag11;
					}
					flag11 = true;
					isBinds = true;
					return flag11;
				}
				case 13:
				{
					ConsortiaEquipControlInfo consortiaEuqipRiches3 = consortiaBussiness.GetConsortiaEuqipRiches(cousortiaID, 3, 1);
					if (consortiaShopLevel < consortiaEuqipRiches3.Level)
					{
						return flag11;
					}
					if (playerRiches < consortiaEuqipRiches3.Riches)
					{
						return flag11;
					}
					flag11 = true;
					isBinds = true;
					return flag11;
				}
				case 14:
				{
					ConsortiaEquipControlInfo consortiaEuqipRiches4 = consortiaBussiness.GetConsortiaEuqipRiches(cousortiaID, 4, 1);
					if (consortiaShopLevel < consortiaEuqipRiches4.Level)
					{
						return flag11;
					}
					if (playerRiches < consortiaEuqipRiches4.Riches)
					{
						return flag11;
					}
					flag11 = true;
					isBinds = true;
					return flag11;
				}
				case 15:
				{
					ConsortiaEquipControlInfo consortiaEuqipRiches5 = consortiaBussiness.GetConsortiaEuqipRiches(cousortiaID, 5, 1);
					if (consortiaShopLevel < consortiaEuqipRiches5.Level)
					{
						return flag11;
					}
					if (playerRiches < consortiaEuqipRiches5.Riches)
					{
						return flag11;
					}
					flag11 = true;
					isBinds = true;
					return flag11;
				}
				case 20:
					flag11 = true;
					isBinds = true;
					return flag11;
				default:
					return flag11;
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
					return flag11;
				}
			}
		}

		public static bool CheckInShopGoodsCanBuy(int iTemplateID)
		{
			return m_ShopGoodsCanBuy.ContainsKey(iTemplateID);
		}

		public static int FindItemTemplateID(int id)
		{
			if (m_shop.ContainsKey(id))
			{
				return m_shop[id].TemplateID;
			}
			return 0;
		}

		public static ShopItemInfo FindShopbyTemplateID(int TemplatID)
		{
			foreach (ShopItemInfo current in m_shop.Values)
			{
				if (current.TemplateID == TemplatID)
				{
					return current;
				}
			}
			return null;
		}

		public static ShopItemInfo FindShopbyID(int ID)
		{
			foreach (ShopItemInfo info in m_shop.Values)
			{
				if (info.ID == ID)
				{
					return info;
				}
			}
			return null;
		}

		public static List<ShopItemInfo> FindShopbyTemplatID(int TemplatID)
		{
			List<ShopItemInfo> list = new List<ShopItemInfo>();
			foreach (ShopItemInfo info in m_shop.Values)
			{
				if (info.TemplateID == TemplatID)
				{
					list.Add(info);
				}
			}
			return list;
		}

		public static void FindSpecialItemInfo(ItemInfo info, ref int gold, ref int money, ref int giftToken, ref int medal, ref int honor, ref int hardCurrency, ref int token, ref int dragonToken, ref int magicStonePoint)
		{
			switch (info.TemplateID)
			{
			case -1400:
				magicStonePoint += info.Count;
				info = null;
				break;
			case -1200:
				dragonToken += info.Count;
				info = null;
				break;
			case -1100:
				giftToken += info.Count;
				info = null;
				break;
			case -1000:
				token += info.Count;
				info = null;
				break;
			case -900:
				hardCurrency += info.Count;
				info = null;
				break;
			case -800:
				honor += info.Count;
				info = null;
				break;
			case -300:
				medal += info.Count;
				info = null;
				break;
			case -200:
				money += info.Count;
				info = null;
				break;
			case -100:
				gold += info.Count;
				info = null;
				break;
			}
		}

		public static void GetItemPrice(int Prices, int Values, decimal beat, ref int damageScore, ref int petScore, ref int iTemplateID, ref int iCount, ref int gold, ref int money, ref int offer, ref int gifttoken, ref int medal, ref int hardCurrency, ref int LeagueMoney, ref int useableScore)
		{
			switch (Prices)
			{
			case -1200:
				useableScore += (int)((decimal)Values * beat);
				return;
			case -1000:
				LeagueMoney += (int)((decimal)Values * beat);
				return;
			case -900:
				hardCurrency += (int)((decimal)Values * beat);
				return;
			case -8:
				petScore += (int)((decimal)Values * beat);
				return;
			case -7:
				damageScore += (int)((decimal)Values * beat);
				return;
			case -6:
				medal += (int)((decimal)Values * beat);
				return;
			case -4:
				gifttoken += (int)((decimal)Values * beat);
				return;
			case -3:
				gold += (int)((decimal)Values * beat);
				return;
			case -2:
				offer += (int)((decimal)Values * beat);
				return;
			case -1:
				money += (int)((decimal)Values * beat);
				return;
			case -300:
				medal += (int)((decimal)Values * beat);
				return;
			case -800:
				medal += (int)((decimal)Values * beat);
				return;
			}
			if (Prices > 0)
			{
				iTemplateID = Prices;
				iCount = Values;
			}
		}

		public static ShopItemInfo GetShopItemInfoById(int ID)
		{
			if (m_shop.ContainsKey(ID))
			{
				return m_shop[ID];
			}
			return null;
		}

		public static bool Init()
		{
			return ReLoad();
		}

		public static bool IsOnShop(int Id)
		{
			if (m_shopGoodsShowLists == null)
			{
				Init();
			}
			if (IsSpecialItem(Id))
			{
				return true;
			}
			m_lock.AcquireReaderLock(10000);
			try
			{
				if (m_shopGoodsShowLists.Keys.Contains(Id))
				{
					return true;
				}
			}
			finally
			{
				m_lock.ReleaseReaderLock();
			}
			return false;
		}

		public static bool IsSpecialItem(int Id)
		{
			if (Id <= 1100801)
			{
				if (Id != 1100401 && Id != 1100801)
				{
					return false;
				}
			}
			else if (Id != 1101201 && Id != 1101601)
			{
				return false;
			}
			return true;
		}

		private static Dictionary<int, ShopItemInfo> LoadFromDatabase()
		{
			Dictionary<int, ShopItemInfo> dictionary = new Dictionary<int, ShopItemInfo>();
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				ShopItemInfo[] aLllShop = bussiness.GetALllShop();
				foreach (ShopItemInfo info in aLllShop)
				{
					if (!dictionary.ContainsKey(info.ID))
					{
						dictionary.Add(info.ID, info);
					}
				}
				return dictionary;
			}
		}

		private static Dictionary<int, ShopGoodsShowListInfo> LoadShopGoodsCanBuyFromDatabase()
		{
			Dictionary<int, ShopGoodsShowListInfo> dictionary = new Dictionary<int, ShopGoodsShowListInfo>();
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				ShopGoodsShowListInfo[] allShopGoodsShowList = bussiness.GetAllShopGoodsShowList();
				foreach (ShopGoodsShowListInfo info in allShopGoodsShowList)
				{
					if (!dictionary.ContainsKey(info.ShopId))
					{
						dictionary.Add(info.ShopId, info);
					}
				}
				return dictionary;
			}
		}

		private static Dictionary<int, ShopGoodsShowListInfo> LoadShowListFromDatabase()
		{
			Dictionary<int, ShopGoodsShowListInfo> dictionary = new Dictionary<int, ShopGoodsShowListInfo>();
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				ShopGoodsShowListInfo[] allShopGoodsShowList = bussiness.GetAllShopGoodsShowList();
				foreach (ShopGoodsShowListInfo info in allShopGoodsShowList)
				{
					if (!dictionary.ContainsKey(info.ShopId))
					{
						dictionary.Add(info.ShopId, info);
					}
				}
				return dictionary;
			}
		}

		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, ShopItemInfo> dictionary = LoadFromDatabase();
				Dictionary<int, ShopGoodsShowListInfo> dictionary2 = LoadShowListFromDatabase();
				if (dictionary.Count > 0)
				{
					Interlocked.Exchange(ref m_shop, dictionary);
				}
				if (dictionary2.Count > 0)
				{
					Interlocked.Exchange(ref m_shopGoodsShowLists, dictionary2);
				}
				try
				{
					Dictionary<int, ShopGoodsShowListInfo> dictionary3 = LoadShopGoodsCanBuyFromDatabase();
					if (dictionary3.Count > 0)
					{
						Interlocked.Exchange(ref m_ShopGoodsCanBuy, dictionary3);
					}
				}
				catch (Exception exception2)
				{
					log.Error("ShopInfoMgr", exception2);
				}
				return true;
			}
			catch (Exception exception2)
			{
				log.Error("ShopInfoMgr", exception2);
			}
			return false;
		}

		public static bool SetItemType(ShopItemInfo shop, int type, ref int damageScore, ref int petScore, ref int iTemplateID, ref int iCount, ref int gold, ref int money, ref int offer, ref int gifttoken, ref int medal, ref int hardCurrency, ref int LeagueMoney, ref int useableScore)
		{
			if (type == 1)
			{
				GetItemPrice(shop.APrice1, shop.AValue1, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);
				GetItemPrice(shop.APrice2, shop.AValue2, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);
				GetItemPrice(shop.APrice3, shop.AValue3, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);
			}
			if (type == 2)
			{
				GetItemPrice(shop.BPrice1, shop.BValue1, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);
				GetItemPrice(shop.BPrice2, shop.BValue2, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);
				GetItemPrice(shop.BPrice3, shop.BValue3, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);
			}
			if (type == 3)
			{
				GetItemPrice(shop.CPrice1, shop.CValue1, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);
				GetItemPrice(shop.CPrice2, shop.CValue2, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);
				GetItemPrice(shop.CPrice3, shop.CValue3, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);
			}
			return true;
		}
	}
}
