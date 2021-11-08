// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.AddDamageTurnEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class AddDamageTurnEffect : BasePlayerEffect
  {
    private int m_count = 0;
    private int m_probability = 0;

    public AddDamageTurnEffect(int count, int probability)
      : base(eEffectType.AddDamageTurnEffect)
    {
      this.m_count = count;
      this.m_probability = probability;
    }

    public override bool Start(Living living)
    {
      AddDamageTurnEffect ofType = living.EffectList.GetOfType(eEffectType.AddDamageTurnEffect) as AddDamageTurnEffect;
      if (ofType == null)
        return base.Start(living);
      this.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.TakePlayerDamage += new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
      player.AfterPlayerShooted += new PlayerEventHandle(this.player_AfterPlayerShooted);
      player.BeginSelfTurn += new LivingEventHandle(this.player_SelfTurn);
      player.Game.SendPlayerPicture((Living) player, 29, true);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.TakePlayerDamage -= new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
      player.AfterPlayerShooted -= new PlayerEventHandle(this.player_AfterPlayerShooted);
      player.BeginSelfTurn -= new LivingEventHandle(this.player_SelfTurn);
      player.Game.SendPlayerPicture((Living) player, 29, false);
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

    private void player_SelfTurn(Living living)
    {
      --this.m_probability;
      if (this.m_probability >= 0)
        return;
      this.Stop();
    }
  }
}
