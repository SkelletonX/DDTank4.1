// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.RoomMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.Battle;
using Game.Server.GameObjects;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.Rooms
{
  public class RoomMgr
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    public static readonly int THREAD_INTERVAL = 100;
    public static readonly int PICK_UP_INTERVAL = 10000;
    public static readonly int CLEAR_ROOM_INTERVAL = 400;
    private static long long_0 = 0;
    private static bool bool_0;
    private static Queue<IAction> queue_0;
    private static Thread thread_0;
    private static BaseRoom[] baseRoom_0;
    private static BaseWaitingRoom baseWaitingRoom_0;

    public static BaseRoom[] Rooms
    {
      get
      {
        return RoomMgr.baseRoom_0;
      }
    }

    public static BaseWaitingRoom WaitingRoom
    {
      get
      {
        return RoomMgr.baseWaitingRoom_0;
      }
    }

    public static bool Setup(int maxRoom)
    {
      maxRoom = maxRoom < 1 ? 1 : maxRoom;
      RoomMgr.thread_0 = new Thread(new ThreadStart(RoomMgr.smethod_0));
      RoomMgr.queue_0 = new Queue<IAction>();
      RoomMgr.baseRoom_0 = new BaseRoom[maxRoom * 10];
      for (int index = 0; index < maxRoom * 10; ++index)
        RoomMgr.baseRoom_0[index] = new BaseRoom(index + 1);
      RoomMgr.baseWaitingRoom_0 = new BaseWaitingRoom();
      return true;
    }
        public static void FakeRoom(string roomName, int playerCount, int maxPlayerCount, int roomType)
        {
          //  AddAction(new FakeRoomAction(roomName, playerCount, maxPlayerCount, roomType));
        }
        public static void Start()
    {
      if (RoomMgr.bool_0)
        return;
      RoomMgr.bool_0 = true;
      RoomMgr.thread_0.Start();
    }

    public static void Stop()
    {
      if (!RoomMgr.bool_0)
        return;
      RoomMgr.bool_0 = false;
      RoomMgr.thread_0.Join();
    }

    private static void smethod_0()
    {
      Thread.CurrentThread.Priority = ThreadPriority.Highest;
      long num1 = 0;
      RoomMgr.long_0 = TickHelper.GetTickCount();
      while (RoomMgr.bool_0)
      {
        long tickCount1 = TickHelper.GetTickCount();
        int num2 = 0;
        try
        {
          num2 = RoomMgr.smethod_1();
          if (RoomMgr.long_0 <= tickCount1)
          {
            RoomMgr.long_0 += (long) RoomMgr.CLEAR_ROOM_INTERVAL;
            RoomMgr.ClearRooms(tickCount1);
          }
        }
        catch (Exception ex)
        {
          RoomMgr.ilog_0.Error((object) "Room Mgr Thread Error:", ex);
        }
        long tickCount2 = TickHelper.GetTickCount();
        num1 += (long) RoomMgr.THREAD_INTERVAL - (tickCount2 - tickCount1);
        if (tickCount2 - tickCount1 > (long) (RoomMgr.THREAD_INTERVAL * 2))
          RoomMgr.ilog_0.WarnFormat("Room Mgr is spent too much times: {0} ms,count:{1}", (object) (tickCount2 - tickCount1), (object) num2);
        if (num1 > 0L)
        {
          Thread.Sleep((int) num1);
          num1 = 0L;
        }
        else if (num1 < -1000L)
          num1 += 1000L;
      }
    }

    private static int smethod_1()
    {
      IAction[] array = (IAction[]) null;
      lock (RoomMgr.queue_0)
      {
        if (RoomMgr.queue_0.Count > 0)
        {
          array = new IAction[RoomMgr.queue_0.Count];
          RoomMgr.queue_0.CopyTo(array, 0);
          RoomMgr.queue_0.Clear();
        }
      }
      if (array == null)
        return 0;
      foreach (IAction action in array)
      {
        try
        {
          long tickCount1 = TickHelper.GetTickCount();
          action.Execute();
          long tickCount2 = TickHelper.GetTickCount();
          if (tickCount2 - tickCount1 > 100L)
            RoomMgr.ilog_0.WarnFormat("RoomMgr action spent too much times:{0},{1}ms!", (object) action.GetType(), (object) (tickCount2 - tickCount1));
        }
        catch (Exception ex)
        {
          RoomMgr.ilog_0.Error((object) "RoomMgr execute action error:", ex);
        }
      }
      return array.Length;
    }

    public static void ClearRooms(long tick)
    {
      lock (RoomMgr.baseRoom_0)
      {
        foreach (BaseRoom baseRoom in RoomMgr.baseRoom_0)
        {
          if (baseRoom.IsUsing && baseRoom.PlayerCount == 0)
            baseRoom.Stop();
        }
      }
    }

    public static BaseRoom FindRoomCanUse()
    {
      lock (RoomMgr.baseRoom_0)
      {
        foreach (BaseRoom baseRoom in RoomMgr.baseRoom_0)
        {
          if (!baseRoom.IsUsing)
          {
            baseRoom.Start();
            return baseRoom;
          }
        }
        return (BaseRoom) null;
      }
    }

    public static BaseRoom FindRandomUsingRoom()
    {
      lock (RoomMgr.baseRoom_0)
      {
        foreach (BaseRoom baseRoom in RoomMgr.baseRoom_0)
        {
          if (!baseRoom.IsUsing)
          {
            baseRoom.Start();
            return baseRoom;
          }
        }
        return (BaseRoom) null;
      }
    }

    public static void AddAction(IAction action)
    {
      lock (RoomMgr.queue_0)
        RoomMgr.queue_0.Enqueue(action);
    }

    public static void CreateRoom(
      GamePlayer player,
      string name,
      string password,
      eRoomType roomType,
      byte timeType)
    {
      RoomMgr.AddAction((IAction) new CreateRoomAction(player, name, password, roomType, timeType));
    }

    public static void CreateSingleRoom(
      GamePlayer player,
      string name,
      string password,
      eRoomType roomType,
      int mapId)
    {
      RoomMgr.AddAction((IAction) new SingleRoomCreateAction(player, name, password, roomType, mapId));
    }

    public static void EnterRoom(
      GamePlayer player,
      int roomId,
      string pwd,
      int type,
      bool isInvite)
    {
      RoomMgr.AddAction((IAction) new EnterRoomAction(player, roomId, pwd, type, isInvite));
    }

    public static void EnterRoom(GamePlayer player)
    {
      RoomMgr.EnterRoom(player, -1, (string) null, 1, true);
    }

    public static void ExitRoom(BaseRoom room, GamePlayer player)
    {
      RoomMgr.AddAction((IAction) new ExitRoomAction(room, player, false));
    }

    public static void StartGame(BaseRoom room)
    {
      RoomMgr.AddAction((IAction) new StartGameAction(room));
    }

    public static void UpdatePlayerState(GamePlayer player, byte state)
    {
      RoomMgr.AddAction((IAction) new UpdatePlayerStateAction(player, player.CurrentRoom, state));
    }

    public static void UpdateRoomPos(BaseRoom room, int pos, bool isOpened)
    {
      RoomMgr.AddAction((IAction) new UpdateRoomPosAction(room, pos, isOpened));
    }

    public static void KickPlayer(BaseRoom baseRoom, byte index)
    {
      RoomMgr.AddAction((IAction) new KickPlayerAction(baseRoom, (int) index));
    }

    public static void EnterWaitingRoom(GamePlayer player)
    {
      RoomMgr.AddAction((IAction) new EnterWaitingRoomAction(player));
    }

    public static void ExitWaitingRoom(GamePlayer player)
    {
      RoomMgr.AddAction((IAction) new ExitWaitRoomAction(player));
    }

    public static void CancelPickup(BattleServer server, BaseRoom room)
    {
      RoomMgr.AddAction((IAction) new CancelPickupAction(server, room));
    }

    public static void UpdateRoomGameType(
      BaseRoom room,
      eRoomType roomType,
      byte timeMode,
      eHardLevel hardLevel,
      int levelLimits,
      int mapId,
      string roomName,
      string roomPass,
      bool isCross,
      int currentFloor)
    {
      RoomMgr.AddAction((IAction) new RoomSetupChangeAction(room, roomType, timeMode, hardLevel, levelLimits, mapId, roomName, roomPass, isCross, currentFloor));
    }

    internal static void smethod_2(GamePlayer gamePlayer_0)
    {
      RoomMgr.AddAction((IAction) new SwitchTeamAction(gamePlayer_0));
    }

    public static void StartGameMission(BaseRoom room)
    {
      RoomMgr.AddAction((IAction) new StartGameMissionAction(room));
    }

    public static List<BaseRoom> GetAllUsingRoom()
    {
      List<BaseRoom> baseRoomList = new List<BaseRoom>();
      lock (RoomMgr.baseRoom_0)
      {
        foreach (BaseRoom baseRoom in RoomMgr.baseRoom_0)
        {
          if (baseRoom.IsUsing)
            baseRoomList.Add(baseRoom);
        }
      }
      return baseRoomList;
    }

    public static List<BaseRoom> GetUsingRoom(int type)
    {
      List<BaseRoom> baseRoomList = new List<BaseRoom>();
      lock (RoomMgr.baseRoom_0)
      {
        foreach (BaseRoom baseRoom in RoomMgr.baseRoom_0)
        {
          if (baseRoom.IsUsing && baseRoom.RoomType == (eRoomType) type)
            baseRoomList.Add(baseRoom);
        }
      }
      return baseRoomList;
    }

    public static void KickUsingRoom(int type)
    {
      foreach (BaseRoom room in RoomMgr.GetUsingRoom(type))
      {
        room.RemoveAllPlayer();
        RoomMgr.StopProxyGame(room);
      }
    }

    public static BaseRoom FindRoom(int id)
    {
      lock (RoomMgr.baseRoom_0)
      {
        if (id < RoomMgr.baseRoom_0.Length)
          return RoomMgr.baseRoom_0[id];
        return (BaseRoom) null;
      }
    }

    public static List<BaseRoom> GetAllMatchRooms()
    {
      List<BaseRoom> baseRoomList = new List<BaseRoom>();
      lock (RoomMgr.baseRoom_0)
      {
        foreach (BaseRoom baseRoom in RoomMgr.baseRoom_0)
        {
          if (!baseRoom.IsEmpty && (baseRoom.RoomType == eRoomType.Match || baseRoom.RoomType == eRoomType.Freedom))
            baseRoomList.Add(baseRoom);
        }
      }
      return baseRoomList;
    }

    public static List<BaseRoom> GetAllDungeonRooms()
    {
      List<BaseRoom> baseRoomList = new List<BaseRoom>();
      lock (RoomMgr.baseRoom_0)
      {
        foreach (BaseRoom baseRoom in RoomMgr.baseRoom_0)
        {
          if (!baseRoom.IsEmpty && baseRoom.RoomType == eRoomType.Dungeon)
            baseRoomList.Add(baseRoom);
        }
      }
      return baseRoomList;
    }

    public static List<BaseRoom> GetAllRooms()
    {
      List<BaseRoom> baseRoomList = new List<BaseRoom>();
      lock (RoomMgr.baseRoom_0)
      {
        foreach (BaseRoom baseRoom in RoomMgr.baseRoom_0)
        {
          if (!baseRoom.IsEmpty)
            baseRoomList.Add(baseRoom);
        }
      }
      return baseRoomList;
    }

    public static List<BaseRoom> GetAllRooms(BaseRoom seffRoom)
    {
      List<BaseRoom> baseRoomList = new List<BaseRoom>();
      baseRoomList.Add(seffRoom);
      lock (RoomMgr.baseRoom_0)
      {
        foreach (BaseRoom baseRoom in RoomMgr.baseRoom_0)
        {
          if (!baseRoom.IsEmpty)
            baseRoomList.Add(baseRoom);
        }
      }
      return baseRoomList;
    }

    public static void StartProxyGame(BaseRoom room, ProxyGame game)
    {
      RoomMgr.AddAction((IAction) new StartProxyGameAction(room, game));
    }

    public static void StopProxyGame(BaseRoom room)
    {
      RoomMgr.AddAction((IAction) new StopProxyGameAction(room));
    }
  }
}
