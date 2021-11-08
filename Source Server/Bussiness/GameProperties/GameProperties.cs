
using Game.Base.Config;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bussiness
{
    public abstract class GameProperties
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly int PRICE_COMPOSE_GOLD = 0;
        public static int SpaAddictionMoneyNeeded = 500;
        [ConfigProperty("AssState", "·À\x00B3ÁÃÔÏµÍ\x00B3µÄ¿ª\x00B9Ø,True´ò¿ª,False\x00B9Ø±Õ", false)]
        public static bool ASS_STATE;
        [ConfigProperty("BagMailEnable", "BagMailEnable", true)]
        public static readonly bool BagMailEnable;
        [ConfigProperty("BeginAuction", "ÅÄÂòÊ±ÆðÊ\x00BCËæ»úÊ±\x00BCä", 20)]
        public static int BeginAuction;
        [ConfigProperty("BigDicePrice", "BigDicePrice", 50000)]
        public static readonly int BigDicePrice;
        [ConfigProperty("BigExp", "µ±Ç°ÓÎÏ·°æ±\x00BE", "11906|99")]
        public static readonly string BigExp;
        [ConfigProperty("BoguAdventurePrice", "BoguAdventurePrice", "200, 500, 100")]
        public static readonly string BoguAdventurePrice;
        [ConfigProperty("BoxAppearCondition", "Ïä×ÓÎïÆ·ÌáÊ\x00BEµÄµÈ\x00BC¶", 4)]
        public static readonly int BOX_APPEAR_CONDITION;
        [ConfigProperty("Cess", "\x00BD»Ò×¿ÛË°", 0.1)]
        public static readonly double Cess;
        [ConfigProperty("CommonDicePrice", "CommonDicePrice", 30000)]
        public static readonly int CommonDicePrice;
        [ConfigProperty("ConsortiaStrengthenEx", "Kinh nghiệm", "1|2")]
        public static readonly string ConsortiaStrengthenEx;
        [ConfigProperty("CustomLimit", "sendattackmail|addaution|PresentGoods|PresentMoney|unknow", "20|20|20|20|20")]
        public static readonly string CustomLimit;
        [ConfigProperty("CheckCount", "×î´óÑéÖ¤ÂëÊ§°Ü´ÎÊý", 2)]
        public static readonly int CHECK_MAX_FAILED_COUNT;
        [ConfigProperty("CheckRewardItem", "ÑéÖ¤Âë\x00BD±ÀøÎïÆ·", 11001)]
        public static readonly int CHECK_REWARD_ITEM;
        [ConfigProperty("ChristmasBeginDate", "ChristmasBeginDate", "2013/12/17 0:00:00")]
        public static readonly string ChristmasBeginDate;
        [ConfigProperty("ChristmasBuildSnowmanDoubleMoney", "ChristmasBuildSnowmanDoubleMoney", 10)]
        public static readonly int ChristmasBuildSnowmanDoubleMoney;
        [ConfigProperty("ChristmasBuyMinute", "ChristmasBuyMinute", 10)]
        public static readonly int ChristmasBuyMinute;
        [ConfigProperty("ChristmasBuyTimeMoney", "ChristmasBuyTimeMoney", 150)]
        public static readonly int ChristmasBuyTimeMoney;
        [ConfigProperty("ChristmasEndDate", "ChristmasEndDate", "2013/12/25 0:00:00")]
        public static readonly string ChristmasEndDate;
        [ConfigProperty("ChristmasGifts", "ChristmasGifts", "201148,10|201149,35|201150,70|201151,120|201152,220|201153,370|201154,650|201155,1000|201156,100")]
        public static readonly string ChristmasGifts;
        [ConfigProperty("ChristmasGiftsMaxNum", "ChristmasGiftsMaxNum", 1000)]
        public static readonly int ChristmasGiftsMaxNum;
        [ConfigProperty("ChristmasMinute", "ChristmasMinute", 60)]
        public static readonly int ChristmasMinute;
        [ConfigProperty("RemoteEnable", "RemoteEnable", false)]
        public static readonly bool RemoteEnable;
        [ConfigProperty("DailyAwardState", "Ã¿ÈÕ\x00BD±Àø¿ª\x00B9Ø,True´ò¿ª,False\x00B9Ø±Õ", true)]
        public static bool DAILY_AWARD_STATE;
        [ConfigProperty("DDPlayActivityBeginDate", "DDPlayActivityBeginDate", "2014/07/01 0:00:00")]
        public static readonly string DDPlayActivityBeginDate;
        [ConfigProperty("DDPlayActivityEndDate", "DDPlayActivityEndDate", "2015/07/01 0:00:00")]
        public static readonly string DDPlayActivityEndDate;
        [ConfigProperty("DDPlayActivityMoney", "DDPlayActivityMoney", 100)]
        public static readonly int DDPlayActivityMoney;
        [ConfigProperty("AdvanceRateValue", "AdvanceRateValue", 2)]
        public static int AdvanceRateValue;
        [ConfigProperty("DiceBeginTime", "DiceBeginTime", "2013/12/17 0:00:00")]
        public static readonly string DiceBeginTime;
        [ConfigProperty("DiceEndTime", "DiceEndTime", "2013/12/25 0:00:00")]
        public static readonly string DiceEndTime;
        [ConfigProperty("DiceGameAwardAndCount", "DiceGameAwardAndCount", "32|16|8|4|2|1")]
        public static readonly string DiceGameAwardAndCount;
        [ConfigProperty("DiceRefreshPrice", "DiceRefreshPrice", 40000)]
        public static readonly int DiceRefreshPrice;
        [ConfigProperty("DisableCommands", "\x00BDûÖ\x00B9Ê\x00B9ÓÃµÄÃüÁî", "")]
        public static readonly string DISABLED_COMMANDS;
        [ConfigProperty("WarriorFamRaidPriceBig", "WarriorFamRaidPriceBig ", 40000)]
        public static readonly int WarriorFamRaidPriceBig;
        [ConfigProperty("WarriorFamRaidDDTPrice", "WarriorFamRaidDDTPrice", 5000)]
        public static readonly int WarriorFamRaidDDTPrice;
        [ConfigProperty("WarriorFamRaidTimeRemain", "WarriorFamRaidTimeRemain", 120)]
        public static readonly int WarriorFamRaidTimeRemain;
        [ConfigProperty("WarriorFamRaidPriceSmall", "WarriorFamRaidPriceSmall", 30000)]
        public static readonly int WarriorFamRaidPriceSmall;
        [ConfigProperty("WarriorFamRaidPricePerMin", "WarriorFamRaidPricePerMin", 10)]
        public static readonly int WarriorFamRaidPricePerMin;
        [ConfigProperty("DoubleDicePrice", "DoubleDicePrice", 40000)]
        public static readonly int DoubleDicePrice;
        [ConfigProperty("DragonBoatAreaMinScore", "DragonBoatAreaMinScore", 20000)]
        public static readonly int DragonBoatAreaMinScore;
        [ConfigProperty("DragonBoatBeginDate", "DragonBoatBeginDate", "2013/12/19 0:00:00")]
        public static readonly string DragonBoatBeginDate;
        [ConfigProperty("DragonBoatByMoney", "DragonBoatByMoney", "100:10,10")]
        public static readonly string DragonBoatByMoney;
        [ConfigProperty("DragonBoatByProps", "DragonBoatByProps", "1:10,10")]
        public static readonly string DragonBoatByProps;
        [ConfigProperty("DragonBoatConvertHours", "DragonBoatConvertHours", 72)]
        public static readonly int DragonBoatConvertHours;
        [ConfigProperty("DragonBoatEndDate", "DragonBoatEndDate", "2013/12/26 0:00:00")]
        public static readonly string DragonBoatEndDate;
        [ConfigProperty("DragonBoatMaxScore", "DragonBoatMaxScore", 30000)]
        public static readonly int DragonBoatMaxScore;
        [ConfigProperty("DragonBoatMinScore", "DragonBoatMinScore", 13000)]
        public static readonly int DragonBoatMinScore;
        [ConfigProperty("DragonBoatProp", "DragonBoatProp", 11690)]
        public static readonly int DragonBoatProp;
        [ConfigProperty("Edition", "µ±Ç°ÓÎÏ·°æ±\x00BE", "2612558")]
        public static readonly string EDITION;
        [ConfigProperty("EndAuction", "ÅÄÂòÊ±\x00BDáÊøËæ»úÊ±\x00BCä", 40)]
        public static int EndAuction;
        [ConfigProperty("EquipMaxRefineryLevel", "ÅÄÂòÊ±\x00BDáÊøËæ»úÊ±\x00BCä", 5)]
        public static int EquipMaxRefineryLevel;
        [ConfigProperty("EquipRefineryExp", "ÅÄÂòÊ±\x00BDáÊøËæ»úÊ±\x00BCä", "500|2000|7250|28250|112250")]
        public static string EquipRefineryExp;
        [ConfigProperty("FightFootballTime", "FightFootballTime", "19|60")]
        public static readonly string FightFootballTime;
        [ConfigProperty("FreeExp", "µ±Ç°ÓÎÏ·°æ±\x00BE", "11901|1")]
        public static readonly string FreeExp;
        [ConfigProperty("FreeMoney", "µ±Ç°ÓÎÏ·°æ±\x00BE", 9990000)]
        public static readonly int FreeMoney;
        [ConfigProperty("HoleLevelUpExpList", "HoleLevelUpExpList", "400|600|700|800|800")]
        public static string HoleLevelUpExpList;
        [ConfigProperty("HotSpringExp", "Kinh nghiệm Spa", "1|2")]
        public static readonly string HotSpringExp;
        [ConfigProperty("IsActiveMoney", "IsActiveMoney", true)]
        public static readonly bool IsActiveMoney;
        [ConfigProperty("IsDDTMoneyActive", "IsDDTMoneyActive", false)]
        public static readonly bool IsDDTMoneyActive;
        [ConfigProperty("IsLimitAuction", "IsLimitAuction", false)]
        public static readonly bool IsLimitAuction;
        [ConfigProperty("IsLimitCount", "IsLimitCount", false)]
        public static readonly bool IsLimitCount;
        [ConfigProperty("IsLimitMail", "IsLimitMail", false)]
        public static readonly bool IsLimitMail;
        [ConfigProperty("IsLimitMoney", "IsLimitMoney", false)]
        public static readonly bool IsLimitMoney;
        [ConfigProperty("WishBeadRate", "WishBeadRate", 0.5)]
        public static double WishBeadRate;
        [ConfigProperty("WishBeadLimitLv", "WishBeadLimitLv", 12)]
        public static readonly int WishBeadLimitLv;
        [ConfigProperty("IsWishBeadLimit", "IsWishBeadLimit", false)]
        public static readonly bool IsWishBeadLimit;
        [ConfigProperty("IsPromotePackageOpen", "IsPromotePackageOpen", false)]
        public static readonly bool IsPromotePackageOpen;
        [ConfigProperty("KingBuffPrice", "KingBuffPrice", "475,1425,2500")]
        public static readonly string KingBuffPrice;
        [ConfigProperty("LightRiddleAnswerScore", "LightRiddleAnswerScore", "29|9")]
        public static readonly string LightRiddleAnswerScore;
        [ConfigProperty("LightRiddleAnswerTime", "LightRiddleAnswerTime", 15)]
        public static readonly int LightRiddleAnswerTime;
        [ConfigProperty("LightRiddleBeginDate", "LightRiddleBeginDate", "2014/2/13 0:00:00")]
        public static readonly string LightRiddleBeginDate;
        [ConfigProperty("LightRiddleBeginTime", "LightRiddleBeginTime", "2014/2/13 12:30:00")]
        public static readonly string LightRiddleBeginTime;
        [ConfigProperty("LightRiddleComboMoney", "LightRiddleComboMoney", 30)]
        public static readonly int LightRiddleComboMoney;
        [ConfigProperty("LightRiddleEndDate", "LightRiddleEndDate", "2014/2/28 0:00:00")]
        public static readonly string LightRiddleEndDate;
        [ConfigProperty("LightRiddleEndTime", "LightRiddleEndTime", "2014/2/13 13:00:00")]
        public static readonly string LightRiddleEndTime;
        [ConfigProperty("LightRiddleFreeComboNum", "LightRiddleFreeComboNum", 2)]
        public static readonly int LightRiddleFreeComboNum;
        [ConfigProperty("LightRiddleFreeHitNum", "LightRiddleFreeHitNum", 2)]
        public static readonly int LightRiddleFreeHitNum;
        [ConfigProperty("LightRiddleHitMoney", "LightRiddleHitMoney", 30)]
        public static readonly int LightRiddleHitMoney;
        [ConfigProperty("LightRiddleOpenLevel", "LightRiddleOpenLevel", 15)]
        public static readonly int LightRiddleOpenLevel;
        [ConfigProperty("LimitAuction", "LimitAuction", 3)]
        public static readonly int LimitAuction;
        [ConfigProperty("LimitCount", "LimitCount", 10)]
        public static readonly int LimitCount;
        [ConfigProperty("LimitMail", "LimitMail", 3)]
        public static readonly int LimitMail;
        [ConfigProperty("LimitMoney", "LimitMoney", 999000)]
        public static readonly int LimitMoney;
        [ConfigProperty("LuckStarActivityBeginDate", "LuckStarActivityBeginDate", "2013/12/1 0:00:00")]
        public static readonly string LuckStarActivityBeginDate;
        [ConfigProperty("LuckStarActivityEndDate", "LuckStarActivityEndDate", "2014/12/24 0:00:00")]
        public static readonly string LuckStarActivityEndDate;
        [ConfigProperty("MagicPackageID", "MagicPackageID", "0|112378|112379|112380")]
        public static readonly string MagicPackageID;
        [ConfigProperty("MagicStoneOpenPoint", "MagicStoneOpenPoint", "0|10|25|50")]
        public static readonly string MagicStoneOpenPoint;
        [ConfigProperty("MaxMissionEnergy", "MaxMissionEnergy", 300)]
        public static readonly int MaxMissionEnergy;
        [ConfigProperty("MinUseNum", "MinUseNum", 1000)]
        public static readonly int MinUseNum;
        [ConfigProperty("NewChickenBeginTime", "NewChickenBeginTime", "2013/12/17 0:00:00")]
        public static readonly string NewChickenBeginTime;
        [ConfigProperty("NewChickenEagleEyePrice", "NewChickenEagleEyePrice", "3000, 2000, 1000")]
        public static readonly string NewChickenEagleEyePrice;
        [ConfigProperty("NewChickenEndTime", "NewChickenEndTime", "2013/12/25 0:00:00")]
        public static readonly string NewChickenEndTime;
        [ConfigProperty("NewChickenFlushPrice", "NewChickenFlushPrice", 10000)]
        public static readonly int NewChickenFlushPrice;
        [ConfigProperty("NewChickenOpenCardPrice", "NewChickenOpenCardPrice", "2500, 2000, 1500, 1000, 500")]
        public static readonly string NewChickenOpenCardPrice;
        [ConfigProperty("OpenMagicStonePackageMoney", "OpenMagicStonePackageMoney", "0|150|350|650")]
        public static readonly string OpenMagicStonePackageMoney;
        [ConfigProperty("OpenRunePackageMoney", "OpenRunePackageMoney", "10|20|50|100")]
        public static readonly string OpenRunePackageMoney;
        [ConfigProperty("OpenRunePackageRange", "OpenRunePackageRange", "1,6|1,5|1,4")]
        public static readonly string OpenRunePackageRange;
        [ConfigProperty("PetExp", "µ±Ç°ÓÎÏ·°æ±\x00BE", "334103|999")]
        public static readonly string PetExp;
        [ConfigProperty("DivorcedMoney", "Àë»éµÄ\x00BCÛ¸ñ", 1499)]
        public static readonly int PRICE_DIVORCED;
        [ConfigProperty("DivorcedDiscountMoney", "Àë»éµÄ\x00BCÛ¸ñ", 999)]
        public static readonly int PRICE_DIVORCED_DISCOUNT;
        [ConfigProperty("MustFusionGold", "ÈÛÁ¶ÏûºÄ\x00BDð±Ò\x00BCÛ¸ñ", 400)]
        public static readonly int PRICE_FUSION_GOLD;
        [ConfigProperty("MarryRoomCreateMoney", "\x00BDá»é·¿\x00BCäµÄ\x00BCÛ¸ñ,2Ð¡Ê±¡¢3Ð¡Ê±¡¢4Ð¡Ê±ÓÃ¶ººÅ·Ö¸ô", "2000,2700,3400")]
        public static readonly string PRICE_MARRY_ROOM;
        [ConfigProperty("HymenealMoney", "Çó»éµÄ\x00BCÛ¸ñ", 300)]
        public static readonly int PRICE_PROPOSE;
        [ConfigProperty("MustStrengthenGold", "Ç¿»¯\x00BDð±ÒÏûºÄ\x00BCÛ¸ñ", 1000)]
        public static readonly int PRICE_STRENGHTN_GOLD;
        [ConfigProperty("PromotePackagePrice", "PromotePackagePrice", 3600)]
        public static readonly int PromotePackagePrice;
        [ConfigProperty("PyramidBeginTime", "PyramidBeginTime", "2013/12/17 0:00:00")]
        public static readonly string PyramidBeginTime;
        [ConfigProperty("PyramidEndTime", "NewChickenEndTime", "2013/12/25 0:00:00")]
        public static readonly string PyramidEndTime;
        [ConfigProperty("PyramidRevivePrice", "PyramidRevivePrice", "10000, 30000, 50000")]
        public static readonly string PyramidRevivePrice;
        [ConfigProperty("PyramydTurnCardPrice", "PyramydTurnCardPrice", 5000)]
        public static readonly int PyramydTurnCardPrice;
        [ConfigProperty("RingLevel", "ÅÄÂòÊ±\x00BDáÊøËæ»úÊ±\x00BCä", "1000|4000|9000|16000|25000|37000|52000|70000|91000")]
        public static string RingLevel;
        [ConfigProperty("RingMaxRefineryLevel", "ÅÄÂòÊ±\x00BDáÊøËæ»úÊ±\x00BCä", 9)]
        public static int RingMaxRefineryLevel;
        [ConfigProperty("RuneLevelUpExp", "Kinh nghiệm châu báu", "1|2")]
        public static readonly string RuneLevelUpExp;
        [ConfigProperty("RunePackageID", "RunePackageID", "311100|311200|311300|311400")]
        public static readonly string RunePackageID;
        [ConfigProperty("SearchGoodsFreeCount", "SearchGoodsFreeCount", 3)]
        public static readonly int SearchGoodsFreeCount;
        [ConfigProperty("SearchGoodsFreeLimit", "SearchGoodsFreeLimit", 0)]
        public static readonly int SearchGoodsFreeLimit;
        [ConfigProperty("SearchGoodsPayMoney", "SearchGoodsPayMoney", 20)]
        public static readonly int SearchGoodsPayMoney;
        [ConfigProperty("SearchGoodsTakeCardMoney", "SearchGoodsTakeCardMoney", "0|50|120")]
        public static readonly string SearchGoodsTakeCardMoney;
        [ConfigProperty("SpaPriRoomContinueTime", "ÅÄÂòÊ±\x00BDáÊøËæ»úÊ±\x00BCä", 60)]
        public static int SpaPriRoomContinueTime;
        [ConfigProperty("SpaPubRoomLoginPay", "ÅÄÂòÊ±\x00BDáÊøËæ»úÊ±\x00BCä", "10000,200")]
        public static string SpaPubRoomLoginPay;
        [ConfigProperty("TestActive", "TestActive", false)]
        public static readonly bool TestActive;
        [ConfigProperty("VIPExpForEachLv", "VIPExpForEachLv", "1|2")]
        public static readonly string VIPExpForEachLv;
        [ConfigProperty("VIPStrengthenEx", "VIPStrengthenEx", "03|06|09|12|15|18|21|24|27|30")]
        public static readonly string VIPStrengthenEx;
        [ConfigProperty("VirtualName", "VirtualName", "Doreamon,Nobita,Xuneo,Xuka")]
        public static readonly string VirtualName;
        [ConfigProperty("TimeForLeague", "TimeForLeague", "19:30|21:30")]
        public static readonly string TimeForLeague;
        [ConfigProperty("AcademyMasterFreezeHours", "AcademyMasterFreezeHours", 48)]
        public static int AcademyMasterFreezeHours;
        [ConfigProperty("AcademyApprenticeFreezeHours", "AcademyApprenticeFreezeHours", 24)]
        public static int AcademyApprenticeFreezeHours;
        [ConfigProperty("AcademyApprenticeAward", "AcademyApprenticeAward", "10|112085,15|112086,18|112087,20|112125")]
        public static string AcademyApprenticeAward;
        [ConfigProperty("AcademyMasterAward", "AcademyMasterAward", "10|112088,15|112089,18|112090,20|112124")]
        public static string AcademyMasterAward;
        [ConfigProperty("AcademyAppAwardComplete", "AcademyAppAwardComplete", "1401|5293,1301|5192")]
        public static string AcademyAppAwardComplete;
        [ConfigProperty("AcademyMasAwardComplete", "AcademyMasAwardComplete", "1414|5409,1314|5306")]
        public static string AcademyMasAwardComplete;
        [ConfigProperty("AcademyMasterExpPlus", "AcademyMasterExpPlus", "10|15")]
        public static string AcademyMasterExpPlus;
        [ConfigProperty("LeftRouterRateData", "LeftRouterRateData", "0.8|0.9|1.0|1.1|1.2|")]
        public static string LeftRouterRateData;
        [ConfigProperty("LeftRouterMaxDay", "LeftRouterMaxDay", 5)]
        public static int LeftRouterMaxDay;
        [ConfigProperty("LeftRouterEndDate", "LeftRouterEndDate", "2012-01-01 20:55:27.270")]
        public static string LeftRouterEndDate;
        [ConfigProperty("EliteGameBlockWeapon", "ÅÄÂòÊ±\x00BDáÊøËæ»úÊ±\x00BCä", "7144|71441|71442|71443|71444|7145|71451|71452|71453|71454")]
        public static string EliteGameBlockWeapon;
        [ConfigProperty("EliteGameDayOpening", "EliteGameDayOpening", "0,6")]
        public static string EliteGameDayOpening;
        [ConfigProperty("FightLibAwardGift", "FightLibAwardGift", "100,300,500")]
        public static string FightLibAwardGift;
        [ConfigProperty("FightLibAwardExp", "FightLibAwardExp", "2000,5000,8000")]
        public static string FightLibAwardExp;
        [ConfigProperty("MissionRiches", "MissionRiches", "3000|3000|5000|5000|8000|8000|10000|10000|12000|12000")]
        public static string MissionRiches;
        [ConfigProperty("MissionAwardRiches", "MissionAwardRiches", "4500|4500|7500|7500|12000|12000|15000|15000|18000|18000")]
        public static string MissionAwardRiches;
        [ConfigProperty("MissionAwardGP", "MissionAwardGP", "100000|100000|200000|200000|300000|300000|400000|400000|500000|500000")]
        public static string MissionAwardGP;
        [ConfigProperty("MissionAwardOffer", "MissionAwardOffer", "500|500|1000|1000|1500|1500|2000|2000|2500|2500")]
        public static string MissionAwardOffer;
        [ConfigProperty("MissionMinute", "MissionMinute", 360)]
        public static int MissionMinute;
        [ConfigProperty("MissionBuffDay", "MissionBuffDay", 1)]
        public static int MissionBuffDay;
        [ConfigProperty("InlayGoldPrice", "InlayGoldPrice", 2000)]
        public static int InlayGoldPrice;
        [ConfigProperty("FightSpiritSwitch", "", true)]
        public static bool FightSpiritSwitch;
        [ConfigProperty("FightSpiritMaxLevel", "FightSpiritMaxLevel", 12)]
        public readonly static int FightSpiritMaxLevel;
        [ConfigProperty("FightSpiritLevelAddDamage", "FightSpiritLevelAddDamage", "12,6")]
        public readonly static string FightSpiritLevelAddDamage;

        public static int ConsortiaStrengExp(int Lv)
        {
            return GameProperties.getProp(GameProperties.ConsortiaStrengthenEx)[Lv];
        }

        public static List<int> getProp(string prop)
        {
            List<int> intList = new List<int>();
            string str1 = prop;
            char[] chArray = new char[1] { '|' };
            foreach (string str2 in str1.Split(chArray))
                intList.Add(Convert.ToInt32(str2));
            return intList;
        }

        public static int HoleLevelUpExp(int lv)
        {
            return GameProperties.getProp(GameProperties.HoleLevelUpExpList)[lv];
        }

        public static int LimitLevel(int index)
        {
            return Convert.ToInt32(GameProperties.CustomLimit.Split('|')[index]);
        }

        private static void Load(Type type)
        {
            using (ServiceBussiness sb = new ServiceBussiness())
            {
                foreach (FieldInfo field in type.GetFields())
                {
                    if (field.IsStatic)
                    {
                        object[] customAttributes = field.GetCustomAttributes(typeof(ConfigPropertyAttribute), false);
                        if ((uint)customAttributes.Length > 0U)
                        {
                            ConfigPropertyAttribute attrib = (ConfigPropertyAttribute)customAttributes[0];
                            field.SetValue((object)null, GameProperties.LoadProperty(attrib, sb));
                        }
                    }
                }
            }
        }

        public static Dictionary<int, int> ConvertIntDict(
          string value,
          char splitChar,
          char subSplitChar)
        {
            string[] strArray1 = value.Split(splitChar);
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            for (int index = 0; index < strArray1.Length; ++index)
            {
                string[] strArray2 = strArray1[index].Split(subSplitChar);
                if (!dictionary.ContainsKey(int.Parse(strArray2[0])))
                    dictionary.Add(int.Parse(strArray2[0]), int.Parse(strArray2[1]));
            }
            return dictionary;
        }

        public static int[] ConvertIntArray(string value, char splitChar)
        {
            string[] strArray = value.Split(splitChar);
            int[] numArray = new int[strArray.Length];
            for (int index = 0; index < strArray.Length; ++index)
                numArray[index] = int.Parse(strArray[index]);
            return numArray;
        }

        public static Dictionary<int, int> AcademyApprenticeAwardArr()
        {
            return GameProperties.ConvertIntDict(GameProperties.AcademyApprenticeAward, ',', '|');
        }

        public static Dictionary<int, int> AcademyMasterAwardArr()
        {
            return GameProperties.ConvertIntDict(GameProperties.AcademyMasterAward, ',', '|');
        }

        public static int[] AcademyMasterExpPlusArr()
        {
            return GameProperties.ConvertIntArray(GameProperties.AcademyMasterExpPlus, '|');
        }

        private static object LoadProperty(ConfigPropertyAttribute attrib, ServiceBussiness sb)
        {
            string key = attrib.Key;
            ServerProperty serverProperty = sb.GetServerPropertyByKey(key);
            if (serverProperty == null)
            {
                serverProperty = new ServerProperty()
                {
                    Key = key,
                    Value = attrib.DefaultValue.ToString()
                };
                GameProperties.log.Error((object)("Cannot find server property " + key + ",keep it default value!"));
            }
            try
            {
                return Convert.ChangeType((object)serverProperty.Value, attrib.DefaultValue.GetType());
            }
            catch (Exception ex)
            {
                GameProperties.log.Error((object)"Exception in GameProperties Load: ", ex);
                return (object)null;
            }
        }

        public static void Refresh()
        {
            GameProperties.log.Info((object)"Refreshing game properties!");
            GameProperties.Load(typeof(GameProperties));
        }

        public static List<int> RuneExp()
        {
            return GameProperties.getProp(GameProperties.RuneLevelUpExp);
        }

        public static void Save()
        {
            GameProperties.log.Info((object)"Saving game properties into db!");
            GameProperties.Save(typeof(GameProperties));
        }

        private static void Save(Type type)
        {
            using (ServiceBussiness sb = new ServiceBussiness())
            {
                foreach (FieldInfo field in type.GetFields())
                {
                    if (field.IsStatic)
                    {
                        object[] customAttributes = field.GetCustomAttributes(typeof(ConfigPropertyAttribute), false);
                        if ((uint)customAttributes.Length > 0U)
                            GameProperties.SaveProperty((ConfigPropertyAttribute)customAttributes[0], sb, field.GetValue((object)null));
                    }
                }
            }
        }

        private static void SaveProperty(
          ConfigPropertyAttribute attrib,
          ServiceBussiness sb,
          object value)
        {
            try
            {
                sb.UpdateServerPropertyByKey(attrib.Key, value.ToString());
            }
            catch (Exception ex)
            {
                GameProperties.log.Error((object)"Exception in GameProperties Save: ", ex);
            }
        }

        public static List<int> VIPExp()
        {
            return GameProperties.getProp(GameProperties.VIPExpForEachLv);
        }

        public static int VIPStrengthenExp(int vipLv)
        {
            return GameProperties.getProp(GameProperties.VIPStrengthenEx)[vipLv];
        }

        public static int[] MissionRichesArr()
        {
            return GameProperties.ConvertIntArray(GameProperties.MissionRiches, '|');
        }

        public static int[] MissionAwardRichesArr()
        {
            return GameProperties.ConvertIntArray(GameProperties.MissionAwardRiches, '|');
        }

        public static int[] MissionAwardGPArr()
        {
            return GameProperties.ConvertIntArray(GameProperties.MissionAwardGP, '|');
        }

        public static int[] MissionAwardOfferArr()
        {
            return GameProperties.ConvertIntArray(GameProperties.MissionAwardOffer, '|');
        }
    }
}
