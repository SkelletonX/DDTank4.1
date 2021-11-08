// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.GhostMoveAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Maths;
using Game.Logic.Phy.Object;
using System.Drawing;

namespace Game.Logic.Actions
{
  public class GhostMoveAction : BaseAction
  {
    private bool m_isSend;
    private Player m_player;
    private Point m_target;
    private Point m_v;

    public GhostMoveAction(Player player, Point target)
      : base(0, 1000)
    {
      this.m_player = player;
      this.m_target = target;
      this.m_v = new Point(target.X - this.m_player.X, target.Y - this.m_player.Y);
      this.m_v.Normalize(2);
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      if (!this.m_isSend)
      {
        this.m_isSend = true;
        game.SendPlayerMove(this.m_player, 2, this.m_target.X, this.m_target.Y, this.m_v.X > 0 ? (byte) 1 : byte.MaxValue, this.m_player.IsLiving, true);
      }
      if (this.m_target.Distance(this.m_player.X, this.m_player.Y) > 2.0)
      {
        this.m_player.SetXY(this.m_player.X + this.m_v.X, this.m_player.Y + this.m_v.Y);
      }
      else
      {
        this.m_player.SetXY(this.m_target.X, this.m_target.Y);
        this.Finish(tick);
      }
      this.m_player.SetXY(this.m_target.X, this.m_target.Y);
    }
  }
}
