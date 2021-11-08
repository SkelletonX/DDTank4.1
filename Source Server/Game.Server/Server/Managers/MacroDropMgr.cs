// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.MacroDropMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Logic;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Timers;

namespace Game.Server.Managers
{
  public class MacroDropMgr
  {
    protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected static ReaderWriterLock m_lock = new ReaderWriterLock();

    public static bool Init()
    {
      MacroDropMgr.m_lock = new ReaderWriterLock();
      return MacroDropMgr.ReLoad();
    }

    private static void OnTimeEvent(object source, ElapsedEventArgs e)
    {
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      MacroDropMgr.m_lock.AcquireWriterLock(-1);
      try
      {
        foreach (KeyValuePair<int, MacroDropInfo> keyValuePair in DropInfoMgr.DropInfo)
        {
          int key = keyValuePair.Key;
          MacroDropInfo macroDropInfo = keyValuePair.Value;
          if (macroDropInfo.SelfDropCount > 0)
          {
            dictionary.Add(key, macroDropInfo.SelfDropCount);
            macroDropInfo.SelfDropCount = 0;
          }
        }
      }
      catch (Exception ex)
      {
        if (MacroDropMgr.log.IsErrorEnabled)
          MacroDropMgr.log.Error((object) "DropInfoMgr OnTimeEvent", ex);
      }
      finally
      {
        MacroDropMgr.m_lock.ReleaseWriterLock();
      }
      if (dictionary.Count <= 0)
        return;
      GSPacketIn packet = new GSPacketIn((short) 178);
      packet.WriteInt(dictionary.Count);
      foreach (KeyValuePair<int, int> keyValuePair in dictionary)
      {
        packet.WriteInt(keyValuePair.Key);
        packet.WriteInt(keyValuePair.Value);
      }
      GameServer.Instance.LoginServer.SendPacket(packet);
    }

    public static bool ReLoad()
    {
      try
      {
        DropInfoMgr.DropInfo = new Dictionary<int, MacroDropInfo>();
        return true;
      }
      catch (Exception ex)
      {
        if (MacroDropMgr.log.IsErrorEnabled)
          MacroDropMgr.log.Error((object) "DropInfoMgr", ex);
      }
      return false;
    }

    public static void Start()
    {
      System.Timers.Timer timer = new System.Timers.Timer();
      timer.Elapsed += new ElapsedEventHandler(MacroDropMgr.OnTimeEvent);
      timer.Interval = 5000.0;
      timer.Enabled = true;
    }

    public static void UpdateDropInfo(Dictionary<int, MacroDropInfo> temp)
    {
      MacroDropMgr.m_lock.AcquireWriterLock(-1);
      try
      {
        foreach (KeyValuePair<int, MacroDropInfo> keyValuePair in temp)
        {
          if (DropInfoMgr.DropInfo.ContainsKey(keyValuePair.Key))
          {
            DropInfoMgr.DropInfo[keyValuePair.Key].DropCount = keyValuePair.Value.DropCount;
            DropInfoMgr.DropInfo[keyValuePair.Key].MaxDropCount = keyValuePair.Value.MaxDropCount;
          }
          else
            DropInfoMgr.DropInfo.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
      catch (Exception ex)
      {
        if (!MacroDropMgr.log.IsErrorEnabled)
          return;
        MacroDropMgr.log.Error((object) "MacroDropMgr UpdateDropInfo", ex);
      }
      finally
      {
        MacroDropMgr.m_lock.ReleaseWriterLock();
      }
    }
  }
}
