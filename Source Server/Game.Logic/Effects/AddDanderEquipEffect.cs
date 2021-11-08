// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.AddDanderEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Actions;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class AddDanderEquipEffect : BasePlayerEffect
  {
    private int m_count;
    private int m_probability;

    public AddDanderEquipEffect(int count, int probability)
      : base(eEffectType.AddDander)
    {
      this.m_count = count;
      this.m_probability = probability;
    }

    private void ChangeProperty(Living living)
    {
      this.IsTrigger = false;
      if (this.rand.Next(100) >= this.m_probability)
        return;
      this.IsTrigger = true;
      if (living is Player)
        (living as Player).AddDander(this.m_count);
      living.EffectTrigger = true;
      living.Game.AddAction((IAction) new LivingSayAction(living, LanguageMgr.GetTranslation("AddDanderEquipEffect.msg"), 9, 0, 1000));
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
      AddDanderEquipEffect ofType = living.EffectList.GetOfType(eEffectType.AddDander) as AddDanderEquipEffect;
      if (ofType == null)
        return base.Start(living);
      this.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
