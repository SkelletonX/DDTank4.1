// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.WaitLivingAttackingAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class WaitLivingAttackingAction : BaseAction
  {
    private TurnedLiving m_living;
    private int m_turnIndex;

    public WaitLivingAttackingAction(TurnedLiving living, int turnIndex, int delay)
      : base(delay)
    {
      this.m_living = living;
      this.m_turnIndex = turnIndex;
      living.EndAttacking += new LivingEventHandle(this.player_EndAttacking);
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      this.Finish(tick);
      if (game.TurnIndex != this.m_turnIndex || !this.m_living.IsAttacking)
        return;
      this.m_living.StopAttacking();
      game.CheckState(0);
    }

    private void player_EndAttacking(Living player)
    {
      player.EndAttacking -= new LivingEventHandle(this.player_EndAttacking);
      this.Finish(TickHelper.GetTickCount());
    }
  }
}
