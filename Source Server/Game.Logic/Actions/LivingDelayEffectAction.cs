// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingDelayEffectAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Effects;
using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class LivingDelayEffectAction : BaseAction
  {
    private AbstractEffect m_effect;
    private Living m_living;

    public LivingDelayEffectAction(Living living, AbstractEffect effect, int delay)
      : base(delay)
    {
      this.m_effect = effect;
      this.m_living = living;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      this.m_effect.Start(this.m_living);
      this.Finish(tick);
    }
  }
}
