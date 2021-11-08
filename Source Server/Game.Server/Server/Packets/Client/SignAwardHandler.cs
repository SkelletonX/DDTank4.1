// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.SignAwardHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
  [PacketHandler(90, "场景用户离开")]
  public class SignAwardHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int DailyLog = packet.ReadInt();
      string message = "As recompensas foram coletadas com sucesso!";
      if (AwardMgr.AddSignAwards(client.Player, DailyLog))
        client.Out.SendMessage(eMessageType.GM_NOTICE, message);
      return 0;
    }
  }
}
