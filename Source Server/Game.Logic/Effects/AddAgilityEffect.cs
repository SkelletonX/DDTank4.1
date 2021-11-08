// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.AddAgilityEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class AddAgilityEffect : BasePlayerEffect
  {
    private int m_added;
    private int m_count;
    private int m_probablity;

    public AddAgilityEffect(int count, int probability)
      : base(eEffectType.AddAgilityEffect)
    {
      this.m_count = count;
      this.m_probablity = probability;
    }

    private void ChangeProperty(Living living)
    {
      living.Agility -= (double) this.m_added;
      this.m_added = 0;
      this.IsTrigger = false;
      if (this.rand.Next(100) >= this.m_probablity)
        return;
      living.EffectTrigger = true;
      this.IsTrigger = true;
      living.Agility += (double) this.m_count;
      this.m_added = this.m_count;
      living.Game.SendEquipEffect(living, LanguageMgr.GetTranslation("AddAgilityEffect.Success", (object) this.m_count));
    }

    private void DefaultProperty(Living living)
    {
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.BeginAttacking += new LivingEventHandle(this.ChangeProperty);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.BeginAttacking -= new LivingEventHandle(this.ChangeProperty);
    }

    public override bool Start(Living living)
    {
      AddAgilityEffect ofType = living.EffectList.GetOfType(eEffectType.AddAgilityEffect) as AddAgilityEffect;
      if (ofType == null)
        return base.Start(living);
      this.m_probablity = this.m_probablity > ofType.m_probablity ? this.m_probablity : ofType.m_probablity;
      return true;
    }
  }
}
