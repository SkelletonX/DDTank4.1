// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.FatalEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Actions;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class FatalEffect : BasePlayerEffect
  {
    private int m_count;
    private int m_probability;
    private int m_saycount;

    public FatalEffect(int count, int probability)
      : base(eEffectType.FatalEffect)
    {
      this.m_count = count;
      this.m_probability = probability;
    }

    private void ChangeProperty(Player player)
    {
      if (player.CurrentBall.IsSpecial())
        return;
      ++this.m_saycount;
      this.IsTrigger = false;
      if (this.rand.Next(100) >= this.m_probability)
        return;
      player.ShootMovieDelay = 50;
      this.IsTrigger = true;
      if (player.CurrentBall.ID != 3)
        player.ControlBall = true;
      if (this.m_saycount != 1)
        return;
      player.EffectTrigger = true;
      player.Game.AddAction((IAction) new LivingSayAction((Living) player, LanguageMgr.GetTranslation("FatalEffect.msg"), 9, 0, 1000));
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerShoot += new PlayerEventHandle(this.ChangeProperty);
      player.TakePlayerDamage += new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
      player.BeginNextTurn += new LivingEventHandle(this.player_BeginNextTurn);
      player.AfterPlayerShooted += new PlayerEventHandle(this.player_AfterPlayerShooted);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerShoot -= new PlayerEventHandle(this.ChangeProperty);
      player.TakePlayerDamage -= new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
      player.BeginNextTurn -= new LivingEventHandle(this.player_BeginNextTurn);
      player.AfterPlayerShooted -= new PlayerEventHandle(this.player_AfterPlayerShooted);
    }

    private void player_AfterPlayerShooted(Player player)
    {
      this.IsTrigger = false;
      player.ControlBall = false;
      player.EffectTrigger = false;
    }

    private void player_BeforeTakeDamage(
      Living living,
      Living source,
      ref int damageAmount,
      ref int criticalAmount)
    {
      if (!this.IsTrigger || !(living is Player))
        return;
      damageAmount = damageAmount * (100 - this.m_count) / 100;
    }

    private void player_BeginNextTurn(Living living)
    {
      this.m_saycount = 0;
    }

    public override bool Start(Living living)
    {
      FatalEffect ofType = living.EffectList.GetOfType(eEffectType.FatalEffect) as FatalEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
