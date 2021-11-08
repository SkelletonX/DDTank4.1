// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.CheckPVPGameStateAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using log4net;
using System.Reflection;

namespace Game.Logic.Actions
{
  public class CheckPVPGameStateAction : IAction
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private bool m_isFinished;
    private long m_tick;

    public CheckPVPGameStateAction(int delay)
    {
      this.m_tick += TickHelper.GetTickCount() + (long) delay;
    }

    public void Execute(BaseGame game, long tick)
    {
      if (this.m_tick > tick)
        return;
      PVPGame pvpGame = game as PVPGame;
      if (pvpGame != null)
      {
        switch (game.GameState)
        {
          case eGameState.Inited:
            pvpGame.Prepare();
            break;
          case eGameState.Prepared:
            pvpGame.StartLoading();
            break;
          case eGameState.Loading:
            if (pvpGame.IsAllComplete())
              pvpGame.StartGame();
            game.SendLoading();
            break;
          case eGameState.Playing:
            if (pvpGame.CurrentPlayer == null || !pvpGame.CurrentPlayer.IsAttacking)
            {
              if (pvpGame.CanGameOver())
              {
                pvpGame.GameOver();
                break;
              }
              pvpGame.NextTurn();
              break;
            }
            break;
          case eGameState.GameOver:
            pvpGame.Stop();
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
