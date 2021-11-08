// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.UserBoxMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Game.Server.Managers
{
  public class UserBoxMgr
  {
    private static Dictionary<int, LoadUserBoxInfo> m_BoxInfo;
    private static ReaderWriterLock m_lock;

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, LoadUserBoxInfo> m_TimeBoxInfo = new Dictionary<int, LoadUserBoxInfo>();
        if (UserBoxMgr.LoadStrengthen(m_TimeBoxInfo))
        {
          UserBoxMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            UserBoxMgr.m_BoxInfo = m_TimeBoxInfo;
            return true;
          }
          catch
          {
          }
          finally
          {
            UserBoxMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(nameof (UserBoxMgr), (object) ex);
      }
      return false;
    }

    public static bool Init()
    {
      try
      {
        UserBoxMgr.m_lock = new ReaderWriterLock();
        UserBoxMgr.m_BoxInfo = new Dictionary<int, LoadUserBoxInfo>();
        return UserBoxMgr.LoadStrengthen(UserBoxMgr.m_BoxInfo);
      }
      catch (Exception ex)
      {
        Console.WriteLine(nameof (UserBoxMgr), (object) ex);
        return false;
      }
    }

    private static bool LoadStrengthen(Dictionary<int, LoadUserBoxInfo> m_TimeBoxInfo)
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (LoadUserBoxInfo loadUserBoxInfo in produceBussiness.GetAllTimeBoxAward())
        {
          if (!m_TimeBoxInfo.ContainsKey(loadUserBoxInfo.ID))
            m_TimeBoxInfo.Add(loadUserBoxInfo.ID, loadUserBoxInfo);
        }
      }
      return true;
    }

    public static LoadUserBoxInfo FindTemplateByCondition(
      int type,
      int level,
      int condition)
    {
      foreach (KeyValuePair<int, LoadUserBoxInfo> keyValuePair in UserBoxMgr.m_BoxInfo)
      {
        if (type == 0)
        {
          if (type == keyValuePair.Value.Type && level <= keyValuePair.Value.Level && condition < keyValuePair.Value.Condition)
            return keyValuePair.Value;
        }
        else if (type == keyValuePair.Value.Type && level < keyValuePair.Value.Level && condition == keyValuePair.Value.Condition)
          return keyValuePair.Value;
      }
      return (LoadUserBoxInfo) null;
    }
  }
}
