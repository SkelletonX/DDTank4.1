// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingBeatAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class LivingBeatAction : BaseAction
  {
    private string m_action;
    private int m_attackEffect;
    private int m_criticalAmount;
    private int m_demageAmount;
    private Living m_living;
    private int m_livingCount;
    private Living m_target;

    public LivingBeatAction(
      Living living,
      Living target,
      int demageAmount,
      int criticalAmount,
      string action,
      int delay,
      int livingCount,
      int attackEffect)
      : base(delay)
    {
      this.m_living = living;
      this.m_target = target;
      this.m_demageAmount = demageAmount;
      this.m_criticalAmount = criticalAmount;
      this.m_action = action;
      this.m_livingCount = livingCount;
      this.m_attackEffect = attackEffect;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      this.m_target.SyncAtTime = false;
      try
      {
        if (this.m_target.TakeDamage(this.m_living, ref this.m_demageAmount, ref this.m_criticalAmount, "LivingFire", 0))
        {
          int totalDemageAmount = this.m_demageAmount + this.m_criticalAmount;
          game.SendLivingBeat(this.m_living, this.m_target, totalDemageAmount, this.m_action, this.m_livingCount, this.m_attackEffect);
        }
        this.m_target.IsFrost = false;
        this.Finish(tick);
      }
      finally
      {
        this.m_target.SyncAtTime = true;
      }
    }
  }
}
