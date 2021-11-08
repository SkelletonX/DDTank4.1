// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.AddDamageEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Actions;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class AddDamageEffect : BasePlayerEffect
  {
    private int m_count;
    private int m_probability;

    public AddDamageEffect(int count, int probability)
      : base(eEffectType.AddDamageEffect)
    {
      this.m_count = count;
      this.m_probability = probability;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.TakePlayerDamage += new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
      player.PlayerShoot += new PlayerEventHandle(this.playerShot);
      player.AfterPlayerShooted += new PlayerEventHandle(this.player_AfterPlayerShooted);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.TakePlayerDamage -= new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
      player.PlayerShoot -= new PlayerEventHandle(this.playerShot);
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
      if (!this.IsTrigger)
        return;
      damageAmount += this.m_count;
    }

    private void playerShot(Player player)
    {
      if (player.CurrentBall.IsSpecial())
        return;
      this.IsTrigger = false;
      if (this.rand.Next(100) >= this.m_probability)
        return;
      this.IsTrigger = true;
      player.FlyingPartical = 65;
      player.EffectTrigger = true;
      player.Game.AddAction((IAction) new LivingSayAction((Living) player, LanguageMgr.GetTranslation("AddDamageEffect.msg"), 9, 0, 1000));
    }

    public override bool Start(Living living)
    {
      AddDamageEffect ofType = living.EffectList.GetOfType(eEffectType.AddDamageEffect) as AddDamageEffect;
      if (ofType == null)
        return base.Start(living);
      this.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
