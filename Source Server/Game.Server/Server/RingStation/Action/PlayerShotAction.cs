// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.Action.PlayerShotAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

namespace Game.Server.RingStation.Action
{
  public class PlayerShotAction : BaseAction
  {
    private int m_angle;
    private int m_force;
    private int m_x;
    private int m_y;

    public PlayerShotAction(int x, int y, int force, int angle, int delay)
      : base(delay, 0)
    {
      this.m_x = x;
      this.m_y = y;
      this.m_force = force;
      this.m_angle = angle;
    }

    protected override void ExecuteImp(RingStationGamePlayer player, long tick)
    {
      player.SendShootTag(true, 0);
      player.SendGameCMDShoot(this.m_x, this.m_y, this.m_force, this.m_angle);
      this.Finish(tick);
    }
  }
}
