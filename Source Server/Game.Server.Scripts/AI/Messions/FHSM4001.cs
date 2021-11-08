using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic.Effects;
using Game.Logic;
using System.Drawing;

namespace GameServerScript.AI.Messions
{
    public class FHSM4001 : AMissionControl
    {
        private List<SimpleNpc> someNpc = new List<SimpleNpc>();

        private int npcCount;

        private SimpleNpc doorNpc = null;

        private SimpleBoss blowNpc = null;

        private SimpleBoss gunNpc = null;

        private int blowNpcID = 4001;

        private int doorNpcID = 4002;

        private int shortNpcID = 4003;

        private int gunNpcID = 4004;

        private Point[] npcBirthPoint = { new Point(1450, 443), new Point(1420, 436), new Point(1400, 474), new Point(1378, 477), new Point(1275, 499), new Point(1221, 495), new Point(1155, 537) };

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
            int[] resources = { blowNpcID, doorNpcID, shortNpcID, gunNpcID };
            int[] gameOverResources = { shortNpcID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResources);
            Game.AddLoadingFile(1, "bombs/48.swf", "tank.resource.bombs.Bomb48");
            Game.SetMap(1126);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
        }

        public override void OnPrepareNewGame()
        {
            base.OnPrepareNewGame();
            doorNpc = Game.CreateNpc(doorNpcID, 1450, 400, 1);
            doorNpc.SetRelateDemagemRect(-26, -170, 52, 170);
            doorNpc.Degree = 1;

            blowNpc = Game.CreateBoss(blowNpcID, 600, 400, 1, 3);
            blowNpc.Degree = 3;
            gunNpc = Game.CreateBoss(gunNpcID, 1400, 400, 1,3);
            gunNpc.SetRelateDemagemRect(-1000, -1000, 2, 2);

            int index = Game.Random.Next(0, npcBirthPoint.Length);
            someNpc.Add(Game.CreateNpc(shortNpcID, npcBirthPoint[index].X, npcBirthPoint[index].Y, 1));
            index = Game.Random.Next(0, npcBirthPoint.Length);
            someNpc.Add(Game.CreateNpc(shortNpcID, npcBirthPoint[index].X, npcBirthPoint[index].Y, 1));
            index = Game.Random.Next(0, npcBirthPoint.Length);
            someNpc.Add(Game.CreateNpc(shortNpcID, npcBirthPoint[index].X, npcBirthPoint[index].Y, 1));
            someNpc[0].Degree = 2;
            someNpc[1].Degree = 2;
            someNpc[2].Degree = 2;
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (!blowNpc.IsLiving)
            {
                blowNpc = Game.CreateBoss(blowNpcID, 600, 400, 1, 3);
                blowNpc.Degree = 3;
            }
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > 500 && player.X < 1450 )
                {
                    player.AddEffect(new ContinueReduceBloodEffect(1, 800, null), 0);
                }
            }
            for( int i = 0;i < 3 - npcCount;i++ )
            {
                int index = Game.Random.Next(0, npcBirthPoint.Length);
                SimpleNpc npc = Game.CreateNpc(shortNpcID, npcBirthPoint[index].X, npcBirthPoint[index].Y, 1);
                npc.Degree = 2;
                someNpc.Add(npc);
            }
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();

            npcCount = 0;

            foreach (SimpleNpc npc in someNpc)
            {
                if (npc.IsLiving)
                {
                    npcCount++;
                }
            }

            if (!doorNpc.IsLiving)
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
            if (!doorNpc.IsLiving)
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
