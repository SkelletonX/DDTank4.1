// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserDeleteItemHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(42, "删除物品")]
  public class UserDeleteItemHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
      {
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
        return 0;
      }
      int num = (int) packet.ReadByte();
      packet.ReadInt();
      return 0;
    }
  }
}
