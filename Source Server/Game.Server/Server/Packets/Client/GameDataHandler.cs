// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.GameDataHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(91, "游戏数据")]
  public class GameDataHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.CurrentRoom != null)
      {
        packet.ClientID = client.Player.PlayerId;
        packet.Parameter1 = client.Player.GameId;
        if (client.Player.CurrentRoom.Game == null && (uint) client.Player.TakeCardTemplateID > 0U)
        {
          GSPacketIn gsPacketIn = packet.Clone();
          gsPacketIn.ClearOffset();
          if (gsPacketIn.ReadByte() == (byte) 98)
          {
            GSPacketIn pkg = new GSPacketIn((short) 91, gsPacketIn.Parameter1);
            pkg.Parameter1 = gsPacketIn.Parameter1;
            pkg.WriteByte((byte) 98);
            pkg.WriteBoolean(true);
            pkg.WriteByte((byte) client.Player.TakeCardPlace);
            pkg.WriteInt(client.Player.TakeCardTemplateID);
            pkg.WriteInt(client.Player.TakeCardCount);
            client.Player.SendTCP(pkg);
          }
          return 0;
        }
        client.Player.CurrentRoom.ProcessData(packet);
      }
      else
        Console.WriteLine("GameDataHandler roomRemoved: " + client.Player.PlayerCharacter.NickName);
      return 0;
    }
  }
}
