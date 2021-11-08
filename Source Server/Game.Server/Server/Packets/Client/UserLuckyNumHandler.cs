// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserLuckyNumHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(161, "场景用户离开")]
  public class UserLuckyNumHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
            packet.ReadBoolean();
            packet.ReadInt();
            GSPacketIn dab = new GSPacketIn((int)ePackageType.USER_LUCKYNUM);
            dab.WriteInt(0);
            dab.WriteString("");
            client.SendTCP(dab);
            return 1;
            /*
      packet.ReadBoolean(); 3.1
      packet.ReadInt();
      return 1; */
        }
  }
}
