﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.SceneChangeChannel
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(24, "用户改变频道")]
  public class SceneChangeChannel : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = (int) packet.ReadByte();
      return 0;
    }
  }
}
