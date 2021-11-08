using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using SqlDataProvider.Data;
using Bussiness;
using System.Drawing;

namespace GameServerScript.AI.Messions
{
    public class GCGCK1161 : AMissionControl
    {
        private List<SimpleNpc> redNpc = new List<SimpleNpc>();

        private List<SimpleNpc> blueNpc = new List<SimpleNpc>();

        private List<Point> m_pointRed = new List<Point>() { new Point(958, 950), new Point(1400, 950), new Point(1034, 950), new Point(1472, 950) };

        private List<Point> m_pointBlue = new List<Point>() { new Point(1150, 950), new Point(1346, 950) };

        private PhysicalObj m_kingMoive;

        private int m_totalRed = 20;

        private int m_totalBlue = 10;

        private int maxRedOnMap = 10;

        private int maxBlueOnMap = 5;

        private int livingRed = 0;

        private int livingBlue = 0;

        private int redNpcID = 7202;

        private int blueNpcID = 7201;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 930)
            {
                return 3;
            }
            else if (score > 850)
            {
                return 2;
            }
            else if (score > 775)
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
            int[] resources = { redNpcID, blueNpcID };
            Game.AddLoadingFile(2, "image/game/living/living176.swf", "game.living.Living176");
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(resources);
            Game.SetMap(1161);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();

            m_kingMoive = Game.Createlayer(1200, 955, "kingmoive", "game.living.Living176", "in", 1, 0);

            RespawnBlueNpc(maxBlueOnMap);
            RespawnRedNpc(maxRedOnMap);
        }

        private void RespawnRedNpc(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Point point = m_pointRed[i];
                redNpc.Add(Game.CreateNpc(redNpcID, point.X, point.Y, 0, -1));
            }
        }

        private void RespawnBlueNpc(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Point point = m_pointBlue[i];
                blueNpc.Add(Game.CreateNpc(redNpcID, point.X, point.Y, 0, -1));
            }
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            // check can create npc
            if (livingRed < maxRedOnMap && redNpc.Count < m_totalRed)
            {
                // create living red
                ///      4            3          11              10            
                int redCountRespawn = maxRedOnMap - livingRed > m_totalRed - redNpc.Count ? m_totalRed - redNpc.Count : maxRedOnMap - livingRed;
                if (redCountRespawn > 0)
                    RespawnRedNpc(redCountRespawn);
            }

            if (livingBlue < maxBlueOnMap && blueNpc.Count < m_totalBlue)
            {
                // create living red
                ///      4            3          11              10            
                int blueCountRespawn = maxBlueOnMap - livingBlue > m_totalBlue - blueNpc.Count ? m_totalBlue - blueNpc.Count : maxBlueOnMap - livingBlue;
                if (blueCountRespawn > 0)
                    RespawnBlueNpc(blueCountRespawn);
            }
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();

            if (Game.GetLivedLivings().Count == 0)
            {
                Game.PveGameDelay = 0;
            }

            livingBlue = 0;
            livingRed = 0;

            foreach (SimpleNpc npc in redNpc)
            {
                if (npc.IsLiving)
                {
                    livingRed++;
                }
            }

            foreach (SimpleNpc blueNpcSingle in blueNpc)
            {
                if (blueNpcSingle.IsLiving)
                {
                    livingBlue++;
                }
            }

            if (blueNpc.Count >= m_totalBlue && redNpc.Count >= m_totalRed && Game.GetLivedLivings().Count <= 0)
            {
                return true;
            }

            if (Game.TurnIndex > Game.MissionInfo.TotalTurn)
            {
                return true;
            }

            return false;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return Game.TotalKillCount;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (Game.GetLivedLivings().Count == 0)
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }
        }
    }
}
