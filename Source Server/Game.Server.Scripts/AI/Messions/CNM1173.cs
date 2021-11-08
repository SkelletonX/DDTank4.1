using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Bussiness;


namespace GameServerScript.AI.Messions
{
    public class CNM1173 : AMissionControl
    {
        private SimpleBoss m_boss;

        private PhysicalObj m_moive;

        private PhysicalObj m_front;

        private int bossID = 1103;

        private int npcID = 1109;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1670)
            {
                return 3;
            }
            else if (score > 1570)
            {
                return 2;
            }
            else if (score > 1475)
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
            Game.AddLoadingFile(1, "bombs/61.swf", "tank.resource.bombs.Bomb61");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.boguoLeaderAsset");
            int[] resources = { bossID, npcID };
            int[] gameOverResource = { bossID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResource);
            Game.SetMap(1073);
            Game.IsBossWar = LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1173.msg1");
        }

        public override void OnStartGame()
        {
            base.OnStartGame();

        }

        public override void OnPrepareNewGame()
        {
            base.OnPrepareNewGame();

            m_moive = Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 0);
            m_front = Game.Createlayer(680, 330, "font", "game.asset.living.boguoLeaderAsset", "out", 1, 0);
            m_boss = Game.CreateBoss(bossID, 770, -1500, -1, 1);

            m_boss.FallFrom(770, 301, "fall", 0, 2, 1000);
            m_boss.SetRelateDemagemRect(34, -35, 11, 18);
            m_boss.AddDelay(10);
            m_boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1173.msg2"), 0, 6000);
            m_boss.PlayMovie("call", 5900, 0);
            m_moive.PlayMovie("in", 9000, 0);
            m_boss.PlayMovie("weakness", 10000, 5000);
            m_front.PlayMovie("in", 9000, 0);
            m_moive.PlayMovie("out", 15000, 0);
            //设置本关卡为Boss关卡，关卡胜利后，玩家可以翻一张牌
            Game.BossCardCount = 1;
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (Game.TurnIndex > 1)
            {
                if (m_moive != null)
                {
                    Game.RemovePhysicalObj(m_moive, true);
                    m_moive = null;
                }
                if (m_front != null)
                {
                    Game.RemovePhysicalObj(m_front, true);
                    m_front = null;
                }
            }
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();

            if (m_boss.IsLiving == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int UpdateUIData()
        {

            if (m_boss == null)
                return 0;

            if (m_boss.IsLiving == false)
            {
                return 1;
            }
            return base.UpdateUIData();
        }

        public override void OnPrepareGameOver()
        {
            base.OnPrepareGameOver();
        }

        public override void OnGameOver()
        {
            base.OnGameOver();

            if (m_boss.IsLiving == false)
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }

            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
            loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/1/show4.jpg", ""));
            Game.SendLoadResource(loadingFileInfos);
        }
    }
}
