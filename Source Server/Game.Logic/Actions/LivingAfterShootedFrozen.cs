// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingAfterShootedFrozen
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class LivingAfterShootedFrozen : BaseAction
  {
    private Living living_0;

    public LivingAfterShootedFrozen(Living living, int delay)
      : base(delay, 0)
    {
      this.living_0 = living;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      this.living_0.OnAfterTakedFrozen();
      this.Finish(tick);
    }
  }
}
