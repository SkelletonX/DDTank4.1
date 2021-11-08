// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.Action.PlayerUsePropAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

namespace Game.Server.RingStation.Action
{
  public class PlayerUsePropAction : BaseAction
  {
    private int m_prop;

    public PlayerUsePropAction(int prop, int delay)
      : base(delay, 0)
    {
      this.m_prop = prop;
    }

    protected override void ExecuteImp(RingStationGamePlayer player, long tick)
    {
      player.SendUseProp(this.m_prop);
      this.Finish(tick);
    }
  }
}
