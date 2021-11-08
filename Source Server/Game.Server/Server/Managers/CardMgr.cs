// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.CardMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
  public class CardMgr
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<int, CardUpdateConditionInfo> dictionary_0;
    private static List<CardUpdateInfo> list_0;
    private static ReaderWriterLock readerWriterLock_0;
    private static ThreadSafeRandom threadSafeRandom_0;

    public static bool Init()
    {
      try
      {
        CardMgr.readerWriterLock_0 = new ReaderWriterLock();
        CardMgr.dictionary_0 = new Dictionary<int, CardUpdateConditionInfo>();
        CardMgr.list_0 = new List<CardUpdateInfo>();
        CardMgr.threadSafeRandom_0 = new ThreadSafeRandom();
        return CardMgr.ReLoad();
      }
      catch (Exception ex)
      {
        if (CardMgr.ilog_0.IsErrorEnabled)
          CardMgr.ilog_0.Error((object) nameof (CardMgr), ex);
        return false;
      }
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, CardUpdateConditionInfo> PpN9moWQoUbKcnEHdIE = new Dictionary<int, CardUpdateConditionInfo>();
        List<CardUpdateInfo> kCtWs5WWQ5Yo0KY8qBs = new List<CardUpdateInfo>();
        if (CardMgr.smethod_0(PpN9moWQoUbKcnEHdIE, kCtWs5WWQ5Yo0KY8qBs))
        {
          CardMgr.readerWriterLock_0.AcquireWriterLock(-1);
          try
          {
            CardMgr.dictionary_0 = PpN9moWQoUbKcnEHdIE;
            CardMgr.list_0 = kCtWs5WWQ5Yo0KY8qBs;
            return true;
          }
          catch
          {
          }
          finally
          {
            CardMgr.readerWriterLock_0.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (CardMgr.ilog_0.IsErrorEnabled)
          CardMgr.ilog_0.Error((object) nameof (CardMgr), ex);
      }
      return false;
    }

    private static bool smethod_0(
      Dictionary<int, CardUpdateConditionInfo> PpN9moWQoUbKcnEHdIE,
      List<CardUpdateInfo> kCtWs5WWQ5Yo0KY8qBs)
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        CardUpdateConditionInfo[] cardUpdateCondition = produceBussiness.GetAllCardUpdateCondition();
        CardUpdateInfo[] allCardUpdateInfo = produceBussiness.GetAllCardUpdateInfo();
        foreach (CardUpdateConditionInfo updateConditionInfo in cardUpdateCondition)
        {
          if (!PpN9moWQoUbKcnEHdIE.ContainsKey(updateConditionInfo.Level))
            PpN9moWQoUbKcnEHdIE.Add(updateConditionInfo.Level, updateConditionInfo);
        }
        foreach (CardUpdateInfo cardUpdateInfo in allCardUpdateInfo)
          kCtWs5WWQ5Yo0KY8qBs.Add(cardUpdateInfo);
      }
      return true;
    }

    public static CardUpdateConditionInfo GetCardUpdateCondition(int level)
    {
      if (CardMgr.dictionary_0.ContainsKey(level))
        return CardMgr.dictionary_0[level];
      return (CardUpdateConditionInfo) null;
    }

    public static CardUpdateInfo GetCardUpdateInfo(int templateId, int level)
    {
      foreach (CardUpdateInfo cardUpdateInfo in CardMgr.list_0)
      {
        if (cardUpdateInfo.Id == templateId && cardUpdateInfo.Level == level)
          return cardUpdateInfo;
      }
      return (CardUpdateInfo) null;
    }

    public static int MaxLevel()
    {
      return CardMgr.dictionary_0.Count;
    }
  }
}
