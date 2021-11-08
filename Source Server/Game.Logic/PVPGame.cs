using Bussiness;
using Game.Base.Packets;
using Game.Logic.Actions;
using Game.Logic.Phy.Maps;
using Game.Logic.Phy.Object;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace Game.Logic
{
    public class PVPGame : BaseGame
    {
        
        private static readonly int MONEY_MIN_RATE_LOSE = int.Parse(ConfigurationManager.AppSettings[nameof(MONEY_MIN_RATE_LOSE)]);
        private static readonly int MONEY_MAX_RATE_LOSE = int.Parse(ConfigurationManager.AppSettings[nameof(MONEY_MAX_RATE_LOSE)]);
        private static readonly int MONEY_MIN_RATE_WIN = int.Parse(ConfigurationManager.AppSettings[nameof(MONEY_MIN_RATE_WIN)]);
        private static readonly int MONEY_MAX_RATE_WIN = int.Parse(ConfigurationManager.AppSettings[nameof(MONEY_MAX_RATE_WIN)]);
        private static readonly int EXP_MIN_RATE_LOSE = int.Parse(ConfigurationManager.AppSettings[nameof(EXP_MIN_RATE_LOSE)]);
        private static readonly int EXP_MAX_RATE_LOSE = int.Parse(ConfigurationManager.AppSettings[nameof(EXP_MAX_RATE_LOSE)]);
        private static readonly int EXP_MIN_RATE_WIN = int.Parse(ConfigurationManager.AppSettings[nameof(EXP_MIN_RATE_WIN)]);
        private static readonly int EXP_MAX_RATE_WIN = int.Parse(ConfigurationManager.AppSettings[nameof(EXP_MAX_RATE_WIN)]);
        private static readonly double GP_RATE = (double)int.Parse(ConfigurationManager.AppSettings[nameof(GP_RATE)]);
        private static readonly int LeagueMoney_Lose = new Random().Next(1, int.Parse(ConfigurationManager.AppSettings[nameof(LeagueMoney_Lose)]));
        private static readonly int LeagueMoney_Win = new Random().Next(3, int.Parse(ConfigurationManager.AppSettings[nameof(LeagueMoney_Win)])); 

        private new static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private int BeginPlayerCount;
        private DateTime beginTime;
        private float m_blueAvgLevel;
        private List<Player> m_blueTeam;
        private float m_redAvgLevel;
        private List<Player> m_redTeam;
        private string teamAStr;
        private string teamBStr;
        public string m_continuousNick;

        public PVPGame(
          int id,
          int roomId,
          List<IGamePlayer> red,
          List<IGamePlayer> blue,
          Map map,
          eRoomType roomType,
          eGameType gameType,
          int timeType)
          : base(id, roomId, map, roomType, gameType, timeType)
        {
            this.m_redTeam = new List<Player>();
            this.m_blueTeam = new List<Player>();
            StringBuilder stringBuilder1 = new StringBuilder();
            this.m_redAvgLevel = 0.0f;
            foreach (IGamePlayer gamePlayer in red)
            {
                Player fp = new Player(gamePlayer, this.PhysicalId++, (BaseGame)this, 1, gamePlayer.PlayerCharacter.hp);
                stringBuilder1.Append(gamePlayer.PlayerCharacter.ID).Append(",");
                fp.Reset();
                fp.Direction = this.m_random.Next(0, 1) == 0 ? 1 : -1;
                this.AddPlayer(gamePlayer, fp);
                this.m_redTeam.Add(fp);
                this.m_redAvgLevel += (float)gamePlayer.PlayerCharacter.Grade;
            }
            this.m_redAvgLevel /= (float)this.m_redTeam.Count;
            this.teamAStr = stringBuilder1.ToString();
            StringBuilder stringBuilder2 = new StringBuilder();
            this.m_blueAvgLevel = 0.0f;
            foreach (IGamePlayer gamePlayer in blue)
            {
                Player fp = new Player(gamePlayer, this.PhysicalId++, (BaseGame)this, 2, gamePlayer.PlayerCharacter.hp);
                stringBuilder2.Append(gamePlayer.PlayerCharacter.ID).Append(",");
                fp.Reset();
                fp.Direction = this.m_random.Next(0, 1) == 0 ? 1 : -1;
                this.AddPlayer(gamePlayer, fp);
                this.m_blueTeam.Add(fp);
                this.m_blueAvgLevel += (float)gamePlayer.PlayerCharacter.Grade;
            }
            this.m_blueAvgLevel /= (float)blue.Count;
            this.teamBStr = stringBuilder2.ToString();
            this.BeginPlayerCount = this.m_redTeam.Count + this.m_blueTeam.Count;
            this.beginTime = DateTime.Now;
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
            bool flag;
            if (player.CanTakeOut == 0)
                flag = false;
            else if (!player.IsActive || index < 0 || (index > this.Cards.Length || player.FinishTakeCard) || this.Cards[index] > 0)
            {
                flag = false;
            }
            else
            {
                --player.CanTakeOut;
                int gold = 0;
                int money = 0;
                int giftToken = 0;
                int templateID = 0;
                List<SqlDataProvider.Data.ItemInfo> info1 = (List<SqlDataProvider.Data.ItemInfo>)null;
                if (DropInventory.CardDrop(this.RoomType, ref info1) && info1 != null)
                {
                    foreach (SqlDataProvider.Data.ItemInfo info2 in info1)
                    {
                        templateID = info2.Template.TemplateID;
                        SqlDataProvider.Data.ItemInfo specialItemInfo = SqlDataProvider.Data.ItemInfo.FindSpecialItemInfo(info2, ref gold, ref money, ref giftToken);
                        if (specialItemInfo != null && templateID > 0)
                            player.PlayerDetail.AddTemplate(specialItemInfo, eBageType.TempBag, info2.Count, eGameView.BatleTypeGet);
                    }
                }
                player.FinishTakeCard = true;
                this.Cards[index] = 1;
                int count = 0;
                switch (templateID)
                {
                    case -300:
                        count = giftToken;
                        break;
                    case -200:
                        count = money;
                        break;
                    case -100:
                        count = gold;
                        break;
                    case 0:
                        templateID = -100;
                        count = 500;
                        break;
                }
                player.PlayerDetail.AddGold(gold);
                player.PlayerDetail.AddMoney(money);
                player.PlayerDetail.AddGiftToken(giftToken);
                this.SendGamePlayerTakeCard(player, index, templateID, count);
                flag = true;
            }
            return flag;
        }

        private int CalculateExperience(
          Player player,
          int winTeam,
          ref int reward,
          ref int rewardServer)
        {
            int num1;
            if (this.m_roomType == eRoomType.Match)
            {
                float num2 = player.Team == 1 ? this.m_blueAvgLevel : this.m_redAvgLevel;
                float num3 = player.Team == 1 ? (float)this.m_blueTeam.Count : (float)this.m_redTeam.Count;
                double num4 = (double)Math.Abs(num2 - (float)player.PlayerDetail.PlayerCharacter.Grade);
                if (player.TotalHurt == 0)
                {
                    if ((double)num2 - (double)player.PlayerDetail.PlayerCharacter.Grade >= 5.0 && this.TotalHurt > 0)
                    {
                        this.SendMessage(player.PlayerDetail, LanguageMgr.GetTranslation("GetGPreward"), (string)null, 2);
                        reward = 200;
                        num1 = 201;
                    }
                    else
                        num1 = 1;
                }
                else
                {
                    float num5 = player.Team == winTeam ? 2f : 0.0f;
                    player.TotalShootCount = player.TotalShootCount == 0 ? 1 : player.TotalShootCount;
                    if (player.TotalShootCount < player.TotalHitTargetCount)
                        player.TotalShootCount = player.TotalHitTargetCount;
                    int num6 = player.Team == 1 ? (int)((double)this.m_blueTeam.Count * (double)this.m_blueAvgLevel * 300.0) : (int)((double)this.m_redAvgLevel * (double)this.m_redTeam.Count * 300.0);
                    int num7 = player.TotalHurt > num6 ? num6 : player.TotalHurt;
                    int gp = (int)Math.Ceiling(((double)num5 + (double)num7 * 0.001 + (double)player.TotalKill * 0.5 + (double)(player.TotalHitTargetCount / player.TotalShootCount * 2)) * (double)num2 * (0.9 + ((double)num3 - 1.0) * 0.3));
                    if ((double)num2 - (double)player.PlayerDetail.PlayerCharacter.Grade >= 5.0 && this.TotalHurt > 0)
                    {
                        this.SendMessage(player.PlayerDetail, LanguageMgr.GetTranslation("GetGPreward"), (string)null, 2);
                        reward = 200;
                        gp += 200;
                    }
                    int num8 = this.GainCoupleGP(player, gp);
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["DoubleEvent"]))
                    {
                        num8 *= 2;
                        rewardServer = num8 / 2;
                    }
                    if (num8 > 12000)
                    {
                        PVPGame.log.Error((object)string.Format("pvpgame ====== player.nickname : {0}, add gp : {1} ======== gp > 10000", (object)player.PlayerDetail.PlayerCharacter.NickName, (object)num8));
                        PVPGame.log.Error((object)string.Format("pvpgame ====== player.nickname : {0}, parameters winPlus: {1}, totalHurt : {2}, totalKill : {3}, totalHitTargetCount : {4}, totalShootCount : {5}, againstTeamLevel : {6}, againstTeamCount : {7}", (object)player.PlayerDetail.PlayerCharacter.NickName, (object)num5, (object)player.TotalHurt, (object)player.TotalKill, (object)player.TotalHitTargetCount, (object)player.TotalShootCount, (object)num2, (object)num3));
                        num8 = 12000;
                    }
                    num1 = num8 < 1 ? 1 : num8;
                }
            }
            else
                num1 = 0;
            return num1;
        }

        private int CalculateGuildMatchResult(List<Player> players, int winTeam)
        {
            if (this.RoomType == eRoomType.Match && this.GameType == eGameType.Guild)
            {
                StringBuilder stringBuilder = new StringBuilder(LanguageMgr.GetTranslation("Game.Server.SceneGames.OnStopping.Msg5"));
                IGamePlayer gamePlayer1 = (IGamePlayer)null;
                IGamePlayer gamePlayer2 = (IGamePlayer)null;
                int totalKillHealth = 0;
                foreach (Player player in players)
                {
                    if (player.Team == winTeam)
                    {
                        stringBuilder.Append(string.Format("[{0}]", (object)player.PlayerDetail.PlayerCharacter.NickName));
                        gamePlayer1 = player.PlayerDetail;
                        int num1 = player.Team == 1 ? (int)((double)this.m_blueTeam.Count * (double)this.m_blueAvgLevel * 300.0) : (int)((double)this.m_redAvgLevel * (double)this.m_redTeam.Count * 300.0);
                        int num2 = player.TotalHurt > num1 ? num1 : player.TotalHurt;
                        totalKillHealth += num2;
                    }
                    else
                        gamePlayer2 = player.PlayerDetail;
                }
                if (gamePlayer2 != null)
                {
                    stringBuilder.Append(LanguageMgr.GetTranslation("Game.Server.SceneGames.OnStopping.Msg1") + gamePlayer2.PlayerCharacter.ConsortiaName + LanguageMgr.GetTranslation("Game.Server.SceneGames.OnStopping.Msg2"));
                    gamePlayer1.ConsortiaFight(gamePlayer1.PlayerCharacter.ConsortiaID, gamePlayer2.PlayerCharacter.ConsortiaID, this.Players, this.RoomType, this.GameType, totalKillHealth, players.Count);
                    int num = winTeam == 1 ? this.m_blueTeam.Count : this.m_redTeam.Count;
                    int riches = (int)(float)(num + totalKillHealth / 2000);
                    gamePlayer1.SendConsortiaFight(gamePlayer1.PlayerCharacter.ConsortiaID, riches, stringBuilder.ToString());
                    if (riches > 100000)
                        PVPGame.log.Error((object)string.Format("pvpgame ======= riches : {0}, count : {1}, teamTotalHurt : {2}", new object[3]
                        {
              (object) riches,
              (object) num,
              (object) totalKillHealth
                        }));
                    foreach (Player player in players)
                    {
                        if (player.Team == winTeam)
                            player.PlayerDetail.AddRobRiches(riches);
                    }
                    return riches;
                }
            }
            return 0;
        }

        public bool CanGameOver()
        {
            bool flag1 = true;
            bool flag2 = true;
            foreach (Physics physics in this.m_redTeam)
            {
                if (physics.IsLiving)
                {
                    flag1 = false;
                    break;
                }
            }
            foreach (Physics physics in this.m_blueTeam)
            {
                if (physics.IsLiving)
                {
                    flag2 = false;
                    break;
                }
            }
            return flag1 | flag2;
        }

        public void CreateNpc()
        {
            int[] array1 = new int[5]
            {
        10008,
        10005,
        10006,
        10009,
        10007
            };
            int[] array2 = new int[3] { 350, 500, 680 };
            this.Shuffer<int>(array2);
            this.Shuffer<int>(array1);
            this.ClearAllNpc();
            int x = array2[this.Random.Next(array2.Length)];
            for (int index = 0; index < array1.Length; ++index)
            {
                this.CreateNpc(array1[index], x, 259, 1, -1);
                x += 210;
            }
        }

        public void CreateNpc(int npcId, int x, int y, int type, int direction)
        {
            SimpleNpc simpleNpc = new SimpleNpc(this.PhysicalId++, (BaseGame)this, NPCInfoMgr.GetNpcInfoById(npcId), type, direction, 0);
            simpleNpc.Reset();
            simpleNpc.SetXY(x, y);
            this.AddLiving((Living)simpleNpc);
            simpleNpc.StartMoving();
        }

        public override void CheckState(int delay)
        {
            this.AddAction((IAction)new CheckPVPGameStateAction(delay));
        }

        public void GameOver()
        {
            if (this.GameState != eGameState.Playing)
                return;
            this.m_gameState = eGameState.GameOver;
            this.ClearWaitTimer();
            this.CurrentTurnTotalDamage = 0;
            List<Player> allFightPlayers = this.GetAllFightPlayers();
            int num1 = -1;
            int num2 = 0;
            int num3 = 0;
            foreach (Player player in allFightPlayers)
            {
                if (player.IsLiving)
                {
                    num1 = player.Team;
                    break;
                }
            }
            foreach (Player player in allFightPlayers)
            {
                if (player.TotalHurt > 0)
                {
                    if (player.Team == 1)
                        num3 = 1;
                    else
                        num2 = 1;
                }
            }
            if (num1 == -1 && this.CurrentPlayer != null)
                num1 = this.CurrentPlayer.Team;
            int guildMatchResult = this.CalculateGuildMatchResult(allFightPlayers, num1);
            if (this.RoomType == eRoomType.Match && this.GameType == eGameType.Guild)
            {
                int num4 = allFightPlayers.Count / 2;
                Math.Round((double)(allFightPlayers.Count / 2) * 0.5);
            }
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)100);
            pkg.WriteInt(this.PlayerCount);
            int beginPlayerCount = this.BeginPlayerCount;
            foreach (Player player in allFightPlayers)
            {
                double num4 = player.Team == 1 ? (double)this.m_blueAvgLevel : (double)this.m_redAvgLevel;
                if (player.Team != 1)
                {
                    int count1 = this.m_redTeam.Count;
                }
                else
                {
                    int count2 = this.m_blueTeam.Count;
                }
                double grade1 = (double)player.PlayerDetail.PlayerCharacter.Grade;
                float num5 = Math.Abs((float)(num4 - grade1));
                int team = player.Team;
                int num6 = 0;
                int val1 = 0;
                int reward = 0;
                int rewardServer = 0;
                if ((uint)player.TotalShootCount > 0U)
                {
                    int totalShootCount = player.TotalShootCount;
                }
                if (this.m_roomType == eRoomType.Match || (double)num5 < 5.0)
                {
                    val1 = this.CalculateOffer(player, num1);
                    num6 = this.CalculateExperience(player, num1, ref reward, ref rewardServer);
                }
                if (player.FightBuffers.ConsortionAddPercentGoldOrGP > 0)
                    num6 += num6 * player.FightBuffers.ConsortionAddPercentGoldOrGP / 100;
                if (player.FightBuffers.ConsortionAddOfferRate > 0)
                    guildMatchResult *= player.FightBuffers.ConsortionAddOfferRate;
                double num7 = Math.Ceiling((double)num6 * player.PlayerDetail.GPApprenticeOnline);
                double num8 = Math.Ceiling((double)num6 * player.PlayerDetail.GPApprenticeTeam);
                int num9 = num6 == 0 ? 1 : num6;
                string msg = "";
                bool val2 = player.Team == num1;
                Console.WriteLine(this.GameType);
                if (this.RoomType == eRoomType.Match)
                {
                    Random random1 = new Random();
                    int num10 = 1;
                    int num12 = 0;
                    int leagueMoneyLose1 = PVPGame.LeagueMoney_Lose;
                    Random random2 = new Random();

                    //Dia
                    DateTime dateTime1 = Convert.ToDateTime("09:00:00");
                    DateTime dateTime2 = Convert.ToDateTime("10:00:00");

                    //Noite
                    DateTime dateTime3 = Convert.ToDateTime("20:00:00");
                    DateTime dateTime4 = Convert.ToDateTime("21:00:00");

                    //Madrugada
                    DateTime dateTime5 = Convert.ToDateTime("02:00:00");
                    DateTime dateTime6 = Convert.ToDateTime("03:00:00");

                    if (player.TotalHurt > 0)
                    {
                        int num13 = 0;
                        if (val2)
                        {
                            int leagueMoneyWin = PVPGame.LeagueMoney_Win;
                            num9 += 5 * num10 + num12;
                            if (this.GameType == eGameType.Free)
                            {
                                num13 = 200;
                            }
                            else if(this.GameType == eGameType.Guild)
                            {
                                num13 = 250;
                            }
                        }
                        else
                        {
                            int leagueMoneyLose2 = PVPGame.LeagueMoney_Lose;
                            num9 += 5 * num10 + num12;
                            if (this.GameType == eGameType.Free)
                            {
                                num13 = 100;
                            }
                            else if (this.GameType == eGameType.Guild)
                            {
                                num13 = 150;
                            }
                        }

                        if((DateTime.Now >= dateTime1 && DateTime.Now <= dateTime2 || DateTime.Now >= dateTime3 && DateTime.Now <= dateTime4 || DateTime.Now >= dateTime5 && DateTime.Now <= dateTime6))
                        {
                            player.PlayerDetail.SendMessage("Vôcê esta jogando em um horario de cupons em dobro");
                            num13 *= 2;
                        };

                        player.PlayerDetail.SendMessage("Parabéns, você recebeu " + (object)num13 + " Cupons");
                        player.PlayerDetail.AddMoney(num13);
                    }
                    else if (val2)
                        player.PlayerDetail.SendMessage("Você não causou nenhum dano ao inimigo.");
                    else
                        player.PlayerDetail.SendMessage("Você não causou nenhum dano ao inimigo.");
                    num9 += 5;
                    if (msg != "" && msg != null)
                        player.PlayerDetail.SendHideMessage(msg);
                    if (this.GameType == eGameType.Guild)
                        new ConsortiaBussiness().ConsortiaRichAdd(player.PlayerDetail.PlayerCharacter.ConsortiaID, ref guildMatchResult);
                    int restCount = player.PlayerDetail.MatchInfo.restCount;
                    int maxCount = player.PlayerDetail.MatchInfo.maxCount;
                    int grade2 = player.PlayerDetail.PlayerCharacter.Grade;
                }
                if (player.PlayerDetail.PlayerCharacter.typeVIP > (byte)0)
                {
                    num9 += 10;
                    ++val1;
                }
                player.GainGP = player.PlayerDetail.AddGP(num9);
                player.GainOffer = player.PlayerDetail.AddOffer(val1);
                player.CanTakeOut = player.Team == 1 ? num3 : num2;
                pkg.WriteInt(player.Id);
                pkg.WriteBoolean(val2);
                pkg.WriteInt(player.PlayerDetail.PlayerCharacter.Grade);
                pkg.WriteInt(player.PlayerDetail.PlayerCharacter.GP);
                pkg.WriteInt(player.TotalKill);
                pkg.WriteInt(num9);
                pkg.WriteInt(player.TotalHitTargetCount);
                pkg.WriteInt(player.psychic);
                pkg.WriteInt(player.PlayerDetail.PlayerCharacter.typeVIP > (byte)0 ? 10 : 0);
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                pkg.WriteInt(rewardServer);
                pkg.WriteInt((int)num7);
                pkg.WriteInt((int)num8);
                pkg.WriteInt(0);
                pkg.WriteInt(reward);
                pkg.WriteInt(0);
                pkg.WriteInt(rewardServer);
                pkg.WriteInt(player.GainGP);
                pkg.WriteInt(val1);
                pkg.WriteInt(0);
                pkg.WriteInt(player.PlayerDetail.PlayerCharacter.typeVIP > (byte)0 ? 1 : 0);
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                pkg.WriteInt(0);
                pkg.WriteInt(rewardServer);
                pkg.WriteInt(player.GainOffer);
                pkg.WriteInt(player.CanTakeOut);
            }
            pkg.WriteInt(guildMatchResult);
            this.SendToAll(pkg);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Player player in allFightPlayers)
                player.PlayerDetail.OnGameOver((AbstractGame)this, player.Team == num1, player.GainGP, false, this.CoupleFight(player), player.Blood, this.BeginPlayerCount);
            this.OnGameOverLog(this.RoomId, this.RoomType, this.GameType, 0, this.beginTime, DateTime.Now, this.BeginPlayerCount, this.Map.Info.ID, this.teamAStr, this.teamBStr, "", num1, this.BossWarField);
            this.WaitTime(20000);
            this.OnGameOverred();
        }

        private int CalculateOffer(Player player, int winTeam)
        {
            int num1;
            if ((uint)this.RoomType > 0U)
            {
                num1 = 0;
            }
            else
            {
                int num2 = 0;
                if (this.GameType == eGameType.Guild)
                {
                    int num3 = player.Team == 1 ? this.m_blueTeam.Count : this.m_redTeam.Count;
                    num2 = player.Team != winTeam ? (int)((double)num3 * 0.5) : num3;
                }
                int gainOffer = player.GainOffer;
                int num4 = (int)((double)gainOffer + (double)num2) - player.KilledPunishmentOffer;
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["DoubleEvent"]))
                    num4 *= 2;
                if (num4 > 1000)
                {
                    PVPGame.log.Error((object)string.Format("pvegame ====== player.nickname : {0}, add offer : {1} ======== offer > 1000", (object)player.PlayerDetail.PlayerCharacter.NickName, (object)num4));
                    PVPGame.log.Error((object)string.Format("pvegame ====== player.nickname : {0}, parameters RoomType : {1}, baseOffer : {2}, appendOffer : {3}", (object)player.PlayerDetail.PlayerCharacter.NickName, (object)this.RoomType, (object)gainOffer, (object)num2));
                }
                num1 = num4;
            }
            return num1;
        }

        public void NextTurn()
        {
            if (this.GameState != eGameState.Playing)
                return;
            this.ClearWaitTimer();
            this.ClearDiedPhysicals();
            this.CheckBox();
            ++this.m_turnIndex;
            List<Box> box = this.CreateBox();
            foreach (Physics physics in this.m_map.GetAllPhysicalSafe())
                physics.PrepareNewTurn();
            this.LastTurnLiving = this.m_currentLiving;
            this.m_currentLiving = this.FindNextTurnedLiving();
            if (this.m_currentLiving.VaneOpen)
                this.UpdateWind(this.GetNextWind(), false);
            if (this.m_currentLiving is Player && (this.m_currentLiving as Player).PlayerDetail.PlayerCharacter.NickName == this.ContinuousRunningPlayer)
            {
                foreach (Player allFightPlayer in this.GetAllFightPlayers())
                    allFightPlayer.PlayerDetail.SendMessage(LanguageMgr.GetTranslation("Game.Logic.ContinuousRunning", (object)this.ContinuousRunningPlayer));
            }
            this.MinusDelays(this.m_currentLiving.Delay);
            this.m_currentLiving.PrepareSelfTurn();
            if (!this.CurrentLiving.IsFrost && this.m_currentLiving.IsLiving && !this.m_currentLiving.BlockTurn)
            {
                this.m_currentLiving.StartAttacking();
                this.SendSyncLifeTime();
                this.SendGameNextTurn((Living)this.m_currentLiving, (BaseGame)this, box);
                if (this.m_currentLiving.IsAttacking)
                    this.AddAction((IAction)new WaitLivingAttackingAction(this.m_currentLiving, this.m_turnIndex, (this.m_timeType + 20) * 1000));
            }
            if (this.m_currentLiving is Player && this.FindNextTurnedLiving() is Player)
                this.ContinuousRunningPlayer = (this.m_currentLiving as Player).PlayerDetail.PlayerCharacter.NickName;
            this.OnBeginNewTurn();
        }

        public void Prepare()
        {
            if ((uint)this.GameState > 0U)
                return;
            this.SendCreateGame();
            this.m_gameState = eGameState.Prepared;
            this.CheckState(0);
        }

        public override Player RemovePlayer(IGamePlayer gp, bool IsKick)
        {
            Player player = base.RemovePlayer(gp, IsKick);
            if (player != null && player.IsLiving && this.GameState != eGameState.Loading)
            {
                gp.RemoveGP(gp.PlayerCharacter.Grade * 12);
                string msg = (string)null;
                string msg1 = (string)null;
                if (this.RoomType == eRoomType.Match)
                {
                    if (this.GameType == eGameType.Guild)
                    {
                        msg = LanguageMgr.GetTranslation("AbstractPacketLib.SendGamePlayerLeave.Msg6", (object)(gp.PlayerCharacter.Grade * 12), (object)15);
                        gp.RemoveOffer(15);
                        msg1 = LanguageMgr.GetTranslation("AbstractPacketLib.SendGamePlayerLeave.Msg7", (object)gp.PlayerCharacter.NickName, (object)(gp.PlayerCharacter.Grade * 12), (object)15);
                    }
                    else if (this.GameType == eGameType.Free)
                    {
                        msg = LanguageMgr.GetTranslation("AbstractPacketLib.SendGamePlayerLeave.Msg6", (object)(gp.PlayerCharacter.Grade * 12), (object)5);
                        gp.RemoveOffer(5);
                        msg1 = LanguageMgr.GetTranslation("AbstractPacketLib.SendGamePlayerLeave.Msg7", (object)gp.PlayerCharacter.NickName, (object)(gp.PlayerCharacter.Grade * 12), (object)5);
                    }
                }
                else
                {
                    msg = LanguageMgr.GetTranslation("AbstractPacketLib.SendGamePlayerLeave.Msg4", (object)(gp.PlayerCharacter.Grade * 12));
                    msg1 = LanguageMgr.GetTranslation("AbstractPacketLib.SendGamePlayerLeave.Msg5", (object)gp.PlayerCharacter.NickName, (object)(gp.PlayerCharacter.Grade * 12));
                }
                this.SendMessage(gp, msg, msg1, 3);
                if (this.GetSameTeam())
                {
                    this.CurrentLiving.StopAttacking();
                    this.CheckState(0);
                }
            }
            return player;
        }

        public void StartGame()
        {
            if (this.GameState != eGameState.Loading)
                return;
            this.m_gameState = eGameState.Playing;
            this.ClearWaitTimer();
            this.SendSyncLifeTime();
            List<Player> allFightPlayers = this.GetAllFightPlayers();
            MapPoint mapRandomPos = MapMgr.GetMapRandomPos(this.m_map.Info.ID);
            GSPacketIn pkg = new GSPacketIn((short)91);
            pkg.WriteByte((byte)99);
            pkg.WriteInt(allFightPlayers.Count);
            foreach (Player player in allFightPlayers)
            {
                player.Reset();
                Point playerPoint = this.GetPlayerPoint(mapRandomPos, player.Team);
                player.SetXY(playerPoint);
                this.m_map.AddPhysical((Physics)player);
                player.StartMoving();
                player.StartGame();
                pkg.WriteInt(player.Id);
                pkg.WriteInt(player.X);
                pkg.WriteInt(player.Y);
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
            this.WaitTime(allFightPlayers.Count * 1000);
            this.OnGameStarted();
        }

        public void StartLoading()
        {
            if (this.GameState != eGameState.Prepared)
                return;
            this.ClearWaitTimer();
            this.SendStartLoading(60);
            this.VaneLoading();
            this.AddAction((IAction)new WaitPlayerLoadingAction((BaseGame)this, 61000));
            this.m_gameState = eGameState.Loading;
        }

        public override void Stop()
        {
            if (this.GameState != eGameState.GameOver)
                return;
            this.m_gameState = eGameState.Stopped;
            foreach (Player allFightPlayer in this.GetAllFightPlayers())
            {
                if (allFightPlayer.IsActive && !allFightPlayer.FinishTakeCard && allFightPlayer.CanTakeOut > 0)
                    this.TakeCard(allFightPlayer);
            }
            lock (this.m_players)
                this.m_players.Clear();
            base.Stop();
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
            foreach (int num1 in intList)
            {
                List<SqlDataProvider.Data.ItemInfo> info = (List<SqlDataProvider.Data.ItemInfo>)null;
                if (!DropInventory.CardDrop(this.RoomType, ref info) || info == null)
                    return;
                if (info != null)
                {
                    int num2 = 0;
                    foreach (SqlDataProvider.Data.ItemInfo itemInfo in info)
                    {
                        val2 = itemInfo.TemplateID;
                        val3 = itemInfo.Count;
                        ++num2;
                        if (num2 >= intList.Count - 1)
                            break;
                    }
                }
                pkg.WriteByte((byte)num1);
                pkg.WriteInt(val2);
                pkg.WriteInt(val3);
            }
            this.SendToAll(pkg);
        }

        public Player CurrentPlayer
        {
            get
            {
                return this.m_currentLiving as Player;
            }
        }

        public string ContinuousRunningPlayer
        {
            get
            {
                return this.m_continuousNick;
            }
            set
            {
                this.m_continuousNick = value;
            }
        }
    }
}
