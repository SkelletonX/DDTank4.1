// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingBoltMoveAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class LivingBoltMoveAction : BaseAction
  {
    private int m_x;
    private int m_y;
    private string m_action;
    private Living m_living;

    public LivingBoltMoveAction(
      Living living,
      int toX,
      int toY,
      string action,
      int delay,
      int finishTime)
      : base(delay, finishTime)
    {
      this.m_living = living;
      this.m_x = toX;
      this.m_y = toY;
      this.m_action = action;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      this.m_living.SetXY(this.m_x, this.m_y);
      this.m_living.StartMoving();
      game.SendLivingBoltMove(this.m_living);
      this.Finish(tick);
    }
  }
}
