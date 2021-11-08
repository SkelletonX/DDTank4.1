// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.HotSpringMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.HotSpringRooms;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
  public class HotSpringMgr
  {
    protected static TankHotSpringLogicProcessor _processor = new TankHotSpringLogicProcessor();
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<int, HotSpringRoom> dictionary_0;
    public static string[] HotSpringEnterPriRoom;
    protected static ReaderWriterLock m_lock;
    protected static ThreadSafeRandom m_rand;
    private static string[] string_0;

    public static HotSpringRoom CreateHotSpringRoomFromDB(HotSpringRoomInfo roomInfo)
    {
      HotSpringMgr.m_lock.AcquireWriterLock(-1);
      try
      {
        HotSpringRoom hotSpringRoom = new HotSpringRoom(roomInfo, (GInterface2) HotSpringMgr._processor);
        if (hotSpringRoom != null)
        {
          HotSpringMgr.dictionary_0.Add(hotSpringRoom.Info.roomID, hotSpringRoom);
          return hotSpringRoom;
        }
      }
      finally
      {
        HotSpringMgr.m_lock.ReleaseWriterLock();
      }
      return (HotSpringRoom) null;
    }

    public static HotSpringRoom[] GetAllHotSpringRoom()
    {
      HotSpringRoom[] array = (HotSpringRoom[]) null;
      HotSpringMgr.m_lock.AcquireReaderLock(-1);
      try
      {
        array = new HotSpringRoom[HotSpringMgr.dictionary_0.Count];
        HotSpringMgr.dictionary_0.Values.CopyTo(array, 0);
      }
      finally
      {
        HotSpringMgr.m_lock.ReleaseReaderLock();
      }
      return array ?? new HotSpringRoom[0];
    }

    public static int GetExpWithLevel(int grade)
    {
      try
      {
        if (grade <= HotSpringMgr.string_0.Length)
          return int.Parse(HotSpringMgr.string_0[grade - 1]);
      }
      catch (Exception ex)
      {
        HotSpringMgr.ilog_0.Error((object) ("GetExpWithLevel Error: " + ex.ToString()));
      }
      return 0;
    }

    public static HotSpringRoom GetHotSpringRoombyID(int id)
    {
      HotSpringRoom hotSpringRoom = (HotSpringRoom) null;
      HotSpringMgr.m_lock.AcquireReaderLock(-1);
      try
      {
        if (id > 0)
        {
          if (HotSpringMgr.dictionary_0.Keys.Contains<int>(id))
            hotSpringRoom = HotSpringMgr.dictionary_0[id];
        }
      }
      finally
      {
        HotSpringMgr.m_lock.ReleaseReaderLock();
      }
      return hotSpringRoom;
    }

    public static HotSpringRoom GetHotSpringRoombyID(
      int id,
      string pwd,
      ref string msg)
    {
      HotSpringRoom hotSpringRoom = (HotSpringRoom) null;
      HotSpringMgr.m_lock.AcquireReaderLock(-1);
      try
      {
        if (id <= 0 || !HotSpringMgr.dictionary_0.Keys.Contains<int>(id))
          return hotSpringRoom;
        if (!(HotSpringMgr.dictionary_0[id].Info.roomPassword != pwd))
          return HotSpringMgr.dictionary_0[id];
        msg = "A senha está incorreta!";
        return hotSpringRoom;
      }
      finally
      {
        HotSpringMgr.m_lock.ReleaseReaderLock();
      }
    }

    public static HotSpringRoom GetRandomRoom()
    {
      HotSpringRoom hotSpringRoom1 = (HotSpringRoom) null;
      HotSpringMgr.m_lock.AcquireReaderLock(-1);
      try
      {
        List<HotSpringRoom> hotSpringRoomList = new List<HotSpringRoom>();
        foreach (HotSpringRoom hotSpringRoom2 in HotSpringMgr.dictionary_0.Values)
        {
          if (hotSpringRoom2.Count < hotSpringRoom2.Info.maxCount)
            hotSpringRoomList.Add(hotSpringRoom2);
        }
        if (hotSpringRoomList.Count > 0)
        {
          int index = HotSpringMgr.m_rand.Next(0, hotSpringRoomList.Count);
          hotSpringRoom1 = hotSpringRoomList[index];
        }
      }
      finally
      {
        HotSpringMgr.m_lock.ReleaseReaderLock();
      }
      return hotSpringRoom1;
    }

    public static bool Init()
    {
      try
      {
        HotSpringMgr.m_lock = new ReaderWriterLock();
        HotSpringMgr.m_rand = new ThreadSafeRandom();
        HotSpringMgr.dictionary_0 = new Dictionary<int, HotSpringRoom>();
        char[] chArray1 = new char[1]{ ',' };
        HotSpringMgr.string_0 = GameProperties.HotSpringExp.Split(chArray1);
        char[] chArray2 = new char[1]{ ',' };
        HotSpringMgr.HotSpringEnterPriRoom = GameProperties.SpaPubRoomLoginPay.Split(chArray2);
        HotSpringMgr.smethod_0();
        return true;
      }
      catch (Exception ex)
      {
        if (HotSpringMgr.ilog_0.IsErrorEnabled)
          HotSpringMgr.ilog_0.Error((object) nameof (HotSpringMgr), ex);
        return false;
      }
    }

    public static void SendUpdateAllRoom(GamePlayer p, HotSpringRoom[] rooms)
    {
      GSPacketIn pkg = new GSPacketIn((short) 197);
      pkg.WriteInt(rooms.Length);
      foreach (HotSpringRoom room in rooms)
      {
        pkg.WriteInt(room.Info.roomNumber);
        pkg.WriteInt(room.Info.roomID);
        pkg.WriteString(room.Info.roomName);
        pkg.WriteString(room.Info.roomPassword != null ? "password" : "");
        pkg.WriteInt(room.Info.effectiveTime);
        pkg.WriteInt(room.Count);
        pkg.WriteInt(room.Info.playerID);
        pkg.WriteString(room.Info.playerName);
        pkg.WriteDateTime(room.Info.startTime);
        pkg.WriteString(room.Info.roomIntroduction);
        pkg.WriteInt(room.Info.roomType);
        pkg.WriteInt(room.Info.maxCount);
      }
      if (p != null)
        p.SendTCP(pkg);
      else
        WorldMgr.HotSpringScene.SendToALL(pkg);
    }

    private static void smethod_0()
    {
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (HotSpringRoomInfo allHotSpringRoom in produceBussiness.GetAllHotSpringRooms())
          HotSpringMgr.CreateHotSpringRoomFromDB(allHotSpringRoom);
      }
    }
  }
}
