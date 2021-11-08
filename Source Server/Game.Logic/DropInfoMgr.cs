// Decompiled with JetBrains decompiler
// Type: Game.Logic.DropInfoMgr
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Logic
{
  public class DropInfoMgr
  {
    protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected static ReaderWriterLock m_lock = new ReaderWriterLock();
    public static Dictionary<int, MacroDropInfo> DropInfo;

    public static bool CanDrop(int templateId)
    {
      if (DropInfoMgr.DropInfo != null)
      {
        DropInfoMgr.m_lock.AcquireWriterLock(-1);
        try
        {
          if (DropInfoMgr.DropInfo.ContainsKey(templateId))
          {
            MacroDropInfo macroDropInfo = DropInfoMgr.DropInfo[templateId];
            if (macroDropInfo.DropCount >= macroDropInfo.MaxDropCount && macroDropInfo.SelfDropCount < macroDropInfo.DropCount)
              return false;
            ++macroDropInfo.SelfDropCount;
            ++macroDropInfo.DropCount;
            return true;
          }
        }
        catch (Exception ex)
        {
          if (DropInfoMgr.log.IsErrorEnabled)
            DropInfoMgr.log.Error((object) "DropInfoMgr CanDrop", ex);
        }
        finally
        {
          DropInfoMgr.m_lock.ReleaseWriterLock();
        }
      }
      return true;
    }
  }
}
