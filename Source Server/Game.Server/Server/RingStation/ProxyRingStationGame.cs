// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.ProxyRingStationGame
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.RingStation.Battle;

namespace Game.Server.RingStation
{
  public class ProxyRingStationGame : AbstractGame
  {
    private RingStationFightConnector m_fightServer;

    public ProxyRingStationGame(
      int id,
      RingStationFightConnector fightServer,
      eRoomType roomType,
      eGameType gameType,
      int timeType)
      : base(id, roomType, gameType, timeType)
    {
      this.m_fightServer = fightServer;
      this.m_fightServer.Disconnected += new ClientEventHandle(this.m_fightingServer_Disconnected);
    }

    private void m_fightingServer_Disconnected(BaseClient client)
    {
      this.Stop();
    }

    public override void ProcessData(GSPacketIn pkg)
    {
      this.m_fightServer.SendToGame(this.Id, pkg);
    }
  }
}
