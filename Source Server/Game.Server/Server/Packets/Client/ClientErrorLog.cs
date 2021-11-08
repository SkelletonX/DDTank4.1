// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ClientErrorLog
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using log4net;

namespace Game.Server.Packets.Client
{
  [PacketHandler(8, "客户端日记")]
  public class ClientErrorLog : IPacketHandler
  {
    public static readonly ILog log = LogManager.GetLogger("FlashErrorLogger");

    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      string str = "Client log: " + packet.ReadString();
      ClientErrorLog.log.Error((object) str);
      return 0;
    }
  }
}
