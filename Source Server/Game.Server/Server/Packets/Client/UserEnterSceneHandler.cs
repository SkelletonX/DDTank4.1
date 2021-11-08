// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserEnterSceneHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
  [PacketHandler(16, "Player enter scene.")]
  public class UserEnterSceneHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      switch (packet.ReadInt())
      {
        case 1:
          client.Player.PlayerState = ePlayerState.Manual;
          break;
        case 2:
          client.Player.PlayerState = ePlayerState.Away;
          break;
        case 8:
          client.Player.PlayerState = ePlayerState.Busy;
          break;
        default:
          client.Player.PlayerState = ePlayerState.Manual;
          break;
      }
      if (WorldMgr.HotSpringScene.GetClientFromID(client.Player.PlayerCharacter.ID) != null)
        WorldMgr.HotSpringScene.RemovePlayer(client.Player);
      return 1;
    }
  }
}
