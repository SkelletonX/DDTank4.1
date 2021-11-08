// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.BaseWaitingRoom
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Logic;
using Game.Server.GameObjects;
using Game.Server.Packets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Server.Rooms
{
  public class BaseWaitingRoom
  {
    private Dictionary<int, GamePlayer> dictionary_0;

    public BaseWaitingRoom()
    {
      this.dictionary_0 = new Dictionary<int, GamePlayer>();
    }

    public bool AddPlayer(GamePlayer player)
    {
      bool flag = false;
      lock (this.dictionary_0)
      {
        if (!this.dictionary_0.ContainsKey(player.PlayerId))
        {
          this.dictionary_0.Add(player.PlayerId, player);
          flag = true;
        }
      }
      if (flag)
        this.method_1(player.Out.SendSceneAddPlayer(player), player);
      return flag;
    }

    public bool RemovePlayer(GamePlayer player)
    {
      bool flag = false;
      lock (this.dictionary_0)
        flag = this.dictionary_0.Remove(player.PlayerId);
      if (flag)
        this.method_1(player.Out.SendSceneRemovePlayer(player), player);
      return true;
    }

    public void SendUpdateRoom(eRoomType roomType)
    {
      GamePlayer[] gamePlayerArray = (GamePlayer[]) null;
      List<BaseRoom> source = new List<BaseRoom>();
      switch (roomType)
      {
        case eRoomType.Match:
        case eRoomType.Freedom:
          gamePlayerArray = this.GetPlayersSafe(ePlayerState.Manual);
          source = RoomMgr.GetAllMatchRooms();
          break;
        case eRoomType.Dungeon:
          gamePlayerArray = this.GetPlayersSafe(ePlayerState.Away);
          source = RoomMgr.GetAllDungeonRooms();
          break;
      }
      if (gamePlayerArray == null)
        return;
      List<BaseRoom> list = source.OrderBy<BaseRoom, Guid>((Func<BaseRoom, Guid>) (a => Guid.NewGuid())).Take<BaseRoom>(8).ToList<BaseRoom>();
      foreach (GamePlayer gamePlayer in gamePlayerArray)
        gamePlayer.Out.SendUpdateRoomList(list);
    }

    public void SendUpdateRoom(BaseRoom room)
    {
      GamePlayer[] gamePlayerArray = (GamePlayer[]) null;
      switch (room.RoomType)
      {
        case eRoomType.Match:
        case eRoomType.Freedom:
          gamePlayerArray = this.GetPlayersSafe(ePlayerState.Manual);
          break;
        case eRoomType.Dungeon:
          gamePlayerArray = this.GetPlayersSafe(ePlayerState.Away);
          break;
      }
      if (gamePlayerArray == null)
        return;
      foreach (GamePlayer gamePlayer in gamePlayerArray)
        gamePlayer.Out.SendUpdateRoomList(new List<BaseRoom>()
        {
          room
        });
    }

    public void SendUpdateCurrentRoom(BaseRoom room)
    {
      List<BaseRoom> allRooms = RoomMgr.GetAllRooms(room);
      GSPacketIn packet = (GSPacketIn) null;
      foreach (GamePlayer player in room.GetPlayers())
      {
        if (packet == null)
          packet = player.Out.SendUpdateRoomList(allRooms);
        else
          player.Out.SendTCP(packet);
      }
    }

    public void method_0(GSPacketIn packet)
    {
      this.method_1(packet, (GamePlayer) null);
    }

    public void method_1(GSPacketIn packet, GamePlayer except)
    {
      GamePlayer[] array = (GamePlayer[]) null;
      lock (this.dictionary_0)
      {
        array = new GamePlayer[this.dictionary_0.Count];
        this.dictionary_0.Values.CopyTo(array, 0);
      }
      if (array == null)
        return;
      foreach (GamePlayer gamePlayer in array)
      {
        if (gamePlayer != null && gamePlayer != except)
          gamePlayer.Out.SendTCP(packet);
      }
    }

    public GamePlayer[] GetPlayersSafe()
    {
      GamePlayer[] array = (GamePlayer[]) null;
      lock (this.dictionary_0)
      {
        array = new GamePlayer[this.dictionary_0.Count];
        this.dictionary_0.Values.CopyTo(array, 0);
      }
      return array ?? new GamePlayer[0];
    }

    public GamePlayer[] GetPlayersSafe(ePlayerState typeState)
    {
      List<GamePlayer> gamePlayerList = new List<GamePlayer>();
      lock (this.dictionary_0)
      {
        foreach (GamePlayer gamePlayer in this.dictionary_0.Values)
        {
          if (gamePlayer.PlayerState == typeState)
            gamePlayerList.Add(gamePlayer);
        }
      }
      return gamePlayerList.ToArray();
    }
  }
}
