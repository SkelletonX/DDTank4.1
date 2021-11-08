// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ChangeDesignationHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(34, "场景用户离开")]
  public class ChangeDesignationHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      bool flag = packet.ReadBoolean();
      client.Player.PlayerCharacter.IsShowConsortia = flag;
      return 0;
    }
  }
}
