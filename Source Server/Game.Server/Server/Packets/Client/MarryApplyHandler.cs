// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MarryApplyHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System.Linq;

namespace Game.Server.Packets.Client
{
  [PacketHandler(247, "求婚")]
  internal class MarryApplyHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.PlayerCharacter.IsMarried)
        return 1;
      if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
      {
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
        return 1;
      }
      int num = packet.ReadInt();
      string loveProclamation = packet.ReadString();
      packet.ReadBoolean();
      bool flag = true;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        PlayerInfo userSingleByUserId = playerBussiness.GetUserSingleByUserID(num);
        if (userSingleByUserId == null || userSingleByUserId.Sex == client.Player.PlayerCharacter.Sex)
          return 1;
        if (userSingleByUserId.IsMarried)
        {
          client.Player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("MarryApplyHandler.Msg2"));
          return 1;
        }
        ItemInfo itemByTemplateId = client.Player.PropBag.GetItemByTemplateID(0, 11103);
        if (itemByTemplateId == null)
        {
          ShopItemInfo shopItemInfo = ShopMgr.FindShopbyTemplatID(11103).FirstOrDefault<ShopItemInfo>();
          if (shopItemInfo == null)
          {
            client.Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("MarryApplyHandler.Msg6"));
            return 1;
          }
          if (!client.Player.MoneyDirect(shopItemInfo.AValue1, false))
            return 1;
          flag = false;
        }
        MarryApplyInfo info = new MarryApplyInfo()
        {
          UserID = num,
          ApplyUserID = client.Player.PlayerCharacter.ID,
          ApplyUserName = client.Player.PlayerCharacter.NickName,
          ApplyType = 1,
          LoveProclamation = loveProclamation,
          ApplyResult = false
        };
        int id = 0;
        if (playerBussiness.SavePlayerMarryNotice(info, 0, ref id))
        {
          if (flag)
            client.Player.RemoveItem(itemByTemplateId);
          else
            ShopMgr.FindShopbyTemplatID(11103).FirstOrDefault<ShopItemInfo>();
          client.Player.Out.SendPlayerMarryApply(client.Player, client.Player.PlayerCharacter.ID, client.Player.PlayerCharacter.NickName, loveProclamation, id);
          GameServer.Instance.LoginServer.SendUpdatePlayerMarriedStates(num);
          string nickName = userSingleByUserId.NickName;
          client.Player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("MarryApplyHandler.Msg3"));
        }
      }
      return 0;
    }
  }
}
