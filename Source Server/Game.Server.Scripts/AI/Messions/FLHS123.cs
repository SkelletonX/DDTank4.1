using System;
using Game.Base.Packets;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic.Actions;
using Game.Logic;

namespace GameServerScript.AI.Messions
{
    public class FLHS123 : AMissionControl
    {
        private SimpleNpc someNpc = null;

        private bool isBeginHit = false;

        private bool isCreateNpcFirst = true;

        private int createNpcCount = 0;

        private int hitCount = 0;

        private int needHitCount = 9;

        private int currTurnID = 0;

        private int redNpcID = 4;

        private int maxTurnSize = 10;

        private int[] createNpcX = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


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

        public override void OnPrepareNewGame()
        {
            base.OnPrepareNewGame();
            int last = -1;
            int x = 0;
            for (int i = 0; i < maxTurnSize; i++)
            {
                do
                {
                    x = Game.Random.Next(5, 20);
                    createNpcX[i] = (x + 1) * 100 + 75;
                } while (x == last);
                last = x;
            }
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (!isBeginHit)
                return;
            if (isBeginHit && someNpc != null && !someNpc.IsLiving)
                hitCount++;
            KillNpc();
            CreateNpc();
        }

        public override void OnMissionEvent(GSPacketIn packet)
        {
            int type = packet.ReadInt();
            switch (type)
            {
                case 0:
                    CreateNpc();
                    break;
                case 1:
                    isBeginHit = false;
                    Reset();
                    KillNpc();
                    break;
                case 2:
                    isBeginHit = true;
                    someNpc = null;
                    Game.AddAction(new LivingCallFunctionAction(null, Skip, 4000));
                    break;
            }
        }

        public void Skip()
        {
            Game.CurrentPlayer.Skip(0);
        }

        public void CreateNpc()
        {
            if (createNpcCount <= maxTurnSize)
            {
                if (isCreateNpcFirst)
                {
                    someNpc = Game.CreateNpc(redNpcID, 1075, 565, 2, -1, true);
                    someNpc.SetRelateDemagemRect(-25, -55, 45, 55);
                    isCreateNpcFirst = false;
                    Game.WaitTime(0);
                    return;
                }
                someNpc = Game.CreateNpc(redNpcID, createNpcX[currTurnID], 565, 2, -1, true);
                someNpc.SetRelateDemagemRect(-25, -55, 45, 55);
                createNpcCount++;
                currTurnID++;
                Game.WaitTime(0);
            }
        }

        public void KillNpc()
        {
            if (someNpc != null && someNpc.IsLiving)
            {
                someNpc.Die();
                someNpc = null;
            }
        }

        public void Reset()
        {
            currTurnID = 0;
            hitCount = 0;
            createNpcCount = 0;
            isCreateNpcFirst = true;
            isBeginHit = false;
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] resources = { redNpcID };
            int[] gameOverResources = { redNpcID, redNpcID, redNpcID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResources);
            Game.SetMap(1135);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            if (Game.CurrentLiving != null)
            {
                ((Player)Game.CurrentLiving).Seal((Player)Game.CurrentLiving, 0, 0);
            }
        }

        public override bool CanGameOver()
        {
            int currHitCount = 0;
            if (isBeginHit && someNpc != null && !someNpc.IsLiving)
                currHitCount = 1;
            Game.TotalKillCount = hitCount + currHitCount;
            if (hitCount + currHitCount >= needHitCount)
            {
                Game.IsWin = true;
                return true;
            }
            
            if (createNpcCount == maxTurnSize)
            {
                if (hitCount + currHitCount >= needHitCount)
                    Game.IsWin = true;
                else
                    Game.IsWin = false;
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

        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
        }
    }
}

