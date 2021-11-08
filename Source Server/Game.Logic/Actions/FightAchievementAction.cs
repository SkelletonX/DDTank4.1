// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.FightAchievementAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class FightAchievementAction : BaseAction
  {
    private int m_delay;
    private Living m_living;
    private int m_num;
    private int m_type;

    public FightAchievementAction(Living living, eFightAchievementType type, int num, int delay)
      : base(delay, 1500)
    {
      this.m_living = living;
      this.m_num = num;
      this.m_type = (int) type;
      this.m_delay = delay;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      game.SendFightAchievement(this.m_living, this.m_type, this.m_num, this.m_delay);
      this.Finish(tick);
    }
  }
}
