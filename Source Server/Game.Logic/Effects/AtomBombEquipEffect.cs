// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.AtomBombEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Actions;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class AtomBombEquipEffect : BasePlayerEffect
  {
    private int m_count;
    private int m_probability;

    public AtomBombEquipEffect(int count, int probability)
      : base(eEffectType.AtomBomb)
    {
      this.m_count = count;
      this.m_probability = probability;
    }

    private void ChangeProperty(Player player)
    {
      if (player.CurrentBall.IsSpecial() || this.rand.Next(100) >= this.m_probability)
        return;
      player.SetBall(4);
      player.Game.AddAction((IAction) new LivingSayAction((Living) player, LanguageMgr.GetTranslation("AtomBombEquipEffect.msg"), 9, 0, 1000));
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
      AtomBombEquipEffect ofType = living.EffectList.GetOfType(eEffectType.AtomBomb) as AtomBombEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
