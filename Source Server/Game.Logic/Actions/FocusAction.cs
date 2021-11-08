// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.FocusAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

namespace Game.Logic.Actions
{
  public class FocusAction : BaseAction
  {
    private int m_x;
    private int m_y;
    private int m_type;

    public FocusAction(int x, int y, int type, int delay, int finishTime)
      : base(delay, finishTime)
    {
      this.m_x = x;
      this.m_y = y;
      this.m_type = type;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      game.SendPhysicalObjFocus(this.m_x, this.m_y, this.m_type);
      this.Finish(tick);
    }
  }
}
