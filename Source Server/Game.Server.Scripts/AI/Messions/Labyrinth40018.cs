using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
  public class Labyrinth40018 : AMissionControl
  {
    private SimpleBoss simpleBoss_0;
    private SimpleBoss simpleBoss_1;
    private List<SimpleNpc> list_0;
    private int int_0;
    private int int_1;
    private int int_2;
    private int int_3;
    private PhysicalObj physicalObj_0;
    private PhysicalObj physicalObj_1;
    private PhysicalObj physicalObj_2;

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
      int[] npcIds1 = new int[2]{ this.int_1, this.int_0 };
      int[] npcIds2 = new int[1]{ this.int_1 };
      this.Game.LoadResources(npcIds1);
      this.Game.LoadNpcGameOverResources(npcIds2);
      this.Game.AddLoadingFile(2, "image/game/effect/5/minigun.swf", "asset.game.4.minigun");
      this.Game.AddLoadingFile(2, "image/game/effect/5/jinqud.swf", "asset.game.4.jinqud");
      this.Game.AddLoadingFile(2, "image/game/effect/5/zap.swf", "asset.game.4.zap");
      this.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
      this.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.gebulinzhihuiguanAsset");
      this.Game.AddLoadingFile(1, "bombs/56.swf", "tank.resource.bombs.Bomb56");
      this.Game.AddLoadingFile(1, "bombs/72.swf", "tank.resource.bombs.Bomb72");
      this.Game.SetMap(1274);
    }

    public override void OnStartGame()
    {
      base.OnStartGame();
      this.physicalObj_1 = (PhysicalObj) this.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 1);
      this.physicalObj_2 = (PhysicalObj) this.Game.Createlayer(1131, 650, "font", "game.asset.living.gebulinzhihuiguanAsset", "out", 1, 1);
      this.physicalObj_0 = (PhysicalObj) this.Game.Createlayer(1567, 810, "moive", "asset.game.4.jinqud", "out", 1, 0);
      this.simpleBoss_1 = this.Game.CreateBoss(this.int_0, 190, 365, 1, 1, "");
      this.simpleBoss_0 = this.Game.CreateBoss(this.int_1, 1477, 768, -1, 0, "");
      this.simpleBoss_0.SetRelateDemagemRect(-110, -179, 220, 180);
      this.simpleBoss_1.SetRelateDemagemRect(-42, -200, 84, 104);
      this.simpleBoss_0.PlayMovie("in", 0, 2000);
      this.simpleBoss_1.PlayMovie("in", 4000, 2000);
      this.simpleBoss_1.PlayMovie("in", 0, 2000);
      this.physicalObj_1.PlayMovie("in", 6000, 0);
      this.physicalObj_2.PlayMovie("in", 6100, 0);
      this.physicalObj_1.PlayMovie("out", 10000, 1000);
      this.physicalObj_2.PlayMovie("out", 9900, 0);
    }

    public override void OnNewTurnStarted()
    {
    }

    public override void OnBeginNewTurn()
    {
      base.OnBeginNewTurn();
      if (this.Game.TurnIndex <= 1)
        return;
      if (this.physicalObj_1 != null)
      {
        this.Game.RemovePhysicalObj(this.physicalObj_1, true);
        this.physicalObj_1 = (PhysicalObj) null;
      }
      if (this.physicalObj_2 == null)
        return;
      this.Game.RemovePhysicalObj(this.physicalObj_2, true);
      this.physicalObj_2 = (PhysicalObj) null;
    }

    public override bool CanGameOver()
    {
      if (this.Game.TurnIndex > this.Game.MissionInfo.TotalTurn - 1)
        return true;
      if (this.simpleBoss_0 != null && !this.simpleBoss_0.IsLiving)
      {
        if (this.Game.CanEnterGate)
          return true;
        ++this.int_3;
        this.Game.CanShowBigBox = true;
      }
      return false;
    }

    public override int UpdateUIData()
    {
      base.UpdateUIData();
      return this.int_3;
    }

    public override void OnGameOverMovie()
    {
      base.OnGameOverMovie();
      if (this.simpleBoss_0 != null && !this.simpleBoss_0.IsLiving)
        this.Game.IsWin = true;
      else
        this.Game.IsWin = false;
    }

    public override void OnShooted()
    {
      if (!this.simpleBoss_0.IsLiving)
        return;
      if (this.simpleBoss_0.Y == 659)
      {
        this.simpleBoss_0.SetXY(1477, 659);
        this.simpleBoss_0.SetXY(1477, 759);
        this.simpleBoss_0.CallFuction(new LivingCallBack(this.method_0), 100);
      }
      else if (this.simpleBoss_0.Y == 559)
      {
        this.simpleBoss_0.SetXY(1477, 559);
        this.simpleBoss_0.SetXY(1477, 759);
        this.simpleBoss_0.CallFuction(new LivingCallBack(this.method_1), 100);
      }
      else if (this.simpleBoss_0.Y == 459)
      {
        this.simpleBoss_0.SetXY(1477, 459);
        this.simpleBoss_0.SetXY(1477, 759);
        this.simpleBoss_0.CallFuction(new LivingCallBack(this.method_2), 100);
      }
      else if (this.simpleBoss_0.Y == 359)
      {
        this.simpleBoss_0.SetXY(1477, 359);
        this.simpleBoss_0.SetXY(1477, 759);
        this.simpleBoss_0.CallFuction(new LivingCallBack(this.method_3), 100);
      }
      else
      {
        if (this.simpleBoss_0.Y != 259)
          return;
        this.simpleBoss_0.SetXY(1477, 259);
        this.simpleBoss_0.SetXY(1477, 759);
        this.simpleBoss_0.CallFuction(new LivingCallBack(this.method_4), 100);
      }
    }

    private void method_0()
    {
      this.int_2 += 155;
      this.simpleBoss_0.PlayMovie("cryA", 1000 + this.int_2, 0);
      this.simpleBoss_0.SetXY(1477, 659);
    }

    private void method_1()
    {
      this.int_2 += 155;
      this.simpleBoss_0.PlayMovie("cryB", 1000 + this.int_2, 0);
      this.simpleBoss_0.SetXY(1477, 559);
    }

    private void method_2()
    {
      this.int_2 += 155;
      this.simpleBoss_0.PlayMovie("cryC", 1000 + this.int_2, 0);
      this.simpleBoss_0.SetXY(1477, 459);
    }

    private void method_3()
    {
      this.int_2 += 155;
      this.simpleBoss_0.PlayMovie("cryD", 1000 + this.int_2, 0);
      this.simpleBoss_0.SetXY(1477, 359);
    }

    private void method_4()
    {
      this.int_2 += 155;
      this.simpleBoss_0.PlayMovie("cryE", 1000 + this.int_2, 0);
      this.simpleBoss_0.SetXY(1477, 259);
    }

    public Labyrinth40018()
    {
      this.list_0 = new List<SimpleNpc>();
      this.int_0 = 5302;
      this.int_1 = 40024;
    }
  }
}
