// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MarryInfoGetHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(235, "获取征婚信息")]
  internal class MarryInfoGetHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if ((uint) client.Player.PlayerCharacter.MarryInfoID > 0U)
      {
        int ID = packet.ReadInt();
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          MarryInfo marryInfoSingle = playerBussiness.GetMarryInfoSingle(ID);
          if (marryInfoSingle != null)
          {
            client.Player.Out.SendMarryInfo(client.Player, marryInfoSingle);
            return 0;
          }
        }
      }
      return 1;
    }
  }
}
