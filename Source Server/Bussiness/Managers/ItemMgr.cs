// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.ItemMgr
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
  public class ItemMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<int, ItemTemplateInfo> _items;
    private static Dictionary<int, LoadUserBoxInfo> _timeBoxs;
    private static List<ItemTemplateInfo> Lists;
    private static ReaderWriterLock m_lock;

    public static LoadUserBoxInfo FindItemBoxTemplate(int Id)
    {
      if (ItemMgr._timeBoxs == null)
        ItemMgr.Init();
      ItemMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (ItemMgr._timeBoxs.Keys.Contains<int>(Id))
          return ItemMgr._timeBoxs[Id];
      }
      finally
      {
        ItemMgr.m_lock.ReleaseReaderLock();
      }
      return (LoadUserBoxInfo) null;
    }

    public static LoadUserBoxInfo FindItemBoxTypeAndLv(int type, int lv)
    {
      if (ItemMgr._timeBoxs == null)
        ItemMgr.Init();
      ItemMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        foreach (LoadUserBoxInfo loadUserBoxInfo in ItemMgr._timeBoxs.Values)
        {
          if (loadUserBoxInfo.Type == type && loadUserBoxInfo.Level == lv)
            return loadUserBoxInfo;
        }
      }
      finally
      {
        ItemMgr.m_lock.ReleaseReaderLock();
      }
      return (LoadUserBoxInfo) null;
    }

    public static ItemTemplateInfo FindItemTemplate(int templateId)
    {
      if (ItemMgr._items == null)
        ItemMgr.Init();
      ItemMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (ItemMgr._items.Keys.Contains<int>(templateId))
          return ItemMgr._items[templateId];
      }
      finally
      {
        ItemMgr.m_lock.ReleaseReaderLock();
      }
      return (ItemTemplateInfo) null;
    }

    public static ItemTemplateInfo GetGoodsbyFusionTypeandLevel(
      int fusionType,
      int level)
    {
      if (ItemMgr._items == null)
        ItemMgr.Init();
      ItemMgr.m_lock.AcquireReaderLock(-1);
      try
      {
        foreach (ItemTemplateInfo itemTemplateInfo in ItemMgr._items.Values)
        {
          if (itemTemplateInfo.FusionType == fusionType && itemTemplateInfo.Level == level)
            return itemTemplateInfo;
        }
      }
      finally
      {
        ItemMgr.m_lock.ReleaseReaderLock();
      }
      return (ItemTemplateInfo) null;
    }

    public static ItemTemplateInfo GetGoodsbyFusionTypeandQuality(
      int fusionType,
      int quality)
    {
      if (ItemMgr._items == null)
        ItemMgr.Init();
      ItemMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        foreach (ItemTemplateInfo itemTemplateInfo in ItemMgr._items.Values)
        {
          if (itemTemplateInfo.FusionType == fusionType && itemTemplateInfo.Quality == quality)
            return itemTemplateInfo;
        }
      }
      finally
      {
        ItemMgr.m_lock.ReleaseReaderLock();
      }
      return (ItemTemplateInfo) null;
    }

    public static bool Init()
    {
      try
      {
        ItemMgr.m_lock = new ReaderWriterLock();
        ItemMgr._items = new Dictionary<int, ItemTemplateInfo>();
        ItemMgr._timeBoxs = new Dictionary<int, LoadUserBoxInfo>();
        ItemMgr.Lists = new List<ItemTemplateInfo>();
        return ItemMgr.LoadItem(ItemMgr._items, ItemMgr._timeBoxs);
      }
      catch (Exception ex)
      {
        if (ItemMgr.log.IsErrorEnabled)
          ItemMgr.log.Error((object) nameof (Init), ex);
        return false;
      }
    }

    public static bool LoadItem(
      Dictionary<int, ItemTemplateInfo> infos,
      Dictionary<int, LoadUserBoxInfo> userBoxs)
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (ItemTemplateInfo allGood in produceBussiness.GetAllGoods())
        {
          if (!infos.Keys.Contains<int>(allGood.TemplateID))
            infos.Add(allGood.TemplateID, allGood);
        }
        foreach (LoadUserBoxInfo loadUserBoxInfo in produceBussiness.GetAllTimeBoxAward())
        {
          if (!userBoxs.Keys.Contains<int>(loadUserBoxInfo.ID))
            userBoxs.Add(loadUserBoxInfo.ID, loadUserBoxInfo);
        }
      }
      return true;
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, ItemTemplateInfo> infos = new Dictionary<int, ItemTemplateInfo>();
        Dictionary<int, LoadUserBoxInfo> userBoxs = new Dictionary<int, LoadUserBoxInfo>();
        if (ItemMgr.LoadItem(infos, userBoxs))
        {
          ItemMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            ItemMgr._items = infos;
            ItemMgr._timeBoxs = userBoxs;
            return true;
          }
          catch
          {
          }
          finally
          {
            ItemMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (ItemMgr.log.IsErrorEnabled)
          ItemMgr.log.Error((object) nameof (ReLoad), ex);
      }
      return false;
    }

    public static List<SqlDataProvider.Data.ItemInfo> SpiltGoodsMaxCount(
      SqlDataProvider.Data.ItemInfo itemInfo)
    {
      List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
      for (int index = 0; index < itemInfo.Count; index += itemInfo.Template.MaxCount)
      {
        int num = itemInfo.Count < itemInfo.Template.MaxCount ? itemInfo.Count : itemInfo.Template.MaxCount;
        SqlDataProvider.Data.ItemInfo itemInfo1 = itemInfo.Clone();
        itemInfo1.Count = num;
        itemInfoList.Add(itemInfo1);
      }
      return itemInfoList;
    }

    public static ItemTemplateInfo FindGoldItemTemplate(int templateId, bool IsGold)
    {
      if (!IsGold)
        return (ItemTemplateInfo) null;
      GoldEquipTemplateInfo goldEquipByTemplate = GoldEquipMgr.FindGoldEquipByTemplate(templateId);
      if (goldEquipByTemplate == null || !ItemMgr._items.Keys.Contains<int>(goldEquipByTemplate.NewTemplateId))
        return (ItemTemplateInfo) null;
      return ItemMgr._items[goldEquipByTemplate.NewTemplateId];
    }
  }
}
