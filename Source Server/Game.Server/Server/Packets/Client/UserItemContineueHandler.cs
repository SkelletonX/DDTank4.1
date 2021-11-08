// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserItemContineueHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;
using System.Text;

namespace Game.Server.Packets.Client
{
  [PacketHandler(62, "续费")]
  public class UserItemContineueHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
      {
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
        return 0;
      }
      StringBuilder stringBuilder = new StringBuilder();
      int num1 = packet.ReadInt();
      string translateId = "UserItemContineueHandler.Success";
      for (int index = 0; index < num1; ++index)
      {
        eBageType bagType = (eBageType) packet.ReadByte();
        int num2 = packet.ReadInt();
        int ID = packet.ReadInt();
        int type = (int) packet.ReadByte();
        bool flag1 = packet.ReadBoolean();
        packet.ReadInt();
        ShopItemInfo shopItemInfoById = ShopMgr.GetShopItemInfoById(ID);
        SqlDataProvider.Data.ItemInfo itemAt = client.Player.GetItemAt(bagType, num2);
        if (bagType == eBageType.EquipBag && itemAt != null && (shopItemInfoById != null && shopItemInfoById.TemplateID == itemAt.TemplateID))
        {
          if ((uint) itemAt.ValidDate > 0U)
          {
            int gold = 0;
            int money = 0;
            int offer = 0;
            int gifttoken = 0;
            int num3 = 0;
            int damageScore = 0;
            int petScore = 0;
            int iTemplateID = 0;
            int iCount = 0;
            int hardCurrency = 0;
            int LeagueMoney = 0;
            int validDate = itemAt.ValidDate;
            DateTime beginDate = itemAt.BeginDate;
            int count = itemAt.Count;
            bool flag2 = itemAt.IsValidItem();
            if (!ShopMgr.SetItemType(shopItemInfoById, type, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref num3, ref hardCurrency, ref LeagueMoney, ref num3))
            {
              client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Não foi possível renovar o item."));
              return 0;
            }
            if (gold <= client.Player.PlayerCharacter.Gold && money <= client.Player.PlayerCharacter.Money + client.Player.PlayerCharacter.MoneyLock && (offer <= client.Player.PlayerCharacter.Offer && gifttoken <= client.Player.PlayerCharacter.GiftToken))
            {
              if (!flag2)
              {
                if (1 == type)
                  itemAt.ValidDate = shopItemInfoById.AUnit;
                if (2 == type)
                  itemAt.ValidDate = shopItemInfoById.BUnit;
                if (3 == type)
                  itemAt.ValidDate = shopItemInfoById.CUnit;
                itemAt.BeginDate = DateTime.Now;
                itemAt.IsUsed = true;
                client.Player.RemoveMoney(money);
                client.Player.RemoveGold(gold);
                client.Player.RemoveOffer(offer);
                client.Player.RemoveGiftToken(gifttoken);
              }
              else
                translateId = "Falha ao efetuar a renovação do item";
            }
            else
            {
              itemAt.ValidDate = validDate;
              itemAt.Count = count;
              translateId = "UserItemContineueHandler.NoMoney";
            }
          }
          if (flag1)
          {
            int itemEpuipSlot = client.Player.EquipBag.FindItemEpuipSlot(itemAt.Template);
            if (client.Player.GetItemAt(bagType, itemEpuipSlot) == null && num2 > client.Player.EquipBag.BeginSlot)
              client.Player.EquipBag.MoveItem(num2, itemEpuipSlot, 1);
          }
          else
            client.Player.EquipBag.UpdateItem(itemAt);
        }
      }
      client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(translateId));
      return 0;
    }
  }
}
