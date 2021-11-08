// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.RecoverBloodEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Actions;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class RecoverBloodEffect : BasePlayerEffect
  {
    private int m_count;
    private int m_probability;

    public RecoverBloodEffect(int count, int probability)
      : base(eEffectType.RecoverBloodEffect)
    {
      this.m_count = count;
      this.m_probability = probability;
    }

    public void ChangeProperty(Living living, Living target, int damageAmount, int criticalAmount)
    {
      if (!living.IsLiving)
        return;
      this.IsTrigger = false;
      if (this.rand.Next(100) >= this.m_probability)
        return;
      this.IsTrigger = true;
      living.EffectTrigger = true;
      living.SyncAtTime = true;
      living.AddBlood(this.m_count);
      living.SyncAtTime = false;
      living.Game.AddAction((IAction) new LivingSayAction(living, LanguageMgr.GetTranslation("RecoverBloodEffect.msg"), 9, 0, 1000));
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.AfterKilledByLiving += new KillLivingEventHanlde(this.ChangeProperty);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.AfterKilledByLiving -= new KillLivingEventHanlde(this.ChangeProperty);
    }

    public override bool Start(Living living)
    {
      RecoverBloodEffect ofType = living.EffectList.GetOfType(eEffectType.RecoverBloodEffect) as RecoverBloodEffect;
      if (ofType == null)
        return base.Start(living);
      this.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
