// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MarryRoomLoginHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using Game.Server.SceneMarryRooms;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(242, "进入礼堂")]
  public class MarryRoomLoginHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      MarryRoom room = (MarryRoom) null;
      string msg = "";
      int id = packet.ReadInt();
      string str = packet.ReadString();
      int num = packet.ReadInt();
      if ((uint) id > 0U)
      {
        room = MarryRoomMgr.GetMarryRoombyID(id, str == null ? "" : str, ref msg);
      }
      else
      {
        if (client.Player.PlayerCharacter.IsCreatedMarryRoom)
        {
          foreach (MarryRoom marryRoom in MarryRoomMgr.GetAllMarryRoom())
          {
            if (marryRoom.Info.GroomID == client.Player.PlayerCharacter.ID || marryRoom.Info.BrideID == client.Player.PlayerCharacter.ID)
            {
              room = marryRoom;
              break;
            }
          }
        }
        if (room == null && (uint) client.Player.PlayerCharacter.SelfMarryRoomID > 0U)
        {
          client.Player.Out.SendMarryRoomLogin(client.Player, false);
          MarryRoomInfo marryRoomInfo = (MarryRoomInfo) null;
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
            marryRoomInfo = playerBussiness.GetMarryRoomInfoSingle(client.Player.PlayerCharacter.SelfMarryRoomID);
          if (marryRoomInfo != null)
          {
            client.Player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("MarryRoomLoginHandler.RoomExist", (object) marryRoomInfo.ServerID, (object) client.Player.PlayerCharacter.SelfMarryRoomID));
            return 0;
          }
        }
      }
      if (room != null)
      {
        if (room.CheckUserForbid(client.Player.PlayerCharacter.ID))
        {
          client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("MarryRoomLoginHandler.Forbid"));
          client.Player.Out.SendMarryRoomLogin(client.Player, false);
          return 1;
        }
        if (room.RoomState == eRoomState.FREE)
        {
          if (room.AddPlayer(client.Player))
          {
            client.Player.MarryMap = num;
            client.Player.Out.SendMarryRoomLogin(client.Player, true);
            room.SendMarryRoomInfoUpdateToScenePlayers(room);
            return 0;
          }
        }
        else
          client.Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("MarryRoomLoginHandler.AlreadyBegin"));
        client.Player.Out.SendMarryRoomLogin(client.Player, false);
      }
      else
      {
        client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation(string.IsNullOrEmpty(msg) ? "MarryRoomLoginHandler.Failed" : msg));
        client.Player.Out.SendMarryRoomLogin(client.Player, false);
      }
      return 1;
    }
  }
}
