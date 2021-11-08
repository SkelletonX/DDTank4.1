// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.PlayerInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;
using System.Collections.Generic;

namespace SqlDataProvider.Data
{
    public class PlayerInfo : DataObject
    {
        private bool _isLocked = true;
        private Dictionary<string, object> _tempInfo = new Dictionary<string, object>();
        private int _agility;
        private int _antiAddiction;
        private DateTime _antiDate;
        private string string_16;
        private int _attack;
        private int _badLuckNumber;
        private bool _canTakeVipReward;
        private string _colors;
        private int _consortiaID;
        private string _consortiaName;
        private bool _consortiaRename;
        private string _checkCode;
        private int _checkCount;
        private DateTime _checkDate;
        private int _checkError;
        private int _dayLoginCount;
        private int _defence;
        private int _escape;
        private DateTime? _expendDate;
        private int _fightPower;
        private int _gold;
        private int _gp;
        private int _grade;
        private int _giftGp;
        private int _giftLevel;
        private int _GiftToken;
        private int _hide;
        private PlayerInfoHistory _history;
        private string _honor;
        private int _hp;
        private int _id;
        private int _inviter;
        private bool _isConsortia;
        private bool _isCreatedMarryRoom;
        private int _IsFirst;
        private bool _isGotRing;
        private bool _isMarried;
        private bool _isOldPlayer;
        private bool _isOldPlayerHasValidEquitAtLogin;
        private bool _isShowConsortia;
        private DateTime _LastAuncherAward;
        private DateTime _LastAward;
        private int _lastLuckNum;
        private DateTime _lastLuckyNumDate;
        private DateTime _LastVIPPackTime;
        private DateTime _LastWeekly;
        private int _LastWeeklyVersion;
        private int _luck;
        private int _luckyNum;
        private int _marryInfoID;
        private int _medal;
        private int _money;
        private int _MoneyPlus;
        private string _nickName;
        private int _nimbus;
        private int _offer;
        private int _optionOnOff;
        private string _password;
        private string _PasswordTwo;
        private int _petScore;
        private byte[] _QuestSite;
        private bool _rename;
        private int _repute;
        private int _richesOffer;
        private int _richesRob;
        private int _score;
        private int _selfMarryRoomID;
        private bool _sex;
        private string _skin;
        private int _spouseID;
        private string _spouseName;
        private int _state;
        private string _style;
        private TexpInfo _texp;
        private DateTime _newDay;
        private int _total;
        private byte _typeVIP;
        private string _userName;
        private int _VIPExp;
        private DateTime _VIPExpireDay;
        private int _VIPLevel;
        private int _VIPNextLevelDaysNeeded;
        private int _VIPOfflineDays;
        private int _VIPOnlineDays;
        private byte[] _weaklessGuildProgress;
        private string _weaklessGuildProgressStr;
        private int _win;
        public int AgiAddPlus;
        public int AttackAddPlus;
        public int DameAddPlus;
        public int DefendAddPlus;
        public int GuardAddPlus;
        public int HpAddPlus;
        public int LuckAddPlus;
        private int m_AchievementPoint;
        private int m_AddDayAchievementPoint;
        private DateTime m_AddGPLastDate;
        private int m_AddWeekAchievementPoint;
        private int m_AlreadyGetBox;
        private int m_AnswerSite;
        private int m_BanChat;
        private DateTime m_BanChatEndDate;
        private int m_bossguildid;
        private DateTime m_BoxGetDate;
        private int m_BoxProgression;
        private int m_ChatCount;
        private int m_FailedPasswordAttemptCount;
        private string m_fightlabPermission;
        private int m_gameActiveHide;
        private string m_gameActiveStyle;
        private int m_getBoxLevel;
        private bool m_IsInSpaPubGoldToday;
        private bool m_IsInSpaPubMoneyToday;
        private bool m_IsOpenGift;
        private DateTime m_lastDate;
        private DateTime m_lastDateSecond;
        private DateTime m_LastSpaDate;
        private int m_GetBoxLevel;
        private bool m_IsRecharged;
        private bool m_IsGetAward;
        private int m_OnlineTime;
        private string m_PasswordQuest1;
        private string m_PasswordQuest2;
        private string m_pvePermission;
        private string m_Rank;
        private int m_SpaPubGoldRoomLimit;
        private int m_SpaPubMoneyRoomLimit;
        private DateTime m_VIPlastDate;
        public int ReduceDamePlus;
        public int StrengthEnchance;
        public int _AddWeekLeagueScore;
        public int _WeekLeagueRanking;
        public int m_apprenticeshipState;
        public int m_masterID;
        public string m_masterOrApprentices;
        public int m_graduatesCount;
        public string m_honourOfMaster;
        public DateTime m_freezesDate;
        private int int_15;
        private int int_65;
        private int int_62;
        private int int_63;
        private int int_64;
        private int int_55;
        private int int_56;
        private int m_moneyLock;
        private DateTime dateTime_6;

