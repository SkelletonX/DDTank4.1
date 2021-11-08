// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.SyncSystemDateHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(5, "同步系统数据")]
  public class SyncSystemDateHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      packet.ClearContext();
      packet.WriteDateTime(DateTime.Now);
      client.Out.SendTCP(packet);
      return 0;
    }
  }
}
