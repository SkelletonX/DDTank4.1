// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.PlayerSpeedMultAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;
using System.Drawing;

namespace Game.Logic.Actions
{
  public class PlayerSpeedMultAction : BaseAction
  {
    private Point point_0;
    private Player player_0;
    private bool bool_0;

    public PlayerSpeedMultAction(Player player, Point target, int delay)
      : base(0, delay)
    {
      this.player_0 = player;
      this.point_0 = target;
      this.bool_0 = false;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      int type = 4;
      if (!this.bool_0)
      {
        this.bool_0 = true;
        game.SendPlayerMove(this.player_0, type, this.point_0.X, this.point_0.Y, this.point_0.X > this.player_0.X ? (byte) 1 : byte.MaxValue, this.player_0.IsLiving, false);
      }
      if (this.player_0.Distance(this.point_0) > (double) this.player_0.StepX && this.point_0.X != this.player_0.X)
      {
        this.player_0.Direction = this.point_0.X > this.player_0.X ? 1 : -1;
        Point point = this.player_0.getNextWalkPoint(this.player_0.Direction);
        if (point == Point.Empty)
        {
          int num = this.player_0.X + this.player_0.Direction * this.player_0.MOVE_SPEED;
          point = this.player_0.FindYLineNotEmptyPointDown(num, this.player_0.Y - this.player_0.StepY);
          if (point == Point.Empty)
          {
            type = 1;
            point = new Point(num, this.point_0.Y);
          }
        }
        this.player_0.SetXY(point.X, point.Y);
        if ((this.player_0.Direction <= 0 || point.X < this.point_0.X) && (this.player_0.Direction >= 0 || point.X > this.point_0.X) && type != 1)
          return;
        this.player_0.StartMoving();
        this.Finish(tick);
      }
      else
        this.Finish(tick);
    }
  }
}
