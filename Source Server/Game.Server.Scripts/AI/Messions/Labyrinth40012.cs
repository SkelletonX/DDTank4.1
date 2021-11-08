using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
  public class Labyrinth40012 : AMissionControl
  {
    private SimpleBoss simpleBoss_0;
    private SimpleBoss simpleBoss_1;
    private int int_0;
    private int int_1;
    private int int_2;

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
      this.Game.AddLoadingFile(2, "image/game/effect/4/feather.swf", "asset.game.4.feather");
      this.Game.LoadResources(npcIds1);
      this.Game.LoadNpcGameOverResources(npcIds2);
      this.Game.SetMap(1236);
    }

    public override void OnStartGame()
    {
      base.OnStartGame();
      LivingConfig config = this.Game.BaseLivingConfig();
      config.IsFly = true;
      this.simpleBoss_0 = this.Game.CreateBoss(this.int_2, 1200, 900, -1, 1, "");
      this.simpleBoss_0.FallFromTo(this.simpleBoss_0.X, this.simpleBoss_0.Y, (string) null, 0, 0, 2000, (LivingCallBack) null);
      this.simpleBoss_0.SetRelateDemagemRect(this.simpleBoss_0.NpcInfo.X, this.simpleBoss_0.NpcInfo.Y, this.simpleBoss_0.NpcInfo.Width, this.simpleBoss_0.NpcInfo.Height);
      this.simpleBoss_1 = this.Game.CreateBoss(this.int_1, 1389, 620, -1, 0, "", config);
      this.simpleBoss_1.SetRelateDemagemRect(this.simpleBoss_1.NpcInfo.X, this.simpleBoss_1.NpcInfo.Y, this.simpleBoss_1.NpcInfo.Width, this.simpleBoss_1.NpcInfo.Height);
    }

    public override void OnNewTurnStarted()
    {
    }

    public override void OnBeginNewTurn()
    {
      base.OnBeginNewTurn();
    }

    public override bool CanGameOver()
    {
      if (this.Game.TurnIndex > this.Game.MissionInfo.TotalTurn - 1)
        return true;
      if (this.simpleBoss_0 != null && !this.simpleBoss_0.IsLiving && (this.simpleBoss_1 != null && !this.simpleBoss_1.IsLiving))
      {
        if (this.Game.CanEnterGate)
          return true;
        ++this.int_0;
        this.Game.CanShowBigBox = true;
      }
      return false;
    }

    public override int UpdateUIData()
    {
      base.UpdateUIData();
      return this.int_0;
    }

    public override void OnGameOverMovie()
    {
      base.OnGameOverMovie();
      if (this.simpleBoss_0 != null && !this.simpleBoss_0.IsLiving && (this.simpleBoss_1 != null && !this.simpleBoss_1.IsLiving))
        this.Game.IsWin = true;
      else
        this.Game.IsWin = false;
    }

    public override void OnGameOver()
    {
      base.OnGameOver();
    }

    public Labyrinth40012()
    {
      this.int_1 = 40015;
      this.int_2 = 40016;
    }
  }
}
