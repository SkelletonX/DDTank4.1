using Game.Logic;
using Game.Logic.Phy.Maps;
using Game.Server.RingStation;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.Games
{
    public class GameMgr
    {
        private static readonly ILog ilog_0;

        public static readonly long THREAD_INTERVAL;

        private static List<BaseGame> list_0;

        private static Thread thread_0;

        private static bool bool_0;

        private static int int_0;

        private static int int_1;

        private static int int_2;

        private static readonly int int_3;

        private static long long_0;

        public static int BoxBroadcastLevel => int_1;

        public static bool Setup(int serverId, int boxBroadcastLevel)
        {
            thread_0 = new Thread(smethod_0);
            list_0 = new List<BaseGame>();
            int_0 = serverId;
            int_1 = boxBroadcastLevel;
            int_2 = 0;
            return true;
        }

        public static bool Start()
        {
            if (!bool_0)
            {
                bool_0 = true;
                thread_0.Start();
            }
            return true;
        }

        public static void Stop()
        {
            if (bool_0)
            {
                bool_0 = false;
                thread_0.Join();
            }
        }

        private static void smethod_0()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            long num = 0L;
            long_0 = TickHelper.GetTickCount();
            while (bool_0)
            {
                long tickCount = TickHelper.GetTickCount();
                int num2 = 0;
                try
                {
                    num2 = smethod_2(tickCount);
                    smethod_3(tickCount);
                    if (long_0 <= tickCount)
                    {
                        long_0 += int_3;
                        ArrayList arrayList = new ArrayList();
                        foreach (BaseGame item2 in list_0)
                        {
                            if (item2.GameState == eGameState.Stopped)
                            {
                                arrayList.Add(item2);
                            }
                        }
                        foreach (BaseGame item3 in arrayList)
                        {
                            list_0.Remove(item3);
                        }
                        ThreadPool.QueueUserWorkItem(smethod_1, arrayList);
                    }
                }
                catch (Exception ex)
                {
                    ilog_0.Error((object)"Game Mgr Thread Error:", ex);
                }
                long tickCount2 = TickHelper.GetTickCount();
                num += THREAD_INTERVAL - (tickCount2 - tickCount);
                if (tickCount2 - tickCount > THREAD_INTERVAL * 2L)
                {
                    ilog_0.WarnFormat("Game Mgr spent too much times: {0} ms, count:{1}", (object)(tickCount2 - tickCount), (object)num2);
                }
                if (num > 0L)
                {
                    Thread.Sleep((int)num);
                    num = 0L;
                }
                else if (num < -1000L)
                {
                    num += 1000L;
                }
            }
        }

        private static void smethod_1(object object_0)
        {
            foreach (BaseGame item in object_0 as ArrayList)
            {
                try
                {
                    item.Dispose();
                }
                catch (Exception ex)
                {
                    ilog_0.Error((object)"game dispose error:", ex);
                }
            }
        }

        private static int smethod_2(long long_1)
        {
            IList allGame = GetAllGame();
            if (allGame != null)
            {
                foreach (BaseGame item in allGame)
                {
                    try
                    {
                        item.Update(long_1);
                    }
                    catch (Exception ex)
                    {
                        ilog_0.Error((object)"Game  updated error:", ex);
                    }
                }
                return allGame.Count;
            }
            return 0;
        }

        private static int smethod_3(long long_1)
        {
            IList allPlayer = RingStationMgr.GetAllPlayer();
            if (allPlayer != null)
            {
                foreach (RingStationGamePlayer item in allPlayer)
                {
                    try
                    {
                        item.Update(long_1);
                    }
                    catch (Exception ex)
                    {
                        ilog_0.Error((object)"Auto Bot updated error:", ex);
                    }
                }
                return allPlayer.Count;
            }
            return 0;
        }

        public static List<BaseGame> GetAllGame()
        {
            List<BaseGame> list = new List<BaseGame>();
            lock (list_0)
            {
                list.AddRange(list_0);
                return list;
            }
        }

        public static BaseGame StartPVPGame(int roomId, List<IGamePlayer> red, List<IGamePlayer> blue, int mapIndex, eRoomType roomType, eGameType gameType, int timeType)
        {
            try
            {
                Map map = MapMgr.CloneMap(MapMgr.GetMapIndex(mapIndex, (byte)roomType, int_0));
                if (map != null)
                {
                    PVPGame pVPGame = new PVPGame(int_2++, roomId, red, blue, map, roomType, gameType, timeType);
                    lock (list_0)
                    {
                        list_0.Add(pVPGame);
                    }
                    pVPGame.Prepare();
                    return pVPGame;
                }
                return null;
            }
            catch (Exception ex)
            {
                ilog_0.Error((object)"Create game error:", ex);
                return null;
            }
        }

        public static BaseGame StartPVEGame(int roomId, List<IGamePlayer> players, int copyId, eRoomType roomType, eGameType gameType, int timeType, eHardLevel hardLevel, int levelLimits, int currentFloor)
        {
            try
            {
                PveInfo pveInfo = null;
                pveInfo = ((copyId == 0 || copyId == 100000) ? PveInfoMgr.GetPveInfoByType(roomType, levelLimits) : PveInfoMgr.GetPveInfoById(copyId));
                if (pveInfo != null)
                {
                    PVEGame pVEGame = new PVEGame(int_2++, roomId, pveInfo, players, null, roomType, gameType, timeType, hardLevel, currentFloor);
                    lock (list_0)
                    {
                        list_0.Add(pVEGame);
                    }
                    pVEGame.Prepare();
                    return pVEGame;
                }
                return null;
            }
            catch (Exception ex)
            {
                ilog_0.Error((object)"Create game error:", ex);
                return null;
            }
        }

        public GameMgr()
        {


        }

        static GameMgr()
        {

            ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            THREAD_INTERVAL = 40L;
            int_3 = 5000;
        }
    }
}
