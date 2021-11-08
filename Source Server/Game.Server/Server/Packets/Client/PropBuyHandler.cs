// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.PropBuyHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Linq;

namespace Game.Server.Packets.Client
{
  [PacketHandler((byte)ePackageType.PROP_BUY, "compra Rapida De Item")]
  public class PropBuyHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int gold = 0;
      int money = 0;
      int offer = 0;
      int gifttoken = 0;
      int num = 0;
      int damageScore = 0;
      int petScore = 0;
      int iTemplateID = 0;
      int iCount = 0;
      int hardCurrency = 0;
      int LeagueMoney = 0;
      int ID = packet.ReadInt();
      int type = 1;
      ShopItemInfo shopItemInfoById = ShopMgr.GetShopItemInfoById(ID);
      if (((IEnumerable<int>) PropItemMgr.PropFightBag).ToList<int>().Contains(shopItemInfoById.TemplateID) && shopItemInfoById != null)
      {
        ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(shopItemInfoById.TemplateID);
        ShopMgr.SetItemType(shopItemInfoById, type, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref num, ref hardCurrency, ref LeagueMoney, ref num);
        if (itemTemplate.CategoryID == 10)
        {
          PlayerInfo playerCharacter = client.Player.PlayerCharacter;
          if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked && (money > 0 || offer > 0 || (gifttoken > 0 || num > 0)))
          {
            client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
            return 0;
          }
          if (gold <= playerCharacter.Gold && (money <= (playerCharacter.Money < 0 ? 0 : playerCharacter.Money) && offer <= playerCharacter.Offer && (gifttoken <= playerCharacter.GiftToken && num <= playerCharacter.medal)))
          {
            ItemInfo fromTemplate = ItemInfo.CreateFromTemplate(itemTemplate, 1, 102);
            if (shopItemInfoById.BuyType == 0)
            {
              if (1 == type)
                fromTemplate.ValidDate = shopItemInfoById.AUnit;
              if (2 == type)
                fromTemplate.ValidDate = shopItemInfoById.BUnit;
              if (3 == type)
                fromTemplate.ValidDate = shopItemInfoById.CUnit;
            }
            else
            {
              if (1 == type)
                fromTemplate.Count = shopItemInfoById.AUnit;
              if (2 == type)
                fromTemplate.Count = shopItemInfoById.BUnit;
              if (3 == type)
                fromTemplate.Count = shopItemInfoById.CUnit;
            }
            if (client.Player.FightBag.AddItem(fromTemplate, 0))
            {
              client.Player.RemoveGold(gold);
              client.Player.RemoveMoney(money);
              client.Player.RemoveOffer(offer);
              client.Player.RemoveGiftToken(gifttoken);
              client.Player.RemoveMedal(num);
            }
          }
          else
            client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("PropBuyHandler.NoMoney"));
        }
      }
      return 0;
    }
  }
}
