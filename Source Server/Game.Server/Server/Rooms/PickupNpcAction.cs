// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.PickupNpcAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.GameObjects;
using Game.Server.Games;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Rooms
{
  public class PickupNpcAction : IAction
  {
    private int m_roomID;

    public PickupNpcAction(int roomID)
    {
      this.m_roomID = roomID;
    }

    public GamePlayer CreateNpcPlayer()
    {
      return new GamePlayer(1, "Null Player", (GameClient) null, (PlayerInfo) null);
    }

    public void Execute()
    {
      BaseRoom room = (BaseRoom) null;
      BaseRoom[] rooms = RoomMgr.Rooms;
      for (int index = 0; index < rooms.Length; ++index)
      {
        if (rooms[index].RoomId == this.m_roomID)
        {
          room = rooms[index];
          break;
        }
      }
      if (room == null)
        return;
      GamePlayer playerById = WorldMgr.GetPlayerById(19);
      if (playerById == null)
        return;
      RoomMgr.WaitingRoom.RemovePlayer(playerById);
      playerById.Out.SendRoomLoginResult(true);
      playerById.Out.SendRoomCreate(room);
      room.AddPlayerUnsafe(playerById);
      if (room.PlayerCount != 2)
        return;
      List<IGamePlayer> red = new List<IGamePlayer>();
      List<IGamePlayer> blue = new List<IGamePlayer>()
      {
        (IGamePlayer) room.Host,
        (IGamePlayer) playerById
      };
      BaseGame baseGame = GameMgr.StartPVPGame(room.RoomId, red, blue, room.MapId, room.RoomType, room.GameType, (int) room.TimeMode);
      if (baseGame != null)
      {
        room.IsPlaying = true;
        room.StartGame((AbstractGame) baseGame);
      }
      else
      {
        room.IsPlaying = false;
        room.SendPlayerState();
      }
    }
  }
}
