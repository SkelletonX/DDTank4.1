using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Game.Logic.Actions;
using System.Drawing;

namespace GameServerScript.AI.Messions
{
    public class FHSM4002 : AMissionControl
    {
        private PhysicalObj m_kingMoive;

        private PhysicalObj m_kingFront;

        private SimpleWingBoss hawkBoss = null;

        private SimpleBoss wolfBoss = null;

        private int hawkBossID = 4005;

        private int wolfBossID = 4006;

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
            int[] resources = { hawkBossID, wolfBossID };
            int[] gameOverResources = { hawkBossID, wolfBossID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResources);
            Game.AddLoadingFile(1, "bombs/48.swf", "tank.resource.bombs.Bomb48");
            Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_left");
            Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_right");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.boguoLeaderAsset");
            Game.AddLoadingFile(2, "image/map/1075/objects/1075Object.swf", "game.crazytank.assetmap.Board001");
            Game.SetMap(1126);
            Game.IsBossWar = "沙漠双雄";
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
            m_kingMoive = Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 0);
            m_kingFront = Game.Createlayer(900, 200, "font", "game.asset.living.boguoLeaderAsset", "out", 1, 0);

            Game.AddAction(new FocusAction(350, 0, 0, 150, 1000));
            hawkBoss = Game.CreateWingBoss(hawkBossID, 800, 300, -1, 0);
            hawkBoss.SetRelateDemagemRect(-30, -75, 60, 70);
            hawkBoss.Degree = 1;
            hawkBoss.Say("沙漠神鹰就是我了,要签名的赶快过来~", 0, 3000);
            hawkBoss.AddDelay(16);

            Game.AddAction(new FocusAction(1000, 400, 0, 7000, 1000));
            wolfBoss = Game.CreateBoss(wolfBossID, 1000, 400, -1, 0);
            wolfBoss.FallFrom(1000, 450, "fall", 0, 2, 1000);
            wolfBoss.SetRelateDemagemRect(-116, -111, 157, 119);
            wolfBoss.Degree = 2;
            wolfBoss.Say("沙漠血狼就是我了,要签名的赶快过来~", 0, 9000);
            wolfBoss.AddDelay(16);
            m_kingMoive.PlayMovie("in", 15000, 0);
            m_kingFront.PlayMovie("in", 15000, 0);
            m_kingMoive.PlayMovie("out", 20000, 0);
            m_kingFront.PlayMovie("out", 20400, 0);
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

            if (!hawkBoss.IsLiving && !wolfBoss.IsLiving)
            {
                Game.IsWin = true;
                return true;
            }

            return false;
        }

        public override int UpdateUIData()
        {
            int killcount = 0;
            List<TurnedLiving> living = Game.TurnQueue;
            foreach (TurnedLiving turnedLiving in living)
            {
                if (turnedLiving.IsLiving && turnedLiving.Degree > 0 && (turnedLiving is SimpleBoss))
                    killcount++;
            }
            if (killcount != 0)
            {
                killcount = 2 - killcount;
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
            if (!hawkBoss.IsLiving && !wolfBoss.IsLiving)
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
