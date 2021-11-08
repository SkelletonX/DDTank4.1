// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingDirectSetXYAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class LivingDirectSetXYAction : BaseAction
  {
    private Living m_living;
    private int m_x;
    private int m_y;

    public LivingDirectSetXYAction(Living living, int x, int y, int delay)
      : base(delay)
    {
      this.m_living = living;
      this.m_x = x;
      this.m_y = y;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      this.m_living.SetXY(this.m_x, this.m_y);
      this.Finish(tick);
    }
  }
}
