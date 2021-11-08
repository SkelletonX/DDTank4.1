// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.WaitPlayerLoadingAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class WaitPlayerLoadingAction : IAction
  {
    private bool m_isFinished;
    private long m_time;

    public WaitPlayerLoadingAction(BaseGame game, int maxTime)
    {
      this.m_time = TickHelper.GetTickCount() + (long) maxTime;
      game.GameStarted += new GameEventHandle(this.game_GameStarted);
    }

    public void Execute(BaseGame game, long tick)
    {
      if (this.m_isFinished || tick <= this.m_time || game.GameState != eGameState.Loading)
        return;
      if (game.GameState == eGameState.Loading)
      {
        foreach (Player allFightPlayer in game.GetAllFightPlayers())
        {
          if (allFightPlayer.LoadingProcess < 100)
          {
            game.SendPlayerRemove(allFightPlayer);
            game.RemovePlayer(allFightPlayer.PlayerDetail, false);
          }
        }
        game.CheckState(0);
      }
      this.m_isFinished = true;
    }

    private void game_GameStarted(AbstractGame game)
    {
      game.GameStarted -= new GameEventHandle(this.game_GameStarted);
      this.m_isFinished = true;
    }

    public bool IsFinished(long tick)
    {
      return this.m_isFinished;
    }
  }
}
