using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using System.Drawing;

namespace GameServerScript.AI.Messions
{
    public class TNSM3101 : AMissionControl
    {
        private List<SimpleNpc> shortNpc = new List<SimpleNpc>();

        private List<SimpleNpc> longNpc = new List<SimpleNpc>();

        private int dieRedCount = 0;

        private int livingShortCount = 0;

        private int livingLongCount = 0;

        private int[] npcIDs = { 3101, 3104 };

        private Point[] npcBirthPoint = { new Point(1550, 393), new Point(1493, 386), new Point(1444, 424), new Point(1378, 427), new Point(1275, 449), new Point(1221, 445), new Point(1155, 487) };

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
            int[] resources = { npcIDs[0], npcIDs[1] };
            int[] gameOverResources = { npcIDs[0], npcIDs[1] };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResources);
            Game.AddLoadingFile(1, "bombs/58.swf", "tank.resource.bombs.Bomb58");
            Game.SetMap(1122);
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
            loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/1141/dead.png", ""));
            Game.SendLoadResource(loadingFileInfos);
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
            SimpleNpc npc = null;

            //右边小怪
            int index = 0;
            for (int i = 0; i < 8; i++)
            {
                index = Game.Random.Next(0, npcBirthPoint.Length);
                npc = Game.CreateNpc(npcIDs[0], npcBirthPoint[index].X, npcBirthPoint[index].Y, 1, -1, 100, 0);
                shortNpc.Add(npc);
            }
            for (int i = 0; i < 2; i++)
            {
                index = Game.Random.Next(0, npcBirthPoint.Length);
                npc = Game.CreateNpc(npcIDs[1], npcBirthPoint[index].X, npcBirthPoint[index].Y, 1, -1, 100, 1);
                longNpc.Add(npc);
            }
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();

            if (Game.GetLivedLivings().Count == 0)
            {
                Game.PveGameDelay = 0;
            }

            if (Game.TurnIndex > 1 && Game.CurrentLiving.Delay > Game.PveGameDelay)
            {
                if (Game.GetLivedLivings().Count + dieRedCount < Game.MissionInfo.TotalCount)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (shortNpc.Count + longNpc.Count == Game.MissionInfo.TotalCount)
                        {
                            break;
                        }
                        else
                        {
                            if (i == 0 && livingShortCount < 8)
                            {
                                livingShortCount++;
                                int index = Game.Random.Next(0, npcBirthPoint.Length);
                                SimpleNpc npc = Game.CreateNpc(npcIDs[0], npcBirthPoint[index].X, npcBirthPoint[index].Y, 1, -1, 100, 0);
                                shortNpc.Add(npc);
                            }
                            if (i == 1 && livingShortCount < 8)
                            {
                                livingShortCount++;
                                int index = Game.Random.Next(0, npcBirthPoint.Length);
                                SimpleNpc npc = Game.CreateNpc(npcIDs[0], npcBirthPoint[index].X, npcBirthPoint[index].Y, 1, -1, 100, 0);
                                shortNpc.Add(npc);
                            }
                            if (i == 2 && livingLongCount < 3)
                            {
                                livingLongCount++;
                                int index = Game.Random.Next(0, npcBirthPoint.Length);
                                SimpleNpc npc = Game.CreateNpc(npcIDs[1], npcBirthPoint[index].X, npcBirthPoint[index].Y, 1, -1, 100, 1);
                                longNpc.Add(npc);
                            }
                        }
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

            livingShortCount = 0;

            livingLongCount = 0;

            foreach (SimpleNpc redNpc in shortNpc)
            {
                if (redNpc.IsLiving)
                {
                    livingShortCount++;
                    result = false;
                }
                else
                {
                    dieRedCount++;
                }
            }

            foreach (SimpleNpc redNpc in longNpc)
            {
                if (redNpc.IsLiving)
                {
                    livingLongCount++;
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
            loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/2/show2", ""));
            Game.SendLoadResource(loadingFileInfos);
        }
    }
}
