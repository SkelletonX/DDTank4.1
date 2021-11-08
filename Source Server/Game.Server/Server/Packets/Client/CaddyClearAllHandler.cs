// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.CaddyClearAllHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameUtils;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(232, "打开物品")]
  public class CaddyClearAllHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      PlayerInventory caddyBag = client.Player.CaddyBag;
      int num1 = 1;
      int num2 = 0;
      int num3 = 0;
      string str1 = "";
      string str2 = "";
      for (int slot = 0; slot < caddyBag.Capalility; ++slot)
      {
        ItemInfo itemAt = caddyBag.GetItemAt(slot);
        if (itemAt != null)
        {
          if (itemAt.Template.ReclaimType == 1)
            num2 += num1 * itemAt.Template.ReclaimValue;
          if (itemAt.Template.ReclaimType == 2)
            num3 += num1 * itemAt.Template.ReclaimValue;
          caddyBag.RemoveItem(itemAt);
        }
      }
      if (num2 > 0)
        str1 = LanguageMgr.GetTranslation("ItemReclaimHandler.Success2", (object) num2);
      if (num3 > 0)
        str2 = LanguageMgr.GetTranslation("ItemReclaimHandler.Success1", (object) num3);
      client.Player.BeginChanges();
      client.Player.AddGold(num2);
      client.Player.AddGiftToken(num3);
      client.Player.CommitChanges();
      client.Out.SendMessage(eMessageType.GM_NOTICE, str1 + " " + str2);
      return 1;
    }
  }
}
