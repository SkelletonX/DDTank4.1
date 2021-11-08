// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.MakeCriticalEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Actions;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class MakeCriticalEffect : BasePlayerEffect
  {
    private int m_count;
    private int m_probability;

    public MakeCriticalEffect(int count, int probability)
      : base(eEffectType.MakeCriticalEffect)
    {
      this.m_count = count;
      this.m_probability = probability;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.TakePlayerDamage += new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
      player.AfterPlayerShooted += new PlayerEventHandle(this.player_AfterPlayerShooted);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.TakePlayerDamage -= new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
      player.AfterPlayerShooted -= new PlayerEventHandle(this.player_AfterPlayerShooted);
    }

    private void player_AfterPlayerShooted(Player player)
    {
      player.FlyingPartical = 0;
    }

    private void player_BeforeTakeDamage(
      Living living,
      Living source,
      ref int damageAmount,
      ref int criticalAmount)
    {
      if ((living as Player).CurrentBall.IsSpecial())
        return;
      this.IsTrigger = false;
      if (this.rand.Next(100) >= this.m_probability)
        return;
      this.IsTrigger = true;
      living.EffectTrigger = true;
      criticalAmount = (int) (0.5 + living.Lucky * 0.0005 * (double) damageAmount);
      living.FlyingPartical = 65;
      living.Game.AddAction((IAction) new LivingSayAction(living, LanguageMgr.GetTranslation("MakeCriticalEffect.msg"), 9, 0, 1000));
    }

    public override bool Start(Living living)
    {
      MakeCriticalEffect ofType = living.EffectList.GetOfType(eEffectType.MakeCriticalEffect) as MakeCriticalEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
