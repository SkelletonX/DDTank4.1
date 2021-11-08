// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserSynchActionHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
  [PacketHandler(36, "用户同步动作")]
  public class UserSynchActionHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      GamePlayer playerById = WorldMgr.GetPlayerById(packet.ClientID);
      if (playerById != null)
      {
        packet.Code = (short) 35;
        packet.ClientID = client.Player.PlayerCharacter.ID;
        playerById.Out.SendTCP(packet);
      }
      return 1;
    }
  }
}
