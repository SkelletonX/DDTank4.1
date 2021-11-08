// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.AddDefenceEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Actions;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class AddDefenceEffect : BasePlayerEffect
  {
    private int m_added;
    private int m_count;
    private int m_probability;

    public AddDefenceEffect(int count, int probability)
      : base(eEffectType.AddDefenceEffect)
    {
      this.m_count = count;
      this.m_probability = probability;
      this.m_added = 0;
    }

    public void ChangeProperty(Living living)
    {
      living.Defence -= (double) this.m_added;
      this.m_added = 0;
      this.IsTrigger = false;
      if (this.rand.Next(100) >= this.m_probability)
        return;
      this.IsTrigger = true;
      living.Defence += (double) this.m_count;
      this.m_added = this.m_count;
      living.EffectTrigger = true;
      living.Game.AddAction((IAction) new LivingSayAction(living, LanguageMgr.GetTranslation("AddDefenceEffect.msg"), 9, 1000, 1000));
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.BeginAttacked += new LivingEventHandle(this.ChangeProperty);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.BeginAttacked -= new LivingEventHandle(this.ChangeProperty);
    }

    public override bool Start(Living living)
    {
      AddDefenceEffect ofType = living.EffectList.GetOfType(eEffectType.AddDefenceEffect) as AddDefenceEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
