// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.ShowBloodItem
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

namespace Game.Logic.Actions
{
  public class ShowBloodItem : BaseAction
  {
    private int m_livingId;

    public ShowBloodItem(int livingId, int delay, int finishTime)
      : base(delay, finishTime)
    {
      this.m_livingId = livingId;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      game.SendShowBloodItem(this.m_livingId);
      this.Finish(tick);
    }
  }
}
