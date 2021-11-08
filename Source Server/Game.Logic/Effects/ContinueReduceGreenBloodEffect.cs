// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.ContinueReduceGreenBloodEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class ContinueReduceGreenBloodEffect : AbstractEffect
  {
    private int int_0;
    private int int_1;
    private Living yktowyBbie;

    public ContinueReduceGreenBloodEffect(int count, int blood, Living liv)
      : base(eEffectType.ContinueReduceGreenBloodEffect)
    {
      this.int_0 = count;
      this.int_1 = blood;
      this.yktowyBbie = liv;
    }

    private void method_0(Living living_0)
    {
      --this.int_0;
      if (this.int_0 < 0)
      {
        this.Stop();
      }
      else
      {
        living_0.AddBlood(-this.int_1, 1);
        if (living_0.Blood > 0)
          return;
        living_0.Die();
        if (this.yktowyBbie == null || !(this.yktowyBbie is Player))
          return;
        int type = 2;
        if (living_0 is Player)
          type = 1;
        (this.yktowyBbie as Player).PlayerDetail.OnKillingLiving((AbstractGame) this.yktowyBbie.Game, type, living_0.Id, living_0.IsLiving, this.int_1);
      }
    }

    public override void OnAttached(Living living)
    {
      living.BeginSelfTurn += new LivingEventHandle(this.method_0);
      living.Game.method_47(living, 28, true);
    }

    public override void OnRemoved(Living living)
    {
      living.BeginSelfTurn -= new LivingEventHandle(this.method_0);
      living.Game.method_47(living, 28, false);
    }

    public override bool Start(Living living)
    {
      ContinueReduceGreenBloodEffect ofType = living.EffectList.GetOfType(eEffectType.ContinueReduceGreenBloodEffect) as ContinueReduceGreenBloodEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.int_0 = this.int_0;
      return true;
    }
  }
}
