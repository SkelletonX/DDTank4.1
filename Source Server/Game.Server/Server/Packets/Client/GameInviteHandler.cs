// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.GameInviteHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.GameObjects;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
  [PacketHandler(70, "邀请")]
  public class GameInviteHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.CurrentRoom != null)
      {
        GamePlayer playerById = WorldMgr.GetPlayerById(packet.ReadInt());
        if (playerById == client.Player)
          return 0;
        GSPacketIn packet1 = new GSPacketIn((short) 70, client.Player.PlayerCharacter.ID);
        foreach (GamePlayer player in client.Player.CurrentRoom.GetPlayers())
        {
          if (player == playerById)
          {
            client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("friendnotinthesameserver.Sameroom"));
            return 0;
          }
        }
        if (playerById != null && playerById.CurrentRoom == null)
        {
          packet1.WriteInt(client.Player.PlayerCharacter.ID);
          packet1.WriteInt(client.Player.CurrentRoom.RoomId);
          packet1.WriteInt(client.Player.CurrentRoom.MapId);
          packet1.WriteByte(client.Player.CurrentRoom.TimeMode);
          packet1.WriteByte((byte) client.Player.CurrentRoom.RoomType);
          packet1.WriteByte((byte) client.Player.CurrentRoom.HardLevel);
          packet1.WriteByte((byte) client.Player.CurrentRoom.LevelLimits);
          packet1.WriteString(client.Player.PlayerCharacter.NickName);
          packet1.WriteBoolean(client.Player.PlayerCharacter.typeVIP > (byte) 0);
          packet1.WriteInt(client.Player.PlayerCharacter.VIPLevel);
          packet1.WriteString(client.Player.CurrentRoom.Name);
          packet1.WriteString(client.Player.CurrentRoom.Password);
          if (client.Player.CurrentRoom.RoomType == eRoomType.Dungeon)
          {
            if (client.Player.CurrentRoom.Game != null)
              packet1.WriteInt((client.Player.CurrentRoom.Game as PVEGame).SessionId);
            else
              packet1.WriteInt(0);
          }
          else
            packet1.WriteInt(-1);
          playerById.Out.SendTCP(packet1);
        }
        else if (playerById != null && playerById.CurrentRoom != null && playerById.CurrentRoom != client.Player.CurrentRoom)
          client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("friendnotinthesameserver.Room"));
        else
          client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("friendnotinthesameserver.Fail"));
      }
      return 0;
    }
  }
}
