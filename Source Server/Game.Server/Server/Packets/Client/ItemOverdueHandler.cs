// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ItemOverdueHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameUtils;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(77, "物品过期")]
  public class ItemOverdueHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.CurrentRoom == null || !client.Player.CurrentRoom.IsPlaying)
      {
        int num = (int) packet.ReadByte();
        int slot = packet.ReadInt();
        PlayerInventory inventory = client.Player.GetInventory((eBageType) num);
        ItemInfo itemAt = inventory.GetItemAt(slot);
        if (itemAt != null && !itemAt.IsValidItem())
        {
          if (num == 0 && slot < 30)
          {
            int firstEmptySlot = inventory.FindFirstEmptySlot();
            if (firstEmptySlot == -1 || !inventory.MoveItem(itemAt.Place, firstEmptySlot, itemAt.Count))
            {
              client.Player.SendItemToMail(itemAt, LanguageMgr.GetTranslation("ItemOverdueHandler.Content"), LanguageMgr.GetTranslation("ItemOverdueHandler.Title"), eMailType.ItemOverdue);
              client.Player.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Receiver);
            }
          }
          else
            inventory.UpdateItem(itemAt);
        }
      }
      return 0;
    }
  }
}
