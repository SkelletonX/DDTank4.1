// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.SealEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Actions;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class SealEquipEffect : BasePlayerEffect
  {
    private int m_count;
    private int m_probability;

    public SealEquipEffect(int count, int probability)
      : base(eEffectType.SealEquipEffect)
    {
      this.m_count = count;
      this.m_probability = probability;
    }

    private void ChangeProperty(Player player)
    {
      if (player.CurrentBall.IsSpecial())
        return;
      this.IsTrigger = false;
      if (this.rand.Next(100) >= this.m_probability)
        return;
      this.IsTrigger = true;
      player.EffectTrigger = true;
      player.Game.AddAction((IAction) new LivingSayAction((Living) player, LanguageMgr.GetTranslation("SealEquipEffect.msg"), 9, 0, 1000));
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerShoot += new PlayerEventHandle(this.ChangeProperty);
      player.AfterKillingLiving += new KillLivingEventHanlde(this.player_AfterKillingLiving);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerShoot -= new PlayerEventHandle(this.ChangeProperty);
      player.AfterKillingLiving -= new KillLivingEventHanlde(this.player_AfterKillingLiving);
    }

    private void player_AfterKillingLiving(
      Living living,
      Living target,
      int damageAmount,
      int criticalAmount)
    {
      if (!this.IsTrigger)
        return;
      target.AddEffect((AbstractEffect) new SealEffect(2), 0);
    }

    public override bool Start(Living living)
    {
      SealEquipEffect ofType = living.EffectList.GetOfType(eEffectType.SealEquipEffect) as SealEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
