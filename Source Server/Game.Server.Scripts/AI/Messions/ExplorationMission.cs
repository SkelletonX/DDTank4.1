using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using System.Drawing;

namespace GameServerScript.AI.Messions
{
    public class NpcCreateParam
    {
        private int _remoteCount;
        private int _livingCount;
        private int _turnCreateRemoteNum;
        private int _turnCreateLivingNum;
        private int _samePingMaxRemoteNum;
        private int _samePingMaxLivingNum;

        public int RemoteCount
        {
            get { return _remoteCount; }
            set { _remoteCount = value; }
        }
        public int LivingCount
        {
            get { return _livingCount; }
            set { _livingCount = value; }
        }

        public int TurnCreateRemoteNum
        {
            get { return _turnCreateRemoteNum; }
            set { _turnCreateRemoteNum = value; }
        }

        public int TurnCreateLivingNum
        {
            get { return _turnCreateLivingNum; }
            set { _turnCreateLivingNum = value; }
        }

        public int SamePingMaxRemoteNum
        {
            get { return _samePingMaxRemoteNum; }
            set { _samePingMaxRemoteNum = value; }
        }
        public int SamePingMaxLivingNum
        {
            get { return _samePingMaxLivingNum; }
            set { _samePingMaxLivingNum = value; }
        }
        /// <summary>
        /// 设置创建NPC参数
        /// </summary>
        /// <param name="remoteCount">需创建远程NPC数量</param>
        /// <param name="livingCount">需创建近身NPC数量</param>
        /// <param name="turnCreateRemoteNum">每回合创建远程NPC数量</param>
        /// <param name="turnCreateLivingNum">每回合创建近身NPC数量</param>
        /// <param name="samePingMaxRemoteNum">同屏最大远程NPC数量</param>
        /// <param name="samePingMaxLivingNum">同屏最大近身NPC数量</param>
        public NpcCreateParam(int remoteCount, int livingCount, int turnCreateRemoteNum, int turnCreateLivingNum, int samePingMaxRemoteNum, int samePingMaxLivingNum)
        {
            RemoteCount = remoteCount;
            LivingCount = livingCount;

            TurnCreateRemoteNum = turnCreateRemoteNum;
            TurnCreateLivingNum = turnCreateLivingNum;

            SamePingMaxRemoteNum = samePingMaxRemoteNum;
            SamePingMaxLivingNum = samePingMaxLivingNum;
        }
    }
    public class ExplorationMission : AMissionControl
    {
        public Dictionary<int, int> ballIds;

        public int[] remoteIds;
        public int[] livingIds;

        private int remoteCount = 0;
        private int livingCount = 0;

        private int currentTotalLivings = 0;    // 当前近身NPC数量
        private int currentTotalRemotIds = 0;   // 当前远程NPC数量

        private int turnCreateRemoteNum = 1;
        private int turnCreateLivingNum = 4;

        private int samePingMaxRemoteNum = 0;
        private int samePingMaxLivingNum = 0;

        public NpcCreateParam npcCreateParamSimple = null;
        public NpcCreateParam npcCreateParamNormal = null;
        public NpcCreateParam npcCreateParamHard = null;
        public NpcCreateParam npcCreateParamTerror = null;

        public List<Living> livings = new List<Living>();
        public List<SimpleBoss> remoteNpc = new List<SimpleBoss>();

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            switch (Game.HandLevel)
            {
                case eHardLevel.Simple:
                    remoteCount = npcCreateParamSimple.RemoteCount;
                    livingCount = npcCreateParamSimple.LivingCount;
                    turnCreateRemoteNum = npcCreateParamSimple.TurnCreateRemoteNum;
                    turnCreateLivingNum = npcCreateParamSimple.TurnCreateLivingNum;
                    samePingMaxRemoteNum = npcCreateParamSimple.SamePingMaxRemoteNum;
                    samePingMaxLivingNum = npcCreateParamSimple.SamePingMaxLivingNum;
                    break;
                case eHardLevel.Normal:
                    remoteCount = npcCreateParamNormal.RemoteCount;
                    livingCount = npcCreateParamNormal.LivingCount;
                    turnCreateRemoteNum = npcCreateParamNormal.TurnCreateRemoteNum;
                    turnCreateLivingNum = npcCreateParamNormal.TurnCreateLivingNum;
                    samePingMaxRemoteNum = npcCreateParamNormal.SamePingMaxRemoteNum;
                    samePingMaxLivingNum = npcCreateParamNormal.SamePingMaxLivingNum;
                    break;
                case eHardLevel.Hard:
                    remoteCount = npcCreateParamHard.RemoteCount;
                    livingCount = npcCreateParamHard.LivingCount;
                    turnCreateRemoteNum = npcCreateParamHard.TurnCreateRemoteNum;
                    turnCreateLivingNum = npcCreateParamHard.TurnCreateLivingNum;
                    samePingMaxRemoteNum = npcCreateParamHard.SamePingMaxRemoteNum;
                    samePingMaxLivingNum = npcCreateParamHard.SamePingMaxLivingNum;
                    break;
                case eHardLevel.Terror:
                    remoteCount = npcCreateParamTerror.RemoteCount;
                    livingCount = npcCreateParamTerror.LivingCount;
                    turnCreateRemoteNum = npcCreateParamTerror.TurnCreateRemoteNum;
                    turnCreateLivingNum = npcCreateParamTerror.TurnCreateLivingNum;
                    samePingMaxRemoteNum = npcCreateParamTerror.SamePingMaxRemoteNum;
                    samePingMaxLivingNum = npcCreateParamTerror.SamePingMaxLivingNum;
                    break;
                default:
                    remoteCount = npcCreateParamSimple.RemoteCount;
                    livingCount = npcCreateParamSimple.LivingCount;
                    turnCreateRemoteNum = npcCreateParamSimple.TurnCreateRemoteNum;
                    turnCreateLivingNum = npcCreateParamSimple.TurnCreateLivingNum;
                    samePingMaxRemoteNum = npcCreateParamSimple.SamePingMaxRemoteNum;
                    samePingMaxLivingNum = npcCreateParamSimple.SamePingMaxLivingNum;
                    break;
            }
        }

