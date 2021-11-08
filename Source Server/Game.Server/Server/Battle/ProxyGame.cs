// Decompiled with JetBrains decompiler
// Type: Game.Server.Battle.ProxyGame
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base;
using Game.Base.Packets;
using Game.Logic;

namespace Game.Server.Battle
{
  public class ProxyGame : AbstractGame
  {
    private FightServerConnector fightServerConnector_0;

    public ProxyGame(
      int id,
      FightServerConnector fightServer,
      eRoomType roomType,
      eGameType gameType,
      int timeType)
      : base(id, roomType, gameType, timeType)
    {
      this.fightServerConnector_0 = fightServer;
      this.fightServerConnector_0.Disconnected += new ClientEventHandle(this.method_0);
    }

    private void method_0(BaseClient baseClient_0)
    {
      this.Stop();
    }

    public override void ProcessData(GSPacketIn pkg)
    {
      this.fightServerConnector_0.SendToGame(this.Id, pkg);
    }
  }
}