        public bool bit(int param1)
        {
            --param1;
            return ((uint)this._weaklessGuildProgress[param1 / 8] & (uint)(1 << param1 % 8)) > 0U;
        }

        public void ClearConsortia()
        {
            this.ConsortiaID = 0;
            this.ConsortiaName = "";
            this.RichesOffer = 0;
            this.ConsortiaRepute = 0;
            this.ConsortiaLevel = 0;
            this.StoreLevel = 0;
            this.ShopLevel = 0;
            this.SmithLevel = 0;
            this.ConsortiaHonor = 0;
            this.RichesOffer = 0;
            this.RichesRob = 0;
            this.DutyLevel = 0;
            this.DutyName = "";
            this.Right = 0;
            this.AddDayGP = 0;
            this.AddWeekGP = 0;
            this.AddDayOffer = 0;
            this.AddWeekOffer = 0;
            this.ConsortiaRiches = 0;
        }

        public void CheckLevelFunction()
        {
            int grade = this.Grade;
            if (grade > 1)
            {
                this.openFunction(Step.GAME_ROOM_OPEN);
                this.openFunction(Step.CHANNEL_OPEN);
            }
            if (grade > 2)
            {
                this.openFunction(Step.SHOP_OPEN);
                this.openFunction(Step.STORE_OPEN);
                this.openFunction(Step.BAG_OPEN);
                this.openFunction(Step.MAIL_OPEN);
                this.openFunction(Step.SIGN_OPEN);
            }
            if (grade > 3)
                this.openFunction(Step.HP_PROP_OPEN);
            if (grade > 4)
            {
                this.openFunction(Step.GAME_ROOM_SHOW_OPEN);
                this.openFunction(Step.CIVIL_OPEN);
                this.openFunction(Step.IM_OPEN);
                this.openFunction(Step.GUILD_PROP_OPEN);
            }
            if (grade > 5)
            {
                this.openFunction(Step.BEAT_ROBOT);
                this.openFunction(Step.MASTER_ROOM_OPEN);
                this.openFunction(Step.POP_ANGLE);
            }
            if (grade > 6)
            {
                this.openFunction(Step.MASTER_ROOM_SHOW);
                this.openFunction(Step.CONSORTIA_OPEN);
                this.openFunction(Step.HIDE_PROP_OPEN);
                this.openFunction(Step.PLANE_PROP_OPEN);
            }
            if (grade > 7)
            {
                this.openFunction(Step.CONSORTIA_SHOW);
                this.openFunction(Step.DUNGEON_OPEN);
                this.openFunction(Step.FROZE_PROP_OPEN);
            }
            if (grade > 8)
            {
                this.openFunction(Step.DUNGEON_SHOW);
                this.openFunction(Step.BEAT_LIVING_LEFT);
            }
            if (grade > 9)
                this.openFunction(Step.CHURCH_OPEN);
            if (grade > 11)
            {
                this.openFunction(Step.CHURCH_SHOW);
                this.openFunction(Step.TOFF_LIST_OPEN);
            }
            if (grade > 12)
            {
                this.openFunction(Step.TOFF_LIST_SHOW);
                this.openFunction(Step.HOT_WELL_OPEN);
            }
            if (grade > 13)
            {
                this.openFunction(Step.HOT_WELL_SHOW);
                this.openFunction(Step.AUCTION_OPEN);
            }
            if (grade <= 14)
                return;
            this.openFunction(Step.AUCTION_SHOW);
            this.openFunction(Step.CAMPAIGN_LAB_OPEN);
        }

        public bool IsLastVIPPackTime()
        {
            return this._LastVIPPackTime.AddDays(7).Date < DateTime.Now.Date;
        }

        public bool CheckNewDay()
        {
            return this._newDay.Date < DateTime.Now.Date;
        }

        public bool IsVIPExpire()
        {
            return this._VIPExpireDay.Date < DateTime.Now.Date;
        }

        public bool IsWeakGuildFinish(int id)
        {
            if (id >= 1)
                return this.bit(id);
            return false;
        }


        public string fightLibMission
        {
            get
            {
                return string_16;
            }
            set
            {
                string_16 = value;
                _isDirty = true;
            }
        }

        public void openFunction(Step step)
        {
            int num1 = (int)(step - 1);
            int index = num1 / 8;
            int num2 = num1 % 8;
            byte[] weaklessGuildProgress = this.weaklessGuildProgress;
            if (weaklessGuildProgress.Length == 0)
                return;
            weaklessGuildProgress[index] = (byte)((uint)weaklessGuildProgress[index] | (uint)(1 << num2));
            this.weaklessGuildProgress = weaklessGuildProgress;
        }

