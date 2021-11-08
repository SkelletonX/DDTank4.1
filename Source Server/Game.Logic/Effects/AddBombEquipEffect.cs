// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.AddBombEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Actions;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class AddBombEquipEffect : BasePlayerEffect
  {
    private int m_count;
    private int m_probability;
    private bool m_show;

    public AddBombEquipEffect(int count, int probability)
      : base(eEffectType.AddBombEquipEffect)
    {
      this.m_count = count;
      this.m_probability = probability;
    }

    private void ChangeProperty(Living living)
    {
      if ((living as Player).CurrentBall.IsSpecial())
        return;
      this.IsTrigger = false;
      if (this.rand.Next(100) >= this.m_probability || !(living is Player))
        return;
      this.m_show = true;
      this.IsTrigger = true;
      (living as Player).ShootCount += this.m_count;
      living.EffectTrigger = true;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerShoot += new PlayerEventHandle(this.playerShot);
      player.BeginAttacking += new LivingEventHandle(this.ChangeProperty);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerShoot -= new PlayerEventHandle(this.playerShot);
      player.BeginAttacking -= new LivingEventHandle(this.ChangeProperty);
    }

    private void playerShot(Player player)
    {
      if (player.CurrentBall.IsSpecial())
      {
        player.ShootCount = 1;
        this.m_show = false;
      }
      if (!this.IsTrigger || !this.m_show)
        return;
      player.Game.AddAction((IAction) new LivingSayAction((Living) player, LanguageMgr.GetTranslation("AddBombEquipEffect.msg"), 9, 0, 1000));
      this.m_show = false;
    }

    public override bool Start(Living living)
    {
      AddBombEquipEffect ofType = living.EffectList.GetOfType(eEffectType.AddBombEquipEffect) as AddBombEquipEffect;
      if (ofType == null)
        return base.Start(living);
      this.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
