// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserWeeklyClickHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(219, "场景用户离开")]
  public class UserWeeklyClickHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      GSPacketIn pkg = new GSPacketIn((short) 219, client.Player.PlayerCharacter.ID);
      pkg.WriteBoolean(false);
      client.SendTCP(pkg);
      return 0;
    }
  }
}
