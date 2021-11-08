// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingPlayeMovieAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class LivingPlayeMovieAction : BaseAction
  {
    private Living m_living;
    private string m_action;
    private LivingCallBack m_callBack;
    private int m_movieTime;

    public LivingPlayeMovieAction(
      Living living,
      string action,
      int delay,
      int movieTime,
      LivingCallBack callBack)
      : base(delay, movieTime)
    {
      this.m_living = living;
      this.m_action = action;
      this.m_callBack = callBack;
      this.m_movieTime = movieTime;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      game.SendLivingPlayMovie(this.m_living, this.m_action);
      if (this.m_callBack != null)
        this.m_living.CallFuction(this.m_callBack, this.m_movieTime);
      this.Finish(tick);
    }
  }
}
