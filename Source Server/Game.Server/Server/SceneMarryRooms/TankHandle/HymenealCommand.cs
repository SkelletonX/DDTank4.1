// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.TankHandle.HymenealCommand
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets;
using SqlDataProvider.Data;

namespace Game.Server.SceneMarryRooms.TankHandle
{
  [MarryCommandAttbute(2)]
  public class HymenealCommand : IMarryCommandHandler
  {
    public bool HandleCommand(
      TankMarryLogicProcessor process,
      GamePlayer player,
      GSPacketIn packet)
    {
      if (player.CurrentMarryRoom == null || player.CurrentMarryRoom.RoomState != eRoomState.FREE || player.PlayerCharacter.ID != player.CurrentMarryRoom.Info.GroomID && player.PlayerCharacter.ID != player.CurrentMarryRoom.Info.BrideID)
        return false;
      int pricePropose = GameProperties.PRICE_PROPOSE;
      if (player.CurrentMarryRoom.Info.IsHymeneal && player.PlayerCharacter.Money + player.PlayerCharacter.MoneyLock < pricePropose)
      {
        player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("UserFirecrackersCommand.MoneyNotEnough"));
        return false;
      }
      GamePlayer playerByUserId1 = player.CurrentMarryRoom.GetPlayerByUserID(player.CurrentMarryRoom.Info.GroomID);
      if (playerByUserId1 == null)
      {
        player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("HymenealCommand.NoGroom"));
        return false;
      }
      GamePlayer playerByUserId2 = player.CurrentMarryRoom.GetPlayerByUserID(player.CurrentMarryRoom.Info.BrideID);
      if (playerByUserId2 == null)
      {
        player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("HymenealCommand.NoBride"));
        return false;
      }
      bool val = false;
      GSPacketIn packet1 = packet.Clone();
      if (1 == packet.ReadInt())
      {
        player.CurrentMarryRoom.RoomState = eRoomState.FREE;
      }
      else
      {
        player.CurrentMarryRoom.RoomState = eRoomState.Hymeneal;
        player.CurrentMarryRoom.BeginTimerForHymeneal(170000);
        bool flag;
        if (!player.PlayerCharacter.IsGotRing)
        {
          flag = true;
          ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(9022);
          ItemInfo fromTemplate1 = ItemInfo.CreateFromTemplate(itemTemplate, 1, 112);
          fromTemplate1.IsBinds = true;
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
          {
            fromTemplate1.UserID = 0;
            playerBussiness.AddGoods(fromTemplate1);
            string translation = LanguageMgr.GetTranslation("HymenealCommand.Content", (object) playerByUserId2.PlayerCharacter.NickName);
            MailInfo mail = new MailInfo()
            {
              Annex1 = fromTemplate1.ItemID.ToString(),
              Content = translation,
              Gold = 0,
              IsExist = true,
              Money = 0,
              Receiver = playerByUserId1.PlayerCharacter.NickName,
              ReceiverID = playerByUserId1.PlayerCharacter.ID,
              Sender = LanguageMgr.GetTranslation("HymenealCommand.Sender"),
              SenderID = 0,
              Title = LanguageMgr.GetTranslation("HymenealCommand.Title"),
              Type = 14
            };
            if (playerBussiness.SendMail(mail))
              val = true;
            player.Out.SendMailResponse(mail.ReceiverID, eMailRespose.Receiver);
          }
          ItemInfo fromTemplate2 = ItemInfo.CreateFromTemplate(itemTemplate, 1, 112);
          fromTemplate2.IsBinds = true;
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
          {
            fromTemplate2.UserID = 0;
            playerBussiness.AddGoods(fromTemplate2);
            string translation = LanguageMgr.GetTranslation("HymenealCommand.Content", (object) playerByUserId1.PlayerCharacter.NickName);
            MailInfo mail = new MailInfo()
            {
              Annex1 = fromTemplate2.ItemID.ToString(),
              Content = translation,
              Gold = 0,
              IsExist = true,
              Money = 0,
              Receiver = playerByUserId2.PlayerCharacter.NickName,
              ReceiverID = playerByUserId2.PlayerCharacter.ID,
              Sender = LanguageMgr.GetTranslation("HymenealCommand.Sender"),
              SenderID = 0,
              Title = LanguageMgr.GetTranslation("HymenealCommand.Title"),
              Type = 14
            };
            if (playerBussiness.SendMail(mail))
              val = true;
            player.Out.SendMailResponse(mail.ReceiverID, eMailRespose.Receiver);
          }
          player.CurrentMarryRoom.Info.IsHymeneal = true;
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
          {
            playerBussiness.UpdateMarryRoomInfo(player.CurrentMarryRoom.Info);
            playerBussiness.UpdatePlayerGotRingProp(playerByUserId1.PlayerCharacter.ID, playerByUserId2.PlayerCharacter.ID);
            playerByUserId1.LoadMarryProp();
            playerByUserId2.LoadMarryProp();
          }
        }
        else
        {
          flag = false;
          val = true;
        }
        if (!flag)
        {
          player.RemoveMoney(pricePropose);
          CountBussiness.InsertSystemPayCount(player.PlayerCharacter.ID, pricePropose, 0, 0, 1);
        }
        packet1.WriteInt(player.CurrentMarryRoom.Info.ID);
        packet1.WriteBoolean(val);
        player.CurrentMarryRoom.SendToAll(packet1);
        if (val)
        {
          string translation = LanguageMgr.GetTranslation("HymenealCommand.Succeed", (object) playerByUserId1.PlayerCharacter.NickName, (object) playerByUserId2.PlayerCharacter.NickName);
          GSPacketIn packet2 = player.Out.SendMessage(eMessageType.ChatNormal, translation);
          player.CurrentMarryRoom.SendToPlayerExceptSelfForScene(packet2, player);
        }
      }
      return true;
    }
  }
}
