// Decompiled with JetBrains decompiler
// Type: Bussiness.Managers.QQTipsMgr
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
  public class QQTipsMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static int qqtipSelectIndex = 0;
    private static Dictionary<int, QQtipsMessagesInfo> _qqtips;
    private static ReaderWriterLock m_lock;

    public static QQtipsMessagesInfo GetQQtipsMessages()
    {
      if (QQTipsMgr._qqtips == null)
        QQTipsMgr.Init();
      QQTipsMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (QQTipsMgr.qqtipSelectIndex >= QQTipsMgr._qqtips.Count)
          QQTipsMgr.qqtipSelectIndex = 1;
        else
          ++QQTipsMgr.qqtipSelectIndex;
        return QQTipsMgr._qqtips[QQTipsMgr.qqtipSelectIndex];
      }
      finally
      {
        QQTipsMgr.m_lock.ReleaseReaderLock();
      }
    }

    public static bool Init()
    {
      try
      {
        QQTipsMgr.m_lock = new ReaderWriterLock();
        QQTipsMgr._qqtips = new Dictionary<int, QQtipsMessagesInfo>();
        return QQTipsMgr.LoadItem(QQTipsMgr._qqtips);
      }
      catch (Exception ex)
      {
        if (QQTipsMgr.log.IsErrorEnabled)
          QQTipsMgr.log.Error((object) nameof (Init), ex);
        return false;
      }
    }

    public static bool LoadItem(Dictionary<int, QQtipsMessagesInfo> infos)
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (QQtipsMessagesInfo qqtipsMessagesInfo in produceBussiness.GetAllQQtipsMessagesLoad())
        {
          if (!infos.Keys.Contains<int>(qqtipsMessagesInfo.ID))
            infos.Add(qqtipsMessagesInfo.ID, qqtipsMessagesInfo);
        }
      }
      return true;
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, QQtipsMessagesInfo> infos = new Dictionary<int, QQtipsMessagesInfo>();
        if (QQTipsMgr.LoadItem(infos))
        {
          QQTipsMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            QQTipsMgr._qqtips = infos;
            return true;
          }
          catch
          {
          }
          finally
          {
            QQTipsMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (QQTipsMgr.log.IsErrorEnabled)
          QQTipsMgr.log.Error((object) nameof (ReLoad), ex);
      }
      return false;
    }
  }
}
