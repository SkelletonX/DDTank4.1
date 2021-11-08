using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
  public class Labyrinth40009 : AMissionControl
  {
    private List<SimpleNpc> list_0;
    private int int_0;
    private int int_1;
    private int int_2;
    private int[] int_3;

    public override int CalculateScoreGrade(int score)
    {
      base.CalculateScoreGrade(score);
      if (score > 1870)
        return 3;
      if (score > 1825)
        return 2;
      return score > 1780 ? 1 : 0;
    }

    public override void OnPrepareNewSession()
    {
      base.OnPrepareNewSession();
      int[] npcIds1 = new int[2]{ this.int_1, this.int_2 };
      int[] npcIds2 = new int[2]{ this.int_1, this.int_2 };
      this.Game.AddLoadingFile(1, "bombs/58.swf", "tank.resource.bombs.Bomb58");
      this.Game.LoadResources(npcIds1);
      this.Game.LoadNpcGameOverResources(npcIds2);
      this.Game.SetMap(1222);
    }

    public override void OnStartGame()
    {
      base.OnStartGame();
      for (int index = 0; index < this.int_3.Length; ++index)
      {
        if (index < 6)
        {
          this.list_0.Add(this.Game.CreateNpc(this.int_1, this.int_3[index], 430, 1, -1));
        }
        else
        {
          this.list_0.Add(this.Game.CreateNpc(this.int_2, this.int_3[index], 430, 1, -1));
          this.list_0.Add(this.Game.CreateNpc(this.int_2, this.int_3[index] + 10, 430, 1, -1));
        }
      }
    }

    public override void OnNewTurnStarted()
    {
      base.OnNewTurnStarted();
    }

    public override void OnBeginNewTurn()
    {
      base.OnBeginNewTurn();
    }

    public override bool CanGameOver()
    {
      bool flag = true;
      base.CanGameOver();
      if (this.Game.TurnIndex > this.Game.MissionInfo.TotalTurn - 1)
        return true;
      this.int_0 = 0;
      foreach (Physics physics in this.list_0)
      {
        if (physics.IsLiving)
          flag = false;
        else
          ++this.int_0;
      }
      if (flag && this.int_0 == this.Game.MissionInfo.TotalCount)
        this.Game.CreateGate(true);
      return flag;
    }

    public override int UpdateUIData()
    {
      return this.Game.TotalKillCount;
    }

    public override void OnGameOverMovie()
    {
      base.OnGameOverMovie();
      if (this.Game.GetLivedLivings().Count == 0)
        this.Game.IsWin = true;
      else
        this.Game.IsWin = false;
    }

    public override void OnGameOver()
    {
      base.OnGameOver();
    }

    public Labyrinth40009()
    {
      this.list_0 = new List<SimpleNpc>();
      this.int_1 = 40010;
      this.int_2 = 40011;
      this.int_3 = new int[8]
      {
        1110,
        1090,
        1060,
        1040,
        1020,
        1000,
        1050,
        1310
      };
    }
  }
}
