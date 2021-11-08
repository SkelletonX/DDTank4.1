// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.ReflexDamageEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class ReflexDamageEquipEffect : BasePlayerEffect
  {
    private int m_count;
    private int m_probability;

    public ReflexDamageEquipEffect(int count, int probability)
      : base(eEffectType.ReflexDamageEquipEffect)
    {
      this.m_count = count;
      this.m_probability = probability;
    }

    public void ChangeProperty(Living living)
    {
      this.IsTrigger = false;
      if (this.rand.Next(100) >= this.m_probability)
        return;
      this.IsTrigger = true;
      living.EffectTrigger = true;
      living.Game.SendEquipEffect(living, LanguageMgr.GetTranslation("ReflexDamageEquipEffect.Success", (object) this.m_count));
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.BeginAttacked += new LivingEventHandle(this.ChangeProperty);
      player.AfterKilledByLiving += new KillLivingEventHanlde(this.player_AfterKilledByLiving);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.BeginAttacked -= new LivingEventHandle(this.ChangeProperty);
    }

    private void player_AfterKilledByLiving(
      Living living,
      Living target,
      int damageAmount,
      int criticalAmount)
    {
      if (!this.IsTrigger)
        return;
      target.AddBlood(-this.m_count);
    }

    public override bool Start(Living living)
    {
      ReflexDamageEquipEffect ofType = living.EffectList.GetOfType(eEffectType.ReflexDamageEquipEffect) as ReflexDamageEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
