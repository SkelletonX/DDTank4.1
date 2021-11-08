// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingMoveToAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Logic.Actions
{
  public class LivingMoveToAction : BaseAction
  {
    private string m_action;
    private LivingCallBack m_callback;
    private int m_delayCallback;
    private int m_index;
    private bool m_isSent;
    private Living m_living;
    private List<Point> m_path;
    private string m_saction;
    private int m_speed;

    public LivingMoveToAction(
      Living living,
      List<Point> path,
      string action,
      int delay,
      int speed,
      LivingCallBack callback)
      : base(delay, 0)
    {
      this.m_living = living;
      this.m_path = path;
      this.m_action = action;
      this.m_isSent = false;
      this.m_index = 0;
      this.m_callback = callback;
      this.m_speed = speed;
    }

    public LivingMoveToAction(
      Living living,
      List<Point> path,
      string action,
      int delay,
      int speed,
      string sAction,
      LivingCallBack callback,
      int delayCallback)
      : base(delay, 0)
    {
      this.m_living = living;
      this.m_path = path;
      this.m_action = action;
      this.m_saction = sAction;
      this.m_isSent = false;
      this.m_index = 0;
      this.m_callback = callback;
      this.m_speed = speed;
      this.m_delayCallback = delayCallback;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      Point point1;
      if (!this.m_isSent)
      {
        this.m_isSent = true;
        Point point2 = this.m_path[this.m_path.Count - 1];
        point1 = this.m_path[this.m_path.Count - 1];
        game.SendLivingMoveTo(this.m_living, this.m_living.X, this.m_living.Y, point1.X, point1.Y, this.m_action, this.m_speed, this.m_saction);
      }
      ++this.m_index;
      if (this.m_index < this.m_path.Count)
        return;
      point1 = this.m_path[this.m_index - 1];
      this.m_living.Direction = point1.X <= this.m_living.X ? -1 : 1;
      point1 = this.m_path[this.m_index - 1];
      point1 = this.m_path[this.m_index - 1];
      this.m_living.SetXY(point1.X, point1.Y);
      if (this.m_callback != null)
        this.m_living.CallFuction(this.m_callback, 0);
      this.Finish(tick);
    }
  }
}
