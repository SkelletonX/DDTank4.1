using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;

namespace GameServerScript.AI.Messions
{
    public class PSM5001 : AMissionControl
    {
        private List<SimpleBoss> Npcs = new List<SimpleBoss>();

        private SimpleBoss m_boss = null;

        private int npcID = 5001;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1750)
            {
                return 3;
            }
            else if (score > 1675)
            {
                return 2;
            }
            else if (score > 1600)
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
            Game.AddLoadingFile(1, "bombs/57.swf", "tank.resource.bombs.Bomb57");

            base.OnPrepareNewSession();
            int[] resources = { npcID };
            int[] gameOverResource = { npcID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResource);
            Game.SetMap(1131);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
        }

        public override void OnPrepareNewGame()
        {
            base.OnPrepareNewGame();

            //for (int i = 0; i < Game.PlayerCount; i++)
            //{
            //    NPCTotalCount++;

            //    Npcs.Add(Game.CreateBoss(NpcID, 1));
            //}
            m_boss = Game.CreateBoss(npcID, 1);
            m_boss.SetRelateDemagemRect(-31, -75, 75, 70);
            //m_boss.Blood = (int)(m_boss.Blood * Game.GetTeamFightPower() / 2);
            //m_boss.BaseDamage = (int)(m_boss.BaseDamage * Game.GetTeamFightPower() / 10);
            //m_boss.BaseGuard = (int)(m_boss.BaseGuard * Game.GetTeamFightPower() / 10);
            //m_boss.Defence = (int)(m_boss.Defence * Game.GetTeamFightPower() / 2);
            //m_boss.Lucky = (int)(m_boss.Lucky * Game.GetTeamFightPower() / 200);
            //Game.BossCardCount = 1;
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
            //foreach (SimpleBoss NpcSingle in Npcs)
            //{
            //    if (NpcSingle.IsLiving)
            //    {
            //        result = false;
            //    }
            //}
            if (!m_boss.IsLiving)
            {
                Game.IsWin = true;
                return true;
            }
            return false;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return Game.TotalKillCount;
        }

        public override void OnPrepareGameOver()
        {
            base.OnPrepareGameOver();
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (Game.GetAllLivingPlayers().Count > 0)
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
            loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/3", ""));
            Game.SendLoadResource(loadingFileInfos);
        }
    }
}
