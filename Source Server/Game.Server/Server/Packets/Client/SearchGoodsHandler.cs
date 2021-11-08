// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.SearchGoodsHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Rooms;

namespace Game.Server.Packets.Client
{
  [PacketHandler(98, "客户端日记")]
  public class SearchGoodsHandler : IPacketHandler
  {
    private static ThreadSafeRandom rand = new ThreadSafeRandom();
    private readonly int[] mapID = new int[3]{ 1, 2, 3 };

    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      byte index = packet.ReadByte();
      if (client.Player.CurrentRoom != null && client.Player == client.Player.CurrentRoom.Host)
        RoomMgr.KickPlayer(client.Player.CurrentRoom, index);
      return 0;
    }
  }
}
