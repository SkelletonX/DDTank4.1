using System;
using Game.Base.Packets;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Bussiness;

namespace GameServerScript.AI.Messions
{
    public class FLSS101 : AMissionControl
    {
        private SimpleNpc someNpc = null;

        private bool isBeginResult = false;

        private bool isCreateNpcFirst = true;

        private int createNpcCount = 0;

        private int arightResultCount = 0;

        private int needArightResult = 3;

        private int timeOut = 30;

        private int currQuizID = 0;

        private int enterQuizID = 0;

        private int redNpcID = 4;

        private int maxQuizSize = 10;

        private string[,] resultStr = new string[10, 3] { 
            {"","",""},
            {"","",""},
            {"","",""},
            {"","",""},
            {"","",""},
            {"","",""},
            {"","",""},
            {"","",""},
            {"","",""},
            {"","",""},
        };

        string[] distanceStr = new string[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", };

        private int[] arightResult = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

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
            InitNpcCoor();
        }

        public void InitNpcCoor()
        {
            List<int> m_tempList = new List<int>();
            int last = -1;
            int x = 0;
            for (int i = 0; i < maxQuizSize; i++)
            {
                m_tempList.Clear();
                do
                {
                    x = Game.Random.Next(0, 10);
                    createNpcX[i] = (x + 1) * 100 + 75;
                } while (x == last);
                last = x;
                int n = Game.Random.Next(0, 3);
                resultStr[i, n] = distanceStr[x];
                arightResult[i] = n;

                if (x - 1 >= 0 && x + 1 < 10)
                {
                    m_tempList.Add(x - 1);
                    m_tempList.Add(x + 1);
                }
                else if (x - 1 < 0)
                {
                    m_tempList.Add(x + 1);
                    m_tempList.Add(x + 2);
                }
                else if (x + 1 >= 10)
                {
                    m_tempList.Add(x - 1);
                    m_tempList.Add(x - 2);
                }

                for (int j = 0; j < 3; j++)
                {
                    if (resultStr[i, j] != distanceStr[x])
                    {
                        resultStr[i, j] = distanceStr[m_tempList[0]];
                        m_tempList.RemoveAt(0);
                    }
                }
            }
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
                    Reset(true);
                    KillNpc();
                    break;
                case 2:
                    SendQuizWindow();
                    break;
                case 3:
                    if (!isBeginResult)
                        break;
                    Reset(true);
                    break;
                case 4:
                    {
                        if (!isBeginResult)
                            break;
                        int quizID = packet.ReadInt();
                        int result = packet.ReadInt();
                        if (quizID != currQuizID)
                            break;
                        if (result == arightResult[quizID - 1])
                        {
                            arightResultCount++;
                            SendQuizWindow();
                        }
                        else
                        {
                            SendQuizWindow();
                        }
                        enterQuizID = quizID;
                        Game.WaitTime(0);
                    }
                    break;
                case 5:
                    {
                        if (!isBeginResult)
                            break;
                        Reset(false);
                        SendQuizWindow();
                    }
                    break;
            }
        }

        public void CreateNpc()
        {
            if (createNpcCount <= maxQuizSize)
            {
                if (isCreateNpcFirst)
                {
                    someNpc = Game.CreateNpc(redNpcID, 575, 505, 2, -1, true);
                    someNpc.SetRelateDemagemRect(-25, -55, 45, 55);
                    isCreateNpcFirst = false;
                    createNpcCount++;
                    return;
                }
                someNpc = Game.CreateNpc(redNpcID, createNpcX[currQuizID], 505, 2, -1, true);
                someNpc.SetRelateDemagemRect(-25, -55, 45, 55);
                createNpcCount++;
            }
        }

        public void KillNpc()
        {
            if (someNpc != null && someNpc.IsLiving)
            {
                someNpc.Die(0);
                someNpc = null;
            }
        }

        public void Reset(bool isFirstNpc)
        {
            currQuizID = 0;
            enterQuizID = 0;
            arightResultCount = 0;
            createNpcCount = 0;
            if (isFirstNpc)
                isCreateNpcFirst = true;
            else
                isCreateNpcFirst = false;
            isBeginResult = false;
            resultStr = new string[10, 3] { 
                                {"","",""},
                                {"","",""},
                                {"","",""},
                                {"","",""},
                                {"","",""},
                                {"","",""},
                                {"","",""},
                                {"","",""},
                                {"","",""},
                                {"","",""},};
            InitNpcCoor();
        }

        private void SendQuizWindow()
        {
            if (currQuizID > maxQuizSize - 1)
                return;
            if (arightResultCount >= needArightResult)
                return;
            KillNpc();
            CreateNpc();
            isBeginResult = true;
            Game.SendQuizWindow(currQuizID + 1, arightResultCount, needArightResult, maxQuizSize, timeOut, LanguageMgr.GetTranslation("FightLab.caption"),
            LanguageMgr.GetTranslation("FightLab.question"), resultStr[currQuizID, 0], resultStr[currQuizID, 1], resultStr[currQuizID, 2]);
            currQuizID++;
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] resources = { redNpcID };
            int[] gameOverResources = { redNpcID, redNpcID, redNpcID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResources);
            Game.SetMap(1136);
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
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override bool CanGameOver()
        {
            if (arightResultCount >= needArightResult)
            {
                Game.SendCloseQuizWindow();
                Game.WaitTime(1000);
                Game.IsWin = true;
                return true;
            }

            if (enterQuizID == maxQuizSize)
            {
                Game.SendCloseQuizWindow();
                Game.WaitTime(1000);
                if (arightResultCount >= needArightResult)
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
            if (arightResultCount >= needArightResult)
                Game.IsWin = true;
            else
                Game.IsWin = false;
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
        }
    }
}

