// Decompiled with JetBrains decompiler
// Type: Game.Logic.PetEffects.PetFatalEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
  public class PetFatalEffect : BasePetEffect
  {
    private int m_count;
    private int m_currentId;
    private int m_delay;
    private int m_probability;
    private int m_type;

    public PetFatalEffect(
      int count,
      int probability,
      int type,
      int skillId,
      int delay,
      string elementID)
      : base(ePetEffectType.FatalEffect, elementID)
    {
      this.m_count = count;
      this.m_probability = probability == -1 ? 10000 : probability;
      this.m_type = type;
      this.m_delay = delay;
      this.m_currentId = skillId;
      if ((uint) (skillId - 136) > 1U)
        return;
      this.m_count = 1;
    }

    public void ChangeProperty(Living living)
    {
      this.IsTrigger = false;
      if (this.rand.Next(10000) >= this.m_probability || living.PetEffects.CurrentUseSkill != this.m_currentId)
        return;
      this.IsTrigger = true;
      living.PetEffectTrigger = true;
      living.PetEffects.PetDelay = this.m_delay;
      if (!living.PetEffects.IsPetUseSkill)
        return;
      living.PetEffects.IsPetUseSkill = false;
      (living as Player).ControlBall = true;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerShoot += new PlayerEventHandle(this.ChangeProperty);
      player.AfterPlayerShooted += new PlayerEventHandle(this.player_AfterPlayerShooted);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerShoot -= new PlayerEventHandle(this.ChangeProperty);
      player.AfterPlayerShooted -= new PlayerEventHandle(this.player_AfterPlayerShooted);
    }

    private void player_AfterPlayerShooted(Player player)
    {
      this.IsTrigger = false;
      player.ControlBall = false;
      player.EffectTrigger = false;
    }

    public override bool Start(Living living)
    {
      PetFatalEffect ofType = living.PetEffectList.GetOfType(ePetEffectType.FatalEffect) as PetFatalEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
