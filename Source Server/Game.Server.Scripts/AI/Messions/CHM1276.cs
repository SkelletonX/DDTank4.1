using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Bussiness;


namespace GameServerScript.AI.Messions
{
    public class CHM1276 : AMissionControl
    {
        private PhysicalObj m_kingMoive = null;

        private PhysicalObj m_kingFront = null;

        private SimpleBoss m_king = null;

        private SimpleBoss m_secondKing = null;

        private PhysicalObj[] m_leftWall = null;

        private PhysicalObj[] m_rightWall = null;

        private int m_kill = 0;

        private int m_state = 1205;

        private int turn = 0;

        private int firstBossID = 1205;

        private int secondBossID = 1206;

        private int npcID = 1210;

        private int direction;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1650)
            {
                return 3;
            }
            else if (score > 1550)
            {
                return 2;
            }
            else if (score > 1450)
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
            Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_left");
            Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_right");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.boguoLeaderAsset");
            int[] resources = { firstBossID, secondBossID, npcID };
            Game.LoadResources(resources);
            int[] gameOverResources = { firstBossID };
            Game.LoadNpcGameOverResources(gameOverResources);
            Game.SetMap(1076);
            Game.IsBossWar = LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1276.msg1");
        }

        public override void OnStartGame()
        {
            base.OnStartGame();

        }

        public override void OnPrepareNewGame()
        {
            base.OnPrepareNewGame();

            m_kingMoive = Game.Createlayer(0, 0, "kingmoive", "game.asset.living.BossBgAsset", "out", 1, 0);
            m_kingFront = Game.Createlayer(720, 495, "font", "game.asset.living.boguoKingAsset", "out", 1, 0);
            m_king = Game.CreateBoss(m_state, 888, 590, -1, 1);

            m_king.FallFrom(888, 690, "fall", 0, 2, 1000);
            m_king.SetRelateDemagemRect(-21, -87, 72, 59);
            m_king.AddDelay(10);

            m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1276.msg2"), 3000, 0);

            m_kingMoive.PlayMovie("in", 9000, 0);
            m_kingFront.PlayMovie("in", 9000, 0);
            m_kingMoive.PlayMovie("out", 13000, 0);
            m_kingFront.PlayMovie("out", 13400, 0);

            turn = Game.TurnIndex;

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

            if (Game.TurnIndex > turn + 1)
            {
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

        }

        public override bool CanGameOver()
        {
            base.CanGameOver();
            if (m_king.IsLiving == false)
            {
                if (m_state == firstBossID)
                {
                    m_state++;
                }
            }

            if (m_state == secondBossID && m_secondKing == null)
            {
                m_secondKing = Game.CreateBoss(m_state, m_king.X, m_king.Y, m_king.Direction, 1);
                Game.RemoveLiving(m_king.Id);


                if (m_secondKing.Direction == 1)
                {
                    m_secondKing.SetRect(-40, -112, 115, 96);

                }
                m_secondKing.SetRelateDemagemRect(-21, -87, 72, 59);

                m_secondKing.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1276.msg3"), 0, 3000);

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

                m_secondKing.AddDelay(minDelay - 2000);
                turn = Game.TurnIndex;
            }

            if (m_secondKing != null && m_secondKing.IsLiving == false)
            {
                m_kill++;
                direction = m_secondKing.Direction;
                return true;
            }

            return false;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return m_kill;
        }

        public override void OnPrepareGameOver()
        {
            base.OnPrepareGameOver();
            m_leftWall = Game.FindPhysicalObjByName("wallLeft");
            m_rightWall = Game.FindPhysicalObjByName("wallRight");

            for (int i = 0; i < m_leftWall.Length; i++)
                Game.RemovePhysicalObj(m_leftWall[i], true);

            for (int i = 0; i < m_rightWall.Length; i++)
                Game.RemovePhysicalObj(m_rightWall[i], true);

            /** 死亡倒播动画
            if (m_secondKing != null && m_secondKing.IsLiving == false)
            {
                PhysicalObj objKing = Game.CreatePhysicalObj(m_secondKing.X, m_secondKing.Y, "king", "game.living.LivingRecover005", "0", -direction, 1, 0);
                Game.RemoveLiving(m_secondKing.Id);
                objKing.PlayMovie("1", 0, 2000);
            }*/
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (m_state == secondBossID && m_secondKing.IsLiving == false)
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }

            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
            loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/show7.jpg", ""));
            Game.SendLoadResource(loadingFileInfos);
        }
    }
}
