// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.CheckPVEGameStateAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

namespace Game.Logic.Actions
{
  public class CheckPVEGameStateAction : IAction
  {
    private long m_time;
    private bool m_isFinished;

    public CheckPVEGameStateAction(int delay)
    {
      this.m_time = TickHelper.GetTickCount() + (long) delay;
      this.m_isFinished = false;
    }

    public void Execute(BaseGame game, long tick)
    {
      if (this.m_time > tick || game.GetWaitTimer() >= tick)
        return;
      PVEGame pveGame = game as PVEGame;
      if (pveGame != null)
      {
        switch (pveGame.GameState)
        {
          case eGameState.Inited:
            pveGame.Prepare();
            break;
          case eGameState.Prepared:
            pveGame.PrepareNewSession();
            break;
          case eGameState.Loading:
            if (pveGame.IsAllComplete())
            {
              pveGame.StartGame();
              break;
            }
            game.SendLoading();
            game.WaitTime(1000);
            break;
          case eGameState.GameStart:
            if (game.RoomType == eRoomType.FightLab)
            {
              if (game.CurrentActionCount <= 1)
              {
                pveGame.PrepareFightingLivings();
                break;
              }
              break;
            }
            pveGame.PrepareNewGame();
            break;
          case eGameState.Playing:
            if ((pveGame.CurrentLiving == null || !pveGame.CurrentLiving.IsAttacking) && game.CurrentActionCount <= 1)
            {
              if (pveGame.CanGameOver())
              {
                if (pveGame.IsLabyrinth() && pveGame.CanEnterGate)
                {
                  pveGame.GameOverMovie();
                  break;
                }
                if (pveGame.CurrentActionCount <= 1)
                {
                  pveGame.GameOver();
                  break;
                }
                break;
              }
              pveGame.NextTurn();
              break;
            }
            break;
          case eGameState.PrepareGameOver:
            if (pveGame.CurrentActionCount <= 1)
            {
              pveGame.GameOver();
              break;
            }
            break;
          case eGameState.GameOver:
            if (!pveGame.HasNextSession())
            {
              pveGame.GameOverAllSession();
              break;
            }
            pveGame.PrepareNewSession();
            break;
          case eGameState.SessionPrepared:
            if (pveGame.CanStartNewSession())
            {
              pveGame.SetupStyle();
              pveGame.StartLoading();
              break;
            }
            game.WaitTime(1000);
            break;
          case eGameState.ALLSessionStopped:
            if (pveGame.PlayerCount != 0 && (uint) pveGame.WantTryAgain > 0U)
            {
              if (pveGame.WantTryAgain == 1)
              {
                pveGame.ShowDragonLairCard();
                pveGame.PrepareNewSession();
                break;
              }
              if (pveGame.WantTryAgain == 2)
              {
                --pveGame.SessionId;
                pveGame.PrepareNewSession();
                break;
              }
              game.WaitTime(1000);
              break;
            }
            pveGame.Stop();
            break;
        }
      }
      this.m_isFinished = true;
    }

    public bool IsFinished(long tick)
    {
      return this.m_isFinished;
    }
  }
}