        public int GetVIPNextLevelDaysNeeded(int viplevel, int vipexp)
        {
            if (viplevel == 0 || vipexp <= 0 || viplevel > 8)
                return 0;
            int exp = 10;
            if (this.typeVIP > 2)
            {
                exp = 15;
            }
            return (Array.ConvertAll<string, int>("0,100,250,550,1250,2200,3100,4100,5650".Split(','), new Converter<string, int>(int.Parse))[viplevel] - vipexp) / exp;
        }

        public int _badgeID { get; set; }

        public int AchievementPoint
        {
            get
            {
                return this.m_AchievementPoint;
            }
            set
            {
                this.m_AchievementPoint = value;
            }
        }

        public int AddDayAchievementPoint
        {
            get
            {
                return this.m_AddDayAchievementPoint;
            }
            set
            {
                this.m_AddDayAchievementPoint = value;
            }
        }

        public int AddDayGP { get; set; }

        public int AddDayGiftGp { get; set; }

        public int AddDayOffer { get; set; }

        public DateTime AddGPLastDate
        {
            get
            {
                return this.m_AddGPLastDate;
            }
            set
            {
                this.m_AddGPLastDate = value;
            }
        }

        public int AddWeekAchievementPoint
        {
            get
            {
                return this.m_AddWeekAchievementPoint;
            }
            set
            {
                this.m_AddWeekAchievementPoint = value;
            }
        }

        public int AddWeekGP { get; set; }

        public int AddWeekGiftGp { get; set; }

        public int AddWeekOffer { get; set; }

        public int Agility
        {
            get
            {
                return this._agility;
            }
            set
            {
                this._agility = value;
                this._isDirty = true;
            }
        }

        public int AlreadyGetBox
        {
            get
            {
                return this.m_AlreadyGetBox;
            }
            set
            {
                this.m_AlreadyGetBox = value;
            }
        }

        public int AnswerSite
        {
            get
            {
                return this.m_AnswerSite;
            }
            set
            {
                this.m_AnswerSite = value;
            }
        }

        public int AntiAddiction
        {
            get
            {
                return this._antiAddiction + (int)(DateTime.Now - this._antiDate).TotalMinutes;
            }
            set
            {
                this._antiAddiction = value;
                this._antiDate = DateTime.Now;
            }
        }

        public DateTime AntiDate
        {
            get
            {
                return this._antiDate;
            }
            set
            {
                this._antiDate = value;
            }
        }

        public int Attack
        {
            get
            {
                return this._attack;
            }
            set
            {
                this._attack = value;
                this._isDirty = true;
            }
        }

        public int badgeID
        {
            get
            {
                return this._badgeID;
            }
            set
            {
                this._badgeID = value;
                this._isDirty = true;
            }
        }

        public int badLuckNumber
        {
            get
            {
                return this._badLuckNumber;
            }
            set
            {
                this._badLuckNumber = value;
            }
        }

        public int BanChat
        {
            get
            {
                return this.m_BanChat;
            }
            set
            {
                this.m_BanChat = value;
            }
        }

        public DateTime BanChatEndDate
        {
            get
            {
                return this.m_BanChatEndDate;
            }
            set
            {
                this.m_BanChatEndDate = value;
            }
        }

        public int BossGuildID
        {
            get
            {
                return this.m_bossguildid;
            }
            set
            {
                this.m_bossguildid = value;
            }
        }

        public DateTime BoxGetDate
        {
            get
            {
                return this.m_BoxGetDate;
            }
            set
            {
                this.m_BoxGetDate = value;
            }
        }

        public bool CanTakeVipReward
        {
            get
            {
                return this._canTakeVipReward;
            }
            set
            {
                this._canTakeVipReward = value;
                this._isDirty = true;
            }
        }

        public string Colors
        {
            get
            {
                return this._colors;
            }
            set
            {
                this._colors = value;
                this._isDirty = true;
            }
        }

        public int ConsortiaGiftGp { get; set; }

        public int ConsortiaHonor { get; set; }

        public int ConsortiaID
        {
            get
            {
                return this._consortiaID;
            }
            set
            {
                if (this._consortiaID == 0 || value == 0)
                {
                    this._richesRob = 0;
                    this._richesOffer = 0;
                }
                this._consortiaID = value;
            }
        }

        public int ConsortiaLevel { get; set; }

        public string ConsortiaName
        {
            get
            {
                return this._consortiaName;
            }
            set
            {
                this._consortiaName = value;
            }
        }

        public bool ConsortiaRename
        {
            get
            {
                return this._consortiaRename;
            }
            set
            {
                if (this._consortiaRename == value)
                    return;
                this._consortiaRename = value;
                this._isDirty = true;
            }
        }

