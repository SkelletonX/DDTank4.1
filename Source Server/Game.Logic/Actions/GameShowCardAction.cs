// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.GameShowCardAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class GameShowCardAction : BaseAction
  {
    private PVEGame m_game;

    public GameShowCardAction(PVEGame game, int delay, int finishTime)
      : base(delay, finishTime)
    {
      this.m_game = game;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      foreach (Player allFightPlayer in this.m_game.GetAllFightPlayers())
      {
        if (allFightPlayer.IsActive && allFightPlayer.CanTakeOut > 0)
        {
          allFightPlayer.HasPaymentTakeCard = true;
          int canTakeOut = allFightPlayer.CanTakeOut;
          for (int index = 0; index < canTakeOut; ++index)
            this.m_game.TakeCard(allFightPlayer);
        }
      }
      this.m_game.SendShowCards();
      base.ExecuteImp(game, tick);
    }
  }
}
