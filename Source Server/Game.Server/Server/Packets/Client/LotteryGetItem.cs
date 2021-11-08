// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.LotteryGetItem
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameUtils;

namespace Game.Server.Packets.Client
{
  [PacketHandler((int)ePackageType.LOTTERY_GET_ITEM, "打开物品")]
  public class LotteryGetItem : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = (int) packet.ReadByte();
      packet.ReadInt();
      PlayerInventory caddyBag = client.Player.CaddyBag;
      PlayerInventory propBag = client.Player.PropBag;
      for (int slot = 0; slot < caddyBag.Capalility; ++slot)
        caddyBag.GetItemAt(slot);
      return 1;
    }
  }
}
