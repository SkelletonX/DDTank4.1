// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserSendGiftHandler
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
  [PacketHandler(221, "领取奖品")]
  public class UserSendGiftHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      string nickName = packet.ReadString();
      int ID = packet.ReadInt();
      int num1 = packet.ReadInt();
      packet.ReadInt();
      if (nickName == client.Player.PlayerCharacter.NickName || num1 <= 0 || num1 > 9999)
        return 0;
      ShopItemInfo shopItemInfoById = ShopMgr.GetShopItemInfoById(ID);
      if (shopItemInfoById == null || shopItemInfoById.AValue1 <= 0)
        return 0;
      GamePlayer byPlayerNickName = WorldMgr.GetClientByPlayerNickName(nickName);
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        PlayerInfo playerInfo = byPlayerNickName == null ? playerBussiness.GetUserSingleByNickName(nickName) : byPlayerNickName.PlayerCharacter;
        if (playerInfo != null)
        {
          int num2 = shopItemInfoById.AValue1 * num1;
          if (client.Player.RemoveMoney(num2) > 0)
          {
            ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(shopItemInfoById.TemplateID);
            int int_1 = itemTemplate.Property2 * num1;
            playerBussiness.AddUserGift(new UserGiftInfo()
            {
              SenderID = client.Player.PlayerCharacter.ID,
              ReceiverID = playerInfo.ID,
              TemplateID = itemTemplate.TemplateID,
              Count = num1
            });
            playerBussiness.UpdateUserCharmGP(playerInfo.ID, int_1);
            if (playerBussiness.SendMail(new MailInfo()
            {
              SenderID = client.Player.PlayerCharacter.ID,
              Sender = client.Player.PlayerCharacter.NickName,
              ReceiverID = playerInfo.ID,
              Receiver = playerInfo.NickName,
              Title = LanguageMgr.GetTranslation("UserGiftSystem.MailTitle"),
              Content = client.Player.PlayerCharacter.NickName + LanguageMgr.GetTranslation("GoodsPresentHandler.Content") + itemTemplate.Name + "]",
              Type = 55
            }) && byPlayerNickName != null)
            {
              byPlayerNickName.PlayerCharacter.charmGP += int_1;
              byPlayerNickName.SendUpdatePublicPlayer();
              byPlayerNickName.Out.SendMailResponse(byPlayerNickName.PlayerCharacter.ID, eMailRespose.Gift);
            }
            GSPacketIn pkg = new GSPacketIn((short) 221);
            pkg.WriteBoolean(true);
            client.SendTCP(pkg);
            client.Player.SendMessage(LanguageMgr.GetTranslation("GoodsPresentHandler.Success"));
          }
          else
            client.Player.SendMessage(LanguageMgr.GetTranslation("GoodsPresentHandler.NoMoney"));
        }
        else
          client.Player.SendMessage(LanguageMgr.GetTranslation("GoodsPresentHandler.NoUser"));
      }
      return 0;
    }
  }
}
