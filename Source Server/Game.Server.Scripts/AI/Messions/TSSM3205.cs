using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Game.Logic.Actions;
using System.Drawing;
using Bussiness;

namespace GameServerScript.AI.Messions
{
    public class TSSM3205 : AMissionControl
    {
        private SimpleBoss guardBoss = null;

        private SimpleNpc battleNpc = null;

        private PhysicalObj eyeObj = null;

        private PhysicalObj doorObj = null;

        private int bossID = 3214;

        private int battleID = 3215;

        private int bloomID = 3207;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1870)
            {
                return 3;
            }
            else if (score > 1825)
            {
                return 2;
            }
            else if (score > 1780)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] resources = { battleID, bossID, bloomID };
            int[] gameOverResources = { bossID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResources);
            Game.AddLoadingFile(2, "image/map/1125/Objects/eye.swf", "game.crazytank.assetmap.WeiredEye");
            Game.AddLoadingFile(2, "image/map/1125/Objects/gate.swf", "game.crazytank.assetmap.Gate");
            Game.SetMap(1125);
            Game.IsBossWar = "邪神守卫";
        }

        public override void OnPrepareStartGame()
        {
            base.OnPrepareStartGame();
        }
        public override void OnStartGame()
        {
            base.OnStartGame();
        }

        public override void OnPrepareNewGame()
        {
            base.OnPrepareNewGame();

            eyeObj = Game.CreatePhysicalObj(977, 163, "eyeObj", "game.crazytank.assetmap.WeiredEye", "1", 1, 1, 0);
            doorObj = Game.CreatePhysicalObj(1394, 715, "doorObj", "game.crazytank.assetmap.Gate", "1", 1, 1, 0);

            Game.AddAction(new FocusAction(977, 163, 0, 150, 0));
            Game.AddAction(new FocusAction(1394, 715, 0, 3000, 0));
            Game.AddAction(new LivingCallFunctionAction(null, Createbattle, 4000));
        }

        public void Createbattle()
        {
            battleNpc = Game.CreateNpc(battleID, 1394, 500, 0, 1, -1);
            battleNpc.SetRelateDemagemRect(-21, -103, 39, 56);
            battleNpc.Degree = 3;
            Game.AddAction(new LivingCallFunctionAction(null, CreateBoss, 2000));
            Game.AddAction(new ShowBloodItem(battleNpc.Id, 0, 0));
        }

        public void CreateBoss()
        {
            guardBoss = Game.CreateBoss(bossID, 1300, 600, -1, 0);
            guardBoss.FallFrom(1300, 702, "fall", 0, 2, 1000);
            guardBoss.SetRelateDemagemRect(-12, -50, 23, 37);
            guardBoss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.TSSM3205.msg1"), 0, 1100, 5000);
            guardBoss.Degree = 1;
            guardBoss.AddDelay(16);
            Game.AddAction(new ShowBloodItem(battleNpc.Id, 0, 0));
            Game.AddAction(new LivingCallFunctionAction(null, CreateBloom, 6500));
        }

        public void CreateBloom()
        {
            SimpleNpc bloom = Game.CreateNpc(bloomID, 750, 486, 0, -1);
            bloom.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.TSSM3205.msg2"), 0, 1500,3000);
            bloom.PlayMovie("die", 5000, 0);
            bloom.Die(6500);
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
            base.CanGameOver();

            if (battleNpc != null && !battleNpc.IsLiving)
            {
                Game.IsWin = true;
                Game.AddAction(new FocusAction(977, 163, 0, 150, 0));
                eyeObj.PlayMovie("die", 1000, 0);
                Game.AddAction(new FocusAction(1394, 715, 0, 3000, 0));
                doorObj.PlayMovie("die", 4000, 2000);
                return true;
            }

            return false;
        }

        public override int UpdateUIData()
        {
            int killcount = 0;
            if (battleNpc != null && !battleNpc.IsLiving)
            {
                killcount = 1;
            }
            return killcount;
        }

        public override void OnPrepareGameOver()
        {
            base.OnPrepareGameOver();
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (battleNpc != null && !battleNpc.IsLiving)
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
            loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/2/show2", ""));
            Game.SendLoadResource(loadingFileInfos);
        }
    }
}
