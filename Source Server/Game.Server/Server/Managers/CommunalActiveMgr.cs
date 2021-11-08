// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.CommunalActiveMgr
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
  public class CommunalActiveMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<int, CommunalActiveAwardInfo> _communalActiveAwards;
    private static Dictionary<int, CommunalActiveExpInfo> _communalActiveExps;
    private static Dictionary<int, CommunalActiveInfo> _communalActives;
    private static ReaderWriterLock m_lock;
    private static ThreadSafeRandom rand;

    public static CommunalActiveInfo FindCommunalActive(int ActiveID)
    {
      if (CommunalActiveMgr._communalActives == null)
        CommunalActiveMgr.Init();
      CommunalActiveMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (CommunalActiveMgr._communalActives.ContainsKey(ActiveID))
          return CommunalActiveMgr._communalActives[ActiveID];
      }
      catch
      {
      }
      finally
      {
        CommunalActiveMgr.m_lock.ReleaseReaderLock();
      }
      return (CommunalActiveInfo) null;
    }

    public static List<CommunalActiveAwardInfo> FindCommunalAwards(
      int isArea)
    {
      if (CommunalActiveMgr._communalActiveAwards == null)
        CommunalActiveMgr.Init();
      List<CommunalActiveAwardInfo> communalActiveAwardInfoList = new List<CommunalActiveAwardInfo>();
      CommunalActiveMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        foreach (CommunalActiveAwardInfo communalActiveAwardInfo in CommunalActiveMgr._communalActiveAwards.Values)
        {
          if (communalActiveAwardInfo.IsArea == isArea)
            communalActiveAwardInfoList.Add(communalActiveAwardInfo);
        }
        return communalActiveAwardInfoList;
      }
      catch
      {
      }
      finally
      {
        CommunalActiveMgr.m_lock.ReleaseReaderLock();
      }
      return communalActiveAwardInfoList;
    }

    public static List<SqlDataProvider.Data.ItemInfo> GetAwardInfos(int type, int rank)
    {
      List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
      foreach (CommunalActiveAwardInfo communalAward in CommunalActiveMgr.FindCommunalAwards(type))
      {
        if (communalAward.RandID == rank)
        {
          SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(communalAward.TemplateID), communalAward.Count, 102);
          if (fromTemplate != null)
          {
            fromTemplate.Count = communalAward.Count;
            fromTemplate.IsBinds = communalAward.IsBind;
            fromTemplate.ValidDate = communalAward.ValidDate;
            fromTemplate.StrengthenLevel = communalAward.StrengthenLevel;
            fromTemplate.AttackCompose = communalAward.AttackCompose;
            fromTemplate.DefendCompose = communalAward.DefendCompose;
            fromTemplate.AgilityCompose = communalAward.AgilityCompose;
            fromTemplate.LuckCompose = communalAward.LuckCompose;
            itemInfoList.Add(fromTemplate);
          }
        }
      }
      return itemInfoList;
    }

    public static int GetGP(int level)
    {
      if (CommunalActiveMgr._communalActiveExps == null)
        CommunalActiveMgr.Init();
      if (CommunalActiveMgr._communalActiveExps.ContainsKey(level))
        return CommunalActiveMgr._communalActiveExps[level].Exp;
      return 0;
    }

    public static bool Init()
    {
      try
      {
        CommunalActiveMgr.m_lock = new ReaderWriterLock();
        CommunalActiveMgr._communalActives = new Dictionary<int, CommunalActiveInfo>();
        CommunalActiveMgr._communalActiveAwards = new Dictionary<int, CommunalActiveAwardInfo>();
        CommunalActiveMgr._communalActiveExps = new Dictionary<int, CommunalActiveExpInfo>();
        CommunalActiveMgr.rand = new ThreadSafeRandom();
        return CommunalActiveMgr.LoadData(CommunalActiveMgr._communalActives, CommunalActiveMgr._communalActiveAwards, CommunalActiveMgr._communalActiveExps);
      }
      catch (Exception ex)
      {
        if (CommunalActiveMgr.log.IsErrorEnabled)
          CommunalActiveMgr.log.Error((object) nameof (CommunalActiveMgr), ex);
        return false;
      }
    }

    private static bool LoadData(
      Dictionary<int, CommunalActiveInfo> CommunalActives,
      Dictionary<int, CommunalActiveAwardInfo> CommunalActiveAwards,
      Dictionary<int, CommunalActiveExpInfo> CommunalActiveExps)
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (CommunalActiveInfo communalActiveInfo in produceBussiness.GetAllCommunalActive())
        {
          if (!CommunalActives.ContainsKey(communalActiveInfo.ActiveID))
            CommunalActives.Add(communalActiveInfo.ActiveID, communalActiveInfo);
        }
        foreach (CommunalActiveAwardInfo communalActiveAwardInfo in produceBussiness.GetAllCommunalActiveAward())
        {
          if (!CommunalActiveAwards.ContainsKey(communalActiveAwardInfo.ID))
            CommunalActiveAwards.Add(communalActiveAwardInfo.ID, communalActiveAwardInfo);
        }
        foreach (CommunalActiveExpInfo communalActiveExpInfo in produceBussiness.GetAllCommunalActiveExp())
        {
          if (!CommunalActiveExps.ContainsKey(communalActiveExpInfo.Grade))
            CommunalActiveExps.Add(communalActiveExpInfo.Grade, communalActiveExpInfo);
        }
      }
      return true;
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, CommunalActiveInfo> CommunalActives = new Dictionary<int, CommunalActiveInfo>();
        Dictionary<int, CommunalActiveAwardInfo> CommunalActiveAwards = new Dictionary<int, CommunalActiveAwardInfo>();
        Dictionary<int, CommunalActiveExpInfo> CommunalActiveExps = new Dictionary<int, CommunalActiveExpInfo>();
        if (CommunalActiveMgr.LoadData(CommunalActives, CommunalActiveAwards, CommunalActiveExps))
        {
          CommunalActiveMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            CommunalActiveMgr._communalActives = CommunalActives;
            CommunalActiveMgr._communalActiveAwards = CommunalActiveAwards;
            CommunalActiveMgr._communalActiveExps = CommunalActiveExps;
            return true;
          }
          catch
          {
          }
          finally
          {
            CommunalActiveMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (CommunalActiveMgr.log.IsErrorEnabled)
          CommunalActiveMgr.log.Error((object) nameof (CommunalActiveMgr), ex);
      }
      return false;
    }

    public static void ResetEvent()
    {
    }
  }
}
