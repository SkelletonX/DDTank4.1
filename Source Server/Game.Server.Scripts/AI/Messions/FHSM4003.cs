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
    public class FHSM4003 : AMissionControl
    {
        private PhysicalObj m_kingMoive;

        private PhysicalObj m_kingFront;

        private SimpleBoss cattleBoss = null;

        private SimpleBoss frantCattleBoss = null;

        private int m_state = 4008;

        private int fireID = 4007;

        private int bossID = 4008;

        private int frantbossID = 4009;

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
            int[] resources = { fireID, bossID, frantbossID };
            int[] gameOverResources = { bossID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResources);
            Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_left");
            Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_right");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.boguoLeaderAsset");
            Game.AddLoadingFile(2, "image/map/1075/objects/1075Object.swf", "game.crazytank.assetmap.Board001");
            Game.SetMap(1126);
            Game.IsBossWar = "最终BOSS";
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
            m_kingFront = Game.Createlayer(900, 300, "font", "game.asset.living.boguoLeaderAsset", "out", 1, 0);

            cattleBoss = Game.CreateBoss(bossID, 1000, 450, -1, 0);
            cattleBoss.FallFrom(1000, 500, "fall", 0, 2, 1000);
            cattleBoss.SetRelateDemagemRect(-35, -130, 70, 130);
            cattleBoss.Say("我就是老牛！你们就是嫩草！", 0, 3000);
            cattleBoss.ChangeMaxBeatDis = 1500;
            cattleBoss.AddDelay(16);
            m_kingMoive.PlayMovie("in", 8000, 0);
            m_kingFront.PlayMovie("in", 8000, 0);
            m_kingMoive.PlayMovie("out", 12000, 0);
            m_kingFront.PlayMovie("out", 12400, 0);
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

            if (!cattleBoss.IsLiving)
            {
                if (m_state == bossID)
                {
                    m_state++;
                }
            }

            if (m_state == frantbossID && frantCattleBoss == null)
            {
                frantCattleBoss = Game.CreateBoss(m_state, cattleBoss.X, cattleBoss.Y, cattleBoss.Direction, 1,20);
                frantCattleBoss.ChangeMaxBeatDis = 1500;
                Game.RemoveLiving(cattleBoss.Id);
                frantCattleBoss.SetRelateDemagemRect(-41, -200, 82, 200);
                List<Player> players = Game.GetAllFightPlayers();
                Player RandomPlayer = Game.FindRandomPlayer();

                int minDelay = 0;

                if (RandomPlayer != null)
                {
                    minDelay = RandomPlayer.Delay;
                }

                foreach (Player player in players)
                {
                    if (player.Delay < minDelay)
                    {
                        minDelay = player.Delay;
                    }
                }

                frantCattleBoss.Delay = minDelay - 1;
            }

            if (frantCattleBoss != null && frantCattleBoss.IsLiving == false)
            {
                if (!frantCattleBoss.IsLiving)
                {
                    Game.IsWin = true;
                    return true;
                }
            }

            return false;
        }

        public override int UpdateUIData()
        {
            int killcount = 1;
            if (frantCattleBoss != null && !frantCattleBoss.IsLiving)
            {
                killcount = 0;
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
            if (frantCattleBoss != null && !frantCattleBoss.IsLiving)
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
