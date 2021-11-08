// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.ContinueReduceBloodEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class ContinueReduceBloodEffect : AbstractEffect
  {
    private int m_blood;
    private int m_count;
    private Living m_liv;

    public ContinueReduceBloodEffect(int count, int blood)
      : base(eEffectType.ContinueReduceBloodEffect)
    {
      this.m_count = count;
      this.m_blood = blood;
    }

    public ContinueReduceBloodEffect(int count, int blood, Living liv)
      : base(eEffectType.ContinueReduceBloodEffect)
    {
      this.m_count = count;
      this.m_blood = blood;
      this.m_liv = liv;
    }

    public override void OnAttached(Living living)
    {
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
      living.Game.SendPlayerPicture(living, 2, true);
    }

    public override void OnRemoved(Living living)
    {
      living.BeginSelfTurn -= new LivingEventHandle(this.player_BeginFitting);
      living.Game.SendPlayerPicture(living, 2, false);
    }

    private void player_BeginFitting(Living living)
    {
      --this.m_count;
      if (this.m_count < 0)
      {
        this.Stop();
      }
      else
      {
        living.AddBlood(-this.m_blood, 1);
        if (living.Blood > 0)
          return;
        living.Die();
        if (this.m_liv == null || !(this.m_liv is Player))
          return;
        (this.m_liv as Player).PlayerDetail.OnKillingLiving((AbstractGame) this.m_liv.Game, 2, living.Id, living.IsLiving, this.m_blood);
      }
    }

    public override bool Start(Living living)
    {
      ContinueReduceBloodEffect ofType = living.EffectList.GetOfType(eEffectType.ContinueReduceBloodEffect) as ContinueReduceBloodEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
