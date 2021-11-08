// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.LotteryFinishBoxHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameUtils;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
  [PacketHandler((int)ePackageType.LOTTERY_FINISH, "打开物品")]
  public class LotteryFinishBoxHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      PlayerInventory caddyBag = client.Player.CaddyBag;
      List<ItemInfo> items1 = new List<ItemInfo>();
      for (int slot = 0; slot < caddyBag.Capalility; ++slot)
      {
        ItemInfo itemAt = caddyBag.GetItemAt(slot);
        if (itemAt != null)
        {
          if (!client.Player.AddItem(itemAt))
            items1.Add(itemAt);
          caddyBag.TakeOutItem(itemAt);
        }
      }
      if (items1.Count > 0)
        client.Player.SendItemsToMail(items1, "Abrindo jarro mágico, itens no correio", "Recompensas do Jarro Mágico", eMailType.BuyItem);
      if (client.Player.Lottery != -1 && client.Player.LotteryAwardList.Count > 0)
      {
        List<ItemInfo> items2 = new List<ItemInfo>();
        foreach (ItemInfo lotteryAward in client.Player.LotteryAwardList)
        {
          if (!client.Player.AddItem(lotteryAward))
            items2.Add(lotteryAward);
        }
        if (items2.Count > 0)
        {
          client.Player.SendItemsToMail(items2, "Mochila cheia", "Mochila cheia", eMailType.BuyItem);
          client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.Lottery.Oversea.MailNotice", (object) ""));
        }
        client.Player.ResetLottery();
      }
      else
        client.Player.ResetLottery();
      return 1;
    }
  }
}
