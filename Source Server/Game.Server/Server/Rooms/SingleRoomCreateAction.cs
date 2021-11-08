// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.SingleRoomCreateAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.GameObjects;
using Game.Server.Games;
using System;
using System.Collections.Generic;

namespace Game.Server.Rooms
{
  public class SingleRoomCreateAction : IAction
  {
    private GamePlayer gamePlayer_0;
    private string string_0;
    private string string_1;
    private eRoomType eRoomType_0;
    private int int_0;
    private Random random_0;

    public SingleRoomCreateAction(
      GamePlayer player,
      string name,
      string password,
      eRoomType roomType,
      int mapId)
    {
      this.gamePlayer_0 = player;
      this.string_0 = name;
      this.string_1 = password;
      this.eRoomType_0 = roomType;
      this.int_0 = mapId;
      this.random_0 = new Random();
    }

    public void Execute()
    {
      if (this.gamePlayer_0.CurrentRoom != null)
        this.gamePlayer_0.CurrentRoom.RemovePlayerUnsafe(this.gamePlayer_0);
      if (!this.gamePlayer_0.IsActive)
        return;
      BaseRoom[] rooms = RoomMgr.Rooms;
      BaseRoom room = (BaseRoom) null;
      int index1 = this.random_0.Next(rooms.Length);
      for (int index2 = 0; index2 < rooms.Length; ++index2)
      {
        if (index2 > 100)
          index1 = index2;
        if (rooms[index1].IsUsing)
        {
          index1 = this.random_0.Next(rooms.Length);
        }
        else
        {
          room = rooms[index1];
          break;
        }
      }
      if (room == null)
        return;
      RoomMgr.WaitingRoom.RemovePlayer(this.gamePlayer_0);
      room.Start();
      room.HardLevel = eHardLevel.Simple;
      room.ZoneId = this.gamePlayer_0.ZoneId;
      room.UpdateRoom(this.string_0, this.string_1, this.eRoomType_0, (byte) 3, this.int_0);
      this.gamePlayer_0.Out.SendSingleRoomCreate(room, this.gamePlayer_0.ZoneId);
      room.AddPlayerUnsafe(this.gamePlayer_0);
      List<GamePlayer> players1 = room.GetPlayers();
      List<IGamePlayer> players2 = new List<IGamePlayer>();
      foreach (GamePlayer gamePlayer in players1)
      {
        if (gamePlayer != null)
          players2.Add((IGamePlayer) gamePlayer);
      }
      BaseGame baseGame = GameMgr.StartPVEGame(room.RoomId, players2, room.MapId, room.RoomType, room.GameType, (int) room.TimeMode, room.HardLevel, room.LevelLimits, room.currentFloor);
      if (baseGame == null)
        return;
      room.IsPlaying = true;
      room.StartGame((AbstractGame) baseGame);
    }
  }
}
