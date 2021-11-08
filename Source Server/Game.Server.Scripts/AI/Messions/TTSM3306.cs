using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Game.Logic.Actions;
using Bussiness;

namespace GameServerScript.AI.Messions
{
    public class TTSM3306 : AMissionControl
    {
        private PhysicalObj m_kingMoive;

        private PhysicalObj m_kingFront;

        private SimpleBoss firstBoss = null;

        private SimpleBoss secondBoss = null;

        private int m_state = 3316;

        private int firstbossID = 3316;

        private int secondbossID = 3317;

        private int bloomID = 3313;

        private int npcID = 3303;

        private int blownpcID = 3312;

        private int bloomSnpcID = 3318;

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
            Game.AddLoadingFile(1, "bombs/53.swf", "tank.resource.bombs.Bomb53");
            Game.AddLoadingFile(1, "bombs/54.swf", "tank.resource.bombs.Bomb54");
            Game.AddLoadingFile(1, "bombs/55.swf", "tank.resource.bombs.Bomb55");
            Game.AddLoadingFile(2, "image/map/1126/object/1126object.swf", "game.assetmap.Flame");
            Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_left");
            Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_right");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.ClanLeaderAsset");
            int[] resources = { bloomID, firstbossID, secondbossID, npcID, blownpcID, bloomSnpcID };
            int[] gameOverResources = { firstbossID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResources);
            Game.SetMap(1126);
            Game.IsBossWar = "原始人部落长";
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
            m_kingFront = Game.Createlayer(570, 300, "font", "game.asset.living.ClanLeaderAsset", "out", 1, 0);

            firstBoss = Game.CreateBoss(firstbossID, 745, 410, -1, 1);
            firstBoss.FallFrom(745, 578, "fall", 0, 2, 1000);
            firstBoss.SetRelateDemagemRect(-15, -117, 26, 96);
            firstBoss.AddDelay(16);
            firstBoss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.TTSM3306.msg1"), 0, 3000);
            firstBoss.PlayMovie("", 3000, 5000);

            m_kingMoive.PlayMovie("in", 10000, 0);
            m_kingFront.PlayMovie("in", 10000, 0);
            m_kingMoive.PlayMovie("out", 15000, 0);
            m_kingFront.PlayMovie("out", 15400, 0);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (m_kingMoive != null)
            {
                Game.RemovePhysicalObj(m_kingMoive, true);
                m_kingMoive = null;
            }

            if (m_kingFront != null)
            {
                Game.RemovePhysicalObj(m_kingFront, true);
                m_kingFront = null;
            }
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();

            if (firstBoss.IsLiving == false)
            {
                if (m_state == firstbossID)
                {
                    m_state++;
                }
            }

            if (m_state == secondbossID && secondBoss == null)
            {
                secondBoss = Game.CreateBoss(m_state, firstBoss.X, firstBoss.Y, firstBoss.Direction, 1);
                Game.RemoveLiving(firstBoss.Id);
                secondBoss.SetRelateDemagemRect(-17, -141, 38, 123);
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

                secondBoss.AddDelay(minDelay - 2000);
            }

            if (secondBoss != null && secondBoss.IsLiving == false)
            {
                if (!secondBoss.IsLiving)
                {
                    Game.IsWin = true;
                    return true;
                }
            }

            return false;
        }

        public override int UpdateUIData()
        {
            int killcount = 0;
            if (secondBoss != null)
            {
                if (!secondBoss.IsLiving)
                {
                    killcount = 1;
                }
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
            if (secondBoss != null)
            {
                if (!secondBoss.IsLiving)
                {
                    Game.IsWin = true;
                }
                else
                {
                    Game.IsWin = false;
                }
            }
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
            loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/2/show2", ""));
            Game.SendLoadResource(loadingFileInfos);
        }
    }
}
