 using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using SqlDataProvider.Data;

namespace GameServerScript.AI.Messions
{
    public class CTM1372 : AMissionControl
    {
        private List<SimpleNpc> redNpc = new List<SimpleNpc>();

        private List<SimpleNpc> blueNpc = new List<SimpleNpc>();

        private int redNpcID = 1301;

        private int blueNpcID = 1302;

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
            base.OnPrepareNewSession();
            int[] resources = { redNpcID, blueNpcID };
            int[] gameOverResource = { blueNpcID, redNpcID, redNpcID, redNpcID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResource);
            Game.SetMap(1072);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();


        }

        public override void OnPrepareNewGame()
        {
            base.OnPrepareNewGame();

            CreateFullNpc();

            Game.BossCardCount = 1;
        }

        public override void OnNewTurnStarted()
        {
            PVEGame pveGame = Game as PVEGame;
            //int turnNpcRank = pveGame.FindTurnNpcRank();

            if (Game.GetLivedLivings().Count == 0)
            {
                pveGame.PveGameDelay = 0;
            }

            if (Game.TurnIndex > 1 && Game.CurrentPlayer.Delay > Game.PveGameDelay)
            {
                if (GetLivingBlueNpc() + GetLivingRedNpc() == 15)
                {
                    return;
                }

                if (redNpc.Count + blueNpc.Count < 15)
                {
                    CreateFullNpc();
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (GetLivingRedNpc() < 12 && redNpc.Count < 4 * Game.MissionInfo.TotalCount / 5)
                        {
                            if (i < 1)
                            {
                                redNpc.Add(Game.CreateNpc(redNpcID, 900 + (i + 1) * 100, 505, 1));
                            }
                            else if (i < 3)
                            {
                                redNpc.Add(Game.CreateNpc(redNpcID, 920 + (i + 1) * 100, 505, 1));
                            }
                            else
                            {
                                redNpc.Add(Game.CreateNpc(redNpcID, 1000 + (i + 1) * 100, 515, 1));
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (GetLivingBlueNpc() < 3 && blueNpc.Count < Game.MissionInfo.TotalCount / 5)
                    {
                        blueNpc.Add(Game.CreateNpc(blueNpcID, 1465, 494, 1));
                    }
                }
            }

        }


        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override bool CanGameOver()
        {
            if (GetLivingRedNpc() == 0 && GetLivingBlueNpc() == 0 && (redNpc.Count + blueNpc.Count) == Game.MissionInfo.TotalCount)
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
            if (Game.GetLivedLivings().Count == 0)
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

        private void CreateFullNpc()
        {
            for (int i = 0; i < 4; i++)
            {
                if (i < 1)
                {
                    redNpc.Add(Game.CreateNpc(redNpcID, 900 + (i + 1) * 100, 505, 1));
                }
                else if (i < 3)
                {
                    redNpc.Add(Game.CreateNpc(redNpcID, 920 + (i + 1) * 100, 505, 1));
                }
                else
                {
                    redNpc.Add(Game.CreateNpc(redNpcID, 1000 + (i + 1) * 100, 515, 1));
                }
            }

            blueNpc.Add(Game.CreateNpc(blueNpcID, 1465, 494, 1));
        }

        private int GetLivingRedNpc()
        {
            int count = 0;
            foreach (SimpleNpc npc in redNpc)
            {
                if (npc.IsLiving)
                    count++;
            }
            return count;
        }

        private int GetLivingBlueNpc()
        {
            int count = 0;
            foreach (SimpleNpc npc in blueNpc)
            {
                if (npc.IsLiving)
                    count++;
            }
            return count;
        }
    }
}
