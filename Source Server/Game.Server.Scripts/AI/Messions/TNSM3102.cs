using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Game.Logic.Actions;
using System.Drawing;
using Bussiness;

namespace GameServerScript.AI.Messions
{
    public class TNSM3102 : AMissionControl
    {
        private List<SimpleNpc> shortNpc = new List<SimpleNpc>();

        private List<SimpleNpc> longNpc = new List<SimpleNpc>();

        private int createLongNpcTime = 0;

        private SimpleNpc bloomNpc = null;

        private SimpleNpc boss = null;

        private int dieRedCount = 0;

        private int livingShortCount = 0;

        private int livingLongCount = 0;

        private int[] npcIDs = { 3102, 3105 };

        private int bloomIDs = 3107;

        private int bossID = 3119;

        private Point[] npcBirthLeftPoint = { new Point(56, 398), new Point(103, 413), new Point(148, 438), new Point(189, 428), new Point(239, 434), new Point(294, 446) };

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
            int[] resources = { npcIDs[0], npcIDs[1], bloomIDs, bossID };
            int[] gameOverResources = { npcIDs[0], npcIDs[1] };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResources);
            Game.AddLoadingFile(1, "bombs/58.swf", "tank.resource.bombs.Bomb58");
            Game.SetMap(1123);
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
            boss = Game.CreateNpc(bossID, 200, 435, 0, 1);
            boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.TNSM3102.msg3"), 0, 2000, 3000);
            boss.PlayMovie("call", 2000, 3000);
            boss.PlayMovie("out", 8000, 0);

            Game.AddAction(new FocusAction(1190, 0, 0, 11000, 1000));
            Game.AddAction(new LivingCallFunctionAction(null, CreateNpc, 6000));
            Game.AddAction(new LivingCallFunctionAction(null, ClearBoss, 10000));
            Game.AddAction(new LivingCallFunctionAction(null, CreateBloomNpc, 12000));
        }

        public void CreateNpc()
        {
            //左边5只小怪
            int index;
            index = Game.Random.Next(0, npcBirthLeftPoint.Length);
            shortNpc.Add(Game.CreateNpc(npcIDs[0], npcBirthLeftPoint[index].X, npcBirthLeftPoint[index].Y, 1, 1, 100, 0));
            index = Game.Random.Next(0, npcBirthLeftPoint.Length);
            shortNpc.Add(Game.CreateNpc(npcIDs[0], npcBirthLeftPoint[index].X, npcBirthLeftPoint[index].Y, 1, 1, 100, 0));
            index = Game.Random.Next(0, npcBirthLeftPoint.Length);
            shortNpc.Add(Game.CreateNpc(npcIDs[0], npcBirthLeftPoint[index].X, npcBirthLeftPoint[index].Y, 1, 1, 100, 0));
            index = Game.Random.Next(0, npcBirthLeftPoint.Length);
            shortNpc.Add(Game.CreateNpc(npcIDs[0], npcBirthLeftPoint[index].X, npcBirthLeftPoint[index].Y, 1, 1, 100, 0));
            index = Game.Random.Next(0, npcBirthLeftPoint.Length);
            longNpc.Add(Game.CreateNpc(npcIDs[1], npcBirthLeftPoint[index].X, npcBirthLeftPoint[index].Y, 1, 1, 100, 1));
        }

        public void CreateBloomNpc()
        {
            bloomNpc = Game.CreateNpc(bloomIDs, 1190, 461, 2, -1, 60, 0);
            bloomNpc.SetRelateDemagemRect(-10, -43, 15, 26);
            bloomNpc.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.TNSM3102.msg1"), 0, 1500, 3000);
            Game.AddAction(new ShowBloodItem(bloomNpc.Id, 0, 0));
        }

        public void ClearBoss()
        {
            boss.Die(0);
            //Game.RemoveLiving(boss.Id);
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
                for (int i = 0; i < 3; i++)
                {
                    if (i == 0 && livingShortCount < 8)
                    {
                        livingShortCount++;
                        int index = Game.Random.Next(0, npcBirthLeftPoint.Length);
                        shortNpc.Add(Game.CreateNpc(npcIDs[0], npcBirthLeftPoint[index].X, npcBirthLeftPoint[index].Y, 1, 1, 100, 0));
                    }
                    if (i == 1 && livingShortCount < 8)
                    {
                        livingShortCount++;
                        int index = Game.Random.Next(0, npcBirthLeftPoint.Length);
                        shortNpc.Add(Game.CreateNpc(npcIDs[0], npcBirthLeftPoint[index].X, npcBirthLeftPoint[index].Y, 1, 1, 100, 0));
                    }
                    if (i == 2 && livingLongCount < 3)
                    {
                        if (createLongNpcTime == 6)
                        {
                            createLongNpcTime = 0;
                            livingLongCount++;
                            int index = Game.Random.Next(0, npcBirthLeftPoint.Length);
                            longNpc.Add(Game.CreateNpc(npcIDs[1], npcBirthLeftPoint[index].X, npcBirthLeftPoint[index].Y, 1, 1, 100, 1));
                        }
                        else
                            createLongNpcTime++;
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
            base.CanGameOver();

            dieRedCount = 0;

            livingShortCount = 0;

            livingLongCount = 0;

            foreach (SimpleNpc redNpc in shortNpc)
            {
                if (redNpc.IsLiving)
                {
                    livingShortCount++;
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
                }
                else
                {
                    dieRedCount++;
                }
            }

            if (!bloomNpc.IsLiving)
            {
                Game.IsWin = false;
                return true;
            }
            if (bloomNpc.Blood == bloomNpc.MaxBlood)
            {
                Game.SendUpdateUiData();
                Game.IsWin = true;
                Game.AddAction(new FocusAction(bloomNpc.X, 0, 0, 0, 2000));
                bloomNpc.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.TNSM3102.msg2"), 0, 1500, 0);
                bloomNpc.PlayMovie("die", 4000, 0);
                Game.AddAction(new FocusAction(bloomNpc.X, 0, 3, 6000, 0));
                bloomNpc.PlayMovie("grow", 8200, 5000);
                return true;
            }

            return false;
        }

        public override int UpdateUIData()
        {
            if (bloomNpc != null && bloomNpc.Blood == bloomNpc.MaxBlood)
                return 1;
            else
                return 0;
        }

        public override void OnPrepareGameOver()
        {
            base.OnPrepareGameOver();
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (!bloomNpc.IsLiving)
            {
                Game.IsWin = false;
            }
            else if (Game.GetLivedLivings().Count == 0 || bloomNpc.Blood == bloomNpc.MaxBlood)
            {
                Game.IsWin = true;
            }
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
            loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/2/show2", ""));
            loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/1141/back.jpg", ""));
            Game.SendLoadResource(loadingFileInfos);
        }
    }
}
