using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
  public class Labyrinth40006 : AMissionControl
  {
    private SimpleBoss simpleBoss_0;
    private SimpleBoss simpleBoss_1;
    private PhysicalObj[] physicalObj_0;
    private PhysicalObj[] physicalObj_1;
    private int int_0;
    private int int_1;
    private int int_2;
    private int int_3;
    private int int_4;
    private int int_5;

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
      int[] npcIds1 = new int[3]{ this.int_4, this.int_5, this.int_2 };
      int[] npcIds2 = new int[1]{ this.int_4 };
      this.Game.LoadResources(npcIds1);
      this.Game.LoadNpcGameOverResources(npcIds2);
      this.Game.AddLoadingFile(2, "image/bomb/blastOut/blastOut61.swf", "bullet61");
      this.Game.AddLoadingFile(2, "image/bomb/bullet/bullet61.swf", "bullet61");
      this.Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_left");
      this.Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_right");
      this.Game.SetMap(1228);
    }

    public override void OnStartGame()
    {
      base.OnStartGame();
      this.simpleBoss_0 = this.Game.CreateBoss(this.int_4, 890, 590, -1, 1, "");
      this.simpleBoss_0.SetRelateDemagemRect(this.simpleBoss_0.NpcInfo.X, this.simpleBoss_0.NpcInfo.Y, this.simpleBoss_0.NpcInfo.Width, this.simpleBoss_0.NpcInfo.Height);
      this.simpleBoss_0.AddDelay(10);
      this.int_1 = this.Game.TurnIndex;
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
      if (!this.simpleBoss_0.IsLiving && this.int_3 == this.int_4)
        ++this.int_3;
      if (this.int_3 == this.int_5 && this.simpleBoss_1 == null)
      {
        this.simpleBoss_1 = this.Game.CreateBoss(this.int_3, this.simpleBoss_0.X, this.simpleBoss_0.Y, this.simpleBoss_0.Direction, 2, "");
        this.Game.RemoveLiving(this.simpleBoss_0.Id);
        if (this.simpleBoss_1.Direction == 1)
          this.simpleBoss_1.SetRect(this.simpleBoss_0.NpcInfo.X, this.simpleBoss_0.NpcInfo.Y, this.simpleBoss_0.NpcInfo.Width, this.simpleBoss_0.NpcInfo.Height);
        this.simpleBoss_1.SetRelateDemagemRect(this.simpleBoss_0.NpcInfo.X, this.simpleBoss_0.NpcInfo.Y, this.simpleBoss_0.NpcInfo.Width, this.simpleBoss_0.NpcInfo.Height);
        List<Player> allFightPlayers = this.Game.GetAllFightPlayers();
        Player randomPlayer = this.Game.FindRandomPlayer();
        int num = 0;
        if (randomPlayer != null)
          num = randomPlayer.Delay;
        foreach (Player player in allFightPlayers)
        {
          if (player.Delay < num)
            num = player.Delay;
        }
        this.simpleBoss_1.AddDelay(num - 2000);
        this.int_1 = this.Game.TurnIndex;
      }
      if (this.simpleBoss_1 != null && !this.simpleBoss_1.IsLiving)
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
      if (this.simpleBoss_1 != null && !this.simpleBoss_1.IsLiving)
        this.Game.IsWin = true;
      else
        this.Game.IsWin = false;
      this.physicalObj_0 = this.Game.FindPhysicalObjByName("wallLeft");
      this.physicalObj_1 = this.Game.FindPhysicalObjByName("wallRight");
      for (int index = 0; index < this.physicalObj_0.Length; ++index)
        this.Game.RemovePhysicalObj(this.physicalObj_0[index], true);
      for (int index = 0; index < this.physicalObj_1.Length; ++index)
        this.Game.RemovePhysicalObj(this.physicalObj_1[index], true);
    }

    public override void OnGameOver()
    {
      base.OnGameOver();
    }

    public Labyrinth40006()
    {
      this.int_2 = 1310;
      this.int_3 = 40006;
      this.int_4 = 40006;
      this.int_5 = 40007;
    }
  }
}
