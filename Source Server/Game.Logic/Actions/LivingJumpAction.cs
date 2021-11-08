// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingJumpAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class LivingJumpAction : BaseAction
  {
    private string m_action;
    private LivingCallBack m_callback;
    private bool m_isSent;
    private Living m_living;
    private int m_speed;
    private int m_toX;
    private int m_toY;
    private int m_type;

    public LivingJumpAction(
      Living living,
      int toX,
      int toY,
      int speed,
      string action,
      int delay,
      int type,
      LivingCallBack callback)
      : base(delay, 2000)
    {
      this.m_living = living;
      this.m_toX = toX;
      this.m_toY = toY;
      this.m_speed = speed;
      this.m_action = action;
      this.m_isSent = false;
      this.m_type = type;
      this.m_callback = callback;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      if (!this.m_isSent)
      {
        this.m_isSent = true;
        game.SendLivingJump(this.m_living, this.m_toX, this.m_toY, this.m_speed, this.m_action, this.m_type);
      }
      if (this.m_toY < this.m_living.Y - this.m_speed)
      {
        this.m_living.SetXY(this.m_toX, this.m_living.Y - this.m_speed);
      }
      else
      {
        this.m_living.SetXY(this.m_toX, this.m_toY);
        if (this.m_callback != null)
          this.m_living.CallFuction(this.m_callback, 0);
        this.Finish(tick);
      }
    }
  }
}
