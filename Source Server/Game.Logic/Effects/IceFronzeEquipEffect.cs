// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.IceFronzeEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Bussiness.Managers;
using Game.Logic.Actions;
using Game.Logic.Phy.Object;
using Game.Logic.Spells;

namespace Game.Logic.Effects
{
  public class IceFronzeEquipEffect : BasePlayerEffect
  {
    private int m_count;
    private int m_probability;

    public IceFronzeEquipEffect(int count, int probability)
      : base(eEffectType.IceFronzeEquipEffect)
    {
      this.m_count = count;
      this.m_probability = probability;
    }

    private void ChangeProperty(Player player)
    {
      if (player.CurrentBall.IsSpecial() || this.rand.Next(100) >= this.m_probability)
        return;
      SpellMgr.ExecuteSpell(player.Game, player, ItemMgr.FindItemTemplate(10015));
      player.Game.AddAction((IAction) new LivingSayAction((Living) player, LanguageMgr.GetTranslation("IceFronzeEquipEffect.msg"), 9, 0, 1000));
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerShoot += new PlayerEventHandle(this.ChangeProperty);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerShoot -= new PlayerEventHandle(this.ChangeProperty);
    }

    public override bool Start(Living living)
    {
      IceFronzeEquipEffect ofType = living.EffectList.GetOfType(eEffectType.IceFronzeEquipEffect) as IceFronzeEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
