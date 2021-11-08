// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.Action.PlayerBuffStuntAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

namespace Game.Server.RingStation.Action
{
  public class PlayerBuffStuntAction : BaseAction
  {
    private int m_type;

    public PlayerBuffStuntAction(int type, int delay)
      : base(delay, 0)
    {
      this.m_type = type;
    }

    protected override void ExecuteImp(RingStationGamePlayer player, long tick)
    {
      player.sendGameCMDStunt(this.m_type);
      this.Finish(tick);
    }
  }
}
