// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.FocusFreeAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

namespace Game.Logic.Actions
{
  public class FocusFreeAction : BaseAction
  {
    private int int_0;
    private int int_1;
    private int int_2;

    public FocusFreeAction(int x, int y, int type, int delay, int finishTime)
      : base(delay, finishTime)
    {
      this.int_0 = x;
      this.int_1 = y;
      this.int_2 = type;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      game.method_10(this.int_0, this.int_1, this.int_2);
      this.Finish(tick);
    }
  }
}
