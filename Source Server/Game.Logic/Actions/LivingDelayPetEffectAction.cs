// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingDelayPetEffectAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.PetEffects;
using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class LivingDelayPetEffectAction : BaseAction
  {
    private AbstractPetEffect abstractPetEffect_0;
    private Living living_0;

    public LivingDelayPetEffectAction(Living living, AbstractPetEffect effect, int delay)
      : base(delay)
    {
      this.abstractPetEffect_0 = effect;
      this.living_0 = living;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      this.abstractPetEffect_0.Start(this.living_0);
      this.Finish(tick);
    }
  }
}
