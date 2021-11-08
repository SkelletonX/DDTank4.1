using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
  public class TVS12001 : AMissionControl
  {
    private List<SimpleNpc> SomeNpc = new List<SimpleNpc>();
    private SimpleNpc npc;
    private bool result = false;
    private int preKillNum = 0;
    public int turnCount;

    public override int CalculateScoreGrade(int score)
    {
      base.CalculateScoreGrade(score);
      if (score > 900)
        return 3;
      if (score > 825)
        return 2;
      return score > 725 ? 1 : 0;
    }

    public override void OnPrepareNewSession()
    {
      base.OnPrepareNewSession();
      int[] npcIds = { 12001, 12002, 12003, 12004 };
      Game.LoadResources(npcIds);
      Game.LoadNpcGameOverResources(npcIds);
      Game.SetMap(1207);
    }

    public override void OnStartGame()
    {
      base.OnStartGame();
      if (Game.GetLivedLivings().Count == 0)
        Game.PveGameDelay = 0;
      for (int index = 0; index < 4; ++index)
      {
        if (index < 1)
          SomeNpc.Add(Game.CreateNpc(12001, 1360, 700, -1, 1));
        else if (index < 3)
          SomeNpc.Add(Game.CreateNpc(12001, 1410, 700, -1, 1));
        else
          SomeNpc.Add(Game.CreateNpc(12002, 1250, 700, -1, 1));
      }
      npc = Game.CreateNpc(12004, 700, 700, -1, 0);
      npc.FallFrom(npc.X, npc.Y, "", 0, 0, 1200, null);
      npc.SetRelateDemagemRect(-42, -200, 84, 194);
      turnCount = 0;
    }

    public override void OnNewTurnStarted()
    {
      base.OnNewTurnStarted();
      if (Game.GetLivedLivings().Count == 0)
        Game.PveGameDelay = 0;
      if (Game.TurnIndex > 1 && Game.CurrentPlayer.Delay > Game.PveGameDelay)
      {
        for (int index = 0; index < 4; ++index)
        {
          if (turnCount < 12)
          {
            ++turnCount;
            if (index < 1)
              SomeNpc.Add(Game.CreateNpc(12001, 1260, 700, -1, 1));
            else if (index < 3)
              SomeNpc.Add(Game.CreateNpc(12001, 1350, 700, -1, 1));
            else
              SomeNpc.Add(Game.CreateNpc(12002, 1400, 700, -1, 1));
          }
        }
      }
      if (Game.TurnIndex != 2)
        return;
      //m_boss = Game.CreateSimple(12303, 100, 700, 1, 2);
      //m_boss.FallFrom(100, 700, "", 0, 0, 1000, (LivingCallBack) null);
      //m_boss.SetRelateDemagemRect(-41, -187, 83, 140);
    }

    public override void OnBeginNewTurn()
    {
      base.OnBeginNewTurn();
    }

    public override bool CanGameOver()
    {
      base.CanGameOver();
      if (Game.TurnIndex > 199)
        return true;
      result = false;
      foreach (Physics physics in SomeNpc)
      {
        if (physics.IsLiving)
          result = true;
      }
      return !result && SomeNpc.Count == 16 || !npc.IsLiving;
    }

    public override int UpdateUIData()
    {
      preKillNum = Game.TotalKillCount;
      return Game.TotalKillCount;
    }

    public override void OnGameOver()
    {
      if (!result && SomeNpc.Count == 16)
      {
        foreach (Player player in Game.GetAllFightPlayers())
          player.CanGetProp = true;
        Game.IsWin = true;
      }
      if (npc.IsLiving)
        return;
      Game.IsWin = false;
    }
  }
}
