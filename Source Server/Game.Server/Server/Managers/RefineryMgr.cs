// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.RefineryMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Server.GameObjects;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
  public class RefineryMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<int, RefineryInfo> m_Item_Refinery = new Dictionary<int, RefineryInfo>();
    private static ThreadSafeRandom rand = new ThreadSafeRandom();

    public static bool Init()
    {
      return RefineryMgr.Reload();
    }

    public static Dictionary<int, RefineryInfo> LoadFromBD()
    {
      List<RefineryInfo> refineryInfoList = new List<RefineryInfo>();
      Dictionary<int, RefineryInfo> dictionary = new Dictionary<int, RefineryInfo>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (RefineryInfo refineryInfo in produceBussiness.GetAllRefineryInfo())
        {
          if (!dictionary.ContainsKey(refineryInfo.RefineryID))
            dictionary.Add(refineryInfo.RefineryID, refineryInfo);
        }
      }
      return dictionary;
    }

    public static ItemTemplateInfo Refinery(
      GamePlayer player,
      List<SqlDataProvider.Data.ItemInfo> Items,
      SqlDataProvider.Data.ItemInfo Item,
      bool Luck,
      int OpertionType,
      ref bool result,
      ref int defaultprobability,
      ref bool IsFormula)
    {
      ItemTemplateInfo itemTemplateInfo = new ItemTemplateInfo();
      foreach (int key in RefineryMgr.m_Item_Refinery.Keys)
      {
        if (RefineryMgr.m_Item_Refinery[key].m_Equip.Contains(Item.TemplateID))
        {
          IsFormula = true;
          int num = 0;
          List<int> intList = new List<int>();
          foreach (SqlDataProvider.Data.ItemInfo itemInfo in Items)
          {
            if (itemInfo.TemplateID == RefineryMgr.m_Item_Refinery[key].Item1 && itemInfo.Count >= RefineryMgr.m_Item_Refinery[key].Item1Count && !intList.Contains(itemInfo.TemplateID))
            {
              intList.Add(itemInfo.TemplateID);
              if ((uint) OpertionType > 0U)
                itemInfo.Count -= RefineryMgr.m_Item_Refinery[key].Item1Count;
              ++num;
            }
            if (itemInfo.TemplateID == RefineryMgr.m_Item_Refinery[key].Item2 && itemInfo.Count >= RefineryMgr.m_Item_Refinery[key].Item2Count && !intList.Contains(itemInfo.TemplateID))
            {
              intList.Add(itemInfo.TemplateID);
              if ((uint) OpertionType > 0U)
                itemInfo.Count -= RefineryMgr.m_Item_Refinery[key].Item2Count;
              ++num;
            }
            if (itemInfo.TemplateID == RefineryMgr.m_Item_Refinery[key].Item3 && itemInfo.Count >= RefineryMgr.m_Item_Refinery[key].Item3Count && !intList.Contains(itemInfo.TemplateID))
            {
              intList.Add(itemInfo.TemplateID);
              if ((uint) OpertionType > 0U)
                itemInfo.Count -= RefineryMgr.m_Item_Refinery[key].Item3Count;
              ++num;
            }
          }
          if (num == 3)
          {
            for (int index = 0; index < RefineryMgr.m_Item_Refinery[key].m_Reward.Count; ++index)
            {
              if (Items[Items.Count - 1].TemplateID == RefineryMgr.m_Item_Refinery[key].m_Reward[index])
              {
                if (Luck)
                  defaultprobability += 20;
                if (OpertionType == 0)
                  return ItemMgr.FindItemTemplate(RefineryMgr.m_Item_Refinery[key].m_Reward[index + 1]);
                if (RefineryMgr.rand.Next(100) < defaultprobability)
                {
                  int templateId = RefineryMgr.m_Item_Refinery[key].m_Reward[index + 1];
                  result = true;
                  return ItemMgr.FindItemTemplate(templateId);
                }
              }
            }
          }
        }
        else
          IsFormula = false;
      }
      return (ItemTemplateInfo) null;
    }

    public static ItemTemplateInfo RefineryTrend(
      int Operation,
      SqlDataProvider.Data.ItemInfo Item,
      ref bool result)
    {
      if (Item != null)
      {
        foreach (int key in RefineryMgr.m_Item_Refinery.Keys)
        {
          if (RefineryMgr.m_Item_Refinery[key].m_Reward.Contains(Item.TemplateID))
          {
            for (int index = 0; index < RefineryMgr.m_Item_Refinery[key].m_Reward.Count; ++index)
            {
              if (RefineryMgr.m_Item_Refinery[key].m_Reward[index] == Operation)
              {
                int templateId = RefineryMgr.m_Item_Refinery[key].m_Reward[index + 2];
                result = true;
                return ItemMgr.FindItemTemplate(templateId);
              }
            }
          }
        }
      }
      return (ItemTemplateInfo) null;
    }

    public static bool Reload()
    {
      try
      {
        Dictionary<int, RefineryInfo> dictionary1 = new Dictionary<int, RefineryInfo>();
        Dictionary<int, RefineryInfo> dictionary2 = RefineryMgr.LoadFromBD();
        if (dictionary2.Count > 0)
          Interlocked.Exchange<Dictionary<int, RefineryInfo>>(ref RefineryMgr.m_Item_Refinery, dictionary2);
        return true;
      }
      catch (Exception ex)
      {
        RefineryMgr.log.Error((object) "NPCInfoMgr", ex);
      }
      return false;
    }
  }
}
