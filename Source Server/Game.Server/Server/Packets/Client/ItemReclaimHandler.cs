// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ItemReclaimHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameUtils;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(127, "物品比较")]
  public class ItemReclaimHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      eBageType bageType = (eBageType) packet.ReadByte();
      int num1 = packet.ReadInt();
      int num2 = packet.ReadInt();
      PlayerInventory inventory = client.Player.GetInventory(bageType);
      if (inventory != null && inventory.GetItemAt(num1) != null)
      {
        if (inventory.GetItemAt(num1).Count <= num2)
          num2 = inventory.GetItemAt(num1).Count;
        ItemTemplateInfo template = inventory.GetItemAt(num1).Template;
        int num3 = num2 * template.ReclaimValue;
        if (template.ReclaimType == 2)
        {
          client.Player.AddGiftToken(num3);
          client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemReclaimHandler.Success1", (object) num3));
        }
        if (template.ReclaimType == 1)
        {
          client.Player.AddGold(num3);
          client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemReclaimHandler.Success2", (object) num3));
        }
        if (template.TemplateID == 11408)
          client.Player.RemoveMedal(num2);
        inventory.RemoveItemAt(num1);
        return 0;
      }
      client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemReclaimHandler.NoSuccess"));
      return 1;
    }
  }
}
