// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingChangeDirectionAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class LivingChangeDirectionAction : BaseAction
  {
    private Living m_Living;
    private int m_direction;

    public LivingChangeDirectionAction(Living living, int direction, int delay)
      : base(delay)
    {
      this.m_Living = living;
      this.m_direction = direction;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      this.m_Living.Direction = this.m_direction;
      this.Finish(tick);
    }
  }
}
