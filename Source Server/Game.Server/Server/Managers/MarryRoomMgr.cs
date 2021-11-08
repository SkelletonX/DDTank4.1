// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.MarryRoomMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Server.GameObjects;
using Game.Server.SceneMarryRooms;
using log4net;
using log4net.Util;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Game.Server.Managers
{
  public class MarryRoomMgr
  {
    protected static ReaderWriterLock _locker = new ReaderWriterLock();
    protected static TankMarryLogicProcessor _processor = new TankMarryLogicProcessor();
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected static Dictionary<int, MarryRoom> _Rooms;

    public static MarryRoom CreateMarryRoom(GamePlayer player, MarryRoomInfo info)
    {
      if (player.PlayerCharacter.IsMarried)
      {
        MarryRoom marryRoom = (MarryRoom) null;
        DateTime now = DateTime.Now;
        info.PlayerID = player.PlayerCharacter.ID;
        info.PlayerName = player.PlayerCharacter.NickName;
        if (player.PlayerCharacter.Sex)
        {
          info.GroomID = info.PlayerID;
          info.GroomName = info.PlayerName;
          info.BrideID = player.PlayerCharacter.SpouseID;
          info.BrideName = player.PlayerCharacter.SpouseName;
        }
        else
        {
          info.BrideID = info.PlayerID;
          info.BrideName = info.PlayerName;
          info.GroomID = player.PlayerCharacter.SpouseID;
          info.GroomName = player.PlayerCharacter.SpouseName;
        }
        info.BeginTime = now;
        info.BreakTime = now;
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          if (playerBussiness.InsertMarryRoomInfo(info))
          {
            marryRoom = new MarryRoom(info, (IMarryProcessor) MarryRoomMgr._processor);
            GameServer.Instance.LoginServer.SendUpdatePlayerMarriedStates(info.GroomID);
            GameServer.Instance.LoginServer.SendUpdatePlayerMarriedStates(info.BrideID);
            GameServer.Instance.LoginServer.SendMarryRoomInfoToPlayer(info.GroomID, true, info);
            GameServer.Instance.LoginServer.SendMarryRoomInfoToPlayer(info.BrideID, true, info);
          }
        }
        if (marryRoom != null)
        {
          MarryRoomMgr._locker.AcquireWriterLock();
          try
          {
            MarryRoomMgr._Rooms.Add(marryRoom.Info.ID, marryRoom);
          }
          finally
          {
            MarryRoomMgr._locker.ReleaseWriterLock();
          }
          if (marryRoom.AddPlayer(player))
          {
            marryRoom.BeginTimer(3600000 * marryRoom.Info.AvailTime);
            return marryRoom;
          }
        }
      }
      return (MarryRoom) null;
    }

    public static MarryRoom CreateMarryRoomFromDB(MarryRoomInfo roomInfo, int timeLeft)
    {
      MarryRoomMgr._locker.AcquireWriterLock();
      try
      {
        MarryRoom marryRoom = new MarryRoom(roomInfo, (IMarryProcessor) MarryRoomMgr._processor);
        if (marryRoom != null)
        {
          MarryRoomMgr._Rooms.Add(marryRoom.Info.ID, marryRoom);
          marryRoom.BeginTimer(60000 * timeLeft);
          return marryRoom;
        }
      }
      finally
      {
        MarryRoomMgr._locker.ReleaseWriterLock();
      }
      return (MarryRoom) null;
    }

    private static void CheckRoomStatus()
    {
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        foreach (MarryRoomInfo marryRoomInfo in playerBussiness.GetMarryRoomInfo())
        {
          if (marryRoomInfo.ServerID == GameServer.Instance.Configuration.ServerID)
          {
            TimeSpan timeSpan = DateTime.Now - marryRoomInfo.BeginTime;
            int timeLeft = marryRoomInfo.AvailTime * 60 - (int) timeSpan.TotalMinutes;
            if (timeLeft > 0)
            {
              MarryRoomMgr.CreateMarryRoomFromDB(marryRoomInfo, timeLeft);
            }
            else
            {
              playerBussiness.DisposeMarryRoomInfo(marryRoomInfo.ID);
              if (GameServer.Instance.LoginServer != null)
              {
                GameServer.Instance.LoginServer.SendUpdatePlayerMarriedStates(marryRoomInfo.GroomID);
                GameServer.Instance.LoginServer.SendUpdatePlayerMarriedStates(marryRoomInfo.BrideID);
                GameServer.Instance.LoginServer.SendMarryRoomInfoToPlayer(marryRoomInfo.GroomID, false, marryRoomInfo);
                GameServer.Instance.LoginServer.SendMarryRoomInfoToPlayer(marryRoomInfo.BrideID, false, marryRoomInfo);
              }
            }
          }
        }
      }
    }

    public static MarryRoom[] GetAllMarryRoom()
    {
      MarryRoom[] array = (MarryRoom[]) null;
      MarryRoomMgr._locker.AcquireReaderLock();
      try
      {
        array = new MarryRoom[MarryRoomMgr._Rooms.Count];
        MarryRoomMgr._Rooms.Values.CopyTo(array, 0);
      }
      finally
      {
        MarryRoomMgr._locker.ReleaseReaderLock();
      }
      return array ?? new MarryRoom[0];
    }

    public static MarryRoom GetMarryRoombyID(int id, string pwd, ref string msg)
    {
      MarryRoom marryRoom = (MarryRoom) null;
      MarryRoomMgr._locker.AcquireReaderLock();
      try
      {
        if (id <= 0 || !MarryRoomMgr._Rooms.Keys.Contains<int>(id))
          return marryRoom;
        if (!(MarryRoomMgr._Rooms[id].Info.Pwd != pwd))
          return MarryRoomMgr._Rooms[id];
        msg = "Game.Server.Managers.PWDError";
        return marryRoom;
      }
      finally
      {
        MarryRoomMgr._locker.ReleaseReaderLock();
      }
    }

    public static bool Init()
    {
      MarryRoomMgr._Rooms = new Dictionary<int, MarryRoom>();
      MarryRoomMgr.CheckRoomStatus();
      return true;
    }

    public static void RemoveMarryRoom(MarryRoom room)
    {
      MarryRoomMgr._locker.AcquireReaderLock();
      try
      {
        if (!MarryRoomMgr._Rooms.Keys.Contains<int>(room.Info.ID))
          return;
        MarryRoomMgr._Rooms.Remove(room.Info.ID);
      }
      finally
      {
        MarryRoomMgr._locker.ReleaseReaderLock();
      }
    }

    public static bool UpdateBreakTimeWhereServerStop()
    {
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
        return playerBussiness.UpdateBreakTimeWhereServerStop();
    }
  }
}
