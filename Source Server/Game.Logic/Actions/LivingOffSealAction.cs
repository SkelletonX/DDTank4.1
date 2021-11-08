// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingOffSealAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class LivingOffSealAction : BaseAction
  {
    private Living m_Living;
    private Living m_Target;

    public LivingOffSealAction(Living Living, Living target, int delay)
      : base(delay, 1000)
    {
      this.m_Living = Living;
      this.m_Target = target;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      this.m_Target.EffectList.GetOfType(eEffectType.SealEffect)?.Stop();
      this.Finish(tick);
    }
  }
}
