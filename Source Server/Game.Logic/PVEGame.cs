using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic.Actions;
using Game.Logic.AI;
using Game.Logic.AI.Game;
using Game.Logic.AI.Mission;
using Game.Logic.Phy.Maps;
using Game.Logic.Phy.Object;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Game.Logic
{
    public class PVEGame : BaseGame
    {
        private Dictionary<int, int> m_NpcTurnQueue = new Dictionary<int, int>();
        public long AllWorldDameBoss;
        private int BeginPlayersCount;
        private DateTime beginTime;
        public int[] BossCards;
        public bool CanEnterGate;
        public bool CanShowBigBox;
        public bool IsKillWorldBoss;
        public bool IsWin;
        private int m_bossCardCount;
        private APVEGameControl m_gameAI;
        private List<string> m_gameOverResources;
        private eHardLevel m_hardLevel;
        private PveInfo m_info;
        private string m_IsBossType;
        private List<int> m_mapHistoryIds;
        private AMissionControl m_missionAI;
        private MissionInfo m_missionInfo;
        private int m_pveGameDelay;
        private MapPoint mapPos;
        public Dictionary<int, MissionInfo> Misssions;
        public int Param1;
        public int Param2;
        public int Param3;
        public int Param4;
        public int Param5;
        public int Param6;
        public int Param7;
        public int Param8;
        public Living ParamLiving;
        public string Pic;
        public int SessionId;
        public int TotalCount;
        public int TotalKillCount;
        public int TotalMissionCount;
        public double TotalNpcExperience;
        public double TotalNpcGrade;
        public int TotalTurn;
        public int WantTryAgain;
        public long WorldbossBood;
        private bool m_isPassDrama;
        public int TakeCardId;
        private int m_countAward;
        private object player;

        public Dictionary<int, int> NpcTurnQueue
        {
            get
            {
                return this.m_NpcTurnQueue;
            }
        }

        public bool IsPassDrama
        {
            get
            {
                return this.m_isPassDrama;
            }
            set
            {
                this.m_isPassDrama = value;
            }
        }

        public PVEGame(
          int id,
          int roomId,
          PveInfo info,
          List<IGamePlayer> players,
          Map map,
          eRoomType roomType,
          eGameType gameType,
          int timeType,
          eHardLevel hardLevel,
          int currentFloor)
          : base(id, roomId, map, roomType, gameType, timeType)
        {
            foreach (IGamePlayer player1 in players)
            {
                Player player2 = new Player(player1, this.PhysicalId++, (BaseGame)this, 1, player1.PlayerCharacter.hp);
                player2.Direction = this.m_random.Next(0, 1) == 0 ? 1 : -1;
                Player fp = player2;
                this.AddPlayer(player1, fp);
                this.WorldbossBood = player1.WorldbossBood;
                this.AllWorldDameBoss = player1.AllWorldDameBoss;
            }
            this.m_isPassDrama = false;
            this.m_info = info;
            this.BeginPlayersCount = players.Count;
            this.TotalKillCount = 0;
            this.TotalNpcGrade = 0.0;
            this.TotalNpcExperience = 0.0;
            this.TotalHurt = 0;
            this.ParamLiving = (Living)null;
            this.m_IsBossType = "";
            this.WantTryAgain = 0;
            this.SessionId = currentFloor <= 0 ? 0 : currentFloor - 1;
            this.m_gameOverResources = new List<string>();
            this.Misssions = new Dictionary<int, MissionInfo>();
            this.m_mapHistoryIds = new List<int>();
            this.m_hardLevel = hardLevel;
            string script = this.GetScript(info, hardLevel);
            this.m_gameAI = ScriptMgr.CreateInstance(script) as APVEGameControl;
            if (this.m_gameAI == null)
            {
                BaseGame.log.ErrorFormat("Can't create game ai :{0}", (object)script);
                this.m_gameAI = (APVEGameControl)SimplePVEGameControl.Simple;
            }
            this.m_gameAI.Game = this;
            this.m_gameAI.OnCreated();
            this.m_missionAI = (AMissionControl)SimpleMissionControl.Simple;
            this.beginTime = DateTime.Now;
            this.m_bossCardCount = 0;
            foreach (Player player in this.m_players.Values)
                player.MissionEventHandle += new PlayerMissionEventHandle(this.m_missionAI.OnMissionEvent);
        }

        public PveInfo PveInfo
        {
            get
            {
                return this.m_info;
            }
        }

        public void AddAllPlayerToTurn()
        {
            foreach (TurnedLiving turnedLiving in this.Players.Values)
                this.TurnQueue.Add(turnedLiving);
        }

        public override void AddLiving(Living living)
        {
            base.AddLiving(living);
            living.Died += new LivingEventHandle(this.living_Died);
        }

        public void AddLiving2(Living living)
        {
            this.AddNormalBoss(living);
            living.Died += new LivingEventHandle(this.living_Died);
        }

        public override Player AddPlayer(IGamePlayer gp)
        {
            if (!this.CanAddPlayer())
                return (Player)null;
            Player player1 = new Player(gp, this.PhysicalId++, (BaseGame)this, 1, gp.PlayerCharacter.hp);
            player1.Direction = this.m_random.Next(0, 1) == 0 ? 1 : -1;
            Player player2 = player1;
            this.AddPlayer(gp, player2);
            this.SendCreateGameToSingle(this, gp);
            this.SendPlayerInfoInGame(this, gp, player2);
            return player2;
        }

        public bool isTankCard()
        {
            foreach (Player player in this.m_players.Values)
            {
                if ((uint)player.CanTakeOut > 0U)
                    return false;
            }
            return true;
        }

        public LivingConfig BaseLivingConfig()
        {
            return new LivingConfig()
            {
                isBotom = 1,
                IsTurn = true,
                isShowBlood = true,
                isShowSmallMapPoint = true,
                ReduceBloodStart = 1,
                CanTakeDamage = true,
                CanFrost = false,
                CanCountKill = true,
                CanHeal = false,
                CanCollied = true
            };
        }

        private int CalculateExperience(Player p)
        {
            if (this.TotalKillCount == 0)
                return 1;
            double num1 = Math.Abs((double)p.Grade - this.TotalNpcGrade / (double)this.TotalKillCount);
            if (num1 >= 7.0)
                return 1;
            double num2 = 0.0;
            if (this.TotalKillCount > 0)
                num2 += (double)p.TotalKill / (double)this.TotalKillCount * 0.4;
            if (this.TotalHurt > 0)
                num2 += (double)p.TotalHurt / (double)this.TotalHurt * 0.4;
            if (p.IsLiving)
                num2 += 0.4;
            double num3 = 1.0;
            if (num1 >= 3.0 && num1 <= 4.0)
                num3 = 0.7;
            else if (num1 >= 5.0 && num1 <= 6.0)
                num3 = 0.4;
            double num4 = (0.9 + (double)(this.BeginPlayersCount - 1) * 0.4) / (double)this.PlayerCount;
            double num5 = this.TotalNpcExperience * num2 * num3 * num4;
            return num5 == 0.0 ? 1 : (int)num5;
        }

        private int CalculateHitRate(int hitTargetCount, int shootCount)
        {
            double num = 0.0;
            if (shootCount > 0)
                num = (double)hitTargetCount / (double)shootCount;
            return (int)(num * 100.0);
        }

        public int CountMosterPlace
        {
            get
            {
                return this.m_countAward;
            }
            set
            {
                this.m_countAward = value;
            }
        }

        private int CalculateScore(Player p)
        {
            int num = (200 - this.TurnIndex) * 5 + p.TotalKill * 5 + (int)((double)p.Blood / (double)p.MaxBlood) * 10;
            if (!this.IsWin)
                num -= 400;
            return num;
        }

        public override bool CanAddPlayer()
        {
            lock (this.m_players)
                return this.GameState == eGameState.SessionPrepared && this.m_players.Count < 4;
        }

        public bool CanGameOver()
        {
            bool flag;
            if (this.PlayerCount == 0)
                flag = true;
            else if (this.TurnIndex > this.TotalTurn - 1)
            {
                this.IsWin = false;
                flag = true;
            }
            else if (this.GetDiedPlayerCount() == this.PlayerCount)
            {
                this.IsWin = false;
                flag = true;
            }
            else
            {
                try
                {
                    return this.m_missionAI.CanGameOver();
                }
                catch (Exception ex)
                {
                    BaseGame.log.ErrorFormat("game ai script {0} error:{1}", (object)this.GameState, (object)ex);
                }
                flag = true;
            }
            return flag;
        }

        public int GetLivingCamp(Living living)
        {
            int num = 0;
            if (living is Player)
                num = 0;
            if (living is SimpleNpc)
                num = ((SimpleNpc)living).NpcInfo.Camp;
            if (living is SimpleBoss)
                num = ((SimpleBoss)living).NpcInfo.Camp;
            if (living is SimpleWingBoss)
                num = ((SimpleBoss)living).NpcInfo.Camp;
            return num;
        }

        public bool CanAddBlood(Living addBloodLiving, Living byaddLiving)
        {
            bool flag = false;
            int livingCamp1 = this.GetLivingCamp(addBloodLiving);
            int livingCamp2 = this.GetLivingCamp(byaddLiving);
            if (livingCamp1 == 0 || livingCamp1 == 1 || livingCamp1 == 3)
                flag = livingCamp2 != 2;
            if (livingCamp1 == 2)
                flag = livingCamp2 == 2 || livingCamp2 == 4;
            return flag;
        }

        public bool CanStartNewSession()
        {
            if ((uint)this.m_turnIndex > 0U)
                return this.IsAllReady();
            return true;
        }

        public void CanStopGame()
        {
            if (!this.IsWin)
            {
                if (this.GameType == eGameType.Dungeon)
                    this.ClearWaitTimer();
            }
            else if (this.Misssions.ContainsKey(1 + this.SessionId) && this.isDragonLair())
                this.WantTryAgain = 1;
            this.SetupStyle(0);
        }

        public void SetupStyle()
        {
            this.SetupStyle(this.m_info == null ? 0 : this.m_info.ID);
        }

        internal void SetupStyle(int ID)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)134);
            List<Player> allFightPlayers = this.GetAllFightPlayers();
            int place = 0;
            pkg.WriteInt(allFightPlayers.Count);
            foreach (Player player in allFightPlayers)
            {
                IGamePlayer playerDetail = player.PlayerDetail;
                string style = playerDetail.PlayerCharacter.Style;
                if (ID == 6)
                    pkg.WriteString(this.GuluOlympics(style, place));
                else
                    pkg.WriteString(style);
                ++place;
                pkg.WriteInt(playerDetail.PlayerCharacter.Hide);
                pkg.WriteBoolean(playerDetail.PlayerCharacter.Sex);
                pkg.WriteString(playerDetail.PlayerCharacter.Skin);
                pkg.WriteString(playerDetail.PlayerCharacter.Colors);
                pkg.WriteInt(playerDetail.PlayerCharacter.ID);
            }
            this.SendToAll(pkg);
        }

        public string GuluOlympics(string style, int place)
        {
            string[] strArray1 = style.Split(',');
            string[] strArray2 = new string[4]
            {
        "13300|suits100",
        "13301|suits101",
        "13302|suits102",
        "13303|suits103"
            };
            string str1 = strArray1[0];
            for (int index = 1; index < this.EquipPlace.Length; ++index)
            {
                string str2 = str1 + ",";
                str1 = this.EquipPlace[index] != 11 ? str2 + strArray1[index] : str2 + strArray1[index] + "," + strArray2[place];
            }
            return str1;
        }

        public void ClearMissionData()
        {
            foreach (Physics living in this.m_livings)
                living.Dispose();
            this.m_livings.Clear();
            List<TurnedLiving> turnedLivingList = new List<TurnedLiving>();
            foreach (TurnedLiving turn in this.TurnQueue)
            {
                if (turn is Player)
                {
                    if (turn.IsLiving)
                        turnedLivingList.Add(turn);
                }
                else
                    turn.Dispose();
            }
            this.TurnQueue.Clear();
            foreach (TurnedLiving turnedLiving in turnedLivingList)
                this.TurnQueue.Add(turnedLiving);
            if (this.m_map == null)
                return;
            foreach (Physics physics in this.m_map.GetAllPhysicalObjSafe())
                physics.Dispose();
        }

        public int GetTeamFightPower()
        {
            int num = 0;
            Player[] allPlayers = this.GetAllPlayers();
            foreach (Player player in allPlayers)
                num += player.PlayerDetail.PlayerCharacter.FightPower;
            return num / allPlayers.Length;
        }

        public SimpleNpc CreateNpc(int npcId, int x, int y, int type)
        {
            return this.CreateNpc(npcId, x, y, type, -1);
        }

        public SimpleNpc CreateNpc(int npcId, int x, int y, int type, int direction)
        {
            return this.CreateNpc(npcId, x, y, type, direction, 100, 0);
        }

        public SimpleNpc CreateNpc(
          int npcId,
          int x,
          int y,
          int type,
          int direction,
          int rank)
        {
            return this.CreateNpc(npcId, x, y, type, direction, 100, rank);
        }

        public SimpleNpc CreateNpc(
          int npcId,
          int x,
          int y,
          int type,
          int direction,
          bool fL)
        {
            SimpleNpc simpleNpc = new SimpleNpc(this.PhysicalId++, (BaseGame)this, NPCInfoMgr.GetNpcInfoById(npcId), type, direction);
            LivingConfig livingConfig = this.BaseLivingConfig();
            livingConfig.CanTakeDamage = true;
            livingConfig.IsTurn = false;
            simpleNpc.Config = livingConfig;
            simpleNpc.Reset();
            simpleNpc.SetXY(x, y);
            this.AddLiving((Living)simpleNpc);
            simpleNpc.StartMoving();
            return simpleNpc;
        }

        public SimpleNpc CreateNpc(
          int npcId,
          int x,
          int y,
          int type,
          int direction,
          int bloodInver,
          int rank)
        {
            NpcInfo npcInfoById = NPCInfoMgr.GetNpcInfoById(npcId);
            SimpleNpc simpleNpc = new SimpleNpc(this.PhysicalId++, (BaseGame)this, npcInfoById, type, direction, rank);
            simpleNpc.Config = this.BaseLivingConfig();
            simpleNpc.Reset();
            if (npcId != 23001)
                simpleNpc.Blood = simpleNpc.Blood / 100 * bloodInver;
            else
                simpleNpc.Blood = npcInfoById.Blood;
            simpleNpc.SetXY(x, y);
            if (rank != -1 && !this.m_NpcTurnQueue.ContainsKey(rank))
                this.m_NpcTurnQueue.Add(rank, this.m_pveGameDelay + rank * 2);
            this.AddLiving((Living)simpleNpc);
            simpleNpc.StartMoving();
            return simpleNpc;
        }

        public SimpleNpc CreateNpc(
          int npcId,
          int x,
          int y,
          int type,
          int direction,
          LivingConfig config)
        {
            NpcInfo npcInfoById = NPCInfoMgr.GetNpcInfoById(npcId);
            SimpleNpc simpleNpc = new SimpleNpc(this.PhysicalId++, (BaseGame)this, npcInfoById, type, direction);
            if (config != null)
                simpleNpc.Config = config;
            if (simpleNpc.Config.ReduceBloodStart > 1)
                simpleNpc.Blood = npcInfoById.Blood / simpleNpc.Config.ReduceBloodStart;
            else
                simpleNpc.Reset();
            simpleNpc.SetXY(x, y);
            this.AddLiving((Living)simpleNpc);
            simpleNpc.StartMoving();
            return simpleNpc;
        }

        public SimpleNpc CreateNpc(
          int npcId,
          int x,
          int y,
          int type,
          int direction,
          int bloodInver,
          int rank,
          LivingConfig config)
        {
            NpcInfo npcInfoById = NPCInfoMgr.GetNpcInfoById(npcId);
            SimpleNpc simpleNpc = new SimpleNpc(this.PhysicalId++, (BaseGame)this, npcInfoById, type, direction, rank);
            if (config != null)
                simpleNpc.Config = config;
            simpleNpc.Reset();
            simpleNpc.Blood = npcInfoById.Blood / 100 * bloodInver;
            simpleNpc.SetXY(x, y);
            if (rank != -1 && !this.m_NpcTurnQueue.ContainsKey(rank))
                this.m_NpcTurnQueue.Add(rank, this.m_pveGameDelay + rank * 2);
            this.AddLiving((Living)simpleNpc);
            simpleNpc.StartMoving();
            return simpleNpc;
        }

        public SimpleNpc CreateNpc(
          int npcId,
          int x,
          int y,
          int type,
          int direction,
          string action,
          LivingConfig config)
        {
            NpcInfo npcInfoById = NPCInfoMgr.GetNpcInfoById(npcId);
            SimpleNpc simpleNpc = new SimpleNpc(this.PhysicalId++, (BaseGame)this, npcInfoById, type, direction, action);
            if (config != null)
                simpleNpc.Config = config;
            if (simpleNpc.Config.ReduceBloodStart > 1)
                simpleNpc.Blood = npcInfoById.Blood / simpleNpc.Config.ReduceBloodStart;
            else
                simpleNpc.Reset();
            simpleNpc.SetXY(x, y);
            this.AddLiving((Living)simpleNpc);
            simpleNpc.StartMoving();
            return simpleNpc;
        }

        public LayerTop CreateLayerTop(
          int x,
          int y,
          string name,
          string model,
          string defaultAction,
          int scale,
          int rotation)
        {
            LayerTop layerTop = new LayerTop(this.PhysicalId++, name, model, defaultAction, scale, rotation);
            layerTop.SetXY(x, y);
            this.AddPhysicalObj((PhysicalObj)layerTop, true);
            return layerTop;
        }

        public Layer CreateLayerBoss(
          int x,
          int y,
          string name,
          string model,
          string defaultAction,
          int scale,
          int rotation)
        {
            Layer layer = new Layer(this.PhysicalId++, name, model, defaultAction, scale, rotation);
            layer.SetXY(x, y);
            this.AddPhysicalObj((PhysicalObj)layer, true);
            return layer;
        }

        private void SetNPCProperty(SimpleBoss boss)
        {
            if (this.PlayerCount > 1)
            {
                Player player1 = (Player)null;
                foreach (Player player2 in this.m_players.Values)
                {
                    if (player1 == null)
                        player1 = player2;
                    else if (player2.PlayerDetail.PlayerCharacter.FightPower > player1.PlayerDetail.PlayerCharacter.FightPower)
                        player1 = player2;
                    boss.MaxBlood += player2.Blood;
                }
                boss.Blood = boss.MaxBlood;
            }
            else
            {
                boss.MaxBlood = boss.MaxBlood * this.GetTeamFightPower() / 2;
                boss.Blood = boss.MaxBlood;
                boss.BaseDamage = (double)(int)(boss.BaseDamage * (double)this.GetTeamFightPower() / 10.0);
                boss.BaseGuard = (double)(int)(boss.BaseGuard * (double)this.GetTeamFightPower() / 10.0);
                boss.Defence = (double)(int)(boss.Defence * (double)this.GetTeamFightPower() / 2.0);
                boss.Lucky = (double)(int)(boss.Lucky * (double)this.GetTeamFightPower() / 200.0);
            }
        }

        public SimpleBoss CreateBoss(int npcId, int type)
        {
            NpcInfo npcInfoById = NPCInfoMgr.GetNpcInfoById(npcId);
            SimpleBoss boss = new SimpleBoss(this.PhysicalId++, (BaseGame)this, npcInfoById, -1, type, "");
            boss.Reset();
            int num1;
            int num2;
            switch (npcId)
            {
                case 5001:
                case 5002:
                    num1 = 1;
                    goto label_6;
                case 5003:
                    num1 = 1;
                    goto label_6;
                case 5004:
                    num2 = 1;
                    break;
                default:
                    num2 = 0;
                    break;
            }
            num1 = num2;
        label_6:
            if ((uint)num1 > 0U)
                this.SetNPCProperty(boss);
            Point playerPoint = this.GetPlayerPoint(this.mapPos, npcInfoById.Camp);
            boss.SetXY(playerPoint.X, playerPoint.Y);
            this.AddLiving((Living)boss);
            boss.StartMoving();
            return boss;
        }

        public SimpleBoss CreateBoss(
          int npcId,
          int x,
          int y,
          int direction,
          int type,
          string action,
          LivingConfig config)
        {
            NpcInfo npcInfoById = NPCInfoMgr.GetNpcInfoById(npcId);
            SimpleBoss simpleBoss = new SimpleBoss(this.PhysicalId++, (BaseGame)this, npcInfoById, direction, type, action);
            if (config != null)
            {
                simpleBoss.Config = new LivingConfig();
                simpleBoss.Config.Clone(config);
            }
            simpleBoss.Reset();
            if (simpleBoss.Config.ReduceBloodStart > 1)
                simpleBoss.Blood = npcInfoById.Blood / simpleBoss.Config.ReduceBloodStart;
            simpleBoss.SetXY(x, y);
            this.AddLiving((Living)simpleBoss);
            simpleBoss.StartMoving();
            return simpleBoss;
        }

        public SimpleBoss CreateBoss(
          int npcId,
          int x,
          int y,
          int direction,
          int type,
          string createAction)
        {
            return this.CreateBoss(npcId, x, y, direction, type, 100, createAction, this.BaseLivingConfig());
        }

        public SimpleBoss CreateBoss(int npcId, int x, int y, int direction, int type)
        {
            return this.CreateBoss(npcId, x, y, direction, type, 100, (string)null);
        }

        public SimpleBoss CreateBoss(
          int npcId,
          int x,
          int y,
          int direction,
          int type,
          int bloodInver)
        {
            return this.CreateBoss(npcId, x, y, direction, type, bloodInver, (string)null);
        }

        public SimpleBoss CreateBoss(
          int npcId,
          int x,
          int y,
          int direction,
          int type,
          int bloodInver,
          string createAction)
        {
            return this.CreateBoss(npcId, x, y, direction, type, bloodInver, createAction, this.BaseLivingConfig());
        }

        public SimpleBoss CreateBoss(
          int npcId,
          int x,
          int y,
          int direction,
          int type,
          int bloodInver,
          string action,
          LivingConfig config)
        {
            NpcInfo npcInfoById = NPCInfoMgr.GetNpcInfoById(npcId);
            SimpleBoss simpleBoss = new SimpleBoss(this.PhysicalId++, (BaseGame)this, npcInfoById, direction, type, action);
            if (config != null)
                simpleBoss.Config = config;
            if (simpleBoss.Config.ReduceBloodStart > 1)
            {
                simpleBoss.Blood = npcInfoById.Blood / simpleBoss.Config.ReduceBloodStart;
            }
            else
            {
                simpleBoss.Reset();
                if (bloodInver > 0 && bloodInver != 100)
                    simpleBoss.Blood = simpleBoss.Blood / 100 * bloodInver;
                if (simpleBoss.Config.isConsortiaBoss)
                    simpleBoss.Blood -= (int)this.AllWorldDameBoss;
            }
            simpleBoss.SetXY(x, y);
            this.AddLiving((Living)simpleBoss);
            simpleBoss.StartMoving();
            return simpleBoss;
        }

        public SimpleWingBoss CreateWingBoss(
          int npcId,
          int x,
          int y,
          int direction,
          int type)
        {
            return this.CreateWingBoss(npcId, x, y, direction, type, 100);
        }

        public SimpleWingBoss CreateWingBoss(
          int npcId,
          int x,
          int y,
          int direction,
          int type,
          int bloodInver)
        {
            SimpleWingBoss simpleWingBoss = new SimpleWingBoss(this.PhysicalId++, (BaseGame)this, NPCInfoMgr.GetNpcInfoById(npcId), direction, type);
            simpleWingBoss.Reset();
            simpleWingBoss.Blood = simpleWingBoss.Blood / 100 * bloodInver;
            simpleWingBoss.SetXY(x, y);
            this.AddLiving((Living)simpleWingBoss);
            return simpleWingBoss;
        }

        public Box CreateBox(int x, int y, string model, SqlDataProvider.Data.ItemInfo item, int subType = 1)
        {
            Box box = new Box(this.PhysicalId++, model, item, subType);
            box.SetXY(x, y);
            this.m_map.AddPhysical((Physics)box);
            this.AddBox(box, true);
            return box;
        }

        public PhysicalObj CreatePhysicalObj(
          int x,
          int y,
          string name,
          string model,
          string defaultAction,
          int scale,
          int rotation)
        {
            PhysicalObj phy = new PhysicalObj(this.PhysicalId++, name, model, defaultAction, scale, rotation, 0);
            phy.SetXY(x, y);
            this.AddPhysicalObj(phy, true);
            return phy;
        }

        public PhysicalObj CreatePhysicalObj(
          int x,
          int y,
          string name,
          string model,
          string defaultAction,
          int scale,
          int rotation,
          int typeEffect)
        {
            PhysicalObj phy = new PhysicalObj(this.PhysicalId++, name, model, defaultAction, scale, rotation, typeEffect);
            phy.SetXY(x, y);
            this.AddPhysicalObj(phy, true);
            return phy;
        }

        public Layer Createlayer(
          int x,
          int y,
          string name,
          string model,
          string defaultAction,
          int scale,
          int rotation)
        {
            Layer layer = new Layer(this.PhysicalId++, name, model, defaultAction, scale, rotation);
            layer.SetXY(x, y);
            this.AddPhysicalObj((PhysicalObj)layer, true);
            return layer;
        }

        public Layer Createlayer1(
          int x,
          int y,
          string name,
          string model,
          string defaultAction,
          int scale,
          int rotation)
        {
            Layer layer = new Layer(this.PhysicalId++, name, model, defaultAction, scale, rotation);
            layer.SetXY(x, y);
            this.AddPhysicalObj((PhysicalObj)layer, true);
            return layer;
        }

        public Layer Createlayerboss(
          int x,
          int y,
          string name,
          string model,
          string defaultAction,
          int scale,
          int rotation)
        {
            Layer layer = new Layer(this.PhysicalId++, name, model, defaultAction, scale, rotation);
            layer.SetXY(x, y);
            this.AddPhysicalObj((PhysicalObj)layer, true);
            return layer;
        }

        public Layer CreateTip(
          int x,
          int y,
          string name,
          string model,
          string defaultAction,
          int scale,
          int rotation)
        {
            Layer layer = new Layer(this.PhysicalId++, name, model, defaultAction, scale, rotation);
            layer.SetXY(x, y);
            this.AddPhysicalTip((PhysicalObj)layer, true);
            return layer;
        }

        public Ball CreateBall(
          int x,
          int y,
          string name,
          string defaultAction,
          int scale,
          int rotation)
        {
            Ball ball = new Ball(this.PhysicalId++, name, defaultAction, scale, rotation);
            ball.SetXY(x, y);
            this.AddPhysicalObj((PhysicalObj)ball, true);
            this.method_25(ball.Id, "pick", name);
            return ball;
        }

        public override void CheckState(int delay)
        {
            this.AddAction((IAction)new CheckPVEGameStateAction(delay));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            foreach (Physics living in this.m_livings)
                living.Dispose();
            try
            {
                this.m_missionAI.Dispose();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script m_missionAI.Dispose() error:{1}", (object)ex);
            }
            try
            {
                this.m_gameAI.Dispose();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script m_gameAI.Dispose() error:{1}", (object)ex);
            }
        }

        public void DoOther()
        {
            try
            {
                this.m_missionAI.DoOther();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script m_gameAI.DoOther() error:{1}", (object)ex);
            }
        }

        public string GetPicNextSession(int missionId, int sessionId, bool less)
        {
            if (!less)
                ++sessionId;
            switch (missionId)
            {
                case 1173:
                    return "show4.jpg";
                case 1273:
                    return "show4.jpg";
                case 1276:
                    return "show6.jpg";
                case 1375:
                    return "show5.jpg";
                case 1376:
                    return "show7.jpg";
                case 7001:
                    return "show.jpg";
                case 7101:
                    return "show3.jpg";
                case 7103:
                    return "show5.jpg";
                default:
                    return string.Format("show{0}.jpg", (object)sessionId);
            }
        }

        public void GameOver()
        {
            Console.WriteLine("teste"); //ok
            if (this.GameState != eGameState.Playing && this.GameState != eGameState.PrepareGameOver)
                return;
            this.m_gameState = eGameState.GameOver;
            this.SendUpdateUiData();
            try
            {
                this.m_missionAI.OnGameOver();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script {0} error:{1}", (object)this.GameState, (object)ex);
            }
            List<Player> allFightPlayers = this.GetAllFightPlayers();
            this.CurrentTurnTotalDamage = 0;
            this.m_bossCardCount = 1;
            this.TakeCardId = this.m_missionInfo.Id;
            bool flag = this.HasNextSession();
            if (!(this.IsWin & flag))
                this.m_bossCardCount = 0;
            if (!(!this.IsWin | flag) && !this.isTrainer())
                this.m_bossCardCount = 2;
            if (this.GameType == eGameType.FightLab && this.IsWin)
            {
                foreach (Player player in allFightPlayers)
                    player.PlayerDetail.SetFightLabPermission(this.m_info.ID, this.m_hardLevel, this.MissionInfo.Id);
            }
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)112);
            pkg.WriteInt(this.BossCardCount);
            if (!flag)
            {
                pkg.WriteBoolean(false);
                pkg.WriteBoolean(false);
            }
            else
            {
                pkg.WriteBoolean(true);
                pkg.WriteString(string.Format("show{0}.jpg", 1 + this.SessionId));
                pkg.WriteBoolean(true);
            }
            pkg.WriteInt(this.PlayerCount);
            foreach (Player p in allFightPlayers)
            {
                p.PlayerDetail.ClearFightBuffOneMatch();
                if (!this.IsWin && this.IsLabyrinth())
                    p.PlayerDetail.OutLabyrinth(this.IsWin);
                int experience = this.CalculateExperience(p);
                if (p.FightBuffers.ConsortionAddPercentGoldOrGP > 0)
                    experience += experience * p.FightBuffers.ConsortionAddPercentGoldOrGP / 100;
                int score = this.CalculateScore(p);
                this.m_missionAI.CalculateScoreGrade(p.TotalAllScore);
                p.CanTakeOut = this.BossCardCount;
                if (p.CurrentIsHitTarget)
                    ++p.TotalHitTargetCount;
                this.CalculateHitRate(p.TotalHitTargetCount, p.TotalShootCount);
                p.TotalAllHurt += p.TotalHurt;
                p.TotalAllCure += p.TotalCure;
                p.TotalAllHitTargetCount += p.TotalHitTargetCount;
                p.TotalAllShootCount += p.TotalShootCount;
                p.GainGP = p.PlayerDetail.AddGP(experience);
                p.TotalAllExperience += p.GainGP;
                p.TotalAllScore += score;
                p.BossCardCount = this.m_bossCardCount;
                pkg.WriteInt(p.PlayerDetail.PlayerCharacter.ID);
                pkg.WriteInt(p.PlayerDetail.PlayerCharacter.Grade);
                pkg.WriteInt(0);
                pkg.WriteInt(p.GainGP);
                pkg.WriteBoolean(this.IsWin);
                pkg.WriteInt(p.BossCardCount);
                pkg.WriteInt(0);
                pkg.WriteBoolean(false);
                pkg.WriteBoolean(false);
            }
            if (this.BossCardCount > 0)
            {
                pkg.WriteInt(this.m_gameOverResources.Count);
                foreach (string gameOverResource in this.m_gameOverResources)
                    pkg.WriteString(gameOverResource);
            }
            this.SendToAll(pkg);
            StringBuilder stringBuilder1 = new StringBuilder();
            foreach (Player player in allFightPlayers)
            {
                stringBuilder1.Append(player.PlayerDetail.PlayerCharacter.ID).Append(",");
                player.Ready = false;
                player.Reset(); // this ??
                player.PlayerDetail.OnMissionOver((AbstractGame)player.Game, this.IsWin, this.MissionInfo.Id, player.TurnNum);
            }
            int _winTeam = this.IsWin ? 1 : 2;
            string _teamA = stringBuilder1.ToString();
            string _teamB = "";
            string _playResult = "";
            if (!this.IsWin)
                this.OnGameStopped();
            StringBuilder stringBuilder2 = new StringBuilder();
            if (this.IsWin && this.IsBossWar != "")
            {
                stringBuilder2.Append(this.IsBossWar).Append(",");
                foreach (Player player in allFightPlayers)
                {
                    stringBuilder2.Append("PlayerCharacter ID: ").Append(player.PlayerDetail.PlayerCharacter.ID).Append(",");
                    stringBuilder2.Append("Grade: ").Append(player.PlayerDetail.PlayerCharacter.Grade).Append(",");
                    stringBuilder2.Append("TurnNum): ").Append(player.TurnNum).Append(",");
                    stringBuilder2.Append("Attack: ").Append(player.PlayerDetail.PlayerCharacter.Attack).Append(",");
                    stringBuilder2.Append("Defence: ").Append(player.PlayerDetail.PlayerCharacter.Defence).Append(",");
                    stringBuilder2.Append("Agility: ").Append(player.PlayerDetail.PlayerCharacter.Agility).Append(",");
                    stringBuilder2.Append("Luck: ").Append(player.PlayerDetail.PlayerCharacter.Luck).Append(",");
                    stringBuilder2.Append("BaseAttack: ").Append(player.PlayerDetail.GetBaseAttack()).Append(",");
                    stringBuilder2.Append("MaxBlood: ").Append(player.MaxBlood).Append(",");
                    stringBuilder2.Append("BaseDefence: ").Append(player.PlayerDetail.GetBaseDefence()).Append(",");
                    if (player.PlayerDetail.SecondWeapon != null)
                    {
                        stringBuilder2.Append("SecondWeapon TemplateID: ").Append(player.PlayerDetail.SecondWeapon.TemplateID).Append(",");
                        stringBuilder2.Append("SecondWeapon StrengthenLevel: ").Append(player.PlayerDetail.SecondWeapon.StrengthenLevel).Append(".");
                    }
                }
            }
            this.BossWarField = stringBuilder2.ToString();
            this.OnGameOverLog(this.RoomId, this.RoomType, this.GameType, 0, this.beginTime, DateTime.Now, this.BeginPlayersCount, this.MissionInfo.Id, _teamA, _teamB, _playResult, _winTeam, this.BossWarField);
            this.OnGameOverred();
        }

        public bool IsLabyrinth()
        {
            return this.RoomType == eRoomType.Labyrinth;
        }

        public void PrepareFightingLivings()
        {
            if (this.GameState != eGameState.GameStart)
                return;
            this.m_gameState = eGameState.Playing;
            this.SendSyncLifeTime();
            this.WaitTime(this.PlayerCount * 1000);
            try
            {
                this.m_missionAI.OnPrepareNewGame();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script {0} error:{1}", (object)this.GameState, (object)ex);
            }
        }

        public void GameOverAllSession()//Cartas Das Estancias 
        {
            if (this.GameState != eGameState.GameOver)
                return;
            this.m_gameState = eGameState.ALLSessionStopped;
            try
            {
                this.m_gameAI.OnGameOverAllSession();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script {0} error:{1}", (object)this.GameState, (object)ex);
            }
            List<Player> allFightPlayers = this.GetAllFightPlayers();
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)115);
            foreach (Player player in allFightPlayers)
                player.PlayerDetail.OnGameOver((AbstractGame)this, this.IsWin, player.GainGP, false, this.CoupleFight(player), player.Blood, this.PlayerCount);
            int num1 = 1;
            
            if (!this.IsWin)
            {
                num1 = 0;
            }
            else if (RoomType == eRoomType.Dungeon || RoomType == eRoomType.Boss)
            {
                num1 = 2;

            }
            pkg.WriteInt(this.PlayerCount);
            
           
