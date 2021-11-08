// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.IceFronzeEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class IceFronzeEffect : AbstractEffect
  {
    private int m_count;

    public IceFronzeEffect(int count)
      : base(eEffectType.IceFronzeEffect)
    {
      this.m_count = count;
    }

    public override void OnAttached(Living living)
    {
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
      living.IsFrost = true;
    }

    public override void OnRemoved(Living living)
    {
      living.BeginSelfTurn -= new LivingEventHandle(this.player_BeginFitting);
      living.IsFrost = false;
    }

    private void player_BeginFitting(Living player)
    {
      --this.m_count;
      if (this.m_count >= 0)
        return;
      this.Stop();
    }

    public override bool Start(Living living)
    {
      IceFronzeEffect ofType = living.EffectList.GetOfType(eEffectType.IceFronzeEffect) as IceFronzeEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
