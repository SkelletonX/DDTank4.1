// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MarryStateHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Rooms;

namespace Game.Server.Packets.Client
{
  [PacketHandler(251, "当前场景状态")]
  internal class MarryStateHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      switch (packet.ReadInt())
      {
        case 0:
          if (client.Player.IsInMarryRoom)
          {
            if (client.Player.MarryMap != 1)
            {
              if (client.Player.MarryMap == 2)
              {
                client.Player.X = 800;
                client.Player.Y = 763;
              }
            }
            else
            {
              client.Player.X = 646;
              client.Player.Y = 1241;
            }
            foreach (GamePlayer allPlayer in client.Player.CurrentMarryRoom.GetAllPlayers())
            {
              if (allPlayer != client.Player && allPlayer.MarryMap == client.Player.MarryMap)
              {
                allPlayer.Out.SendPlayerEnterMarryRoom(client.Player);
                client.Player.Out.SendPlayerEnterMarryRoom(allPlayer);
              }
            }
            break;
          }
          break;
        case 1:
          RoomMgr.EnterWaitingRoom(client.Player);
          break;
      }
      return 0;
    }
  }
}