        public int ConsortiaRepute { get; set; }

        public int ConsortiaRiches { get; set; }

        public string ChairmanName { get; set; }

        public int ChatCount
        {
            get
            {
                return this.m_ChatCount;
            }
            set
            {
                this.m_ChatCount = value;
            }
        }

        public string CheckCode
        {
            get
            {
                return this._checkCode;
            }
            set
            {
                this._checkDate = DateTime.Now;
                this._checkCode = value;
                string.IsNullOrEmpty(this._checkCode);
            }
        }

        public int CheckCount
        {
            get
            {
                return this._checkCount;
            }
            set
            {
                this._checkCount = value;
                this._isDirty = true;
            }
        }

        public DateTime CheckDate
        {
            get
            {
                return this._checkDate;
            }
        }

        public int CheckError
        {
            get
            {
                return this._checkError;
            }
            set
            {
                this._checkError = value;
            }
        }

        public int DayLoginCount
        {
            get
            {
                return this._dayLoginCount;
            }
            set
            {
                this._dayLoginCount = value;
                this._isDirty = true;
            }
        }

        public int Defence
        {
            get
            {
                return this._defence;
            }
            set
            {
                this._defence = value;
                this._isDirty = true;
            }
        }

        public int DutyLevel { get; set; }

        public string DutyName { get; set; }

        public int Escape
        {
            get
            {
                return this._escape;
            }
            set
            {
                this._escape = value;
                this._isDirty = true;
            }
        }

        public DateTime? ExpendDate
        {
            get
            {
                return this._expendDate;
            }
            set
            {
                this._expendDate = value;
                this._isDirty = true;
            }
        }

        public int FailedPasswordAttemptCount
        {
            get
            {
                return this.m_FailedPasswordAttemptCount;
            }
            set
            {
                this.m_FailedPasswordAttemptCount = value;
            }
        }

        public string FightLabPermission
        {
            get
            {
                return this.m_fightlabPermission;
            }
            set
            {
                this.m_fightlabPermission = value;
            }
        }

        public int FightPower
        {
            get
            {
                return this._fightPower;
            }
            set
            {
                if (this._fightPower == value)
                    return;
                this._fightPower = value;
                this._isDirty = true;
            }
        }

        public int GameActiveHide
        {
            get
            {
                return this.m_gameActiveHide;
            }
            set
            {
                this.m_gameActiveHide = value;
            }
        }

        public string GameActiveStyle
        {
            get
            {
                return this.m_gameActiveStyle;
            }
            set
            {
                this.m_gameActiveStyle = value;
            }
        }

        public int Gold
        {
            get
            {
                return this._gold;
            }
            set
            {
                this._gold = value;
                this._isDirty = true;
            }
        }

        public int GP
        {
            get
            {
                return this._gp;
            }
            set
            {
                this._gp = value;
                this._isDirty = true;
            }
        }

        public int Grade
        {
            get
            {
                return this._grade;
            }
            set
            {
                this._grade = value;
                this._isDirty = true;
            }
        }

        public int GiftGp
        {
            get
            {
                return this._giftGp;
            }
            set
            {
                this._giftGp = value;
                this._isDirty = true;
            }
        }

        public int GiftLevel
        {
            get
            {
                return this._giftLevel;
            }
            set
            {
                this._giftLevel = value;
                this._isDirty = true;
            }
        }

        public int GiftToken
        {
            get
            {
                return this._GiftToken;
            }
            set
            {
                this._GiftToken = value;
            }
        }

        public bool HasBagPassword
        {
            get
            {
                return !string.IsNullOrEmpty(this._PasswordTwo);
            }
        }

        public int Hide
        {
            get
            {
                return this._hide;
            }
            set
            {
                this._hide = value;
                this._isDirty = true;
            }
        }

        public PlayerInfoHistory History
        {
            get
            {
                return this._history;
            }
            set
            {
                this._history = value;
            }
        }

        public string Honor
        {
            get
            {
                return this._honor;
            }
            set
            {
                this._honor = value;
            }
        }

