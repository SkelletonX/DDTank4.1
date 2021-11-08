// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserHideItemHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(60, "隐藏装备")]
  public class UserHideItemHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      bool hide = packet.ReadBoolean();
      int categoryID = packet.ReadInt();
      switch (categoryID)
      {
        case 13:
          categoryID = 3;
          break;
        case 15:
          categoryID = 4;
          break;
      }
      client.Player.HideEquip(categoryID, hide);
      return 0;
    }
  }
}
