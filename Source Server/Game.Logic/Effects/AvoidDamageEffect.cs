// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.AvoidDamageEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Actions;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class AvoidDamageEffect : BasePlayerEffect
  {
    private int m_count;
    private int m_probability;

    public AvoidDamageEffect(int count, int probability)
      : base(eEffectType.AvoidDamageEffect)
    {
      this.m_count = count;
      this.m_probability = probability;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.BeforeTakeDamage += new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.BeforeTakeDamage -= new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
    }

    private void player_BeforeTakeDamage(
      Living living,
      Living source,
      ref int damageAmount,
      ref int criticalAmount)
    {
      this.IsTrigger = false;
      if (this.rand.Next(100) >= this.m_probability)
        return;
      this.IsTrigger = true;
      living.EffectTrigger = true;
      damageAmount = damageAmount * (100 - this.m_count) / 100;
      living.Game.AddAction((IAction) new LivingSayAction(living, LanguageMgr.GetTranslation("AvoidDamageEffect.msg"), 9, 0, 1000));
    }

    public override bool Start(Living living)
    {
      AvoidDamageEffect ofType = living.EffectList.GetOfType(eEffectType.AvoidDamageEffect) as AvoidDamageEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
