// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.EnterRoomHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Rooms;

namespace Game.Server.Packets.Client
{
  [PacketHandler(81, "游戏创建")]
  public class EnterRoomHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      bool isInvite = packet.ReadBoolean();
      int type = packet.ReadInt();
      int num = packet.ReadInt();
      int roomId = -1;
      string pwd = (string) null;
      if (num == -1)
      {
        roomId = packet.ReadInt();
        pwd = packet.ReadString();
      }
      switch (type)
      {
        case 1:
          type = 0;
          break;
        case 2:
          type = 4;
          break;
      }
      RoomMgr.EnterRoom(client.Player, roomId, pwd, type, isInvite);
      return 0;
    }
  }
}
