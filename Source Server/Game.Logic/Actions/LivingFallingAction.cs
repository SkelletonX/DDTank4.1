// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingFallingAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class LivingFallingAction : BaseAction
  {
    private string m_action;
    private LivingCallBack m_callback;
    private int m_fallSpeed;
    private bool m_isSent;
    private Living m_living;
    private int m_toX;
    private int m_toY;
    private int m_type;

    public LivingFallingAction(
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
      this.m_fallSpeed = speed;
      this.m_action = action;
      this.m_toX = toX;
      this.m_toY = toY;
      this.m_isSent = false;
      this.m_type = type;
      this.m_callback = callback;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      if (!this.m_isSent)
      {
        this.m_isSent = true;
        game.SendLivingFall(this.m_living, this.m_toX, this.m_toY, this.m_fallSpeed, this.m_action, this.m_type);
      }
      if (this.m_toY > this.m_living.Y + this.m_fallSpeed)
      {
        this.m_living.SetXY(this.m_toX, this.m_living.Y + this.m_fallSpeed);
      }
      else
      {
        this.m_living.SetXY(this.m_toX, this.m_toY);
        if (game.Map.IsOutMap(this.m_toX, this.m_toY))
        {
          this.m_living.SyncAtTime = false;
          this.m_living.Die();
        }
        if (this.m_callback != null)
          this.m_living.CallFuction(this.m_callback, 0);
        this.Finish(tick);
      }
    }
  }
}
