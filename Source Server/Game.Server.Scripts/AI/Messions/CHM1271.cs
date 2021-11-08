using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;

namespace GameServerScript.AI.Messions
{
    public class CHM1271 : AMissionControl
    {
        private List<SimpleNpc> someNpc = new List<SimpleNpc>();

        private int dieRedCount = 0;

        private int redNpcID = 1201;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1810)
            {
                return 3;
            }
            else if (score > 1750)
            {
                return 2;
            }
            else if (score > 1690)
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
            int[] resources = { redNpcID };
            int[] gameOverResource = { redNpcID, redNpcID, redNpcID };
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

            for (int i = 0; i < 4; i++)
            {

                if (i < 1)
                {
                    someNpc.Add(Game.CreateNpc(redNpcID, 900 + (i + 1) * 100, 505, 1));
                }
                else if (i < 3)
                {
                    someNpc.Add(Game.CreateNpc(redNpcID, 920 + (i + 1) * 100, 505, 1));
                }
                else
                {
                    someNpc.Add(Game.CreateNpc(redNpcID, 1000 + (i + 1) * 100, 515, 1));
                }
            }

            someNpc.Add(Game.CreateNpc(redNpcID, 1465, 494, 1));

            Game.BossCardCount = 1;
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();


            PVEGame pveGame = Game as PVEGame;

            if (Game.GetLivedLivings().Count == 0)
            {
                pveGame.PveGameDelay = 0;
            }
            if (Game.TurnIndex > 1 && Game.CurrentPlayer.Delay > Game.PveGameDelay)
            {
                if (Game.GetLivedLivings().Count == 15)
                {
                    return;
                }

                if (someNpc.Count < 15)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (i < 1)
                        {
                            someNpc.Add(Game.CreateNpc(redNpcID, 900 + (i + 1) * 100, 505, 1));
                        }
                        else if (i < 3)
                        {
                            someNpc.Add(Game.CreateNpc(redNpcID, 920 + (i + 1) * 100, 505, 1));
                        }
                        else
                        {
                            someNpc.Add(Game.CreateNpc(redNpcID, 1000 + (i + 1) * 100, 515, 1));
                        }
                    }
                    someNpc.Add(Game.CreateNpc(redNpcID, 1465, 494, 1));
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (Game.GetLivedLivings().Count < 15 && someNpc.Count < Game.MissionInfo.TotalCount)
                        {
                            if (i < 1)
                            {
                                someNpc.Add(Game.CreateNpc(redNpcID, 900 + (i + 1) * 100, 505, 1));
                            }
                            else if (i < 3)
                            {
                                someNpc.Add(Game.CreateNpc(redNpcID, 920 + (i + 1) * 100, 505, 1));
                            }
                            else
                            {
                                someNpc.Add(Game.CreateNpc(redNpcID, 1000 + (i + 1) * 100, 515, 1));
                            }

                        }
                        else
                        {
                            break;
                        }
                    }

                    if (Game.GetLivedLivings().Count < 15 && someNpc.Count < Game.MissionInfo.TotalCount)
                    {
                        someNpc.Add(Game.CreateNpc(redNpcID, 1465, 494, 1));
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
            bool result = true;

            base.CanGameOver();

            dieRedCount = 0;

            foreach (SimpleNpc redNpc in someNpc)
            {
                if (redNpc.IsLiving)
                {
                    result = false;
                }
                else
                {
                    dieRedCount++;
                }
            }

            if (result && dieRedCount == Game.MissionInfo.TotalCount)
            {
                Game.IsWin = true;
                return true;
            }
            return false;
        }

        public override int UpdateUIData()
        {
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
            loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/2", ""));
            Game.SendLoadResource(loadingFileInfos);
        }
    }
}
