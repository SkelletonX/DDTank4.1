// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.GameTakeTempItemsHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.GameUtils;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
  [PacketHandler(108, "选取")]
  public class GameTakeTempItemsHandler : IPacketHandler
  {
    private bool GetItem(GamePlayer player, ItemInfo item, ref string message)
    {
      if (item != null)
      {
        if (item.Template.BagType == eBageType.Card)
        {
          if (player.CardBag.AddCard(item.Template.TemplateID, item.Count))
            player.TempBag.RemoveItem(item);
          return true;
        }
        PlayerInventory itemInventory = player.GetItemInventory(item.Template);
        if (itemInventory.AddItem(item))
        {
          player.TempBag.RemoveItem(item);
          item.IsExist = true;
          return true;
        }
        itemInventory.UpdateChangedPlaces();
        message = LanguageMgr.GetTranslation("GameTakeTempItemsHandler.Msg");
        List<ItemInfo> items = player.TempBag.GetItems();
        if (player.SendItemsToMail(items, "Itens enviados para o correio, inventário cheio", "Mochila cheia", eMailType.ItemOverdue))
        {
          foreach (ItemInfo itemInfo in items)
            player.TempBag.RemoveItem(itemInfo);
        }
      }
      return false;
    }

    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      string empty = string.Empty;
      int slot = packet.ReadInt();
      if (slot != -1)
      {
        ItemInfo itemAt = client.Player.TempBag.GetItemAt(slot);
        this.GetItem(client.Player, itemAt, ref empty);
      }
      else
      {
        foreach (ItemInfo itemInfo in client.Player.TempBag.GetItems())
        {
          if (!this.GetItem(client.Player, itemInfo, ref empty))
            break;
        }
      }
      if (!string.IsNullOrEmpty(empty))
        client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, empty);
      return 0;
    }
  }
}
