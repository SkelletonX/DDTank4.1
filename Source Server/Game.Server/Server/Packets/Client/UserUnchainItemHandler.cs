// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserUnchainItemHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(47, "解除物品")]
  public class UserUnchainItemHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.CurrentRoom == null || !client.Player.CurrentRoom.IsPlaying)
      {
        int fromSlot = packet.ReadInt();
        int firstEmptySlot = client.Player.EquipBag.FindFirstEmptySlot(31);
        client.Player.EquipBag.MoveItem(fromSlot, firstEmptySlot, 0);
      }
      return 0;
    }
  }
}