;            foreach (Player player in allFightPlayers)
                
            {
                              
                player.CanTakeOut = num1;
                pkg.WriteInt(player.PlayerDetail.PlayerCharacter.ID);
                pkg.WriteInt(player.TotalAllKill);
                pkg.WriteInt(player.TotalAllHurt);
                pkg.WriteInt(player.TotalAllScore);
                pkg.WriteInt(player.TotalAllCure);
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                pkg.WriteInt(player.TotalAllExperience);
                pkg.WriteBoolean(this.IsWin);
            }
            pkg.WriteInt(this.m_gameOverResources.Count);
            foreach (string gameOverResource in this.m_gameOverResources)
                pkg.WriteString(gameOverResource);
            this.SendToAll(pkg);
            this.WaitTime(25000);
            this.CanStopGame();
        }

        private bool LabyrinthAward(string string_1)
        {
            bool flag = false;
            if (string_1.Length > 0)
            {
                string str1 = string_1;
                char[] chArray = new char[1] { '-' };
                foreach (string str2 in str1.Split(chArray))
                {
                    if (str2 == this.SessionId.ToString())
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }

        public void GameOverMovie()
        {
            if (this.GameState != eGameState.Playing)
                return;
            this.m_gameState = eGameState.GameOver;
            this.ClearWaitTimer();
            this.ClearDiedPhysicals();
            List<Player> allFightPlayers = this.GetAllFightPlayers();
            foreach (Player player in allFightPlayers)
            {
                if (this.LabyrinthAward(player.PlayerDetail.ProcessLabyrinthAward))
                    player.PlayerDetail.UpdateLabyrinth(this.SessionId, this.m_missionInfo.Id, false);
            }
            try
            {
                this.m_missionAI.OnGameOverMovie();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script {0} error:{1}", (object)this.GameState, (object)ex);
            }
            bool val = this.HasNextSession();
            if (!val)
            {
                GSPacketIn pkg = new GSPacketIn((short)91);
                pkg.WriteByte((byte)112);
                pkg.WriteInt(0);
                pkg.WriteBoolean(val);
                pkg.WriteBoolean(false);
                pkg.WriteInt(this.PlayerCount);
                foreach (Player p in allFightPlayers)
                {
                    if (this.IsLabyrinth())
                        p.PlayerDetail.OutLabyrinth(this.IsWin);
                    p.PlayerDetail.ClearFightBuffOneMatch();
                    int experience = this.CalculateExperience(p);
                    int score = this.CalculateScore(p);
                    this.m_missionAI.CalculateScoreGrade(p.TotalAllScore);
                    if (p.CurrentIsHitTarget)
                        ++p.TotalHitTargetCount;
                    this.CalculateHitRate(p.TotalHitTargetCount, p.TotalShootCount);
                    p.TotalAllHurt += p.TotalHurt;
                    p.TotalAllCure += p.TotalCure;
                    p.TotalAllHitTargetCount += p.TotalHitTargetCount;
                    p.TotalAllShootCount += p.TotalShootCount;
                    p.GainGP = p.PlayerDetail.AddGP(experience);
                    p.TotalAllExperience += p.GainGP;
                    p.TotalAllScore += score;
                    p.BossCardCount = this.BossCardCount;
                    pkg.WriteInt(p.PlayerDetail.PlayerCharacter.ID);
                    pkg.WriteInt(p.PlayerDetail.PlayerCharacter.Grade);
                    pkg.WriteInt(0);
                    pkg.WriteInt(p.GainGP);
                    pkg.WriteBoolean(this.IsWin);
                    pkg.WriteInt(this.BossCardCount);
                    pkg.WriteInt(p.BossCardCount);
                    pkg.WriteBoolean(false);
                    pkg.WriteBoolean(false);
                }
                if (this.BossCardCount > 0)
                {
                    pkg.WriteInt(this.m_gameOverResources.Count);
                    foreach (string gameOverResource in this.m_gameOverResources)
                        pkg.WriteString(gameOverResource);
                }
                this.SendToAll(pkg);
                this.OnGameStopped();
                this.OnGameOverred();
            }
            else
            {
                foreach (Physics physics in this.m_map.GetAllPhysicalSafe())
                    physics.PrepareNewTurn();
                this.m_currentLiving = this.FindNextTurnedLiving();
                if (this.m_currentLiving != null && this.CanEnterGate)
                {
                    ++this.m_turnIndex;
                    this.m_currentLiving.PrepareSelfTurn();
                    this.SendGameNextTurn((Living)this.m_currentLiving, (BaseGame)this, new List<Box>());
                    this.CanEnterGate = false;
                    this.CanShowBigBox = false;
                    this.EnterNextFloor();
                }
                this.OnBeginNewTurn();
                try
                {
                    this.m_missionAI.OnBeginNewTurn();
                }
                catch (Exception ex)
                {
                    BaseGame.log.ErrorFormat("game ai script {0} error:{1}", (object)this.GameState, (object)ex);
                }
            }
        }

        public void EnterNextFloor()
        {
            int foregroundWidth = this.Map.Info.ForegroundWidth;
            Player randomPlayer = this.FindRandomPlayer();
            int num = 150;
            int x1 = randomPlayer.X;
            int y = randomPlayer.Y;
            int x2 = x1 + 150 <= foregroundWidth ? x1 + num : x1 - num;
            Point point = this.m_map.FindYLineNotEmptyPointDown(x2, y);
            if (point == Point.Empty)
                point = new Point(x2, this.Map.Bound.Height + 1);
            this.CreateTransmissionGate(point.X, point.Y - 60, "transmitted", "asset.game.transmitted", "out", 1, 1);
        }

        public TransmissionGate CreateTransmissionGate(
          int x,
          int y,
          string name,
          string model,
          string defaultAction,
          int scale,
          int rotation)
        {
            TransmissionGate transmissionGate = new TransmissionGate(this.PhysicalId++, name, model, defaultAction, scale, rotation);
            transmissionGate.SetXY(x, y);
            this.AddPhysicalObj((PhysicalObj)transmissionGate, true);
            return transmissionGate;
        }

        public string GetMissionIdStr(string missionIds, int randomCount)
        {
            if (string.IsNullOrEmpty(missionIds))
                return "";
            string[] strArray = missionIds.Split(',');
            if (strArray.Length < randomCount)
                return "";
            List<string> stringList = new List<string>();
            int length = strArray.Length;
            int num = 0;
            while (num < randomCount)
            {
                int index = this.Random.Next(length);
                string str = strArray[index];
                if (!stringList.Contains(str))
                {
                    stringList.Add(str);
                    ++num;
                }
            }
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string str in stringList)
                stringBuilder.Append(str).Append(",");
            return stringBuilder.Remove(stringBuilder.Length - 1, 1).ToString();
        }

        public new SimpleNpc[] GetNPCLivingWithID(int id)
        {
            List<SimpleNpc> simpleNpcList = new List<SimpleNpc>();
            foreach (Living living in this.m_livings)
            {
                if (living is SimpleNpc && living.IsLiving && (living as SimpleNpc).NpcInfo.ID == id)
                    simpleNpcList.Add(living as SimpleNpc);
            }
            return simpleNpcList.ToArray();
        }

        private string GetScript(PveInfo pveInfo, eHardLevel hardLevel)
        {
            switch (hardLevel)
            {
                case eHardLevel.Easy:
                    return pveInfo.SimpleGameScript;
                case eHardLevel.Normal:
                    return pveInfo.NormalGameScript;
                case eHardLevel.Hard:
                    return pveInfo.HardGameScript;
                case eHardLevel.Terror:
                    return pveInfo.TerrorGameScript;
                case eHardLevel.Epic:
                    return pveInfo.EpicGameScript;
                default:
                    return pveInfo.SimpleGameScript;
            }
        }

        public bool HasNextSession()
        {
            return this.PlayerCount != 0 && this.IsWin && this.Misssions.ContainsKey(this.SessionId + 1);
        }

        public bool IsAllReady()
        {
            foreach (Player player in this.Players.Values)
            {
                if (!player.Ready)
                    return false;
            }
            return true;
        }

        public bool isDragonLair()
        {
            if (this.Misssions.ContainsKey(1 + this.SessionId))
                return this.m_info.ID == 5;
            return false;
        }

        private void living_Died(Living living)
        {
            if (this.CurrentLiving != null && this.CurrentLiving is Player && (!(living is Player) && living != this.CurrentLiving && living.Config.CanCountKill))
            {
                if (this.RoomType != eRoomType.FightLab)
                    ++this.TotalKillCount;
                this.TotalNpcExperience += (double)living.Experience;
                this.TotalNpcGrade += (double)living.Grade;
            }
            if (living is SimpleBoss)
            {
                ((SimpleBoss)living).DiedEvent();
                ((SimpleBoss)living).DiedSay();
            }
            if (living is SimpleNpc)
            {
                ((SimpleNpc)living).DiedEvent();
                ((SimpleNpc)living).DiedSay();
                ((SimpleNpc)living).OnDie();
            }
            if (living is Player && this.CurrentLiving is SimpleBoss)
                ((SimpleBoss)this.CurrentLiving).KillPlayerSay();
            this.m_missionAI.OnDied();
        }

        public void LivingRandSay()
        {
            if (this.m_livings == null || this.m_livings.Count == 0)
                return;
            int count = this.m_livings.Count;
            foreach (Living living in this.m_livings)
                living.IsSay = false;
            if (this.TurnIndex % 2 == 0)
                return;
            int num1 = count > 5 ? (count <= 5 || count > 10 ? this.Random.Next(1, 4) : this.Random.Next(1, 3)) : this.Random.Next(0, 2);
            if (num1 <= 0)
                return;
            int num2 = 0;
            while (num2 < num1)
            {
                int index = this.Random.Next(0, count);
                if (!this.m_livings[index].IsSay)
                {
                    this.m_livings[index].IsSay = true;
                    ++num2;
                }
            }
        }

        public void LoadNpcGameOverResources(int[] npcIds)
        {
            if (npcIds == null || npcIds.Length == 0)
                return;
            for (int index = 0; index < npcIds.Length; ++index)
            {
                NpcInfo npcInfoById = NPCInfoMgr.GetNpcInfoById(npcIds[index]);
                if (npcInfoById == null)
                    BaseGame.log.Error((object)"LoadGameOverResources npcInfo resoure is not exits");
                else
                    this.m_gameOverResources.Add(npcInfoById.ModelID);
            }
        }

        public void LoadResources(int[] npcIds)
        {
            if (npcIds == null || npcIds.Length == 0)
                return;
            for (int index = 0; index < npcIds.Length; ++index)
            {
                NpcInfo npcInfoById = NPCInfoMgr.GetNpcInfoById(npcIds[index]);
                if (npcInfoById == null)
                    BaseGame.log.Error((object)"LoadResources npcInfo resoure is not exits");
                else
                    this.AddLoadingFile(2, npcInfoById.ResourcesPath, npcInfoById.ModelID);
            }
        }

        internal void method_52()
        {
            try
            {
                this.m_missionAI.OnShooted();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script m_gameAI.OnShooted() error:{0}", (object)ex);
            }
        }

        public override void MinusDelays(int lowestDelay)
        {
            foreach (int index in this.m_NpcTurnQueue.Keys.ToArray<int>())
                this.m_NpcTurnQueue[index] = this.m_NpcTurnQueue[index] - lowestDelay > 0 ? this.m_NpcTurnQueue[index] - lowestDelay : 0;
            base.MinusDelays(lowestDelay);
        }

        public override void MissionStart(IGamePlayer host)
        {
            if (this.GameState != eGameState.SessionPrepared && this.GameState != eGameState.GameOver)
                return;
            foreach (Player player in this.Players.Values)
                player.Ready = true;
            this.CheckState(0);
        }

        public void ChangeMissionDelay(int rankID, int delay)
        {
            if (!this.m_NpcTurnQueue.ContainsKey(rankID))
                return;
            this.m_NpcTurnQueue[rankID] = delay;
        }

        public void ConfigLivingSayRule()
        {
            if (this.m_livings == null || this.m_livings.Count == 0)
                return;
            int count = this.m_livings.Count;
            foreach (Living living in this.m_livings)
                living.IsSay = false;
            if (this.TurnIndex % 2 == 0)
                return;
            int length = count > 5 ? (count <= 5 || count > 10 ? this.Random.Next(1, 4) : this.Random.Next(1, 3)) : this.Random.Next(0, 2);
            if (length <= 0)
                return;
            int[] numArray = new int[length];
            int num = 0;
            while (num < length)
            {
                int index = this.Random.Next(0, count);
                if (!this.m_livings[index].IsSay)
                {
                    this.m_livings[index].IsSay = true;
                    ++num;
                }
            }
        }

        public int FindTurnNpcRank()
        {
            int num1 = int.MaxValue;
            int num2 = 0;
            foreach (int key in this.m_NpcTurnQueue.Keys)
            {
                if (this.m_NpcTurnQueue[key] < num1)
                {
                    num1 = this.m_NpcTurnQueue[key];
                    num2 = key;
                }
            }
            return num2;
        }

        public void NextTurn()
        {
            if (this.GameState != eGameState.Playing)
                return;
            this.IsPassDrama = false;
            this.ClearWaitTimer();
            this.ClearDiedPhysicals();
            this.CheckBox();
            this.ConfigLivingSayRule();
            string empty = string.Empty;
            List<Box> box = this.CreateBox();
            foreach (Physics physics in this.m_map.GetAllPhysicalSafe())
                physics.PrepareNewTurn();
            this.LastTurnLiving = this.m_currentLiving;
            this.m_currentLiving = this.FindNextTurnedLiving();
            try
            {
                this.m_missionAI.OnNewTurnStarted();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script {0} error:{1}", (object)this.GameState, (object)ex);
            }
            if (this.m_currentLiving != null)
            {
                ++this.m_turnIndex;
                this.SendUpdateUiData();
                List<Living> livedLivingsHadTurn = this.GetLivedLivingsHadTurn();
                int turnNpcRank = this.FindTurnNpcRank();
                if (livedLivingsHadTurn.Count > 0 && this.m_NpcTurnQueue.Count > 0 && this.m_currentLiving.Delay > this.m_NpcTurnQueue[turnNpcRank])
                {
                    this.MinusDelays(this.m_NpcTurnQueue[turnNpcRank]);
                    bool flag = false;
                    foreach (Living living in this.m_livings)
                    {
                        if (((SimpleNpc)living).Rank == turnNpcRank)
                        {
                            living.PrepareSelfTurn();
                            if (!living.IsFrost)
                                living.StartAttacking();
                            if (!flag)
                            {
                                this.SendGameNextTurn(living, (BaseGame)this, box);
                                flag = true;
                            }
                        }
                    }
                    foreach (Living living in this.m_livings)
                    {
                        if (((SimpleNpc)living).Rank == turnNpcRank && living.IsAttacking)
                            living.StopAttacking();
                    }
                    Dictionary<int, int> npcTurnQueue;
                    int index;
                    (npcTurnQueue = this.m_NpcTurnQueue)[index = turnNpcRank] = npcTurnQueue[index] + this.MissionInfo.IncrementDelay;
                    this.CheckState(0);
                }
                else
                {
                    if (this.CanShowBigBox)
                    {
                        this.ShowBigBox();
                        this.CanEnterGate = true;
                    }
                    this.MinusDelays(this.m_currentLiving.Delay);
                    this.UpdateWind(this.GetNextWind(), false);
                    this.m_currentLiving.PrepareSelfTurn();
                    if (this.m_currentLiving.IsLiving && !this.m_currentLiving.IsFrost && !this.m_currentLiving.BlockTurn)
                    {
                        this.m_currentLiving.StartAttacking();
                        this.SendSyncLifeTime();
                        this.SendGameNextTurn((Living)this.m_currentLiving, (BaseGame)this, box);
                        if (this.m_currentLiving.IsAttacking)
                            this.AddAction((IAction)new WaitLivingAttackingAction(this.m_currentLiving, this.m_turnIndex, (this.GetTurnWaitTime() + 28) * 1000));
                    }
                }
            }
            this.OnBeginNewTurn();
            try
            {
                this.m_missionAI.OnBeginNewTurn();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script {0} error:{1}", (object)this.GameState, (object)ex);
            }
        }

        public void ShowBigBox()
        {
            List<SqlDataProvider.Data.ItemInfo> info = (List<SqlDataProvider.Data.ItemInfo>)null;
            DropInventory.CopyDrop(this.m_missionInfo.Id, this.SessionId, ref info);
            List<int> BhV2sgMkjBpUnK7WcH = new List<int>();
            if (info != null)
            {
                foreach (SqlDataProvider.Data.ItemInfo itemInfo in info)
                    BhV2sgMkjBpUnK7WcH.Add(itemInfo.TemplateID);
            }
            foreach (Player allFightPlayer in this.GetAllFightPlayers())
            {
                if (this.LabyrinthAward(allFightPlayer.PlayerDetail.ProcessLabyrinthAward))
                {
                    this.method_34((Living)allFightPlayer, BhV2sgMkjBpUnK7WcH);
                    allFightPlayer.PlayerDetail.UpdateLabyrinth(this.SessionId, this.m_missionInfo.Id, true);
                }
            }
        }

        internal void OnShooted()
        {
            try
            {
                this.m_missionAI.OnShooted();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script m_gameAI.OnShooted() error:{1}", (object)ex);
            }
        }

        public void Prepare()
        {
            if ((uint)this.GameState > 0U)
                return;
            this.m_gameState = eGameState.Prepared;
            this.SendCreateGame();
            this.CheckState(0);
            try
            {
                this.m_gameAI.OnPrepated();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script {0} error:{1}", (object)this.GameState, (object)ex);
            }
        }

        public void PrepareNewGame()
        {
            if (this.GameState != eGameState.GameStart)
                return;
            this.m_gameState = eGameState.Playing;
            this.BossCardCount = 0;
            this.SendSyncLifeTime();
            this.WaitTime(this.PlayerCount * 1000);
            try
            {
                this.m_missionAI.OnPrepareNewGame();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script {0} error:{1}", (object)this.GameState, (object)ex);
            }
            try
            {
                this.m_missionAI.OnStartGame();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script {0} error:{1}", (object)this.GameState, (object)ex);
            }
        }

        public void PrepareGameOver()
        {
            if (this.GameState != eGameState.Playing)
                return;
            this.m_gameState = eGameState.PrepareGameOver;
            try
            {
                this.m_missionAI.OnPrepareGameOver();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script {0} error:{1}", (object)this.GameState, (object)ex);
            }
        }

        public void PrepareNewSession()
        {
            if (this.GameState != eGameState.Prepared && this.GameState != eGameState.GameOver && this.GameState != eGameState.ALLSessionStopped)
                return;
            this.m_gameState = eGameState.SessionPrepared;
            ++this.SessionId;
            this.ClearLoadingFiles();
            this.ClearMissionData();
            this.m_gameOverResources.Clear();
            this.WantTryAgain = 0;
            this.m_missionInfo = this.Misssions[this.SessionId];
            this.m_pveGameDelay = this.m_missionInfo.Delay;
            this.TotalCount = this.m_missionInfo.TotalCount;
            this.TotalTurn = this.m_missionInfo.TotalTurn;
            this.Param1 = this.m_missionInfo.Param1;
            this.Param2 = this.m_missionInfo.Param2;
            this.Param3 = -1;
            this.Param4 = -1;
            this.Pic = string.Format("show{0}.jpg", this.SessionId);
            foreach (Player player in this.m_players.Values)
                player.MissionEventHandle -= new PlayerMissionEventHandle(this.m_missionAI.OnMissionEvent);
            this.m_missionAI = ScriptMgr.CreateInstance(this.m_missionInfo.Script) as AMissionControl;
            foreach (Player player in this.m_players.Values)
                player.MissionEventHandle += new PlayerMissionEventHandle(this.m_missionAI.OnMissionEvent);
            if (this.m_missionAI == null)
            {
                BaseGame.log.ErrorFormat("Can't create game mission ai :{0}", (object)this.m_missionInfo.Script);
                this.m_missionAI = (AMissionControl)SimpleMissionControl.Simple;
            }
            this.m_missionAI.Game = this;
            try
            {
                this.m_missionAI.OnPrepareNewSession();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script {0} error:{1}", (object)this.GameState, (object)ex);
            }
        }

        public void Print(string str)
        {
            Console.WriteLine(str);
        }

        public override Player RemovePlayer(IGamePlayer gp, bool isKick)
        {
            Player player = this.GetPlayer(gp);
            if (player != null)
            {
                player.PlayerDetail.RemoveGP(gp.PlayerCharacter.Grade * 12);
                player.PlayerDetail.ClearFightBuffOneMatch();
                string msg = (string)null;
                if (player.IsLiving && (this.GameState == eGameState.GameStart || this.GameState == eGameState.Playing))
                {
                    string translation1 = LanguageMgr.GetTranslation("AbstractPacketLib.SendGamePlayerLeave.Msg4", (object)(gp.PlayerCharacter.Grade * 12));
                    string translation2 = LanguageMgr.GetTranslation("AbstractPacketLib.SendGamePlayerLeave.Msg5", (object)gp.PlayerCharacter.NickName, (object)(gp.PlayerCharacter.Grade * 12));
                    this.SendMessage(gp, translation1, translation2, 3);
                }
                else
                {
                    string translation = LanguageMgr.GetTranslation("AbstractPacketLib.SendGamePlayerLeave.Msg1", (object)gp.PlayerCharacter.NickName);
                    this.SendMessage(gp, msg, translation, 3);
                }
                base.RemovePlayer(gp, isKick);
            }
            return player;
        }

        internal void SendAddPhysicalObj(PhysicalObj obj, int layer)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.Parameter1 = obj.Id;
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
            pkg.WriteInt(layer);
            this.SendToAll(pkg);
        }

        private void SendCreateGameToSingle(PVEGame game, IGamePlayer gamePlayer)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)121);
            pkg.WriteInt(game.Map.Info.ID);
            pkg.WriteInt((int)(byte)game.RoomType);
            pkg.WriteInt((int)(byte)game.GameType);
            pkg.WriteInt(game.TimeType);
            List<Player> allFightPlayers = game.GetAllFightPlayers();
            pkg.WriteInt(allFightPlayers.Count);
            foreach (Player player in allFightPlayers)
            {
                IGamePlayer playerDetail = player.PlayerDetail;
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
                    pkg.WriteInt(playerDetail.MainWeapon.Template.RefineryLevel);
                    pkg.WriteString(playerDetail.MainWeapon.Template.Name);
                    pkg.WriteDateTime(DateTime.MinValue);
                }
                if (playerDetail.SecondWeapon == null)
                    pkg.WriteInt(0);
                else
                    pkg.WriteInt(playerDetail.SecondWeapon.TemplateID);
                pkg.WriteInt(playerDetail.PlayerCharacter.ConsortiaID);
                pkg.WriteString(playerDetail.PlayerCharacter.ConsortiaName);
                pkg.WriteInt(playerDetail.PlayerCharacter.badgeID);
                pkg.WriteInt(playerDetail.PlayerCharacter.ConsortiaLevel);
                pkg.WriteInt(playerDetail.PlayerCharacter.ConsortiaRepute);
                pkg.WriteBoolean(false);
                pkg.WriteInt(0);
                pkg.WriteInt(player.Team);
                pkg.WriteInt(player.Id);
                pkg.WriteInt(player.MaxBlood);
                pkg.WriteBoolean(player.Ready);
            }
            int sessionId = game.SessionId;
            MissionInfo misssion = game.Misssions[sessionId];
            pkg.WriteString(misssion.Name);
            pkg.WriteString(string.Format("show{0}.jpg", (object)sessionId));
            pkg.WriteString(misssion.Success);
            pkg.WriteString(misssion.Failure);
            pkg.WriteString(misssion.Description);
            pkg.WriteInt(game.TotalMissionCount);
            pkg.WriteInt(sessionId);
            gamePlayer.SendTCP(pkg);
        }

        public bool CanCampAttack(Living beatLiving, Living killLiving)
        {
            bool flag = false;
            int livingCamp1 = this.GetLivingCamp(beatLiving);
            int livingCamp2 = this.GetLivingCamp(killLiving);
            if (livingCamp1 == 0 || livingCamp1 == 1 || livingCamp1 == 3)
                flag = livingCamp2 == 0 || livingCamp2 == 1 || livingCamp2 == 2;
            if (livingCamp1 == 2)
                flag = livingCamp2 != 2 && livingCamp2 != 4;
            return flag;
        }

        public void SendFreeFocus(int x, int y, int type, int delay, int finishTime)
        {
            this.AddAction((IAction)new FocusFreeAction(x, y, type, delay, finishTime));
        }

        public void SendGameFocus(Physics p, int delay, int finishTime)
        {
            this.AddAction((IAction)new FocusAction(p.X, p.Y, 1, delay, finishTime));
        }

        public void SendGameFocus(int x, int y, int type, int delay, int finishTime)
        {
            this.Createlayer(x, y, "pic", "", "", 1, 0);
            this.SendGameObjectFocus(1, "pic", delay, finishTime);
        }

        public void SendGameObjectFocus(int type, string name, int delay, int finishTime)
        {
            foreach (Physics physics in (Physics[])this.FindPhysicalObjByName(name))
                this.AddAction((IAction)new FocusAction(physics.X, physics.Y, type, delay, finishTime));
        }

        public void SendHideBlood(Living living, int hide)
        {
            this.PedSuikAov(living, hide);
        }

        public void SendLivingActionMapping(Living liv, string source, string value)
        {
            if (liv == null)
                return;
            this.method_25(liv.Id, source, value);
        }

        public void SendLoadResource(List<LoadingFileInfo> loadingFileInfos)
        {
            if (loadingFileInfos == null || loadingFileInfos.Count <= 0)
                return;
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)67);
            pkg.WriteInt(loadingFileInfos.Count);
            foreach (LoadingFileInfo loadingFileInfo in loadingFileInfos)
            {
                pkg.WriteInt(loadingFileInfo.Type);
                pkg.WriteString(loadingFileInfo.Path);
                pkg.WriteString(loadingFileInfo.ClassName);
            }
            this.SendToAll(pkg);
        }

        public void SendMissionInfo()
        {
            if (this.m_missionInfo == null)
                return;
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)113);
            pkg.WriteInt(this.m_missionInfo.Id);
            pkg.WriteString(this.m_missionInfo.Name);
            pkg.WriteString(this.m_missionInfo.Success);
            pkg.WriteString(this.m_missionInfo.Failure);
            pkg.WriteString(this.m_missionInfo.Description);
            pkg.WriteString(this.m_missionInfo.Title);
            pkg.WriteInt(this.TotalMissionCount);
            pkg.WriteInt(this.SessionId);
            pkg.WriteInt(this.TotalTurn);
            pkg.WriteInt(this.TotalCount);
            pkg.WriteInt(this.Param1);
            pkg.WriteInt(this.Param2);
            pkg.WriteInt(this.WantTryAgain);
            pkg.WriteString(this.Pic);
            this.SendToAll(pkg);
        }

        public void SendObjectFocus(Physics m_helper, int p1, int p2, int p3)
        {
            this.AddAction((IAction)new FocusAction(m_helper.X, m_helper.Y, p1, p2, p3));
        }

        public void SendPlayerInfoInGame(PVEGame game, IGamePlayer gp, Player p)
        {
            GSPacketIn pkg = new GSPacketIn((short)91)
            {
                Parameter2 = this.LifeTime
            };
            pkg.WriteByte((byte)120);
            pkg.WriteInt(gp.ZoneId);
            pkg.WriteInt(gp.PlayerCharacter.ID);
            pkg.WriteInt(p.Team);
            pkg.WriteInt(p.Id);
            pkg.WriteInt(p.MaxBlood);
            pkg.WriteBoolean(p.Ready);
            game.SendToAll(pkg);
        }

        public void SendPlayersPicture(Living living, int type, bool state)
        {
            this.method_47(living, type, state);
        }

        public void SendPlaySound(string playStr)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)63);
            pkg.WriteString(playStr);
            this.SendToAll(pkg);
        }

        internal void SendShowCards()
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)89);
            int val1 = 0;
            List<int> intList = new List<int>();
            for (int index = 0; index < this.Cards.Length; ++index)
            {
                if (this.Cards[index] == 0)
                {
                    intList.Add(index);
                    ++val1;
                }
            }
            int val2 = 0;
            int val3 = 0;
            pkg.WriteInt(val1);
            int id = this.m_missionInfo.Id;
            foreach (int num in intList)
            {
                List<SqlDataProvider.Data.ItemInfo> itemInfoList = DropInventory.CopySystemDrop(id, intList.Count);
                if (itemInfoList != null)
                {
                    foreach (SqlDataProvider.Data.ItemInfo itemInfo in itemInfoList)
                    {
                        val2 = itemInfo.TemplateID;
                        val3 = itemInfo.Count;
                    }
                }
                pkg.WriteByte((byte)num);
                pkg.WriteInt(val2);
                pkg.WriteInt(val3);
            }
            this.SendToAll(pkg);
        }

        public void SendUpdateUiData()
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)104);
            int val = 0;
            try
            {
                val = this.m_missionAI.UpdateUIData();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script {0} error:{1}", (object)string.Format("m_missionAI.UpdateUIData()"), (object)ex);
            }
            pkg.WriteInt(this.Param1 != -1 ? this.Param1 : this.TurnIndex);
            pkg.WriteInt(val);
            pkg.WriteInt(this.Param3);
            pkg.WriteInt(this.Param4);
            this.SendToAll(pkg);
        }

        public void SetupMissions(string missionIds)
        {
            if (string.IsNullOrEmpty(missionIds))
                return;
            int key = 0;
            string str = missionIds;
            char[] chArray = new char[1] { ',' };
            foreach (string s in str.Split(chArray))
            {
                ++key;
                MissionInfo missionInfo = MissionInfoMgr.GetMissionInfo(int.Parse(s));
                this.Misssions.Add(key, missionInfo);
            }
        }

        public void ShowDragonLairCard()
        {
            if (this.GameState != eGameState.ALLSessionStopped || !this.IsWin)
                return;
            foreach (Player allFightPlayer in this.GetAllFightPlayers())
            {
                if (allFightPlayer.IsActive && allFightPlayer.CanTakeOut > 0)
                {
                    allFightPlayer.HasPaymentTakeCard = true;
                    int canTakeOut = allFightPlayer.CanTakeOut;
                    for (int index = 0; index < canTakeOut; ++index)
                        this.TakeCard(allFightPlayer);
                }
            }
            this.SendShowCards();
        }

        public void CreateGate(bool isEnter)
        {
            this.CanEnterGate = isEnter;
        }

        public void StartGame()
        {
            if (this.GameState != eGameState.Loading)
                return;
            this.m_gameState = eGameState.GameStart;
            this.SendSyncLifeTime();
            this.TotalKillCount = 0;
            this.TotalNpcGrade = 0.0;
            this.TotalNpcExperience = 0.0;
            this.TotalHurt = 0;
            this.m_bossCardCount = 0;
            this.BossCards = (int[])null;
            List<Player> allFightPlayers = this.GetAllFightPlayers();
            this.mapPos = MapMgr.GetPVEMapRandomPos(this.m_map.Info.ID);
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)99);
            pkg.WriteInt(allFightPlayers.Count);
            foreach (Player player in allFightPlayers)
            {
                if (!player.IsLiving)
                    this.AddLiving((Living)player);
                player.Reset();
                Point playerPoint = this.GetPlayerPoint(this.mapPos, player.Team);
                player.SetXY(playerPoint);
                this.m_map.AddPhysical((Physics)player);
                player.StartMoving();
                player.StartGame();
                pkg.WriteInt(player.Id);
                pkg.WriteInt(player.X);
                pkg.WriteInt(player.Y);
                if (playerPoint.X < 600)
                    player.Direction = 1;
                else
                    player.Direction = -1;
                pkg.WriteInt(player.Direction);
                pkg.WriteInt(player.Blood);
                pkg.WriteInt(player.MaxBlood);
                pkg.WriteInt(player.Team);
                pkg.WriteInt(player.Weapon.RefineryLevel);
                pkg.WriteInt(50);
                pkg.WriteInt(player.Dander);
                pkg.WriteInt(player.PlayerDetail.FightBuffs.Count);
                foreach (BufferInfo fightBuff in player.PlayerDetail.FightBuffs)
                {
                    pkg.WriteInt(fightBuff.Type);
                    pkg.WriteInt(fightBuff.Value);
                }
                pkg.WriteInt(0);
                pkg.WriteBoolean(player.IsFrost);
                pkg.WriteBoolean(player.IsHide);
                pkg.WriteBoolean(player.IsNoHole);
                pkg.WriteBoolean(false);
                pkg.WriteInt(0);
            }
            pkg.WriteDateTime(DateTime.Now);
            this.SendToAll(pkg);
            this.SendUpdateUiData();
            this.WaitTime(this.PlayerCount * 2500 + 1000);
            this.OnGameStarted();
        }

        public void StartGameMovie()
        {
            if (this.GameState != eGameState.Loading)
                return;
            try
            {
                this.m_missionAI.OnStartMovie();
            }
            catch (Exception ex)
            {
                BaseGame.log.ErrorFormat("game ai script {0} error:{1}", (object)this.GameState, (object)ex);
            }
        }

        public void StartLoading()
        {
            if (this.GameState != eGameState.SessionPrepared)
                return;
            this.m_gameState = eGameState.Loading;
            this.m_turnIndex = 0;
            this.SendMissionInfo();
            this.SendStartLoading(60);
            this.VaneLoading();
            this.AddAction((IAction)new WaitPlayerLoadingAction((BaseGame)this, 61000));
        }

        public override void Stop()
        {
            if (this.GameState != eGameState.ALLSessionStopped)
                return;
            this.m_gameState = eGameState.Stopped;
            if (this.IsWin)
            {
                List<Player> allFightPlayers = this.GetAllFightPlayers();
                foreach (Player player in allFightPlayers)
                {
                    
                    if (player.IsActive && player.CanTakeOut > 0)
                    {
                        player.HasPaymentTakeCard = true;
                        int canTakeOut = player.CanTakeOut;
                        for (int index =
                            0; index < canTakeOut; ++index)
                            TakeCard(player);

                       
                        

                    }
                   
                }
                if (RoomType == eRoomType.Dungeon)
                    this.SendShowCards();
                if (RoomType == eRoomType.Dungeon)
                {
                    foreach (Player player in allFightPlayers)
                        player.PlayerDetail.SetPvePermission(this.m_info.ID, this.m_hardLevel);
                }
            }
            lock (this.m_players)
                this.m_players.Clear();
            this.OnGameStopped();
        }

        public override bool TakeCard(Player player)
        {
            int index1 = 0;
            for (int index2 = 0; index2 < this.Cards.Length; ++index2)
            {
                if (this.Cards[index2] == 0)
                {
                    index1 = index2;
                    break;
                }
            }
            return this.TakeCard(player, index1);
        }

        public override bool TakeCard(Player player, int index)
        {
            if (player.CanTakeOut == 0)
            {
                player.PlayerDetail.AddLog("PVE", "Error No. 1");
                return false;
            }
            if (!player.IsActive || index < 0 || (index > this.Cards.Length || player.FinishTakeCard) || this.Cards[index] > 0)
            {
                player.PlayerDetail.AddLog("PVE", "Error No. 2");
                return false;
            }
            int gold = 0;
            int money = 0;
            int giftToken = 0;
            int medal = 0;
            int honor = 0;
            int hardCurrency = 0;
            int token = 0;
            int dragonToken = 0;
            int magicStonePoint = 0;
            int templateID = 0;
            int count = 0;
            List<SqlDataProvider.Data.ItemInfo> info = (List<SqlDataProvider.Data.ItemInfo>)null;
            if (DropInventory.CopyDrop(this.TakeCardId == 0 ? this.m_missionInfo.Id : this.TakeCardId, 1, ref info))
            {
                if (info != null)
                {
                    foreach (SqlDataProvider.Data.ItemInfo itemInfo in info)
                    {
                        ShopMgr.FindSpecialItemInfo(itemInfo, ref gold, ref money, ref giftToken, ref medal, ref honor, ref hardCurrency, ref token, ref dragonToken, ref magicStonePoint);
                        if (itemInfo != null && itemInfo.TemplateID > 0)
                        {
                            templateID = itemInfo.TemplateID;
                            count = itemInfo.Count;
                            player.PlayerDetail.AddTemplate(itemInfo, eBageType.TempBag, itemInfo.Count, eGameView.dungeonTypeGet);
                        }
                        if (itemInfo.IsTips)
                            player.PlayerDetail.PVEFightMessage(LanguageMgr.GetTranslation("PVEFight.congratulation", (object)player.PlayerDetail.PlayerCharacter.NickName, (object)"@"), itemInfo, player.PlayerDetail.ZoneId);
                    }
                }
                player.PlayerDetail.AddGold(gold);
                player.PlayerDetail.AddMoney(money);
                player.PlayerDetail.LogAddMoney(AddMoneyType.Award, AddMoneyType.Award_TakeCard, player.PlayerDetail.PlayerCharacter.ID, money, player.PlayerDetail.PlayerCharacter.Money);
                player.PlayerDetail.AddGiftToken(giftToken);
                if (templateID == 0 && gold > 0)
                {
                    templateID = -100;
                    count = gold;
                }
            }
            

            if (RoomType != eRoomType.Dungeon && RoomType != eRoomType.Boss)
            {
                player.FinishTakeCard = true;
            }
            else 
            {
                --player.CanTakeOut;
                if (player.CanTakeOut == 0)
                {
                    player.FinishTakeCard = true;
                }
            }            
            Cards[index] = 1;
            SendGamePlayerTakeCard(player, index, templateID, count);
            return true;
        }

        public void TakeConsortiaBossAward(bool isWin)
        {
            foreach (Player allFightPlayer in this.GetAllFightPlayers())
                allFightPlayer.PlayerDetail.UpdatePveResult("consortiaboss", allFightPlayer.TotalDameLiving, isWin);
        }

        public int BossCardCount
        {
            get
            {
                return this.m_bossCardCount;
            }
            set
            {
                if (value <= 0)
                    return;
                this.BossCards = new int[9];
                this.m_bossCardCount = value;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return this.m_currentLiving as Player;
            }
        }

        public TurnedLiving CurrentTurnLiving
        {
            get
            {
                return this.m_currentLiving;
            }
        }

        public List<string> GameOverResources
        {
            get
            {
                return this.m_gameOverResources;
            }
        }

        public eHardLevel HandLevel
        {
            get
            {
                return this.m_hardLevel;
            }
        }

        public string IsBossWar
        {
            get
            {
                return this.m_IsBossType;
            }
            set
            {
                this.m_IsBossType = value;
            }
        }

        public List<int> MapHistoryIds
        {
            get
            {
                return this.m_mapHistoryIds;
            }
            set
            {
                this.m_mapHistoryIds = value;
            }
        }

        public MapPoint MapPos
        {
            get
            {
                return this.mapPos;
            }
        }

        public AMissionControl MissionAI
        {
            get
            {
                return this.m_missionAI;
            }
        }

        public MissionInfo MissionInfo
        {
            get
            {
                return this.m_missionInfo;
            }
            set
            {
                this.m_missionInfo = value;
            }
        }

        public int PveGameDelay
        {
            get
            {
                return this.m_pveGameDelay;
            }
            set
            {
                this.m_pveGameDelay = value;
            }
        }

        public void SendQuizWindow(
          int QuizID,
          int ArightResult,
          int NeedArightResult,
          int MaxQuizSize,
          int TimeOut,
          string Caption,
          string QuizStr,
          string ResultStrFirst,
          string ResultStrSecond,
          string ResultStrThird)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)24);
            pkg.WriteBoolean(true);
            pkg.WriteInt(QuizID);
            pkg.WriteInt(ArightResult);
            pkg.WriteInt(NeedArightResult);
            pkg.WriteInt(MaxQuizSize);
            pkg.WriteInt(TimeOut);
            pkg.WriteString(Caption);
            pkg.WriteString(QuizStr);
            pkg.WriteString(ResultStrFirst);
            pkg.WriteString(ResultStrSecond);
            pkg.WriteString(ResultStrThird);
            this.SendToAll(pkg);
        }

        public void SendCloseQuizWindow()
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)24);
            pkg.WriteBoolean(false);
            this.SendToAll(pkg);
        }

        public void SendPassDrama(bool isShowPassButton)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)133);
            pkg.WriteBoolean(isShowPassButton);
            this.SendToAll(pkg);
        }

        public void SendPlayBackgroundSound(bool isPlay)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)71);
            pkg.WriteBoolean(isPlay);
            this.SendToAll(pkg);
        }

        public void SendLivingToTop(Living living)
        {
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)70);
            pkg.WriteInt(living.Id);
            this.SendToAll(pkg);
        }
    }
}
