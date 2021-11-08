// Decompiled with JetBrains decompiler
// Type: Game.Logic.DropInventory
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Bussiness.Managers;
using Bussiness.Protocol;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Game.Logic
{
  public class DropInventory
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ThreadSafeRandom random = new ThreadSafeRandom();
    public static int roundDate = 0;

    public static bool AnswerDrop(int answerId, ref List<SqlDataProvider.Data.ItemInfo> info)
    {
      int dropCondiction = DropInventory.GetDropCondiction(eDropType.Answer, answerId.ToString(), "0");
      if (dropCondiction > 0)
      {
        List<SqlDataProvider.Data.ItemInfo> itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
        if (DropInventory.GetDropItems(eDropType.Answer, dropCondiction, ref itemInfos))
        {
          info = itemInfos ?? (List<SqlDataProvider.Data.ItemInfo>) null;
          return true;
        }
      }
      return false;
    }

    public static bool BossDrop(int missionId, ref List<SqlDataProvider.Data.ItemInfo> info)
    {
      int dropCondiction = DropInventory.GetDropCondiction(eDropType.Boss, missionId.ToString(), "0");
      if (dropCondiction > 0)
      {
        List<SqlDataProvider.Data.ItemInfo> itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
        if (DropInventory.GetDropItems(eDropType.Boss, dropCondiction, ref itemInfos))
        {
          info = itemInfos ?? (List<SqlDataProvider.Data.ItemInfo>) null;
          return true;
        }
      }
      return false;
    }

    public static bool BoxDrop(eRoomType e, ref List<SqlDataProvider.Data.ItemInfo> info)
    {
      int dropCondiction = DropInventory.GetDropCondiction(eDropType.Box, ((int) e).ToString(), "0");
      if (dropCondiction > 0)
      {
        List<SqlDataProvider.Data.ItemInfo> itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
        if (DropInventory.GetDropItems(eDropType.Box, dropCondiction, ref itemInfos))
        {
          info = itemInfos ?? (List<SqlDataProvider.Data.ItemInfo>) null;
          return true;
        }
      }
      return false;
    }

    public static bool CardDrop(eRoomType e, ref List<SqlDataProvider.Data.ItemInfo> info)
    {
      int dropCondiction = DropInventory.GetDropCondiction(eDropType.Cards, ((int) e).ToString(), "0");
      if (dropCondiction > 0)
      {
        List<SqlDataProvider.Data.ItemInfo> itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
        if (DropInventory.GetDropItems(eDropType.Cards, dropCondiction, ref itemInfos))
        {
          info = itemInfos ?? (List<SqlDataProvider.Data.ItemInfo>) null;
          return true;
        }
      }
      return false;
    }

    public static bool CopyAllDrop(int copyId, ref List<SqlDataProvider.Data.ItemInfo> info)
    {
      int dropCondiction = DropInventory.GetDropCondiction(eDropType.Copy, copyId.ToString(), "0");
      if (dropCondiction > 0)
      {
        List<SqlDataProvider.Data.ItemInfo> itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
        if (DropInventory.GetAllDropItems(eDropType.Copy, dropCondiction, ref itemInfos))
        {
          info = itemInfos ?? (List<SqlDataProvider.Data.ItemInfo>) null;
          return true;
        }
      }
      return false;
    }

    public static bool CopyDrop(int copyId, int user, ref List<SqlDataProvider.Data.ItemInfo> info)
    {
      int dropCondiction = DropInventory.GetDropCondiction(eDropType.Copy, copyId.ToString(), user.ToString());
      if (dropCondiction > 0)
      {
        List<SqlDataProvider.Data.ItemInfo> itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
        if (DropInventory.GetDropItems(eDropType.Copy, dropCondiction, ref itemInfos))
        {
          info = itemInfos ?? (List<SqlDataProvider.Data.ItemInfo>) null;
          return true;
        }
      }
      return false;
    }

    public static List<SqlDataProvider.Data.ItemInfo> CopySystemDrop(
      int copyId,
      int OpenCount)
    {
      int int32_1 = Convert.ToInt32((double) OpenCount * 0.1);
      int int32_2 = Convert.ToInt32((double) OpenCount * 0.3);
      int num = OpenCount - int32_1 - int32_2;
      List<SqlDataProvider.Data.ItemInfo> list = new List<SqlDataProvider.Data.ItemInfo>();
      List<SqlDataProvider.Data.ItemInfo> itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
      int dropCondiction1 = DropInventory.GetDropCondiction(eDropType.Copy, copyId.ToString(), "2");
      if (dropCondiction1 > 0)
      {
        for (int index = 0; index < int32_1; ++index)
        {
          if (DropInventory.GetDropItems(eDropType.Copy, dropCondiction1, ref itemInfos))
          {
            list.Add(itemInfos[0]);
            itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
          }
        }
      }
      int dropCondiction2 = DropInventory.GetDropCondiction(eDropType.Copy, copyId.ToString(), "3");
      if (dropCondiction2 > 0)
      {
        for (int index = 0; index < int32_2; ++index)
        {
          if (DropInventory.GetDropItems(eDropType.Copy, dropCondiction2, ref itemInfos))
          {
            list.Add(itemInfos[0]);
            itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
          }
        }
      }
      int dropCondiction3 = DropInventory.GetDropCondiction(eDropType.Copy, copyId.ToString(), "4");
      if (dropCondiction3 > 0)
      {
        for (int index = 0; index < num; ++index)
        {
          if (DropInventory.GetDropItems(eDropType.Copy, dropCondiction3, ref itemInfos))
          {
            list.Add(itemInfos[0]);
            itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
          }
        }
      }
      return DropInventory.RandomSortList(list);
    }

    public static bool FireDrop(eRoomType e, ref List<SqlDataProvider.Data.ItemInfo> info)
    {
      int dropCondiction = DropInventory.GetDropCondiction(eDropType.Fire, ((int) e).ToString(), "0");
      if (dropCondiction > 0)
      {
        List<SqlDataProvider.Data.ItemInfo> itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
        if (DropInventory.GetDropItems(eDropType.Fire, dropCondiction, ref itemInfos))
        {
          info = itemInfos ?? (List<SqlDataProvider.Data.ItemInfo>) null;
          return true;
        }
      }
      return false;
    }

    private static bool GetAllDropItems(eDropType type, int dropId, ref List<SqlDataProvider.Data.ItemInfo> itemInfos)
    {
      if ((uint) dropId > 0U)
      {
        try
        {
          int num1 = 1;
          List<DropItem> dropItem1 = DropMgr.FindDropItem(dropId);
          int maxRound = ThreadSafeRandom.NextStatic(dropItem1.Select<DropItem, int>((Func<DropItem, int>) (s => s.Random)).Max());
          int num2 = dropItem1.Where<DropItem>((Func<DropItem, bool>) (s => s.Random >= maxRound)).ToList<DropItem>().Count<DropItem>();
          if (num2 == 0)
            return false;
          int count1 = num1 > num2 ? num2 : num1;
          DropInventory.GetRandomUnrepeatArray(0, num2 - 1, count1);
          foreach (DropItem dropItem2 in dropItem1)
          {
            int count2 = ThreadSafeRandom.NextStatic(dropItem2.BeginData, dropItem2.EndData);
            ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(dropItem2.ItemId);
            SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(itemTemplate, count2, 101);
            if (fromTemplate != null)
            {
              fromTemplate.IsBinds = dropItem2.IsBind;
              fromTemplate.ValidDate = dropItem2.ValueDate;
              fromTemplate.IsTips = dropItem2.IsTips;
              fromTemplate.IsLogs = dropItem2.IsLogs;
              if (itemInfos == null)
                itemInfos = new List<SqlDataProvider.Data.ItemInfo>();
              if (DropInfoMgr.CanDrop(itemTemplate.TemplateID))
                itemInfos.Add(fromTemplate);
            }
          }
          return true;
        }
        catch
        {
          if (DropInventory.log.IsErrorEnabled)
            DropInventory.log.Error((object) ("Drop Error：" + (object) type + " dropId " + (object) dropId));
        }
      }
      return false;
    }

    public static bool GetDrop(int copyId, int user, ref List<SqlDataProvider.Data.ItemInfo> info)
    {
      int dropCondiction = DropInventory.GetDropCondiction(eDropType.Trminhpc, copyId.ToString(), user.ToString());
      if (dropCondiction > 0)
      {
        List<SqlDataProvider.Data.ItemInfo> itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
        if (DropInventory.GetDropItems(eDropType.Trminhpc, dropCondiction, ref itemInfos))
        {
          info = itemInfos ?? (List<SqlDataProvider.Data.ItemInfo>) null;
          return true;
        }
      }
      return false;
    }

    private static int GetDropCondiction(eDropType type, string para1, string para2)
    {
      try
      {
        return DropMgr.FindCondiction(type, para1, para2);
      }
      catch (Exception ex)
      {
        if (DropInventory.log.IsErrorEnabled)
          DropInventory.log.Error((object) ("Drop Error：" + (object) type + " @ " + (object) ex));
      }
      return 0;
    }

    private static bool GetDropItems(eDropType type, int dropId, ref List<SqlDataProvider.Data.ItemInfo> itemInfos)
    {
      if ((uint) dropId > 0U)
      {
        try
        {
          int num1 = 1;
          List<DropItem> dropItem = DropMgr.FindDropItem(dropId);
          int maxRound = ThreadSafeRandom.NextStatic(dropItem.Select<DropItem, int>((Func<DropItem, int>) (s => s.Random)).Max());
          List<DropItem> list = dropItem.Where<DropItem>((Func<DropItem, bool>) (s => s.Random >= maxRound)).ToList<DropItem>();
          int num2 = list.Count<DropItem>();
          if (num2 == 0)
            return false;
          int count1 = num1 > num2 ? num2 : num1;
          foreach (int randomUnrepeat in DropInventory.GetRandomUnrepeatArray(0, num2 - 1, count1))
          {
            int count2 = ThreadSafeRandom.NextStatic(list[randomUnrepeat].BeginData, list[randomUnrepeat].EndData);
            ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(list[randomUnrepeat].ItemId);
            SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(itemTemplate, count2, 101);
            if (fromTemplate != null)
            {
              fromTemplate.IsBinds = list[randomUnrepeat].IsBind;
              fromTemplate.ValidDate = list[randomUnrepeat].ValueDate;
              fromTemplate.IsTips = list[randomUnrepeat].IsTips;
              fromTemplate.IsLogs = list[randomUnrepeat].IsLogs;
              if (itemInfos == null)
                itemInfos = new List<SqlDataProvider.Data.ItemInfo>();
              if (DropInfoMgr.CanDrop(itemTemplate.TemplateID))
                itemInfos.Add(fromTemplate);
            }
          }
          return true;
        }
        catch
        {
          if (DropInventory.log.IsErrorEnabled)
            DropInventory.log.Error((object) ("Drop Error：" + (object) type + " dropId " + (object) dropId));
        }
      }
      return false;
    }

    public static int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
    {
      int[] numArray = new int[count];
      for (int index1 = 0; index1 < count; ++index1)
      {
        int num1 = ThreadSafeRandom.NextStatic(minValue, maxValue + 1);
        int num2 = 0;
        for (int index2 = 0; index2 < index1; ++index2)
        {
          if (numArray[index2] == num1)
            ++num2;
        }
        if (num2 == 0)
          numArray[index1] = num1;
        else
          --index1;
      }
      return numArray;
    }

    public static bool NPCDrop(int dropId, ref List<SqlDataProvider.Data.ItemInfo> info)
    {
      if (dropId > 0)
      {
        List<SqlDataProvider.Data.ItemInfo> itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
        if (DropInventory.GetDropItems(eDropType.NPC, dropId, ref itemInfos))
        {
          info = itemInfos ?? (List<SqlDataProvider.Data.ItemInfo>) null;
          return true;
        }
      }
      return false;
    }

    public static bool PvEQuestsDrop(int npcId, ref List<SqlDataProvider.Data.ItemInfo> info)
    {
      int dropCondiction = DropInventory.GetDropCondiction(eDropType.PveQuests, npcId.ToString(), "0");
      if (dropCondiction > 0)
      {
        List<SqlDataProvider.Data.ItemInfo> itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
        if (DropInventory.GetDropItems(eDropType.PveQuests, dropCondiction, ref itemInfos))
        {
          info = itemInfos ?? (List<SqlDataProvider.Data.ItemInfo>) null;
          return true;
        }
      }
      return false;
    }

    public static bool PvPQuestsDrop(eRoomType e, bool playResult, ref List<SqlDataProvider.Data.ItemInfo> info)
    {
      int dropCondiction = DropInventory.GetDropCondiction(eDropType.PvpQuests, ((int) e).ToString(), Convert.ToInt16(playResult).ToString());
      if (dropCondiction > 0)
      {
        List<SqlDataProvider.Data.ItemInfo> itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
        if (DropInventory.GetDropItems(eDropType.PvpQuests, dropCondiction, ref itemInfos))
        {
          info = itemInfos ?? (List<SqlDataProvider.Data.ItemInfo>) null;
          return true;
        }
      }
      return false;
    }

    public static List<SqlDataProvider.Data.ItemInfo> RandomSortList(List<SqlDataProvider.Data.ItemInfo> list)
    {
      return list.OrderBy<SqlDataProvider.Data.ItemInfo, int>((Func<SqlDataProvider.Data.ItemInfo, int>) (key => DropInventory.random.Next())).ToList<SqlDataProvider.Data.ItemInfo>();
    }

    public static bool RetrieveDrop(int user, ref List<SqlDataProvider.Data.ItemInfo> info)
    {
      int dropCondiction = DropInventory.GetDropCondiction(eDropType.Retrieve, user.ToString(), "0");
      if (dropCondiction > 0)
      {
        List<SqlDataProvider.Data.ItemInfo> itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
        if (DropInventory.GetDropItems(eDropType.Retrieve, dropCondiction, ref itemInfos))
        {
          info = itemInfos ?? (List<SqlDataProvider.Data.ItemInfo>) null;
          return true;
        }
      }
      return false;
    }

    public static bool SpecialDrop(int missionId, int boxType, ref List<SqlDataProvider.Data.ItemInfo> info)
    {
      int dropCondiction = DropInventory.GetDropCondiction(eDropType.Special, missionId.ToString(), boxType.ToString());
      if (dropCondiction > 0)
      {
        List<SqlDataProvider.Data.ItemInfo> itemInfos = (List<SqlDataProvider.Data.ItemInfo>) null;
        if (DropInventory.GetDropItems(eDropType.Special, dropCondiction, ref itemInfos))
        {
          info = itemInfos ?? (List<SqlDataProvider.Data.ItemInfo>) null;
          return true;
        }
      }
      return false;
    }

    public static bool FightLabUserDrop(int copyId, ref List<SqlDataProvider.Data.ItemInfo> info)
    {
      int dropCondiction = DropInventory.GetDropCondiction(eDropType.FightLab, copyId.ToString(), "1");
      bool flag;
      if (dropCondiction > 0)
      {
        List<DropItem> dropItem = DropMgr.FindDropItem(dropCondiction);
        for (int index = 0; index < dropItem.Count; ++index)
        {
          int count = DropInventory.random.Next(dropItem[index].BeginData, dropItem[index].EndData);
          SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(dropItem[index].ItemId), count, copyId);
          if (fromTemplate != null)
          {
            fromTemplate.IsBinds = dropItem[index].IsBind;
            fromTemplate.ValidDate = dropItem[index].ValueDate;
            fromTemplate.IsTips = dropItem[index].IsTips;
            fromTemplate.IsLogs = dropItem[index].IsLogs;
            if (info == null)
              info = new List<SqlDataProvider.Data.ItemInfo>();
            info.Add(fromTemplate);
          }
        }
        flag = true;
      }
      else
      {
          Console.WriteLine("Drop Não Encontrado: MissionID:{0} CondiionType:{1}", copyId, (int)eDropType.FightLab);
          flag = false;
      }
      return flag;
    }
  }
}