        public int hp
        {
            get
            {
                return this._hp;
            }
            set
            {
                this._hp = value;
                this._isDirty = true;
            }
        }

        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
                this._isDirty = true;
            }
        }

        public int Inviter
        {
            get
            {
                return this._inviter;
            }
            set
            {
                this._inviter = value;
            }
        }

        public bool IsAutoBot { get; set; }

        public bool IsBanChat { get; set; }

        public bool IsConsortia
        {
            get
            {
                return this._isConsortia;
            }
            set
            {
                this._isConsortia = value;
            }
        }

        public bool IsCreatedMarryRoom
        {
            get
            {
                return this._isCreatedMarryRoom;
            }
            set
            {
                if (this._isCreatedMarryRoom == value)
                    return;
                this._isCreatedMarryRoom = value;
                this._isDirty = true;
            }
        }

        public int IsFirst
        {
            get
            {
                return this._IsFirst;
            }
            set
            {
                this._IsFirst = value;
            }
        }

        public bool IsGotRing
        {
            get
            {
                return this._isGotRing;
            }
            set
            {
                if (this._isGotRing == value)
                    return;
                this._isGotRing = value;
                this._isDirty = true;
            }
        }

        public bool IsInSpaPubGoldToday
        {
            get
            {
                return this.m_IsInSpaPubGoldToday;
            }
            set
            {
                this.m_IsInSpaPubGoldToday = value;
            }
        }

        public bool IsInSpaPubMoneyToday
        {
            get
            {
                return this.m_IsInSpaPubMoneyToday;
            }
            set
            {
                this.m_IsInSpaPubMoneyToday = value;
            }
        }

        public bool IsLocked
        {
            get
            {
                return this._isLocked;
            }
            set
            {
                this._isLocked = value;
            }
        }

        public bool IsMarried
        {
            get
            {
                return this._isMarried;
            }
            set
            {
                this._isMarried = value;
                this._isDirty = true;
            }
        }

        public bool IsOldPlayer
        {
            get
            {
                return this._isOldPlayer;
            }
            set
            {
                this._isOldPlayer = value;
                this._isDirty = true;
            }
        }

        public bool isOldPlayerHasValidEquitAtLogin
        {
            get
            {
                return this._isOldPlayerHasValidEquitAtLogin;
            }
            set
            {
                this._isOldPlayerHasValidEquitAtLogin = value;
            }
        }

        public bool IsOpenGift
        {
            get
            {
                return this.m_IsOpenGift;
            }
            set
            {
                this.m_IsOpenGift = value;
            }
        }

        public bool IsShowConsortia
        {
            get
            {
                return this._isShowConsortia;
            }
            set
            {
                this._isShowConsortia = value;
            }
        }

        public DateTime LastAuncherAward
        {
            get
            {
                return this._LastAuncherAward;
            }
            set
            {
                this._LastAuncherAward = value;
            }
        }

        public DateTime LastAward
        {
            get
            {
                return this._LastAward;
            }
            set
            {
                this._LastAward = value;
            }
        }

        public DateTime LastDate
        {
            get
            {
                return this.m_lastDate;
            }
            set
            {
                this.m_lastDate = value;
            }
        }

        public DateTime LastDateSecond
        {
            get
            {
                return this.m_lastDateSecond;
            }
            set
            {
                this.m_lastDateSecond = value;
            }
        }

        public int lastLuckNum
        {
            get
            {
                return this._lastLuckNum;
            }
            set
            {
                this._lastLuckNum = value;
            }
        }

        public DateTime lastLuckyNumDate
        {
            get
            {
                return this._lastLuckyNumDate;
            }
            set
            {
                this._lastLuckyNumDate = value;
            }
        }

        public DateTime LastSpaDate
        {
            get
            {
                return this.m_LastSpaDate;
            }
            set
            {
                this.m_LastSpaDate = value;
            }
        }

        public DateTime LastVIPPackTime
        {
            get
            {
                return this._LastVIPPackTime;
            }
            set
            {
                this._LastVIPPackTime = value;
                this._isDirty = true;
            }
        }

        public DateTime LastWeekly
        {
            get
            {
                return this._LastWeekly;
            }
            set
            {
                this._LastWeekly = value;
            }
        }

        public int LastWeeklyVersion
        {
            get
            {
                return this._LastWeeklyVersion;
            }
            set
            {
                this._LastWeeklyVersion = value;
            }
        }

        public int Luck
        {
            get
            {
                return this._luck;
            }
            set
            {
                this._luck = value;
                this._isDirty = true;
            }
        }

        public int luckyNum
        {
            get
            {
                return this._luckyNum;
            }
            set
            {
                this._luckyNum = value;
            }
        }

        public int MarryInfoID
        {
            get
            {
                return this._marryInfoID;
            }
            set
            {
                if (this._marryInfoID == value)
                    return;
                this._marryInfoID = value;
                this._isDirty = true;
            }
        }

        public int medal
        {
            get
            {
                return this._medal;
            }
            set
            {
                this._medal = value;
                this._isDirty = true;
            }
        }

        public int Money
        {
            get
            {
                return this._money;
            }
            set
            {
                this._money = value;
                this._isDirty = true;
            }
        }

        public int MoneyPlus
        {
            get
            {
                return this._MoneyPlus;
            }
            set
            {
                this._MoneyPlus = value;
            }
        }

        public string NickName
        {
            get
            {
                return this._nickName;
            }
            set
            {
                this._nickName = value;
                this._isDirty = true;
            }
        }

        public int Nimbus
        {
            get
            {
                return this._nimbus;
            }
            set
            {
                if (this._nimbus == value)
                    return;
                this._nimbus = value;
                this._isDirty = true;
            }
        }

        public int Offer
        {
            get
            {
                return this._offer;
            }
            set
            {
                this._offer = value;
                this._isDirty = true;
            }
        }

        public int OnlineTime
        {
            get
            {
                return this.m_OnlineTime;
            }
            set
            {
                this.m_OnlineTime = value;
            }
        }

        public int OptionOnOff
        {
            get
            {
                return this._optionOnOff;
            }
            set
            {
                this._optionOnOff = value;
            }
        }

        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
                this._isDirty = true;
            }
        }

        public string PasswordQuest1
        {
            get
            {
                return this.m_PasswordQuest1;
            }
            set
            {
                this.m_PasswordQuest1 = value;
            }
        }

        public string PasswordQuest2
        {
            get
            {
                return this.m_PasswordQuest2;
            }
            set
            {
                this.m_PasswordQuest2 = value;
            }
        }

        public string PasswordTwo
        {
            get
            {
                return this._PasswordTwo;
            }
            set
            {
                this._PasswordTwo = value;
                this._isDirty = true;
            }
        }

        public int petScore
        {
            get
            {
                return this._petScore;
            }
            set
            {
                this._petScore = value;
            }
        }

        public string PvePermission
        {
            get
            {
                return this.m_pvePermission;
            }
            set
            {
                this.m_pvePermission = value;
            }
        }

        public byte[] QuestSite
        {
            get
            {
                return this._QuestSite;
            }
            set
            {
                this._QuestSite = value;
            }
        }

        public string Rank
        {
            get
            {
                return this.m_Rank;
            }
            set
            {
                this.m_Rank = value;
            }
        }

        public int BoxProgression
        {
            get
            {
                return this.m_BoxProgression;
            }
            set
            {
                this.m_BoxProgression = value;
            }
        }

        public int GetBoxLevel
        {
            get
            {
                return this.m_GetBoxLevel;
            }
            set
            {
                this.m_GetBoxLevel = value;
            }
        }

        public bool IsRecharged
        {
            get
            {
                return this.m_IsRecharged;
            }
            set
            {
                this.m_IsRecharged = value;
            }
        }

        public bool IsGetAward
        {
            get
            {
                return this.m_IsGetAward;
            }
            set
            {
                this.m_IsGetAward = value;
            }
        }

        public bool Rename
        {
            get
            {
                return this._rename;
            }
            set
            {
                if (this._rename == value)
                    return;
                this._rename = value;
                this._isDirty = true;
            }
        }

        public int Repute
        {
            get
            {
                return this._repute;
            }
            set
            {
                this._repute = value;
                this._isDirty = true;
            }
        }

        public int ReputeOffer { get; set; }

        public int Riches
        {
            get
            {
                return this.RichesRob + this.RichesOffer;
            }
        }

        public int RichesOffer
        {
            get
            {
                return this._richesOffer;
            }
            set
            {
                this._richesOffer = value;
                this._isDirty = true;
            }
        }

        public int RichesRob
        {
            get
            {
                return this._richesRob;
            }
            set
            {
                this._richesRob = value;
                this._isDirty = true;
            }
        }

        public int Right { get; set; }

        public int RoomId { get; set; }

        public int Score
        {
            get
            {
                return this._score;
            }
            set
            {
                this._score = value;
            }
        }

        public int SelfMarryRoomID
        {
            get
            {
                return this._selfMarryRoomID;
            }
            set
            {
                if (this._selfMarryRoomID == value)
                    return;
                this._selfMarryRoomID = value;
                this._isDirty = true;
            }
        }

        public bool Sex
        {
            get
            {
                return this._sex;
            }
            set
            {
                this._sex = value;
                this._isDirty = true;
            }
        }

        public int ShopLevel { get; set; }

        public int SkillLevel { get; set; }

        public string Skin
        {
            get
            {
                return this._skin;
            }
            set
            {
                this._skin = value;
                this._isDirty = true;
            }
        }

        public int SmithLevel { get; set; }

        public int SpaPubGoldRoomLimit
        {
            get
            {
                return this.m_SpaPubGoldRoomLimit;
            }
            set
            {
                this.m_SpaPubGoldRoomLimit = value;
            }
        }

        public int SpaPubMoneyRoomLimit
        {
            get
            {
                return this.m_SpaPubMoneyRoomLimit;
            }
            set
            {
                this.m_SpaPubMoneyRoomLimit = value;
            }
        }

        public int MoneyLock
        {
            get
            {
                return this.m_moneyLock;
            }
            set
            {
                this.m_moneyLock = value;
                this._isDirty = true;
            }
        }

        public int SpouseID
        {
            get
            {
                return this._spouseID;
            }
            set
            {
                if (this._spouseID == value)
                    return;
                this._spouseID = value;
                this._isDirty = true;
            }
        }

        public string SpouseName
        {
            get
            {
                return this._spouseName;
            }
            set
            {
                if (!(this._spouseName != value))
                    return;
                this._spouseName = value;
                this._isDirty = true;
            }
        }

        public int State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
                this._isDirty = true;
            }
        }

        public int StoreLevel { get; set; }

        public string Style
        {
            get
            {
                return this._style;
            }
            set
            {
                this._style = value;
                this._isDirty = true;
            }
        }

        public Dictionary<string, object> TempInfo
        {
            get
            {
                return this._tempInfo;
            }
        }

        public TexpInfo Texp
        {
            get
            {
                return this._texp;
            }
            set
            {
                this._texp = value;
                this._isDirty = true;
            }
        }

        public DateTime NewDay
        {
            get
            {
                return this._newDay;
            }
            set
            {
                this._newDay = value;
            }
        }

        public int Total
        {
            get
            {
                return this._total;
            }
            set
            {
                this._total = value;
                this._isDirty = true;
            }
        }

        private int int_100;

        public int totemId
        {
            get
            {
                return this.int_100;
            }
            set
            {
                this.int_100 = value;
                if (this.int_100 <= 10000)
                {
                    this.int_100 = 0;
                }
            }
        }

        private int int_103;
        public int myHonor
        {
            get
            {
                return this.int_103;
            }
            set
            {
                this.int_103 = value;
            }
        }

        private int int_53;

        public int MaxBuyHonor
        {
            get
            {
                return this.int_53;
            }
            set
            {
                this.int_53 = value;
                this._isDirty = true;
            }
        }

        public byte typeVIP
        {
            get
            {
                return this._typeVIP;
            }
            set
            {
                if ((int)this._typeVIP == (int)value)
                    return;
                this._typeVIP = value;
                this._isDirty = true;
            }
        }

        public string UserName
        {
            get
            {
                return this._userName;
            }
            set
            {
                this._userName = value;
                this._isDirty = true;
            }
        }

        public int VIPExp
        {
            get
            {
                return this._VIPExp;
            }
            set
            {
                if (this._VIPExp == value)
                    return;
                this._VIPExp = value;
                this._isDirty = true;
            }
        }

        public DateTime VIPExpireDay
        {
            get
            {
                return this._VIPExpireDay;
            }
            set
            {
                this._VIPExpireDay = value;
                this._isDirty = true;
            }
        }

        public DateTime VIPLastDate
        {
            get
            {
                return this.m_VIPlastDate;
            }
            set
            {
                this.m_VIPlastDate = value;
            }
        }

        public int VIPLevel
        {
            get
            {
                return this._VIPLevel;
            }
            set
            {
                if (this._VIPLevel == value)
                    return;
                this._VIPLevel = value;
                this._isDirty = true;
            }
        }

        public int VIPNextLevelDaysNeeded
        {
            get
            {
                return this._VIPNextLevelDaysNeeded;
            }
            set
            {
                this._VIPNextLevelDaysNeeded = value;
                this._isDirty = true;
            }
        }

        public int VIPOfflineDays
        {
            get
            {
                return this._VIPOfflineDays;
            }
            set
            {
                this._VIPOfflineDays = value;
            }
        }

        public int VIPOnlineDays
        {
            get
            {
                return this._VIPOnlineDays;
            }
            set
            {
                this._VIPOnlineDays = value;
            }
        }

        public byte[] weaklessGuildProgress
        {
            get
            {
                return this._weaklessGuildProgress;
            }
            set
            {
                this._weaklessGuildProgress = value;
                this._isDirty = true;
            }
        }

        public string WeaklessGuildProgressStr
        {
            get
            {
                return this._weaklessGuildProgressStr;
            }
            set
            {
                this._weaklessGuildProgressStr = value;
                this._isDirty = true;
            }
        }

        public int Win
        {
            get
            {
                return this._win;
            }
            set
            {
                this._win = value;
                this._isDirty = true;
            }
        }

        public int AddWeekLeagueScore
        {
            get
            {
                return this._AddWeekLeagueScore;
            }
            set
            {
                this._AddWeekLeagueScore = value;
                this._isDirty = true;
            }
        }

        public int WeekLeagueRanking
        {
            get
            {
                return this._WeekLeagueRanking;
            }
            set
            {
                this._WeekLeagueRanking = value;
                this._isDirty = true;
            }
        }

        public int apprenticeshipState
        {
            get
            {
                return this.m_apprenticeshipState;
            }
            set
            {
                this.m_apprenticeshipState = value;
                this._isDirty = true;
            }
        }

        public int masterID
        {
            get
            {
                return this.m_masterID;
            }
            set
            {
                this.m_masterID = value;
                this._isDirty = true;
            }
        }

        public string masterOrApprentices
        {
            get
            {
                return this.m_masterOrApprentices;
            }
            set
            {
                this.m_masterOrApprentices = value;
                this._isDirty = true;
                this.updateMasterOrApprenticesArr(value);
            }
        }

        public int graduatesCount
        {
            get
            {
                return this.m_graduatesCount;
            }
            set
            {
                this.m_graduatesCount = value;
                this._isDirty = true;
            }
        }

        public string honourOfMaster
        {
            get
            {
                return this.m_honourOfMaster;
            }
            set
            {
                this.m_honourOfMaster = value;
                this._isDirty = true;
            }
        }

        public DateTime freezesDate
        {
            get
            {
                return this.m_freezesDate;
            }
            set
            {
                this.m_freezesDate = value;
                this._isDirty = true;
            }
        }

        private Dictionary<int, string> _masterOrApprenticesArr { get; set; }

        public Dictionary<int, string> MasterOrApprenticesArr
        {
            get
            {
                return this._masterOrApprenticesArr;
            }
        }

        public void updateMasterOrApprenticesArr(string val)
        {
            if (this._masterOrApprenticesArr == null)
                this._masterOrApprenticesArr = new Dictionary<int, string>();
            lock (this._masterOrApprenticesArr)
            {
                this._masterOrApprenticesArr.Clear();
                if (val == null)
                    return;
                if (!(val != ""))
                    return;
                try
                {
                    string str1 = val;
                    char[] chArray1 = new char[1] { ',' };
                    foreach (string str2 in str1.Split(chArray1))
                    {
                        char[] chArray2 = new char[1] { '|' };
                        string[] strArray = str2.Split(chArray2);
                        this._masterOrApprenticesArr.Add(int.Parse(strArray[0]), strArray[1]);
                    }
                }
                catch
                {
                }
            }
        }

        public void ConvertMasterOrApprentices()
        {
            List<string> stringList = new List<string>();
            lock (this._masterOrApprenticesArr)
            {
                if (this._masterOrApprenticesArr.Count == 0)
                    this.apprenticeshipState = 0;
                else if (this._masterOrApprenticesArr.Count >= 3)
                    this.apprenticeshipState = 3;
                else if (this._masterOrApprenticesArr.Count > 0 && (uint)this.masterID > 0U)
                    this.apprenticeshipState = 1;
                else if (this._masterOrApprenticesArr.Count > 0 && this.masterID == 0)
                    this.apprenticeshipState = 2;
                foreach (KeyValuePair<int, string> keyValuePair in this._masterOrApprenticesArr)
                    stringList.Add(keyValuePair.Key.ToString() + "|" + keyValuePair.Value);
            }
            this.m_masterOrApprentices = string.Join(",", (IEnumerable<string>)stringList);
            this._isDirty = true;
        }

        public bool AddMasterOrApprentices(int id, string nickname)
        {
            bool flag = false;
            if (!this.MasterOrApprenticesArr.ContainsKey(id))
            {
                this.MasterOrApprenticesArr.Add(id, nickname);
                this.ConvertMasterOrApprentices();
                flag = true;
            }
            return flag;
        }

        public bool RemoveMasterOrApprentices(int id)
        {
            bool flag = false;
            if (this.MasterOrApprenticesArr.ContainsKey(id))
            {
                this.MasterOrApprenticesArr.Remove(id);
                this.ConvertMasterOrApprentices();
                flag = true;
            }
            return flag;
        }

        public int charmGP
        {
            get
            {
                return this.int_65;
            }
            set
            {
                this.int_65 = value;
                this._isDirty = true;
            }
        }

        public DateTime ShopFinallyGottenTime
        {
            get
            {
                return this.dateTime_6;
            }
            set
            {
                this.dateTime_6 = value;
                this._isDirty = true;
            }
        }

        public int evolutionGrade
        {
            get
            {
                return this.int_62;
            }
            set
            {
                this.int_62 = value;
            }
        }

        public int evolutionExp
        {
            get
            {
                return this.int_63;
            }
            set
            {
                this.int_63 = value;
            }
        }

        public int hardCurrency
        {
            get
            {
                return this.int_64;
            }
            set
            {
                this.int_64 = value;
            }
        }

        public int EliteScore
        {
            get
            {
                return this.int_55;
            }
            set
            {
                this.int_55 = value;
                this._isDirty = true;
            }
        }

        public int UseOffer
        {
            get
            {
                return this.int_15;
            }
            set
            {
                this.int_15 = value;
                this._isDirty = true;
            }
        }

        public int EliteRank
        {
            get
            {
                return this.int_56;
            }
            set
            {
                this.int_56 = value;
            }
        }
    }
}
