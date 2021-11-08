// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.ContinueReduceDamageEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class ContinueReduceDamageEffect : AbstractEffect
  {
    private int m_count;

    public ContinueReduceDamageEffect(int count)
      : base(eEffectType.ContinueReduceDamageEffect)
    {
      this.m_count = count;
    }

    public override void OnAttached(Living living)
    {
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
      if (living is Player)
        (living as Player).BaseDamage = (living as Player).BaseDamage * 5.0 / 100.0;
      living.Game.SendPlayerPicture(living, 4, true);
    }

    public override void OnRemoved(Living living)
    {
      living.BeginSelfTurn -= new LivingEventHandle(this.player_BeginFitting);
      (living as Player).BaseDamage = (living as Player).BaseDamage * 100.0 / 5.0;
      living.Game.SendPlayerPicture(living, 4, false);
    }

    private void player_BeginFitting(Living living)
    {
      --this.m_count;
      if (this.m_count >= 0)
        return;
      this.Stop();
    }

    public override bool Start(Living living)
    {
      ContinueReduceDamageEffect ofType = living.EffectList.GetOfType(eEffectType.ContinueReduceDamageEffect) as ContinueReduceDamageEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
