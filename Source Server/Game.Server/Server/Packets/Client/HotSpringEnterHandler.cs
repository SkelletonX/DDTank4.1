// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.HotSpringEnterHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.HotSpringRooms;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
  [PacketHandler((byte)ePackageType.HOTSPRING_ENTER, "礼堂数据")]
  public class HotSpringEnterHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (WorldMgr.HotSpringScene.GetClientFromID(client.Player.PlayerCharacter.ID) == null)
        WorldMgr.HotSpringScene.AddPlayer(client.Player);
      HotSpringRoom[] allHotSpringRoom = HotSpringMgr.GetAllHotSpringRoom();
      HotSpringMgr.SendUpdateAllRoom(client.Player, allHotSpringRoom);
      return 0;
    }
  }
}
