// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingWalkToAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Logic.Actions
{
  public class LivingWalkToAction : BaseAction
  {
    private Living m_living;
    private List<Point> m_path;
    private string m_action;
    private bool m_isSent;
    private int m_index;
    private int m_speed;
    private LivingCallBack m_callback;

    public LivingWalkToAction(
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

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      if (!this.m_isSent)
      {
        this.m_isSent = true;
        game.SendLivingWalkTo(this.m_living, this.m_living.X, this.m_living.Y, this.m_path[this.m_path.Count - 1].X, this.m_path[this.m_path.Count - 1].Y, this.m_action, this.m_speed);
      }
      ++this.m_index;
      if (this.m_index < this.m_path.Count)
        return;
      this.m_living.Direction = this.m_path[this.m_index - 1].X <= this.m_living.X ? -1 : 1;
      Living living = this.m_living;
      Point point = this.m_path[this.m_index - 1];
      int x = point.X;
      point = this.m_path[this.m_index - 1];
      int y = point.Y;
      living.SetXY(x, y);
      if (this.m_callback != null)
        this.m_living.CallFuction(this.m_callback, 0);
      this.Finish(tick);
    }
  }
}
