// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MarryApplyReplyHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(250, "求婚答复")]
  internal class MarryApplyReplyHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      bool result = packet.ReadBoolean();
      int UserID = packet.ReadInt();
      int answerId = packet.ReadInt();
      if (result && client.Player.PlayerCharacter.IsMarried)
        client.Player.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("MarryApplyReplyHandler.Msg2"));
      using (PlayerBussiness db = new PlayerBussiness())
      {
        PlayerInfo userSingleByUserId = db.GetUserSingleByUserID(UserID);
        if (!result)
        {
          this.SendGoodManCard(userSingleByUserId.NickName, userSingleByUserId.ID, client.Player.PlayerCharacter.NickName, client.Player.PlayerCharacter.ID, db);
          GameServer.Instance.LoginServer.SendUpdatePlayerMarriedStates(userSingleByUserId.ID);
        }
        if (userSingleByUserId == null || userSingleByUserId.Sex == client.Player.PlayerCharacter.Sex)
          return 1;
        if (userSingleByUserId.IsMarried)
          client.Player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("MarryApplyReplyHandler.Msg3"));
        MarryApplyInfo info = new MarryApplyInfo()
        {
          UserID = UserID,
          ApplyUserID = client.Player.PlayerCharacter.ID,
          ApplyUserName = client.Player.PlayerCharacter.NickName,
          ApplyType = 2,
          LoveProclamation = "",
          ApplyResult = result
        };
        int id = 0;
        new PlayerBussiness().AddDailyRecord(new DailyRecordInfo()
        {
          UserID = client.Player.PlayerCharacter.ID,
          Type = 3,
          Value = client.Player.PlayerCharacter.SpouseName
        });
        if (db.SavePlayerMarryNotice(info, answerId, ref id))
        {
          if (result)
          {
            client.Player.Out.SendMarryApplyReply(client.Player, userSingleByUserId.ID, userSingleByUserId.NickName, result, false, id);
            client.Player.LoadMarryProp();
          }
          GameServer.Instance.LoginServer.SendUpdatePlayerMarriedStates(userSingleByUserId.ID);
          return 0;
        }
      }
      return 1;
    }

    public void SendGoodManCard(
      string receiverName,
      int receiverID,
      string senderName,
      int senderID,
      PlayerBussiness db)
    {
      ItemInfo fromTemplate = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11105), 1, 112);
      fromTemplate.IsBinds = true;
      fromTemplate.UserID = 0;
      db.AddGoods(fromTemplate);
      MailInfo mail = new MailInfo()
      {
        Annex1 = fromTemplate.ItemID.ToString(),
        Content = LanguageMgr.GetTranslation("MarryApplyReplyHandler.Content"),
        Gold = 0,
        IsExist = true,
        Money = 0,
        Receiver = receiverName,
        ReceiverID = receiverID,
        Sender = senderName,
        SenderID = senderID,
        Title = LanguageMgr.GetTranslation("MarryApplyReplyHandler.Title"),
        Type = 14
      };
      db.SendMail(mail);
    }

    public void SendSYSMessages(GamePlayer player, PlayerInfo spouse)
    {
      string translation = LanguageMgr.GetTranslation("MarryApplyReplyHandler.Msg1", player.PlayerCharacter.Sex ? (object) player.PlayerCharacter.NickName : (object) spouse.NickName, player.PlayerCharacter.Sex ? (object) spouse.NickName : (object) player.PlayerCharacter.NickName);
      player.OnPlayerMarry();
      GSPacketIn packet = new GSPacketIn((short) 10);
      packet.WriteInt(2);
      packet.WriteString(translation);
      GameServer.Instance.LoginServer.SendPacket(packet);
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
        allPlayer.Out.SendTCP(packet);
    }
  }
}
