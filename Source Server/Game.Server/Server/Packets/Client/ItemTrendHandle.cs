// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ItemTrendHandle
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameUtils;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Text;

namespace Game.Server.Packets.Client
{
  [PacketHandler(120, "物品倾向转移")]
  public class ItemTrendHandle : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      eBageType bagType1 = (eBageType) packet.ReadInt();
      int place1 = packet.ReadInt();
      eBageType bagType2 = (eBageType) packet.ReadInt();
      List<ShopItemInfo> shopItemInfoList = new List<ShopItemInfo>();
      int place2 = packet.ReadInt();
      int Operation = packet.ReadInt();
      ItemInfo itemInfo;
      if (place2 == -1)
      {
        packet.ReadInt();
        packet.ReadInt();
        int num1 = 0;
        int num2 = 0;
        itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(34101), 1, 102);
        List<ShopItemInfo> shopbyTemplatId = ShopMgr.FindShopbyTemplatID(34101);
        for (int index = 0; index < shopbyTemplatId.Count; ++index)
        {
          if (shopbyTemplatId[index].APrice1 == -1 && (uint) shopbyTemplatId[index].AValue1 > 0U)
          {
            num2 = shopbyTemplatId[index].AValue1;
            itemInfo.ValidDate = shopbyTemplatId[index].AUnit;
          }
        }
        if (itemInfo != null)
        {
          if (num1 <= client.Player.PlayerCharacter.Gold && num2 <= client.Player.PlayerCharacter.Money + client.Player.PlayerCharacter.MoneyLock)
          {
            client.Player.RemoveMoney(num2);
            client.Player.RemoveGold(num1);
          }
          else
            itemInfo = (ItemInfo) null;
        }
      }
      else
        itemInfo = client.Player.GetItemAt(bagType2, place2);
      ItemInfo itemAt = client.Player.GetItemAt(bagType1, place1);
      StringBuilder stringBuilder = new StringBuilder();
      if (itemInfo != null && itemAt != null)
      {
        bool result = false;
        ItemTemplateInfo itemTemplateInfo = RefineryMgr.RefineryTrend(Operation, itemAt, ref result);
        if (result && itemTemplateInfo != null)
        {
          ItemInfo fromTemplate = ItemInfo.CreateFromTemplate(itemTemplateInfo, 1, 115);
          AbstractInventory itemInventory = (AbstractInventory) client.Player.GetItemInventory(itemTemplateInfo);
          if (itemInventory.AddItem(fromTemplate, itemInventory.BeginSlot))
          {
            client.Player.UpdateItem(fromTemplate);
            client.Player.RemoveItem(itemAt);
            --itemInfo.Count;
            client.Player.UpdateItem(itemInfo);
            client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemTrendHandle.Success"));
          }
          else
          {
            stringBuilder.Append("NoPlace");
            client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(fromTemplate.GetBagName()) + LanguageMgr.GetTranslation("ItemFusionHandler.NoPlace"));
          }
          return 1;
        }
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemTrendHandle.Fail"));
      }
      return 1;
    }
  }
}
