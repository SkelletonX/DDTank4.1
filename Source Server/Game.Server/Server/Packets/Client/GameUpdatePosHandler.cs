// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.GameUpdatePosHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(100, "客户端日记")]
  public class GameUpdatePosHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.CurrentRoom != null && client.Player == client.Player.CurrentRoom.Host)
      {
        byte num1 = packet.ReadByte();
        int num2 = packet.ReadInt();
        packet.ReadBoolean();
        packet.ReadInt();
        if ((num1 == (byte) 8 || num1 == (byte) 9) && num2 == -1)
        {
          client.Player.SendMessage("Você deve alcançar o nível 40 para abrir salas com modo espectador");
          return 0;
        }
      }
      return 0;
    }
  }
}
