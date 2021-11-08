// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingSayAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class LivingSayAction : BaseAction
  {
    private Living m_living;
    private string m_msg;
    private int m_type;

    public LivingSayAction(Living living, string msg, int type, int delay, int finishTime)
      : base(delay, finishTime)
    {
      this.m_living = living;
      this.m_msg = msg;
      this.m_type = type;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      game.SendLivingSay(this.m_living, this.m_msg, this.m_type);
      this.Finish(tick);
    }
  }
}