        public override int CalculateScoreGrade(int score)
        {
            if (score > 930)
            {
                return 3;
            }
            if (score > 850)
            {
                return 2;
            }
            if (score > 775)
            {
                return 1;
            }
            return 0;
        }

        public override void OnStartGame()
        {
            CreateLiving();
            CreateRemote();
            Game.TotalCount = livingCount + remoteCount;
            Game.SendMissionInfo();

        }

        public override void OnPrepareNewGame()
        {
            base.OnPrepareNewGame();
        }

        public override void OnNewTurnStarted()
        {
            int totalNpcCount = remoteCount + livingCount;
            int currentNpcCount = currentTotalRemotIds + currentTotalLivings;
            if (Game.TurnIndex < 2 || currentNpcCount >= totalNpcCount)
            {
                return;
            }
            CreateLiving();
            CreateRemote();
        }

        public override void OnBeginNewTurn()
        {

        }

        public override bool CanGameOver()
        {
            bool over = true;
            foreach (SimpleBoss remoteId in remoteNpc)
            {
                if (remoteId.IsLiving)
                {
                    over = false;
                    break;
                }
            }
            foreach (Living livingId in livings)
            {
                if (livingId.IsLiving)
                {
                    over = false;
                    break;
                }
            }
            if (over)
            {
                Game.IsWin = true;
            }
            return over;
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

        }

        private Point GetNpcBornPos()
        {
            List<Point> points = Game.MapPos.PosX1;
            int rand = Game.Random.Next(points.Count);
            Point point = points[rand];
            return point;
        }

        private int GetRandomNpcId(int[] list)
        {
            int index = Game.Random.Next(0, list.Length);
            return list[index];
        }

        private int GetLivedRemoteCount()
        {
            int i = 0;
            foreach (TurnedLiving tl in Game.TurnQueue)
            {
                if (tl is SimpleBoss && tl.IsLiving)
                    i++;
            }
            return i;
        }

        public int GetMapId(int[] mapIds, int defaultMapId)
        {
            for (int i = 0; i < 100; i++)
            {
                int index = Game.Random.Next(0, mapIds.Length);
                int mapId = mapIds[index];
                if (!Game.MapHistoryIds.Contains(mapId))
                {
                    Game.MapHistoryIds.Add(mapId);
                    return mapId;
                }
            }
            return defaultMapId;
        }

        private void CreateRemote()
        {
            if (remoteIds.Length == 0)
            {
                return;
            }
            int livedRemotes = GetLivedRemoteCount();
            if (remoteCount <= 0 || livedRemotes >= samePingMaxRemoteNum)
            {
                return;
            }
            if (currentTotalRemotIds >= remoteCount)
            {
                return;
            }
            for (int i = 0; i < turnCreateRemoteNum; i++)
            {
                if (currentTotalRemotIds >= remoteCount)
                {
                    return;
                }
                if (livedRemotes >= samePingMaxRemoteNum)
                {
                    return;
                }
                Point point = GetNpcBornPos();
                int remoteId = GetRandomNpcId(remoteIds);
                remoteNpc.Add(Game.CreateBoss(remoteId, point.X, point.Y, -1, 0));
                remoteNpc[currentTotalRemotIds].NpcInfo.CurrentBallId = ballIds[remoteId];
                currentTotalRemotIds++;
                livedRemotes = GetLivedRemoteCount();
            }
        }

        private void CreateLiving()
        {
            if (livingIds.Length == 0)
            {
                return;
            }
            int livedLivings = Game.GetLivedLivings().Count;
            if (livingCount <= 0 || livedLivings >= samePingMaxLivingNum)
            {
                return;
            }
            if (currentTotalLivings >= livingCount)
            {
                return;
            }
            for (int i = 0; i < turnCreateLivingNum; i++)
            {
                if (currentTotalLivings >= livingCount)
                {
                    return;
                }
                if (livedLivings >= samePingMaxLivingNum)
                {
                    return;
                }
                Point point = GetNpcBornPos();
                int livingId = GetRandomNpcId(livingIds);
                livings.Add(Game.CreateNpc(livingId, point.X, point.Y, 0));
                currentTotalLivings++;
                livedLivings = Game.GetLivedLivings().Count;
            }
        }
    }
}
