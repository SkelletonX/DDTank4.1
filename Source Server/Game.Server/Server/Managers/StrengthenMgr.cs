// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.StrengthenMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
  public class StrengthenMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    public static readonly List<double> RateItems = new List<double>()
    {
      0.75,
      3.0,
      12.0,
      48.0,
      240.0,
      768.0
    };
    public static readonly double VIPStrengthenEx = 0.3;
    public static readonly int[] StrengthenExp = new int[16]
    {
      0,
      10,
      50,
      150,
      350,
      700,
      1500,
      2300,
      3300,
      4500,
      6000,
      7500,
      9000,
      15000,
      25000,
      50000
    };
    private static Dictionary<int, StrengthenInfo> dictionary_0;
    private static Dictionary<int, StrengthenInfo> dictionary_1;
    private static Dictionary<int, StrengthenGoodsInfo> dictionary_2;
    private static ReaderWriterLock readerWriterLock_0;
    private static ThreadSafeRandom threadSafeRandom_0;

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, StrengthenInfo> HUE9g0WPOJWvlUtLFD7 = new Dictionary<int, StrengthenInfo>();
        Dictionary<int, StrengthenInfo> akCxYTWS0RdWhUEgYeQ = new Dictionary<int, StrengthenInfo>();
        Dictionary<int, StrengthenGoodsInfo> dictionary = new Dictionary<int, StrengthenGoodsInfo>();
        if (StrengthenMgr.smethod_0(HUE9g0WPOJWvlUtLFD7, akCxYTWS0RdWhUEgYeQ))
        {
          StrengthenMgr.readerWriterLock_0.AcquireWriterLock(-1);
          try
          {
            StrengthenMgr.dictionary_0 = HUE9g0WPOJWvlUtLFD7;
            StrengthenMgr.dictionary_1 = akCxYTWS0RdWhUEgYeQ;
            StrengthenMgr.dictionary_2 = dictionary;
            return true;
          }
          catch
          {
          }
          finally
          {
            StrengthenMgr.readerWriterLock_0.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (StrengthenMgr.log.IsErrorEnabled)
          StrengthenMgr.log.Error((object) nameof (StrengthenMgr), ex);
      }
      return false;
    }

    public static bool Init()
    {
      try
      {
        StrengthenMgr.readerWriterLock_0 = new ReaderWriterLock();
        StrengthenMgr.dictionary_0 = new Dictionary<int, StrengthenInfo>();
        StrengthenMgr.dictionary_1 = new Dictionary<int, StrengthenInfo>();
        StrengthenMgr.dictionary_2 = new Dictionary<int, StrengthenGoodsInfo>();
        StrengthenMgr.threadSafeRandom_0 = new ThreadSafeRandom();
        return StrengthenMgr.smethod_0(StrengthenMgr.dictionary_0, StrengthenMgr.dictionary_1);
      }
      catch (Exception ex)
      {
        if (StrengthenMgr.log.IsErrorEnabled)
          StrengthenMgr.log.Error((object) nameof (StrengthenMgr), ex);
        return false;
      }
    }

    private static bool smethod_0(
      Dictionary<int, StrengthenInfo> HUE9g0WPOJWvlUtLFD7,
      Dictionary<int, StrengthenInfo> akCxYTWS0RdWhUEgYeQ)
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        StrengthenInfo[] allStrengthen = produceBussiness.GetAllStrengthen();
        StrengthenInfo[] refineryStrengthen = produceBussiness.GetAllRefineryStrengthen();
        StrengthenGoodsInfo[] strengthenGoodsInfo1 = produceBussiness.GetAllStrengthenGoodsInfo();
        foreach (StrengthenInfo strengthenInfo in allStrengthen)
        {
          if (!HUE9g0WPOJWvlUtLFD7.ContainsKey(strengthenInfo.StrengthenLevel))
            HUE9g0WPOJWvlUtLFD7.Add(strengthenInfo.StrengthenLevel, strengthenInfo);
        }
        foreach (StrengthenInfo strengthenInfo in refineryStrengthen)
        {
          if (!akCxYTWS0RdWhUEgYeQ.ContainsKey(strengthenInfo.StrengthenLevel))
            akCxYTWS0RdWhUEgYeQ.Add(strengthenInfo.StrengthenLevel, strengthenInfo);
        }
        foreach (StrengthenGoodsInfo strengthenGoodsInfo2 in strengthenGoodsInfo1)
        {
          if (!StrengthenMgr.dictionary_2.ContainsKey(strengthenGoodsInfo2.ID))
            StrengthenMgr.dictionary_2.Add(strengthenGoodsInfo2.ID, strengthenGoodsInfo2);
        }
      }
      return true;
    }

    public static StrengthenInfo FindStrengthenInfo(int level)
    {
      StrengthenMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        if (StrengthenMgr.dictionary_0.ContainsKey(level))
          return StrengthenMgr.dictionary_0[level];
      }
      catch
      {
      }
      finally
      {
        StrengthenMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return (StrengthenInfo) null;
    }

    public static StrengthenInfo FindRefineryStrengthenInfo(int level)
    {
      StrengthenMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        if (StrengthenMgr.dictionary_1.ContainsKey(level))
          return StrengthenMgr.dictionary_1[level];
      }
      catch
      {
      }
      finally
      {
        StrengthenMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return (StrengthenInfo) null;
    }

    public static StrengthenGoodsInfo FindStrengthenGoodsInfo(
      int level,
      int templateId)
    {
      foreach (StrengthenGoodsInfo strengthenGoodsInfo in StrengthenMgr.dictionary_2.Values)
      {
        if (strengthenGoodsInfo.Level == level && templateId == strengthenGoodsInfo.CurrentEquip)
          return strengthenGoodsInfo;
      }
      return (StrengthenGoodsInfo) null;
    }

    public static StrengthenGoodsInfo FindTransferInfo(int level, int templateId)
    {
      foreach (StrengthenGoodsInfo strengthenGoodsInfo in StrengthenMgr.dictionary_2.Values)
      {
        if (strengthenGoodsInfo.Level == level && templateId == strengthenGoodsInfo.CurrentEquip)
          return strengthenGoodsInfo;
      }
      return (StrengthenGoodsInfo) null;
    }

    public static StrengthenGoodsInfo FindTransferInfo(int templateId)
    {
      foreach (StrengthenGoodsInfo strengthenGoodsInfo in StrengthenMgr.dictionary_2.Values)
      {
        if (templateId == strengthenGoodsInfo.GainEquip || templateId == strengthenGoodsInfo.CurrentEquip)
          return strengthenGoodsInfo;
      }
      return (StrengthenGoodsInfo) null;
    }

    public static StrengthenGoodsInfo FindRealStrengthenGoodInfo(
      int level,
      int templateid)
    {
      StrengthenGoodsInfo transferInfo = StrengthenMgr.FindTransferInfo(templateid);
      if (transferInfo != null)
        return StrengthenMgr.FindStrengthenGoodsInfo(level, transferInfo.OrginEquip);
      return (StrengthenGoodsInfo) null;
    }

    public static void InheritTransferProperty(
      ref SqlDataProvider.Data.ItemInfo itemZero,
      ref SqlDataProvider.Data.ItemInfo itemOne,
      bool tranHole,
      bool tranHoleFivSix)
    {
      int hole1_1 = itemZero.Hole1;
      int hole2_1 = itemZero.Hole2;
      int hole3_1 = itemZero.Hole3;
      int hole4_1 = itemZero.Hole4;
      int hole5_1 = itemZero.Hole5;
      int hole6_1 = itemZero.Hole6;
      int hole5Exp1 = itemZero.Hole5Exp;
      int hole5Level1 = itemZero.Hole5Level;
      int hole6Exp1 = itemZero.Hole6Exp;
      int hole6Level1 = itemZero.Hole6Level;
      int attackCompose1 = itemZero.AttackCompose;
      int defendCompose1 = itemZero.DefendCompose;
      int agilityCompose1 = itemZero.AgilityCompose;
      int luckCompose1 = itemZero.LuckCompose;
      int strengthenLevel1 = itemZero.StrengthenLevel;
      int strengthenExp1 = itemZero.StrengthenExp;
      int num1 = itemZero.isGold ? 1 : 0;
      int goldValidDate1 = itemZero.goldValidDate;
      DateTime goldBeginTime1 = itemZero.goldBeginTime;
      int hole1_2 = itemOne.Hole1;
      int hole2_2 = itemOne.Hole2;
      int hole3_2 = itemOne.Hole3;
      int hole4_2 = itemOne.Hole4;
      int hole5_2 = itemOne.Hole5;
      int hole6_2 = itemOne.Hole6;
      int hole5Exp2 = itemOne.Hole5Exp;
      int hole5Level2 = itemOne.Hole5Level;
      int hole6Exp2 = itemOne.Hole6Exp;
      int hole6Level2 = itemOne.Hole6Level;
      int attackCompose2 = itemOne.AttackCompose;
      int defendCompose2 = itemOne.DefendCompose;
      int agilityCompose2 = itemOne.AgilityCompose;
      int luckCompose2 = itemOne.LuckCompose;
      int strengthenLevel2 = itemOne.StrengthenLevel;
      int strengthenExp2 = itemOne.StrengthenExp;
      int num2 = itemOne.isGold ? 1 : 0;
      int goldValidDate2 = itemOne.goldValidDate;
      DateTime goldBeginTime2 = itemOne.goldBeginTime;
      if (tranHole)
      {
        itemOne.Hole1 = hole1_1;
        itemZero.Hole1 = hole1_2;
        itemOne.Hole2 = hole2_1;
        itemZero.Hole2 = hole2_2;
        itemOne.Hole3 = hole3_1;
        itemZero.Hole3 = hole3_2;
        itemOne.Hole4 = hole4_1;
        itemZero.Hole4 = hole4_2;
      }
      if (tranHoleFivSix)
      {
        itemOne.Hole5 = hole5_1;
        itemZero.Hole5 = hole5_2;
        itemOne.Hole6 = hole6_1;
        itemZero.Hole6 = hole6_2;
      }
      itemOne.Hole5Exp = hole5Exp1;
      itemZero.Hole5Exp = hole5Exp2;
      itemOne.Hole5Level = hole5Level1;
      itemZero.Hole5Level = hole5Level2;
      itemOne.Hole6Exp = hole6Exp1;
      itemZero.Hole6Exp = hole6Exp2;
      itemOne.Hole6Level = hole6Level1;
      itemZero.Hole6Level = hole6Level2;
      itemZero.StrengthenLevel = strengthenLevel2;
      itemOne.StrengthenLevel = strengthenLevel1;
      itemZero.StrengthenExp = strengthenExp2;
      itemOne.StrengthenExp = strengthenExp1;
      itemZero.AttackCompose = attackCompose2;
      itemOne.AttackCompose = attackCompose1;
      itemZero.DefendCompose = defendCompose2;
      itemOne.DefendCompose = defendCompose1;
      itemZero.LuckCompose = luckCompose2;
      itemOne.LuckCompose = luckCompose1;
      itemZero.AgilityCompose = agilityCompose2;
      itemOne.AgilityCompose = agilityCompose1;
      if (itemZero.IsBinds || itemOne.IsBinds)
      {
        itemOne.IsBinds = true;
        itemZero.IsBinds = true;
      }
      itemZero.goldBeginTime = goldBeginTime2;
      itemOne.goldBeginTime = goldBeginTime1;
      itemZero.goldValidDate = goldValidDate2;
      itemOne.goldValidDate = goldValidDate1;
    }

    public static ItemTemplateInfo GetRealWeaponTemplate(SqlDataProvider.Data.ItemInfo item)
    {
      ItemTemplateInfo itemTemplateInfo = (ItemTemplateInfo) null;
      if (item.Template.CategoryID == 7)
      {
        string str = "";
        if (item.StrengthenLevel >= 10 && item.StrengthenLevel <= 12)
        {
          switch (item.StrengthenLevel)
          {
            case 10:
            case 11:
              str = "1";
              break;
            case 12:
              str = "2";
              break;
          }
        }
        itemTemplateInfo = ItemMgr.FindItemTemplate(int.Parse(item.TemplateID.ToString().Substring(0, 4) + str));
      }
      return itemTemplateInfo;
    }

    public static int GetNeedRate(SqlDataProvider.Data.ItemInfo mainItem)
    {
      int num = 0;
      StrengthenInfo strengthenInfo = StrengthenMgr.FindStrengthenInfo(mainItem.StrengthenLevel + 1);
      switch (mainItem.Template.CategoryID)
      {
        case 1:
          num = strengthenInfo.Rock1;
          break;
        case 5:
          num = strengthenInfo.Rock2;
          break;
        case 7:
          num = strengthenInfo.Rock;
          break;
        case 17:
          num = strengthenInfo.Rock3;
          break;
      }
      return num;
    }
  }
}
