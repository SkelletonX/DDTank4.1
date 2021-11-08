// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.InvinciblyEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class InvinciblyEffect : AbstractEffect
  {
    private int m_count;

    public InvinciblyEffect(int count)
      : base(eEffectType.InvinciblyEffect)
    {
      this.m_count = count;
    }

    public override void OnAttached(Living living)
    {
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
    }

    public override void OnRemoved(Living living)
    {
      living.BeginSelfTurn -= new LivingEventHandle(this.player_BeginFitting);
    }

    private void player_BeginFitting(Living player)
    {
      --this.m_count;
      if (this.m_count > 0)
        return;
      this.Stop();
    }

    public override bool Start(Living living)
    {
      InvinciblyEffect ofType = living.EffectList.GetOfType(eEffectType.InvinciblyEffect) as InvinciblyEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
