// Decompiled with JetBrains decompiler
// Type: Game.Logic.TangCapVipMgr
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Logic
{
  public class TangCapVipMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<int, TangCapVip> _tangCapVip;
    private static ReaderWriterLock m_lock;

    public static bool Init()
    {
      try
      {
        TangCapVipMgr.m_lock = new ReaderWriterLock();
        TangCapVipMgr._tangCapVip = new Dictionary<int, TangCapVip>();
        return TangCapVipMgr.LoadTangCapMgr(TangCapVipMgr._tangCapVip);
      }
      catch (Exception ex)
      {
        if (TangCapVipMgr.log.IsErrorEnabled)
          TangCapVipMgr.log.Error((object) "TangCapVipInfoMgr", ex);
        return false;
      }
    }

    public static TangCapVip GetTangCapProperty(int level)
    {
      if (TangCapVipMgr._tangCapVip == null)
        TangCapVipMgr.Init();
      TangCapVipMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (TangCapVipMgr._tangCapVip.ContainsKey(level))
          return TangCapVipMgr._tangCapVip[level];
      }
      catch
      {
      }
      finally
      {
        TangCapVipMgr.m_lock.ReleaseReaderLock();
      }
      return (TangCapVip) null;
    }

    private static bool LoadTangCapMgr(Dictionary<int, TangCapVip> PetFightProp)
    {
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        foreach (TangCapVip tangCapVip in playerBussiness.GetAllTangCap())
        {
          if (!PetFightProp.ContainsKey(tangCapVip.ID))
            PetFightProp.Add(tangCapVip.ID, tangCapVip);
        }
      }
      return true;
    }
  }
}
