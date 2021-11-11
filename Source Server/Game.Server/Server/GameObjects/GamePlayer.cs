using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Logic.Phy.Object;
using Game.Server.Achievement;
using Game.Server.Buffer;
using Game.Server.Consortia;
using Game.Server.ConsortiaTask;
using Game.Server.Event;
using Game.Server.GameUtils;
using Game.Server.HotSpringRooms;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.Pet;
using Game.Server.Quests;
using Game.Server.Rooms;
using Game.Server.SceneMarryRooms;
using Game.Server.Statics;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Game.Server.GameObjects
{
    public class GamePlayer : IGamePlayer
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static char[] permissionChars = new char[4]
        {
      '1',
      '3',
      '7',
      'F'
        };
        private static readonly int[] StyleIndex = new int[15]
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
        private static char[] fightlabpermissionChars = new char[4]
        {
      '0',
      '1',
      '2',
      '3'
        };
        public Dictionary<int, CardInfo> Card = new Dictionary<int, CardInfo>();
        public CardInfo[] CardsTakeOut = new CardInfo[9];
        public double GuildRichAddPlus = 1.0;
        public readonly string[] labyrinthGolds = new string[40]
        {
      "0|0",
      "2|2",
      "0|0",
      "2|2",
      "0|0",
      "2|3",
      "0|0",
      "3|3",
      "0|0",
      "3|4",
      "0|0",
      "3|4",
      "0|0",
      "4|5",
      "0|0",
      "4|5",
      "0|0",
      "4|6",
      "0|0",
      "5|6",
      "0|0",
      "5|7",
      "0|0",
      "5|7",
      "0|0",
      "6|8",
      "0|0",
      "6|8",
      "0|0",
      "6|10",
      "0|0",
      "8|10",
      "0|0",
      "8|11",
      "0|0",
      "8|11",
      "0|0",
      "10|12",
      "0|0",
      "10|12"
        };
        private int m_immunity = (int)byte.MaxValue;
        private Dictionary<string, object> m_tempProperties = new Dictionary<string, object>();
        private Dictionary<int, int> _friends;
        private List<int> _viFarms;
        public DateTime BossBoxStartTime;
        public DateTime LastOpenHole;
        public int canTakeOut;
        public int CurrentRoomIndex;
        public int CurrentRoomTeam;
        public int FightPower;
        public int Hot_Direction;
        public int Hot_X;
        public int Hot_Y;
        public int HotMap;
        private HotSpringRoom hotSpringRoom_0;
        public bool IsInChristmasRoom;
        public bool IsInWorldBossRoom;
        public bool isPowerFullUsed;
        public bool KickProtect;
        public DateTime LastAttachMail;
        public DateTime LastChatTime;
        public DateTime LastDrillUpTime;
        public DateTime LastEnterWorldBoss;
        public DateTime LastFigUpTime;
        public DateTime LastOpenCard;
        private ePlayerState ePlayerState_0;
        internal void SendItemsToMail(List<ItemInfo> list, string v, eMailType storeCanel)
        {
            throw new NotImplementedException();
        }

        public DateTime LastOpenChristmasPackage;
        public DateTime LastOpenGrowthPackage;
        public DateTime LastOpenPack;
        public DateTime LastOpenYearMonterPackage;
        public List<SqlDataProvider.Data.ItemInfo> LotteryAwardList;
        private string m_account;
        private AchievementInventory m_achievementInventory;
        private EventInventory m_eventLiveInventory;
        private PlayerBattle m_battle;
        private BufferList m_bufferList;
        private PlayerInventory m_caddyBag;
        private CardInventory m_cardBag;
        protected GameClient m_client;
        private PlayerInventory m_ConsortiaBag;
        private UTF8Encoding m_converter;
        private MarryRoom m_currentMarryRoom;
        private BaseRoom m_currentRoom;
        private SqlDataProvider.Data.ItemInfo m_currentSecondWeapon;
        private int m_changed;
        private PlayerInfo m_character;
        private PlayerEquipInventory m_equipBag;
        private List<int> m_equipEffect;
        private PlayerExtra m_extra;
        private PlayerInventory m_farmBag;
        private PlayerInventory m_fightBag;
        private List<BufferInfo> m_fightBuffInfo;
        private PlayerInventory m_food;
        protected BaseGame m_game;
        private SqlDataProvider.Data.ItemInfo m_healstone;
        private bool m_isAASInfo;
        private bool m_isMinor;
        private SqlDataProvider.Data.ItemInfo m_MainWeapon;
        private UsersPetInfo m_pet;
        private PlayerInventory m_petEgg;
        private long m_pingTime;
        private int m_playerId;
        private PlayerProperty m_playerProp;
        protected Player m_players;
        private ePlayerState m_playerState;
        private PlayerInventory m_propBag;
        private char[] m_pvepermissions;
        private QuestInventory m_questInventory;
        private PlayerRank m_rank;
        private bool m_showPP;
        private PlayerInventory m_storeBag;
        private PlayerInventory m_tempBag;
        public bool m_toemview;
        public DateTime BoxBeginTime;
        private Dictionary<int, UserDrillInfo> m_userDrills;
        public int MarryMap;
        public long PingStart;
        public byte States;
        public int takeoutCount;
        public int winningStreak;
        public int WorldBossMap;
        public int X;
        public int Y;
        private char[] m_fightlabpermissions;
        public int missionPlayed;
        public int playersKilled;
        protected ConsortiaLogicProcessor m_consortiaProcessor;
        private ConsortiaProcessor consortiaProcessor_0;
        protected PetLogicProcessor m_petProcessor;
        private PetProcessor petProcessor_0;
        private PetInventory m_petBag;
        public DateTime LastMovePlaceItem;
        public Dictionary<int, int[]> CardResetTempProp;
        private UserLabyrinthInfo userLabyrinthInfo;
        private List<UserGemStone> list_2;
        public int TakeCardPlace;
        public int TakeCardTemplateID;
        public int TakeCardCount;
        private PlayerActives m_playerActive;
        private int int_6;

        public event GamePlayer.PlayerAchievementFinish AchievementFinishEvent;

        public event GamePlayer.PlayerAdoptPetEventHandle AdoptPetEvent;

        public event GamePlayer.PlayerGameKillBossEventHandel AfterKillingBoss;

        public event GamePlayer.PlayerGameKillEventHandel AfterKillingLiving;

        public event GamePlayer.PlayerItemPropertyEventHandle AfterUsingItem;

        public event GamePlayer.EnterSpa EnterSPA;

        public event GamePlayer.PlayerCropPrimaryEventHandle CropPrimaryEvent;

        public event GamePlayer.PlayerEnterHotSpring EnterHotSpringEvent;

        public event GamePlayer.PlayerVIPUpgrade Event_0;

        public event GamePlayer.PlayerFightAddOffer FightAddOfferEvent;

        public event GamePlayer.PlayerFightOneBloodIsWin FightOneBloodIsWin;

        public event GamePlayer.GameKillDropEventHandel GameKillDrop;

        public event GamePlayer.PlayerOwnConsortiaEventHandle GuildChanged;

        public event GamePlayer.PlayerHotSpingExpAdd HotSpingExpAdd;

        public event GamePlayer.PlayerLoginEventHandle PlayerLogin;

        public event GamePlayer.PlayerItemComposeEventHandle ItemCompose;

        public event GamePlayer.PlayerItemFusionEventHandle ItemFusion;

        public event GamePlayer.PlayerItemInsertEventHandle ItemInsert;

        public event GamePlayer.PlayerItemMeltEventHandle ItemMelt;

        public event GamePlayer.PlayerItemStrengthenEventHandle ItemStrengthen;


        public event GamePlayer.PlayerMoneyChargeHandle MoneyCharge;


        public event GamePlayer.PlayerAchievementQuestHandle AchievementQuest;

        public event GamePlayer.PlayerEventHandle LevelUp;

        public event GamePlayer.PlayerMissionOverEventHandle MissionOver;

        public event GamePlayer.PlayerMissionTurnOverEventHandle MissionTurnOver;

        public event GamePlayer.PlayerNewGearEventHandle NewGearEvent;

        public event GamePlayer.PlayerShopEventHandle Paid;

        public event GamePlayer.PlayerUnknowQuestConditionEventHandle UnknowQuestConditionEvent;

        public event GamePlayer.PlayerUpLevelPetEventHandle UpLevelPetEvent;

        public event GamePlayer.PlayerEventHandle UseBuffer;

        public event GamePlayer.PlayerUserToemGemstoneEventHandle UserToemGemstonetEvent;

        public event GamePlayer.PlayerPropertyChangedEventHandel PlayerPropertyChanged;

        public event GamePlayer.PlayerAddItemEventHandel PlayerAddItem;

        public event GamePlayer.PlayerOwnSpaEventHandle PlayerSpa;

        protected PetLogicProcessor petProcessor_;
        protected ConsortiaProcessor ConsortiaProcessor;
        public GamePlayer(int playerId, string account, GameClient client, PlayerInfo info)
        {
            this.m_playerId = playerId;
            this.m_account = account;
            this.m_client = client;
            this.m_character = info;
            this.m_equipBag = new PlayerEquipInventory(this);
            this.m_propBag = new PlayerInventory(this, true, 49, 1, 0, true);
            this.m_ConsortiaBag = new PlayerInventory(this, true, 100, 11, 0, true);
            this.m_storeBag = new PlayerInventory(this, true, 20, 12, 0, true);
            this.m_fightBag = new PlayerInventory(this, false, 3, 3, 0, false);
            this.m_tempBag = new PlayerInventory(this, false, 60, 4, 0, true);
            this.m_caddyBag = new PlayerInventory(this, false, 30, 5, 0, true);
            this.m_farmBag = new PlayerInventory(this, true, 30, 13, 0, true);
            this.m_food = new PlayerInventory(this, true, 30, 34, 0, true);
            this.m_petEgg = new PlayerInventory(this, true, 30, 35, 0, true);
            this.m_cardBag = new CardInventory(this, true, 100, 5);
            this.m_petBag = new PetInventory(this, true, 20, 8, 0);
            this.m_rank = new PlayerRank(this, true);
            this.m_playerProp = new PlayerProperty(this);
            this.m_battle = new PlayerBattle(this, true);
            this.m_extra = new PlayerExtra(this, true);
            this.m_playerActive = new PlayerActives(this, true);
            this.m_questInventory = new QuestInventory(this);
            this.m_achievementInventory = new AchievementInventory(this);
            this.m_eventLiveInventory = new EventInventory(this);
            this.m_bufferList = new BufferList(this);
            this.m_fightBuffInfo = new List<BufferInfo>();
            this.m_equipEffect = new List<int>();
            this.list_2 = new List<UserGemStone>();
            this.m_userDrills = new Dictionary<int, UserDrillInfo>();
            this.GPAddPlus = 1.0;
            this.OfferAddPlus = 1.0;
            this.m_toemview = true;
            this.X = 646;
            this.Y = 1241;
            this.MarryMap = 0;
            this.LastChatTime = DateTime.Today;
            this.LastFigUpTime = DateTime.Today;
            this.LastDrillUpTime = DateTime.Today;
            this.LastOpenPack = DateTime.Today;
            this.LastMovePlaceItem = DateTime.Today;
            this.m_showPP = false;
            this.m_converter = new UTF8Encoding();
            this.BossBoxStartTime = DateTime.Now;
            this.ResetLottery();
            this.CardResetTempProp = new Dictionary<int, int[]>();
            this.userLabyrinthInfo = (UserLabyrinthInfo)null;
            this.m_consortiaProcessor = new ConsortiaLogicProcessor();
            this.m_petProcessor = new PetLogicProcessor();
            this.LastOpenHole = DateTime.Now;
        }

        public int AddAchievementPoint(int value)
        {
            if (value <= 0)
                return 0;
            this.m_character.AchievementPoint += value;
            this.OnPropertiesChanged();
            return value;
        }

        public void SendUpdatePublicPlayer()
        {
            this.Out.SendUpdatePublicPlayer(this.PlayerCharacter, this.MatchInfo, this.Extra.Info);
        }

        public void AddExpVip(int value)
        {
            List<int> list = GameProperties.VIPExp();

            if (value >= 100)
            {
                this.m_character.VIPExp += value / 100;
            }
            else if(Level <9)
            {
                this.m_character.VIPExp += value;
            }

            for (int i = 0; i < list.Count; i++)
            {
                int vIPExp = this.m_character.VIPExp;
                int vIPLevel = this.m_character.VIPLevel;
                if ((vIPLevel < 9) && this.canUpLv(vIPExp, vIPLevel))
                {
                    this.m_character.VIPLevel++;
                }
            }
            if (!this.m_character.IsVIPExpire())
                return;
            
            this.Out.SendOpenVIP(this);
        }

        public int AddGold(int value)
        {
            if (value <= 0)
                return 0;
            this.m_character.Gold += value;
            if (this.m_character.Gold == int.MinValue)
            {
                this.m_character.Gold = int.MaxValue;
                this.SendMessage("Limite excedido!");
            }
            this.OnPlayerAddItem("Gold", value);
            this.OnPropertiesChanged();
            return value;
        }

        public int AddGP(int gp)
        {
            if (gp < 0)
                return 0;
            if (AntiAddictionMgr.ISASSon)
                gp = (int)((double)gp * AntiAddictionMgr.GetAntiAddictionCoefficient(this.PlayerCharacter.AntiAddiction));
            gp = (int)((double)gp * (double)RateMgr.GetRate(eRateType.Experience_Rate));
            if (this.GPAddPlus > 0.0)
                gp = (int)((double)gp * this.GPAddPlus);
            this.m_character.GP += gp;
            if (this.m_character.GP < 1)
                this.m_character.GP = 1;
            this.Level = LevelMgr.GetLevel(this.m_character.GP);
            int maxLevel = LevelMgr.MaxLevel;
            LevelInfo level = LevelMgr.FindLevel(maxLevel);
            if (this.Level == maxLevel && level != null)
            {
                this.m_character.GP = level.GP;
                int num = gp / 100;
                if (num > 0)
                {
                    this.AddOffer(num);
                    this.SendHideMessage(string.Format("Seu nível atingiu o nível mais alto, portanto a Experiência extra será convertida em mérito.", (object)num));
                }
            }
            this.UpdateFightPower();
            this.OnPropertiesChanged();
            return gp;
        }

        public void AddGift(eGiftType type)
        {
            List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
            bool testActive = GameProperties.TestActive;
            switch (type)
            {
                case eGiftType.MONEY:
                    if (testActive)
                    {
                        this.AddMoney(GameProperties.FreeMoney);
                        break;
                    }
                    break;
                case eGiftType.SMALL_EXP:
                    string[] strArray1 = GameProperties.FreeExp.Split('|');
                    ItemTemplateInfo itemTemplate1 = ItemMgr.FindItemTemplate(Convert.ToInt32(strArray1[0]));
                    if (itemTemplate1 != null)
                    {
                        itemInfoList.Add(SqlDataProvider.Data.ItemInfo.CreateFromTemplate(itemTemplate1, Convert.ToInt32(strArray1[1]), 102));
                        break;
                    }
                    break;
                case eGiftType.BIG_EXP:
                    string[] strArray2 = GameProperties.BigExp.Split('|');
                    ItemTemplateInfo itemTemplate2 = ItemMgr.FindItemTemplate(Convert.ToInt32(strArray2[0]));
                    if (itemTemplate2 != null & testActive)
                    {
                        itemInfoList.Add(SqlDataProvider.Data.ItemInfo.CreateFromTemplate(itemTemplate2, Convert.ToInt32(strArray2[1]), 102));
                        break;
                    }
                    break;
                case eGiftType.PET_EXP:
                    string[] strArray3 = GameProperties.PetExp.Split('|');
                    ItemTemplateInfo itemTemplate3 = ItemMgr.FindItemTemplate(Convert.ToInt32(strArray3[0]));
                    if (itemTemplate3 != null & testActive)
                    {
                        itemInfoList.Add(SqlDataProvider.Data.ItemInfo.CreateFromTemplate(itemTemplate3, Convert.ToInt32(strArray3[1]), 102));
                        break;
                    }
                    break;
            }
            foreach (SqlDataProvider.Data.ItemInfo cloneItem in itemInfoList)
            {
                cloneItem.IsBinds = true;
                this.AddTemplate(cloneItem, cloneItem.Template.BagType, cloneItem.Count, eGameView.dungeonTypeGet);
            }
        }

        public int AddGiftToken(int value)
        {
            if (value <= 0)
                return 0;
            this.m_character.GiftToken += value;
            this.OnPlayerAddItem("GiftToken", value);
            this.OnPropertiesChanged();
            return value;
        }

        public bool AddItem(SqlDataProvider.Data.ItemInfo item)
        {
            AbstractInventory itemInventory = (AbstractInventory)this.GetItemInventory(item.Template);
            return itemInventory.AddItem(item, itemInventory.BeginSlot);
        }

        public int AddLeagueMoney(int value)
        {
            if (value <= 0)
                return 0;
            this.m_battle.MatchInfo.dailyScore += value;
            this.m_battle.MatchInfo.weeklyScore += value;
            this.OnPropertiesChanged();
            return value;
        }

        public void AddLog(string type, string content)
        {
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
                playerBussiness.AddUserLogEvent(this.PlayerCharacter.ID, this.PlayerCharacter.UserName, this.PlayerCharacter.NickName, type, content);
        }

        public int AddMoney(int value)
        {
            if (value <= 0)
                return 0;
            this.m_character.Money += value;
            if (this.m_character.Money == int.MinValue)
            {
                this.m_character.Money = int.MaxValue;
                this.SendMessage("Limite de cupons excedido.");
            }
            this.OnPropertiesChanged();
            return value;
        }

        public int AddBadLuckCaddy(int value)
        {
            if (value <= 0)
                return 0;
            this.m_character.badLuckNumber += value;
            if (this.m_character.badLuckNumber == int.MinValue)
            {
                this.m_character.badLuckNumber = int.MaxValue;
                this.SendMessage("Limite de gemas excedido.");
            }
            this.OnPropertiesChanged();
            return value;
        }

        public int AddOffer(int value)
        {
            return this.AddOffer(value, true);
        }

        public int AddHonor(int value)
        {
            if (value <= 0)
                return 0;
            this.m_character.myHonor += value;
            this.OnPropertiesChanged();
            return value;
        }

        public int AddMaxHonor(int value)
        {
            if (value <= 0)
                return 0;
            this.m_character.MaxBuyHonor += value;
            this.OnPropertiesChanged();
            return value;
        }

        public int AddmyHonor(int value)
        {
            if (value <= 0)
                return 0;
            this.m_character.myHonor += value;
            this.OnPropertiesChanged();
            return value;
        }

        public int AddTotem(int value)
        {
            if (value <= 0)
                return this.m_character.totemId;
            this.m_character.totemId = value;
            this.OnPropertiesChanged();
            return value;
        }

        public int RefreshLeagueGetReward(int awardGot, int Score)
        {
            if (awardGot <= 0)
                return 0;
            this.MatchInfo.leagueItemsGet = awardGot;
            this.MatchInfo.weeklyScore -= Score;
            this.OnPropertiesChanged();
            return awardGot;
        }

        public int AddOffer(int value, bool IsRate)
        {
            if (value <= 0)
                return 0;
            if (AntiAddictionMgr.ISASSon)
                value = (int)((double)value * AntiAddictionMgr.GetAntiAddictionCoefficient(this.PlayerCharacter.AntiAddiction));
            if (IsRate)
                value *= (int)this.OfferAddPlus == 0 ? 1 : (int)this.OfferAddPlus;
            this.m_character.Offer += value;
            this.OnFightAddOffer(value);
            this.OnPropertiesChanged();
            return value;
        }

        public int AddPetScore(int value)
        {
            if (value <= 0)
                return 0;
            this.m_character.petScore += value;
            if (this.m_character.petScore == int.MinValue)
            {
                this.m_character.petScore = int.MaxValue;
                this.SendMessage("Limite excedido.");
            }
            this.OnPropertiesChanged();
            return value;
        }

        public void AddPrestige(bool isWin)
        {
            this.BattleData.AddPrestige(isWin);
        }

        public int AddRichesOffer(int value)
        {
            if (value <= 0)
                return 0;
            this.m_character.RichesOffer += value;
            this.OnPropertiesChanged();
            return value;
        }

        public int AddRobRiches(int value)
        {
            if (value <= 0)
                return 0;
            if (AntiAddictionMgr.ISASSon)
                value = (int)((double)value * AntiAddictionMgr.GetAntiAddictionCoefficient(this.PlayerCharacter.AntiAddiction));
            this.m_character.RichesRob += value;
            this.OnPlayerAddItem("RichesRob", value);
            this.OnPropertiesChanged();
            return value;
        }

        public int AddScore(int value)
        {
            if (value <= 0)
                return 0;
            this.m_character.Score += value;
            this.OnPropertiesChanged();
            return value;
        }

        public bool AddTemplate(SqlDataProvider.Data.ItemInfo cloneItem)
        {
            return this.AddTemplate(cloneItem, cloneItem.Template.BagType, cloneItem.Count, eGameView.OtherTypeGet);
        }

        public bool AddTemplate(List<SqlDataProvider.Data.ItemInfo> infos)
        {
            return this.AddTemplate(infos, eGameView.OtherTypeGet);
        }

        public bool AddTemplate(SqlDataProvider.Data.ItemInfo cloneItem, string name)
        {
            return this.AddTemplate(cloneItem, cloneItem.Template.BagType, cloneItem.Count, eGameView.OtherTypeGet, name);
        }

        public bool AddTemplate(List<SqlDataProvider.Data.ItemInfo> infos, eGameView typeGet)
        {
            if (infos == null)
                return false;
            List<SqlDataProvider.Data.ItemInfo> infos1 = new List<SqlDataProvider.Data.ItemInfo>();
            foreach (SqlDataProvider.Data.ItemInfo info in infos)
            {
                info.IsBinds = true;
                if (!this.StackItemToAnother(info) && !this.AddItem(info))
                    infos1.Add(info);
            }
            this.BagFullSendToMail(infos1);
            return true;
        }

        public bool AddTemplate(List<SqlDataProvider.Data.ItemInfo> infos, int count, eGameView gameView)
        {
            if (infos == null)
                return false;
            List<SqlDataProvider.Data.ItemInfo> infos1 = new List<SqlDataProvider.Data.ItemInfo>();
            foreach (SqlDataProvider.Data.ItemInfo info in infos)
            {
                info.IsBinds = true;
                info.Count = count;
                if (!this.StackItemToAnother(info) && !this.AddItem(info))
                    infos1.Add(info);
            }
            this.BagFullSendToMail(infos1);
            return true;
        }

        public bool AddTemplate(SqlDataProvider.Data.ItemInfo cloneItem, eBageType bagType, int count, eGameView gameView)
        {
            if (eBageType.FightBag == bagType)
                return this.FightBag.AddItem(cloneItem);
            return this.AddTemplate(cloneItem, bagType, count, gameView, "no");
        }

        public bool AddTemplate(
          SqlDataProvider.Data.ItemInfo cloneItem,
          eBageType bagType,
          int count,
          eGameView gameView,
          string Name)
        {
            PlayerInventory inventory = this.GetInventory(bagType);
            if (cloneItem == null)
                return false;
            List<SqlDataProvider.Data.ItemInfo> infos = new List<SqlDataProvider.Data.ItemInfo>();
            if (!inventory.StackItemToAnother(cloneItem) && !inventory.AddItem(cloneItem))
                infos.Add(cloneItem);
            this.BagFullSendToMail(infos);
            if (Name != "no")
                this.SendItemNotice(cloneItem, (int)gameView, Name);
            return true;
        }

        public bool AddTemplate(SqlDataProvider.Data.ItemInfo cloneItem, eBageType bagType, int count, bool backToMail)
        {
            PlayerInventory inventory = this.GetInventory(bagType);
            if (inventory != null && !cloneItem.Template.IsSpecial())
            {
                if (inventory.AddTemplate(cloneItem, count))
                {
                    if (this.CurrentRoom != null && this.CurrentRoom.IsPlaying)
                        this.SendItemNotice(cloneItem);
                    return true;
                }
                if (backToMail && cloneItem.Template.CategoryID != 10)
                    this.SendItemsToMail(cloneItem, LanguageMgr.GetTranslation("GamePlayer.Msg18"), LanguageMgr.GetTranslation("GamePlayer.Msg18"), eMailType.BuyItem);
            }
            return false;
        }

        public bool RemoveTemplateInShop(int templateid, int count)
        {
            ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(templateid);
            if (itemTemplate != null)
            {
                PlayerInventory itemInventory = this.GetItemInventory(itemTemplate);
                if (itemInventory != null)
                    return itemInventory.RemoveTemplate(templateid, count);
            }
            return false;
        }

        public int GetTemplateCount(int templateId)
        {
            ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(templateId);
            if (itemTemplate != null)
            {
                PlayerInventory itemInventory = this.GetItemInventory(itemTemplate);
                if (itemInventory != null)
                    return itemInventory.GetItemCount(templateId);
            }
            return 0;
        }

        private void SendItemNotice(SqlDataProvider.Data.ItemInfo item)
        {
            GSPacketIn gsPacketIn = new GSPacketIn((short)14);
            gsPacketIn.WriteString(this.PlayerCharacter.NickName);
            gsPacketIn.WriteInt(1);
            gsPacketIn.WriteInt(item.TemplateID);
            gsPacketIn.WriteBoolean(item.IsBinds);
            gsPacketIn.WriteInt(1);
            if (item.Template.Quality >= 3 && item.Template.Quality < 5)
            {
                if (this.CurrentRoom == null)
                    return;
                this.CurrentRoom.SendToTeam(gsPacketIn, this.CurrentRoomTeam, this);
            }
            else
            {
                if (item.Template.Quality < 5)
                    return;
                GameServer.Instance.LoginServer.SendPacket(gsPacketIn);
                foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                {
                    if (allPlayer != this)
                        allPlayer.Out.SendTCP(gsPacketIn);
                }
            }
        }

        public void ApertureEquip(int level)
        {
            this.EquipShowImp(0, level < 5 ? 1 : (level < 7 ? 2 : 3));
        }

        public void BagFullSendToMail(List<SqlDataProvider.Data.ItemInfo> infos)
        {
            if (infos.Count <= 0)
                return;
            bool flag = false;
            using (new PlayerBussiness())
                flag = this.SendItemsToMail(infos, "Mochila cheia", "Mochila cheia", eMailType.BuyItem);
            if (!flag)
                return;
            this.Out.SendMailResponse(this.PlayerCharacter.ID, eMailRespose.Receiver);
        }

        public void BagFullSendToMail(ItemInfo infos)
        {
            if (infos.Count == null)
                return;
            bool flag = false;
            using (new PlayerBussiness())
                flag = this.SendItemsToMail(infos, "Mochila cheia", "Mochila cheia", eMailType.BuyItem);
            if (!flag)
                return;
            this.Out.SendMailResponse(this.PlayerCharacter.ID, eMailRespose.Receiver);
        }

        public void BeginAllChanges()
        {
            this.BeginChanges();
            this.m_bufferList.BeginChanges();
            this.m_equipBag.BeginChanges();
            this.m_propBag.BeginChanges();
            this.m_farmBag.BeginChanges();
        }

        public void BeginChanges()
        {
            Interlocked.Increment(ref this.m_changed);
        }

        public void RemoveLotteryItems(int templateId, int count)
        {
            foreach (ItemBoxInfo lotteryItem in this.LotteryItems)
            {
                if (lotteryItem.TemplateId == templateId && lotteryItem.ItemCount == count)
                {
                    this.LotteryItems.Remove(lotteryItem);
                    break;
                }
            }
        }

        public bool CanEquip(ItemTemplateInfo item)
        {
            bool flag = true;
            string message = "";
            if (!item.CanEquip)
            {
                flag = false;
                message = LanguageMgr.GetTranslation("Game.Server.GameObjects.NoEquip");
            }
            else if (this.m_character.Grade < item.NeedLevel)
            {
                flag = false;
                message = LanguageMgr.GetTranslation("Game.Server.GameObjects.CanLevel");
            }
            if (!flag)
                this.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, message);
            return flag;
        }

        public int GetVIPNextLevelDaysNeeded(int viplevel, int vipexp)
        {
            int exp = 10;
            if(this.m_character.typeVIP >= 2)
            {
                exp = 15;
            }
            

            if (viplevel != 0 && vipexp > 0 && viplevel <= 8)
            {
                int num = (int)Math.Round(((Array.ConvertAll<string, double>("0, 100, 250, 550, 1250, 2200, 3100, 4100, 5650".Split(','), new Converter<string, double>(double.Parse))[viplevel] - vipexp) / exp));
                this.OnVIPUpgrade(this.m_character.VIPLevel, this.m_character.VIPExp);
                return num;
            }
            this.OnVIPUpgrade(this.m_character.VIPLevel, this.m_character.VIPExp);
            return (int)Math.Round(((Array.ConvertAll<string, double>("0, 100, 250, 550, 1250, 2200, 3100, 4100, 5650".Split(','), new Converter<string, double>(double.Parse))[viplevel] - vipexp) / exp));
        }

        public bool canUpLv(int exp, int _curLv)
        {
            List<int> intList = GameProperties.VIPExp();
            if (exp >= intList[0] && _curLv == 0 || exp >= intList[1] && _curLv == 1 || (exp >= intList[2] && _curLv == 2 || exp >= intList[3] && _curLv == 3) || (exp >= intList[4] && _curLv == 4 || exp >= intList[5] && _curLv == 5 || (exp >= intList[6] && _curLv == 6 || exp >= intList[7] && _curLv == 7)))
                return true;
            if (exp >= intList[8])
                return _curLv == 8;
            return false;
        }

        public void ClearCaddyBag()
        {
            List<SqlDataProvider.Data.ItemInfo> infos = new List<SqlDataProvider.Data.ItemInfo>();
            for (int slot = 0; slot < this.CaddyBag.Capalility; ++slot)
            {
                SqlDataProvider.Data.ItemInfo itemAt = this.CaddyBag.GetItemAt(slot);
                if (itemAt != null)
                {
                    SqlDataProvider.Data.ItemInfo itemInfo = SqlDataProvider.Data.ItemInfo.CloneFromTemplate(itemAt.Template, itemAt);
                    itemInfo.Count = 1;
                    infos.Add(itemInfo);
                }
            }
            this.CaddyBag.ClearBag();
            this.AddTemplate(infos);
        }

        public int GetMedalNum()
        {
            int itemCount = this.PropBag.GetItemCount(11408);
            if (this.m_character.IsConsortia)
                itemCount += this.ConsortiaBag.GetItemCount(11408);
            return itemCount;
        }

        public bool SendEventLiveRewards(EventLiveInfo eventLiveInfo)
        {
            List<EventLiveGoods> eventGoods = EventLiveMgr.GetEventGoods(eventLiveInfo);
            List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
            foreach (EventLiveGoods eventLiveGoods in eventGoods)
            {
                if (eventLiveGoods.TemplateID != -100 && eventLiveGoods.TemplateID != -200)
                {
                    ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(eventLiveGoods.TemplateID);
                    if (itemTemplate != null)
                    {
                        int num = this.PlayerCharacter.Sex ? 1 : 2;
                        if (itemTemplate.NeedSex == 0 || itemTemplate.NeedSex == num)
                        {
                            int count1 = eventLiveGoods.Count;
                            for (int index = 0; index < count1; index += itemTemplate.MaxCount)
                            {
                                int count2 = index + itemTemplate.MaxCount > count1 ? count1 - index : itemTemplate.MaxCount;
                                SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(itemTemplate, count2, 120);
                                if (fromTemplate != null)
                                {
                                    fromTemplate.StrengthenLevel = eventLiveGoods.StrengthenLevel;
                                    fromTemplate.AttackCompose = eventLiveGoods.AttackCompose;
                                    fromTemplate.DefendCompose = eventLiveGoods.DefendCompose;
                                    fromTemplate.AgilityCompose = eventLiveGoods.AgilityCompose;
                                    fromTemplate.LuckCompose = eventLiveGoods.LuckCompose;
                                    fromTemplate.IsBinds = eventLiveGoods.IsBind;
                                    fromTemplate.ValidDate = eventLiveGoods.ValidDate;
                                    this.SendItemToMail(fromTemplate, LanguageMgr.GetTranslation("Game.Server.GameObjects.SendEventLiveRewards.Content", (object)eventLiveInfo.Description), LanguageMgr.GetTranslation("Game.Server.GameObjects.SendEventLiveRewards.Title"), eMailType.Manage);
                                }
                            }
                        }
                        else
                            continue;
                    }
                }
                if (eventLiveGoods.TemplateID == -100)
                    this.AddGold(eventLiveGoods.Count);
                if (eventLiveGoods.TemplateID == -200)
                    this.AddMoney(eventLiveGoods.Count);
                if (eventLiveGoods.TemplateID == -300)
                    this.AddGiftToken(eventLiveGoods.Count);
            }
            return true;
        }

        public void ClearConsortia(bool isclear)
        {
            if (isclear)
                this.PlayerCharacter.ClearConsortia();
            if (this.PlayerCharacter.ConsortiaID != 0 || this.ConsortiaBag.GetItems().Count <= 0)
                return;
            List<SqlDataProvider.Data.ItemInfo> items = new List<SqlDataProvider.Data.ItemInfo>();
            foreach (SqlDataProvider.Data.ItemInfo itemInfo in this.ConsortiaBag.GetItems())
            {
                if (itemInfo.IsValidItem())
                    items.Add(itemInfo);
            }
            this.OnPropertiesChanged();
            this.QuestInventory.ClearConsortiaQuest();
            string translation1 = LanguageMgr.GetTranslation("Game.Server.GameUtils.CommonBag.Sender");
            string translation2 = LanguageMgr.GetTranslation("Game.Server.GameUtils.Title");
            if (!this.SendItemsToMail(items, translation1, translation2, eMailType.StoreCanel))
                return;
            this.ConsortiaBag.ClearBag();
        }

        public bool ClearFightBag()
        {
            this.FightBag.ClearBag();
            return true;
        }

        public void ClearFightBuffOneMatch()
        {
            List<BufferInfo> bufferInfoList = new List<BufferInfo>();
            foreach (BufferInfo fightBuff in this.FightBuffs)
            {
                if (fightBuff != null && fightBuff.Type >= 400 && fightBuff.Type <= 406)
                    bufferInfoList.Add(fightBuff);
            }
            foreach (BufferInfo bufferInfo in bufferInfoList)
                this.FightBuffs.Remove(bufferInfo);
            bufferInfoList.Clear();
        }

        public void ClearFootballCard()
        {
            for (int index = 0; index < this.CardsTakeOut.Length; ++index)
                this.CardsTakeOut[index] = (CardInfo)null;
        }

        public void ClearStoreBag()
        {
            for (int slot = 0; slot < this.StoreBag.Capalility; ++slot)
            {
                SqlDataProvider.Data.ItemInfo itemAt = this.StoreBag.GetItemAt(slot);
                if (itemAt != null)
                {
                    if ((uint)itemAt.Template.BagType > 0U)
                    {
                        PlayerInventory inventory = this.GetInventory(itemAt.Template.BagType);
                        int firstEmptySlot = inventory.FindFirstEmptySlot();
                        if (inventory.StackItemToAnother(itemAt) || inventory.AddItemTo(itemAt, firstEmptySlot))
                            this.StoreBag.TakeOutItem(itemAt);
                    }
                    else
                    {
                        int firstEmptySlot = this.EquipBag.FindFirstEmptySlot();
                        if (this.EquipBag.StackItemToAnother(itemAt) || this.EquipBag.AddItemTo(itemAt, firstEmptySlot))
                            this.StoreBag.TakeOutItem(itemAt);
                    }
                }
            }
            if (this.StoreBag.GetItems().Count <= 0)
                return;
            this.StoreBag.SendAllItemsToMail("Sistema do Ferreiro", "A mochila está cheia, os itens foram devolvidos", eMailType.StoreCanel);
        }

        public bool ClearTempBag()
        {
            this.TempBag.ClearBag();
            return true;
        }

        public void CommitAllChanges()
        {
            this.CommitChanges();
            this.m_bufferList.CommitChanges();
            this.m_equipBag.CommitChanges();
            this.m_propBag.CommitChanges();
            this.m_farmBag.CommitChanges();
        }

        public void CommitChanges()
        {
            Interlocked.Decrement(ref this.m_changed);
            this.OnPropertiesChanged();
        }

        public int ConsortiaFight(
          int consortiaWin,
          int consortiaLose,
          Dictionary<int, Player> players,
          eRoomType roomType,
          eGameType gameClass,
          int totalKillHealth,
          int count)
        {
            return ConsortiaMgr.ConsortiaFight(consortiaWin, consortiaLose, players, roomType, gameClass, totalKillHealth, count);
        }

        public void ContinuousVIP(int days, DateTime ExpireDayOut)
        {
            this.m_character.VIPExpireDay = ExpireDayOut;
            this.m_character.typeVIP = this.SetTypeVIP(days);
            
        }

        public string ConverterPvePermission(char[] chArray)
        {
            string str = "";
            for (int index = 0; index < chArray.Length; ++index)
                str += chArray[index].ToString();
            return str;
        }

        public List<SqlDataProvider.Data.ItemInfo> CopyDrop(
          int SessionId,
          int m_missionInfoId)
        {
            List<SqlDataProvider.Data.ItemInfo> info = (List<SqlDataProvider.Data.ItemInfo>)null;
            DropInventory.CopyDrop(m_missionInfoId, SessionId, ref info);
            return info;
        }

        public bool MoneyDirect(int value)
        {
            if (GameProperties.IsDDTMoneyActive)
                return this.MoneyDirect(MoneyType.DDTMoney, value);
            return this.MoneyDirect(MoneyType.Money, value);
        }

        public bool MoneyDirect(MoneyType type, int value)
        {
            if (value < 0)
                return false;
            if (type == MoneyType.Money)
            {
                if (this.PlayerCharacter.Money < value)
                {
                    this.SendInsufficientMoney(0);
                    return false;
                }
                this.RemoveMoney(value);
                return true;
            }
            this.RemoveGiftToken(value);
            return true;
        }

        public void ChargeToUser()
        {
            using (PlayerBussiness pb = new PlayerBussiness())
            {
                int money = 0;
                string translation1 = LanguageMgr.GetTranslation("ChargeToUser.Title");
                if (!pb.ChargeToUser(this.m_character.UserName, ref money, this.m_character.NickName))
                    return;
                string translation2 = LanguageMgr.GetTranslation("ChargeToUser.Content", (object)money);
                if (money <= 0)
                    return;
                this.SendMailToUser(pb, translation2, translation1, eMailType.Manage);
                this.AddMoney(money);
                this.OnMoneyCharge(money);
                this.Out.SendMailResponse(this.PlayerCharacter.ID, eMailRespose.Receiver);
                if (this.Extra.CheckNoviceActiveOpen(NoviceActiveType.RECHANGE_MONEY_ACTIVE))
                    this.Extra.UpdateEventCondition(4, money, true, 0);
                this.PlayerCharacter.IsRecharged = true;
                this.Out.SendUpdateFirstRecharge(this.PlayerCharacter.IsRecharged, this.PlayerCharacter.IsGetAward);
                //Out.SendLeftRouleteOpen(Extra.Info);
            }
        }

        public void ChecVipkExpireDay()
        {
            if (this.m_character.IsVIPExpire())
            {
                this.m_character.CanTakeVipReward = false;
                this.m_character.typeVIP = 0;
            }
            else if (this.m_character.IsLastVIPPackTime())
                this.m_character.CanTakeVipReward = true;
            else
                this.m_character.CanTakeVipReward = false;
        }

        public bool DeletePropItem(int place)
        {
            this.FightBag.RemoveItemAt(place);
            return true;
        }

        public void Disconnect()
        {
            this.SendMessage("Você Foi Desconectado.");
            this.m_client.Disconnect();

        }


        private void EquipShowImp(int categoryID, int para)
        {
            this.UpdateHide(this.m_character.Hide + (int)(Math.Pow(10.0, (double)categoryID) * (double)(para - this.m_character.Hide / (int)Math.Pow(10.0, (double)categoryID) % 10)));
        }

        public bool FindEmptySlot(eBageType bagType)
        {
            PlayerInventory inventory = this.GetInventory(bagType);
            inventory.FindFirstEmptySlot();
            return inventory.FindFirstEmptySlot() > 0;
        }

        public void FriendsAdd(int playerID, int relation)
        {
            if (!this._friends.ContainsKey(playerID))
                this._friends.Add(playerID, relation);
            else
                this._friends[playerID] = relation;
        }

        public void FriendsRemove(int playerID)
        {
            if (!this._friends.ContainsKey(playerID))
                return;
            this._friends.Remove(playerID);
        }

        public double GetBaseAgility()
        {
            return 1.0 - (double)this.m_character.Agility * 0.001;
        }

        public double GPApprenticeOnline
        {
            get
            {
                if (this.m_character.MasterOrApprenticesArr.Count > 0)
                {
                    foreach (KeyValuePair<int, string> keyValuePair in this.m_character.MasterOrApprenticesArr)
                    {
                        if (WorldMgr.GetPlayerById(keyValuePair.Key) != null)
                            return 0.05;
                    }
                }
                return 0.0;
            }
            set
            {
            }
        }

        public double GPApprenticeTeam
        {
            get
            {
                if (this.CurrentRoom != null)
                {
                    foreach (GamePlayer player in this.CurrentRoom.GetPlayers())
                    {
                        if (player != this && player.PlayerCharacter.MasterOrApprenticesArr.ContainsKey(this.PlayerId))
                            return 0.1;
                    }
                }
                return 0.0;
            }
            set
            {
            }
        }

        public double GPSpouseTeam
        {
            get
            {
                if (this.CurrentRoom != null)
                {
                    foreach (GamePlayer player in this.CurrentRoom.GetPlayers())
                    {
                        if (player != this && player.PlayerCharacter.SpouseID == this.PlayerId)
                            return 0.05;
                    }
                }
                return 0.0;
            }
            set
            {
            }
        }

        public double GetBaseAttack()
        {
            int num1 = 0;
            int num2 = 0;
            foreach (UsersCardInfo card in this.CardBag.GetCards(0, 4))
            {
                ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(card.TemplateID);
                if (itemTemplate != null)
                    num2 += itemTemplate.Property4 + card.Damage;
            }
            int baseattack = num1 + num2;
            SqlDataProvider.Data.ItemInfo itemAt1 = this.m_equipBag.GetItemAt(6);
            if (itemAt1 != null)
            {
                double property7 = (double)itemAt1.Template.Property7;
                int num3 = itemAt1.isGold ? 1 : 0;
                double para2 = (double)(itemAt1.StrengthenLevel + num3);
                baseattack += (int)(this.getHertAddition(property7, para2) + property7);
                if (itemAt1.Hole1 > 0)
                    this.BaseAttack(itemAt1.Hole1, ref baseattack);
                if (itemAt1.Hole2 > 0)
                    this.BaseAttack(itemAt1.Hole2, ref baseattack);
                if (itemAt1.Hole3 > 0)
                    this.BaseAttack(itemAt1.Hole3, ref baseattack);
                if (itemAt1.Hole4 > 0)
                    this.BaseAttack(itemAt1.Hole4, ref baseattack);
                if (itemAt1.Hole5 > 0)
                    this.BaseAttack(itemAt1.Hole5, ref baseattack);
                if (itemAt1.Hole6 > 0)
                    this.BaseAttack(itemAt1.Hole6, ref baseattack);
            }
            SqlDataProvider.Data.ItemInfo itemAt2 = this.m_equipBag.GetItemAt(0);
            SqlDataProvider.Data.ItemInfo itemAt3 = this.m_equipBag.GetItemAt(4);
            if (itemAt2 != null)
            {
                if (itemAt2.Hole1 > 0)
                    this.BaseAttack(itemAt2.Hole1, ref baseattack);
                if (itemAt2.Hole2 > 0)
                    this.BaseAttack(itemAt2.Hole2, ref baseattack);
                if (itemAt2.Hole3 > 0)
                    this.BaseAttack(itemAt2.Hole3, ref baseattack);
                if (itemAt2.Hole4 > 0)
                    this.BaseAttack(itemAt2.Hole4, ref baseattack);
                if (itemAt2.Hole5 > 0)
                    this.BaseAttack(itemAt2.Hole5, ref baseattack);
                if (itemAt2.Hole6 > 0)
                    this.BaseAttack(itemAt2.Hole6, ref baseattack);
            }
            if (itemAt3 != null)
            {
                if (itemAt3.Hole1 > 0)
                    this.BaseAttack(itemAt3.Hole1, ref baseattack);
                if (itemAt3.Hole2 > 0)
                    this.BaseAttack(itemAt3.Hole2, ref baseattack);
                if (itemAt3.Hole3 > 0)
                    this.BaseAttack(itemAt3.Hole3, ref baseattack);
                if (itemAt3.Hole4 > 0)
                    this.BaseAttack(itemAt3.Hole4, ref baseattack);
                if (itemAt3.Hole5 > 0)
                    this.BaseAttack(itemAt3.Hole5, ref baseattack);
                if (itemAt3.Hole6 > 0)
                    this.BaseAttack(itemAt3.Hole6, ref baseattack);
            }
            return (double)baseattack;
        }

        public void BaseAttack(int template, ref int baseattack)
        {
            ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(template);
            if (itemTemplate == null || itemTemplate.CategoryID != 11 || (itemTemplate.Property1 != 31 || itemTemplate.Property2 != 3))
                return;
            baseattack += itemTemplate.Property7;
        }

        public double GetBaseBlood()
        {
            SqlDataProvider.Data.ItemInfo itemAt = this.EquipBag.GetItemAt(12);
            if (itemAt != null)
                return (100.0 + (double)itemAt.Template.Property1) / 100.0;
            return 1.0;
        }

        public double GetBaseDefence()
        {
            int defence = 0;
            int num1 = 0;
            double num5 = 0;
            EatPetsInfo eatPets = this.PetBag.EatPets;//#7 For EatPets
            if (eatPets != null)
            {
                PetMoePropertyInfo petMoeProperty = PetMoePropertyMgr.FindPetMoeProperty(eatPets.hatLevel);
                if (petMoeProperty != null)
                    num5 += (double)petMoeProperty.Guard;
            }
            this.PlayerProp.UpadateBaseProp(true, "Armor", "Pet", num5);
            foreach (UsersCardInfo card in this.CardBag.GetCards(0, 4))
            {
                ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(card.TemplateID);
                if (itemTemplate != null)
                    num1 += itemTemplate.Property5 + card.Guard;
            }
            //int basedefence = (double)TotemMgr.GetTotemProp(this.m_character.totemId, "gua");
            SqlDataProvider.Data.ItemInfo itemAt1 = this.m_equipBag.GetItemAt(6);
            SqlDataProvider.Data.ItemInfo itemAt2 = this.m_equipBag.GetItemAt(0);
            SqlDataProvider.Data.ItemInfo itemAt3 = this.m_equipBag.GetItemAt(4);
            if (itemAt2 != null)
            {
                double property7 = (double)itemAt2.Template.Property7;
                int num2 = itemAt2.isGold ? 1 : 0;
                double para2 = (double)(itemAt2.StrengthenLevel + num2);
                defence += (int)(this.getHertAddition(property7, para2) + property7);
                this.AddProperty(itemAt2, ref defence);
            }
            if (itemAt3 != null)
            {
                double property7 = (double)itemAt3.Template.Property7;
                int num2 = itemAt3.isGold ? 1 : 0;
                double para2 = (double)(itemAt3.StrengthenLevel + num2);
                defence += (int)(this.getHertAddition(property7, para2) + property7);
                this.AddProperty(itemAt3, ref defence);
            }
            if (itemAt1 != null)
                this.AddProperty(itemAt1, ref defence);
            return (double)(defence + num1);
        }

        public void AddProperty(SqlDataProvider.Data.ItemInfo item, ref int defence)
        {
            if (item.Hole1 > 0)
                this.BaseDefence(item.Hole1, ref defence);
            if (item.Hole2 > 0)
                this.BaseDefence(item.Hole2, ref defence);
            if (item.Hole3 > 0)
                this.BaseDefence(item.Hole3, ref defence);
            if (item.Hole4 > 0)
                this.BaseDefence(item.Hole4, ref defence);
            if (item.Hole5 > 0)
                this.BaseDefence(item.Hole5, ref defence);
            if (item.Hole6 <= 0)
                return;
            this.BaseDefence(item.Hole6, ref defence);
        }

        public void BaseDefence(int template, ref int defence)
        {
            ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(template);
            if (itemTemplate == null || itemTemplate.CategoryID != 11 || (itemTemplate.Property1 != 31 || itemTemplate.Property2 != 3))
                return;
            defence += itemTemplate.Property8;
        }

        public void PVEFightMessage(string translation, SqlDataProvider.Data.ItemInfo itemInfo, int areaID)
        {
            if (translation == null)
                return;
            GameServer.Instance.LoginServer.SendPacket(WorldMgr.SendSysNotice(eMessageType.ChatNormal, translation, itemInfo.ItemID == 0 ? 1 : itemInfo.ItemID, itemInfo.TemplateID, ""));
        }

        public double getHertAddition(double para1, double para2)
        {
            return Math.Round(para1 * Math.Pow(1.1, para2) - para1);
        }

        public PlayerInventory GetInventory(eBageType bageType)
        {
            switch (bageType)
            {
                case eBageType.EquipBag:
                    return (PlayerInventory)this.m_equipBag;
                case eBageType.PropBag:
                    return this.m_propBag;
                case eBageType.FightBag:
                    return this.m_fightBag;
                case eBageType.TempBag:
                    return this.m_tempBag;
                case eBageType.CaddyBag:
                    return this.m_caddyBag;
                case eBageType.Consortia:
                    return this.m_ConsortiaBag;
                case eBageType.Store:
                    return this.m_storeBag;
                case eBageType.FarmBag:
                    return this.m_farmBag;
                case eBageType.Food:
                    return this.m_food;
                case eBageType.PetEgg:
                    return this.m_petEgg;
                case eBageType.MagicStone:
                    return (PlayerInventory)null;
                default:
                    throw new NotSupportedException(string.Format("Did not support this type bag: {0} PlayerID: {1} Nickname: {2}", (object)bageType, (object)this.PlayerCharacter.ID, (object)this.PlayerCharacter.NickName));
            }
        }

        public string GetInventoryName(eBageType bageType)
        {
            switch (bageType)
            {
                case eBageType.EquipBag:
                    return LanguageMgr.GetTranslation("Game.Server.GameObjects.Equip");
                case eBageType.PropBag:
                    return LanguageMgr.GetTranslation("Game.Server.GameObjects.Prop");
                case eBageType.FightBag:
                    return LanguageMgr.GetTranslation("Game.Server.GameObjects.FightBag");
                case eBageType.FarmBag:
                    return LanguageMgr.GetTranslation("Game.Server.GameObjects.FarmBag");
                case eBageType.BeadBag:
                    return LanguageMgr.GetTranslation("Game.Server.GameObjects.BeadBag");
                default:
                    return bageType.ToString();
            }
        }

        public SqlDataProvider.Data.ItemInfo GetItemAt(eBageType bagType, int place)
        {
            return this.GetInventory(bagType)?.GetItemAt(place);
        }

        public SqlDataProvider.Data.ItemInfo GetItemByTemplateID(int templateID)
        {
            return (this.GetInventory(eBageType.EquipBag).GetItemByTemplateID(31, templateID) ?? this.GetInventory(eBageType.PropBag).GetItemByTemplateID(0, templateID)) ?? this.GetInventory(eBageType.Consortia).GetItemByTemplateID(0, templateID);
        }

        public int GetItemCount(int templateId)
        {
            return this.m_propBag.GetItemCount(templateId) + this.m_equipBag.GetItemCount(templateId) + this.m_ConsortiaBag.GetItemCount(templateId);
        }

        public PlayerInventory GetItemInventory(ItemTemplateInfo template)
        {
            return this.GetInventory(template.BagType);
        }

        public void HideEquip(int categoryID, bool hide)
        {
            if (categoryID < 0 || categoryID >= 10)
                return;
            this.EquipShowImp(categoryID, hide ? 2 : 1);
        }

        public char[] InitPvePermission()
        {
            char[] chArray = new char[50];
            for (int index = 0; index < chArray.Length; ++index)
                chArray[index] = '1';
            return chArray;
        }

        public bool IsBlackFriend(int playerID)
        {
            if (this._friends == null)
                return true;
            if (this._friends.ContainsKey(playerID))
                return this._friends[playerID] == 1;
            return false;
        }

        public bool IsConsortia()
        {
            return ConsortiaMgr.FindConsortiaInfo(this.PlayerCharacter.ConsortiaID) != null;
        }

        public bool IsLimitCount(int count)
        {
            if (!GameProperties.IsLimitCount || count <= GameProperties.LimitCount)
                return false;
            this.SendMessage(string.Format("O limite de {0} foi alcançado.", (object)GameProperties.LimitCount));
            return true;
        }

        public bool IsLimitMoney(int count)
        {
            if (!GameProperties.IsLimitMoney || count <= GameProperties.LimitMoney)
                return false;
            this.SendMessage(string.Format("O limite de {0} foi alcançado de cupons.", (object)GameProperties.LimitMoney));
            return true;
        }

        public bool IsPveEpicPermission(int copyId)
        {
            string str1 = "1-2-3-4-5-6-7-8-9-10-11-12-13";
            if (str1.Length > 0)
            {
                string str2 = str1;
                char[] chArray = new char[1] { '-' };
                foreach (string str3 in str2.Split(chArray))
                {
                    if (str3 == copyId.ToString())
                        return true;
                }
            }
            return false;
        }

        public bool UsePayBuff(BuffType type)
        {
            bool flag = false;
            AbstractBuffer ofType = this.BufferList.GetOfType(type);
            if (ofType != null && ofType.Check())
            {
                ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(ofType.Info.TemplateID);
                if (itemTemplate != null)
                {
                    if (itemTemplate.Property3 > 0 && ofType.Info.ValidCount > 0)
                    {
                        --ofType.Info.ValidCount;
                        this.BufferList.UpdateBuffer(ofType);
                        flag = true;
                    }
                    else if (itemTemplate.Property3 == 0)
                        flag = true;
                }
            }
            return flag;
        }

        public bool IsPvePermission(int copyId, eHardLevel hardLevel)
        {
            if (copyId > this.m_pvepermissions.Length || copyId <= 0)
                return true;
            if (hardLevel == eHardLevel.Epic)
                return this.IsPveEpicPermission(copyId);
            return (int)this.m_pvepermissions[copyId - 1] >= (int)GamePlayer.permissionChars[(int)hardLevel];
        }

        public event GamePlayer.PlayerPropertisChange PropertiesChange;

        public void OnPropertiesChange()
        {
            if (this.PropertiesChange == null)
                return;
            this.PropertiesChange(this.PlayerCharacter);
        }

        public void LastVIPPackTime()
        {
            this.m_character.LastVIPPackTime = DateTime.Now;
            this.m_character.CanTakeVipReward = false;
        }

        public bool LoadFromDatabase()
        {
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                PlayerInfo userSingleByUserId = playerBussiness.GetUserSingleByUserID(this.m_character.ID);
                if (userSingleByUserId == null)
                {
                    this.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.Forbid"));
                    this.Client.Disconnect();
                    return false;
                }
                this.m_character = userSingleByUserId;
                this.m_battle.LoadFromDatabase();
                this.m_battle.UpdateLeagueGrade();
                this.m_character.Texp = playerBussiness.GetUserTexpInfoSingle(this.m_character.ID);
                if (this.m_character.Texp.IsValidadteTexp())
                    this.m_character.Texp.texpCount = 0;
                if (userSingleByUserId.Grade >= 20)
                    this.LoadGemStone(playerBussiness);
                this.Out.SendUpdateInventorySlot(this.FightBag, new int[3]
                {
          0,
          1,
          2
                });
                this.UpdateWeaklessGuildProgress();
                this.UpdateItemForUser((object)1);
                this.ClearConsortia(false);
                this.ChecVipkExpireDay();
                this.UpdateLevel();
                this.UpdatePet(this.m_petBag.GetPetIsEquip());
                if (this.m_character.CheckNewDay())
                {
                    this.QuestInventory.Restart();
                    this.QuestInventory.LoadFromDatabase(this.PlayerCharacter.ID);
                    this.OnPlayerLogin();
                    this.m_character.NewDay = DateTime.Now;
                    if (this.m_character.typeVIP == 1)
                    { 
                        this.AddExpVip(10);
                    }
                    else if(this.m_character.typeVIP >= 2)
                    {
                        this.AddExpVip(15);
                    }
                    this.m_battle.Reset();
                    this.m_character.MaxBuyHonor = 0;
                    this.m_extra.Info.MinHotSpring = 60;
                    this.Extra.ResetNoviceEvent(NoviceActiveType.RECHANGE_MONEY_ACTIVE);
                    this.Extra.ResetNoviceEvent(NoviceActiveType.USE_MONEY_ACTIVE);
                }
                if (this.m_character.totemId > TotemMgr.MaxTotem())
                    this.m_character.totemId = TotemMgr.MaxTotem();
                this.m_pvepermissions = string.IsNullOrEmpty(this.m_character.PvePermission) ? this.InitPvePermission() : this.m_character.PvePermission.ToCharArray();
                this.m_fightlabpermissions = string.IsNullOrEmpty(this.m_character.FightLabPermission) ? this.InitFightLabPermission() : this.m_character.FightLabPermission.ToCharArray();
                this.LoadPvePermission();
                this._friends = new Dictionary<int, int>();
                this._friends = playerBussiness.GetFriendsIDAll(this.m_character.ID);
                this._viFarms = new List<int>();
                this.m_character.State = 1;
                this.ClearStoreBag();
                this.ClearCaddyBag();
                this.PlayerCharacter.VIPNextLevelDaysNeeded = this.GetVIPNextLevelDaysNeeded(this.PlayerCharacter.VIPLevel, this.PlayerCharacter.VIPExp);
                this.LoadMedals();
                playerBussiness.UpdateUserTexpInfo(this.m_character.Texp);
                playerBussiness.UpdatePlayer(this.m_character);
                playerBussiness.UpdateUserMatchInfo(this.MatchInfo);
                this.SavePlayerInfo();
                return true;
            }
        }

        public int[] FightLabPermissionInt()
        {
            int[] numArray = new int[50];
            for (int index = 0; index < 50; ++index)
                numArray[index] = (int)this.m_fightlabpermissions[index];
            return numArray;
        }

        public char[] InitFightLabPermission()
        {
            char[] chArray = new char[50];
            for (int index = 0; index < 50; ++index)
                chArray[index] = index != 0 ? '0' : '1';
            return chArray;
        }

        public bool SetFightLabPermission(int copyId, eHardLevel hardLevel, int missionId)
        {
            switch (copyId)
            {
                case 1000:
                    copyId = 5;
                    break;
                case 1001:
                    copyId = 6;
                    break;
                case 1002:
                    copyId = 7;
                    break;
                case 1003:
                    copyId = 8;
                    break;
                case 1004:
                    copyId = 9;
                    break;
            }
            bool flag1;
            if (copyId > this.m_fightlabpermissions.Length || copyId <= 0)
            {
                return true;
            }

            else
            {
                int index = (copyId - 5) * 2;
                if ((int)this.m_fightlabpermissions[index] != (int)GamePlayer.fightlabpermissionChars[(int)(hardLevel + 1)])
                {
                    flag1 = true;
                }
                else
                {
                    if (this.m_fightlabpermissions[index + 1] <= '2' && (int)this.m_fightlabpermissions[index] - (int)this.m_fightlabpermissions[index + 1] == 1)
                    {
                        this.m_fightlabpermissions[index + 1] = this.m_fightlabpermissions[index];
                        int gold = 0;
                        int money = 0;
                        int giftToken = 0;
                        int gp = 0;
                        List<SqlDataProvider.Data.ItemInfo> info1 = new List<SqlDataProvider.Data.ItemInfo>();
                        if (DropInventory.FightLabUserDrop(missionId, ref info1) && info1 != null)
                        {
                            bool flag2 = false;
                            string message = LanguageMgr.GetTranslation("OpenUpArkHandler.FightLabStart") + ": ";
                            foreach (SqlDataProvider.Data.ItemInfo info2 in info1)
                            {
                                message = message + LanguageMgr.GetTranslation("Game.Server.Quests.FinishQuest.RewardProp", (object)info2.Template.Name, (object)info2.Count) + " ";
                                if (info1.Count > 0 && this.PropBag.GetEmptyCount() < 1)
                                {
                                    if (info2.TemplateID != 11107 && info2.TemplateID != -100 && (info2.TemplateID != -200 && info2.TemplateID != -300))
                                    {
                                        string translation1 = LanguageMgr.GetTranslation("Game.Server.GameUtils.Content2");
                                        string translation2 = LanguageMgr.GetTranslation("Game.Server.GameUtils.Title2");
                                        if (this.SendItemsToMail(new List<SqlDataProvider.Data.ItemInfo>()
                                        {
                                          info2
                                        }, translation1, translation2, eMailType.ItemOverdue))
                                            this.Out.SendMailResponse(this.PlayerCharacter.ID, eMailRespose.Receiver);
                                        flag2 = true;
                                    }
                                }
                                else if (!this.PropBag.StackItemToAnother(info2) && info2.TemplateID != 11107 && (info2.TemplateID != -100 && info2.TemplateID != -200) && info2.TemplateID != -300)
                                    this.PropBag.AddItem(info2);
                                SqlDataProvider.Data.ItemInfo.FindSpecialItemInfo(info2, ref gold, ref money, ref giftToken, ref gp);
                            }
                            this.AddGold(gold);
                            this.AddMoney(money);
                            this.AddGiftToken(giftToken);
                            this.AddGP(gp);
                            if (flag2)
                                message += LanguageMgr.GetTranslation("Game.Server.GameUtils.Title2");
                            this.Out.SendMessage(eMessageType.GM_NOTICE, message);
                        }
                    }
                    if (copyId == 5 && hardLevel == eHardLevel.Normal)
                    {
                        if (this.m_fightlabpermissions[2] == '0')
                            this.m_fightlabpermissions[2] = '1';
                        if (this.m_fightlabpermissions[4] == '0')
                            this.m_fightlabpermissions[4] = '1';
                        if (this.m_fightlabpermissions[6] == '0')
                            this.m_fightlabpermissions[6] = '1';
                    }
                    if ((copyId == 7 || copyId == 8) && (hardLevel == eHardLevel.Hard && this.m_fightlabpermissions[8] == '0'))
                        this.m_fightlabpermissions[8] = '1';
                    if (hardLevel < eHardLevel.Hard && (int)this.m_fightlabpermissions[index] < (int)GamePlayer.fightlabpermissionChars[(int)(hardLevel + 2)])
                        this.m_fightlabpermissions[index] = GamePlayer.fightlabpermissionChars[(int)(hardLevel + 2)];
                    this.m_character.FightLabPermission = new string(this.m_fightlabpermissions).ToString();
                    this.OnPropertiesChanged();
                    flag1 = true;
                }
            }
            return flag1;
        }

        public bool IsFightLabPermission(int copyId, eHardLevel hardLevel)
        {
            return copyId > this.m_fightlabpermissions.Length || copyId <= 0 || (int)this.m_fightlabpermissions[(copyId - 5) * 2] >= (int)GamePlayer.fightlabpermissionChars[(int)(hardLevel + 1)];
        }

        public eHardLevel GetMaxFightLabPermission(int copyId)
        {
            eHardLevel eHardLevel;
            if (copyId > this.m_fightlabpermissions.Length)
            {
                eHardLevel = eHardLevel.Simple;
            }
            else
            {
                switch (this.m_fightlabpermissions[copyId - 5])
                {
                    case '2':
                        eHardLevel = eHardLevel.Normal;
                        break;
                    case '3':
                        eHardLevel = eHardLevel.Hard;
                        break;
                    default:
                        eHardLevel = eHardLevel.Simple;
                        break;
                }
            }
            return eHardLevel;
        }

        public bool GetLeagueOpen(bool isMsg)
        {
            DateTime now = DateTime.Now;
            string[] strArray = GameProperties.TimeForLeague.Split('|');
            if (!(now >= Convert.ToDateTime(strArray[0])) || !(now <= Convert.ToDateTime(strArray[1])))
                return false;
            if (isMsg)
                this.SendMessage("Você jogou 2VS2 e ganhou mais porque é um jogador profissional de muitos jogadores e fãs");
            return true;
        }

        public void LoadMedals()
        {
            this.m_character.medal = this.GetMedalNum();
            this.Client.Player.SaveIntoDatabase();
        }

        public void SendPkgLimitGrate()
        {
            int id = this.PlayerCharacter.ID;
            this.SendUpdatePlayerFigSpirit();
        }

        public void SendUpdatePlayerFigSpirit()
        {
            if (this.PlayerCharacter.Grade < 30)
                return;
            this.Out.SendPlayerFigSpiritinit(this.PlayerCharacter.ID, this.GemStone);
        }

        public void LoadMarryMessage()
        {
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                MarryApplyInfo[] playerMarryApply = playerBussiness.GetPlayerMarryApply(this.PlayerCharacter.ID);
                if (playerMarryApply == null)
                    return;
                foreach (MarryApplyInfo marryApplyInfo in playerMarryApply)
                {
                    switch (marryApplyInfo.ApplyType)
                    {
                        case 1:
                            this.Out.SendPlayerMarryApply(this, marryApplyInfo.ApplyUserID, marryApplyInfo.ApplyUserName, marryApplyInfo.LoveProclamation, marryApplyInfo.ID);
                            break;
                        case 2:
                            this.Out.SendMarryApplyReply(this, marryApplyInfo.ApplyUserID, marryApplyInfo.ApplyUserName, marryApplyInfo.ApplyResult, true, marryApplyInfo.ID);
                            if (!marryApplyInfo.ApplyResult)
                            {
                                this.Out.SendMailResponse(this.PlayerCharacter.ID, eMailRespose.Receiver);
                                break;
                            }
                            break;
                        case 3:
                            this.Out.SendPlayerDivorceApply(this, true, false);
                            break;
                    }
                }
            }
        }

        public void LoadMarryProp()
        {
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                MarryProp marryProp = playerBussiness.GetMarryProp(this.PlayerCharacter.ID);
                this.PlayerCharacter.IsMarried = marryProp.IsMarried;
                this.PlayerCharacter.SpouseID = marryProp.SpouseID;
                this.PlayerCharacter.SpouseName = marryProp.SpouseName;
                this.PlayerCharacter.IsCreatedMarryRoom = marryProp.IsCreatedMarryRoom;
                this.PlayerCharacter.SelfMarryRoomID = marryProp.SelfMarryRoomID;
                this.PlayerCharacter.IsGotRing = marryProp.IsGotRing;
                this.Out.SendMarryProp(this, marryProp);
            }
        }

        public void LoadPvePermission()
        {
            foreach (PveInfo pveInfo in PveInfoMgr.GetPveInfo())
            {
                if (this.m_character.Grade > pveInfo.LevelLimits)
                {
                    bool flag = this.SetPvePermission(pveInfo.ID, eHardLevel.Easy);
                    if (flag)
                        flag = this.SetPvePermission(pveInfo.ID, eHardLevel.Normal);
                    if (flag)
                        this.SetPvePermission(pveInfo.ID, eHardLevel.Hard);
                }
            }
        }

        public void LogAddMoney(
          AddMoneyType masterType,
          AddMoneyType sonType,
          int userId,
          int moneys,
          int SpareMoney)
        {
        }

        public bool Login()
        {
            if (!WorldMgr.AddPlayer(this.m_character.ID, this))
                return false;
            try
            {
                if (LoadFromDatabase())
                {
                    DateTime dateTime = this.PlayerCharacter.BoxGetDate;
                    string shortDateString1 = dateTime.ToShortDateString();
                    dateTime = DateTime.Now;
                    string shortDateString2 = dateTime.ToShortDateString();
                    if (shortDateString1 != shortDateString2)
                    {
                        this.PlayerCharacter.AlreadyGetBox = 0;
                        this.PlayerCharacter.BoxProgression = 0;
                    }
                    method_5();
                    ePlayerState_0 = ePlayerState.Online;
                    BossBoxStartTime = DateTime.Now;
                    this.Out.SendLoginSuccess();
                    this.Out.SendUpdatePublicPlayer(this.PlayerCharacter, this.MatchInfo, this.Extra.Info);
                    this.Out.SendWeaklessGuildProgress(this.PlayerCharacter);
                    this.ProcessConsortiaAndPet();
                    this.Out.SendDateTime();
                    Out.SendUserLuckyNum();
                    Out.SendCheckCode();
                    this.Out.SendDailyAward(this);
                    this.LoadMarryMessage();
                    if (!this.m_showPP)
                    {
                        this.m_playerProp.ViewCurrent();
                        this.m_showPP = true;
                    }
                    int num = 0;
                    if (this.Extra.CheckNoviceActiveOpen(NoviceActiveType.RECHANGE_MONEY_ACTIVE))
                        this.Extra.UpdateEventCondition(4, num, true, 0);
                    int id = this.PlayerCharacter.ID;
                    this.Out.SendUserRanks(this.Rank.GetRank());
                    this.Out.SendOpenVIP(this);
                    this.EquipBag.UpdatePlayerProperties();
                    this.Out.SendEnthrallLight();
                    this.Out.SendEdictumVersion();
                    this.m_playerState = ePlayerState.Manual;
                    this.Out.SendBufferList(this, this.m_bufferList.GetAllBufferByTemplate());
                    this.Out.SendUpdateAchievementData(this.AchievementInventory.GetSuccessAchievement());
                    this.BoxBeginTime = DateTime.Now;
                    this.OpenAllNoviceActive();
                    this.PetBag.UpdateEatPets();//#6 For EatPets
                    this.SendUpdatePlayerFigSpirit();
                    ConsortiaTaskMgr.AddPlayer(this);
                    this.Out.SendUpdateFirstRecharge(this.PlayerCharacter.IsRecharged, this.PlayerCharacter.IsGetAward);
                    this.ChargeToUser();
                    GameServer.Instance.LoginServer.SendGetLightriddleInfo(m_character.ID);
                    //this.Out.SendLittleGameActived(); Infuncional 
                    
                    return true;
                }
                WorldMgr.RemovePlayer(this.m_character.ID);
            }
            catch (Exception ex)
            {
                GamePlayer.log.Error((object)"Error Login!", ex);
            }
            return false;
        }

        private void ProcessConsortiaAndPet()
        {
            this.consortiaProcessor_0 = new ConsortiaProcessor((GInterface3)this.m_consortiaProcessor);
            this.petProcessor_0 = new PetProcessor((IPetProcessor)this.m_petProcessor);
        }

        public PetProcessor PetHandler
        {
            get
            {
                return this.petProcessor_0;
            }
        }

        public bool MoneyDirect(int value, bool IsAntiMult)
        {
            return this.MoneyDirect(MoneyType.Money, value, IsAntiMult);
        }

        public bool MoneyDirect(MoneyType type, int value, bool IsAntiMult)
        {
            if (value >= 0 && value <= int.MaxValue)
            {
                if (type == MoneyType.Money)
                {
                    if (this.PlayerCharacter.Money + this.PlayerCharacter.MoneyLock >= value)
                    {
                        this.RemoveMoney(value, IsAntiMult);
                        this.UpdateProperties();
                        return true;
                    }
                    this.SendInsufficientMoney(0);
                }
                else
                {
                    if (this.PlayerCharacter.GiftToken >= value)
                    {
                        this.RemoveGiftToken(value);
                        this.UpdateProperties();
                        return true;
                    }
                    this.SendMessage("Moedas grátis insuficientes.");
                }
            }
            return false;
        }

        public void OnAchievementFinish(AchievementData info)
        {
            if (this.AchievementFinishEvent == null)
                return;
            this.AchievementFinishEvent(info);
        }

        public void OnAdoptPetEvent()
        {
            if (this.AdoptPetEvent == null)
                return;
            this.AdoptPetEvent();
        }

        public void OnCropPrimaryEvent()
        {
            if (this.CropPrimaryEvent == null)
                return;
            this.CropPrimaryEvent();
        }

        public void OnEnterHotSpring()
        {
            if (this.EnterHotSpringEvent == null)
                return;
            this.EnterHotSpringEvent(this);
        }

        public void OnFightAddOffer(int offer)
        {
            if (this.FightAddOfferEvent == null)
                return;
            this.FightAddOfferEvent(offer);
        }

        public void OnGuildChanged()
        {
            if (this.GuildChanged == null)
                return;
            this.GuildChanged();
        }

        public void OnHotSpingExpAdd(int minutes, int exp)
        {
            if (this.HotSpingExpAdd == null)
                return;
            this.HotSpingExpAdd(minutes, exp);
        }

        public void OnItemCompose(int composeType)
        {
            if (this.ItemCompose == null)
                return;
            this.ItemCompose(composeType);
        }

        public void OnItemFusion(int fusionType)
        {
            if (this.ItemFusion == null)
                return;
            this.ItemFusion(fusionType);
        }

        public void OnItemInsert()
        {
            if (this.ItemInsert == null)
                return;
            this.ItemInsert();
        }

        public void OnItemMelt(int categoryID)
        {
            if (this.ItemMelt == null)
                return;
            this.ItemMelt(categoryID);
        }

        public event GamePlayer.SendGiftMail GiftMail;

        public void OnSendGiftmail(int templateID, int count)
        {
            if (this.GiftMail == null)
                return;
            this.GiftMail(templateID, count);
        }

        public void OnItemStrengthen(int categoryID, int level)
        {
            if (this.ItemStrengthen == null)
                return;
            this.ItemStrengthen(categoryID, level);
        }

        public void OnMoneyCharge(int money)
        {
            if (this.MoneyCharge == null)
                return;
            this.MoneyCharge(money);
        }

        public void OnAchievementQuest()
        {
            if (this.AchievementQuest == null)
                return;
            this.AchievementQuest();
        }

        public void OnKillingBoss(AbstractGame game, NpcInfo npc, int damage)
        {
            if (this.AfterKillingBoss == null)
                return;
            this.AfterKillingBoss(game, npc, damage);
        }

        public void OnKillingLiving(AbstractGame game, int type, int id, bool isLiving, int damage)
        {
            if (this.AfterKillingLiving != null)
            {
                this.AfterKillingLiving(game, type, id, isLiving, damage,true);
            }
            if ((this.GameKillDrop != null) && !isLiving)
            {
                this.GameKillDrop(game, type, id, isLiving);
            }
        }

        public void OnLevelUp(int grade)
        {
            if (this.LevelUp == null)
                return;
            this.LevelUp(this);
        }

        public event GamePlayer.PlayerMissionFullOverEventHandle MissionFullOver;

        public void OnMissionOver(AbstractGame game, bool isWin, int missionId, int turnNum)
        {
            if (this.MissionOver != null)
                this.MissionOver(game, missionId, isWin);
            if (this.MissionTurnOver != null & isWin)
                this.MissionTurnOver(game, missionId, turnNum);
            if (this.MissionFullOver == null)
                return;
            this.MissionFullOver(game, missionId, isWin, turnNum);
        }

        

        public void OnNewGearEvent(ItemInfo CategoryID)
        {
            if (NewGearEvent == null)
                return;
          NewGearEvent(CategoryID);
        }

        public void OnPaid(
          int money,
          int gold,
          int offer,
          int gifttoken,
          int medal,
          string payGoods)
        {
            if (this.Paid == null)
                return;
            this.Paid(money, gold, offer, gifttoken, medal, payGoods);
        }

        protected void OnPropertiesChanged()
        {
            this.UpdateProperties();
            this.OnPlayerPropertyChanged(this.m_character);
        }

        public void OnUnknowQuestConditionEvent()
        {
            if (this.UnknowQuestConditionEvent == null)
                return;
            this.UnknowQuestConditionEvent();
        }

        public void OnUpLevelPetEvent()
        {
            if (this.UpLevelPetEvent == null)
                return;
            this.UpLevelPetEvent();
        }

        public void OnUseBuffer()
        {
            if (this.UseBuffer == null)
                return;
            this.UseBuffer(this);
        }

        public void OnUserToemGemstoneEvent()
        {
            if (this.UserToemGemstonetEvent == null)
                return;
            this.UserToemGemstonetEvent();
        }

        public void OnUsingItem(int templateID, int count)
        {
            if (this.AfterUsingItem == null)
                return;
            this.AfterUsingItem(templateID, count);
        }

        public void OnEnterSpa()
        {
            if (this.EnterSPA == null)
                return;
            this.EnterSPA();
        }

        public void OpenVIP(int days, DateTime ExpireDayOut)
        {
            int Level = m_character.VIPLevel;
            if(Level != 0)
            { 
                this.m_character.VIPExpireDay = ExpireDayOut;
                this.m_character.typeVIP = SetTypeVIP(days);
                this.m_character.VIPLastDate = DateTime.Now;

            }
            else
            {
                this.m_character.VIPExpireDay = ExpireDayOut;
                this.m_character.typeVIP = SetTypeVIP(days);
                this.m_character.VIPLevel = 1;
                this.m_character.VIPExp = 0;
                this.m_character.VIPLastDate = DateTime.Now;
                this.m_character.VIPNextLevelDaysNeeded = 9;
                this.m_character.CanTakeVipReward = true;
            }
        }

        public void OpenVIP2(int days, DateTime ExpireDayOut)
        {
            if (this.m_character.typeVIP == 0)
            {
                this.Extra.CreateSaveLifeBuff();
            }

            this.m_character.typeVIP = 1;
            this.m_character.VIPLevel = 1;
            this.m_character.VIPExp = 0;
            this.m_character.VIPExpireDay = ExpireDayOut;
            this.m_character.VIPLastDate = DateTime.Now;
            this.m_character.VIPNextLevelDaysNeeded = 9;
            this.m_character.CanTakeVipReward = true;

        }

        public byte SetTypeVIP(int days)
        {
            DateTime Fim = this.PlayerCharacter.VIPExpireDay;
            double a = Fim.Subtract(DateTime.Now).TotalDays;
            if ((a / 31)>=3)
            {
                return (byte)2;
            }
            else
            { 
            byte num = 1;
            if (days / 31 >= 3)
                num = (byte)2;
            return num;
            }

        }

        public void ResetLottery()
        {
            this.Lottery = -1;
            this.LotteryID = 0;
            this.LotteryItems = new List<ItemBoxInfo>();
            this.LotteryAwardList = new List<SqlDataProvider.Data.ItemInfo>();
        }

        public virtual bool Quit()
        {
            try
            {
                try
                {
                    if (this.Level == 1)
                    {
                        SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(7008), 1, 105);
                        fromTemplate.ValidDate = 365;
                        this.EquipBag.AddItemTo(fromTemplate, 6);
                    }
                    if (this.CurrentRoom != null)
                    {
                        this.CurrentRoom.RemovePlayerUnsafe(this);
                        this.CurrentRoom = (BaseRoom)null;
                    }
                    else
                        RoomMgr.WaitingRoom.RemovePlayer(this);
                    if (this.CurrentMarryRoom != null)
                    {
                        this.CurrentMarryRoom.RemovePlayer(this);
                        this.CurrentMarryRoom = (MarryRoom)null;
                    }
                    if (this.LotteryAwardList.Count > 0)
                    {
                        if (this.Lottery != -1)
                        {
                            this.SendItemsToMail(this.LotteryAwardList, "", LanguageMgr.GetTranslation("Game.Server.Lottery.Oversea.MailTitle"), eMailType.BuyItem);
                            this.ResetLottery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    GamePlayer.log.Error((object)"Player exit Game Error!", ex);
                }
                this.m_character.State = 0;
                this.SaveIntoDatabase();
            }
            catch (Exception ex)
            {
                GamePlayer.log.Error((object)"Player exit Error!!!", ex);
            }
            finally
            {
                ConsortiaTaskMgr.RemovePlayer(this);
                WorldMgr.RemovePlayer(this.m_character.ID);
            }
            return true;
        }

        public bool RemoveAt(eBageType bagType, int place)
        {
            PlayerInventory inventory = this.GetInventory(bagType);
            if (inventory != null)
                return inventory.RemoveItemAt(place);
            return false;
        }

        public bool RemoveCountFromStack(SqlDataProvider.Data.ItemInfo item, int count)
        {
            if (item.BagType == this.m_propBag.BagType)
                return this.m_propBag.RemoveCountFromStack(item, count);
            if (item.BagType == this.m_ConsortiaBag.BagType)
                return this.m_ConsortiaBag.RemoveCountFromStack(item, count);
            return this.m_equipBag.RemoveCountFromStack(item, count);
        }

        public int RemoveGold(int value)
        {
            if (value <= 0 || value > this.m_character.Gold)
                return 0;
            this.m_character.Gold -= value;
            this.OnPropertiesChanged();
            this.UpdateProperties();
            return value;
        }

        public int RemoveGP(int gp)
        {
            if (gp <= 0)
                return 0;
            this.m_character.GP -= gp;
            if (this.m_character.GP < 1)
                this.m_character.GP = 1;
            if (this.Level > LevelMgr.GetLevel(this.m_character.GP))
                this.m_character.GP += gp;
            this.UpdateProperties();
            this.UpdateLevel();
            return gp;
        }

        public int RemoveGiftToken(int value)
        {
            if (value <= 0 || value > this.m_character.GiftToken)
                return 0;
            this.m_character.GiftToken -= value;
            this.OnPropertiesChanged();
            this.UpdateProperties();
            return value;
        }

        public bool RemoveHealstone()
        {
            SqlDataProvider.Data.ItemInfo itemAt = this.m_equipBag.GetItemAt(18);
            return itemAt != null && itemAt.Count > 0;
        }

        public bool RemoveItem(SqlDataProvider.Data.ItemInfo item)
        {
            if (item.BagType == this.m_farmBag.BagType)
                return this.m_farmBag.RemoveItem(item);
            if (item.BagType == this.m_propBag.BagType)
                return this.m_propBag.RemoveItem(item);
            if (item.BagType == this.m_fightBag.BagType)
                return this.m_fightBag.RemoveItem(item);
            return this.m_equipBag.RemoveItem(item);
        }

        public int AddMedal(int value)
        {
            if (value <= 0)
                return 0;
            SqlDataProvider.Data.ItemInfo itemByTemplateId = this.GetInventory(eBageType.PropBag).GetItemByTemplateID(1, 11408);
            if (itemByTemplateId != null)
            {
                this.PropBag.AddCountToStack(itemByTemplateId, value);
                this.PropBag.UpdateItem(itemByTemplateId);
            }
            else
                this.PropBag.AddTemplate(SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11408), value, 104), value);
            this.m_character.medal = this.GetMedalNum();
            this.OnPropertiesChanged();
            this.UpdateProperties();
            this.UpdateChangedPlaces();
            return value;
        }

        public int RemoveMedal(int value)
        {
            if (value <= 0 || value > this.m_character.medal && value > this.GetItemCount(11408))
                return 0;
            this.RemoveTemplate(11408, value);
            this.m_character.medal = this.GetMedalNum();
            this.OnPropertiesChanged();
            this.UpdateProperties();
            this.UpdateChangedPlaces();
            return value;
        }

        public int RemoveMoney(int value)
        {
            if (value > 0 && value <= this.m_character.MoneyLock)
            {
                this.m_character.MoneyLock -= value;
                if (this.Extra.CheckNoviceActiveOpen(NoviceActiveType.USE_MONEY_ACTIVE))
                    this.Extra.UpdateEventCondition(3, value, true, 0);
                this.OnPropertiesChanged();
                this.UpdateProperties();
                return value;
            }
            if (value <= 0 || value > this.m_character.Money)
                return 0;
            this.m_character.Money -= value;
            if (this.Extra.CheckNoviceActiveOpen(NoviceActiveType.USE_MONEY_ACTIVE))
                this.Extra.UpdateEventCondition(3, value, true, 0);
            this.OnPropertiesChanged();
            this.UpdateProperties();
            return value;
        }

        public int RemoveMoney(int value, bool IsAntiMult)
        {
            if (value > 0 && value <= this.m_character.MoneyLock && !IsAntiMult)
            {
                this.m_character.MoneyLock -= value;
                if (this.Extra.CheckNoviceActiveOpen(NoviceActiveType.USE_MONEY_ACTIVE) && !IsAntiMult)
                    this.Extra.UpdateEventCondition(3, value, true, 0);
                this.OnPropertiesChanged();
                this.UpdateProperties();
                return value;
            }
            if (value <= 0 || value > this.m_character.Money)
                return 0;
            this.m_character.Money -= value;
            if (this.Extra.CheckNoviceActiveOpen(NoviceActiveType.USE_MONEY_ACTIVE) && !IsAntiMult)
                this.Extra.UpdateEventCondition(3, value, true, 0);
            this.OnPropertiesChanged();
            this.UpdateProperties();
            return value;
        }

        public int RemoveOffer(int value)
        {
            if (value <= 0)
                return 0;
            if (value >= this.m_character.Offer)
                value = this.m_character.Offer;
            this.m_character.Offer -= value;
            this.OnPropertiesChanged();
            this.UpdateProperties();
            return value;
        }

        public int RemoveHonor(int value)
        {
            if (value <= 0)
                return 0;
            this.m_character.myHonor -= value;
            this.OnPropertiesChanged();
            return value;
        }

        public int RemovemyHonor(int value)
        {
            if (value <= 0 || value > this.m_character.myHonor)
                return 0;
            this.m_character.myHonor -= value;
            this.OnPropertiesChanged();
            return value;
        }

        public int RemoveRichesOffer(int value)
        {
            if (value <= 0)
                return 0;
            if (value >= this.m_character.RichesOffer)
                value = this.m_character.RichesOffer;
            this.m_character.RichesOffer -= value;
            this.OnPropertiesChanged();
            this.UpdateProperties();
            return value;
        }

        public int RemoveConsortiaRiches(int value)
        {
            if (value <= 0)
                return 0;
            if (value >= this.m_character.ConsortiaRiches)
                value = this.m_character.ConsortiaRiches;
            this.m_character.ConsortiaRiches -= value;
            this.OnPropertiesChanged();
            this.UpdateProperties();
            this.OnGuildChanged();
            return value;
        }

        public int RemovePetScore(int value)
        {
            if (value <= 0 || value > this.m_character.petScore)
                return 0;
            this.m_character.petScore -= value;
            this.OnPropertiesChanged();
            this.UpdateProperties();
            return value;
        }

        public int RemoveScore(int value)
        {
            if (value <= 0 || value > this.m_character.Score)
                return 0;
            this.m_character.Score -= value;
            this.OnPropertiesChanged();
            this.UpdateProperties();
            return value;
        }

        public bool RemoveTempate(eBageType bagType, ItemTemplateInfo template, int count)
        {
            PlayerInventory inventory = this.GetInventory(bagType);
            if (inventory != null)
                return inventory.RemoveTemplate(template.TemplateID, count);
            return false;
        }

        public bool RemoveTemplate(ItemTemplateInfo template, int count)
        {
            PlayerInventory itemInventory = this.GetItemInventory(template);
            if (itemInventory != null)
                return itemInventory.RemoveTemplate(template.TemplateID, count);
            return false;
        }

        public bool RemoveTemplate(int templateId, int count)
        {
            int itemCount1 = this.m_equipBag.GetItemCount(templateId);
            int itemCount2 = this.m_propBag.GetItemCount(templateId);
            int itemCount3 = this.m_ConsortiaBag.GetItemCount(templateId);
            int num = itemCount1 + itemCount2 + itemCount3;
            ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(templateId);
            if (templateId == 11408 && count <= itemCount2 + itemCount3)
            {
                this.m_character.medal -= count;
                this.UpdateProperties();
            }
            if (itemTemplate != null && num >= count)
            {
                if (itemCount1 > 0 && count > 0 && this.RemoveTempate(eBageType.EquipBag, itemTemplate, itemCount1 > count ? count : itemCount1))
                    count = count < itemCount1 ? 0 : count - itemCount1;
                if (itemCount2 > 0 && count > 0 && this.RemoveTempate(eBageType.PropBag, itemTemplate, itemCount2 > count ? count : itemCount2))
                    count = count < itemCount2 ? 0 : count - itemCount2;
                if (itemCount3 > 0 && count > 0 && this.RemoveTempate(eBageType.Consortia, itemTemplate, itemCount3 > count ? count : itemCount3))
                    count = count < itemCount3 ? 0 : count - itemCount3;
                if (count == 0)
                    return true;
                if (GamePlayer.log.IsErrorEnabled)
                    GamePlayer.log.Error((object)string.Format("Item Remover Error：PlayerId {0} Remover TemplateId{1} Is Not Zero!", (object)this.m_playerId, (object)templateId));
            }
            return false;
        }

        public UserLabyrinthInfo LoadLabyrinth(int sType)
        {
            if (this.userLabyrinthInfo == null)
            {
                using (PlayerBussiness playerBussiness = new PlayerBussiness())
                {
                    this.userLabyrinthInfo = playerBussiness.GetSingleLabyrinth(this.PlayerCharacter.ID);
                    if (this.userLabyrinthInfo == null)
                    {
                        this.userLabyrinthInfo = new UserLabyrinthInfo();
                        this.userLabyrinthInfo.UserID = this.PlayerCharacter.ID;
                        this.userLabyrinthInfo.sType = sType;
                        this.userLabyrinthInfo.myProgress = 0;
                        this.userLabyrinthInfo.myRanking = 0;
                        this.userLabyrinthInfo.completeChallenge = true;
                        this.userLabyrinthInfo.isDoubleAward = false;
                        this.userLabyrinthInfo.currentFloor = 1;
                        this.userLabyrinthInfo.accumulateExp = 0;
                        this.userLabyrinthInfo.remainTime = 0;
                        this.userLabyrinthInfo.currentRemainTime = 0;
                        this.userLabyrinthInfo.cleanOutAllTime = 0;
                        this.userLabyrinthInfo.cleanOutGold = 50;
                        this.userLabyrinthInfo.tryAgainComplete = true;
                        this.userLabyrinthInfo.isInGame = false;
                        this.userLabyrinthInfo.isCleanOut = false;
                        this.userLabyrinthInfo.serverMultiplyingPower = false;
                        this.userLabyrinthInfo.LastDate = DateTime.Now;
                        this.userLabyrinthInfo.ProcessAward = this.InitProcessAward();
                        playerBussiness.AddUserLabyrinth(this.userLabyrinthInfo);
                    }
                    else
                    {
                        this.ProcessLabyrinthAward = this.userLabyrinthInfo.ProcessAward;
                        this.userLabyrinthInfo.sType = sType;
                    }
                }
            }
            return this.Labyrinth;
        }

        public string InitProcessAward()
        {
            string[] strArray = new string[99];
            for (int index = 0; index < strArray.Length; ++index)
                strArray[index] = index.ToString();
            this.ProcessLabyrinthAward = string.Join("-", strArray);
            return this.ProcessLabyrinthAward;
        }

        public string CompleteGetAward(int floor)
        {
            string[] strArray1 = new string[floor];
            for (int index = 0; index < floor; ++index)
                strArray1[index] = "i";
            string[] strArray2 = this.userLabyrinthInfo.ProcessAward.Split('-');
            string str = string.Join("-", strArray1);
            for (int index = floor; index < strArray2.Length; ++index)
                str = str + "-" + strArray2[index];
            return str;
        }

        public bool isDoubleAward()
        {
            if (this.userLabyrinthInfo == null)
                return false;
            return this.userLabyrinthInfo.isDoubleAward;
        }

        public void OutLabyrinth(bool isWin)
        {
            if (!isWin && this.userLabyrinthInfo != null && this.userLabyrinthInfo.currentFloor > 1)
                this.SendLabyrinthTryAgain();
            this.ResetLabyrinth();
        }

        public void SendLabyrinthTryAgain()
        {
            GSPacketIn pkg = new GSPacketIn((short)131, this.PlayerId);
            pkg.WriteByte((byte)9);
            pkg.WriteInt(this.LabyrinthTryAgainMoney());
            this.SendTCP(pkg);
        }

        public int LabyrinthTryAgainMoney()
        {
            for (int index = 0; index < this.Labyrinth.myProgress; index += 2)
            {
                if (this.Labyrinth.currentFloor == index)
                    return GameProperties.WarriorFamRaidPriceBig;
            }
            return GameProperties.WarriorFamRaidPriceSmall;
        }

        public void ResetLabyrinth()
        {
            if (this.userLabyrinthInfo == null)
                return;
            this.userLabyrinthInfo.isInGame = false;
            this.userLabyrinthInfo.completeChallenge = false;
            this.userLabyrinthInfo.ProcessAward = this.InitProcessAward();
        }

        public void CalculatorClearnOutLabyrinth()
        {
            if (this.userLabyrinthInfo == null)
                return;
            int num1 = 0;
            for (int currentFloor = this.userLabyrinthInfo.currentFloor; currentFloor <= this.userLabyrinthInfo.myProgress; ++currentFloor)
                num1 += 2;
            int num2 = num1 * 60;
            this.userLabyrinthInfo.remainTime = num2;
            this.userLabyrinthInfo.currentRemainTime = num2;
            this.userLabyrinthInfo.cleanOutAllTime = num2;
        }

        public int[] CreateExps()
        {
            int[] numArray = new int[99];
            int num = 660;
            for (int index = 0; index < numArray.Length; ++index)
            {
                numArray[index] = num;
                num += 690;
            }
            return numArray;
        }

        public PlayerActives Actives
        {
            get
            {
                return this.m_playerActive;
            }
        }

        public void UpdateLabyrinth(int floor, int m_missionInfoId, bool bigAward)
        {
            int[] exps = this.CreateExps();
            int num1 = floor - 1 > exps.Length ? exps.Length - 1 : floor - 1;
            int index = num1 < 0 ? 0 : num1;
            int gp = exps[index];
            string labyrinthGold = this.labyrinthGolds[index];
            int count = int.Parse(labyrinthGold.Split('|')[0]);
            int num2 = int.Parse(labyrinthGold.Split('|')[1]);
            if (this.userLabyrinthInfo != null)
            {
                ++floor;
                this.ProcessLabyrinthAward = this.CompleteGetAward(floor);
                this.userLabyrinthInfo.ProcessAward = this.ProcessLabyrinthAward;
                if (this.PropBag.GetItemByTemplateID(0, 11916) == null || !this.RemoveTemplate(11916, 1))
                    this.userLabyrinthInfo.isDoubleAward = false;
                if (this.userLabyrinthInfo.isDoubleAward)
                {
                    gp *= 2;
                    count *= 2;
                    num2 *= 2;
                }
                if (floor > this.userLabyrinthInfo.myProgress)
                    this.userLabyrinthInfo.myProgress = floor;
                if (floor > this.userLabyrinthInfo.currentFloor)
                    this.userLabyrinthInfo.currentFloor = floor;
                this.userLabyrinthInfo.accumulateExp += gp;
                string msg = LanguageMgr.GetTranslation("UpdateLabyrinth.Exp", (object)gp);
                this.AddGP(gp);
                if (bigAward)
                {
                    List<SqlDataProvider.Data.ItemInfo> itemInfoList = this.CopyDrop(2, 40002);
                    if (itemInfoList != null)
                    {
                        foreach (SqlDataProvider.Data.ItemInfo cloneItem in itemInfoList)
                        {
                            cloneItem.IsBinds = true;
                            this.AddTemplate(cloneItem, cloneItem.Template.BagType, count, true);
                            msg += string.Format(", {0} x{1}", (object)cloneItem.Template.Name, (object)count);
                        }
                    }
                    this.AddHardCurrency(num2);
                    msg = msg + LanguageMgr.GetTranslation("UpdateLabyrinth.GoldLaby") + (object)num2;
                }
                this.SendHideMessage(msg);
            }
            this.Out.SendLabyrinthUpdataInfo(this.userLabyrinthInfo.UserID, this.userLabyrinthInfo);
        }

        public int AddHardCurrency(int value)
        {
            if (value <= 0)
                return 0;
            this.PlayerCharacter.hardCurrency += value;
            this.OnPropertiesChanged();
            return value;
        }

        public bool SaveIntoDatabase()
        {
            try
            {
                if (this.m_character.IsDirty)
                {
                    using (PlayerBussiness playerBussiness = new PlayerBussiness())
                    {
                        playerBussiness.UpdatePlayer(this.m_character);
                        if (this.userLabyrinthInfo != null)
                            playerBussiness.UpdateLabyrinthInfo(this.userLabyrinthInfo);
                        foreach (UserGemStone g in this.list_2)
                            playerBussiness.UpdateGemStoneInfo(g);
                    }
                }
                this.EquipBag.SaveToDatabase();
                this.PropBag.SaveToDatabase();
                this.ConsortiaBag.SaveToDatabase();
                this.FarmBag.SaveToDatabase();
                this.CardBag.SaveToDatabase();
                this.StoreBag.SaveToDatabase();
                this.Rank.SaveToDatabase();
                this.QuestInventory.SaveToDatabase();
                this.AchievementInventory.SaveToDatabase();
                this.BufferList.SaveToDatabase();
                this.BattleData.SaveToDatabase();
                this.Extra.SaveToDatabase();
                this.PetBag.SaveToDatabase(true);
                return true;
            }
            catch (Exception ex)
            {
                GamePlayer.log.Error((object)("Error saving player " + this.m_character.NickName + "!"), ex);
                return false;
            }
        }

        public bool SaveNewItems()
        {
            try
            {
                EquipBag.SaveToDatabase();
                PropBag.SaveToDatabase();
                StoreBag.SaveToDatabase();
                CaddyBag.SaveToDatabase();
                CardBag.SaveToDatabase();
                ConsortiaBag.SaveToDatabase();
                return true;
            }
            catch (Exception exception)
            {
                log.Error("Error saving player " + m_character.NickName + "!", exception);
                return false;
            }
        }

        public bool SaveNewsItemIntoDatabase()
        {
            try
            {
                this.EquipBag.SaveNewsItemIntoDatabas();
                this.PropBag.SaveNewsItemIntoDatabas();
                return true;
            }
            catch (Exception ex)
            {
                GamePlayer.log.Error((object)("Error saving Save Bag Into Database " + this.m_character.NickName + "!"), ex);
                return false;
            }
        }

        public bool SavePlayerInfo()
        {
            try
            {
                if (this.m_character.IsDirty)
                {
                    using (PlayerBussiness playerBussiness = new PlayerBussiness())
                        playerBussiness.UpdatePlayer(this.m_character);
                }
                return true;
            }
            catch (Exception ex)
            {
                GamePlayer.log.Error((object)("Error saving player info of " + this.m_character.UserName + "!"), ex);
                return false;
            }
        }

        public void SendConsortiaBossInfo(ConsortiaInfo info)
        {
            RankingPersonInfo rankingPersonInfo1 = (RankingPersonInfo)null;
            List<RankingPersonInfo> rankingPersonInfoList = new List<RankingPersonInfo>();
            foreach (RankingPersonInfo rankingPersonInfo2 in info.RankList.Values)
            {
                if (rankingPersonInfo2.Name == this.PlayerCharacter.NickName)
                    rankingPersonInfo1 = rankingPersonInfo2;
                else
                    rankingPersonInfoList.Add(rankingPersonInfo2);
            }
            GSPacketIn pkg = new GSPacketIn((short)129, this.PlayerCharacter.ID);
            pkg.WriteByte((byte)30);
            pkg.WriteByte((byte)info.bossState);
            pkg.WriteBoolean(rankingPersonInfo1 != null);
            if (rankingPersonInfo1 != null)
            {
                pkg.WriteInt(rankingPersonInfo1.ID);
                pkg.WriteInt(rankingPersonInfo1.TotalDamage);
                pkg.WriteInt(rankingPersonInfo1.Honor);
                pkg.WriteInt(rankingPersonInfo1.Damage);
            }
            pkg.WriteByte((byte)rankingPersonInfoList.Count);
            foreach (RankingPersonInfo rankingPersonInfo2 in rankingPersonInfoList)
            {
                pkg.WriteString(rankingPersonInfo2.Name);
                pkg.WriteInt(rankingPersonInfo2.ID);
                pkg.WriteInt(rankingPersonInfo2.TotalDamage);
                pkg.WriteInt(rankingPersonInfo2.Honor);
                pkg.WriteInt(rankingPersonInfo2.Damage);
            }
            pkg.WriteByte((byte)info.extendAvailableNum);
            pkg.WriteDateTime(info.endTime);
            pkg.WriteInt(info.callBossLevel);
            this.SendTCP(pkg);
        }

        public void SendConsortiaBossOpenClose(int type)
        {
            GSPacketIn pkg = new GSPacketIn((short)129, this.PlayerCharacter.ID);
            pkg.WriteByte((byte)31);
            pkg.WriteByte((byte)type);
            this.SendTCP(pkg);
        }



        public void SendConsortiaFight(int consortiaID, int riches, string msg)
        {
            GSPacketIn packet = new GSPacketIn((short)158);
            packet.WriteInt(consortiaID);
            packet.WriteInt(riches);
            packet.WriteString(msg);
            GameServer.Instance.LoginServer.SendPacket(packet);
        }

        public void SendHideMessage(string msg)
        {
            GSPacketIn pkg = new GSPacketIn((short)3);
            pkg.WriteInt(3);
            pkg.WriteString(msg);
            this.SendTCP(pkg);
        }

        public void SendInsufficientMoney(int type)
        {
            GSPacketIn pkg = new GSPacketIn((short)88, this.PlayerId);
            pkg.WriteByte((byte)type);
            pkg.WriteBoolean(false);
            this.SendTCP(pkg);
        }

        public void SendItemNotice(SqlDataProvider.Data.ItemInfo info, int typeGet, string Name)
        {
            if (info == null)
                return;
            int val;
            switch (typeGet)
            {
                case -1:
                    val = 3;
                    break;
                case 0:
                case 1:
                    val = 2;
                    break;
                case 2:
                case 3:
                case 4:
                    val = 1;
                    break;
                default:
                    val = 3;
                    break;
            }
            GSPacketIn packet = new GSPacketIn((short)14);
            packet.WriteString(this.PlayerCharacter.NickName);
            packet.WriteInt(typeGet);
            packet.WriteInt(info.TemplateID);
            packet.WriteBoolean(info.IsBinds);
            packet.WriteInt(val);
            if (val == 3)
                packet.WriteString(Name);
            if (!info.IsTips && info.Template.Quality < 5 && !info.IsBead())
                return;
            foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                allPlayer.Out.SendTCP(packet);
        }

        public bool SendItemsToMail(SqlDataProvider.Data.ItemInfo item, string content, string title, eMailType type)
        {
            return this.SendItemsToMail(new List<SqlDataProvider.Data.ItemInfo>()
      {
        item
      }, content, title, type);
        }

        public bool SendItemsToMail(
          List<SqlDataProvider.Data.ItemInfo> items,
          string content,
          string title,
          eMailType type)
        {
            using (PlayerBussiness pb = new PlayerBussiness())
            {
                List<SqlDataProvider.Data.ItemInfo> items1 = new List<SqlDataProvider.Data.ItemInfo>();
                foreach (SqlDataProvider.Data.ItemInfo itemInfo1 in items)
                {
                    if (itemInfo1.Template.MaxCount == 1)
                    {
                        for (int index = 0; index < itemInfo1.Count; ++index)
                        {
                            SqlDataProvider.Data.ItemInfo itemInfo2 = SqlDataProvider.Data.ItemInfo.CloneFromTemplate(itemInfo1.Template, itemInfo1);
                            itemInfo2.Count = 1;
                            items1.Add(itemInfo2);
                        }
                    }
                    else
                        items1.Add(itemInfo1);
                }
                return this.SendItemsToMail(items1, content, title, type, pb);
            }
        }

        public bool SendItemsToMail(
          List<SqlDataProvider.Data.ItemInfo> items,
          string content,
          string title,
          eMailType type,
          PlayerBussiness pb)
        {
            bool flag = true;
            for (int index1 = 0; index1 < items.Count; index1 += 5)
            {
                MailInfo mail = new MailInfo()
                {
                    Title = title != null ? title : LanguageMgr.GetTranslation("Game.Server.GameUtils.Title"),
                    Gold = 0,
                    IsExist = true,
                    Money = 0,
                    Receiver = this.PlayerCharacter.NickName,
                    ReceiverID = this.PlayerId,
                    Sender = this.PlayerCharacter.NickName,
                    SenderID = this.PlayerId,
                    Type = (int)type,
                    GiftToken = 0
                };
                List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
                StringBuilder stringBuilder1 = new StringBuilder();
                StringBuilder stringBuilder2 = new StringBuilder();
                stringBuilder1.Append(LanguageMgr.GetTranslation("Game.Server.GameUtils.CommonBag.AnnexRemark"));
                content = content != null ? LanguageMgr.GetTranslation(content) : "";
                int index2 = index1;
                int itemId;
                if (items.Count > index2)
                {
                    SqlDataProvider.Data.ItemInfo itemInfo = items[index2];
                    if (itemInfo.ItemID == 0)
                        pb.AddGoods(itemInfo);
                    else
                        itemInfoList.Add(itemInfo);
                    if (title == null)
                        mail.Title = itemInfo.Template.Name;
                    MailInfo mailInfo = mail;
                    itemId = itemInfo.ItemID;
                    string str = itemId.ToString();
                    mailInfo.Annex1 = str;
                    mail.Annex1Name = itemInfo.Template.Name;
                    stringBuilder1.Append("1、" + mail.Annex1Name + "x" + (object)itemInfo.Count + ";");
                    stringBuilder2.Append("1、" + mail.Annex1Name + "x" + (object)itemInfo.Count + ";");
                }
                int index3 = index1 + 1;
                if (items.Count > index3)
                {
                    SqlDataProvider.Data.ItemInfo itemInfo = items[index3];
                    if (itemInfo.ItemID == 0)
                        pb.AddGoods(itemInfo);
                    else
                        itemInfoList.Add(itemInfo);
                    MailInfo mailInfo = mail;
                    itemId = itemInfo.ItemID;
                    string str = itemId.ToString();
                    mailInfo.Annex2 = str;
                    mail.Annex2Name = itemInfo.Template.Name;
                    stringBuilder1.Append("2、" + mail.Annex2Name + "x" + (object)itemInfo.Count + ";");
                    stringBuilder2.Append("2、" + mail.Annex2Name + "x" + (object)itemInfo.Count + ";");
                }
                int index4 = index1 + 2;
                if (items.Count > index4)
                {
                    SqlDataProvider.Data.ItemInfo itemInfo = items[index4];
                    if (itemInfo.ItemID == 0)
                        pb.AddGoods(itemInfo);
                    else
                        itemInfoList.Add(itemInfo);
                    MailInfo mailInfo = mail;
                    itemId = itemInfo.ItemID;
                    string str = itemId.ToString();
                    mailInfo.Annex3 = str;
                    mail.Annex3Name = itemInfo.Template.Name;
                    stringBuilder1.Append("3、" + mail.Annex3Name + "x" + (object)itemInfo.Count + ";");
                    stringBuilder2.Append("3、" + mail.Annex3Name + "x" + (object)itemInfo.Count + ";");
                }
                int index5 = index1 + 3;
                if (items.Count > index5)
                {
                    SqlDataProvider.Data.ItemInfo itemInfo = items[index5];
                    if (itemInfo.ItemID == 0)
                        pb.AddGoods(itemInfo);
                    else
                        itemInfoList.Add(itemInfo);
                    MailInfo mailInfo = mail;
                    itemId = itemInfo.ItemID;
                    string str = itemId.ToString();
                    mailInfo.Annex4 = str;
                    mail.Annex4Name = itemInfo.Template.Name;
                    stringBuilder1.Append("4、" + mail.Annex4Name + "x" + (object)itemInfo.Count + ";");
                    stringBuilder2.Append("4、" + mail.Annex4Name + "x" + (object)itemInfo.Count + ";");
                }
                int index6 = index1 + 4;
                if (items.Count > index6)
                {
                    SqlDataProvider.Data.ItemInfo itemInfo = items[index6];
                    if (itemInfo.ItemID == 0)
                        pb.AddGoods(itemInfo);
                    else
                        itemInfoList.Add(itemInfo);
                    MailInfo mailInfo = mail;
                    itemId = itemInfo.ItemID;
                    string str = itemId.ToString();
                    mailInfo.Annex5 = str;
                    mail.Annex5Name = itemInfo.Template.Name;
                    stringBuilder1.Append("5、" + mail.Annex5Name + "x" + (object)itemInfo.Count + ";");
                    stringBuilder2.Append("5、" + mail.Annex5Name + "x" + (object)itemInfo.Count + ";");
                }
                mail.AnnexRemark = stringBuilder1.ToString();
                mail.Content = content != null || stringBuilder2.ToString() != null ? (!(content != "") ? stringBuilder2.ToString() : content) : LanguageMgr.GetTranslation("Game.Server.GameUtils.Content");
                if (pb.SendMail(mail))
                {
                    foreach (SqlDataProvider.Data.ItemInfo itemInfo in itemInfoList)
                        this.TakeOutItem(itemInfo);
                }
                else
                    flag = false;
            }
            return flag;
        }

        public bool SendItemToMail(int templateID, string content, string title)
        {
            ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(templateID);
            if (itemTemplate == null)
                return false;
            if (content == "")
                content = itemTemplate.Name + "x1";
            SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(itemTemplate, 1, 104);
            fromTemplate.IsBinds = true;
            return this.SendItemToMail(fromTemplate, content, title, eMailType.Active);
        }

        public bool SendItemToMail(SqlDataProvider.Data.ItemInfo item, string content, string title, eMailType type)
        {
            using (PlayerBussiness pb = new PlayerBussiness())
                return this.SendItemToMail(item, pb, content, title, type);
        }

        public bool SendItemToMail(
          SqlDataProvider.Data.ItemInfo item,
          PlayerBussiness pb,
          string content,
          string title,
          eMailType type)
        {
            MailInfo mail = new MailInfo()
            {
                Content = content != null ? content : LanguageMgr.GetTranslation("Game.Server.GameUtils.Content"),
                Title = title != null ? title : LanguageMgr.GetTranslation("Game.Server.GameUtils.Title"),
                Gold = 0,
                IsExist = true,
                Money = 0,
                GiftToken = 0,
                Receiver = this.PlayerCharacter.NickName,
                ReceiverID = this.PlayerCharacter.ID,
                Sender = this.PlayerCharacter.NickName,
                SenderID = this.PlayerCharacter.ID,
                Type = (int)type
            };
            if (item.ItemID == 0)
                pb.AddGoods(item);
            mail.Annex1 = item.ItemID.ToString();
            mail.Annex1Name = item.Template.Name;
            if (!pb.SendMail(mail))
                return false;
            this.TakeOutItem(item);
            return true;
        }

        public bool SendMailToUser(PlayerBussiness pb, string content, string title, eMailType type)
        {
            MailInfo mail = new MailInfo()
            {
                Content = content,
                Title = title,
                Gold = 0,
                IsExist = true,
                Money = 0,
                GiftToken = 0,
                Receiver = this.PlayerCharacter.NickName,
                ReceiverID = this.PlayerCharacter.ID,
                Sender = this.PlayerCharacter.NickName,
                SenderID = this.PlayerCharacter.ID,
                Type = (int)type
            };
            mail.Annex1 = "";
            mail.Annex1Name = "";
            return pb.SendMail(mail);
        }

        public void SendMessage(string msg)
        {
            GSPacketIn pkg = new GSPacketIn((short)3);
            pkg.WriteInt(0);
            pkg.WriteString(msg);
            this.SendTCP(pkg);
        }

        public bool SendMoneyMailToUser(
          PlayerBussiness pb,
          string content,
          string title,
          int money,
          eMailType type)
        {
            MailInfo mail = new MailInfo()
            {
                Content = content,
                Title = title,
                Gold = 0,
                IsExist = true,
                Money = money,
                GiftToken = 0,
                Receiver = this.PlayerCharacter.NickName,
                ReceiverID = this.PlayerCharacter.ID,
                Sender = this.PlayerCharacter.NickName,
                SenderID = this.PlayerCharacter.ID,
                Type = (int)type
            };
            mail.Annex1 = "";
            mail.Annex1Name = "";
            return pb.SendMail(mail);
        }

        public void SendPrivateChat(
          int receiverID,
          string receiver,
          string sender,
          string msg,
          bool isAutoReply)
        {
            GSPacketIn pkg = new GSPacketIn((short)37, this.PlayerCharacter.ID);
            pkg.WriteInt(receiverID);
            pkg.WriteString(receiver);
            pkg.WriteString(sender);
            pkg.WriteString(msg);
            pkg.WriteBoolean(isAutoReply);
            this.SendTCP(pkg);
        }

        public void SendTCP(GSPacketIn pkg)
        {
            if (!this.m_client.IsConnected)
                return;
            this.m_client.SendTCP(pkg);
        }

        public ConsortiaProcessor Consortia
        {
            get
            {
                return this.consortiaProcessor_0;
            }
        }

        public List<UserGemStone> GemStone
        {
            get
            {
                return this.list_2;
            }
            set
            {
                this.list_2 = value;
            }
        }

        public UserLabyrinthInfo Labyrinth
        {
            get
            {
                return this.userLabyrinthInfo;
            }
            set
            {
                this.userLabyrinthInfo = value;
            }
        }

        public bool SetPvePermission(int copyId, eHardLevel hardLevel)
        {
            if (hardLevel != eHardLevel.Epic && copyId <= this.m_pvepermissions.Length && (copyId > 0 && hardLevel != eHardLevel.Terror) && (int)this.m_pvepermissions[copyId - 1] == (int)GamePlayer.permissionChars[(int)hardLevel])
            {
                this.m_pvepermissions[copyId - 1] = GamePlayer.permissionChars[(int)(hardLevel + 1)];
                this.m_character.PvePermission = this.ConverterPvePermission(this.m_pvepermissions);
                this.OnPropertiesChanged();
            }
            return true;
        }

        public void OpenAllNoviceActive()
        {
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                foreach (EventRewardProcessInfo rewardProcessInfo in playerBussiness.GetUserEventProcess(this.PlayerId))
                    this.Out.SendOpenNoviceActive(0, rewardProcessInfo.ActiveType, rewardProcessInfo.Conditions, rewardProcessInfo.AwardGot, DateTime.Now, DateTime.Now.AddYears(2));
            }
        }

        public void ShowAllFootballCard()
        {
            for (int index = 0; index < this.CardsTakeOut.Length; ++index)
            {
                if (this.CardsTakeOut[index] == null)
                {
                    this.CardsTakeOut[index] = this.Card[index];
                    if (this.takeoutCount > 0)
                        this.TakeFootballCard(this.Card[index]);
                }
            }
        }

        public bool StackItemToAnother(SqlDataProvider.Data.ItemInfo item)
        {
            return this.GetItemInventory(item.Template).StackItemToAnother(item);
        }

        public void TakeFootballCard(CardInfo card)
        {
            List<SqlDataProvider.Data.ItemInfo> infos = new List<SqlDataProvider.Data.ItemInfo>();
            for (int index = 0; index < this.CardsTakeOut.Length; ++index)
            {
                if (card.place == index)
                {
                    this.CardsTakeOut[index] = card;
                    this.CardsTakeOut[index].IsTake = true;
                    ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(card.templateID);
                    if (itemTemplate != null)
                        infos.Add(SqlDataProvider.Data.ItemInfo.CreateFromTemplate(itemTemplate, card.count, 110));
                    --this.takeoutCount;
                    break;
                }
            }
            if (infos.Count <= 0)
                return;
            foreach (SqlDataProvider.Data.ItemInfo itemInfo in infos)
                this.AddTemplate(infos);
        }

        public bool TakeOutItem(SqlDataProvider.Data.ItemInfo item)
        {
            if (item.BagType == this.m_propBag.BagType)
                return this.m_propBag.TakeOutItem(item);
            if (item.BagType == this.m_fightBag.BagType)
                return this.m_fightBag.TakeOutItem(item);
            if (item.BagType == this.m_ConsortiaBag.BagType)
                return this.m_ConsortiaBag.TakeOutItem(item);
            return this.m_equipBag.TakeOutItem(item);
        }


        public override string ToString()
        {
            return string.Format("Id:{0} nickname:{1} room:{2} ", (object)this.PlayerId, (object)this.PlayerCharacter.NickName, (object)this.CurrentRoom);
        }

        public void UpdateAnswerSite(int id)
        {
            if (this.PlayerCharacter.AnswerSite < id)
                this.PlayerCharacter.AnswerSite = id;
            this.UpdateWeaklessGuildProgress();
            this.Out.SendWeaklessGuildProgress(this.PlayerCharacter);
        }

        public void UpdateBadgeId(int Id)
        {
            this.m_character.badgeID = Id;
        }

        public void UpdateBarrier(int barrier, string pic)
        {
            if (this.CurrentRoom == null)
                return;
            this.CurrentRoom.Pic = pic;
            this.CurrentRoom.barrierNum = barrier;
            this.CurrentRoom.currentFloor = barrier;
        }

        public void UpdateBaseProperties(int attack, int defence, int agility, int lucky, int hp)
        {
            if (attack != this.m_character.Attack || defence != this.m_character.Defence || (agility != this.m_character.Agility || lucky != this.m_character.Luck))
            {
                this.m_character.Attack = attack;
                this.m_character.Defence = defence;
                this.m_character.Agility = agility;
                this.m_character.Luck = lucky;
                this.OnPropertiesChanged();
            }
            this.m_character.hp = (int)((double)(hp + this.LevelPlusBlood + this.m_character.Defence / 10) * this.GetBaseBlood());
        }

        public bool UpdateChangedPlaces()
        {
            try
            {
                this.EquipBag.UpdateChangedPlaces();
                this.PropBag.UpdateChangedPlaces();
                return true;
            }
            catch (Exception ex)
            {
                GamePlayer.log.Error((object)("Error Update Changed Places " + this.m_character.NickName + "!"), ex);
                return false;
            }
        }

        public void UpdateDrill(int index, UserDrillInfo drill)
        {
            this.m_userDrills[index] = drill;
        }

        public void UpdateFightBuff(BufferInfo info)
        {
            int num = -1;
            for (int index = 0; index < this.FightBuffs.Count; ++index)
            {
                if (info != null && info.Type == this.FightBuffs[index].Type)
                {
                    this.FightBuffs[index] = info;
                    num = info.Type;
                }
            }
            if (num != -1)
                return;
            this.FightBuffs.Add(info);
        }

        public void UpdateFightPower()
        {
            int num1 = 0;
            this.FightPower = 0;
            int hp = this.PlayerCharacter.hp;
            int num2 = num1 + this.PlayerCharacter.Attack + this.PlayerCharacter.Defence + this.PlayerCharacter.Agility + this.PlayerCharacter.Luck;
            double baseAttack = this.GetBaseAttack();
            double baseDefence = this.GetBaseDefence();
            this.FightPower += (int)((double)(num2 + 1000) * (baseAttack * baseAttack * baseAttack + 3.5 * baseDefence * baseDefence * baseDefence) / 100000000.0 + (double)hp * 0.95);
            if (this.m_currentSecondWeapon != null)
                this.FightPower += (int)((double)this.m_currentSecondWeapon.Template.Property7 * Math.Pow(1.1, (double)this.m_currentSecondWeapon.StrengthenLevel));
            if (this.FightPower < 0)
                this.FightPower = int.MaxValue;
            this.PlayerCharacter.FightPower = this.FightPower;
            this.OnPlayerPropertyChanged(this.m_character);
        }

        public void UpdateHealstone(SqlDataProvider.Data.ItemInfo item)
        {
            if (item == null)
                return;
            this.m_healstone = item;
        }

        public void UpdateHide(int hide)
        {
            if (hide == this.m_character.Hide)
                return;
            this.m_character.Hide = hide;
            this.OnPropertiesChanged();
        }

        public void UpdateHonor(string honor)
        {
            this.PlayerCharacter.Honor = honor;
            if (!this.Rank.IsRank(honor))
                return;
            this.EquipBag.UpdatePlayerProperties();
        }

        public void UpdateItem(SqlDataProvider.Data.ItemInfo item)
        {
            this.GetInventory((eBageType)item.BagType)?.UpdateItem(item);
        }

        public void LoadGemStone(PlayerBussiness db)
        {
            lock (this.list_2)
            {
                this.list_2 = db.GetSingleGemStones(this.m_character.ID);
                if (this.list_2.Count != 0)
                    return;
                List<int> intList1 = new List<int>()
        {
          11,
          5,
          2,
          3,
          13
        };
                List<int> intList2 = new List<int>()
        {
          100002,
          100003,
          100001,
          100004,
          100005
        };
                for (int index = 0; index < intList1.Count; ++index)
                {
                    UserGemStone userGemStone1 = new UserGemStone();
                    userGemStone1.ID = 0;
                    int id = this.m_character.ID;
                    userGemStone1.UserID = id;
                    int num1 = intList2[index];
                    userGemStone1.FigSpiritId = num1;
                    string str = "0,0,0|0,0,1|0,0,2";
                    userGemStone1.FigSpiritIdValue = str;
                    int num2 = intList1[index];
                    userGemStone1.EquipPlace = num2;
                    UserGemStone userGemStone2 = userGemStone1;
                    this.list_2.Add(userGemStone2);
                    db.AddUserGemStone(userGemStone2);
                }
            }
        }

        public UserGemStone GetGemStone(int place)
        {
            return this.list_2.FirstOrDefault<UserGemStone>((Func<UserGemStone, bool>)(g => place == g.EquipPlace));
        }

        public void UpdateGemStone(int place, UserGemStone gem)
        {
            lock (this.list_2)
            {
                for (int index = 0; index < this.list_2.Count; ++index)
                {
                    if (place == this.list_2[index].EquipPlace)
                    {
                        this.list_2[index] = gem;
                        break;
                    }
                }
            }
        }

        public void UpdateItemForUser(object state)
        {
            this.m_battle.LoadFromDatabase();
            this.m_equipBag.LoadFromDatabase();
            this.m_propBag.LoadFromDatabase();
            this.m_ConsortiaBag.LoadFromDatabase();
            this.m_storeBag.LoadFromDatabase();
            this.m_cardBag.LoadFromDatabase();
            this.m_questInventory.LoadFromDatabase(this.m_character.ID);
            this.m_achievementInventory.LoadFromDatabase(this.m_character.ID);
            this.m_eventLiveInventory.LoadFromDatabase();
            this.m_bufferList.LoadFromDatabase(this.m_character.ID);
            this.m_rank.LoadFromDatabase();
            this.m_extra.LoadFromDatabase();
            this.m_petBag.LoadFromDatabase();
        }

        public void UpdateLevel()
        {
            this.Level = LevelMgr.GetLevel(this.m_character.GP);
            int maxLevel = LevelMgr.MaxLevel;
            LevelInfo level = LevelMgr.FindLevel(maxLevel);
            if (this.Extra.CheckNoviceActiveOpen(NoviceActiveType.GRADE_UP_ACTIVE))
                this.Extra.UpdateEventCondition(1, this.Level);
            this.OnLevelUp(this.Level);
            if (this.Level != maxLevel || level == null)
                return;
            this.m_character.GP = level.GP;
        }

        public void UpdatePet(UsersPetInfo pet)
        {
            this.m_pet = pet;
        }

        public void UpdateProperties()
        {
            this.Out.SendUpdatePrivateInfo(this.m_character, this.GetMedalNum());
            GSPacketIn pkg = this.Out.SendUpdatePublicPlayer(this.m_character, this.MatchInfo, this.m_extra.Info);
            if (this.m_currentRoom == null)
                return;
            this.m_currentRoom.SendToAll(pkg, this);
        }

        public void UpdatePveResult(string type, int value, bool isWin)
        {
           
            string msg = "";
            if (type != null)
            {
                if (type != "worldboss")
                {
                    if (type != "consortiaboss")
                    {
                        if (type != "yearmonter" && type == "qx")
                        {
                            if (!isWin)
                            {
                                List<SqlDataProvider.Data.ItemInfo> info = (List<SqlDataProvider.Data.ItemInfo>)null;
                                DropInventory.CopyAllDrop(value, ref info);
                                int num2 = value - 70000;
                                if (value >= 70006)
                                    num2 -= 2;
                                string title = "Recompensas " + (object)num2;
                                if (info != null)
                                    WorldEventMgr.SendItemsToMail(info, this.PlayerCharacter.ID, this.PlayerCharacter.NickName, title, (string)null);
                            }
                            
                        }
                    }
                    else
                    {
                        int richer = value / 800;
                        int honor = value / 1200;
                        msg = string.Format("Parabéns! Recebeu {0} e {1} pontos de honras.", (object)richer, (object)honor);
                        this.AddRichesOffer(richer);
                        ConsortiaBossMgr.UpdateBlood(this.PlayerCharacter.ConsortiaID, value);
                        ConsortiaBossMgr.UpdateRank(this.PlayerCharacter.ConsortiaID, value, richer, honor, this.PlayerCharacter.NickName, this.PlayerCharacter.ID);
                    }
                }
                else
                    msg = LanguageMgr.GetTranslation("GamePlayer.Msg20", (object)(value / 400), (object)(value / 1200));
            }
            if (string.IsNullOrEmpty(msg))
                return;
            this.SendMessage(msg);
        }

        public int AddEliteScore(int value)
        {
            if (value > 0)
            {
                this.PlayerCharacter.EliteScore += value;
                GameServer.Instance.LoginServer.SendEliteScoreUpdate(this.PlayerCharacter.ID, this.PlayerCharacter.NickName, this.PlayerCharacter.Grade <= 40 ? 1 : 2, this.PlayerCharacter.EliteScore);
            }
            return 0;
        }

        public int RemoveEliteScore(int value)
        {
            if (value > 0)
            {
                this.PlayerCharacter.EliteScore -= value;
                if (this.PlayerCharacter.EliteScore <= 0)
                    this.PlayerCharacter.EliteScore = 1;
                GameServer.Instance.LoginServer.SendEliteScoreUpdate(this.PlayerCharacter.ID, this.PlayerCharacter.NickName, this.PlayerCharacter.Grade <= 40 ? 1 : 2, this.PlayerCharacter.EliteScore);
            }
            return 0;
        }

        public void SendWinEliteChampion()
        {
            EliteGameRoundInfo eliteRoundByUser = ExerciseMgr.FindEliteRoundByUser(this.PlayerCharacter.ID);
            if (eliteRoundByUser != null)
            {
                eliteRoundByUser.PlayerWin = eliteRoundByUser.PlayerOne.UserID == this.PlayerCharacter.ID ? eliteRoundByUser.PlayerOne : eliteRoundByUser.PlayerTwo;
                GameServer.Instance.LoginServer.SendEliteChampionRoundUpdate(eliteRoundByUser);
                ExerciseMgr.RemoveEliteRound(eliteRoundByUser);
            }
            else
                GamePlayer.log.Error((object)("////// ELITEGAME Send Win Elite Champion Round ERROR NOT FOUND: " + this.PlayerCharacter.UserName));
        }

        public void OnTakeCard(int roomType, int place, int templateId, int count)
        {
            this.TakeCardPlace = place;
            this.TakeCardTemplateID = templateId;
            this.TakeCardCount = count;
        }

        public void UpdateReduceDame(SqlDataProvider.Data.ItemInfo item)
        {
            if (item == null || item.Template == null)
                return;
            this.PlayerCharacter.ReduceDamePlus = item.Template.Property1;
        }

        public void UpdateSecondWeapon(SqlDataProvider.Data.ItemInfo item)
        {
            if (item == this.m_currentSecondWeapon)
                return;
            this.m_currentSecondWeapon = item;
            this.OnPropertiesChanged();
        }

        public void UpdateStyle(string style, string colors, string skin)
        {
            if (!(style != this.m_character.Style) && !(colors != this.m_character.Colors) && !(skin != this.m_character.Skin))
                return;
            this.m_character.Style = style;
            this.m_character.Colors = colors;
            this.m_character.Skin = skin;
            this.OnPropertiesChanged();
        }

        public void UpdateWeaklessGuildProgress()
        {
            if (this.PlayerCharacter.weaklessGuildProgress == null)
                this.PlayerCharacter.weaklessGuildProgress = Base64.decodeToByteArray(this.PlayerCharacter.WeaklessGuildProgressStr);
            this.PlayerCharacter.CheckLevelFunction();
            if (this.PlayerCharacter.Grade == 1)
                this.PlayerCharacter.openFunction(Step.GAIN_ADDONE);
            if (this.PlayerCharacter.IsOldPlayer)
                this.PlayerCharacter.openFunction(Step.OLD_PLAYER);
            this.PlayerCharacter.WeaklessGuildProgressStr = Base64.encodeByteArray(this.PlayerCharacter.weaklessGuildProgress);
        }

        public void UpdateWeapon(SqlDataProvider.Data.ItemInfo item)
        {
            if (item == this.m_MainWeapon)
                return;
            this.m_MainWeapon = item;
            this.OnPropertiesChanged();
        }

        public bool UsePropItem(AbstractGame game, int bag, int place, int templateId, bool isLiving)
        {
            if (bag == 1)
            {
                ItemTemplateInfo fightingProp = PropItemMgr.FindFightingProp(templateId);
                if (isLiving && fightingProp != null)
                {
                    this.OnUsingItem(fightingProp.TemplateID, 1);
                    if (place == -1 && this.CanUseProp)
                        return true;
                    SqlDataProvider.Data.ItemInfo itemAt = this.m_propBag.GetItemAt(place);
                    if (itemAt != null && itemAt.IsValidItem() && itemAt.Count >= 0)
                    {
                        this.m_propBag.RemoveCountFromStack(itemAt, 1);
                        return true;
                    }
                }
            }
            else
            {
                SqlDataProvider.Data.ItemInfo itemAt = this.m_fightBag.GetItemAt(place);
                if (itemAt != null)
                {
                    this.OnUsingItem(itemAt.TemplateID, 1);
                    if (itemAt.TemplateID == templateId)
                        return this.m_fightBag.RemoveItem(itemAt);
                }
            }
            return false;
        }

        public void ViFarmsAdd(int playerID)
        {
            if (this._viFarms.Contains(playerID))
                return;
            this._viFarms.Add(playerID);
        }

        public void OnPlayerAddItem(string type, int value)
        {
            if (this.PlayerAddItem == null)
                return;
            this.PlayerAddItem(type, value);
        }

        public void OnPlayerSpa(int onlineTimeSpa)
        {
            if (this.PlayerSpa == null)
                return;
            this.PlayerSpa(onlineTimeSpa);
        }

        public void ViFarmsRemove(int playerID)
        {
            if (!this._viFarms.Contains(playerID))
                return;
            this._viFarms.Remove(playerID);
        }

        public string Account
        {
            get
            {
                return this.m_account;
            }
        }

        public AchievementInventory AchievementInventory
        {
            get
            {
                return this.m_achievementInventory;
            }
        }

        public EventInventory EventLiveInventory
        {
            get
            {
                return this.m_eventLiveInventory;
            }
        }

        public long AllWorldDameBoss { get; set; }

        public PlayerBattle BattleData
        {
            get
            {
                return this.m_battle;
            }
        }

        public bool bool_1 { get; set; }

        public bool Boolean_0
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
            }
        }

        public BufferList BufferList
        {
            get
            {
                return this.m_bufferList;
            }
        }

        public PlayerInventory CaddyBag
        {
            get
            {
                return this.m_caddyBag;
            }
        }

        public bool CanUseProp { get; set; }

        public double GPAddPlus { get; set; }

        public double OfferAddPlus { get; set; }

        public bool CanX2Exp { get; set; }

        public bool CanX3Exp { get; set; }

        public CardInventory CardBag
        {
            get
            {
                return this.m_cardBag;
            }
        }

        public GameClient Client
        {
            get
            {
                return this.m_client;
            }
        }

        public PlayerInventory ConsortiaBag
        {
            get
            {
                return this.m_ConsortiaBag;
            }
        }

        public HotSpringRoom CurrentHotSpringRoom
        {
            get
            {
                return this.hotSpringRoom_0;
            }
            set
            {
                this.hotSpringRoom_0 = value;
            }
        }

        public MarryRoom CurrentMarryRoom
        {
            get
            {
                return this.m_currentMarryRoom;
            }
            set
            {
                this.m_currentMarryRoom = value;
            }
        }

        public BaseRoom CurrentRoom
        {
            get
            {
                return this.m_currentRoom;
            }
            set
            {
                BaseRoom room = Interlocked.Exchange<BaseRoom>(ref this.m_currentRoom, value);
                if (room == null)
                    return;
                RoomMgr.ExitRoom(room, this);
            }
        }

        public PlayerEquipInventory EquipBag
        {
            get
            {
                return this.m_equipBag;
            }
        }

        public List<int> EquipEffect
        {
            get
            {
                return this.m_equipEffect;
            }
            set
            {
                this.m_equipEffect = value;
            }
        }

        public PlayerExtra Extra
        {
            get
            {
                return this.m_extra;
            }
        }

        public PlayerInventory FarmBag
        {
            get
            {
                return this.m_farmBag;
            }
        }

        public PlayerInventory FightBag
        {
            get
            {
                return this.m_fightBag;
            }
        }

        public List<BufferInfo> FightBuffs
        {
            get
            {
                return this.m_fightBuffInfo;
            }
            set
            {
                this.m_fightBuffInfo = value;
            }
        }

        public PlayerInventory Food
        {
            get
            {
                return this.m_food;
            }
        }

        public Dictionary<int, int> Friends
        {
            get
            {
                return this._friends;
            }
        }

        public BaseGame game
        {
            get
            {
                return this.m_game;
            }
            set
            {
                this.m_game = value;
            }
        }

        public int GameId
        {
            get
            {
                return this.int_6;
            }
            set
            {
                this.int_6 = value;
            }
        }

        public int GamePlayerId { get; set; }

        public SqlDataProvider.Data.ItemInfo Healstone
        {
            get
            {
                if (this.m_healstone == null)
                    return (SqlDataProvider.Data.ItemInfo)null;
                return this.m_healstone;
            }
        }

        public int Immunity
        {
            get
            {
                return this.m_immunity;
            }
            set
            {
                this.m_immunity = value;
            }
        }

        public bool IsAASInfo
        {
            get
            {
                return this.m_isAASInfo;
            }
            set
            {
                this.m_isAASInfo = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return this.m_client.IsConnected;
            }
        }

        public bool IsInMarryRoom
        {
            get
            {
                return this.m_currentMarryRoom != null;
            }
        }

        public bool IsMinor
        {
            get
            {
                return this.m_isMinor;
            }
            set
            {
                this.m_isMinor = value;
            }
        }

        public int Level
        {
            get
            {
                return this.m_character.Grade;
            }
            set
            {
                if (value == this.m_character.Grade)
                    return;
                int grade = this.m_character.Grade;
                new PlayerBussiness().AddDailyRecord(new DailyRecordInfo()
                {
                    UserID = this.m_character.ID,
                    Type = 2,
                    Value = string.Format("{0},{1}", (object)this.m_character.Grade, (object)value)
                });
                this.Extra.UpdateEventCondition(1, value);
                this.m_character.Grade = value;
                if (value == 6)
                    this.AddTemplate(SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(112098), 1, 104));
                if (value == 8)
                    this.PlayerCharacter.WeaklessGuildProgressStr = "////b7D/ht8WDQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";
                if (this.m_character.masterID != 0 && grade < this.m_character.Grade)
                    AcademyMgr.UpdateAwardApp(this, grade);
                this.OnLevelUp(value);
                this.OnPropertiesChanged();
            }
        }



        private void method_5()
        {
            ConsortiaProcessor = new ConsortiaProcessor(m_consortiaProcessor);
            petProcessor_0 = new PetProcessor(m_petProcessor);
        }
        public int LevelPlusBlood
        {
            get
            {
                return LevelMgr.FindLevel(this.m_character.Grade).Blood;
            }
        }

        public SqlDataProvider.Data.ItemInfo MainWeapon
        {
            get
            {
                return this.m_MainWeapon;
            }
        }

        public UserMatchInfo MatchInfo
        {
            get
            {
                return this.m_battle.MatchInfo;
            }
        }

        public IPacketLib Out
        {
            get
            {
                return this.m_client.Out;
            }
        }

        public UsersPetInfo Pet
        {
            get
            {
                return this.m_pet;
            }
        }

        public long PingTime
        {
            get
            {
                return this.m_pingTime;
            }
            set
            {
                this.m_pingTime = value;
                GSPacketIn pkg = this.Out.SendNetWork(this.PlayerCharacter.ID, this.m_pingTime);
                if (this.m_currentRoom == null)
                    return;
                this.m_currentRoom.SendToAll(pkg, this);
            }
        }

        public PlayerInfo PlayerCharacter
        {
            get
            {
                return this.m_character;
            }
        }

        public PetInventory PetBag
        {
            get
            {
                return this.m_petBag;
            }
        }

        public int PlayerId
        {
            get
            {
                return this.m_playerId;
            }
        }

        public PlayerProperty PlayerProp
        {
            get
            {
                return this.m_playerProp;
            }
        }

        public Player Players
        {
            get
            {
                return this.m_players;
            }
        }

        public ePlayerState PlayerState
        {
            get
            {
                return this.m_playerState;
            }
            set
            {
                this.m_playerState = value;
            }
        }

        public string ProcessLabyrinthAward { get; set; }

        public PlayerInventory PropBag
        {
            get
            {
                return this.m_propBag;
            }
        }

        public QuestInventory QuestInventory
        {
            get
            {
                return this.m_questInventory;
            }
        }

        public PlayerRank Rank
        {
            get
            {
                return this.m_rank;
            }
        }

        public SqlDataProvider.Data.ItemInfo SecondWeapon
        {
            get
            {
                if (this.m_currentSecondWeapon == null)
                    return (SqlDataProvider.Data.ItemInfo)null;
                return this.m_currentSecondWeapon;
            }
        }

        public int ServerID { get; set; }
        public bool ShowPP
        {
            get
            {
                return this.m_showPP;
            }
            set
            {
                this.m_showPP = value;
            }
        }

        public PlayerInventory StoreBag
        {
            get
            {
                return this.m_storeBag;
            }
        }

        public PlayerInventory TempBag
        {
            get
            {
                return this.m_tempBag;
            }
        }

        public Dictionary<string, object> TempProperties
        {
            get
            {
                return this.m_tempProperties;
            }
        }

        public bool Toemview
        {
            get
            {
                return this.m_toemview;
            }
            set
            {
                this.m_toemview = value;
            }
        }

        public Dictionary<int, UserDrillInfo> UserDrills
        {
            get
            {
                return this.m_userDrills;
            }
            set
            {
                this.m_userDrills = value;
            }
        }

        public PlayerInfo UserVIPInfo
        {
            get
            {
                return this.m_character;
            }
        }

        public List<int> ViFarms
        {
            get
            {
                return this._viFarms;
            }
        }

        public long WorldbossBood { get; set; }

        public int ZoneId
        {
            get
            {
                return GameServer.Instance.Configuration.ServerID;
            }
        }

        public string ZoneName
        {
            get
            {
                return GameServer.Instance.Configuration.ServerName;
            }
        }

        public int Lottery { get; internal set; }

        public List<ItemBoxInfo> LotteryItems { get; internal set; }

        public int LotteryID { get; internal set; }

        public DateTime LastRequestTime { get; internal set; }

        public int CurrentEnemyId { get; set; }

        public event GamePlayer.PlayerQuestFinishEventHandel PlayerQuestFinish;

        public event GamePlayer.PlayerGameOverCountTeamEventHandle GameOverCountTeam;

        public event GamePlayer.PlayerMarryTeamEventHandle GameMarryTeam;

        public void OnPlayerQuestFinish(BaseQuest baseQuest)
        {
            if (this.PlayerQuestFinish == null)
                return;
            this.PlayerQuestFinish(baseQuest);
        }

        public void OnPlayerLogin()
        {
            if (this.PlayerLogin == null)
                return;
            this.PlayerLogin();
        }

        public void OnPlayerPropertyChanged(PlayerInfo character)
        {
            if (this.PlayerPropertyChanged == null)
                return;
            this.PlayerPropertyChanged(character);
        }

        public void OnVIPUpgrade(int level, int exp)
        {
            if (this.Event_0 == null || this.m_character.typeVIP <= 0 || this.m_character.VIPLevel != level)
                return;
            this.Event_0(level, exp);
        }

        public event GamePlayer.PlayerUseBugle UseBugle;

        public void OnUseBugle(int value)
        {
            if (this.UseBugle == null)
                return;
            this.UseBugle(value);
        }

        public event GamePlayer.PlayerMarryEventHandel PlayerMarry;

        public void OnPlayerMarry()
        {
            if (this.PlayerMarry == null)
                return;
            this.PlayerMarry();
        }

        public event GamePlayer.PlayerDispatchesEventHandel PlayerDispatches;

        public void OnPlayerDispatches()
        {
            if (this.PlayerDispatches == null)
                return;
            this.PlayerDispatches();
        }

        public event GamePlayer.PlayerGameOverEventHandle GameOver;

        public event GamePlayer.PlayerGameOverEvent2v2Handle GameOver2v2;

        public event GamePlayer.PlayerAcademyEventHandle AcademyEvent;

        public void OnGameOver(
          AbstractGame game,
          bool isWin,
          int gainXp,
          bool isSpanArea,
          bool isCouple,
          int blood,
          int playerCount)
        {
            if (game.RoomType == eRoomType.Match)
            {
                ++this.PlayerCharacter.CheckCount;
                this.Out.SendCheckCode();
                if (isWin)
                    ++this.m_character.Win;
                ++this.m_character.Total;
                this.UpdateProperties();
            }
            if (blood == 1)
                this.OnFightOneBloodIsWin(game.RoomType, isWin);
            if (playerCount == 4)
                this.OnGameOver2v2(isWin);
            if (isCouple)
                this.GameMarryTeam(game, isWin, gainXp, playerCount);
            if (game.RoomType == eRoomType.Dungeon && this.CurrentRoom != null)
            {
                this.CurrentRoom.LevelLimits = (int)this.CurrentRoom.GetLevelLimit(this);
                this.CurrentRoom.MapId = 10000;
                this.CurrentRoom.HardLevel = eHardLevel.Normal;
                this.CurrentRoom.currentFloor = 0;
                this.CurrentRoom.SendRoomSetupChange(this.CurrentRoom);
            }
            if (this.GameOverCountTeam != null)
                this.GameOverCountTeam(game, isWin, gainXp, playerCount);
            if (this.GameOver == null)
                return;
            this.GameOver(game, isWin, gainXp, isSpanArea, isCouple);
        }

        public void OnFightOneBloodIsWin(eRoomType roomType, bool isWin)
        {
            if (this.FightOneBloodIsWin == null)
                return;
            this.FightOneBloodIsWin(roomType, isWin);
        }

        public void OnGameOver2v2(bool isWin)
        {
            if (this.GameOver2v2 == null)
                return;
            this.GameOver2v2(isWin);
        }

        public void OnAcademyEvent(GamePlayer friendly, int type)
        {
            if (this.AcademyEvent == null)
                return;
            this.AcademyEvent(friendly, type);
        }

        public void UpdateRestCount()
        {
            throw new NotImplementedException();
        }

        public int AddActiveMoney(int value)
        {
            throw new NotImplementedException();
        }

        public delegate void PlayerOwnSpaEventHandle(int onlineTimeSpa);

        //public delegate void PlayerUserToemGemstoneEventHandle(int type);

        public delegate void PlayerAddItemEventHandel(string type, int value);

        public delegate void GameKillDropEventHandel(
          AbstractGame game,
          int type,
          int npcId,
          bool playResult);

        public delegate void PlayerAchievementFinish(AchievementData info);

        public delegate void PlayerAdoptPetEventHandle();

        public delegate void PlayerCropPrimaryEventHandle();

        public delegate void PlayerEnterHotSpring(GamePlayer player);

        public delegate void PlayerEventHandle(GamePlayer player);

        public delegate void PlayerFightAddOffer(int offer);

        public delegate void PlayerFightOneBloodIsWin(eRoomType roomType, bool isWin);

        public delegate void PlayerGameKillBossEventHandel(AbstractGame game, NpcInfo npc, int damage);

        public delegate void PlayerGameKillEventHandel(
          AbstractGame game,
          int type,
          int id,
          bool isLiving,
          int demage,
          bool isSpanArea);

        public delegate void PlayerGoldCollection(int value);

        public delegate void PlayerGiftTokenCollection(int value);

        public delegate void PlayerHotSpingExpAdd(int minutes, int exp);

        public delegate void PlayerLoginEventHandle();

        public delegate void PlayerItemComposeEventHandle(int composeType);

        public delegate void PlayerMoneyChargeHandle(int money);

        public delegate void PlayerItemFusionEventHandle(int fusionType);

        public delegate void PlayerItemInsertEventHandle();

        public delegate void PlayerItemMeltEventHandle(int categoryID);

        public delegate void PlayerItemPropertyEventHandle(int templateID, int count);

        public delegate void EnterSpa();

        public delegate void PlayerItemStrengthenEventHandle(int categoryID, int level);


        public delegate void PlayerMissionFullOverEventHandle(
          AbstractGame game,
          int missionId,
          bool isWin,
          int turnNum);

        public delegate void PlayerMissionOverEventHandle(AbstractGame game, int missionId, bool isWin);

        public delegate void PlayerMissionTurnOverEventHandle(
          AbstractGame game,
          int missionId,
          int turnNum);

        public delegate void PlayerNewGearEventHandle(SqlDataProvider.Data.ItemInfo item);

        public delegate void PlayerNewGearEventHandle2(SqlDataProvider.Data.ItemInfo item);

        public delegate void PlayerOwnConsortiaEventHandle();

        public delegate void PlayerAchievementQuestHandle();

        public delegate void PlayerPropertisChange(PlayerInfo player);

        public delegate void PlayerSeedFoodPetEventHandle();

        public delegate void PlayerShopEventHandle(
          int money,
          int gold,
          int offer,
          int gifttoken,
          int medal,
          string payGoods);

        public delegate void PlayerUnknowQuestConditionEventHandle();

        public delegate void PlayerUpLevelPetEventHandle();

        public delegate void PlayerUseBugle(int value);

        public delegate void PlayerUserToemGemstoneEventHandle();

        public delegate void PlayerVIPUpgrade(int level, int exp);

        public delegate void PlayerQuestFinishEventHandel(BaseQuest baseQuest);

        public delegate void PlayerPropertyChangedEventHandel(PlayerInfo character);

        public delegate void PlayerMarryTeamEventHandle(
          AbstractGame game,
          bool isWin,
          int gainXp,
          int countPlayersTeam);

        public delegate void PlayerGameOverCountTeamEventHandle(
          AbstractGame game,
          bool isWin,
          int gainXp,
          int countPlayersTeam);

        public delegate void PlayerMarryEventHandel();

        public delegate void PlayerDispatchesEventHandel();

        public delegate void PlayerGameOverEventHandle(
          AbstractGame game,
          bool isWin,
          int gainXp,
          bool isSpanArea,
          bool isCouple);

        public delegate void PlayerGameOverEvent2v2Handle(bool isWin);

        public delegate void PlayerAcademyEventHandle(GamePlayer friendly, int type);

        public delegate void SendGiftMail(int templateID, int count);
    }
}
