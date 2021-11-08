// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingCreateChildAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class LivingCreateChildAction : BaseAction
  {
    private Living m_living;
    private int m_type;

    public LivingCreateChildAction(Living living, int type, int delay)
      : base(delay)
    {
      this.m_living = living;
      this.m_type = type;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      this.Finish(tick);
    }
  }
}
