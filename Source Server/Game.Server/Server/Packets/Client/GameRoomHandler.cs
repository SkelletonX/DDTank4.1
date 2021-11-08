// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.GameRoomHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Server.Packets.Client
{
  [PacketHandler(94, "游戏创建")]
  public class GameRoomHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num1 = packet.ReadInt();
      switch (num1)
      {
        case 0:
          byte num2 = packet.ReadByte();
          byte timeType = packet.ReadByte();
          string name = packet.ReadString();
          string password = packet.ReadString();
          if (num2 == (byte) 15)
          {
            if (!client.Player.Labyrinth.completeChallenge)
            {
              client.Player.SendMessage(LanguageMgr.GetTranslation("Você já completou a fase de hoje"));
              break;
            }
            client.Player.Labyrinth.isInGame = true;
          }
          RoomMgr.CreateRoom(client.Player, name, password, (eRoomType) num2, timeType);
          break;
        case 1:
          bool isInvite = packet.ReadBoolean();
          int type = packet.ReadInt();
          int num3 = packet.ReadInt();
          int roomId = -1;
          string pwd = (string) null;
          if (num3 == -1)
          {
            roomId = packet.ReadInt();
            pwd = packet.ReadString();
          }
          switch (type)
          {
            case 1:
              type = 0;
              break;
            case 2:
              type = 4;
              break;
          }
          RoomMgr.EnterRoom(client.Player, roomId, pwd, type, isInvite);
          break;
        case 2:
          if (client.Player.CurrentRoom != null && client.Player == client.Player.CurrentRoom.Host && !client.Player.CurrentRoom.IsPlaying)
          {
            int mapId = packet.ReadInt();
            eRoomType roomType = (eRoomType) packet.ReadByte();
            string roomPass = packet.ReadString();
            string roomName = packet.ReadString();
            byte timeMode = packet.ReadByte();
            byte num4 = packet.ReadByte();
            int levelLimits = packet.ReadInt();
            bool isCross = packet.ReadBoolean();
            int currentFloor = 1;
            if (mapId == 0 && roomType == eRoomType.Labyrinth)
            {
              mapId = 401;
              currentFloor = client.Player.Labyrinth.currentFloor;
            }
            RoomMgr.UpdateRoomGameType(client.Player.CurrentRoom, roomType, timeMode, (eHardLevel) num4, levelLimits, mapId, roomName, roomPass, isCross, currentFloor);
            break;
          }
          if (!client.Player.CurrentRoom.IsPlaying)
          {
            client.Player.SendMessage("A sala não foi encontrada");
            break;
          }
          break;
        case 3:
          if (client.Player.CurrentRoom != null && client.Player == client.Player.CurrentRoom.Host)
          {
            RoomMgr.KickPlayer(client.Player.CurrentRoom, packet.ReadByte());
            break;
          }
          break;
        case 5:
          if (client.Player.CurrentRoom != null)
          {
            RoomMgr.ExitRoom(client.Player.CurrentRoom, client.Player);
            break;
          }
          break;
        case 6:
          if (client.Player.CurrentRoom == null || client.Player.CurrentRoom.RoomType == eRoomType.Match)
            return 0;
          RoomMgr.smethod_2(client.Player);
          break;
        case 7:
          BaseRoom currentRoom = client.Player.CurrentRoom;
          if (currentRoom != null && currentRoom.Host == client.Player)
          {
            if (client.Player.MainWeapon == null)
            {
              client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.SceneGames.NoEquip"));
              return 0;
            }
            if (currentRoom.RoomType == eRoomType.Dungeon)
            {
              if (!client.Player.IsPvePermission(currentRoom.MapId, currentRoom.HardLevel))
              {
                client.Player.SendMessage("Você não possui permissões para entrar na sala");
                return 0;
              }
            }
            else if (currentRoom.RoomType == eRoomType.Boss && client.Player.RemoveMoney(100) <= 0)
            {
              client.Player.SendInsufficientMoney(0);
              return 0;
            }
            RoomMgr.StartGame(client.Player.CurrentRoom);
            break;
          }
          client.Player.SendMessage("A sala está cheia");
          break;
        case 9:
          int num5 = packet.ReadInt();
          packet.ReadInt();
          if (packet.ReadInt() >= 0)
            ;
          List<BaseRoom> source = num5 != 1 ? RoomMgr.GetAllDungeonRooms() : RoomMgr.GetAllMatchRooms();
          if (source.Count > 0)
          {
            List<BaseRoom> list = source.OrderBy<BaseRoom, Guid>((Func<BaseRoom, Guid>) (a => Guid.NewGuid())).Take<BaseRoom>(8).ToList<BaseRoom>();
            client.Out.SendUpdateRoomList(list);
            break;
          }
          break;
        case 10:
          if (client.Player.CurrentRoom != null && client.Player == client.Player.CurrentRoom.Host)
          {
            RoomMgr.UpdateRoomPos(client.Player.CurrentRoom, (int) packet.ReadByte(), packet.ReadBoolean());
            break;
          }
          break;
        case 11:
          if (client.Player.CurrentRoom != null && client.Player.CurrentRoom.BattleServer != null)
          {
            client.Player.CurrentRoom.BattleServer.RemoveRoom(client.Player.CurrentRoom);
            break;
          }
          break;
        case 12:
          if (client.Player.CurrentRoom != null)
          {
            if (packet.ReadInt() == 0)
              client.Player.CurrentRoom.GameType = eGameType.Free;
            else if (client.Player.CurrentRoom.IsAllSameGuild())
              client.Player.CurrentRoom.GameType = eGameType.Guild;
            GSPacketIn pkg = client.Player.Out.SendRoomType(client.Player, client.Player.CurrentRoom);
            client.Player.CurrentRoom.SendToAll(pkg, client.Player);
            break;
          }
          break;
        case 15:
          if (client.Player.MainWeapon == null)
          {
            client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.SceneGames.NoEquip"));
            break;
          }
          if (client.Player.CurrentRoom.Host == client.Player)
          {
            client.Player.CurrentRoom.SendPlaceState();
            break;
          }
          if (client.Player.CurrentRoom != null)
          {
            RoomMgr.UpdatePlayerState(client.Player, packet.ReadByte());
            break;
          }
          break;
        default:
          Console.WriteLine("//gameroomcmd: " + (object) num1);
          break;
      }
      return 0;
    }
  }
}
