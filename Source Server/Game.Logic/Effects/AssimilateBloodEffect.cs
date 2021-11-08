// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.AssimilateBloodEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Actions;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class AssimilateBloodEffect : BasePlayerEffect
  {
    private int m_count;
    private int m_probability;

    public AssimilateBloodEffect(int count, int probability)
      : base(eEffectType.AssimilateBloodEffect)
    {
      this.m_count = count;
      this.m_probability = probability;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.TakePlayerDamage += new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
      player.PlayerShoot += new PlayerEventHandle(this.player_PlayerShoot);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.TakePlayerDamage -= new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
      player.PlayerShoot -= new PlayerEventHandle(this.player_PlayerShoot);
    }

    private void player_BeforeTakeDamage(
      Living living,
      Living source,
      ref int damageAmount,
      ref int criticalAmount)
    {
      if (!living.IsLiving || !this.IsTrigger)
        return;
      living.SyncAtTime = true;
      living.AddBlood(damageAmount * this.m_count / 100);
      living.SyncAtTime = false;
    }

    private void player_PlayerShoot(Player player)
    {
      if (player.CurrentBall.IsSpecial())
        return;
      this.IsTrigger = false;
      if (this.rand.Next(100) >= this.m_probability)
        return;
      this.IsTrigger = true;
      player.EffectTrigger = true;
      player.Game.AddAction((IAction) new LivingSayAction((Living) player, LanguageMgr.GetTranslation("AssimilateBloodEffect.msg"), 9, 0, 1000));
    }

    public override bool Start(Living living)
    {
      AssimilateBloodEffect ofType = living.EffectList.GetOfType(eEffectType.AssimilateBloodEffect) as AssimilateBloodEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
