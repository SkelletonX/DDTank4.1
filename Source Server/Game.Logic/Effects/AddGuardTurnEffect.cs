// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.AddGuardTurnEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class AddGuardTurnEffect : BasePlayerEffect
  {
    private int m_count = 0;
    private int m_probability = 0;

    public AddGuardTurnEffect(int count, int probability)
      : base(eEffectType.AddGuardTurnEffect)
    {
      this.m_count = count;
      this.m_probability = probability;
    }

    public override bool Start(Living living)
    {
      AddGuardTurnEffect ofType = living.EffectList.GetOfType(eEffectType.AddGuardTurnEffect) as AddGuardTurnEffect;
      if (ofType == null)
        return base.Start(living);
      this.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.BeforeTakeDamage += new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
      player.BeginSelfTurn += new LivingEventHandle(this.player_SelfTurn);
      player.Game.SendPlayerPicture((Living) player, 30, true);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.BeforeTakeDamage -= new LivingTakedDamageEventHandle(this.player_BeforeTakeDamage);
      player.BeginSelfTurn -= new LivingEventHandle(this.player_SelfTurn);
      player.Game.SendPlayerPicture((Living) player, 30, false);
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
      damageAmount -= this.m_count;
      if (damageAmount > 0)
        return;
      damageAmount = 1;
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
