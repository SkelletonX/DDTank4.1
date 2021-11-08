// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingMoveToAction2
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Logic.Actions
{
  public class LivingMoveToAction2 : BaseAction
  {
    private bool bool_0;
    private int int_0;
    private int int_1;
    private int int_2;
    private List<Point> list_0;
    private Living living_0;
    private LivingCallBack livingCallBack_0;
    private string string_0;
    private string string_1;

    public LivingMoveToAction2(
      Living living,
      List<Point> path,
      string action,
      string saction,
      int speed,
      int delay,
      LivingCallBack callback,
      int delayCallback)
      : base(delay, 0)
    {
      this.living_0 = living;
      this.list_0 = path;
      this.string_0 = action;
      this.string_1 = saction;
      this.bool_0 = false;
      this.int_0 = 0;
      this.livingCallBack_0 = callback;
      this.int_1 = speed;
      this.int_2 = delayCallback;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      Point point1;
      if (!this.bool_0)
      {
        this.bool_0 = true;
        Point point2 = this.list_0[this.list_0.Count - 1];
        point1 = this.list_0[this.list_0.Count - 1];
        game.method_18(this.living_0, this.living_0.X, this.living_0.Y, point1.X, point1.Y, this.string_0, this.string_1, this.int_1);
      }
      ++this.int_0;
      if (this.int_0 < this.list_0.Count)
        return;
      point1 = this.list_0[this.int_0 - 1];
      this.living_0.Direction = point1.X <= this.living_0.X ? -1 : 1;
      point1 = this.list_0[this.int_0 - 1];
      point1 = this.list_0[this.int_0 - 1];
      this.living_0.SetXY(point1.X, point1.Y);
      if (this.livingCallBack_0 != null)
        this.living_0.CallFuction(this.livingCallBack_0, 0);
      this.Finish(tick);
    }
  }
}
