using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
  public class Labyrinth40020 : AMissionControl
  {
    private SimpleBoss simpleBoss_0;
    private int int_0;
    private SimpleBoss simpleBoss_1;
    private SimpleBoss simpleBoss_2;
    private PhysicalObj physicalObj_0;
    private PhysicalObj physicalObj_1;
    private PhysicalObj physicalObj_2;
    private PhysicalObj physicalObj_3;
    private PhysicalObj physicalObj_4;
    private PhysicalObj[] physicalObj_5;
    private PhysicalObj[] physicalObj_6;
    private PhysicalObj physicalObj_7;
    private List<SimpleNpc> list_0;
    private PhysicalObj physicalObj_8;
    private PhysicalObj physicalObj_9;
    private PhysicalObj physicalObj_10;
    private PhysicalObj physicalObj_11;
    private int int_1;
    private int int_2;
    private int int_3;
    private int int_4;
    private int int_5;
    private int int_6;
    private int int_7;
    private int int_8;

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
      this.Game.AddLoadingFile(1, "bombs/56.swf", "tank.resource.bombs.Bomb56");
      this.Game.AddLoadingFile(2, "image/game/effect/5/mubiao.swf", "asset.game.4.mubiao");
      this.Game.AddLoadingFile(2, "image/game/effect/5/xiaopao.swf", "asset.game.4.xiaopao");
      this.Game.AddLoadingFile(2, "image/game/effect/5/zao.swf", "asset.game.4.zao");
      this.Game.AddLoadingFile(2, "image/game/living/living144.swf", "game.living.Living144");
      this.Game.AddLoadingFile(2, "image/game/living/living152.swf", "game.living.Living152");
      this.Game.AddLoadingFile(2, "image/game/living/living154.swf", "game.living.Living154");
      this.Game.AddLoadingFile(2, "image/game/living/living147.swf", "game.living.Living147");
      this.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
      this.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.gebulinzhihuiguanAsset");
      int[] npcIds = new int[6]{ this.int_3, this.int_4, this.int_5, this.int_6, this.int_7, this.int_8 };
      this.Game.LoadResources(npcIds);
      this.Game.LoadNpcGameOverResources(npcIds);
      this.Game.SetMap(1269);
    }

    public override void OnStartGame()
    {
      base.OnStartGame();
      this.physicalObj_0 = (PhysicalObj) this.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 0);
      this.physicalObj_4 = (PhysicalObj) this.Game.Createlayer(1100, 395, "font", "game.asset.living.gebulinzhihuiguanAsset", "out", 1, 0);
      this.physicalObj_7 = this.Game.CreatePhysicalObj(1460, 580, "wallLeft", "asset.game.4.zao", "1", 1, 0);
      this.physicalObj_7.SetRect(-75, -159, 100, 130);
      this.simpleBoss_0 = this.Game.CreateBoss(this.int_3, 1480, 610, -1, 1, "");
      this.simpleBoss_2 = this.Game.CreateBoss(this.int_4, 1617, 544, -1, 1, "");
      this.simpleBoss_1 = this.Game.CreateBoss(this.int_5, 1300, 650, -1, 1, "");
      this.simpleBoss_1.FallFrom(1300, 650, "", 0, 0, 1000);
      this.physicalObj_8 = (PhysicalObj) this.Game.Createlayer(1550, 650, "NPC", "game.living.Living154", "stand", 1, 0);
      this.physicalObj_9 = (PhysicalObj) this.Game.Createlayer(1367, 845, "NPC", "game.living.Living147", "stand", 1, 0);
      this.simpleBoss_2.SetRelateDemagemRect(-34, -35, 50, 40);
      this.simpleBoss_1.SetRelateDemagemRect(-34, -35, 50, 40);
      this.simpleBoss_0.SetRelateDemagemRect(-34, -35, 50, 40);
      this.physicalObj_0.PlayMovie("in", 3000, 0);
      this.physicalObj_4.PlayMovie("in", 3000, 0);
      this.physicalObj_0.PlayMovie("out", 4000, 0);
      this.physicalObj_4.PlayMovie("out", 4000, 0);
      this.physicalObj_1 = (PhysicalObj) this.Game.Createlayer(1617, 530, "moive", "asset.game.4.mubiao", "out", 1, 0);
      this.physicalObj_2 = (PhysicalObj) this.Game.Createlayer(1300, 635, "moive", "asset.game.4.mubiao", "out", 1, 0);
      this.physicalObj_3 = (PhysicalObj) this.Game.Createlayer(1480, 595, "moive", "asset.game.4.mubiao", "out", 1, 0);
    }

    public override void OnNewTurnStarted()
    {
      base.OnBeginNewTurn();
      if (this.Game.TurnIndex <= 1)
        return;
      if (this.physicalObj_0 != null)
      {
        this.Game.RemovePhysicalObj(this.physicalObj_0, true);
        this.physicalObj_0 = (PhysicalObj) null;
      }
      if (this.physicalObj_1 != null)
      {
        this.Game.RemovePhysicalObj(this.physicalObj_1, true);
        this.physicalObj_1 = (PhysicalObj) null;
      }
      if (this.physicalObj_2 != null)
      {
        this.Game.RemovePhysicalObj(this.physicalObj_2, true);
        this.physicalObj_2 = (PhysicalObj) null;
      }
      if (this.physicalObj_3 != null)
      {
        this.Game.RemovePhysicalObj(this.physicalObj_3, true);
        this.physicalObj_3 = (PhysicalObj) null;
      }
      if (this.physicalObj_4 != null)
      {
        this.Game.RemovePhysicalObj(this.physicalObj_4, true);
        this.physicalObj_4 = (PhysicalObj) null;
      }
      if (this.physicalObj_8 != null)
      {
        this.Game.RemovePhysicalObj(this.physicalObj_8, true);
        this.physicalObj_8 = (PhysicalObj) null;
      }
      if (this.physicalObj_9 == null)
        return;
      this.Game.RemovePhysicalObj(this.physicalObj_9, true);
      this.physicalObj_9 = (PhysicalObj) null;
    }

    public override void OnBeginNewTurn()
    {
      base.OnBeginNewTurn();
      if (this.simpleBoss_1 == null || this.simpleBoss_1.IsLiving || (this.simpleBoss_2 == null || this.simpleBoss_2.IsLiving))
        return;
      this.physicalObj_5 = this.Game.FindPhysicalObjByName("wallLeft", false);
      this.physicalObj_6 = this.Game.FindPhysicalObjByName("wallRight", false);
      this.physicalObj_7.SetRect(0, 0, 0, 0);
      foreach (PhysicalObj phy in this.physicalObj_5)
      {
        if (phy != null)
          this.Game.RemovePhysicalObj(phy, true);
      }
      foreach (PhysicalObj phy in this.physicalObj_6)
      {
        if (phy != null)
          this.Game.RemovePhysicalObj(phy, true);
      }
    }

    public override bool CanGameOver()
    {
      if (this.Game.TurnIndex > this.Game.MissionInfo.TotalTurn - 1)
        return true;
      if (this.simpleBoss_0 != null && !this.simpleBoss_0.IsLiving)
      {
        if (this.Game.CanEnterGate)
          return true;
        this.Game.CanShowBigBox = true;
      }
      return false;
    }

    public override int UpdateUIData()
    {
      base.UpdateUIData();
      return 0;
    }

    public override void OnGameOverMovie()
    {
      base.OnGameOverMovie();
      if (this.simpleBoss_0 != null && !this.simpleBoss_0.IsLiving)
        this.Game.IsWin = true;
      else
        this.Game.IsWin = false;
      this.physicalObj_5 = this.Game.FindPhysicalObjByName("wallLeft");
      this.physicalObj_6 = this.Game.FindPhysicalObjByName("wallRight");
      for (int index = 0; index < this.physicalObj_5.Length; ++index)
        this.Game.RemovePhysicalObj(this.physicalObj_5[index], true);
      for (int index = 0; index < this.physicalObj_6.Length; ++index)
        this.Game.RemovePhysicalObj(this.physicalObj_6[index], true);
    }

    private void method_0()
    {
      if (!this.simpleBoss_2.IsLiving && this.int_1 == 0)
      {
        this.physicalObj_10 = (PhysicalObj) this.Game.Createlayer(this.simpleBoss_2.X, this.simpleBoss_2.Y, "", "game.living.Living144", "standB", 1, 0);
        this.int_1 = 1;
      }
      if (this.simpleBoss_1.IsLiving || this.int_2 != 0)
        return;
      this.physicalObj_11 = (PhysicalObj) this.Game.Createlayer(this.simpleBoss_1.X, this.simpleBoss_1.Y, "", "game.living.Living152", "standB", 1, 0);
      this.int_2 = 1;
    }

    public override void OnShooted()
    {
      if (!this.simpleBoss_2.IsLiving)
        this.simpleBoss_2.CallFuction(new LivingCallBack(this.method_0), 8000);
      if (this.simpleBoss_1.IsLiving)
        return;
      this.simpleBoss_1.CallFuction(new LivingCallBack(this.method_0), 8000);
    }

    public Labyrinth40020()
    {
      this.list_0 = new List<SimpleNpc>();
      this.int_3 = 40028;
      this.int_4 = 5313;
      this.int_5 = 5312;
      this.int_6 = 40026;
      this.int_7 = 40027;
      this.int_8 = 5311;
    }
  }
}
