// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.NoHoleEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Actions;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class NoHoleEquipEffect : BasePlayerEffect
  {
    private int m_count;
    private int m_probability;

    public NoHoleEquipEffect(int count, int probability)
      : base(eEffectType.NoHoleEquipEffect)
    {
      this.m_count = count;
      this.m_probability = probability;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.CollidByObject += new PlayerEventHandle(this.player_AfterKilledByLiving);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.CollidByObject -= new PlayerEventHandle(this.player_AfterKilledByLiving);
    }

    private void player_AfterKilledByLiving(Living living)
    {
      if (this.rand.Next(100) >= this.m_probability)
        return;
      living.EffectTrigger = true;
      new NoHoleEffect(1).Start(living);
      living.Game.AddAction((IAction) new LivingSayAction(living, LanguageMgr.GetTranslation("NoHoleEquipEffect.msg"), 9, 0, 1000));
    }

    public override bool Start(Living living)
    {
      NoHoleEquipEffect ofType = living.EffectList.GetOfType(eEffectType.NoHoleEquipEffect) as NoHoleEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
