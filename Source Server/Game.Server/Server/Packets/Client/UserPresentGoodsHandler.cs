// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserPresentGoodsHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using System.Text;

namespace Game.Server.Packets.Client
{
  [PacketHandler(57, "购买物品")]
  public class UserPresentGoodsHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      StringBuilder stringBuilder = new StringBuilder();
      packet.ReadString();
      packet.ReadString();
      packet.ReadInt();
      client.Player.SendMessage("Presente enviado com sucesso!");
      return 0;
    }
  }
}
