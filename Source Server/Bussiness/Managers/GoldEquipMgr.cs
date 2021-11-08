// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.GoldEquipMgr
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bussiness.Managers
{
  public class GoldEquipMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<int, GoldEquipTemplateInfo> dictionary_0;

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, GoldEquipTemplateInfo> infos = new Dictionary<int, GoldEquipTemplateInfo>();
        if (GoldEquipMgr.LoadItem(infos))
        {
          try
          {
            GoldEquipMgr.dictionary_0 = infos;
            return true;
          }
          catch
          {
          }
        }
      }
      catch (Exception ex)
      {
        if (GoldEquipMgr.log.IsErrorEnabled)
          GoldEquipMgr.log.Error((object) nameof (ReLoad), ex);
      }
      return false;
    }

    public static bool Init()
    {
      try
      {
        GoldEquipMgr.dictionary_0 = new Dictionary<int, GoldEquipTemplateInfo>();
        return GoldEquipMgr.LoadItem(GoldEquipMgr.dictionary_0);
      }
      catch (Exception ex)
      {
        if (GoldEquipMgr.log.IsErrorEnabled)
          GoldEquipMgr.log.Error((object) nameof (Init), ex);
        return false;
      }
    }

    public static bool LoadItem(Dictionary<int, GoldEquipTemplateInfo> infos)
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (GoldEquipTemplateInfo equipTemplateInfo in produceBussiness.GetAllGoldEquipTemplateLoad())
        {
          if (!infos.Keys.Contains<int>(equipTemplateInfo.ID))
            infos.Add(equipTemplateInfo.ID, equipTemplateInfo);
        }
      }
      return true;
    }

    public static GoldEquipTemplateInfo FindGoldEquipByTemplate(int templateId)
    {
      if (GoldEquipMgr.dictionary_0 == null)
        GoldEquipMgr.Init();
      try
      {
        foreach (GoldEquipTemplateInfo equipTemplateInfo in GoldEquipMgr.dictionary_0.Values)
        {
          if (equipTemplateInfo.OldTemplateId == templateId)
            return equipTemplateInfo;
        }
      }
      catch
      {
      }
      return (GoldEquipTemplateInfo) null;
    }

    public static GoldEquipTemplateInfo FindGoldEquipOldTemplate(int TemplateId)
    {
      if (GoldEquipMgr.dictionary_0 == null)
        GoldEquipMgr.Init();
      try
      {
        foreach (GoldEquipTemplateInfo equipTemplateInfo in GoldEquipMgr.dictionary_0.Values)
        {
          string str = equipTemplateInfo.OldTemplateId.ToString();
          if (equipTemplateInfo.NewTemplateId == TemplateId && str.Substring(6) != "6")
            return equipTemplateInfo;
        }
      }
      catch
      {
      }
      return (GoldEquipTemplateInfo) null;
    }

    public static GoldEquipTemplateInfo FindGoldEquipByTemplate(
      int templateId,
      int categoryId)
    {
      GoldEquipTemplateInfo equipTemplateInfo1 = (GoldEquipTemplateInfo) null;
      if (GoldEquipMgr.dictionary_0 == null)
        GoldEquipMgr.Init();
      try
      {
        foreach (GoldEquipTemplateInfo equipTemplateInfo2 in GoldEquipMgr.dictionary_0.Values)
        {
          if (equipTemplateInfo2.OldTemplateId == templateId || equipTemplateInfo2.OldTemplateId == -1 && equipTemplateInfo2.CategoryID == categoryId)
          {
            equipTemplateInfo1 = equipTemplateInfo2;
            break;
          }
        }
      }
      catch
      {
      }
      return equipTemplateInfo1;
    }
  }
}
