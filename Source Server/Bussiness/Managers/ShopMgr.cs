// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.ShopMgr
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
  public static class ShopMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ReaderWriterLock m_lock = new ReaderWriterLock();
    private static Dictionary<int, ShopItemInfo> m_shop = new Dictionary<int, ShopItemInfo>();
    private static Dictionary<int, ShopGoodsShowListInfo> m_ShopGoodsCanBuy = new Dictionary<int, ShopGoodsShowListInfo>();
    private static Dictionary<int, ShopGoodsShowListInfo> m_shopGoodsShowLists = new Dictionary<int, ShopGoodsShowListInfo>();

    public static bool CanBuy(
      int shopID,
      int consortiaShopLevel,
      ref bool isBinds,
      int cousortiaID,
      int playerRiches)
    {
      bool flag = false;
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        switch (shopID)
        {
          case 1:
            flag = true;
            isBinds = false;
            break;
          case 2:
            flag = true;
            isBinds = false;
            break;
          case 3:
            flag = true;
            isBinds = false;
            break;
          case 4:
            flag = true;
            isBinds = false;
            break;
          case 11:
            ConsortiaEquipControlInfo consortiaEuqipRiches1 = consortiaBussiness.GetConsortiaEuqipRiches(cousortiaID, 1, 1);
            if (consortiaShopLevel >= consortiaEuqipRiches1.Level)
            {
              if (playerRiches >= consortiaEuqipRiches1.Riches)
              {
                flag = true;
                isBinds = true;
                break;
              }
              break;
            }
            break;
          case 12:
            ConsortiaEquipControlInfo consortiaEuqipRiches2 = consortiaBussiness.GetConsortiaEuqipRiches(cousortiaID, 2, 1);
            if (consortiaShopLevel >= consortiaEuqipRiches2.Level)
            {
              if (playerRiches >= consortiaEuqipRiches2.Riches)
              {
                flag = true;
                isBinds = true;
                break;
              }
              break;
            }
            break;
          case 13:
            ConsortiaEquipControlInfo consortiaEuqipRiches3 = consortiaBussiness.GetConsortiaEuqipRiches(cousortiaID, 3, 1);
            if (consortiaShopLevel >= consortiaEuqipRiches3.Level)
            {
              if (playerRiches >= consortiaEuqipRiches3.Riches)
              {
                flag = true;
                isBinds = true;
                break;
              }
              break;
            }
            break;
          case 14:
            ConsortiaEquipControlInfo consortiaEuqipRiches4 = consortiaBussiness.GetConsortiaEuqipRiches(cousortiaID, 4, 1);
            if (consortiaShopLevel >= consortiaEuqipRiches4.Level)
            {
              if (playerRiches >= consortiaEuqipRiches4.Riches)
              {
                flag = true;
                isBinds = true;
                break;
              }
              break;
            }
            break;
          case 15:
            ConsortiaEquipControlInfo consortiaEuqipRiches5 = consortiaBussiness.GetConsortiaEuqipRiches(cousortiaID, 5, 1);
            if (consortiaShopLevel >= consortiaEuqipRiches5.Level)
            {
              if (playerRiches >= consortiaEuqipRiches5.Riches)
              {
                flag = true;
                isBinds = true;
                break;
              }
              break;
            }
            break;
          case 20:
            flag = true;
            isBinds = true;
            break;
        }
      }
      return flag;
    }

    public static bool CheckInShopGoodsCanBuy(int iTemplateID)
    {
      return ShopMgr.m_ShopGoodsCanBuy.ContainsKey(iTemplateID);
    }

    public static int FindItemTemplateID(int id)
    {
      if (ShopMgr.m_shop.ContainsKey(id))
        return ShopMgr.m_shop[id].TemplateID;
      return 0;
    }

    public static ShopItemInfo FindShopbyTemplateID(int TemplatID)
    {
      foreach (ShopItemInfo shopItemInfo in ShopMgr.m_shop.Values)
      {
        if (shopItemInfo.TemplateID == TemplatID)
          return shopItemInfo;
      }
      return (ShopItemInfo) null;
    }

    public static ShopItemInfo FindShopbyID(int ID)
    {
      foreach (ShopItemInfo shopItemInfo in ShopMgr.m_shop.Values)
      {
        if (shopItemInfo.ID == ID)
          return shopItemInfo;
      }
      return (ShopItemInfo) null;
    }

    public static List<ShopItemInfo> FindShopbyTemplatID(int TemplatID)
    {
      List<ShopItemInfo> shopItemInfoList = new List<ShopItemInfo>();
      foreach (ShopItemInfo shopItemInfo in ShopMgr.m_shop.Values)
      {
        if (shopItemInfo.TemplateID == TemplatID)
          shopItemInfoList.Add(shopItemInfo);
      }
      return shopItemInfoList;
    }

    public static void FindSpecialItemInfo(
      SqlDataProvider.Data.ItemInfo info,
      ref int gold,
      ref int money,
      ref int giftToken,
      ref int medal,
      ref int honor,
      ref int hardCurrency,
      ref int token,
      ref int dragonToken,
      ref int magicStonePoint)
    {
      switch (info.TemplateID)
      {
        case -1400:
          magicStonePoint += info.Count;
          info = (SqlDataProvider.Data.ItemInfo) null;
          break;
        case -1200:
          dragonToken += info.Count;
          info = (SqlDataProvider.Data.ItemInfo) null;
          break;
        case -1100:
          giftToken += info.Count;
          info = (SqlDataProvider.Data.ItemInfo) null;
          break;
        case -1000:
          token += info.Count;
          info = (SqlDataProvider.Data.ItemInfo) null;
          break;
        case -900:
          hardCurrency += info.Count;
          info = (SqlDataProvider.Data.ItemInfo) null;
          break;
        case -800:
          honor += info.Count;
          info = (SqlDataProvider.Data.ItemInfo) null;
          break;
        case -300:
          medal += info.Count;
          info = (SqlDataProvider.Data.ItemInfo) null;
          break;
        case -200:
          money += info.Count;
          info = (SqlDataProvider.Data.ItemInfo) null;
          break;
        case -100:
          gold += info.Count;
          info = (SqlDataProvider.Data.ItemInfo) null;
          break;
      }
    }

    public static void GetItemPrice(
      int Prices,
      int Values,
      Decimal beat,
      ref int damageScore,
      ref int petScore,
      ref int iTemplateID,
      ref int iCount,
      ref int gold,
      ref int money,
      ref int offer,
      ref int gifttoken,
      ref int medal,
      ref int hardCurrency,
      ref int LeagueMoney,
      ref int useableScore)
    {
      switch (Prices)
      {
        case 11408:
          medal += (int)((Decimal)Values * beat);
          break;
        case -1200:
          useableScore += (int) ((Decimal) Values * beat);
          break;
        case -1000:
          LeagueMoney += (int) ((Decimal) Values * beat);
          break;
        case -900:
          hardCurrency += (int) ((Decimal) Values * beat);
          break;
        case -800:
          medal += (int) ((Decimal) Values * beat);
          break;
        case -300:
          medal += (int) ((Decimal) Values * beat);
          break;
        case -8:
          petScore += (int) ((Decimal) Values * beat);
          break;
        case -7:
          damageScore += (int) ((Decimal) Values * beat);
          break;
        case -6:
          medal += (int) ((Decimal) Values * beat);
          break;
        case -4:
          gifttoken += (int) ((Decimal) Values * beat);
          break;
        case -3:
          offer += (int) ((Decimal) Values * beat);
          break;
        case -2:
          gold += (int) ((Decimal) Values * beat);
          
          break;
        case -1:
          money += (int) ((Decimal) Values * beat);
          break;
        default:
          if (Prices <= 0)
            break;
          iTemplateID = Prices;
          iCount = Values;
          break;
      }
    }

    public static ShopItemInfo GetShopItemInfoById(int ID)
    {
      if (ShopMgr.m_shop.ContainsKey(ID))
        return ShopMgr.m_shop[ID];
      return (ShopItemInfo) null;
    }

    public static bool Init()
    {
      return ShopMgr.ReLoad();
    }

    public static bool IsOnShop(int Id)
    {
      if (ShopMgr.m_shopGoodsShowLists == null)
        ShopMgr.Init();
      if (ShopMgr.IsSpecialItem(Id))
        return true;
      ShopMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (ShopMgr.m_shopGoodsShowLists.Keys.Contains<int>(Id))
          return true;
      }
      finally
      {
        ShopMgr.m_lock.ReleaseReaderLock();
      }
      return false;
    }

    public static bool IsSpecialItem(int Id)
    {
      if (Id <= 1100801)
      {
        if (Id != 1100401 && Id != 1100801)
          return false;
      }
      else if (Id != 1101201 && Id != 1101601)
        return false;
      return true;
    }

    private static Dictionary<int, ShopItemInfo> LoadFromDatabase()
    {
      Dictionary<int, ShopItemInfo> dictionary = new Dictionary<int, ShopItemInfo>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (ShopItemInfo shopItemInfo in produceBussiness.GetALllShop())
        {
          if (!dictionary.ContainsKey(shopItemInfo.ID))
            dictionary.Add(shopItemInfo.ID, shopItemInfo);
        }
      }
      return dictionary;
    }

    private static Dictionary<int, ShopGoodsShowListInfo> LoadShopGoodsCanBuyFromDatabase()
    {
      Dictionary<int, ShopGoodsShowListInfo> dictionary = new Dictionary<int, ShopGoodsShowListInfo>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (ShopGoodsShowListInfo allShopGoodsShow in produceBussiness.GetAllShopGoodsShowList())
        {
          if (!dictionary.ContainsKey(allShopGoodsShow.ShopId))
            dictionary.Add(allShopGoodsShow.ShopId, allShopGoodsShow);
        }
      }
      return dictionary;
    }

    private static Dictionary<int, ShopGoodsShowListInfo> LoadShowListFromDatabase()
    {
      Dictionary<int, ShopGoodsShowListInfo> dictionary = new Dictionary<int, ShopGoodsShowListInfo>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (ShopGoodsShowListInfo allShopGoodsShow in produceBussiness.GetAllShopGoodsShowList())
        {
          if (!dictionary.ContainsKey(allShopGoodsShow.ShopId))
            dictionary.Add(allShopGoodsShow.ShopId, allShopGoodsShow);
        }
      }
      return dictionary;
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, ShopItemInfo> dictionary1 = ShopMgr.LoadFromDatabase();
        Dictionary<int, ShopGoodsShowListInfo> dictionary2 = ShopMgr.LoadShowListFromDatabase();
        if (dictionary1.Count > 0)
          Interlocked.Exchange<Dictionary<int, ShopItemInfo>>(ref ShopMgr.m_shop, dictionary1);
        if (dictionary2.Count > 0)
          Interlocked.Exchange<Dictionary<int, ShopGoodsShowListInfo>>(ref ShopMgr.m_shopGoodsShowLists, dictionary2);
        try
        {
          Dictionary<int, ShopGoodsShowListInfo> dictionary3 = ShopMgr.LoadShopGoodsCanBuyFromDatabase();
          if (dictionary3.Count > 0)
            Interlocked.Exchange<Dictionary<int, ShopGoodsShowListInfo>>(ref ShopMgr.m_ShopGoodsCanBuy, dictionary3);
        }
        catch (Exception ex)
        {
          ShopMgr.log.Error((object) "ShopInfoMgr", ex);
        }
        return true;
      }
      catch (Exception ex)
      {
        ShopMgr.log.Error((object) "ShopInfoMgr", ex);
      }
      return false;
    }

    public static bool SetItemType(
      ShopItemInfo shop,
      int type,
      ref int damageScore,
      ref int petScore,
      ref int iTemplateID,
      ref int iCount,
      ref int gold,
      ref int money,
      ref int offer,
      ref int gifttoken,
      ref int medal,
      ref int hardCurrency,
      ref int LeagueMoney,
      ref int useableScore)
    {
      if (type == 1)
      {
        ShopMgr.GetItemPrice(shop.APrice1, shop.AValue1, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);
        ShopMgr.GetItemPrice(shop.APrice2, shop.AValue2, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);
        ShopMgr.GetItemPrice(shop.APrice3, shop.AValue3, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);

            }
      if (type == 2)
      {
        ShopMgr.GetItemPrice(shop.BPrice1, shop.BValue1, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);
        ShopMgr.GetItemPrice(shop.BPrice2, shop.BValue2, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);
        ShopMgr.GetItemPrice(shop.BPrice3, shop.BValue3, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);

            }
      if (type == 3)
      {
        ShopMgr.GetItemPrice(shop.CPrice1, shop.CValue1, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);
        ShopMgr.GetItemPrice(shop.CPrice2, shop.CValue2, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);
        ShopMgr.GetItemPrice(shop.CPrice3, shop.CValue3, shop.Beat, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref hardCurrency, ref LeagueMoney, ref useableScore);

            }

      return true;
    }
  }
}
