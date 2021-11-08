// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.IMHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(160, "添加好友")]
  public class IMHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      switch (packet.ReadByte())
      {
        case 51:
          int num1 = packet.ReadInt();
          string msg = packet.ReadString();
          packet.ReadBoolean();
          GamePlayer playerById = WorldMgr.GetPlayerById(num1);
          if (playerById != null)
          {
            client.Player.Out.sendOneOnOneTalk(num1, false, client.Player.PlayerCharacter.NickName, msg, client.Player.PlayerCharacter.ID);
            playerById.Out.sendOneOnOneTalk(client.Player.PlayerCharacter.ID, false, client.Player.PlayerCharacter.NickName, msg, num1);
            return 1;
          }
          client.Player.Out.SendMessage(eMessageType.GM_NOTICE, "对方不在线!");
          return 1;
        case 160:
          string nickName = packet.ReadString();
          int relation = packet.ReadInt();
          if (relation < 0 || relation > 1)
            return 1;
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
          {
            GamePlayer byPlayerNickName = WorldMgr.GetClientByPlayerNickName(nickName);
            PlayerInfo user = byPlayerNickName == null ? playerBussiness.GetUserSingleByNickName(nickName) : byPlayerNickName.PlayerCharacter;
            if (user == null)
            {
              client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("FriendAddHandler.Success") + nickName);
              return 1;
            }
            if (client.Player.Friends.ContainsKey(user.ID) && client.Player.Friends[user.ID] == relation)
            {
              client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("FriendAddHandler.Falied"));
              return 1;
            }
            if (playerBussiness.AddFriends(new FriendInfo()
            {
              FriendID = user.ID,
              IsExist = true,
              Remark = "",
              UserID = client.Player.PlayerCharacter.ID,
              Relation = relation
            }))
            {
              client.Player.FriendsAdd(user.ID, relation);
              if (relation != 1 && (uint) user.State > 0U)
              {
                GSPacketIn gsPacketIn = new GSPacketIn((short) 160, client.Player.PlayerCharacter.ID);
                gsPacketIn.WriteByte((byte) 166);
                gsPacketIn.WriteInt(user.ID);
                gsPacketIn.WriteString(client.Player.PlayerCharacter.NickName);
                gsPacketIn.WriteBoolean(false);
                if (byPlayerNickName != null)
                  byPlayerNickName.SendTCP(gsPacketIn);
                else
                  GameServer.Instance.LoginServer.SendPacket(gsPacketIn);
              }
              client.Out.SendAddFriend(user, relation, true);
              client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("FriendAddHandler.Success2"));
            }
            return 1;
          }
        case 161:
          int num2 = packet.ReadInt();
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
          {
            if (playerBussiness.DeleteFriends(client.Player.PlayerCharacter.ID, num2))
            {
              client.Player.FriendsRemove(num2);
              client.Out.SendFriendRemove(num2);
            }
            return 1;
          }
        case 165:
          int num3 = packet.ReadInt();
          GSPacketIn packet1 = new GSPacketIn((short) 160, client.Player.PlayerCharacter.ID);
          packet1.WriteByte((byte) 165);
          packet1.WriteInt(num3);
          packet1.WriteByte(client.Player.PlayerCharacter.typeVIP);
          packet1.WriteInt(client.Player.PlayerCharacter.VIPLevel);
          packet1.WriteBoolean(false);
          GameServer.Instance.LoginServer.SendPacket(packet1);
          WorldMgr.ChangePlayerState(client.Player.PlayerCharacter.ID, num3, client.Player.PlayerCharacter.ConsortiaID);
          return 1;
        default:
          return 1;
      }
    }
  }
}
