// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.ReduceStrengthEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class ReduceStrengthEffect : AbstractEffect
  {
    private int m_count;
    private int m_reduce;

    public ReduceStrengthEffect(int count, int reduce = 0)
      : base(eEffectType.ReduceStrengthEffect)
    {
      this.m_count = count;
      this.m_reduce = reduce;
    }

    public override void OnAttached(Living living)
    {
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
      living.Game.SendPlayerPicture(living, 1, true);
    }

    public override void OnRemoved(Living living)
    {
      living.BeginSelfTurn -= new LivingEventHandle(this.player_BeginFitting);
      living.Game.SendPlayerPicture(living, 1, false);
    }

    private void player_BeginFitting(Living living)
    {
      --this.m_count;
      if (living is Player)
        (living as Player).Energy -= this.m_reduce;
      if (this.m_count >= 0)
        return;
      this.Stop();
    }

    public override bool Start(Living living)
    {
      ReduceStrengthEffect ofType = living.EffectList.GetOfType(eEffectType.ReduceStrengthEffect) as ReduceStrengthEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
