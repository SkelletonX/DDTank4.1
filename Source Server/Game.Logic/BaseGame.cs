// Decompiled with JetBrains decompiler
// Type: Game.Logic.BaseGame
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic.Actions;
using Game.Logic.Phy.Maps;
using Game.Logic.Phy.Object;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace Game.Logic
{
    public class BaseGame : AbstractGame
    {
        public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public bool CanEnd = false;
        public int blueScore;
        public string BossWarField;
        public int[] Cards;
        public int ConsortiaAlly;

        public List<Player> GetAllLivingPlayerOnRange(int minX, int maxX)
        {
            List<Player> list = new List<Player>();
            foreach (Player allLivingPlayer in GetAllLivingPlayers())
            {
                if (allLivingPlayer.X >= minX && allLivingPlayer.X <= maxX)
                {
                    list.Add(allLivingPlayer);
                }
            }
            return list;
        }

        public int CurrentActionCount;
        public int CurrentTurnTotalDamage;
        public TurnedLiving LastTurnLiving;
        public readonly int[] EquipPlace;
        private long long_1;
        private ArrayList m_actions;
        protected TurnedLiving m_currentLiving;
        protected eGameState m_gameState;
        private bool m_GetBlood;
        private int m_lifeTime;
        protected List<Living> m_livings;
        private List<LoadingFileInfo> m_loadingFiles;
        public Map m_map;
        protected int m_nextPlayerId;
        protected int m_nextWind;
        public bool m_confineWind;
        private long m_passTick;
        protected Dictionary<int, Player> m_players;
        protected Random m_random;
        private int m_roomId;
        private List<Ball> m_tempBall;
        private List<Box> m_tempBox;
        private List<Point> m_tempPoints;
        private List<Point> m_tempGhostPoints;
        private List<TurnedLiving> m_turnQueue;
        private long m_waitTimer;
        public int PhysicalId;
        public int redScore;
        public int RichesRate;
        public int TotalCostGold;
        public int TotalCostMoney;
        public int TotalHurt;
        protected int turnIndex;
        protected List<Living> m_decklivings;

        public event GameEventHandle BeginNewTurn;

        public event BaseGame.GameNpcDieEventHandle GameNpcDie;

        public event BaseGame.GameOverLogEventHandle GameOverLog;

        public event GameEventHandle GameOverred;

        public BaseGame(
          int id,
          int roomId,
          Map map,
          eRoomType roomType,
          eGameType gameType,
          int timeType)
          : base(id, roomType, gameType, timeType)
        {
            this.m_loadingFiles = new List<LoadingFileInfo>();
            this.long_1 = 0L;
            this.m_roomId = roomId;
            this.m_players = new Dictionary<int, Player>();
            this.m_turnQueue = new List<TurnedLiving>();
            this.m_livings = new List<Living>();
            this.m_random = new Random();
            this.m_map = map;
            this.m_actions = new ArrayList();
            this.PhysicalId = 0;
            this.EquipPlace = new int[15]
            {
        1,
        2,
        3,
        4,
        5,
        6,
        11,
        13,
        14,
        15,
        16,
        17,
        18,
        19,
        20
            };
            this.m_confineWind = false;
            this.BossWarField = "";
            this.m_tempBox = new List<Box>();
            this.m_tempGhostPoints = new List<Point>();
            this.m_tempBall = new List<Ball>();
            this.m_tempPoints = new List<Point>();
            this.m_decklivings = new List<Living>();
            this.Cards = this.RoomType != eRoomType.Dungeon ? new int[9] : new int[21];
            this.m_gameState = eGameState.Inited;
        }

        public Player[] GetSameTeamPlayer(Player player)
        {
            List<Player> playerList = new List<Player>();
            foreach (Player allFightPlayer in this.GetAllFightPlayers())
            {
                if (allFightPlayer != player && allFightPlayer.Team == player.Team)
                    playerList.Add(allFightPlayer);
            }
            return playerList.ToArray();
        }

        public bool CoupleFight(Player player)
        {
            foreach (Player player1 in this.GetSameTeamPlayer(player))
            {
                if (player1.PlayerDetail.PlayerCharacter.SpouseID == player.PlayerDetail.PlayerCharacter.ID)
                    return true;
            }
            return false;
        }

        public void AddAction(IAction action)
        {
            lock (this.m_actions)
                this.m_actions.Add((object)action);
        }

        public void AddAction(ArrayList actions)
        {
            lock (this.m_actions)
                this.m_actions.AddRange((ICollection)actions);
        }

        public bool ConFineWind
        {
            get
            {
                return this.m_confineWind;
            }
            set
            {
                this.m_confineWind = value;
            }
        }

        public virtual void AddLiving(Living living)
        {
            this.m_map.AddPhysical((Physics)living);
            if (living is Player && (living as Player).Weapon == null)
                return;
            if (living is TurnedLiving)
                this.m_turnQueue.Add(living as TurnedLiving);
            else
                this.m_livings.Add(living);
            this.SendAddLiving(living);
        }

        public virtual void AddGhostBoxObj(PhysicalObj phy)
        {
            this.m_map.AddPhysical((Physics)phy);
            phy.SetGame(this);
        }

        public void AddLoadingFile(int type, string file, string className)
        {
            if (file == null || className == null)
                return;
            this.m_loadingFiles.Add(new LoadingFileInfo(type, file, className));
        }

        public virtual void AddNormalBoss(Living living)
        {
            this.m_map.AddPhysical((Physics)living);
            if (living is Player && (living as Player).Weapon == null)
                return;
            this.m_livings.Add(living);
            this.SendAddLiving(living);
        }

        protected void AddPlayer(IGamePlayer gp, Player fp)
        {
            lock (this.m_players)
            {
                this.m_players.Add(fp.Id, fp);
                if (fp.Weapon == null)
                    return;
                this.m_turnQueue.Add((TurnedLiving)fp);
            }
        }

        public virtual void AddPhysicalObj(PhysicalObj phy, bool sendToClient)
        {
            this.m_map.AddPhysical((Physics)phy);
            phy.SetGame(this);
            if (!sendToClient)
                return;
            this.SendAddPhysicalObj(phy);
        }

        public virtual void AddPhysicalTip(PhysicalObj phy, bool sendToClient)
        {
            this.m_map.AddPhysical((Physics)phy);
            phy.SetGame(this);
            if (!sendToClient)
                return;
            this.SendAddPhysicalTip(phy);
        }

        public void AddTempPoint(int x, int y)
        {
            this.m_tempPoints.Add(new Point(x, y));
        }

        public void AfterUseItem(SqlDataProvider.Data.ItemInfo item)
        {
        }

        internal void capnhattrangthai(Living player, string loai1, string loai2)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id);
            pkg.WriteByte((byte)41);
            pkg.WriteString(loai1);
            pkg.WriteString(loai2);
            this.SendToAll(pkg);
        }

        public void ClearAllChild()
        {
            List<Living> livingList = new List<Living>();
            foreach (Living living in this.m_livings)
            {
                if (living.IsLiving && living is SimpleNpc)
                    livingList.Add(living);
            }
            foreach (Living living in livingList)
            {
                this.m_livings.Remove(living);
                living.Dispose();
                this.RemoveLiving(living.Id);
            }
        }

        public void ClearAllNpc()
        {
            List<Living> livingList = new List<Living>();
            foreach (Living living in this.m_livings)
            {
                if (living is SimpleNpc)
                    livingList.Add(living);
            }
            foreach (Living living in livingList)
            {
                this.m_livings.Remove(living);
                living.Dispose();
                this.SendRemoveLiving(living.Id);
            }
            foreach (Physics phy in this.m_map.GetAllPhysicalSafe())
            {
                if (phy is SimpleNpc)
                    this.m_map.RemovePhysical(phy);
            }
        }

        public void ClearBall()
        {
            List<Ball> ballList = new List<Ball>();
            foreach (Ball ball in this.m_tempBall)
                ballList.Add(ball);
            foreach (Ball ball in ballList)
            {
                this.m_tempBall.Remove(ball);
                this.RemovePhysicalObj((PhysicalObj)ball, true);
            }
        }

        public void ClearDiedPhysicals()
        {
            List<Living> livingList1 = new List<Living>();
            foreach (Living living in this.m_livings)
            {
                if (!living.IsLiving)
                    livingList1.Add(living);
            }
            foreach (Living living in livingList1)
            {
                this.m_livings.Remove(living);
                living.Dispose();
            }
            List<Living> livingList2 = new List<Living>();
            foreach (TurnedLiving turn in this.m_turnQueue)
            {
                if (!turn.IsLiving)
                    livingList2.Add((Living)turn);
            }
            foreach (TurnedLiving turnedLiving in livingList2)
                this.m_turnQueue.Remove(turnedLiving);
            foreach (Physics phy in this.m_map.GetAllPhysicalSafe())
            {
                if (!phy.IsLiving && !(phy is Player))
                    this.m_map.RemovePhysical(phy);
            }
        }

        public void ClearLoadingFiles()
        {
            this.m_loadingFiles.Clear();
        }

        public void ClearWaitTimer()
        {
            this.m_waitTimer = 0L;
        }

        public Box AddBox(SqlDataProvider.Data.ItemInfo item, Point pos, bool sendToClient)
        {
            Box box = new Box(this.PhysicalId++, "1", item, 1);
            box.SetXY(pos);
            this.AddPhysicalObj((PhysicalObj)box, sendToClient);
            return this.AddBox(box, sendToClient);
        }

        public Box AddBox(Box box, bool sendToClient)
        {
            this.m_tempBox.Add(box);
            this.AddPhysicalObj((PhysicalObj)box, sendToClient);
            return box;
        }

        public void AddTempGhostPoint(int x, int y)
        {
            this.m_tempGhostPoints.Add(new Point(x, y));
        }

        public Box AddGhostBox(Point pos, int type)
        {
            Box box = new Box(this.PhysicalId++, "1", (SqlDataProvider.Data.ItemInfo)null, type);
            box.SetXY(pos);
            return this.AddGhostBox(box);
        }

        public Box AddGhostBox(Box box)
        {
            this.m_tempBox.Add(box);
            this.AddGhostBoxObj((PhysicalObj)box);
            return box;
        }

        public int CheckGhostBox()
        {
            List<Box> source = new List<Box>();
            foreach (Box box in this.m_tempBox)
            {
                if (box.Type > 1)
                    source.Add(box);
            }
            return source.Count<Box>();
        }

        public void CheckBox()
        {
            List<Box> boxList = new List<Box>();
            foreach (Box box in this.m_tempBox)
            {
                if (!box.IsLiving)
                    boxList.Add(box);
            }
            foreach (Box box in boxList)
            {
                this.m_tempBox.Remove(box);
                this.RemovePhysicalObj((PhysicalObj)box, true);
            }
        }

        public List<Point> DrawCirclePoints(int points, int dis, double radius, Point center)
        {
            List<Point> pointList = new List<Point>();
            double num1 = 2.0 * Math.PI / (double)points;
            for (double num2 = radius; num2 > (double)dis; num2 -= (double)dis)
            {
                for (int index = 0; index < points; ++index)
                {
                    double num3 = num1 * (double)index;
                    Point point = new Point((int)((double)center.X + num2 * Math.Cos(num3)), (int)((double)center.Y + num2 * Math.Sin(num3)));
                    pointList.Add(point);
                }
            }
            return pointList;
        }

        public void CreateGhostPoints()
        {
            int backroundHeight = this.m_map.Info.BackroundHeight;
            Point center = new Point(this.m_map.Info.BackroundWidht / 2, backroundHeight / 2);
            this.m_tempGhostPoints = this.DrawCirclePoints(backroundHeight, 30, (double)(backroundHeight - 180), center);
        }

        public List<Box> CreateBox()
        {
            int num1 = this.m_players.Count + 2;
            int num2 = 0;
            List<SqlDataProvider.Data.ItemInfo> info = (List<SqlDataProvider.Data.ItemInfo>)null;
            if (this.CurrentTurnTotalDamage > 0)
            {
                num2 = this.m_random.Next(1, 3);
                if (this.m_tempBox.Count + num2 > num1)
                    num2 = num1 - this.m_tempBox.Count;
                if (num2 > 0)
                    DropInventory.BoxDrop(this.m_roomType, ref info);
            }
            int diedPlayerCount = this.GetDiedPlayerCount();
            int num3 = 0;
            List<Box> boxList = new List<Box>();
            if (diedPlayerCount > 0)
            {
                num3 = this.m_random.Next(diedPlayerCount);
                if (this.m_tempGhostPoints.Count < num1)
                    this.CreateGhostPoints();
                for (int index1 = 0; index1 < this.m_tempGhostPoints.Count; ++index1)
                {
                    int index2 = this.m_random.Next(this.m_tempGhostPoints.Count);
                    Point tempGhostPoint = this.m_tempGhostPoints[index2];
                    this.m_tempGhostPoints[index2] = this.m_tempGhostPoints[index1];
                    this.m_tempGhostPoints[index1] = tempGhostPoint;
                }
                int num4 = diedPlayerCount + num1 - this.CheckGhostBox();
                if (this.m_tempGhostPoints.Count > num4)
                {
                    int[] numArray = new int[2] { 2, 3 };
                    for (int index1 = 0; index1 < num4; ++index1)
                    {
                        int index2 = this.m_random.Next(numArray.Length);
                        int index3 = this.m_random.Next(this.m_tempGhostPoints.Count);
                        boxList.Add(this.AddGhostBox(this.m_tempGhostPoints[index3], numArray[index2]));
                    }
                }
            }
            if (this.m_tempBox.Count + num2 + num3 > num1)
            {
                int count = this.m_tempBox.Count;
            }
            if (info != null)
            {
                for (int index1 = 0; index1 < this.m_tempPoints.Count; ++index1)
                {
                    int index2 = this.m_random.Next(this.m_tempPoints.Count);
                    Point tempPoint = this.m_tempPoints[index2];
                    this.m_tempPoints[index2] = this.m_tempPoints[index1];
                    this.m_tempPoints[index1] = tempPoint;
                }
                int num4 = Math.Min(info.Count, this.m_tempPoints.Count);
                for (int index = 0; index < num4; ++index)
                    boxList.Add(this.AddBox(info[index], this.m_tempPoints[index], false));
            }
            this.m_tempPoints.Clear();
            this.m_tempGhostPoints.Clear();
            return boxList;
        }

        public virtual void CheckState(int delay)
        {
            this.AddAction((IAction)new CheckPVPGameStateAction(delay));
        }

        public SimpleBoss[] FindAllBoss()
        {
            List<SimpleBoss> simpleBossList = new List<SimpleBoss>();
            foreach (Living living in this.m_livings)
            {
                if (living is SimpleBoss)
                    simpleBossList.Add(living as SimpleBoss);
            }
            return simpleBossList.ToArray();
        }

        public List<SimpleBoss> FindAllBossTurned()
        {
            List<SimpleBoss> simpleBossList = new List<SimpleBoss>();
            foreach (TurnedLiving turn in this.m_turnQueue)
            {
                if (turn is SimpleBoss)
                    simpleBossList.Add(turn as SimpleBoss);
            }
            return simpleBossList;
        }

        public SimpleNpc[] FindAllNpc()
        {
            List<SimpleNpc> simpleNpcList = new List<SimpleNpc>();
            foreach (Living living in this.m_livings)
            {
                if (living is SimpleNpc)
                {
                    simpleNpcList.Add(living as SimpleNpc);
                    return simpleNpcList.ToArray();
                }
            }
            return (SimpleNpc[])null;
        }

        public SimpleNpc[] FindAllNpcLiving()
        {
            List<SimpleNpc> simpleNpcList = new List<SimpleNpc>();
            foreach (Living living in this.m_livings)
            {
                if (living is SimpleNpc && living.IsLiving)
                    simpleNpcList.Add(living as SimpleNpc);
            }
            return simpleNpcList.ToArray();
        }

        public List<SimpleBoss> FindAllTurnBoss()
        {
            List<SimpleBoss> simpleBossList = new List<SimpleBoss>();
            foreach (Living living in this.m_livings)
            {
                if (living is SimpleBoss)
                    simpleBossList.Add(living as SimpleBoss);
            }
            return simpleBossList;
        }

        public SimpleNpc[] FindNpcLiving(Point p)
        {
            List<SimpleNpc> simpleNpcList = new List<SimpleNpc>();
            foreach (Living living in this.m_livings)
            {
                if (living is SimpleNpc && living.IsLiving && living.X == p.X && living.Y == p.Y)
                    simpleNpcList.Add(living as SimpleNpc);
            }
            return simpleNpcList.ToArray();
        }

        public SimpleNpc[] GetNPCLivingWithID(int id)
        {
            List<SimpleNpc> simpleNpcList = new List<SimpleNpc>();
            foreach (Living living in this.m_livings)
            {
                if (living is SimpleNpc && living.IsLiving && (living as SimpleNpc).NpcInfo.ID == id)
                    simpleNpcList.Add(living as SimpleNpc);
            }
            return simpleNpcList.ToArray();
        }

        public List<Living> FindAllTurnBossLiving()
        {
            List<Living> livingList = new List<Living>();
            foreach (Living turn in this.m_turnQueue)
            {
                if (turn is SimpleBoss && turn.IsLiving)
                    livingList.Add(turn);
            }
            return livingList;
        }

        public Player FindFarPlayer(int x, int y)
        {
            lock (this.m_players)
            {
                double num1 = double.MinValue;
                Player player1 = (Player)null;
                foreach (Player player2 in this.m_players.Values)
                {
                    if (player2.IsLiving)
                    {
                        double num2 = player2.Distance(x, y);
                        if (num2 > num1)
                        {
                            num1 = num2;
                            player1 = player2;
                        }
                    }
                }
                return player1;
            }
        }

        public int GainCoupleGP(Player player, int gp)
        {
            foreach (Player player1 in this.GetSameTeamPlayer(player))
            {
                if (player1.PlayerDetail.PlayerCharacter.SpouseID == player.PlayerDetail.PlayerCharacter.SpouseID)
                    return (int)((double)gp * 1.5);
            }
            return gp;
        }

        public int FindlivingbyDir(Living npc)
        {
            int num1 = 0;
            int num2 = 0;
            foreach (Player player in this.m_players.Values)
            {
                if (player.IsLiving)
                {
                    if (player.X > npc.X)
                        ++num2;
                    else
                        ++num1;
                }
            }
            if (num2 > num1)
                return 1;
            if (num2 < num1)
                return -1;
            return -npc.Direction;
        }

        public SimpleBoss[] FindLivingTurnBossWithID(int id)
        {
            List<SimpleBoss> simpleBossList = new List<SimpleBoss>();
            foreach (Living turn in this.m_turnQueue)
            {
                if (turn is SimpleBoss && turn.IsLiving && (turn as SimpleBoss).NpcInfo.ID == id)
                    simpleBossList.Add(turn as SimpleBoss);
            }
            return simpleBossList.ToArray();
        }

        public Living FindNearestHelper(int x, int y)
        {
            double num1 = double.MaxValue;
            Living living = (Living)null;
            foreach (Living turn in this.m_turnQueue)
            {
                if (turn.IsLiving && (turn is Player || turn.Config.IsHelper))
                {
                    double num2 = turn.Distance(x, y);
                    if (num2 < num1)
                    {
                        num1 = num2;
                        living = turn;
                    }
                }
            }
            return living;
        }

        public Player FindNearestPlayer(int x, int y)
        {
            double num1 = double.MaxValue;
            Player player1 = (Player)null;
            foreach (Player player2 in this.m_players.Values)
            {
                if (player2.IsLiving)
                {
                    double num2 = player2.Distance(x, y);
                    if (num2 < num1)
                    {
                        num1 = num2;
                        player1 = player2;
                    }
                }
            }
            return player1;
        }

        public SimpleNpc FindHealthyHelper()
        {
            SimpleNpc simpleNpc = (SimpleNpc)null;
            foreach (SimpleNpc living in this.m_livings)
            {
                if (living.Config.IsHelper && !living.Config.CanHeal)
                    simpleNpc = living;
            }
            return simpleNpc;
        }

        public SimpleNpc FindNearestAdverseNpc(int x, int y, int camp)
        {
            double num1 = double.MaxValue;
            SimpleNpc simpleNpc = (SimpleNpc)null;
            foreach (SimpleNpc living in this.m_livings)
            {
                if (living.IsLiving && living.NpcInfo.Camp != camp)
                {
                    double num2 = living.Distance(x, y);
                    if (num2 < num1)
                    {
                        num1 = num2;
                        simpleNpc = living;
                    }
                }
            }
            foreach (SimpleNpc deckliving in this.m_decklivings)
            {
                if (deckliving.IsLiving && deckliving.NpcInfo.Camp != camp)
                {
                    double num2 = deckliving.Distance(x, y);
                    if (num2 < num1)
                    {
                        num1 = num2;
                        simpleNpc = deckliving;
                    }
                }
            }
            return simpleNpc;
        }

        public TurnedLiving FindNextTurnedLiving()
        {
            if (this.m_turnQueue.Count == 0)
                return (TurnedLiving)null;
            TurnedLiving turn = this.m_turnQueue[this.m_random.Next(this.m_turnQueue.Count - 1)];
            int delay = turn.Delay;
            for (int index = 0; index < this.m_turnQueue.Count; ++index)
            {
                if (this.m_turnQueue[index].Delay < delay && this.m_turnQueue[index].IsLiving)
                {
                    delay = this.m_turnQueue[index].Delay;
                    turn = this.m_turnQueue[index];
                }
            }
            ++turn.TurnNum;
            return turn;
        }

        public Player FindPlayer(int id)
        {
            lock (this.m_players)
            {
                if (this.m_players.ContainsKey(id))
                    return this.m_players[id];
            }
            return (Player)null;
        }

        public PhysicalObj[] FindPhysicalObjByName(string name)
        {
            List<PhysicalObj> physicalObjList = new List<PhysicalObj>();
            foreach (PhysicalObj physicalObj in this.m_map.GetAllPhysicalObjSafe())
            {
                if (physicalObj.Name == name)
                    physicalObjList.Add(physicalObj);
            }
            return physicalObjList.ToArray();
        }

        public PhysicalObj[] FindPhysicalObjByName(string name, bool CanPenetrate)
        {
            List<PhysicalObj> physicalObjList = new List<PhysicalObj>();
            foreach (PhysicalObj physicalObj in this.m_map.GetAllPhysicalObjSafe())
            {
                if (physicalObj.Name == name && physicalObj.CanPenetrate == CanPenetrate)
                    physicalObjList.Add(physicalObj);
            }
            return physicalObjList.ToArray();
        }

        public Living FindRandomLiving()
        {
            List<Living> livingList = new List<Living>();
            Living living1 = (Living)null;
            foreach (Living living2 in this.m_livings)
            {
                if (living2.IsLiving)
                    livingList.Add(living2);
            }
            int index = this.Random.Next(0, livingList.Count);
            if (livingList.Count > 0)
                living1 = livingList[index];
            return living1;
        }

        public Player FindRandomPlayer()
        {
            List<Player> playerList = new List<Player>();
            Player player1 = (Player)null;
            foreach (Player player2 in this.m_players.Values)
            {
                if (player2.IsLiving)
                    playerList.Add(player2);
            }
            int index = this.Random.Next(0, playerList.Count);
            if (playerList.Count > 0)
                player1 = playerList[index];
            return player1;
        }

        public Player FindRandomPlayerNotLock()
        {
            List<Player> source = new List<Player>();
            foreach (Player player in this.m_players.Values)
            {
                if (player.IsLiving && player.State != 9)
                    source.Add(player);
            }
            if (source.Count <= 0)
                return (Player)null;
            int index = this.Random.Next(0, source.Count);
            return source.ElementAt<Player>(index);
        }

        public List<Player> GetAllEnemyPlayers(Living living)
        {
            List<Player> playerList = new List<Player>();
            lock (this.m_players)
            {
                foreach (Player player in this.m_players.Values)
                {
                    if (player.Team != living.Team)
                        playerList.Add(player);
                }
            }
            return playerList;
        }

        public List<Player> GetAllFightPlayers()
        {
            List<Player> playerList = new List<Player>();
            lock (this.m_players)
                playerList.AddRange((IEnumerable<Player>)this.m_players.Values);
            return playerList;
        }

        public Player[] FindRandomPlayer(int max)
        {
            List<Player> playerList1 = new List<Player>();
            if (this.m_players.Count > 0)
            {
                List<Player> playerList2 = new List<Player>();
                foreach (Player player in this.m_players.Values)
                {
                    if (player.IsLiving)
                        playerList2.Add(player);
                }
                for (int index1 = 0; index1 < max; ++index1)
                {
                    int index2 = this.Random.Next(0, playerList2.Count);
                    playerList1.Add(playerList2[index2]);
                    playerList2.RemoveAt(index2);
                    if (playerList2.Count <= 0)
                        break;
                }
            }
            return playerList1.ToArray();
        }

        public int GetLowDelayTurn()
        {
            List<Living> livingList = new List<Living>();
            int num = int.MaxValue;
            foreach (TurnedLiving turn in this.m_turnQueue)
            {
                if (turn != null && turn.Delay < num)
                    num = turn.Delay;
            }
            return num;
        }

        public List<Player> FindRangePlayers(int minX, int maxX)
        {
            lock (this.m_players)
            {
                List<Player> playerList = new List<Player>();
                foreach (Player player in this.m_players.Values)
                {
                    if (player.IsLiving && player.X >= minX && player.X <= maxX)
                        playerList.Add(player);
                }
                return playerList;
            }
        }

        public Player FindPlayerWithId(int id)
        {
            lock (this.m_players)
            {
                if (this.m_players.Count > 0)
                {
                    foreach (Player player in this.m_players.Values)
                    {
                        if (player.IsLiving && player.Id == id)
                            return player;
                    }
                }
                return (Player)null;
            }
        }

        public string ListPlayersName()
        {
            List<string> stringList = new List<string>();
            foreach (Player allLivingPlayer in this.GetAllLivingPlayers())
                stringList.Add(allLivingPlayer.PlayerDetail.PlayerCharacter.NickName);
            return string.Join(",", (IEnumerable<string>)stringList);
        }

        public List<Player> GetAllLivingPlayers()
        {
            List<Player> playerList = new List<Player>();
            lock (this.m_players)
            {
                foreach (Player player in this.m_players.Values)
                {
                    if (player.IsLiving)
                        playerList.Add(player);
                }
            }
            return playerList;
        }

        public Player[] GetAllPlayers()
        {
            return this.GetAllFightPlayers().ToArray();
        }

        public List<Player> GetAllTeamPlayers(Living living)
        {
            List<Player> playerList = new List<Player>();
            lock (this.m_players)
            {
                foreach (Player player in this.m_players.Values)
                {
                    if (player.Team == living.Team)
                        playerList.Add(player);
                }
            }
            return playerList;
        }

        public List<Living> GetBossLivings()
        {
            List<Living> livingList = new List<Living>();
            foreach (Living living in this.m_livings)
            {
                if (living.IsLiving && living is SimpleBoss)
                    livingList.Add(living);
            }
            return livingList;
        }

        public List<Living> FindAppointDeGreeNpc(int degree)
        {
            List<Living> livingList = new List<Living>();
            foreach (Living living in this.m_livings)
            {
                if (living.IsLiving && living.Degree == degree)
                    livingList.Add(living);
            }
            foreach (Living deckliving in this.m_decklivings)
            {
                if (deckliving.IsLiving && deckliving.Degree == degree)
                    livingList.Add(deckliving);
            }
            foreach (Living turn in this.m_turnQueue)
            {
                if (turn.IsLiving && turn.Degree == degree)
                    livingList.Add(turn);
            }
            return livingList;
        }

        public int GetDiedBossCount()
        {
            int num = 0;
            foreach (Physics physics in this.FindAllBoss())
            {
                if (!physics.IsLiving)
                    ++num;
            }
            return num;
        }

        public int GetDiedCount()
        {
            return this.GetDiedNPCCount() + this.GetDiedBossCount();
        }

        public int GetDiedNPCCount()
        {
            int num = 0;
            foreach (Physics physics in this.FindAllNpc())
            {
                if (!physics.IsLiving)
                    ++num;
            }
            return num;
        }

        public int GetDiedPlayerCount()
        {
            int num = 0;
            foreach (Player player in this.m_players.Values)
            {
                if (player.IsActive && !player.IsLiving)
                    ++num;
            }
            return num;
        }

        public List<Living> GetFightFootballLivings()
        {
            List<Living> livingList = new List<Living>();
            foreach (Living living in this.m_livings)
            {
                if (living is SimpleNpc)
                    livingList.Add(living);
            }
            return livingList;
        }

        public Player GetFrostPlayerRadom()
        {
            List<Player> allFightPlayers = this.GetAllFightPlayers();
            List<Player> source = new List<Player>();
            foreach (Player player in allFightPlayers)
            {
                if (player.IsFrost)
                    source.Add(player);
            }
            if (source.Count <= 0)
                return (Player)null;
            int index = this.Random.Next(0, source.Count);
            return source.ElementAt<Player>(index);
        }

        public int GetHighDelayTurn()
        {
            List<Living> livingList = new List<Living>();
            int num = int.MinValue;
            foreach (TurnedLiving turn in this.m_turnQueue)
            {
                if (turn != null && turn.Delay > num)
                    num = turn.Delay;
            }
            return num;
        }

        public List<Living> GetLivedLivings()
        {
            List<Living> livingList = new List<Living>();
            foreach (Living living in this.m_livings)
            {
                if (living.IsLiving)
                    livingList.Add(living);
            }
            return livingList;
        }

        public List<Living> GetLivedLivingsHadTurn()
        {
            List<Living> livingList = new List<Living>();
            foreach (Living living in this.m_livings)
            {
                if (living.IsLiving && living is SimpleNpc && living.Config.IsTurn)
                    livingList.Add(living);
            }
            return livingList;
        }

        public List<Living> GetLivedNpcs(int npcId)
        {
            List<Living> livingList = new List<Living>();
            foreach (Living living in this.m_livings)
            {
                if (living.IsLiving && living is SimpleNpc && (living as SimpleNpc).NpcInfo.ID == npcId)
                    livingList.Add(living);
            }
            return livingList;
        }

        public float GetNextWind()
        {
            int num1 = (int)((double)this.Wind * 10.0);
            int num2;
            if (num1 > this.m_nextWind)
            {
                num2 = num1 - this.m_random.Next(11);
                if (num1 <= this.m_nextWind)
                    this.m_nextWind = this.m_random.Next(-40, 40);
            }
            else
            {
                num2 = num1 + this.m_random.Next(11);
                if (num1 >= this.m_nextWind)
                    this.m_nextWind = this.m_random.Next(-40, 40);
            }
            return (float)num2 / 10f;
        }

        public Player GetNoHolePlayerRandom()
        {
            List<Player> allFightPlayers = this.GetAllFightPlayers();
            List<Player> source = new List<Player>();
            foreach (Player player in allFightPlayers)
            {
                if (player.IsNoHole)
                    source.Add(player);
            }
            if (source.Count <= 0)
                return (Player)null;
            int index = this.Random.Next(0, source.Count);
            return source.ElementAt<Player>(index);
        }

        public Player GetPlayer(IGamePlayer gp)
        {
            Player player1 = (Player)null;
            lock (this.m_players)
            {
                foreach (Player player2 in this.m_players.Values)
                {
                    if (player2.PlayerDetail == gp)
                        return player2;
                }
                return player1;
            }
        }

        public Player GetPlayerByIndex(int index)
        {
            return this.m_players.ElementAt<KeyValuePair<int, Player>>(index).Value;
        }

        public int GetPlayerCount()
        {
            return this.GetAllFightPlayers().Count;
        }

        protected Point GetPlayerPoint(MapPoint mapPos, int team)
        {
            List<Point> pointList = team == 1 ? mapPos.PosX : mapPos.PosX1;
            int index = this.m_random.Next(pointList.Count);
            Point point = pointList[index];
            pointList.Remove(point);
            return point;
        }

        public bool GetSameTeam()
        {
            bool flag = false;
            Player[] allPlayers = this.GetAllPlayers();
            foreach (Living living in allPlayers)
            {
                if (living.Team != allPlayers[0].Team)
                    return false;
                flag = true;
            }
            return flag;
        }

        public int getTurnTime()
        {
            switch (this.m_timeType)
            {
                case 1:
                    return 8;
                case 2:
                    return 10;
                case 3:
                    return 12;
                case 4:
                    return 16;
                case 5:
                    return 21;
                case 6:
                    return 31;
                default:
                    return -1;
            }
        }

        public int GetTurnWaitTime(bool isNextTurn)
        {
            int num;
            switch (this.m_timeType)
            {
                case 1:
                    num = 5;
                    break;
                case 2:
                    num = 7;
                    break;
                case 3:
                    num = 10;
                    break;
                case 4:
                    num = 15;
                    break;
                case 5:
                    num = 20;
                    break;
                case 6:
                    num = 30;
                    break;
                default:
                    num = 7;
                    break;
            }
            return num;
        }

        public int GetTurnWaitTime()
        {
            return this.m_timeType;
        }

        public byte GetVane(int Wind, int param)
        {
            int wind = Math.Abs(Wind);
            if (param == 1)
                return WindMgr.GetWindID(wind, 1);
            if (param == 3)
                return WindMgr.GetWindID(wind, 3);
            return 0;
        }

        public long GetWaitTimer()
        {
            return this.m_waitTimer;
        }

        public int GetWaitTimerLeft()
        {
            if (this.long_1 <= 0L)
                return 0;
            long num = TickHelper.GetTickCount() > this.long_1 ? TickHelper.GetTickCount() - this.long_1 : this.long_1 - TickHelper.GetTickCount();
            if (num > 10000L)
                return 1000;
            return (int)num;
        }

        public bool IsAllComplete()
        {
            foreach (Player allFightPlayer in this.GetAllFightPlayers())
            {
                if (allFightPlayer.LoadingProcess < 100)
                    return false;
            }
            return true;
        }

        internal bool isTrainer()
        {
            return this.RoomType == eRoomType.Freshman;
        }

        internal void method_68(Living living_0, int int_4, bool bool_1)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.Parameter1 = living_0.Id;
            pkg.WriteByte((byte)128);
            pkg.WriteInt(int_4);
            pkg.WriteBoolean(bool_1);
            this.SendToAll(pkg);
        }

        internal void method_43(Living living_0, PetSkillElementInfo petSkillElementInfo_0, bool bool_1)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(91, living_0.Id)
            {
                Parameter1 = living_0.Id
            };
            gSPacketIn.WriteByte(145);
            gSPacketIn.WriteInt(petSkillElementInfo_0.ID);
            gSPacketIn.WriteString("");
            gSPacketIn.WriteString("");
            gSPacketIn.WriteString(petSkillElementInfo_0.Pic.ToString());
            gSPacketIn.WriteString(petSkillElementInfo_0.EffectPic);
            gSPacketIn.WriteBoolean(bool_1);
            this.SendToAll(gSPacketIn);
        }

        internal void method_7(Living living_0, PetSkillElementInfo petSkillElementInfo_0, bool bool_1)
        {
            this.method_9(living_0, petSkillElementInfo_0, bool_1, 3, null);
        }

        internal void method_8(Living living_0, PetSkillElementInfo petSkillElementInfo_0, bool bool_1, int int_4)
        {
            this.method_9(living_0, petSkillElementInfo_0, bool_1, int_4, null);
        }

        internal void method_9(Living living_0, PetSkillElementInfo petSkillElementInfo_0, bool bool_1, int int_4, IGamePlayer igamePlayer_0)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living_0.Id);
            pkg.Parameter1 = living_0.Id;
            pkg.WriteByte((byte)145);
            pkg.WriteInt(petSkillElementInfo_0.ID);
            pkg.WriteString(petSkillElementInfo_0.Name);
            pkg.WriteString(petSkillElementInfo_0.Description);
            pkg.WriteString(petSkillElementInfo_0.Pic.ToString());
            pkg.WriteString(petSkillElementInfo_0.EffectPic);
            pkg.WriteBoolean(bool_1);
            this.SendToAll(pkg);
        }

        internal void method_10(int int_3, int int_4, int int_5)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)62);
            pkg.WriteInt(int_5);
            pkg.WriteInt(int_3);
            pkg.WriteInt(int_4);
            this.SendToAll(pkg);
        }

        internal void method_11(Physics physics_0, int int_4)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)62);
            pkg.WriteInt(int_4);
            pkg.WriteInt(physics_0.X);
            pkg.WriteInt(physics_0.Y);
            this.SendToAll(pkg);
        }

        internal void method_12(PhysicalObj physicalObj_0)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)66);
            pkg.WriteInt(physicalObj_0.Id);
            pkg.WriteString(physicalObj_0.CurrentAction);
            this.SendToAll(pkg);
        }

        internal void method_14(int int_4)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)53);
            pkg.WriteInt(int_4);
            this.SendToAll(pkg);
        }

        internal void method_18(
          Living living_0,
          int int_4,
          int int_5,
          int int_6,
          int int_7,
          string string_0,
          string string_1,
          int int_8)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living_0.Id)
            {
                Parameter1 = living_0.Id
            };
            pkg.WriteByte((byte)55);
            pkg.WriteInt(int_4);
            pkg.WriteInt(int_5);
            pkg.WriteInt(int_6);
            pkg.WriteInt(int_7);
            pkg.WriteInt(int_8);
            pkg.WriteString(!string.IsNullOrEmpty(string_0) ? string_0 : "");
            pkg.WriteString(!string.IsNullOrEmpty(string_1) ? string_1 : "");
            this.SendToAll(pkg);
        }

        internal void method_24(Living living_0)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living_0.Id)
            {
                Parameter1 = living_0.Id
            };
            pkg.WriteByte((byte)72);
            pkg.WriteInt(living_0.X);
            pkg.WriteInt(living_0.Y);
            this.SendToAll(pkg);
        }

        internal void method_25(int int_4, string string_0, string string_1)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, int_4)
            {
                Parameter1 = int_4
            };
            pkg.WriteByte((byte)223);
            pkg.WriteInt(int_4);
            pkg.WriteString(string_0);
            pkg.WriteString(string_1);
            this.SendToAll(pkg);
        }

        internal void method_30(Living living_0)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living_0.Id)
            {
                Parameter1 = living_0.Id
            };
            pkg.WriteByte((byte)33);
            pkg.WriteBoolean(living_0.IsFrost);
            this.SendToAll(pkg);
        }

        internal void method_34(Living living_0, string string_0, string string_1)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living_0.Id)
            {
                Parameter1 = living_0.Id
            };
            pkg.WriteByte((byte)41);
            pkg.WriteString(string_0);
            pkg.WriteString(string_1);
            this.SendToAll(pkg);
        }

        internal void method_39(Player player_0, int int_4, int int_5, int int_6)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player_0.Id)
            {
                Parameter1 = player_0.Id
            };
            pkg.WriteByte((byte)32);
            pkg.WriteByte((byte)int_4);
            pkg.WriteInt(int_5);
            pkg.WriteInt(int_6);
            pkg.WriteInt(player_0.Id);
            pkg.WriteBoolean(false);
            this.SendToAll(pkg);
        }

        internal void SendLockFocus(bool IsLock)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)69);
            pkg.WriteBoolean(IsLock);
            this.SendToAll(pkg);
        }

        internal void method_47(Living living_0, int int_4, bool bool_0)
        {
            GSPacketIn pkg = new GSPacketIn((short)91)
            {
                Parameter1 = living_0.Id
            };
            pkg.WriteByte((byte)128);
            pkg.WriteInt(int_4);
            pkg.WriteBoolean(bool_0);
            this.SendToAll(pkg);
        }

        internal void SendLivingBoltMove(Living living)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living.Id);
            pkg.Parameter1 = living.Id;
            pkg.WriteByte((byte)72);
            pkg.WriteInt(living.X);
            pkg.WriteInt(living.Y);
            this.SendToAll(pkg);
        }

        public virtual void MinusDelays(int lowestDelay)
        {
            foreach (TurnedLiving turn in this.m_turnQueue)
                turn.Delay -= lowestDelay;
        }

        protected void OnBeginNewTurn()
        {
            if (this.BeginNewTurn == null)
                return;
            this.BeginNewTurn((AbstractGame)this);
        }

        public void OnGameNpcDie(int Id)
        {
            if (this.GameNpcDie == null)
                return;
            this.GameNpcDie(Id);
        }

        public void OnGameOverLog(
          int _roomId,
          eRoomType _roomType,
          eGameType _fightType,
          int _changeTeam,
          DateTime _playBegin,
          DateTime _playEnd,
          int _userCount,
          int _mapId,
          string _teamA,
          string _teamB,
          string _playResult,
          int _winTeam,
          string BossWar)
        {
            if (this.GameOverLog == null)
                return;
            this.GameOverLog(_roomId, _roomType, _fightType, _changeTeam, _playBegin, _playEnd, _userCount, _mapId, _teamA, _teamB, _playResult, _winTeam, this.BossWarField);
        }

        protected void OnGameOverred()
        {
            if (this.GameOverred == null)
                return;
            this.GameOverred((AbstractGame)this);
        }

        public override void Pause(int time)
        {
            this.m_passTick = Math.Max(this.m_passTick, TickHelper.GetTickCount() + (long)time);
        }

        internal void PedSuikAov(Living living_0, int int_4)
        {
            GSPacketIn pkg = new GSPacketIn((short)91)
            {
                Parameter1 = living_0.Id
            };
            pkg.WriteByte((byte)80);
            pkg.WriteInt(living_0.Id);
            pkg.WriteInt(int_4);
            this.SendToAll(pkg);
        }

        public override void ProcessData(GSPacketIn packet)
        {
            if (!this.m_players.ContainsKey(packet.Parameter1))
                return;
            this.AddAction((IAction)new ProcessPacketAction(this.m_players[packet.Parameter1], packet));
        }

        public Player FindPlayerWithUserId(int userid)
        {
            lock (this.m_players)
            {
                if (this.m_players.Count > 0)
                {
                    foreach (Player player in this.m_players.Values)
                    {
                        if (player.PlayerDetail.PlayerCharacter.ID == userid)
                            return player;
                    }
                }
                return (Player)null;
            }
        }

        public void RemoveLiving(int id)
        {
            foreach (Living living in this.m_livings)
            {
                if (living.Id == id)
                {
                    this.m_map.RemovePhysical((Physics)living);
                    if (living is TurnedLiving)
                        this.m_turnQueue.Remove(living as TurnedLiving);
                    else
                        this.m_livings.Remove(living);
                }
            }
            this.SendRemoveLiving(id);
        }

        public void RemoveLiving(Living living, bool sendToClient)
        {
            this.m_map.RemovePhysical((Physics)living);
            if (!sendToClient)
                return;
            this.method_14(living.Id);
        }

        public override Player RemovePlayer(IGamePlayer gp, bool IsKick)
        {
            Player player1 = (Player)null;
            lock (this.m_players)
            {
                foreach (Player player2 in this.m_players.Values)
                {
                    if (player2.PlayerDetail == gp)
                    {
                        player1 = player2;
                        this.m_players.Remove(player2.Id);
                        break;
                    }
                }
            }
            lock (this.m_turnQueue)
            {
                if (player1 != null)
                    this.m_turnQueue.Remove((TurnedLiving)player1);
            }
            if (player1 != null)
                this.AddAction((IAction)new RemovePlayerAction(player1));
            return player1;
        }

        public void RemovePhysicalObj(PhysicalObj phy, bool sendToClient)
        {
            this.m_map.RemovePhysical((Physics)phy);
            phy.SetGame((BaseGame)null);
            if (!sendToClient)
                return;
            this.SendRemovePhysicalObj(phy);
        }

        public override void Resume()
        {
            this.m_passTick = 0L;
        }

        public void SelectObject(int id, int zoneId)
        {
            lock (this.m_players);
        }

        internal void SendShowBloodItem(int livingId)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)73);
            pkg.WriteInt(livingId);
            this.SendToAll(pkg);
        }

        internal void SendAddLiving(Living living)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.Parameter1 = living.Id;
            pkg.WriteByte((byte)64);
            pkg.WriteByte((byte)living.Type);
            pkg.WriteInt(living.Id);
            pkg.WriteString(living.Name);
            pkg.WriteString(living.ModelId);
            pkg.WriteString(living.ActionStr);
            pkg.WriteInt(living.X);
            pkg.WriteInt(living.Y);
            pkg.WriteInt(living.Blood);
            pkg.WriteInt(living.MaxBlood);
            pkg.WriteInt(living.Team);
            pkg.WriteByte((byte)living.Direction);
            pkg.WriteByte(living.Config.isBotom);
            pkg.WriteBoolean(living.Config.isShowBlood);
            pkg.WriteBoolean(living.Config.isShowSmallMapPoint);
            pkg.WriteInt(0);
            pkg.WriteInt(0);
            pkg.WriteBoolean(living.IsFrost);
            pkg.WriteBoolean(living.IsHide);
            pkg.WriteBoolean(living.IsNoHole);
            pkg.WriteBoolean(false);
            pkg.WriteInt(0);
            this.SendToAll(pkg);
        }

        internal void SendAddPhysicalObj(PhysicalObj obj)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)48);
            pkg.WriteInt(obj.Id);
            pkg.WriteInt(obj.Type);
            pkg.WriteInt(obj.X);
            pkg.WriteInt(obj.Y);
            pkg.WriteString(obj.Model);
            pkg.WriteString(obj.CurrentAction);
            pkg.WriteInt(obj.Scale);
            pkg.WriteInt(obj.Scale);
            pkg.WriteInt(obj.Rotation);
            pkg.WriteInt(obj.phyBringToFront);
            pkg.WriteInt(obj.typeEffect);
            this.SendToAll(pkg);
        }

        internal void SendAddPhysicalTip(PhysicalObj obj)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)68);
            pkg.WriteInt(obj.Id);
            pkg.WriteInt(obj.Type);
            pkg.WriteInt(obj.X);
            pkg.WriteInt(obj.Y);
            pkg.WriteString(obj.Model);
            pkg.WriteString(obj.CurrentAction);
            pkg.WriteInt(obj.Scale);
            pkg.WriteInt(obj.Rotation);
            this.SendToAll(pkg);
        }

        internal void SendAttackEffect(Living player, int type)
        {
            GSPacketIn pkg = new GSPacketIn((short)91)
            {
                Parameter1 = player.Id
            };
            pkg.WriteByte((byte)129);
            pkg.WriteBoolean(true);
            pkg.WriteInt(type);
            this.SendToAll(pkg);
        }

        internal void method_10(
          Living living_0,
          PetSkillElementInfo petSkillElementInfo_0,
          bool bool_1)
        {
            this.method_12(living_0, petSkillElementInfo_0, bool_1, 3, (IGamePlayer)null);
        }

        internal void method_11(
          Living living_0,
          PetSkillElementInfo petSkillElementInfo_0,
          bool bool_1,
          int int_4)
        {
            this.method_12(living_0, petSkillElementInfo_0, bool_1, int_4, (IGamePlayer)null);
        }

        internal void method_12(
          Living living_0,
          PetSkillElementInfo petSkillElementInfo_0,
          bool bool_1,
          int int_4,
          IGamePlayer igamePlayer_0)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living_0.Id);
            pkg.Parameter1 = living_0.Id;
            pkg.WriteByte((byte)145);
            pkg.WriteInt(petSkillElementInfo_0.ID);
            pkg.WriteString(petSkillElementInfo_0.Name);
            pkg.WriteString(petSkillElementInfo_0.Description);
            pkg.WriteString(petSkillElementInfo_0.Pic.ToString());
            pkg.WriteString(petSkillElementInfo_0.EffectPic);
            pkg.WriteBoolean(bool_1);
            this.SendToAll(pkg);
        }

        internal void SendCreateGame()
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)101);
            pkg.WriteInt((int)(byte)this.m_roomType);
            pkg.WriteInt((int)(byte)this.m_gameType);
            pkg.WriteInt(this.m_timeType);
            List<Player> allFightPlayers = this.GetAllFightPlayers();
            pkg.WriteInt(allFightPlayers.Count);
            foreach (Player player in allFightPlayers)
            {
                IGamePlayer playerDetail = player.PlayerDetail;
                pkg.WriteInt(playerDetail.ZoneId);
                pkg.WriteString(playerDetail.ZoneName);
                pkg.WriteInt(playerDetail.PlayerCharacter.ID);
                pkg.WriteString(playerDetail.PlayerCharacter.NickName);
                pkg.WriteBoolean(false);
                pkg.WriteByte(playerDetail.PlayerCharacter.typeVIP);
                pkg.WriteInt(playerDetail.PlayerCharacter.VIPLevel);
                pkg.WriteBoolean(playerDetail.PlayerCharacter.Sex);
                pkg.WriteInt(playerDetail.PlayerCharacter.Hide);
                pkg.WriteString(playerDetail.PlayerCharacter.Style);
                pkg.WriteString(playerDetail.PlayerCharacter.Colors);
                pkg.WriteString(playerDetail.PlayerCharacter.Skin);
                pkg.WriteInt(playerDetail.PlayerCharacter.Grade);
                pkg.WriteInt(playerDetail.PlayerCharacter.Repute);
                if (playerDetail.MainWeapon == null)
                {
                    pkg.WriteInt(0);
                }
                else
                {
                    pkg.WriteInt(playerDetail.MainWeapon.TemplateID);
                    pkg.WriteInt(playerDetail.MainWeapon.RefineryLevel);
                    pkg.WriteString(playerDetail.MainWeapon.Template.Name);
                    pkg.WriteDateTime(DateTime.MinValue);
                }
                if (playerDetail.SecondWeapon == null)
                    pkg.WriteInt(0);
                else
                    pkg.WriteInt(playerDetail.SecondWeapon.TemplateID);
                pkg.WriteInt(playerDetail.PlayerCharacter.Nimbus);
                pkg.WriteBoolean(playerDetail.PlayerCharacter.IsShowConsortia);
                pkg.WriteInt(playerDetail.PlayerCharacter.ConsortiaID);
                pkg.WriteString(playerDetail.PlayerCharacter.ConsortiaName);
                pkg.WriteInt(playerDetail.PlayerCharacter.badgeID);
                pkg.WriteInt(playerDetail.PlayerCharacter.ConsortiaLevel);
                pkg.WriteInt(playerDetail.PlayerCharacter.ConsortiaRepute);
                pkg.WriteInt(playerDetail.PlayerCharacter.Win);
                pkg.WriteInt(playerDetail.PlayerCharacter.Total);
                pkg.WriteInt(playerDetail.PlayerCharacter.FightPower);
                pkg.WriteInt(playerDetail.PlayerCharacter.apprenticeshipState);
                pkg.WriteInt(playerDetail.PlayerCharacter.masterID);
                pkg.WriteString(playerDetail.PlayerCharacter.masterOrApprentices);
                pkg.WriteInt(playerDetail.PlayerCharacter.AchievementPoint);
                pkg.WriteString(playerDetail.PlayerCharacter.Honor);
                pkg.WriteInt(playerDetail.PlayerCharacter.Offer);
                pkg.WriteBoolean(player.PlayerDetail.MatchInfo.DailyLeagueFirst);
                pkg.WriteInt(player.PlayerDetail.MatchInfo.DailyLeagueLastScore);
                pkg.WriteBoolean(playerDetail.PlayerCharacter.IsMarried);
                if (playerDetail.PlayerCharacter.IsMarried)
                {
                    pkg.WriteInt(playerDetail.PlayerCharacter.SpouseID);
                    pkg.WriteString(playerDetail.PlayerCharacter.SpouseName);
                }
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                pkg.WriteInt(player.Team);
                pkg.WriteInt(player.Id);
                pkg.WriteInt(player.MaxBlood);
                if (player.Pet == null)
                {
                    pkg.WriteInt(0);
                }
                else
                {
                    pkg.WriteInt(1);
                    pkg.WriteInt(player.Pet.Place);
                    pkg.WriteInt(player.Pet.TemplateID);
                    pkg.WriteInt(player.Pet.ID);
                    pkg.WriteString(player.Pet.Name);
                    pkg.WriteInt(player.Pet.UserID);
                    pkg.WriteInt(player.Pet.Level);
                    List<string> skillEquip = player.Pet.GetSkillEquip();
                    pkg.WriteInt(skillEquip.Count);
                    foreach (string str in skillEquip)
                    {
                        pkg.WriteInt(int.Parse(str.Split(',')[1]));
                        pkg.WriteInt(int.Parse(str.Split(',')[0]));
                    }
                }
            }
            this.SendToAll(pkg);
        }

        internal void SendEquipEffect(Living player, string buffer)
        {
            GSPacketIn pkg = new GSPacketIn((short)3);
            pkg.WriteInt(3);
            pkg.WriteString(buffer);
            this.SendToAll(pkg);
        }

        internal void SendFightAchievement(Living living, int achievID, int dis, int delay)
        {
            if (living.Game.RoomType != eRoomType.Match && living.Game.RoomType != eRoomType.Freedom)
                return;
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)238);
            pkg.WriteInt(achievID);
            pkg.WriteInt(dis);
            pkg.WriteInt(delay);
            this.SendToAll(pkg);
        }

        internal void SendGameActionMapping(Living player, Ball ball)
        {
            string currentAction = ball.CurrentAction;
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id);
            pkg.WriteByte((byte)223);
            pkg.WriteInt(ball.Id);
            pkg.WriteString(currentAction);
            pkg.WriteString(ball.ActionMapping[currentAction]);
            this.SendToAll(pkg);
        }

        internal void SendGameBigBox(Living player, List<int> listTemplate)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id);
            pkg.WriteByte((byte)136);
            pkg.WriteInt(listTemplate.Count);
            foreach (int val in listTemplate)
                pkg.WriteInt(val);
            this.SendToAll(pkg);
        }

        internal void SendGameNextTurn(Living living, BaseGame game, List<Box> newBoxes)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living.Id);
            pkg.Parameter1 = living.Id;
            pkg.WriteByte((byte)6);
            int Wind = (int)((double)game.Wind * 10.0);
            pkg.WriteBoolean(Wind > 0);
            pkg.WriteByte(this.GetVane(Wind, 1));
            pkg.WriteByte(this.GetVane(Wind, 2));
            pkg.WriteByte(this.GetVane(Wind, 3));
            pkg.WriteBoolean(living.IsHide);
            pkg.WriteInt(this.getTurnTime());
            pkg.WriteInt(newBoxes.Count);
            foreach (Box newBox in newBoxes)
            {
                pkg.WriteInt(newBox.Id);
                pkg.WriteInt(newBox.X);
                pkg.WriteInt(newBox.Y);
                pkg.WriteInt(newBox.Type);
            }
            List<Player> allFightPlayers = game.GetAllFightPlayers();
            pkg.WriteInt(allFightPlayers.Count);
            foreach (Player player in allFightPlayers)
            {
                pkg.WriteInt(player.Id);
                pkg.WriteBoolean(player.IsLiving);
                pkg.WriteInt(player.X);
                pkg.WriteInt(player.Y);
                pkg.WriteInt(player.Blood);
                pkg.WriteBoolean(player.IsNoHole);
                pkg.WriteInt(player.Energy);
                pkg.WriteInt(player.psychic);
                pkg.WriteInt(player.Dander);
                if (player.Pet == null)
                {
                    pkg.WriteInt(0);
                    pkg.WriteInt(0);
                }
                else
                {
                    pkg.WriteInt(player.PetMaxMP);
                    pkg.WriteInt(player.PetMP);
                }
                pkg.WriteInt(player.ShootCount);
                pkg.WriteInt(player.flyCount);
            }
            pkg.WriteInt(game.TurnIndex);
            this.SendToAll(pkg);
        }

        internal void method_50(Player player_0, int int_4)
        {
            this.method_51(player_0, player_0.PetEffects.CurrentUseSkill, (uint)player_0.PetEffects.CurrentUseSkill > 0U, int_4);
        }

        internal void method_51(Player player_0, int int_4, bool bool_1, int int_5)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player_0.Id);
            pkg.Parameter1 = player_0.Id;
            pkg.WriteByte((byte)144);
            pkg.WriteInt(int_4);
            pkg.WriteBoolean(bool_1);
            pkg.WriteInt(int_5);
            this.SendToAll(pkg);
        }

        internal void method_51(Living living_0)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)4);
            (living_0 as Player).PlayerDetail.SendTCP(pkg);
        }

        internal void SendGamePickBox(Living player, int index, int arkType, string goods)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id);
            pkg.WriteByte((byte)49);
            pkg.WriteByte((byte)index);
            pkg.WriteByte((byte)arkType);
            pkg.WriteString(goods);
            this.SendToAll(pkg);
        }

        internal void SendGamePlayerProperty(Living living, string type, string state)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living.Id)
            {
                Parameter1 = living.Id
            };
            pkg.WriteByte((byte)41);
            pkg.WriteString(type);
            pkg.WriteString(state);
            this.SendToAll(pkg);
        }

        internal void SendGamePlayerTakeCard(Player player, int index, int templateID, int count)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id)
            {
                Parameter1 = player.Id
            };
            pkg.WriteByte((byte)98);
            pkg.WriteBoolean(false);
            pkg.WriteByte((byte)index);
            pkg.WriteInt(templateID);
            pkg.WriteInt(count);
            pkg.WriteBoolean(false);
            this.SendToAll(pkg);
        }

        internal void SendGameUpdateBall(Player player, bool Special)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id)
            {
                Parameter1 = player.Id
            };
            pkg.WriteByte((byte)20);
            pkg.WriteBoolean(Special);
            pkg.WriteInt(player.CurrentBall.ID);
            this.SendToAll(pkg);
        }

        internal void SendGameUpdateDander(TurnedLiving player)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id)
            {
                Parameter1 = player.Id
            };
            pkg.WriteByte((byte)14);
            pkg.WriteInt(player.Dander);
            this.SendToAll(pkg);
        }

        internal void SendGameUpdateFrozenState(Living player)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id)
            {
                Parameter1 = player.Id
            };
            pkg.WriteByte((byte)33);
            pkg.WriteBoolean(player.IsFrost);
            this.SendToAll(pkg);
        }

        internal void SendGameUpdateHealth(Living living, int type, int value)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living.Id)
            {
                Parameter1 = living.Id
            };
            pkg.WriteByte((byte)11);
            pkg.WriteByte((byte)type);
            pkg.WriteInt(living.Blood);
            pkg.WriteInt(value);
            this.SendToAll(pkg);
        }

        internal void SendGameUpdateHideState(Living player)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id)
            {
                Parameter1 = player.Id
            };
            pkg.WriteByte((byte)35);
            pkg.WriteBoolean(player.IsHide);
            this.SendToAll(pkg);
        }

        internal void SendGameUpdateNoHoleState(Living player)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id)
            {
                Parameter1 = player.Id
            };
            pkg.WriteByte((byte)82);
            pkg.WriteBoolean(player.IsNoHole);
            this.SendToAll(pkg);
        }

        internal void SendGameUpdateSealState(Living player, int type)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.Parameter1 = player.Id;
            pkg.WriteByte((byte)41);
            pkg.WriteByte((byte)type);
            pkg.WriteBoolean(player.GetSealState());
            this.SendToAll(pkg);
        }

        internal void SendGameUpdateShootCount(Player player)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id)
            {
                Parameter1 = player.Id
            };
            pkg.WriteByte((byte)46);
            pkg.WriteByte((byte)player.ShootCount);
            this.SendToAll(pkg);
        }

        internal void SendGameUpdateWind(float wind)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)38);
            int num = (int)((double)wind * 10.0);
            pkg.WriteInt(num);
            pkg.WriteBoolean(num > 0);
            pkg.WriteByte(this.GetVane(num, 1));
            pkg.WriteByte(this.GetVane(num, 2));
            pkg.WriteByte(this.GetVane(num, 3));
            this.SendToAll(pkg);
        }

        internal void SendGameWindPic(byte windId, byte[] windpic)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)241);
            pkg.WriteByte(windId);
            pkg.Write(windpic);
            this.SendToAll(pkg);
        }

        internal void SendIsLastMission(bool isLast)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)160);
            pkg.WriteBoolean(isLast);
            this.SendToAll(pkg);
        }

        internal void SendLivingBeat(
          Living living,
          Living target,
          int totalDemageAmount,
          string action,
          int livingCount,
          int attackEffect)
        {
            int val = 0;
            if (target is Player)
                val = (target as Player).Dander;
            GSPacketIn pkg = new GSPacketIn((short)91, living.Id)
            {
                Parameter1 = living.Id
            };
            pkg.WriteByte((byte)58);
            pkg.WriteString(!string.IsNullOrEmpty(action) ? action : "");
            pkg.WriteInt(livingCount);
            for (int index = 1; index <= livingCount; ++index)
            {
                pkg.WriteInt(target.Id);
                pkg.WriteInt(totalDemageAmount);
                pkg.WriteInt(target.Blood);
                pkg.WriteInt(val);
                pkg.WriteInt(attackEffect);
            }
            this.SendToAll(pkg);
        }

        internal void SendLivingFall(
          Living living,
          int toX,
          int toY,
          int speed,
          string action,
          int type)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living.Id)
            {
                Parameter1 = living.Id
            };
            pkg.WriteByte((byte)56);
            pkg.WriteInt(toX);
            pkg.WriteInt(toY);
            pkg.WriteInt(speed);
            pkg.WriteString(!string.IsNullOrEmpty(action) ? action : "");
            pkg.WriteInt(type);
            this.SendToAll(pkg);
        }

        internal void SendLivingJump(
          Living living,
          int toX,
          int toY,
          int speed,
          string action,
          int type)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living.Id)
            {
                Parameter1 = living.Id
            };
            pkg.WriteByte((byte)57);
            pkg.WriteInt(toX);
            pkg.WriteInt(toY);
            pkg.WriteInt(speed);
            pkg.WriteString(!string.IsNullOrEmpty(action) ? action : "");
            pkg.WriteInt(type);
            this.SendToAll(pkg);
        }

        internal void SendLivingMoveTo(
          Living living,
          int fromX,
          int fromY,
          int toX,
          int toY,
          string action,
          int speed)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living.Id)
            {
                Parameter1 = living.Id
            };
            pkg.WriteByte((byte)55);
            pkg.WriteInt(fromX);
            pkg.WriteInt(fromY);
            pkg.WriteInt(toX);
            pkg.WriteInt(toY);
            pkg.WriteInt(speed);
            pkg.WriteString(!string.IsNullOrEmpty(action) ? action : "");
            pkg.WriteString("");
            this.SendToAll(pkg);
        }

        internal void SendLivingMoveTo(
          Living living,
          int fromX,
          int fromY,
          int toX,
          int toY,
          string action,
          int speed,
          string sAction)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living.Id)
            {
                Parameter1 = living.Id
            };
            pkg.WriteByte((byte)55);
            pkg.WriteInt(fromX);
            pkg.WriteInt(fromY);
            pkg.WriteInt(toX);
            pkg.WriteInt(toY);
            pkg.WriteInt(speed);
            pkg.WriteString(!string.IsNullOrEmpty(action) ? action : "");
            pkg.WriteString(!string.IsNullOrEmpty(sAction) ? sAction : "");
            this.SendToAll(pkg);
        }

        internal void SendLivingPlayMovie(Living living, string action)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living.Id)
            {
                Parameter1 = living.Id
            };
            pkg.WriteByte((byte)60);
            pkg.WriteString(action);
            this.SendToAll(pkg);
        }

        internal void SendLivingSay(Living living, string msg, int type)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living.Id)
            {
                Parameter1 = living.Id
            };
            pkg.WriteByte((byte)59);
            pkg.WriteString(msg);
            pkg.WriteInt(type);
            this.SendToAll(pkg);
        }

        internal void SendLivingShowBlood(Player player, int isShow)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id);
            pkg.WriteByte((byte)80);
            pkg.WriteInt(player.Id);
            pkg.WriteInt(isShow);
            this.SendToAll(pkg);
        }

        internal void SendLivingShowBlood(Player player, long blood, int x, int y)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id);
            pkg.WriteByte((byte)39);
            pkg.WriteInt(player.Id);
            pkg.WriteLong(blood);
            pkg.WriteInt(x);
            pkg.WriteInt(y);
            this.SendToAll(pkg);
        }

        internal void SendLivingTurnRotation(Player player, int rotation, int speed, string endPlay)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id)
            {
                Parameter1 = player.Id
            };
            pkg.WriteByte((byte)85);
            pkg.WriteInt(rotation);
            pkg.WriteInt(speed);
            pkg.WriteString(endPlay);
            this.SendToAll(pkg);
        }

        internal void SendLivingUpdateAngryState(Living living)
        {
            GSPacketIn pkg = new GSPacketIn((short)91)
            {
                Parameter1 = living.Id
            };
            pkg.WriteByte((byte)118);
            pkg.WriteInt(living.State);
            this.SendToAll(pkg);
        }

        internal void SendLivingUpdateDirection(Living living)
        {
            GSPacketIn pkg = new GSPacketIn((short)91)
            {
                Parameter1 = living.Id
            };
            pkg.WriteByte((byte)7);
            pkg.WriteInt(living.Direction);
            this.SendToAll(pkg);
        }

        internal void SendMessage(IGamePlayer player, string msg, string msg1, int type)
        {
            if (msg != null)
            {
                GSPacketIn pkg = new GSPacketIn((short)3);
                pkg.WriteInt(type);
                pkg.WriteString(msg);
                player.SendTCP(pkg);
            }
            if (msg1 == null)
                return;
            GSPacketIn pkg1 = new GSPacketIn((short)3);
            pkg1.WriteInt(type);
            pkg1.WriteString(msg1);
            this.SendToAll(pkg1, player);
        }

        internal void SendMissionTryAgain(int type)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)119);
            pkg.WriteInt(type);
            this.SendToAll(pkg);
        }

        internal void SendOpenSelectLeaderWindow(int maxTime)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)102);
            pkg.WriteInt(maxTime);
            this.SendToAll(pkg);
        }

        internal void SendPetBuff(Living player, PetSkillElementInfo info, bool isActive)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id)
            {
                Parameter1 = player.Id
            };
            pkg.WriteByte((byte)145);
            pkg.WriteInt(info.ID);
            pkg.WriteString("");
            pkg.WriteString("");
            pkg.WriteString(info.Pic.ToString());
            pkg.WriteString(info.EffectPic);
            pkg.WriteBoolean(isActive);
            this.SendToAll(pkg);
        }

        internal void SendPetSkillCd(Living player, int skillInfoID, int ColdDown)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)147);
            pkg.WriteInt(skillInfoID);
            pkg.WriteInt(ColdDown);
            (player as Player).PlayerDetail.SendTCP(pkg);
        }

        internal void SendPetUseKill(Player player, int type)
        {
            this.SendPetUseKill(player, player.PetEffects.CurrentUseSkill, player.PetEffects.CurrentUseSkill != 0, type);
        }
        internal void SendPetUseKill(Player player, int killId, bool isUse, int type)
        {
            GSPacketIn packet = new GSPacketIn((short)91, player.Id);
            packet.Parameter1 = player.Id;
            packet.WriteByte((byte)144);
            packet.WriteInt(killId);
            packet.WriteBoolean(isUse);
            packet.WriteInt(type);
            this.SendToAll(packet);
        }

        internal void SendSelfTurn(Living living)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)4);
            (living as Player).PlayerDetail.SendTCP(pkg);
        }

        internal void SendPlayerMove(
          Player player,
          int type,
          int x,
          int y,
          byte dir,
          bool isLiving,
          bool sendExcept)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id)
            {
                Parameter1 = player.Id
            };
            pkg.WriteByte((byte)9);
            pkg.WriteBoolean(false);
            pkg.WriteByte((byte)type);
            pkg.WriteInt(x);
            pkg.WriteInt(y);
            pkg.WriteByte(dir);
            pkg.WriteBoolean(isLiving);
            if (type == 2)
            {
                pkg.WriteInt(this.m_tempBox.Count);
                foreach (Box box in this.m_tempBox)
                {
                    pkg.WriteInt(box.X);
                    pkg.WriteInt(box.Y);
                }
            }
            if (sendExcept)
                this.SendToAll(pkg, player.PlayerDetail);
            else
                this.SendToAll(pkg);
        }

        internal void SendPlayerMove(
          Player player,
          int type,
          int x,
          int y,
          byte dir,
          bool isLiving,
          string action)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id)
            {
                Parameter1 = player.Id
            };
            pkg.WriteByte((byte)9);
            pkg.WriteByte((byte)type);
            pkg.WriteInt(x);
            pkg.WriteInt(y);
            pkg.WriteByte(dir);
            pkg.WriteBoolean(isLiving);
            pkg.WriteString(!string.IsNullOrEmpty(action) ? action : "move");
            this.SendToAll(pkg);
        }

        internal void SendPlayerMove2(
          Player player,
          int type,
          int x,
          int y,
          byte dir,
          bool isLiving)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id)
            {
                Parameter1 = player.Id
            };
            pkg.WriteByte((byte)9);
            pkg.WriteByte((byte)type);
            pkg.WriteInt(x);
            pkg.WriteInt(y);
            pkg.WriteByte(dir);
            pkg.WriteBoolean(isLiving);
            if (type == 2)
            {
                pkg.WriteInt(this.m_tempBox.Count);
                foreach (Box box in this.m_tempBox)
                {
                    pkg.WriteInt(box.X);
                    pkg.WriteInt(box.Y);
                }
            }
            this.SendToAll(pkg);
        }

        internal void SendPlayerPicture(Living living, int type, bool state)
        {
            GSPacketIn pkg = new GSPacketIn((short)91)
            {
                Parameter1 = living.Id
            };
            pkg.WriteByte((byte)128);
            pkg.WriteInt(type);
            pkg.WriteBoolean(state);
            this.SendToAll(pkg);
        }

        internal void SendPlayerRemove(Player player)
        {
            GSPacketIn pkg = new GSPacketIn((short)94, player.PlayerDetail.PlayerCharacter.ID);
            pkg.WriteByte((byte)5);
            pkg.WriteInt(player.PlayerDetail.ZoneId);
            this.SendToAll(pkg);
        }

        internal void SendLoading()
        {
            foreach (Player allFightPlayer in this.GetAllFightPlayers())
            {
                GSPacketIn gSPacketIn = new GSPacketIn(91);
                gSPacketIn.WriteByte(16);
                gSPacketIn.WriteInt(allFightPlayer.LoadingProcess);
                gSPacketIn.WriteInt(allFightPlayer.PlayerDetail.ZoneId);
                gSPacketIn.WriteInt(allFightPlayer.PlayerDetail.PlayerCharacter.ID);
                SendToAll(gSPacketIn);
            }
        }

        internal void SendPlayerUseProp(Player player, int type, int place, int templateID)
        {
            this.SendPlayerUseProp((Living)player, type, place, templateID, player);
        }

        internal void SendPlayerUseProp(Living player, int type, int place, int templateID, Player p)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id)
            {
                Parameter1 = player.Id
            };
            pkg.WriteByte((byte)32);
            pkg.WriteByte((byte)type);
            pkg.WriteInt(place);
            pkg.WriteInt(templateID);
            pkg.WriteInt(p.Id);
            pkg.WriteBoolean(templateID == 10017);
            this.SendToAll(pkg);
        }

        internal void SendPhysicalObjFocus(int x, int y, int type)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)62);
            pkg.WriteInt(type);
            pkg.WriteInt(x);
            pkg.WriteInt(y);
            this.SendToAll(pkg);
        }

        internal void SendPhysicalObjPlayAction(PhysicalObj obj)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)66);
            pkg.WriteInt(obj.Id);
            pkg.WriteString(obj.CurrentAction);
            this.SendToAll(pkg);
        }

        internal void SendRemoveLiving(int id)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)53);
            pkg.WriteInt(id);
            this.SendToAll(pkg);
        }

        internal void SendRemovePhysicalObj(PhysicalObj obj)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)53);
            pkg.WriteInt(obj.Id);
            this.SendToAll(pkg);
        }

        internal void SendSkipNext(Player player)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)12);
            player.PlayerDetail.SendTCP(pkg);
        }

        internal void SendStartLoading(int maxTime)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)103);
            pkg.WriteInt(maxTime);
            pkg.WriteInt(this.m_map.Info.ID);
            pkg.WriteInt(this.m_loadingFiles.Count);
            foreach (LoadingFileInfo loadingFile in this.m_loadingFiles)
            {
                pkg.WriteInt(loadingFile.Type);
                pkg.WriteString(loadingFile.Path);
                pkg.WriteString(loadingFile.ClassName);
            }
            if (this.IsSpecialPVE())
            {
                pkg.WriteInt(0);
            }
            else
            {
                GameNeedPetSkillInfo[] gameNeedPetSkill = PetMgr.GetGameNeedPetSkill();
                pkg.WriteInt(gameNeedPetSkill.Length);
                foreach (GameNeedPetSkillInfo needPetSkillInfo in gameNeedPetSkill)
                {
                    pkg.WriteString(needPetSkillInfo.Pic.ToString());
                    pkg.WriteString(needPetSkillInfo.EffectPic);
                }
            }
            this.SendToAll(pkg);
        }

        internal void method_53(Living living_0, string string_0)
        {
            GSPacketIn pkg = new GSPacketIn((short)3);
            pkg.WriteInt(0);
            pkg.WriteString(string_0);
            this.SendToAll(pkg);
        }

        public void SendSyncLifeTime()
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.Parameter2 = -1;
            pkg.WriteByte((byte)131);
            pkg.WriteInt(this.m_lifeTime);
            this.SendToAll(pkg);
        }

        internal void method_34(Living living_0, List<int> BhV2sgMkjBpUnK7WcH)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, living_0.Id);
            pkg.WriteByte((byte)136);
            pkg.WriteInt(BhV2sgMkjBpUnK7WcH.Count);
            foreach (int val in BhV2sgMkjBpUnK7WcH)
                pkg.WriteInt(val);
            this.SendToAll(pkg);
        }

        public virtual void SendToAll(GSPacketIn pkg)
        {
            this.SendToAll(pkg, (IGamePlayer)null);
        }

        public virtual void SendToAll(GSPacketIn pkg, IGamePlayer except)
        {
            if (pkg.Parameter2 == 0)
                pkg.Parameter2 = this.LifeTime;
            foreach (Player allFightPlayer in this.GetAllFightPlayers())
            {
                if (allFightPlayer.IsActive && allFightPlayer.PlayerDetail != except)
                    allFightPlayer.PlayerDetail.SendTCP(pkg);
            }
        }

        public virtual void SendToTeam(GSPacketIn pkg, int team)
        {
            this.SendToTeam(pkg, team, (IGamePlayer)null);
        }

        public virtual void SendToTeam(GSPacketIn pkg, int team, IGamePlayer except)
        {
            foreach (Player allFightPlayer in this.GetAllFightPlayers())
            {
                if (allFightPlayer.IsActive && allFightPlayer.PlayerDetail != except && allFightPlayer.Team == team)
                    allFightPlayer.PlayerDetail.SendTCP(pkg);
            }
        }

        public void SendChatToTeam(GSPacketIn pkg, Player p)
        {
            if (pkg.Parameter2 == 0)
                pkg.Parameter2 = this.LifeTime;
            foreach (Player allTeamPlayer in this.GetAllTeamPlayers((Living)p))
            {
                if (allTeamPlayer.IsActive && allTeamPlayer.PlayerDetail != p)
                    allTeamPlayer.PlayerDetail.SendTCP(pkg);
            }
        }

        internal void SendUseDeputyWeapon(Player player, int ResCount)
        {
            GSPacketIn pkg = new GSPacketIn((short)91, player.Id)
            {
                Parameter1 = player.Id
            };
            pkg.WriteByte((byte)84);
            pkg.WriteInt(ResCount);
            player.PlayerDetail.SendTCP(pkg);
        }

        public bool SetMap(int mapId)
        {
            if (this.GameState != eGameState.Playing)
            {
                Map map = MapMgr.CloneMap(mapId);
                if (map != null)
                {
                    this.m_map = map;
                    return true;
                }
            }
            return false;
        }

        public void SetWind(int wind)
        {
            this.m_map.wind = (float)wind;
        }

        public void Shuffer<T>(T[] array)
        {
            for (int length = array.Length; length > 1; --length)
            {
                int index = this.Random.Next(length);
                T obj = array[index];
                array[index] = array[length - 1];
                array[length - 1] = obj;
            }
        }

        public Ball AddBall(Ball ball, bool sendToClient)
        {
            this.m_tempBall.Add(ball);
            this.AddPhysicalObj((PhysicalObj)ball, sendToClient);
            return ball;
        }

        public virtual bool TakeCard(Player player)
        {
            return false;
        }
        public virtual bool TakeCard(Player player, int index, bool isSystem)
        {
            return false;
        }

        public virtual bool TakeCard(Player player, int index)
        {
            return false;
        }

        public int FindBombPlayerX(int blowArea)
        {
            Dictionary<int, int> dictionary1 = new Dictionary<int, int>();
            Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
            List<int> intList = new List<int>();
            List<Player> allFightPlayers = this.GetAllFightPlayers();
            int num1 = 0;
            foreach (Player player in allFightPlayers)
            {
                if (player.IsLiving)
                {
                    for (int index = 0; index < 10; ++index)
                    {
                        int key;
                        do
                        {
                            key = this.Random.Next(player.X - blowArea, player.X + blowArea);
                        }
                        while (dictionary1.ContainsKey(key));
                        dictionary1.Add(key, 0);
                    }
                }
            }
            foreach (int key in dictionary1.Keys)
            {
                foreach (Player player in allFightPlayers)
                {
                    if (player.X > key - blowArea && player.X < key + blowArea)
                    {
                        if (dictionary2.ContainsKey(key))
                        {
                            Dictionary<int, int> dictionary3;
                            int index;
                            (dictionary3 = dictionary2)[index = key] = dictionary3[index] + 1;
                        }
                        else
                            dictionary2.Add(key, 1);
                    }
                }
            }
            foreach (int num2 in dictionary2.Values)
            {
                if (num2 > num1)
                    num1 = num2;
            }
            foreach (int key in dictionary2.Keys)
            {
                if (dictionary2[key] == num1)
                    intList.Add(key);
            }
            int index1 = this.Random.Next(0, intList.Count);
            return intList[index1];
        }

        public Player Timnguoichoigannhat()
        {
            List<Player> playerList = new List<Player>();
            List<int> source = new List<int>();
            foreach (Player player in this.m_players.Values)
            {
                if (player.IsLiving)
                {
                    playerList.Add(player);
                    source.Add(player.X);
                }
            }
            int num = source.Max();
            foreach (Player player in playerList)
            {
                if (player.X == num)
                    return player;
            }
            return (Player)null;
        }

        public override string ToString()
        {
            return string.Format("Id:{0},player:{1},state:{2},current:{3},turnIndex:{4},actions:{5}", (object)this.Id, (object)this.PlayerCount, (object)this.GameState, (object)this.CurrentLiving, (object)this.m_turnIndex, (object)this.m_actions.Count);
        }

        public bool IsSpecialPVE()
        {
            switch (this.RoomType)
            {
                case eRoomType.FightLab:
                case eRoomType.Freshman:
                    return true;
                default:
                    return false;
            }
        }

        public void Update(long tick)
        {
            if (this.m_passTick >= tick)
                return;
            ++this.m_lifeTime;
            ArrayList arrayList;
            lock (this.m_actions)
            {
                arrayList = (ArrayList)this.m_actions.Clone();
                this.m_actions.Clear();
            }
            if (arrayList == null || this.GameState == eGameState.Stopped)
                return;
            this.CurrentActionCount = arrayList.Count;
            if (arrayList.Count > 0)
            {
                ArrayList actions = new ArrayList();
                foreach (IAction action in arrayList)
                {
                    try
                    {
                        action.Execute(this, tick);
                        if (!action.IsFinished(tick))
                            actions.Add((object)action);
                    }
                    catch (Exception ex)
                    {
                        BaseGame.log.Error((object)"Map update error:", ex);
                    }
                }
                this.AddAction(actions);
            }
            else
            {
                if (this.m_waitTimer >= tick)
                    return;
                this.CheckState(0);
            }
        }

        public void UpdateWind(float wind, bool sendToClient)
        {
            if (this.m_confineWind || (double)this.m_map.wind == (double)wind)
                return;
            this.m_map.wind = wind;
            if (!sendToClient)
                return;
            this.SendGameUpdateWind(wind);
        }

        public void VaneLoading()
        {
            foreach (WindInfo windInfo in WindMgr.GetWind())
                this.SendGameWindPic((byte)windInfo.WindID, windInfo.WindPic);
        }

        public void WaitTime(int delay)
        {
            this.m_waitTimer = Math.Max(this.m_waitTimer, TickHelper.GetTickCount() + (long)delay);
            this.long_1 = this.m_waitTimer;
        }

        public TurnedLiving CurrentLiving
        {
            get
            {
                return this.m_currentLiving;
            }
        }

        public eGameState GameState
        {
            get
            {
                return this.m_gameState;
            }
        }

        public bool GetBlood
        {
            get
            {
                return this.m_GetBlood;
            }
            set
            {
                this.m_GetBlood = value;
            }
        }

        public bool HasPlayer
        {
            get
            {
                return this.m_players.Count > 0;
            }
        }

        public int LifeTime
        {
            get
            {
                return this.m_lifeTime;
            }
        }

        protected int m_turnIndex
        {
            get
            {
                return this.turnIndex;
            }
            set
            {
                this.turnIndex = value;
            }
        }

        public Map Map
        {
            get
            {
                return this.m_map;
            }
        }

        public int nextPlayerId
        {
            get
            {
                return this.m_nextPlayerId;
            }
            set
            {
                this.m_nextPlayerId = value;
            }
        }

        public int PlayerCount
        {
            get
            {
                lock (this.m_players)
                    return this.m_players.Count;
            }
        }

        public Dictionary<int, Player> Players
        {
            get
            {
                return this.m_players;
            }
        }

        public Random Random
        {
            get
            {
                return this.m_random;
            }
        }

        public int RoomId
        {
            get
            {
                return this.m_roomId;
            }
        }

        public int TurnIndex
        {
            get
            {
                return this.m_turnIndex;
            }
            set
            {
                this.m_turnIndex = value;
            }
        }

        public List<TurnedLiving> TurnQueue
        {
            get
            {
                return this.m_turnQueue;
            }
        }

        public float Wind
        {
            get
            {
                return this.m_map.wind;
            }
        }

        internal void SendLivingWalkTo(
          Living m_living,
          int p1,
          int p2,
          int p3,
          int p4,
          string m_action,
          int m_speed)
        {
            throw new NotImplementedException();
        }

        public delegate void GameNpcDieEventHandle(int NpcId);

        public delegate void GameOverLogEventHandle(
          int roomId,
          eRoomType roomType,
          eGameType fightType,
          int changeTeam,
          DateTime playBegin,
          DateTime playEnd,
          int userCount,
          int mapId,
          string teamA,
          string teamB,
          string playResult,
          int winTeam,
          string BossWar);
    }
}
