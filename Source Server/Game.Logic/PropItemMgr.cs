// Decompiled with JetBrains decompiler
// Type: Game.Logic.PropItemMgr
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Game.Logic
{
  public class PropItemMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    public static int[] PropBag = new int[22]
    {
      10001,
      10002,
      10003,
      10004,
      10005,
      10006,
      10007,
      10008,
      10009,
      10010,
      10011,
      10012,
      10013,
      10014,
      10015,
      10016,
      10017,
      10018,
      10019,
      10020,
      10021,
      10022
    };
    public static int[] PropFightBag = new int[22]
    {
      10001,
      10002,
      10003,
      10004,
      10005,
      10006,
      10007,
      10008,
      10009,
      10010,
      10011,
      10012,
      10013,
      10014,
      10015,
      10016,
      10017,
      10018,
      10019,
      10020,
      10021,
      10022
    };
    private static ThreadSafeRandom random = new ThreadSafeRandom();
    private static Dictionary<int, ItemTemplateInfo> _allProp;
    private static ReaderWriterLock m_lock;

    public static ItemTemplateInfo FindAllProp(int id)
    {
      PropItemMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (PropItemMgr._allProp.ContainsKey(id))
          return PropItemMgr._allProp[id];
      }
      catch
      {
      }
      finally
      {
        PropItemMgr.m_lock.ReleaseReaderLock();
      }
      return (ItemTemplateInfo) null;
    }

    public static ItemTemplateInfo FindFightingProp(int id)
    {
      PropItemMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (!((IEnumerable<int>) PropItemMgr.PropBag).Contains<int>(id) || !PropItemMgr._allProp.ContainsKey(id))
          return (ItemTemplateInfo) null;
        return PropItemMgr._allProp[id];
      }
      catch
      {
      }
      finally
      {
        PropItemMgr.m_lock.ReleaseReaderLock();
      }
      return (ItemTemplateInfo) null;
    }

    public static bool Init()
    {
      try
      {
        PropItemMgr.m_lock = new ReaderWriterLock();
        PropItemMgr._allProp = new Dictionary<int, ItemTemplateInfo>();
        return PropItemMgr.LoadProps(PropItemMgr._allProp);
      }
      catch (Exception ex)
      {
        if (PropItemMgr.log.IsErrorEnabled)
          PropItemMgr.log.Error((object) "InitProps", ex);
        return false;
      }
    }

    private static bool LoadProps(Dictionary<int, ItemTemplateInfo> allProp)
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (ItemTemplateInfo itemTemplateInfo in produceBussiness.GetSingleCategory(10))
          allProp.Add(itemTemplateInfo.TemplateID, itemTemplateInfo);
      }
      return true;
    }

    public static bool Reload()
    {
      try
      {
        Dictionary<int, ItemTemplateInfo> allProp = new Dictionary<int, ItemTemplateInfo>();
        if (PropItemMgr.LoadProps(allProp))
        {
          PropItemMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            PropItemMgr._allProp = allProp;
            return true;
          }
          catch
          {
          }
          finally
          {
            PropItemMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (PropItemMgr.log.IsErrorEnabled)
          PropItemMgr.log.Error((object) "ReloadProps", ex);
      }
      return false;
    }
  }
}
