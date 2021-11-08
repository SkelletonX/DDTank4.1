// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingDieAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class LivingDieAction : BaseAction
  {
    private Living m_living;

    public LivingDieAction(Living living, int delay)
      : base(delay, 1000)
    {
      this.m_living = living;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      this.m_living.Die();
      this.Finish(tick);
    }
  }
}
