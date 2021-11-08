// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ChickenBoxHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Rooms;

namespace Game.Server.Packets.Client
{
  [PacketHandler(87, "客户端日记")]
  public class ChickenBoxHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.MainWeapon == null)
      {
        client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.SceneGames.NoEquip"));
        return 0;
      }
      if (client.Player.CurrentRoom != null)
        RoomMgr.UpdatePlayerState(client.Player, packet.ReadByte());
      return 0;
    }
  }
}
