using Bussiness.CenterService;
using Bussiness.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Bussiness
{
    public class PlayerBussiness : BaseBussiness
    {
        public bool ActivePlayer(ref PlayerInfo player, string userName, string passWord, bool sex, int gold, int money, string IP, string site)
        {
            bool result = false;
            try
            {
                player = new PlayerInfo();
                player.Agility = 0;
                player.Attack = 0;
                player.Colors = ",,,,,,";
                player.Skin = "";
                player.ConsortiaID = 0;
                player.Defence = 0;
                player.Gold = 0;
                player.GP = 1;
                player.Grade = 1;
                player.ID = 0;
                player.Luck = 0;
                player.Money = 0;
                player.NickName = "";
                player.Sex = sex;
                player.State = 0;
                player.Style = ",,,,,,";
                player.Hide = 1111111111;
                SqlParameter[] array = new SqlParameter[21];
                array[0] = new SqlParameter("@UserID", SqlDbType.Int);
                array[0].Direction = ParameterDirection.Output;
                array[1] = new SqlParameter("@Attack", player.Attack);
                array[2] = new SqlParameter("@Colors", (player.Colors == null) ? "" : player.Colors);
                array[3] = new SqlParameter("@ConsortiaID", player.ConsortiaID);
                array[4] = new SqlParameter("@Defence", player.Defence);
                array[5] = new SqlParameter("@Gold", player.Gold);
                array[6] = new SqlParameter("@GP", player.GP);
                array[7] = new SqlParameter("@Grade", player.Grade);
                array[8] = new SqlParameter("@Luck", player.Luck);
                array[9] = new SqlParameter("@Money", player.Money);
                array[10] = new SqlParameter("@Style", (player.Style == null) ? "" : player.Style);
                array[11] = new SqlParameter("@Agility", player.Agility);
                array[12] = new SqlParameter("@State", player.State);
                array[13] = new SqlParameter("@UserName", userName);
                array[14] = new SqlParameter("@PassWord", passWord);
                array[15] = new SqlParameter("@Sex", sex);
                array[16] = new SqlParameter("@Hide", player.Hide);
                array[17] = new SqlParameter("@ActiveIP", IP);
                array[18] = new SqlParameter("@Skin", (player.Skin == null) ? "" : player.Skin);
                array[19] = new SqlParameter("@Result", SqlDbType.Int);
                array[19].Direction = ParameterDirection.ReturnValue;
                array[20] = new SqlParameter("@Site", site);
                result = db.RunProcedure("SP_Users_Active", array);
                player.ID = (int)array[0].Value;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool AddAuction(AuctionInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[18]
                {
                    new SqlParameter("@AuctionID", info.AuctionID),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                };
                array[0].Direction = ParameterDirection.Output;
                array[1] = new SqlParameter("@AuctioneerID", info.AuctioneerID);
                array[2] = new SqlParameter("@AuctioneerName", (info.AuctioneerName == null) ? "" : info.AuctioneerName);
                array[3] = new SqlParameter("@BeginDate", info.BeginDate);
                array[4] = new SqlParameter("@BuyerID", info.BuyerID);
                array[5] = new SqlParameter("@BuyerName", (info.BuyerName == null) ? "" : info.BuyerName);
                array[6] = new SqlParameter("@IsExist", info.IsExist);
                array[7] = new SqlParameter("@ItemID", info.ItemID);
                array[8] = new SqlParameter("@Mouthful", info.Mouthful);
                array[9] = new SqlParameter("@PayType", info.PayType);
                array[10] = new SqlParameter("@Price", info.Price);
                array[11] = new SqlParameter("@Rise", info.Rise);
                array[12] = new SqlParameter("@ValidDate", info.ValidDate);
                array[13] = new SqlParameter("@TemplateID", info.TemplateID);
                array[14] = new SqlParameter("Name", info.Name);
                array[15] = new SqlParameter("Category", info.Category);
                array[16] = new SqlParameter("Random", info.Random);
                array[17] = new SqlParameter("goodsCount", info.goodsCount);
                result = db.RunProcedure("SP_Auction_Add", array);
                info.AuctionID = (int)array[0].Value;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool AddCards(UsersCardInfo item)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[19]
                {
                    new SqlParameter("@CardID", item.CardID),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                };
                array[0].Direction = ParameterDirection.Output;
                array[1] = new SqlParameter("@UserID", item.UserID);
                array[2] = new SqlParameter("@TemplateID", item.TemplateID);
                array[3] = new SqlParameter("@Place", item.Place);
                array[4] = new SqlParameter("@Count", item.Count);
                array[5] = new SqlParameter("@Attack", item.Attack);
                array[6] = new SqlParameter("@Defence", item.Defence);
                array[7] = new SqlParameter("@Agility", item.Agility);
                array[8] = new SqlParameter("@Luck", item.Luck);
                array[9] = new SqlParameter("@Guard", item.Guard);
                array[10] = new SqlParameter("@Damage", item.Damage);
                array[11] = new SqlParameter("@Level", item.Level);
                array[12] = new SqlParameter("@CardGP", item.CardGP);
                array[14] = new SqlParameter("@isFirstGet", item.isFirstGet);
                array[15] = new SqlParameter("@AttackReset", item.AttackReset);
                array[16] = new SqlParameter("@DefenceReset", item.DefenceReset);
                array[17] = new SqlParameter("@AgilityReset", item.AgilityReset);
                array[18] = new SqlParameter("@LuckReset", item.LuckReset);
                array[13] = new SqlParameter("@Result", SqlDbType.Int);
                array[13].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_UserCard_Add", array);
                result = ((int)array[13].Value == 0);
                item.CardID = (int)array[0].Value;
                item.IsDirty = false;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool AddChargeMoney(string chargeID, string userName, int money, string payWay, decimal needMoney, ref int userID, ref int isResult, DateTime date, string IP, string nickName)
        {
            bool result = false;
            userID = 0;
            try
            {
                SqlParameter[] array = new SqlParameter[10]
                {
                    new SqlParameter("@ChargeID", chargeID),
                    new SqlParameter("@UserName", userName),
                    new SqlParameter("@Money", money),
                    new SqlParameter("@Date", date.ToString("yyyy-MM-dd HH:mm:ss")),
                    new SqlParameter("@PayWay", payWay),
                    new SqlParameter("@NeedMoney", needMoney),
                    new SqlParameter("@UserID", userID),
                    null,
                    null,
                    null
                };
                array[6].Direction = ParameterDirection.InputOutput;
                array[7] = new SqlParameter("@Result", SqlDbType.Int);
                array[7].Direction = ParameterDirection.ReturnValue;
                array[8] = new SqlParameter("@IP", IP);
                array[9] = new SqlParameter("@NickName", nickName);
                result = db.RunProcedure("SP_Charge_Money_Add", array);
                userID = (int)array[6].Value;
                isResult = (int)array[7].Value;
                result = (isResult == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool AddChargeMoney(string chargeID, string userName, int money, string payWay, decimal needMoney, ref int userID, ref int isResult, DateTime date, string IP, int UserID)
        {
            bool result = false;
            userID = 0;
            try
            {
                SqlParameter[] array = new SqlParameter[10]
                {
                    new SqlParameter("@ChargeID", chargeID),
                    new SqlParameter("@UserName", userName),
                    new SqlParameter("@Money", money),
                    new SqlParameter("@Date", date.ToString("yyyy-MM-dd HH:mm:ss")),
                    new SqlParameter("@PayWay", payWay),
                    new SqlParameter("@NeedMoney", needMoney),
                    new SqlParameter("@UserID", userID),
                    null,
                    null,
                    null
                };
                array[6].Direction = ParameterDirection.InputOutput;
                array[7] = new SqlParameter("@Result", SqlDbType.Int);
                array[7].Direction = ParameterDirection.ReturnValue;
                array[8] = new SqlParameter("@IP", IP);
                array[9] = new SqlParameter("@SourceUserID", UserID);
                result = db.RunProcedure("SP_Charge_Money_UserId_Add", array);
                userID = (int)array[6].Value;
                isResult = (int)array[7].Value;
                result = (isResult == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool AddFriends(FriendInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[7]
                {
                    new SqlParameter("@ID", info.ID),
                    new SqlParameter("@AddDate", DateTime.Now),
                    new SqlParameter("@FriendID", info.FriendID),
                    new SqlParameter("@IsExist", true),
                    new SqlParameter("@Remark", (info.Remark == null) ? "" : info.Remark),
                    new SqlParameter("@UserID", info.UserID),
                    new SqlParameter("@Relation", info.Relation)
                };
                result = db.RunProcedure("SP_Users_Friends_Add", sqlParameters);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool AddGoods(ItemInfo item)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[35];
                array[0] = new SqlParameter("@ItemID", item.ItemID);
                array[0].Direction = ParameterDirection.Output;
                array[1] = new SqlParameter("@UserID", item.UserID);
                array[2] = new SqlParameter("@TemplateID", item.Template.TemplateID);
                array[3] = new SqlParameter("@Place", item.Place);
                array[4] = new SqlParameter("@AgilityCompose", item.AgilityCompose);
                array[5] = new SqlParameter("@AttackCompose", item.AttackCompose);
                array[6] = new SqlParameter("@BeginDate", item.BeginDate);
                array[7] = new SqlParameter("@Color", (item.Color == null) ? "" : item.Color);
                array[8] = new SqlParameter("@Count", item.Count);
                array[9] = new SqlParameter("@DefendCompose", item.DefendCompose);
                array[10] = new SqlParameter("@IsBinds", item.IsBinds);
                array[11] = new SqlParameter("@IsExist", item.IsExist);
                array[12] = new SqlParameter("@IsJudge", item.IsJudge);
                array[13] = new SqlParameter("@LuckCompose", item.LuckCompose);
                array[14] = new SqlParameter("@StrengthenLevel", item.StrengthenLevel);
                array[15] = new SqlParameter("@ValidDate", item.ValidDate);
                array[16] = new SqlParameter("@BagType", item.BagType);
                array[17] = new SqlParameter("@Skin", (item.Skin == null) ? "" : item.Skin);
                array[18] = new SqlParameter("@IsUsed", item.IsUsed);
                array[19] = new SqlParameter("@RemoveType", item.RemoveType);
                array[20] = new SqlParameter("@Hole1", item.Hole1);
                array[21] = new SqlParameter("@Hole2", item.Hole2);
                array[22] = new SqlParameter("@Hole3", item.Hole3);
                array[23] = new SqlParameter("@Hole4", item.Hole4);
                array[24] = new SqlParameter("@Hole5", item.Hole5);
                array[25] = new SqlParameter("@Hole6", item.Hole6);
                array[26] = new SqlParameter("@StrengthenTimes", item.StrengthenTimes);
                array[27] = new SqlParameter("@Hole5Level", item.Hole5Level);
                array[28] = new SqlParameter("@Hole5Exp", item.Hole5Exp);
                array[29] = new SqlParameter("@Hole6Level", item.Hole6Level);
                array[30] = new SqlParameter("@Hole6Exp", item.Hole6Exp);
                array[31] = new SqlParameter("@IsGold", item.IsGold);
                array[32] = new SqlParameter("@goldValidDate", item.goldValidDate);
                array[33] = new SqlParameter("@goldBeginTime", item.goldBeginTime);
                array[34] = new SqlParameter("@StrengthenExp", item.StrengthenExp);
                result = db.RunProcedure("SP_Users_Items_Add", array);
                item.ItemID = (int)array[0].Value;
                item.IsDirty = false;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool AddGoods(ItemInfo item, bool Strengthen)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[34];
                array[0] = new SqlParameter("@ItemID", item.ItemID);
                array[0].Direction = ParameterDirection.Output;
                array[1] = new SqlParameter("@UserID", item.UserID);
                array[2] = new SqlParameter("@TemplateID", item.Template.TemplateID);
                array[3] = new SqlParameter("@Place", item.Place);
                array[4] = new SqlParameter("@AgilityCompose", item.AgilityCompose);
                array[5] = new SqlParameter("@AttackCompose", item.AttackCompose);
                array[6] = new SqlParameter("@BeginDate", item.BeginDate);
                array[7] = new SqlParameter("@Color", (item.Color == null) ? "" : item.Color);
                array[8] = new SqlParameter("@Count", item.Count);
                array[9] = new SqlParameter("@DefendCompose", item.DefendCompose);
                array[10] = new SqlParameter("@IsBinds", item.IsBinds);
                array[11] = new SqlParameter("@IsExist", item.IsExist);
                array[12] = new SqlParameter("@IsJudge", item.IsJudge);
                array[13] = new SqlParameter("@LuckCompose", item.LuckCompose);
                array[14] = new SqlParameter("@StrengthenLevel", item.StrengthenLevel);
                array[15] = new SqlParameter("@ValidDate", item.ValidDate);
                array[16] = new SqlParameter("@BagType", item.BagType);
                array[17] = new SqlParameter("@Skin", (item.Skin == null) ? "" : item.Skin);
                array[18] = new SqlParameter("@IsUsed", item.IsUsed);
                array[19] = new SqlParameter("@RemoveType", item.RemoveType);
                array[20] = new SqlParameter("@Hole1", item.Hole1);
                array[21] = new SqlParameter("@Hole2", item.Hole2);
                array[22] = new SqlParameter("@Hole3", item.Hole3);
                array[23] = new SqlParameter("@Hole4", item.Hole4);
                array[24] = new SqlParameter("@Hole5", item.Hole5);
                array[25] = new SqlParameter("@Hole6", item.Hole6);
                array[26] = new SqlParameter("@StrengthenTimes", item.StrengthenTimes);
                array[27] = new SqlParameter("@Hole5Level", item.Hole5Level);
                array[28] = new SqlParameter("@Hole5Exp", item.Hole5Exp);
                array[29] = new SqlParameter("@Hole6Level", item.Hole6Level);
                array[30] = new SqlParameter("@Hole6Exp", item.Hole6Exp);
                array[31] = new SqlParameter("@IsGold", item.IsGold);
                array[32] = new SqlParameter("@goldValidDate", item.goldValidDate);
                array[33] = new SqlParameter("@goldBeginTime", item.goldBeginTime);
                result = db.RunProcedure("SP_Users_Items_Add", array);
                item.ItemID = (int)array[0].Value;
                item.IsDirty = false;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool AddMarryInfo(MarryInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[5]
                {
                    new SqlParameter("@ID", info.ID),
                    null,
                    null,
                    null,
                    null
                };
                array[0].Direction = ParameterDirection.Output;
                array[1] = new SqlParameter("@UserID", info.UserID);
                array[2] = new SqlParameter("@IsPublishEquip", info.IsPublishEquip);
                array[3] = new SqlParameter("@Introduction", info.Introduction);
                array[4] = new SqlParameter("@RegistTime", info.RegistTime);
                result = db.RunProcedure("SP_MarryInfo_Add", array);
                info.ID = (int)array[0].Value;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("AddMarryInfo", exception);
                return result;
            }
        }

        public bool AddStore(ItemInfo item)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[14]
                {
                    new SqlParameter("@ItemID", item.ItemID),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                };
                array[0].Direction = ParameterDirection.Output;
                array[1] = new SqlParameter("@UserID", item.UserID);
                array[2] = new SqlParameter("@TemplateID", item.Template.TemplateID);
                array[3] = new SqlParameter("@Place", item.Place);
                array[4] = new SqlParameter("@AgilityCompose", item.AgilityCompose);
                array[5] = new SqlParameter("@AttackCompose", item.AttackCompose);
                array[6] = new SqlParameter("@BeginDate", item.BeginDate);
                array[7] = new SqlParameter("@Color", (item.Color == null) ? "" : item.Color);
                array[8] = new SqlParameter("@Count", item.Count);
                array[9] = new SqlParameter("@DefendCompose", item.DefendCompose);
                array[10] = new SqlParameter("@IsBinds", item.IsBinds);
                array[11] = new SqlParameter("@IsExist", item.IsExist);
                array[12] = new SqlParameter("@IsJudge", item.IsJudge);
                array[13] = new SqlParameter("@LuckCompose", item.LuckCompose);
                result = db.RunProcedure("SP_Users_Items_Add", array);
                item.ItemID = (int)array[0].Value;
                item.IsDirty = false;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool AddUserMatchInfo(UserMatchInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[16]
                {
                    new SqlParameter("@ID", info.ID),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                };
                array[0].Direction = ParameterDirection.Output;
                array[1] = new SqlParameter("@UserID", info.UserID);
                array[2] = new SqlParameter("@dailyScore", info.dailyScore);
                array[3] = new SqlParameter("@dailyWinCount", info.dailyWinCount);
                array[4] = new SqlParameter("@dailyGameCount", info.dailyGameCount);
                array[5] = new SqlParameter("@DailyLeagueFirst", info.DailyLeagueFirst);
                array[6] = new SqlParameter("@DailyLeagueLastScore", info.DailyLeagueLastScore);
                array[7] = new SqlParameter("@weeklyScore", info.weeklyScore);
                array[8] = new SqlParameter("@weeklyGameCount", info.weeklyGameCount);
                array[9] = new SqlParameter("@weeklyRanking", info.weeklyRanking);
                array[10] = new SqlParameter("@addDayPrestge", info.addDayPrestge);
                array[11] = new SqlParameter("@totalPrestige", info.totalPrestige);
                array[12] = new SqlParameter("@restCount", info.restCount);
                array[13] = new SqlParameter("@leagueGrade", info.leagueGrade);
                array[14] = new SqlParameter("@leagueItemsGet", info.leagueItemsGet);
                array[15] = new SqlParameter("@Result", SqlDbType.Int);
                array[15].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_UserMatch_Add", array);
                result = ((int)array[15].Value == 0);
                info.ID = (int)array[0].Value;
                info.IsDirty = false;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool AddUserRank(UserRankInfo item)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[14]
                {
                    new SqlParameter("@ID", item.ID),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                };
                array[0].Direction = ParameterDirection.Output;
                array[1] = new SqlParameter("@UserID", item.UserID);
                array[2] = new SqlParameter("@UserRank", item.UserRank);
                array[3] = new SqlParameter("@Attack", item.Attack);
                array[4] = new SqlParameter("@Defence", item.Defence);
                array[5] = new SqlParameter("@Luck", item.Luck);
                array[6] = new SqlParameter("@Agility", item.Agility);
                array[7] = new SqlParameter("@HP", item.HP);
                array[8] = new SqlParameter("@Damage", item.Damage);
                array[9] = new SqlParameter("@Guard", item.Guard);
                array[10] = new SqlParameter("@BeginDate", item.BeginDate);
                array[11] = new SqlParameter("@Validate", item.Validate);
                array[12] = new SqlParameter("@IsExit", item.IsExit);
                array[13] = new SqlParameter("@Result", SqlDbType.Int);
                array[13].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_UserRank_Add", array);
                result = ((int)array[13].Value == 0);
                item.ID = (int)array[0].Value;
                item.IsDirty = false;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool AddUserUserDrill(UserDrillInfo item)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[6]
                {
                    new SqlParameter("@UserID", item.UserID),
                    new SqlParameter("@BeadPlace", item.BeadPlace),
                    new SqlParameter("@HoleExp", item.HoleExp),
                    new SqlParameter("@HoleLv", item.HoleLv),
                    new SqlParameter("@DrillPlace", item.DrillPlace),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[5].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Users_UserDrill_Add", array);
                result = ((int)array[5].Value == 0);
                item.IsDirty = false;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool CancelPaymentMail(int userid, int mailID, ref int senderID)
        {
            bool flag = false;
            try
            {
                SqlParameter[] array = new SqlParameter[4]
                {
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@mailID", mailID),
                    new SqlParameter("@senderID", SqlDbType.Int),
                    null
                };
                array[2].Value = senderID;
                array[2].Direction = ParameterDirection.InputOutput;
                array[3] = new SqlParameter("@Result", SqlDbType.Int);
                array[3].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Mail_PaymentCancel", array);
                flag = ((int)array[3].Value == 0);
                if (!flag)
                {
                    return flag;
                }
                senderID = (int)array[2].Value;
                return flag;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return flag;
                }
                BaseBussiness.log.Error("Init", exception);
                return flag;
            }
        }

        public bool ClearDatabase()
        {
            bool result = false;
            try
            {
                result = db.RunProcedure("SP_Sys_Clear_All");
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool ChargeToUser(string userName, ref int money, string nickName)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[3]
                {
                    new SqlParameter("@UserName", userName),
                    new SqlParameter("@money", SqlDbType.Int),
                    null
                };
                array[1].Direction = ParameterDirection.Output;
                array[2] = new SqlParameter("@NickName", nickName);
                result = db.RunProcedure("SP_Charge_To_User", array);
                money = (int)array[1].Value;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool CheckAccount(string username, string password)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[3]
                {
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Password", password),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[2].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_CheckAccount", array);
                result = ((int)array[2].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool CheckEmailIsValid(string Email)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[2]
                {
                    new SqlParameter("@Email", Email),
                    new SqlParameter("@count", SqlDbType.BigInt)
                };
                array[1].Direction = ParameterDirection.Output;
                db.RunProcedure("CheckEmailIsValid", array);
                if (int.Parse(array[1].Value.ToString()) != 0)
                {
                    return result;
                }
                result = true;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init CheckEmailIsValid", exception);
                return result;
            }
        }

        public bool DeleteAuction(int auctionID, int userID, ref string msg)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[3]
                {
                    new SqlParameter("@AuctionID", auctionID),
                    new SqlParameter("@UserID", userID),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[2].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Auction_Delete", array);
                int num = (int)array[2].Value;
                result = (num == 0);
                switch (num)
                {
                    case 0:
                        msg = LanguageMgr.GetTranslation("PlayerBussiness.DeleteAuction.Msg1");
                        return result;
                    case 1:
                        msg = LanguageMgr.GetTranslation("PlayerBussiness.DeleteAuction.Msg2");
                        return result;
                    case 2:
                        msg = LanguageMgr.GetTranslation("PlayerBussiness.DeleteAuction.Msg3");
                        return result;
                    default:
                        msg = LanguageMgr.GetTranslation("PlayerBussiness.DeleteAuction.Msg4");
                        return result;
                }
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool DeleteFriends(int UserID, int FriendID)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[2]
                {
                    new SqlParameter("@ID", FriendID),
                    new SqlParameter("@UserID", UserID)
                };
                result = db.RunProcedure("SP_Users_Friends_Delete", sqlParameters);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool DeleteGoods(int itemID)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@ID", itemID)
                };
                result = db.RunProcedure("SP_Users_Items_Delete", sqlParameters);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool DeleteMail(int UserID, int mailID, out int senderID)
        {
            bool result = false;
            senderID = 0;
            try
            {
                SqlParameter[] array = new SqlParameter[4]
                {
                    new SqlParameter("@ID", mailID),
                    new SqlParameter("@UserID", UserID),
                    new SqlParameter("@SenderID", SqlDbType.Int),
                    null
                };
                array[2].Value = senderID;
                array[2].Direction = ParameterDirection.InputOutput;
                array[3] = new SqlParameter("@Result", SqlDbType.Int);
                array[3].Direction = ParameterDirection.ReturnValue;
                result = db.RunProcedure("SP_Mail_Delete", array);
                if ((int)array[3].Value != 0)
                {
                    return result;
                }
                result = true;
                senderID = (int)array[2].Value;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool DeleteMail2(int UserID, int mailID, out int senderID)
        {
            bool result = false;
            senderID = 0;
            try
            {
                SqlParameter[] array = new SqlParameter[4]
                {
                    new SqlParameter("@ID", mailID),
                    new SqlParameter("@UserID", UserID),
                    new SqlParameter("@SenderID", SqlDbType.Int),
                    null
                };
                array[2].Value = senderID;
                array[2].Direction = ParameterDirection.InputOutput;
                array[3] = new SqlParameter("@Result", SqlDbType.Int);
                array[3].Direction = ParameterDirection.ReturnValue;
                result = db.RunProcedure("SP_Mail_Delete", array);
                if ((int)array[3].Value != 0)
                {
                    return result;
                }
                result = true;
                senderID = (int)array[2].Value;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool DeleteMarryInfo(int ID, int userID, ref string msg)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[3]
                {
                    new SqlParameter("@ID", ID),
                    new SqlParameter("@UserID", userID),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[2].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_MarryInfo_Delete", array);
                int num = (int)array[2].Value;
                result = (num == 0);
                if (num != 0)
                {
                    return result;
                }
                msg = LanguageMgr.GetTranslation("PlayerBussiness.DeleteAuction.Succeed");
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("DeleteAuction", exception);
                return result;
            }
        }

        public bool DisableUser(string userName, bool isExit)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[3]
                {
                    new SqlParameter("@UserName", userName),
                    new SqlParameter("@IsExist", isExit),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[2].Direction = ParameterDirection.ReturnValue;
                result = db.RunProcedure("SP_Disable_User", array);
                if ((int)array[2].Value != 0)
                {
                    return result;
                }
                result = true;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("DisableUser", exception);
                return result;
            }
        }

        public bool DisposeMarryRoomInfo(int ID)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[2]
                {
                    new SqlParameter("@ID", ID),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[1].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Dispose_Marry_Room_Info", array);
                result = ((int)array[1].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("DisposeMarryRoomInfo", exception);
                return result;
            }
        }

        public ConsortiaUserInfo[] GetAllMemberByConsortia(int ConsortiaID)
        {
            List<ConsortiaUserInfo> list = new List<ConsortiaUserInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@ConsortiaID", SqlDbType.Int, 4)
                };
                array[0].Value = ConsortiaID;
                db.GetReader(ref ResultDataReader, "SP_Consortia_Users_All", array);
                while (ResultDataReader.Read())
                {
                    list.Add(InitConsortiaUserInfo(ResultDataReader));
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public UserMatchInfo[] GetAllUserMatchInfo()
        {
            List<UserMatchInfo> list = new List<UserMatchInfo>();
            SqlDataReader ResultDataReader = null;
            int num = 1;
            try
            {
                db.GetReader(ref ResultDataReader, "SP_UserMatch_All_DESC");
                while (ResultDataReader.Read())
                {
                    UserMatchInfo item = new UserMatchInfo
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        totalPrestige = (int)ResultDataReader["totalPrestige"],
                        rank = num
                    };
                    list.Add(item);
                    num++;
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetAllUserMatchDESC", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public AuctionInfo[] GetAuctionPage(int page, string name, int type, int pay, ref int total, int userID, int buyID, int order, bool sort, int size, string string_1)
        {
            List<AuctionInfo> list = new List<AuctionInfo>();
            try
            {
                string text = " IsExist=1 ";
                if (!string.IsNullOrEmpty(name))
                {
                    text = text + " and Name like '%" + name + "%' ";
                }
                switch (type)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                    case 17:
                    case 19:
                        text = text + " and Category =" + type + " ";
                        break;
                    case 21:
                        text += " and Category in(1,2,5,8,9) ";
                        break;
                    case 22:
                        text += " and Category in(13,15,6,4,3) ";
                        break;
                    case 23:
                        text += " and Category in(16,11,10) ";
                        break;
                    case 24:
                        text += " and Category in(8,9) ";
                        break;
                    case 25:
                        text += " and Category in (7,17) ";
                        break;
                    case 26:
                        text += " and TemplateId>=311000 and TemplateId<=313999";
                        break;
                    case 27:
                        text += " and TemplateId>=311000 and TemplateId<=311999 ";
                        break;
                    case 28:
                        text += " and TemplateId>=312000 and TemplateId<=312999 ";
                        break;
                    case 29:
                        text += " and TemplateId>=313000 and TempLateId<=313999";
                        break;
                    case 35:
                        text += " and TemplateID in (11560,11561,11562)";
                        break;
                    case 1100:
                        text += " and TemplateID in (11019,11021,11022,11023) ";
                        break;
                    case 1101:
                        text += " and TemplateID='11019' ";
                        break;
                    case 1102:
                        text += " and TemplateID='11021' ";
                        break;
                    case 1103:
                        text += " and TemplateID='11022' ";
                        break;
                    case 1104:
                        text += " and TemplateID='11023' ";
                        break;
                    case 1105:
                        text += " and TemplateID in (11001,11002,11003,11004,11005,11006,11007,11008,11009,11010,11011,11012,11013,11014,11015,11016) ";
                        break;
                    case 1106:
                        text += " and TemplateID in (11001,11002,11003,11004) ";
                        break;
                    case 1107:
                        text += " and TemplateID in (11005,11006,11007,11008) ";
                        break;
                    case 1108:
                        text += " and TemplateID in (11009,11010,11011,11012) ";
                        break;
                    case 1109:
                        text += " and TemplateID in (11013,11014,11015,11016) ";
                        break;
                    case 1110:
                        text += " and TemplateID='11024' ";
                        break;
                    case 1111:
                    case 1112:
                        text += " and Category in (11) and Property1 = 10";
                        break;
                    case 1113:
                        text += " and TemplateID in (314101,314102,314103,314104,314105,314106,314107,314108,314109,314110,314111,314112,314113,314114,314115,314116,314121,314122,314123,314124,314125,314126,314127,314128,314129,314130,314131,314132,314133,314134) ";
                        break;
                    case 1114:
                        text += " and TemplateID in (314117,314118,314119,314120,314135,314136,314137,314138,314139) ";
                        break;
                    case 1116:
                        text += " and TemplateID='11035' ";
                        break;
                    case 1117:
                        text += " and TemplateID='11036' ";
                        break;
                    case 1118:
                        text += " and TemplateID='11026' ";
                        break;
                    case 1119:
                        text += " and TemplateID='11027' ";
                        break;
                }
                if (pay != -1)
                {
                    text = text + " and PayType =" + pay + " ";
                }
                if (userID != -1)
                {
                    text = text + " and AuctioneerID =" + userID + " ";
                }
                if (buyID != -1)
                {
                    text = text + " and (BuyerID =" + buyID + " or AuctionID in (" + string_1 + ")) ";
                }
                string str = "Category,Name,Price,dd,AuctioneerID";
                switch (order)
                {
                    case 0:
                        str = "Name";
                        break;
                    case 2:
                        str = "dd";
                        break;
                    case 3:
                        str = "AuctioneerName";
                        break;
                    case 4:
                        str = "Price";
                        break;
                    case 5:
                        str = "BuyerName";
                        break;
                }
                string value = str + (sort ? " desc" : "") + ",AuctionID ";
                SqlParameter[] array = new SqlParameter[8]
                {
                    new SqlParameter("@QueryStr", "V_Auction_Scan"),
                    new SqlParameter("@QueryWhere", text),
                    new SqlParameter("@PageSize", size),
                    new SqlParameter("@PageCurrent", page),
                    new SqlParameter("@FdShow", "*"),
                    new SqlParameter("@FdOrder", value),
                    new SqlParameter("@FdKey", "AuctionID"),
                    new SqlParameter("@TotalRow", total)
                };
                array[7].Direction = ParameterDirection.Output;
                DataTable dataTable = db.GetDataTable("Auction", "SP_CustomPage", array);
                total = (int)array[7].Value;
                foreach (DataRow row in dataTable.Rows)
                {
                    list.Add(new AuctionInfo
                    {
                        AuctioneerID = (int)row["AuctioneerID"],
                        AuctioneerName = row["AuctioneerName"].ToString(),
                        AuctionID = (int)row["AuctionID"],
                        BeginDate = (DateTime)row["BeginDate"],
                        BuyerID = (int)row["BuyerID"],
                        BuyerName = row["BuyerName"].ToString(),
                        Category = (int)row["Category"],
                        IsExist = (bool)row["IsExist"],
                        ItemID = (int)row["ItemID"],
                        Name = row["Name"].ToString(),
                        Mouthful = (int)row["Mouthful"],
                        PayType = (int)row["PayType"],
                        Price = (int)row["Price"],
                        Rise = (int)row["Rise"],
                        ValidDate = (int)row["ValidDate"]
                    });
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            return list.ToArray();
        }

        public AuctionInfo GetAuctionSingle(int auctionID)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@AuctionID", auctionID)
                };
                db.GetReader(ref ResultDataReader, "SP_Auction_Single", sqlParameters);
                if (ResultDataReader.Read())
                {
                    return InitAuctionInfo(ResultDataReader);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public BestEquipInfo[] GetCelebByDayBestEquip()
        {
            List<BestEquipInfo> list = new List<BestEquipInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                db.GetReader(ref ResultDataReader, "SP_Users_BestEquip");
                while (ResultDataReader.Read())
                {
                    BestEquipInfo item = new BestEquipInfo
                    {
                        Date = (DateTime)ResultDataReader["RemoveDate"],
                        GP = (int)ResultDataReader["GP"],
                        Grade = (int)ResultDataReader["Grade"],
                        ItemName = ((ResultDataReader["Name"] == null) ? "" : ResultDataReader["Name"].ToString()),
                        NickName = ((ResultDataReader["NickName"] == null) ? "" : ResultDataReader["NickName"].ToString()),
                        Sex = (bool)ResultDataReader["Sex"],
                        Strengthenlevel = (int)ResultDataReader["Strengthenlevel"],
                        UserName = ((ResultDataReader["UserName"] == null) ? "" : ResultDataReader["UserName"].ToString())
                    };
                    list.Add(item);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public ChargeRecordInfo[] GetChargeRecordInfo(DateTime date, int SaveRecordSecond)
        {
            List<ChargeRecordInfo> list = new List<ChargeRecordInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[2]
                {
                    new SqlParameter("@Date", date.ToString("yyyy-MM-dd HH:mm:ss")),
                    new SqlParameter("@Second", SaveRecordSecond)
                };
                db.GetReader(ref ResultDataReader, "SP_Charge_Record", sqlParameters);
                while (ResultDataReader.Read())
                {
                    ChargeRecordInfo item = new ChargeRecordInfo
                    {
                        BoyTotalPay = (int)ResultDataReader["BoyTotalPay"],
                        GirlTotalPay = (int)ResultDataReader["GirlTotalPay"],
                        PayWay = ((ResultDataReader["PayWay"] == null) ? "" : ResultDataReader["PayWay"].ToString()),
                        TotalBoy = (int)ResultDataReader["TotalBoy"],
                        TotalGirl = (int)ResultDataReader["TotalGirl"]
                    };
                    list.Add(item);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public ExerciseInfo GetExerciseSingle(int Grade)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@Grage", Grade)
                };
                db.GetReader(ref ResultDataReader, "SP_Get_Exercise_By_Grade", sqlParameters);
                if (ResultDataReader.Read())
                {
                    return new ExerciseInfo
                    {
                        Grage = (int)ResultDataReader["Grage"],
                        GP = (int)ResultDataReader["GP"],
                        ExerciseA = (int)ResultDataReader["ExerciseA"],
                        ExerciseAG = (int)ResultDataReader["ExerciseAG"],
                        ExerciseD = (int)ResultDataReader["ExerciseD"],
                        ExerciseH = (int)ResultDataReader["ExerciseH"],
                        ExerciseL = (int)ResultDataReader["ExerciseL"]
                    };
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetExerciseInfoSingle", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public FriendInfo[] GetFriendsAll(int UserID)
        {
            List<FriendInfo> list = new List<FriendInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = UserID;
                db.GetReader(ref ResultDataReader, "SP_Users_Friends", array);
                while (ResultDataReader.Read())
                {
                    list.Add(new FriendInfo
                    {
                        AddDate = (DateTime)ResultDataReader["AddDate"],
                        Colors = ((ResultDataReader["Colors"] == null) ? "" : ResultDataReader["Colors"].ToString()),
                        FriendID = (int)ResultDataReader["FriendID"],
                        Grade = (int)ResultDataReader["Grade"],
                        Hide = (int)ResultDataReader["Hide"],
                        ID = (int)ResultDataReader["ID"],
                        IsExist = (bool)ResultDataReader["IsExist"],
                        NickName = ((ResultDataReader["NickName"] == null) ? "" : ResultDataReader["NickName"].ToString()),
                        Remark = ((ResultDataReader["Remark"] == null) ? "" : ResultDataReader["Remark"].ToString()),
                        Sex = (((bool)ResultDataReader["Sex"]) ? 1 : 0),
                        State = (int)ResultDataReader["State"],
                        Style = ((ResultDataReader["Style"] == null) ? "" : ResultDataReader["Style"].ToString()),
                        UserID = (int)ResultDataReader["UserID"],
                        ConsortiaName = ((ResultDataReader["ConsortiaName"] == null) ? "" : ResultDataReader["ConsortiaName"].ToString()),
                        Offer = (int)ResultDataReader["Offer"],
                        Win = (int)ResultDataReader["Win"],
                        Total = (int)ResultDataReader["Total"],
                        Escape = (int)ResultDataReader["Escape"],
                        Relation = (int)ResultDataReader["Relation"],
                        Repute = (int)ResultDataReader["Repute"],
                        UserName = ((ResultDataReader["UserName"] == null) ? "" : ResultDataReader["UserName"].ToString()),
                        DutyName = ((ResultDataReader["DutyName"] == null) ? "" : ResultDataReader["DutyName"].ToString()),
                        Nimbus = (int)ResultDataReader["Nimbus"],
                        apprenticeshipState = (int)ResultDataReader["apprenticeshipState"]
                    });
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public FriendInfo[] GetFriendsBbs(string condictArray)
        {
            List<FriendInfo> list = new List<FriendInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@SearchUserName", SqlDbType.NVarChar, 4000)
                };
                array[0].Value = condictArray;
                db.GetReader(ref ResultDataReader, "SP_Users_FriendsBbs", array);
                while (ResultDataReader.Read())
                {
                    FriendInfo item = new FriendInfo
                    {
                        NickName = ((ResultDataReader["NickName"] == null) ? "" : ResultDataReader["NickName"].ToString()),
                        UserID = (int)ResultDataReader["UserID"],
                        UserName = ((ResultDataReader["UserName"] == null) ? "" : ResultDataReader["UserName"].ToString()),
                        IsExist = ((int)ResultDataReader["UserID"] > 0)
                    };
                    list.Add(item);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public ArrayList GetFriendsGood(string UserName)
        {
            ArrayList arrayList = new ArrayList();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserName", SqlDbType.NVarChar)
                };
                array[0].Value = UserName;
                db.GetReader(ref ResultDataReader, "SP_Users_Friends_Good", array);
                while (ResultDataReader.Read())
                {
                    arrayList.Add((ResultDataReader["UserName"] == null) ? "" : ResultDataReader["UserName"].ToString());
                }
                return arrayList;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return arrayList;
                }
                BaseBussiness.log.Error("Init", exception);
                return arrayList;
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
        }

        public Dictionary<int, int> GetFriendsIDAll(int UserID)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = UserID;
                db.GetReader(ref ResultDataReader, "SP_Users_Friends_All", array);
                while (ResultDataReader.Read())
                {
                    if (!dictionary.ContainsKey((int)ResultDataReader["FriendID"]))
                    {
                        dictionary.Add((int)ResultDataReader["FriendID"], (int)ResultDataReader["Relation"]);
                    }
                    else
                    {
                        dictionary[(int)ResultDataReader["FriendID"]] = (int)ResultDataReader["Relation"];
                    }
                }
                return dictionary;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return dictionary;
                }
                BaseBussiness.log.Error("Init", exception);
                return dictionary;
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
        }

        public MailInfo[] GetMailBySenderID(int userID)
        {
            List<MailInfo> list = new List<MailInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = userID;
                db.GetReader(ref ResultDataReader, "SP_Mail_BySenderID", array);
                while (ResultDataReader.Read())
                {
                    list.Add(InitMail(ResultDataReader));
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public MailInfo[] GetMailByUserID(int userID)
        {
            List<MailInfo> list = new List<MailInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = userID;
                db.GetReader(ref ResultDataReader, "SP_Mail_ByUserID", array);
                while (ResultDataReader.Read())
                {
                    list.Add(InitMail(ResultDataReader));
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public MailInfo GetMailSingle(int UserID, int mailID)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[2]
                {
                    new SqlParameter("@ID", mailID),
                    new SqlParameter("@UserID", UserID)
                };
                db.GetReader(ref ResultDataReader, "SP_Mail_Single", sqlParameters);
                if (ResultDataReader.Read())
                {
                    return InitMail(ResultDataReader);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public MarryInfo[] GetMarryInfoPage(int page, string name, bool sex, int size, ref int total)
        {
            List<MarryInfo> list = new List<MarryInfo>();
            try
            {
                string text = (!sex) ? " IsExist=1 and Sex=0 and UserExist=1" : " IsExist=1 and Sex=1 and UserExist=1";
                if (!string.IsNullOrEmpty(name))
                {
                    text = text + " and NickName like '%" + name + "%' ";
                }
                string value = "State desc,IsMarried";
                SqlParameter[] array = new SqlParameter[8]
                {
                    new SqlParameter("@QueryStr", "V_Sys_Marry_Info"),
                    new SqlParameter("@QueryWhere", text),
                    new SqlParameter("@PageSize", size),
                    new SqlParameter("@PageCurrent", page),
                    new SqlParameter("@FdShow", "*"),
                    new SqlParameter("@FdOrder", value),
                    new SqlParameter("@FdKey", "ID"),
                    new SqlParameter("@TotalRow", total)
                };
                array[7].Direction = ParameterDirection.Output;
                DataTable dataTable = db.GetDataTable("V_Sys_Marry_Info", "SP_CustomPage", array);
                total = (int)array[7].Value;
                foreach (DataRow row in dataTable.Rows)
                {
                    MarryInfo item = new MarryInfo
                    {
                        ID = (int)row["ID"],
                        UserID = (int)row["UserID"],
                        IsPublishEquip = (bool)row["IsPublishEquip"],
                        Introduction = row["Introduction"].ToString(),
                        NickName = row["NickName"].ToString(),
                        IsConsortia = (bool)row["IsConsortia"],
                        ConsortiaID = (int)row["ConsortiaID"],
                        Sex = (bool)row["Sex"],
                        Win = (int)row["Win"],
                        Total = (int)row["Total"],
                        Escape = (int)row["Escape"],
                        GP = (int)row["GP"],
                        Honor = row["Honor"].ToString(),
                        Style = row["Style"].ToString(),
                        Colors = row["Colors"].ToString(),
                        Hide = (int)row["Hide"],
                        Grade = (int)row["Grade"],
                        State = (int)row["State"],
                        Repute = (int)row["Repute"],
                        Skin = row["Skin"].ToString(),
                        Offer = (int)row["Offer"],
                        IsMarried = (bool)row["IsMarried"],
                        ConsortiaName = row["ConsortiaName"].ToString(),
                        DutyName = row["DutyName"].ToString(),
                        Nimbus = (int)row["Nimbus"],
                        FightPower = (int)row["FightPower"],
                        typeVIP = Convert.ToByte(row["typeVIP"]),
                        VIPLevel = (int)row["VIPLevel"]
                    };
                    list.Add(item);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            return list.ToArray();
        }

        public MarryInfo GetMarryInfoSingle(int ID)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@ID", ID)
                };
                db.GetReader(ref ResultDataReader, "SP_MarryInfo_Single", sqlParameters);
                if (ResultDataReader.Read())
                {
                    return new MarryInfo
                    {
                        ID = (int)ResultDataReader["ID"],
                        UserID = (int)ResultDataReader["UserID"],
                        IsPublishEquip = (bool)ResultDataReader["IsPublishEquip"],
                        Introduction = ResultDataReader["Introduction"].ToString(),
                        RegistTime = (DateTime)ResultDataReader["RegistTime"]
                    };
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetMarryInfoSingle", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public MarryProp GetMarryProp(int id)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", id)
                };
                db.GetReader(ref ResultDataReader, "SP_Select_Marry_Prop", sqlParameters);
                if (ResultDataReader.Read())
                {
                    return new MarryProp
                    {
                        IsMarried = (bool)ResultDataReader["IsMarried"],
                        SpouseID = (int)ResultDataReader["SpouseID"],
                        SpouseName = ResultDataReader["SpouseName"].ToString(),
                        IsCreatedMarryRoom = (bool)ResultDataReader["IsCreatedMarryRoom"],
                        SelfMarryRoomID = (int)ResultDataReader["SelfMarryRoomID"],
                        IsGotRing = (bool)ResultDataReader["IsGotRing"]
                    };
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetMarryProp", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public MarryRoomInfo[] GetMarryRoomInfo()
        {
            SqlDataReader ResultDataReader = null;
            List<MarryRoomInfo> list = new List<MarryRoomInfo>();
            try
            {
                db.GetReader(ref ResultDataReader, "SP_Get_Marry_Room_Info");
                while (ResultDataReader.Read())
                {
                    MarryRoomInfo item = new MarryRoomInfo
                    {
                        ID = (int)ResultDataReader["ID"],
                        Name = ResultDataReader["Name"].ToString(),
                        PlayerID = (int)ResultDataReader["PlayerID"],
                        PlayerName = ResultDataReader["PlayerName"].ToString(),
                        GroomID = (int)ResultDataReader["GroomID"],
                        GroomName = ResultDataReader["GroomName"].ToString(),
                        BrideID = (int)ResultDataReader["BrideID"],
                        BrideName = ResultDataReader["BrideName"].ToString(),
                        Pwd = ResultDataReader["Pwd"].ToString(),
                        AvailTime = (int)ResultDataReader["AvailTime"],
                        MaxCount = (int)ResultDataReader["MaxCount"],
                        GuestInvite = (bool)ResultDataReader["GuestInvite"],
                        MapIndex = (int)ResultDataReader["MapIndex"],
                        BeginTime = (DateTime)ResultDataReader["BeginTime"],
                        BreakTime = (DateTime)ResultDataReader["BreakTime"],
                        RoomIntroduction = ResultDataReader["RoomIntroduction"].ToString(),
                        ServerID = (int)ResultDataReader["ServerID"],
                        IsHymeneal = (bool)ResultDataReader["IsHymeneal"],
                        IsGunsaluteUsed = (bool)ResultDataReader["IsGunsaluteUsed"]
                    };
                    list.Add(item);
                }
                return list.ToArray();
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetMarryRoomInfo", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public MarryRoomInfo GetMarryRoomInfoSingle(int id)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@ID", id)
                };
                db.GetReader(ref ResultDataReader, "SP_Get_Marry_Room_Info_Single", sqlParameters);
                if (ResultDataReader.Read())
                {
                    return new MarryRoomInfo
                    {
                        ID = (int)ResultDataReader["ID"],
                        Name = ResultDataReader["Name"].ToString(),
                        PlayerID = (int)ResultDataReader["PlayerID"],
                        PlayerName = ResultDataReader["PlayerName"].ToString(),
                        GroomID = (int)ResultDataReader["GroomID"],
                        GroomName = ResultDataReader["GroomName"].ToString(),
                        BrideID = (int)ResultDataReader["BrideID"],
                        BrideName = ResultDataReader["BrideName"].ToString(),
                        Pwd = ResultDataReader["Pwd"].ToString(),
                        AvailTime = (int)ResultDataReader["AvailTime"],
                        MaxCount = (int)ResultDataReader["MaxCount"],
                        GuestInvite = (bool)ResultDataReader["GuestInvite"],
                        MapIndex = (int)ResultDataReader["MapIndex"],
                        BeginTime = (DateTime)ResultDataReader["BeginTime"],
                        BreakTime = (DateTime)ResultDataReader["BreakTime"],
                        RoomIntroduction = ResultDataReader["RoomIntroduction"].ToString(),
                        ServerID = (int)ResultDataReader["ServerID"],
                        IsHymeneal = (bool)ResultDataReader["IsHymeneal"],
                        IsGunsaluteUsed = (bool)ResultDataReader["IsGunsaluteUsed"]
                    };
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetMarryRoomInfo", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public void GetPasswordInfo(int userID, ref string PasswordQuestion1, ref string PasswordAnswer1, ref string PasswordQuestion2, ref string PasswordAnswer2, ref int Count)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", userID)
                };
                db.GetReader(ref ResultDataReader, "SP_Users_PasswordInfo", sqlParameters);
                while (ResultDataReader.Read())
                {
                    PasswordQuestion1 = ((ResultDataReader["PasswordQuestion1"] == null) ? "" : ResultDataReader["PasswordQuestion1"].ToString());
                    PasswordAnswer1 = ((ResultDataReader["PasswordAnswer1"] == null) ? "" : ResultDataReader["PasswordAnswer1"].ToString());
                    PasswordQuestion2 = ((ResultDataReader["PasswordQuestion2"] == null) ? "" : ResultDataReader["PasswordQuestion2"].ToString());
                    PasswordAnswer2 = ((ResultDataReader["PasswordAnswer2"] == null) ? "" : ResultDataReader["PasswordAnswer2"].ToString());
                    if ((DateTime)ResultDataReader["LastFindDate"] == DateTime.Today)
                    {
                        Count = (int)ResultDataReader["FailedPasswordAttemptCount"];
                    }
                    else
                    {
                        Count = 5;
                    }
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
        }

        public MarryApplyInfo[] GetPlayerMarryApply(int UserID)
        {
            SqlDataReader ResultDataReader = null;
            List<MarryApplyInfo> list = new List<MarryApplyInfo>();
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", UserID)
                };
                db.GetReader(ref ResultDataReader, "SP_Get_Marry_Apply", sqlParameters);
                while (ResultDataReader.Read())
                {
                    MarryApplyInfo item = new MarryApplyInfo
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        ApplyUserID = (int)ResultDataReader["ApplyUserID"],
                        ApplyUserName = ResultDataReader["ApplyUserName"].ToString(),
                        ApplyType = (int)ResultDataReader["ApplyType"],
                        ApplyResult = (bool)ResultDataReader["ApplyResult"],
                        LoveProclamation = ResultDataReader["LoveProclamation"].ToString(),
                        ID = (int)ResultDataReader["Id"]
                    };
                    list.Add(item);
                }
                return list.ToArray();
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetPlayerMarryApply", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public PlayerInfo[] GetPlayerMathPage(int page, int size, ref int total, ref bool resultValue)
        {
            List<PlayerInfo> list = new List<PlayerInfo>();
            try
            {
                string queryWhere = "  ";
                string fdOreder = "weeklyScore desc";
                foreach (DataRow row in GetPage("V_Sys_Users_Math", queryWhere, page, size, "*", fdOreder, "UserID", ref total).Rows)
                {
                    list.Add(new PlayerInfo
                    {
                        ID = (int)row["UserID"],
                        Colors = ((row["Colors"] == null) ? "" : row["Colors"].ToString()),
                        GP = (int)row["GP"],
                        Grade = (int)row["Grade"],
                        NickName = ((row["NickName"] == null) ? "" : row["NickName"].ToString()),
                        Sex = (bool)row["Sex"],
                        State = (int)row["State"],
                        Style = ((row["Style"] == null) ? "" : row["Style"].ToString()),
                        Hide = (int)row["Hide"],
                        Repute = (int)row["Repute"],
                        UserName = ((row["UserName"] == null) ? "" : row["UserName"].ToString()),
                        Skin = ((row["Skin"] == null) ? "" : row["Skin"].ToString()),
                        Win = (int)row["Win"],
                        Total = (int)row["Total"],
                        Nimbus = (int)row["Nimbus"],
                        FightPower = (int)row["FightPower"],
                        AchievementPoint = (int)row["AchievementPoint"],
                        typeVIP = Convert.ToByte(row["typeVIP"]),
                        VIPLevel = (int)row["VIPLevel"],
                        AddWeekLeagueScore = (int)row["weeklyScore"]
                    });
                }
                resultValue = true;
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            return list.ToArray();
        }

        public PlayerInfo[] GetPlayerPage(int page, int size, ref int total, int order, int userID, ref bool resultValue)
        {
            return GetPlayerPage(page, size, ref total, order, 0, userID, "UserID", ref resultValue);
        }

        public string GetSingleRandomName(int sex)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                if (sex > 1)
                {
                    sex = 1;
                }
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@Sex", SqlDbType.Int, 4)
                };
                array[0].Value = sex;
                db.GetReader(ref ResultDataReader, "SP_GetSingle_RandomName", array);
                if (ResultDataReader.Read())
                {
                    return (ResultDataReader["Name"] == null) ? "unknown" : ResultDataReader["Name"].ToString();
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetSingleRandomName", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public UserMatchInfo GetSingleUserMatchInfo(int UserID)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = UserID;
                db.GetReader(ref ResultDataReader, "SP_GetSingleUserMatchInfo", array);
                if (ResultDataReader.Read())
                {
                    return new UserMatchInfo
                    {
                        ID = (int)ResultDataReader["ID"],
                        UserID = (int)ResultDataReader["UserID"],
                        dailyScore = (int)ResultDataReader["dailyScore"],
                        dailyWinCount = (int)ResultDataReader["dailyWinCount"],
                        dailyGameCount = (int)ResultDataReader["dailyGameCount"],
                        DailyLeagueFirst = (bool)ResultDataReader["DailyLeagueFirst"],
                        DailyLeagueLastScore = (int)ResultDataReader["DailyLeagueLastScore"],
                        weeklyScore = (int)ResultDataReader["weeklyScore"],
                        weeklyGameCount = (int)ResultDataReader["weeklyGameCount"],
                        weeklyRanking = (int)ResultDataReader["weeklyRanking"],
                        addDayPrestge = (int)ResultDataReader["addDayPrestge"],
                        totalPrestige = (int)ResultDataReader["totalPrestige"],
                        restCount = (int)ResultDataReader["restCount"],
                        leagueGrade = (int)ResultDataReader["leagueGrade"],
                        leagueItemsGet = (int)ResultDataReader["leagueItemsGet"]
                    };
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("SP_GetSingleUserMatchInfo", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public List<UserRankInfo> GetSingleUserRank(int UserID)
        {
            SqlDataReader ResultDataReader = null;
            List<UserRankInfo> list = new List<UserRankInfo>();
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = UserID;
                db.GetReader(ref ResultDataReader, "SP_GetSingleUserRank", array);
                while (ResultDataReader.Read())
                {
                    UserRankInfo item = new UserRankInfo
                    {
                        ID = (int)ResultDataReader["ID"],
                        UserID = (int)ResultDataReader["UserID"],
                        UserRank = (string)ResultDataReader["UserRank"],
                        Attack = (int)ResultDataReader["Attack"],
                        Defence = (int)ResultDataReader["Defence"],
                        Luck = (int)ResultDataReader["Luck"],
                        Agility = (int)ResultDataReader["Agility"],
                        HP = (int)ResultDataReader["HP"],
                        Damage = (int)ResultDataReader["Damage"],
                        Guard = (int)ResultDataReader["Guard"],
                        BeginDate = (DateTime)ResultDataReader["BeginDate"],
                        Validate = (int)ResultDataReader["Validate"],
                        IsExit = (bool)ResultDataReader["IsExit"]
                    };
                    list.Add(item);
                }
                return list;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return list;
                }
                BaseBussiness.log.Error("SP_GetSingleUserRankInfo", exception);
                return list;
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
        }

        public UsersExtraInfo GetSingleUsersExtra(int UserID)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = UserID;
                db.GetReader(ref ResultDataReader, "SP_GetSingleUsersExtra", array);
                if (ResultDataReader.Read())
                {
                    return new UsersExtraInfo
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        LastTimeHotSpring = (DateTime)ResultDataReader["LastTimeHotSpring"],
                        LastFreeTimeHotSpring = (DateTime)ResultDataReader["LastFreeTimeHotSpring"],
                        MinHotSpring = (int)ResultDataReader["MinHotSpring"],
                        coupleBossEnterNum = (int)ResultDataReader["coupleBossEnterNum"],
                        coupleBossHurt = (int)ResultDataReader["coupleBossHurt"],
                        coupleBossBoxNum = (int)ResultDataReader["coupleBossBoxNum"]
                    };
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("SP_GetSingleUsersExtra", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public AchievementData[] GetUserAchievement(int userID)
        {
            List<AchievementData> list = new List<AchievementData>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = userID;
                db.GetReader(ref ResultDataReader, "SP_Get_User_AchievementData", array);
                while (ResultDataReader.Read())
                {
                    AchievementData item = new AchievementData
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        AchievementID = (int)ResultDataReader["AchievementID"],
                        IsComplete = (bool)ResultDataReader["IsComplete"],
                        CompletedDate = (DateTime)ResultDataReader["CompletedDate"]
                    };
                    list.Add(item);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public ItemInfo[] GetUserBagByType(int UserID, int bagType)
        {
            List<ItemInfo> list = new List<ItemInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[2]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4),
                    null
                };
                array[0].Value = UserID;
                array[1] = new SqlParameter("@BagType", bagType);
                db.GetReader(ref ResultDataReader, "SP_Users_BagByType", array);
                while (ResultDataReader.Read())
                {
                    list.Add(InitItem(ResultDataReader));
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public List<ItemInfo> GetUserBeadEuqip(int UserID)
        {
            List<ItemInfo> list = new List<ItemInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = UserID;
                db.GetReader(ref ResultDataReader, "SP_Users_Bead_Equip", array);
                while (ResultDataReader.Read())
                {
                    list.Add(InitItem(ResultDataReader));
                }
                return list;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return list;
                }
                BaseBussiness.log.Error("Init", exception);
                return list;
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
        }

        public BufferInfo[] GetUserBuffer(int userID)
        {
            List<BufferInfo> list = new List<BufferInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = userID;
                db.GetReader(ref ResultDataReader, "SP_User_Buff_All", array);
                while (ResultDataReader.Read())
                {
                    BufferInfo item = new BufferInfo
                    {
                        BeginDate = (DateTime)ResultDataReader["BeginDate"],
                        Data = ((ResultDataReader["Data"] == null) ? "" : ResultDataReader["Data"].ToString()),
                        Type = (int)ResultDataReader["Type"],
                        UserID = (int)ResultDataReader["UserID"],
                        ValidDate = (int)ResultDataReader["ValidDate"],
                        Value = (int)ResultDataReader["Value"],
                        IsExist = (bool)ResultDataReader["IsExist"],
                        ValidCount = (int)ResultDataReader["ValidCount"],
                        TemplateID = (int)ResultDataReader["TemplateID"],
                        IsDirty = false
                    };
                    list.Add(item);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public UsersCardInfo GetUserCardByPlace(int Place)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@Place", SqlDbType.Int, 4)
                };
                array[0].Value = Place;
                db.GetReader(ref ResultDataReader, "SP_Get_UserCard_By_Place", array);
                if (ResultDataReader.Read())
                {
                    return InitCard(ResultDataReader);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public List<UsersCardInfo> GetUserCardEuqip(int UserID)
        {
            List<UsersCardInfo> list = new List<UsersCardInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = UserID;
                db.GetReader(ref ResultDataReader, "SP_Users_Items_Card_Equip", array);
                while (ResultDataReader.Read())
                {
                    list.Add(InitCard(ResultDataReader));
                }
                return list;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return list;
                }
                BaseBussiness.log.Error("Init", exception);
                return list;
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
        }

        public UsersCardInfo[] GetUserCardSingles(int UserID)
        {
            List<UsersCardInfo> list = new List<UsersCardInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = UserID;
                db.GetReader(ref ResultDataReader, "SP_Get_UserCard_By_ID", array);
                while (ResultDataReader.Read())
                {
                    list.Add(InitCard(ResultDataReader));
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public ConsortiaBufferInfo[] GetUserConsortiaBuffer(int ConsortiaID)
        {
            List<ConsortiaBufferInfo> list = new List<ConsortiaBufferInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@ConsortiaID", SqlDbType.Int, 4)
                };
                array[0].Value = ConsortiaID;
                db.GetReader(ref ResultDataReader, "SP_User_Consortia_Buff_All", array);
                while (ResultDataReader.Read())
                {
                    list.Add(new ConsortiaBufferInfo
                    {
                        ConsortiaID = (int)ResultDataReader["ConsortiaID"],
                        BufferID = (int)ResultDataReader["BufferID"],
                        IsOpen = (bool)ResultDataReader["IsOpen"],
                        BeginDate = (DateTime)ResultDataReader["BeginDate"],
                        ValidDate = (int)ResultDataReader["ValidDate"],
                        Type = (int)ResultDataReader["Type"],
                        Value = (int)ResultDataReader["Value"],
                        Group = (int)ResultDataReader["Group"]
                    });
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init SP_User_Consortia_Buff_All", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public ConsortiaBufferInfo[] GetUserConsortiaBufferLess(int ConsortiaID, int LessID)
        {
            List<ConsortiaBufferInfo> list = new List<ConsortiaBufferInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[2]
                {
                    new SqlParameter("@ConsortiaID", SqlDbType.Int, 4),
                    null
                };
                array[0].Value = ConsortiaID;
                array[1] = new SqlParameter("@LessID", LessID);
                db.GetReader(ref ResultDataReader, "SP_User_Consortia_Buff_AllL", array);
                while (ResultDataReader.Read())
                {
                    list.Add(new ConsortiaBufferInfo
                    {
                        ConsortiaID = (int)ResultDataReader["ConsortiaID"],
                        BufferID = (int)ResultDataReader["BufferID"],
                        IsOpen = (bool)ResultDataReader["IsOpen"],
                        BeginDate = (DateTime)ResultDataReader["BeginDate"],
                        ValidDate = (int)ResultDataReader["ValidDate"],
                        Type = (int)ResultDataReader["Type"],
                        Value = (int)ResultDataReader["Value"],
                        Group = (int)ResultDataReader["Group"]
                    });
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init SP_User_Consortia_Buff_AllL", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public ConsortiaBufferInfo GetUserConsortiaBufferSingle(int ID)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@ID", SqlDbType.Int, 4)
                };
                array[0].Value = ID;
                db.GetReader(ref ResultDataReader, "SP_User_Consortia_Buff_Single", array);
                if (ResultDataReader.Read())
                {
                    return new ConsortiaBufferInfo
                    {
                        ConsortiaID = (int)ResultDataReader["ConsortiaID"],
                        BufferID = (int)ResultDataReader["BufferID"],
                        IsOpen = (bool)ResultDataReader["IsOpen"],
                        BeginDate = (DateTime)ResultDataReader["BeginDate"],
                        ValidDate = (int)ResultDataReader["ValidDate"],
                        Type = (int)ResultDataReader["Type"],
                        Value = (int)ResultDataReader["Value"],
                        Group = (int)ResultDataReader["Group"]
                    };
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init SP_User_Consortia_Buff_Single", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public List<ItemInfo> GetUserEuqip(int UserID)
        {
            List<ItemInfo> list = new List<ItemInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = UserID;
                db.GetReader(ref ResultDataReader, "SP_Users_Items_Equip", array);
                while (ResultDataReader.Read())
                {
                    list.Add(InitItem(ResultDataReader));
                }
                return list;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return list;
                }
                BaseBussiness.log.Error("Init", exception);
                return list;
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
        }

        public List<ItemInfo> GetUserEuqipByNick(string Nick)
        {
            List<ItemInfo> list = new List<ItemInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@NickName", SqlDbType.NVarChar, 200)
                };
                array[0].Value = Nick;
                db.GetReader(ref ResultDataReader, "SP_Users_Items_Equip_By_Nick", array);
                while (ResultDataReader.Read())
                {
                    list.Add(InitItem(ResultDataReader));
                }
                return list;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return list;
                }
                BaseBussiness.log.Error("Init", exception);
                return list;
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
        }

        public EventRewardProcessInfo[] GetUserEventProcess(int userID)
        {
            SqlDataReader ResultDataReader = null;
            List<EventRewardProcessInfo> list = new List<EventRewardProcessInfo>();
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = userID;
                db.GetReader(ref ResultDataReader, "SP_Get_User_EventProcess", array);
                while (ResultDataReader.Read())
                {
                    EventRewardProcessInfo item = new EventRewardProcessInfo
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        ActiveType = (int)ResultDataReader["ActiveType"],
                        Conditions = (int)ResultDataReader["Conditions"],
                        AwardGot = (int)ResultDataReader["AwardGot"]
                    };
                    list.Add(item);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public UserInfo GetUserInfo(int UserId)
        {
            SqlDataReader ResultDataReader = null;
            UserInfo userInfo = new UserInfo
            {
                UserID = UserId
            };
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", UserId)
                };
                db.GetReader(ref ResultDataReader, "SP_Get_User_Info", sqlParameters);
                while (ResultDataReader.Read())
                {
                    userInfo.UserID = int.Parse(ResultDataReader["UserID"].ToString());
                    userInfo.UserEmail = ((ResultDataReader["UserEmail"] == null) ? "" : ResultDataReader["UserEmail"].ToString());
                    userInfo.UserPhone = ((ResultDataReader["UserPhone"] == null) ? "" : ResultDataReader["UserPhone"].ToString());
                    userInfo.UserOther1 = ((ResultDataReader["UserOther1"] == null) ? "" : ResultDataReader["UserOther1"].ToString());
                    userInfo.UserOther2 = ((ResultDataReader["UserOther2"] == null) ? "" : ResultDataReader["UserOther2"].ToString());
                    userInfo.UserOther3 = ((ResultDataReader["UserOther3"] == null) ? "" : ResultDataReader["UserOther3"].ToString());
                }
                return userInfo;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return userInfo;
                }
                BaseBussiness.log.Error("Init", exception);
                return userInfo;
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
        }

        public ItemInfo[] GetUserItem(int UserID)
        {
            List<ItemInfo> list = new List<ItemInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = UserID;
                db.GetReader(ref ResultDataReader, "SP_Users_Items_All", array);
                while (ResultDataReader.Read())
                {
                    list.Add(InitItem(ResultDataReader));
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public ItemInfo GetUserItemSingle(int itemID)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@ID", SqlDbType.Int, 4)
                };
                array[0].Value = itemID;
                db.GetReader(ref ResultDataReader, "SP_Users_Items_Single", array);
                if (ResultDataReader.Read())
                {
                    return InitItem(ResultDataReader);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }


        public LevelInfo GetUserLevelSingle(int Grade)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@Grade", Grade)
                };
                db.GetReader(ref ResultDataReader, "SP_Get_Level_By_Grade", sqlParameters);
                if (ResultDataReader.Read())
                {
                    return new LevelInfo
                    {
                        Grade = (int)ResultDataReader["Grade"],
                        GP = (int)ResultDataReader["GP"],
                        Blood = (int)ResultDataReader["Blood"]
                    };
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetLevelInfoSingle", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public PlayerLimitInfo GetUserLimitByUserName(string userName)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserName", SqlDbType.NVarChar, 200)
                };
                array[0].Value = userName;
                db.GetReader(ref ResultDataReader, "SP_Users_LimitByUserName", array);
                if (ResultDataReader.Read())
                {
                    return new PlayerLimitInfo
                    {
                        ID = (int)ResultDataReader["UserID"],
                        NickName = (string)ResultDataReader["NickName"]
                    };
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public PlayerInfo[] GetUserLoginList(string userName)
        {
            List<PlayerInfo> list = new List<PlayerInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserName", SqlDbType.NVarChar, 200)
                };
                array[0].Value = userName;
                db.GetReader(ref ResultDataReader, "SP_Users_LoginList", array);
                while (ResultDataReader.Read())
                {
                    list.Add(InitPlayerInfo(ResultDataReader));
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public QuestDataInfo[] GetUserQuest(int userID)
        {
            List<QuestDataInfo> list = new List<QuestDataInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = userID;
                db.GetReader(ref ResultDataReader, "SP_QuestData_All", array);
                while (ResultDataReader.Read())
                {
                    list.Add(new QuestDataInfo
                    {
                        CompletedDate = (DateTime)ResultDataReader["CompletedDate"],
                        IsComplete = (bool)ResultDataReader["IsComplete"],
                        Condition1 = (int)ResultDataReader["Condition1"],
                        Condition2 = (int)ResultDataReader["Condition2"],
                        Condition3 = (int)ResultDataReader["Condition3"],
                        Condition4 = (int)ResultDataReader["Condition4"],
                        QuestID = (int)ResultDataReader["QuestID"],
                        UserID = (int)ResultDataReader["UserId"],
                        IsExist = (bool)ResultDataReader["IsExist"],
                        RandDobule = (int)ResultDataReader["RandDobule"],
                        RepeatFinish = (int)ResultDataReader["RepeatFinish"],
                        IsDirty = false
                    });
                }
            }
            catch (Exception exception)
            {
                BaseBussiness.log.Error("Init", exception);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public QuestDataInfo GetUserQuestSiger(int userID, int QuestID)
        {
            new QuestDataInfo();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[2]
                {
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@QuestID", SqlDbType.Int)
                };
                array[0].Value = userID;
                array[1].Value = QuestID;
                db.GetReader(ref ResultDataReader, "SP_QuestData_One", array);
                if (ResultDataReader.Read())
                {
                    return new QuestDataInfo
                    {
                        CompletedDate = (DateTime)ResultDataReader["CompletedDate"],
                        IsComplete = (bool)ResultDataReader["IsComplete"],
                        Condition1 = (int)ResultDataReader["Condition1"],
                        Condition2 = (int)ResultDataReader["Condition2"],
                        Condition3 = (int)ResultDataReader["Condition3"],
                        Condition4 = (int)ResultDataReader["Condition4"],
                        QuestID = (int)ResultDataReader["QuestID"],
                        UserID = (int)ResultDataReader["UserId"],
                        IsExist = (bool)ResultDataReader["IsExist"],
                        RandDobule = (int)ResultDataReader["RandDobule"],
                        RepeatFinish = (int)ResultDataReader["RepeatFinish"]
                    };
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public PlayerInfo GetUserSingleByNickName(string nickName)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@NickName", SqlDbType.NVarChar, 200)
                };
                array[0].Value = nickName;
                db.GetReader(ref ResultDataReader, "SP_Users_SingleByNickName", array);
                if (ResultDataReader.Read())
                {
                    return InitPlayerInfo(ResultDataReader);
                }
            }
            catch
            {
                throw new Exception();
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public PlayerInfo GetUserSingleByUserID(int UserID)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = UserID;
                db.GetReader(ref ResultDataReader, "SP_Users_SingleByUserID", array);
                if (ResultDataReader.Read())
                {
                    return InitPlayerInfo(ResultDataReader);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public PlayerInfo GetUserSingleByUserName(string userName)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserName", SqlDbType.NVarChar, 200)
                };
                array[0].Value = userName;
                db.GetReader(ref ResultDataReader, "SP_Users_SingleByUserName", array);
                if (ResultDataReader.Read())
                {
                    return InitPlayerInfo(ResultDataReader);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public TexpInfo GetUserTexpInfoSingle(int ID)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", ID)
                };
                db.GetReader(ref ResultDataReader, "SP_Get_UserTexp_By_ID", sqlParameters);
                if (ResultDataReader.Read())
                {
                    return new TexpInfo
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        attTexpExp = (int)ResultDataReader["attTexpExp"],
                        defTexpExp = (int)ResultDataReader["defTexpExp"],
                        hpTexpExp = (int)ResultDataReader["hpTexpExp"],
                        lukTexpExp = (int)ResultDataReader["lukTexpExp"],
                        spdTexpExp = (int)ResultDataReader["spdTexpExp"],
                        texpCount = (int)ResultDataReader["texpCount"],
                        texpTaskCount = (int)ResultDataReader["texpTaskCount"],
                        texpTaskDate = (DateTime)ResultDataReader["texpTaskDate"]
                    };
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetTexpInfoSingle", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public UserVIPInfo GetUserVIP(int userID)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = userID;
                db.GetReader(ref ResultDataReader, "SP_Get_User_VIP", array);
                if (ResultDataReader.Read())
                {
                    return new UserVIPInfo
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        typeVIP = Convert.ToByte(ResultDataReader["typeVIP"]),
                        VIPLevel = (int)ResultDataReader["VIPLevel"],
                        VIPExp = (int)ResultDataReader["VIPExp"],
                        VIPOnlineDays = (int)ResultDataReader["VIPOnlineDays"],
                        VIPOfflineDays = (int)ResultDataReader["VIPOfflineDays"],
                        VIPExpireDay = (DateTime)ResultDataReader["VIPExpireDay"],
                        LastVIPPackTime = (DateTime)ResultDataReader["LastVIPPackTime"],
                        VIPLastdate = (DateTime)ResultDataReader["VIPLastdate"],
                        VIPNextLevelDaysNeeded = (int)ResultDataReader["VIPNextLevelDaysNeeded"],
                        CanTakeVipReward = (bool)ResultDataReader["CanTakeVipReward"]
                    };
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public UsersCardInfo[] GetSingleUserCard(int UserID)
        {
            SqlDataReader ResultDataReader = null;
            List<UsersCardInfo> list = new List<UsersCardInfo>();
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = UserID;
                db.GetReader(ref ResultDataReader, "SP_GetSingleUserCard", array);
                while (ResultDataReader.Read())
                {
                    UsersCardInfo item = InitCard(ResultDataReader);
                    list.Add(item);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetSingleUserCard", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public int GetVip(string UserName)
        {
            int result = 0;
            try
            {
                SqlParameter[] array = new SqlParameter[2]
                {
                    new SqlParameter("@UserName", UserName),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[1].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_GetVip", array);
                result = (int)array[1].Value;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public AuctionInfo InitAuctionInfo(SqlDataReader reader)
        {
            return new AuctionInfo
            {
                AuctioneerID = (int)reader["AuctioneerID"],
                AuctioneerName = ((reader["AuctioneerName"] == null) ? "" : reader["AuctioneerName"].ToString()),
                AuctionID = (int)reader["AuctionID"],
                BeginDate = (DateTime)reader["BeginDate"],
                BuyerID = (int)reader["BuyerID"],
                BuyerName = ((reader["BuyerName"] == null) ? "" : reader["BuyerName"].ToString()),
                IsExist = (bool)reader["IsExist"],
                ItemID = (int)reader["ItemID"],
                Mouthful = (int)reader["Mouthful"],
                PayType = (int)reader["PayType"],
                Price = (int)reader["Price"],
                Rise = (int)reader["Rise"],
                ValidDate = (int)reader["ValidDate"],
                Name = reader["Name"].ToString(),
                Category = (int)reader["Category"],
                goodsCount = (int)reader["goodsCount"]
            };
        }

        private UsersCardInfo InitCard(SqlDataReader sqlDataReader_0)
        {
            return new UsersCardInfo
            {
                CardID = (int)sqlDataReader_0["CardID"],
                UserID = (int)sqlDataReader_0["UserID"],
                TemplateID = (int)sqlDataReader_0["TemplateID"],
                Place = (int)sqlDataReader_0["Place"],
                Count = (int)sqlDataReader_0["Count"],
                Attack = (int)sqlDataReader_0["Attack"],
                Defence = (int)sqlDataReader_0["Defence"],
                Agility = (int)sqlDataReader_0["Agility"],
                Luck = (int)sqlDataReader_0["Luck"],
                AttackReset = (int)sqlDataReader_0["AttackReset"],
                DefenceReset = (int)sqlDataReader_0["DefenceReset"],
                AgilityReset = (int)sqlDataReader_0["AgilityReset"],
                LuckReset = (int)sqlDataReader_0["LuckReset"],
                Guard = (int)sqlDataReader_0["Guard"],
                Damage = (int)sqlDataReader_0["Damage"],
                Level = (int)sqlDataReader_0["Level"],
                CardGP = (int)sqlDataReader_0["CardGP"],
                isFirstGet = (bool)sqlDataReader_0["isFirstGet"]
            };
        }

        public CardGrooveUpdateInfo InitCardGrooveUpdate(SqlDataReader reader)
        {
            return new CardGrooveUpdateInfo
            {
                ID = (int)reader["ID"],
                Attack = (int)reader["Attack"],
                Defend = (int)reader["Defend"],
                Agility = (int)reader["Agility"],
                Lucky = (int)reader["Lucky"],
                Damage = (int)reader["Damage"],
                Guard = (int)reader["Guard"],
                Level = (int)reader["Level"],
                Type = (int)reader["Type"],
                Exp = (int)reader["Exp"]
            };
        }

        public CardTemplateInfo InitCardTemplate(SqlDataReader reader)
        {
            return new CardTemplateInfo
            {
                ID = (int)reader["ID"],
                CardID = (int)reader["CardID"],
                Count = (int)reader["Count"],
                probability = (int)reader["probability"],
                AttackRate = (int)reader["Attack"],
                AddAttack = (int)reader["AddAttack"],
                DefendRate = (int)reader["DefendRate"],
                AddDefend = (int)reader["AddDefend"],
                AgilityRate = (int)reader["AgilityRate"],
                AddAgility = (int)reader["AddAgility"],
                LuckyRate = (int)reader["LuckyRate"],
                AddLucky = (int)reader["AddLucky"],
                DamageRate = (int)reader["DamageRate"],
                AddDamage = (int)reader["AddDamage"],
                GuardRate = (int)reader["GuardRate"],
                AddGuard = (int)reader["AddGuard"]
            };
        }

        public ConsortiaUserInfo InitConsortiaUserInfo(SqlDataReader dr)
        {
            ConsortiaUserInfo consortiaUserInfo = new ConsortiaUserInfo();
            consortiaUserInfo.ID = (int)dr["ID"];
            consortiaUserInfo.ConsortiaID = (int)dr["ConsortiaID"];
            consortiaUserInfo.DutyID = (int)dr["DutyID"];
            consortiaUserInfo.DutyName = dr["DutyName"].ToString();
            consortiaUserInfo.IsExist = (bool)dr["IsExist"];
            consortiaUserInfo.RatifierID = (int)dr["RatifierID"];
            consortiaUserInfo.RatifierName = dr["RatifierName"].ToString();
            consortiaUserInfo.Remark = dr["Remark"].ToString();
            consortiaUserInfo.UserID = (int)dr["UserID"];
            consortiaUserInfo.UserName = dr["UserName"].ToString();
            consortiaUserInfo.Grade = (int)dr["Grade"];
            consortiaUserInfo.GP = (int)dr["GP"];
            consortiaUserInfo.Repute = (int)dr["Repute"];
            consortiaUserInfo.State = (int)dr["State"];
            consortiaUserInfo.Right = (int)dr["Right"];
            consortiaUserInfo.Offer = (int)dr["Offer"];
            consortiaUserInfo.Colors = dr["Colors"].ToString();
            consortiaUserInfo.Style = dr["Style"].ToString();
            consortiaUserInfo.Hide = (int)dr["Hide"];
            consortiaUserInfo.Skin = ((dr["Skin"] == null) ? "" : consortiaUserInfo.Skin);
            consortiaUserInfo.Level = (int)dr["Level"];
            consortiaUserInfo.LastDate = (DateTime)dr["LastDate"];
            consortiaUserInfo.Sex = (bool)dr["Sex"];
            consortiaUserInfo.IsBanChat = (bool)dr["IsBanChat"];
            consortiaUserInfo.Win = (int)dr["Win"];
            consortiaUserInfo.Total = (int)dr["Total"];
            consortiaUserInfo.Escape = (int)dr["Escape"];
            consortiaUserInfo.RichesOffer = (int)dr["RichesOffer"];
            consortiaUserInfo.RichesRob = (int)dr["RichesRob"];
            consortiaUserInfo.LoginName = ((dr["LoginName"] == null) ? "" : dr["LoginName"].ToString());
            consortiaUserInfo.Nimbus = (int)dr["Nimbus"];
            consortiaUserInfo.FightPower = (int)dr["FightPower"];
            consortiaUserInfo.typeVIP = Convert.ToByte(dr["typeVIP"]);
            consortiaUserInfo.VIPLevel = (int)dr["VIPLevel"];
            return consortiaUserInfo;
        }

        public ItemInfo InitItem(SqlDataReader reader)
        {
            ItemInfo itemInfo = new ItemInfo(ItemMgr.FindItemTemplate((int)reader["TemplateID"]));
            itemInfo.AgilityCompose = (int)reader["AgilityCompose"];
            itemInfo.AttackCompose = (int)reader["AttackCompose"];
            itemInfo.Color = reader["Color"].ToString();
            itemInfo.Count = (int)reader["Count"];
            itemInfo.DefendCompose = (int)reader["DefendCompose"];
            itemInfo.ItemID = (int)reader["ItemID"];
            itemInfo.LuckCompose = (int)reader["LuckCompose"];
            itemInfo.Place = (int)reader["Place"];
            itemInfo.StrengthenLevel = (int)reader["StrengthenLevel"];
            itemInfo.TemplateID = (int)reader["TemplateID"];
            itemInfo.UserID = (int)reader["UserID"];
            itemInfo.ValidDate = (int)reader["ValidDate"];
            itemInfo.IsDirty = false;
            itemInfo.IsExist = (bool)reader["IsExist"];
            itemInfo.IsBinds = (bool)reader["IsBinds"];
            itemInfo.IsUsed = (bool)reader["IsUsed"];
            itemInfo.BeginDate = (DateTime)reader["BeginDate"];
            itemInfo.IsJudge = (bool)reader["IsJudge"];
            itemInfo.BagType = (int)reader["BagType"];
            itemInfo.Skin = reader["Skin"].ToString();
            itemInfo.RemoveDate = (DateTime)reader["RemoveDate"];
            itemInfo.RemoveType = (int)reader["RemoveType"];
            itemInfo.Hole1 = (int)reader["Hole1"];
            itemInfo.Hole2 = (int)reader["Hole2"];
            itemInfo.Hole3 = (int)reader["Hole3"];
            itemInfo.Hole4 = (int)reader["Hole4"];
            itemInfo.Hole5 = (int)reader["Hole5"];
            itemInfo.Hole6 = (int)reader["Hole6"];
            itemInfo.Hole5Level = (int)reader["Hole5Level"];
            itemInfo.Hole5Exp = (int)reader["Hole5Exp"];
            itemInfo.Hole6Level = (int)reader["Hole6Level"];
            itemInfo.Hole6Exp = (int)reader["Hole6Exp"];
            itemInfo.StrengthenTimes = (int)reader["StrengthenTimes"];
            itemInfo.goldBeginTime = (DateTime)reader["goldBeginTime"];
            itemInfo.goldValidDate = (int)reader["goldValidDate"];
            itemInfo.StrengthenExp = (int)reader["StrengthenExp"];
            itemInfo.GoldEquip = ItemMgr.FindGoldItemTemplate(itemInfo.TemplateID, itemInfo.isGold);
            itemInfo.IsDirty = false;
            return itemInfo;
        }

        public MailInfo InitMail(SqlDataReader reader)
        {
            return new MailInfo
            {
                Annex1 = reader["Annex1"].ToString(),
                Annex2 = reader["Annex2"].ToString(),
                Content = reader["Content"].ToString(),
                Gold = (int)reader["Gold"],
                ID = (int)reader["ID"],
                IsExist = (bool)reader["IsExist"],
                Money = (int)reader["Money"],
                GiftToken = (int)reader["GiftToken"],
                Receiver = reader["Receiver"].ToString(),
                ReceiverID = (int)reader["ReceiverID"],
                Sender = reader["Sender"].ToString(),
                SenderID = (int)reader["SenderID"],
                Title = reader["Title"].ToString(),
                Type = (int)reader["Type"],
                ValidDate = (int)reader["ValidDate"],
                IsRead = (bool)reader["IsRead"],
                SendTime = (DateTime)reader["SendTime"],
                Annex1Name = ((reader["Annex1Name"] == null) ? "" : reader["Annex1Name"].ToString()),
                Annex2Name = ((reader["Annex2Name"] == null) ? "" : reader["Annex2Name"].ToString()),
                Annex3 = reader["Annex3"].ToString(),
                Annex4 = reader["Annex4"].ToString(),
                Annex5 = reader["Annex5"].ToString(),
                Annex3Name = ((reader["Annex3Name"] == null) ? "" : reader["Annex3Name"].ToString()),
                Annex4Name = ((reader["Annex4Name"] == null) ? "" : reader["Annex4Name"].ToString()),
                Annex5Name = ((reader["Annex5Name"] == null) ? "" : reader["Annex5Name"].ToString()),
                AnnexRemark = ((reader["AnnexRemark"] == null) ? "" : reader["AnnexRemark"].ToString())
            };
        }

        public PlayerInfo InitPlayerInfo(SqlDataReader reader)
        {
            PlayerInfo obj = new PlayerInfo
            {
                Password = (string)reader["Password"],
                IsConsortia = (bool)reader["IsConsortia"],
                Agility = (int)reader["Agility"],
                Attack = (int)reader["Attack"],
                hp = (int)reader["hp"],
                Colors = ((reader["Colors"] == null) ? "" : reader["Colors"].ToString()),
                ConsortiaID = (int)reader["ConsortiaID"],
                Defence = (int)reader["Defence"],
                Gold = (int)reader["Gold"],
                GP = (int)reader["GP"],
                Grade = (int)reader["Grade"],
                ID = (int)reader["UserID"],
                Luck = (int)reader["Luck"],
                Money = (int)reader["Money"],
                NickName = (((string)reader["NickName"] == null) ? "" : ((string)reader["NickName"])),
                Sex = (bool)reader["Sex"],
                State = (int)reader["State"],
                Style = ((reader["Style"] == null) ? "" : reader["Style"].ToString()),
                Hide = (int)reader["Hide"],
                Repute = (int)reader["Repute"],
                UserName = ((reader["UserName"] == null) ? "" : reader["UserName"].ToString()),
                ConsortiaName = ((reader["ConsortiaName"] == null) ? "" : reader["ConsortiaName"].ToString()),
                Offer = (int)reader["Offer"],
                Win = (int)reader["Win"],
                Total = (int)reader["Total"],
                Escape = (int)reader["Escape"],
                Skin = ((reader["Skin"] == null) ? "" : reader["Skin"].ToString()),
                IsBanChat = (bool)reader["IsBanChat"],
                ReputeOffer = (int)reader["ReputeOffer"],
                ConsortiaRepute = (int)reader["ConsortiaRepute"],
                ConsortiaLevel = (int)reader["ConsortiaLevel"],
                StoreLevel = (int)reader["StoreLevel"],
                ShopLevel = (int)reader["ShopLevel"],
                SmithLevel = (int)reader["SmithLevel"],
                ConsortiaHonor = (int)reader["ConsortiaHonor"],
                RichesOffer = (int)reader["RichesOffer"],
                RichesRob = (int)reader["RichesRob"],
                AntiAddiction = (int)reader["AntiAddiction"],
                DutyLevel = (int)reader["DutyLevel"],
                DutyName = ((reader["DutyName"] == null) ? "" : reader["DutyName"].ToString()),
                Right = (int)reader["Right"],
                ChairmanName = ((reader["ChairmanName"] == null) ? "" : reader["ChairmanName"].ToString()),
                AddDayGP = (int)reader["AddDayGP"],
                AddDayOffer = (int)reader["AddDayOffer"],
                AddWeekGP = (int)reader["AddWeekGP"],
                AddWeekOffer = (int)reader["AddWeekOffer"],
                ConsortiaRiches = (int)reader["ConsortiaRiches"],
                CheckCount = (int)reader["CheckCount"],
                IsMarried = (bool)reader["IsMarried"],
                SpouseID = (int)reader["SpouseID"],
                SpouseName = ((reader["SpouseName"] == null) ? "" : reader["SpouseName"].ToString()),
                MarryInfoID = (int)reader["MarryInfoID"],
                IsCreatedMarryRoom = (bool)reader["IsCreatedMarryRoom"],
                DayLoginCount = (int)reader["DayLoginCount"],
                PasswordTwo = ((reader["PasswordTwo"] == null) ? "" : reader["PasswordTwo"].ToString()),
                SelfMarryRoomID = (int)reader["SelfMarryRoomID"],
                IsGotRing = (bool)reader["IsGotRing"],
                Rename = (bool)reader["Rename"],
                ConsortiaRename = (bool)reader["ConsortiaRename"],
                IsDirty = false,
                IsFirst = (int)reader["IsFirst"],
                Nimbus = (int)reader["Nimbus"],
                LastAward = (DateTime)reader["LastAward"],
                GiftToken = (int)reader["GiftToken"],
                QuestSite = ((reader["QuestSite"] == null) ? new byte[200] : ((byte[])reader["QuestSite"])),
                PvePermission = ((reader["PvePermission"] == null) ? "" : reader["PvePermission"].ToString()),
                FightPower = (int)reader["FightPower"],
                PasswordQuest1 = ((reader["PasswordQuestion1"] == null) ? "" : reader["PasswordQuestion1"].ToString()),
                PasswordQuest2 = ((reader["PasswordQuestion2"] == null) ? "" : reader["PasswordQuestion2"].ToString())
            };
            PlayerInfo playerInfo = obj;
            if ((DateTime)reader["LastFindDate"] != DateTime.Today.Date)
            {
                playerInfo.FailedPasswordAttemptCount = 5;
            }
            else
            {
                playerInfo.FailedPasswordAttemptCount = (int)reader["FailedPasswordAttemptCount"];
            }
            obj.AnswerSite = (int)reader["AnswerSite"];
            obj.medal = (int)reader["Medal"];
            obj.ChatCount = (int)reader["ChatCount"];
            obj.SpaPubGoldRoomLimit = (int)reader["SpaPubGoldRoomLimit"];
            obj.LastSpaDate = (DateTime)reader["LastSpaDate"];
            obj.FightLabPermission = (string)reader["FightLabPermission"];
            obj.SpaPubMoneyRoomLimit = (int)reader["SpaPubMoneyRoomLimit"];
            obj.IsInSpaPubGoldToday = (bool)reader["IsInSpaPubGoldToday"];
            obj.IsInSpaPubMoneyToday = (bool)reader["IsInSpaPubMoneyToday"];
            obj.AchievementPoint = (int)reader["AchievementPoint"];
            obj.LastWeekly = (DateTime)reader["LastWeekly"];
            obj.LastWeeklyVersion = (int)reader["LastWeeklyVersion"];
            obj.badgeID = (int)reader["BadgeID"];
            obj.typeVIP = Convert.ToByte(reader["typeVIP"]);
            obj.VIPLevel = (int)reader["VIPLevel"];
            obj.VIPExp = (int)reader["VIPExp"];
            obj.VIPExpireDay = (DateTime)reader["VIPExpireDay"];
            obj.VIPNextLevelDaysNeeded = (int)reader["VIPNextLevelDaysNeeded"];
            obj.LastVIPPackTime = (DateTime)reader["LastVIPPackTime"];
            obj.CanTakeVipReward = (bool)reader["CanTakeVipReward"];
            obj.WeaklessGuildProgressStr = (string)reader["WeaklessGuildProgressStr"];
            obj.IsOldPlayer = (bool)reader["IsOldPlayer"];
            obj.LastDate = (DateTime)reader["LastDate"];
            obj.VIPLastDate = (DateTime)reader["VIPLastDate"];
            obj.Score = (int)reader["Score"];
            obj.OptionOnOff = (int)reader["OptionOnOff"];
            obj.isOldPlayerHasValidEquitAtLogin = (bool)reader["isOldPlayerHasValidEquitAtLogin"];
            obj.badLuckNumber = (int)reader["badLuckNumber"];
            obj.OnlineTime = (int)reader["OnlineTime"];
            obj.luckyNum = (int)reader["luckyNum"];
            obj.lastLuckyNumDate = (DateTime)reader["lastLuckyNumDate"];
            obj.lastLuckNum = (int)reader["lastLuckNum"];
            obj.IsShowConsortia = (bool)reader["IsShowConsortia"];
            obj.NewDay = (DateTime)reader["NewDay"];
            obj.Honor = (string)reader["Honor"];
            obj.BoxGetDate = (DateTime)reader["BoxGetDate"];
            obj.AlreadyGetBox = (int)reader["AlreadyGetBox"];
            obj.BoxProgression = (int)reader["BoxProgression"];
            obj.GetBoxLevel = (int)reader["GetBoxLevel"];
            obj.IsRecharged = (bool)reader["IsRecharged"];
            obj.IsGetAward = (bool)reader["IsGetAward"];
            obj.apprenticeshipState = (int)reader["apprenticeshipState"];
            obj.masterID = (int)reader["masterID"];
            obj.masterOrApprentices = ((reader["masterOrApprentices"] == DBNull.Value) ? "" : ((string)reader["masterOrApprentices"]));
            obj.graduatesCount = (int)reader["graduatesCount"];
            obj.honourOfMaster = ((reader["honourOfMaster"] == DBNull.Value) ? "" : ((string)reader["honourOfMaster"]));
            obj.freezesDate = ((reader["freezesDate"] == DBNull.Value) ? DateTime.Now : ((DateTime)reader["freezesDate"]));
            obj.charmGP = ((reader["charmGP"] != DBNull.Value) ? ((int)reader["charmGP"]) : 0);
            obj.evolutionGrade = (int)reader["evolutionGrade"];
            obj.evolutionExp = (int)reader["evolutionExp"];
            obj.hardCurrency = (int)reader["hardCurrency"];
            obj.EliteScore = (int)reader["EliteScore"];
            obj.ShopFinallyGottenTime = ((reader["ShopFinallyGottenTime"] == DBNull.Value) ? DateTime.Now.AddDays(-1.0) : ((DateTime)reader["ShopFinallyGottenTime"]));
            obj.MoneyLock = ((reader["MoneyLock"] != DBNull.Value) ? ((int)reader["MoneyLock"]) : 0);
            return obj;
        }

        public bool InsertMarryRoomInfo(MarryRoomInfo info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] array = new SqlParameter[20]
                {
                    new SqlParameter("@ID", info.ID),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                };
                array[0].Direction = ParameterDirection.InputOutput;
                array[1] = new SqlParameter("@Name", info.Name);
                array[2] = new SqlParameter("@PlayerID", info.PlayerID);
                array[3] = new SqlParameter("@PlayerName", info.PlayerName);
                array[4] = new SqlParameter("@GroomID", info.GroomID);
                array[5] = new SqlParameter("@GroomName", info.GroomName);
                array[6] = new SqlParameter("@BrideID", info.BrideID);
                array[7] = new SqlParameter("@BrideName", info.BrideName);
                array[8] = new SqlParameter("@Pwd", info.Pwd);
                array[9] = new SqlParameter("@AvailTime", info.AvailTime);
                array[10] = new SqlParameter("@MaxCount", info.MaxCount);
                array[11] = new SqlParameter("@GuestInvite", info.GuestInvite);
                array[12] = new SqlParameter("@MapIndex", info.MapIndex);
                array[13] = new SqlParameter("@BeginTime", info.BeginTime);
                array[14] = new SqlParameter("@BreakTime", info.BreakTime);
                array[15] = new SqlParameter("@RoomIntroduction", info.RoomIntroduction);
                array[16] = new SqlParameter("@ServerID", info.ServerID);
                array[17] = new SqlParameter("@IsHymeneal", info.IsHymeneal);
                array[18] = new SqlParameter("@IsGunsaluteUsed", info.IsGunsaluteUsed);
                array[19] = new SqlParameter("@Result", SqlDbType.Int);
                array[19].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Insert_Marry_Room_Info", array);
                flag = ((int)array[19].Value == 0);
                if (!flag)
                {
                    return flag;
                }
                info.ID = (int)array[0].Value;
                return flag;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return flag;
                }
                BaseBussiness.log.Error("InsertMarryRoomInfo", exception);
                return flag;
            }
        }

        public bool InsertPlayerMarryApply(MarryApplyInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[7]
                {
                    new SqlParameter("@UserID", info.UserID),
                    new SqlParameter("@ApplyUserID", info.ApplyUserID),
                    new SqlParameter("@ApplyUserName", info.ApplyUserName),
                    new SqlParameter("@ApplyType", info.ApplyType),
                    new SqlParameter("@ApplyResult", info.ApplyResult),
                    new SqlParameter("@LoveProclamation", info.LoveProclamation),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[6].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Insert_Marry_Apply", array);
                result = ((int)array[6].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("InsertPlayerMarryApply", exception);
                return result;
            }
        }

        public bool InsertUserTexpInfo(TexpInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[10]
                {
                    new SqlParameter("@UserID", info.UserID),
                    new SqlParameter("@attTexpExp", info.attTexpExp),
                    new SqlParameter("@defTexpExp", info.defTexpExp),
                    new SqlParameter("@hpTexpExp", info.hpTexpExp),
                    new SqlParameter("@lukTexpExp", info.lukTexpExp),
                    new SqlParameter("@spdTexpExp", info.spdTexpExp),
                    new SqlParameter("@texpCount", info.texpCount),
                    new SqlParameter("@texpTaskCount", info.texpTaskCount),
                    new SqlParameter("@texpTaskDate", info.texpTaskDate.ToString("yyyy-MM-dd HH:mm:ss")),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[9].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_UserTexp_Add", array);
                result = ((int)array[9].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("InsertTexpInfo", exception);
                return result;
            }
        }

        public int PullDown(int activeID, string awardID, int userID, ref string msg)
        {
            int num = 1;
            try
            {
                SqlParameter[] array = new SqlParameter[4]
                {
                    new SqlParameter("@ActiveID", activeID),
                    new SqlParameter("@AwardID", awardID),
                    new SqlParameter("@UserID", userID),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[3].Direction = ParameterDirection.ReturnValue;
                if (db.RunProcedure("SP_Active_PullDown", array))
                {
                    num = (int)array[3].Value;
                    switch (num)
                    {
                        case 0:
                            msg = "ActiveBussiness.Msg0";
                            return num;
                        case 1:
                            msg = "ActiveBussiness.Msg1";
                            return num;
                        case 2:
                            msg = "ActiveBussiness.Msg2";
                            return num;
                        case 3:
                            msg = "ActiveBussiness.Msg3";
                            return num;
                        case 4:
                            msg = "ActiveBussiness.Msg4";
                            return num;
                        case 5:
                            msg = "ActiveBussiness.Msg5";
                            return num;
                        case 6:
                            msg = "ActiveBussiness.Msg6";
                            return num;
                        case 7:
                            msg = "ActiveBussiness.Msg7";
                            return num;
                        case 8:
                            msg = "ActiveBussiness.Msg8";
                            return num;
                        default:
                            msg = "ActiveBussiness.Msg9";
                            return num;
                    }
                }
                return num;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return num;
                }
                BaseBussiness.log.Error("Init", exception);
                return num;
            }
        }

        public bool AddActiveNumber(string AwardID, int ActiveID)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[3]
                {
                    new SqlParameter("@AwardID", AwardID),
                    new SqlParameter("@ActiveID", ActiveID),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[2].Direction = ParameterDirection.ReturnValue;
                result = db.RunProcedure("SP_Active_Number_Add", array);
                result = ((int)array[2].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public PlayerInfo LoginGame(string username, string password)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[2]
                {
                    new SqlParameter("@UserName", username),
                    new SqlParameter("@Password", password)
                };
                db.GetReader(ref ResultDataReader, "SP_Users_Login", sqlParameters);
                if (ResultDataReader.Read())
                {
                    return InitPlayerInfo(ResultDataReader);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public PlayerInfo LoginGame(string username, ref int isFirst, ref bool isExist, ref bool isError, bool firstValidate, ref DateTime forbidDate, string nickname)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[4]
                {
                    new SqlParameter("@UserName", username),
                    new SqlParameter("@Password", ""),
                    new SqlParameter("@FirstValidate", firstValidate),
                    new SqlParameter("@Nickname", nickname)
                };
                db.GetReader(ref ResultDataReader, "SP_Users_LoginWeb", sqlParameters);
                if (ResultDataReader.Read())
                {
                    isFirst = (int)ResultDataReader["IsFirst"];
                    isExist = (bool)ResultDataReader["IsExist"];
                    forbidDate = (DateTime)ResultDataReader["ForbidDate"];
                    if (isFirst > 1)
                    {
                        isFirst--;
                    }
                    return InitPlayerInfo(ResultDataReader);
                }
            }
            catch (Exception exception)
            {
                isError = true;
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public PlayerInfo LoginGame(string username, ref int isFirst, ref bool isExist, ref bool isError, bool firstValidate, ref DateTime forbidDate, ref string nickname, string ActiveIP)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[5]
                {
                    new SqlParameter("@UserName", username),
                    new SqlParameter("@Password", ""),
                    new SqlParameter("@FirstValidate", firstValidate),
                    new SqlParameter("@Nickname", nickname),
                    new SqlParameter("@ActiveIP", ActiveIP)
                };
                db.GetReader(ref ResultDataReader, "SP_Users_LoginWeb", sqlParameters);
                if (ResultDataReader.Read())
                {
                    isFirst = (int)ResultDataReader["IsFirst"];
                    isExist = (bool)ResultDataReader["IsExist"];
                    forbidDate = (DateTime)ResultDataReader["ForbidDate"];
                    nickname = (string)ResultDataReader["NickName"];
                    if (isFirst > 1)
                    {
                        isFirst--;
                    }
                    return InitPlayerInfo(ResultDataReader);
                }
            }
            catch (Exception exception)
            {
                isError = true;
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public bool RegisterPlayer(string userName, string passWord, string nickName, string bStyle, string gStyle, string armColor, string hairColor, string faceColor, string clothColor, string hatColor, int sex, ref string msg, int validDate)
        {
            bool result = false;
            try
            {
                string[] array = bStyle.Split(',');
                string[] array2 = gStyle.Split(',');
                SqlParameter[] array3 = new SqlParameter[21]
                {
                    new SqlParameter("@UserName", userName),
                    new SqlParameter("@PassWord", passWord),
                    new SqlParameter("@NickName", nickName),
                    new SqlParameter("@BArmID", int.Parse(array[0])),
                    new SqlParameter("@BHairID", int.Parse(array[1])),
                    new SqlParameter("@BFaceID", int.Parse(array[2])),
                    new SqlParameter("@BClothID", int.Parse(array[3])),
                    new SqlParameter("@BHatID", int.Parse(array[4])),
                    new SqlParameter("@GArmID", int.Parse(array2[0])),
                    new SqlParameter("@GHairID", int.Parse(array2[1])),
                    new SqlParameter("@GFaceID", int.Parse(array2[2])),
                    new SqlParameter("@GClothID", int.Parse(array2[3])),
                    new SqlParameter("@GHatID", int.Parse(array2[4])),
                    new SqlParameter("@ArmColor", armColor),
                    new SqlParameter("@HairColor", hairColor),
                    new SqlParameter("@FaceColor", faceColor),
                    new SqlParameter("@ClothColor", clothColor),
                    new SqlParameter("@HatColor", clothColor),
                    new SqlParameter("@Sex", sex),
                    new SqlParameter("@StyleDate", validDate),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array3[20].Direction = ParameterDirection.ReturnValue;
                result = db.RunProcedure("SP_Users_RegisterNotValidate", array3);
                int num = (int)array3[20].Value;
                result = (num == 0);
                switch (num)
                {
                    case 2:
                        msg = LanguageMgr.GetTranslation("PlayerBussiness.RegisterPlayer.Msg2");
                        return result;
                    default:
                        return result;
                    case 3:
                        msg = LanguageMgr.GetTranslation("PlayerBussiness.RegisterPlayer.Msg3");
                        return result;
                }
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool RegisterUser(string UserName, string NickName, string Password, bool Sex, int Money, int GiftToken, int Gold)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[8]
                {
                    new SqlParameter("@UserName", UserName),
                    new SqlParameter("@Password", Password),
                    new SqlParameter("@NickName", NickName),
                    new SqlParameter("@Sex", Sex),
                    new SqlParameter("@Money", Money),
                    new SqlParameter("@GiftToken", GiftToken),
                    new SqlParameter("@Gold", Gold),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[7].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Account_Register", array);
                if ((int)array[7].Value != 0)
                {
                    return result;
                }
                result = true;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init Register", exception);
                return result;
            }
        }

        public bool RegisterUserInfo(UserInfo userinfo)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[6]
                {
                    new SqlParameter("@UserID", userinfo.UserID),
                    new SqlParameter("@UserEmail", userinfo.UserEmail),
                    new SqlParameter("@UserPhone", (userinfo.UserPhone == null) ? string.Empty : userinfo.UserPhone),
                    new SqlParameter("@UserOther1", (userinfo.UserOther1 == null) ? string.Empty : userinfo.UserOther1),
                    new SqlParameter("@UserOther2", (userinfo.UserOther2 == null) ? string.Empty : userinfo.UserOther2),
                    new SqlParameter("@UserOther3", (userinfo.UserOther3 == null) ? string.Empty : userinfo.UserOther3)
                };
                return db.RunProcedure("SP_User_Info_Add", sqlParameters);
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            return false;
        }

        public PlayerInfo ReLoadPlayer(int ID)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@ID", ID)
                };
                db.GetReader(ref ResultDataReader, "SP_Users_Reload", sqlParameters);
                if (ResultDataReader.Read())
                {
                    return InitPlayerInfo(ResultDataReader);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public bool RemoveIsArrange(int ID)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[2]
                {
                    new SqlParameter("@UserID", ID),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[1].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_RemoveIsArrange", array);
                result = ((int)array[1].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_RemoveIsArrange", exception);
                return result;
            }
        }

        public bool RemoveTreasureDataByUser(int ID)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[2]
                {
                    new SqlParameter("@UserID", ID),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[1].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_RemoveTreasureDataByUser", array);
                result = ((int)array[1].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_RemoveTreasureDataByUser", exception);
                return result;
            }
        }

        public bool RenameNick(string userName, string nickName, string newNickName)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[4]
                {
                    new SqlParameter("@UserName", userName),
                    new SqlParameter("@NickName", nickName),
                    new SqlParameter("@NewNickName", newNickName),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[3].Direction = ParameterDirection.ReturnValue;
                result = db.RunProcedure("SP_Users_RenameNick2", array);
                result = ((int)array[3].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("RenameNick", exception);
                return result;
            }
        }

        public bool RenameNick(string userName, string nickName, string newNickName, ref string msg)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[4]
                {
                    new SqlParameter("@UserName", userName),
                    new SqlParameter("@NickName", nickName),
                    new SqlParameter("@NewNickName", newNickName),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[3].Direction = ParameterDirection.ReturnValue;
                result = db.RunProcedure("SP_Users_RenameNick", array);
                int num = (int)array[3].Value;
                result = (num == 0);
                if (num - 4 <= 1)
                {
                    msg = LanguageMgr.GetTranslation("PlayerBussiness.RenameNick.Msg4");
                    return result;
                }
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("RenameNick", exception);
                return result;
            }
        }

        public bool ResetCommunalActive(int ActiveID, bool IsReset)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[3]
                {
                    new SqlParameter("@ActiveID", ActiveID),
                    new SqlParameter("@IsReset", IsReset),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[2].Direction = ParameterDirection.ReturnValue;
                result = db.RunProcedure("SP_ReCommunalActive", array);
                result = ((int)array[2].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init CommunalActive", exception);
                return result;
            }
        }

        public bool ResetDragonBoat()
        {
            bool result = false;
            try
            {
                result = db.RunProcedure("SP_ReDragonBoat_Data");
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init ResetDragonBoat", exception);
                return result;
            }
        }

        public bool ResetLuckStarRank()
        {
            bool result = false;
            try
            {
                result = db.RunProcedure("SP_ReLuckStar_Rank_Info");
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init ResetLuckStar", exception);
                return result;
            }
        }

        public bool SaveBuffer(BufferInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[9]
                {
                    new SqlParameter("@UserID", info.UserID),
                    new SqlParameter("@Type", info.Type),
                    new SqlParameter("@BeginDate", info.BeginDate),
                    new SqlParameter("@Data", (info.Data == null) ? "" : info.Data),
                    new SqlParameter("@IsExist", info.IsExist),
                    new SqlParameter("@ValidDate", info.ValidDate),
                    new SqlParameter("@ValidCount", info.ValidCount),
                    new SqlParameter("@Value", info.Value),
                    new SqlParameter("@TemplateID", info.TemplateID)
                };
                result = db.RunProcedure("SP_User_Buff_Add", sqlParameters);
                info.IsDirty = false;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool SaveConsortiaBuffer(ConsortiaBufferInfo info)
        {
            bool result = false;
            try
            {
                result = db.RunProcedure("SP_User_Consortia_Buff_Add", new SqlParameter[8]
                {
                    new SqlParameter("@ConsortiaID", info.ConsortiaID),
                    new SqlParameter("@BufferID", info.BufferID),
                    new SqlParameter("@IsOpen", info.IsOpen ? 1 : 0),
                    new SqlParameter("@BeginDate", info.BeginDate),
                    new SqlParameter("@ValidDate", info.ValidDate),
                    new SqlParameter("@Type ", info.Type),
                    new SqlParameter("@Value", info.Value),
                    new SqlParameter("@Group", info.Group)
                });
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool SavePlayerMarryNotice(MarryApplyInfo info, int answerId, ref int id)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[9]
                {
                    new SqlParameter("@UserID", info.UserID),
                    new SqlParameter("@ApplyUserID", info.ApplyUserID),
                    new SqlParameter("@ApplyUserName", info.ApplyUserName),
                    new SqlParameter("@ApplyType", info.ApplyType),
                    new SqlParameter("@ApplyResult", info.ApplyResult),
                    new SqlParameter("@LoveProclamation", info.LoveProclamation),
                    new SqlParameter("@AnswerId", answerId),
                    new SqlParameter("@ouototal", SqlDbType.Int),
                    null
                };
                array[7].Direction = ParameterDirection.Output;
                array[8] = new SqlParameter("@Result", SqlDbType.Int);
                array[8].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Insert_Marry_Notice", array);
                id = (int)array[7].Value;
                result = ((int)array[8].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SavePlayerMarryNotice", exception);
                return result;
            }
        }

        public bool ScanAuction(ref string noticeUserID, double cess)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[2]
                {
                    new SqlParameter("@NoticeUserID", SqlDbType.NVarChar, 4000),
                    null
                };
                array[0].Direction = ParameterDirection.Output;
                array[1] = new SqlParameter("@Cess", cess);
                db.RunProcedure("SP_Auction_Scan", array);
                noticeUserID = array[0].Value.ToString();
                result = true;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool ScanMail(ref string noticeUserID)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@NoticeUserID", SqlDbType.NVarChar, 4000)
                };
                array[0].Direction = ParameterDirection.Output;
                db.RunProcedure("SP_Mail_Scan", array);
                noticeUserID = array[0].Value.ToString();
                result = true;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool SendMail(MailInfo mail)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[29];
                array[0] = new SqlParameter("@ID", mail.ID);
                array[0].Direction = ParameterDirection.Output;
                array[1] = new SqlParameter("@Annex1", (mail.Annex1 == null) ? "" : mail.Annex1);
                array[2] = new SqlParameter("@Annex2", (mail.Annex2 == null) ? "" : mail.Annex2);
                array[3] = new SqlParameter("@Content", (mail.Content == null) ? "" : mail.Content);
                array[4] = new SqlParameter("@Gold", mail.Gold);
                array[5] = new SqlParameter("@IsExist", true);
                array[6] = new SqlParameter("@Money", mail.Money);
                array[7] = new SqlParameter("@Receiver", (mail.Receiver == null) ? "" : mail.Receiver);
                array[8] = new SqlParameter("@ReceiverID", mail.ReceiverID);
                array[9] = new SqlParameter("@Sender", (mail.Sender == null) ? "" : mail.Sender);
                array[10] = new SqlParameter("@SenderID", mail.SenderID);
                array[11] = new SqlParameter("@Title", (mail.Title == null) ? "" : mail.Title);
                array[12] = new SqlParameter("@IfDelS", false);
                array[13] = new SqlParameter("@IsDelete", false);
                array[14] = new SqlParameter("@IsDelR", false);
                array[15] = new SqlParameter("@IsRead", false);
                array[16] = new SqlParameter("@SendTime", DateTime.Now);
                array[17] = new SqlParameter("@Type", mail.Type);
                array[18] = new SqlParameter("@Annex1Name", (mail.Annex1Name == null) ? "" : mail.Annex1Name);
                array[19] = new SqlParameter("@Annex2Name", (mail.Annex2Name == null) ? "" : mail.Annex2Name);
                array[20] = new SqlParameter("@Annex3", (mail.Annex3 == null) ? "" : mail.Annex3);
                array[21] = new SqlParameter("@Annex4", (mail.Annex4 == null) ? "" : mail.Annex4);
                array[22] = new SqlParameter("@Annex5", (mail.Annex5 == null) ? "" : mail.Annex5);
                array[23] = new SqlParameter("@Annex3Name", (mail.Annex3Name == null) ? "" : mail.Annex3Name);
                array[24] = new SqlParameter("@Annex4Name", (mail.Annex4Name == null) ? "" : mail.Annex4Name);
                array[25] = new SqlParameter("@Annex5Name", (mail.Annex5Name == null) ? "" : mail.Annex5Name);
                array[26] = new SqlParameter("@ValidDate", mail.ValidDate);
                array[27] = new SqlParameter("@AnnexRemark", (mail.AnnexRemark == null) ? "" : mail.AnnexRemark);
                array[28] = new SqlParameter("@GiftToken", mail.GiftToken);
                result = db.RunProcedure("SP_Mail_Send", array);
                mail.ID = (int)array[0].Value;
                using (CenterServiceClient centerServiceClient = new CenterServiceClient())
                {
                    centerServiceClient.MailNotice(mail.ReceiverID);
                    return result;
                }
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool SendMailAndItem(MailInfo mail, ItemInfo item, ref int returnValue)
        {
            bool flag = false;
            try
            {
                SqlParameter[] array = new SqlParameter[34]
                {
                    new SqlParameter("@ItemID", item.ItemID),
                    new SqlParameter("@UserID", item.UserID),
                    new SqlParameter("@TemplateID", item.TemplateID),
                    new SqlParameter("@Place", item.Place),
                    new SqlParameter("@AgilityCompose", item.AgilityCompose),
                    new SqlParameter("@AttackCompose", item.AttackCompose),
                    new SqlParameter("@BeginDate", item.BeginDate),
                    new SqlParameter("@Color", (item.Color == null) ? "" : item.Color),
                    new SqlParameter("@Count", item.Count),
                    new SqlParameter("@DefendCompose", item.DefendCompose),
                    new SqlParameter("@IsBinds", item.IsBinds),
                    new SqlParameter("@IsExist", item.IsExist),
                    new SqlParameter("@IsJudge", item.IsJudge),
                    new SqlParameter("@LuckCompose", item.LuckCompose),
                    new SqlParameter("@StrengthenLevel", item.StrengthenLevel),
                    new SqlParameter("@ValidDate", item.ValidDate),
                    new SqlParameter("@BagType", item.BagType),
                    new SqlParameter("@ID", mail.ID),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                };
                array[17].Direction = ParameterDirection.Output;
                array[18] = new SqlParameter("@Annex1", (mail.Annex1 == null) ? "" : mail.Annex1);
                array[19] = new SqlParameter("@Annex2", (mail.Annex2 == null) ? "" : mail.Annex2);
                array[20] = new SqlParameter("@Content", (mail.Content == null) ? "" : mail.Content);
                array[21] = new SqlParameter("@Gold", mail.Gold);
                array[22] = new SqlParameter("@Money", mail.Money);
                array[23] = new SqlParameter("@Receiver", (mail.Receiver == null) ? "" : mail.Receiver);
                array[24] = new SqlParameter("@ReceiverID", mail.ReceiverID);
                array[25] = new SqlParameter("@Sender", (mail.Sender == null) ? "" : mail.Sender);
                array[26] = new SqlParameter("@SenderID", mail.SenderID);
                array[27] = new SqlParameter("@Title", (mail.Title == null) ? "" : mail.Title);
                array[28] = new SqlParameter("@IfDelS", false);
                array[29] = new SqlParameter("@IsDelete", false);
                array[30] = new SqlParameter("@IsDelR", false);
                array[31] = new SqlParameter("@IsRead", false);
                array[32] = new SqlParameter("@SendTime", DateTime.Now);
                array[33] = new SqlParameter("@Result", SqlDbType.Int);
                array[33].Direction = ParameterDirection.ReturnValue;
                flag = db.RunProcedure("SP_Admin_SendUserItem", array);
                returnValue = (int)array[33].Value;
                flag = (returnValue == 0);
                if (flag)
                {
                    using (CenterServiceClient centerServiceClient = new CenterServiceClient())
                    {
                        centerServiceClient.MailNotice(mail.ReceiverID);
                        return flag;
                    }
                }
                return flag;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return flag;
                }
                BaseBussiness.log.Error("Init", exception);
                return flag;
            }
        }

        public int SendMailAndItem(string title, string content, int userID, int gold, int money, string param)
        {
            int num = 1;
            try
            {
                SqlParameter[] array = new SqlParameter[8]
                {
                    new SqlParameter("@Title", title),
                    new SqlParameter("@Content", content),
                    new SqlParameter("@UserID", userID),
                    new SqlParameter("@Gold", gold),
                    new SqlParameter("@Money", money),
                    new SqlParameter("@GiftToken", SqlDbType.BigInt),
                    new SqlParameter("@Param", param),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[7].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Admin_SendAllItem", array);
                num = (int)array[7].Value;
                if (num == 0)
                {
                    using (CenterServiceClient centerServiceClient = new CenterServiceClient())
                    {
                        centerServiceClient.MailNotice(userID);
                        return num;
                    }
                }
                return num;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return num;
                }
                BaseBussiness.log.Error("Init", exception);
                return num;
            }
        }

        public int SendMailAndItem(string title, string content, int UserID, int templateID, int count, int validDate, int gold, int money, int StrengthenLevel, int AttackCompose, int DefendCompose, int AgilityCompose, int LuckCompose, bool isBinds)
        {
            MailInfo mail = new MailInfo
            {
                Annex1 = "",
                Content = title,
                Gold = gold,
                Money = money,
                Receiver = "",
                ReceiverID = UserID,
                Sender = "Administrators",
                SenderID = 0,
                Title = content
            };
            ItemInfo item = new ItemInfo(null)
            {
                AgilityCompose = AgilityCompose,
                AttackCompose = AttackCompose,
                BeginDate = DateTime.Now,
                Color = "",
                DefendCompose = DefendCompose,
                IsDirty = false,
                IsExist = true,
                IsJudge = true,
                LuckCompose = LuckCompose,
                StrengthenLevel = StrengthenLevel,
                TemplateID = templateID,
                ValidDate = validDate,
                Count = count,
                IsBinds = isBinds
            };
            int returnValue = 1;
            SendMailAndItem(mail, item, ref returnValue);
            return returnValue;
        }

        public int SendMailAndItemByNickName(string title, string content, string nickName, int gold, int money, string param)
        {
            PlayerInfo userSingleByNickName = GetUserSingleByNickName(nickName);
            if (userSingleByNickName != null)
            {
                return SendMailAndItem(title, content, userSingleByNickName.ID, gold, money, param);
            }
            return 2;
        }

        public int SendMailAndItemByNickName(string title, string content, string NickName, int templateID, int count, int validDate, int gold, int money, int StrengthenLevel, int AttackCompose, int DefendCompose, int AgilityCompose, int LuckCompose, bool isBinds)
        {
            PlayerInfo userSingleByNickName = GetUserSingleByNickName(NickName);
            if (userSingleByNickName != null)
            {
                return SendMailAndItem(title, content, userSingleByNickName.ID, templateID, count, validDate, gold, money, StrengthenLevel, AttackCompose, DefendCompose, AgilityCompose, LuckCompose, isBinds);
            }
            return 2;
        }

        public int SendMailAndItemByUserName(string title, string content, string userName, int gold, int money, string param)
        {
            PlayerInfo userSingleByUserName = GetUserSingleByUserName(userName);
            if (userSingleByUserName != null)
            {
                return SendMailAndItem(title, content, userSingleByUserName.ID, gold, money, param);
            }
            return 2;
        }

        public int SendMailAndItemByUserName(string title, string content, string userName, int templateID, int count, int validDate, int gold, int money, int StrengthenLevel, int AttackCompose, int DefendCompose, int AgilityCompose, int LuckCompose, bool isBinds)
        {
            PlayerInfo userSingleByUserName = GetUserSingleByUserName(userName);
            if (userSingleByUserName != null)
            {
                return SendMailAndItem(title, content, userSingleByUserName.ID, templateID, count, validDate, gold, money, StrengthenLevel, AttackCompose, DefendCompose, AgilityCompose, LuckCompose, isBinds);
            }
            return 2;
        }

        public bool SendMailAndMoney(MailInfo mail, ref int returnValue)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[18]
                {
                    new SqlParameter("@ID", mail.ID),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                };
                array[0].Direction = ParameterDirection.Output;
                array[1] = new SqlParameter("@Annex1", (mail.Annex1 == null) ? "" : mail.Annex1);
                array[2] = new SqlParameter("@Annex2", (mail.Annex2 == null) ? "" : mail.Annex2);
                array[3] = new SqlParameter("@Content", (mail.Content == null) ? "" : mail.Content);
                array[4] = new SqlParameter("@Gold", mail.Gold);
                array[5] = new SqlParameter("@IsExist", true);
                array[6] = new SqlParameter("@Money", mail.Money);
                array[7] = new SqlParameter("@Receiver", (mail.Receiver == null) ? "" : mail.Receiver);
                array[8] = new SqlParameter("@ReceiverID", mail.ReceiverID);
                array[9] = new SqlParameter("@Sender", (mail.Sender == null) ? "" : mail.Sender);
                array[10] = new SqlParameter("@SenderID", mail.SenderID);
                array[11] = new SqlParameter("@Title", (mail.Title == null) ? "" : mail.Title);
                array[12] = new SqlParameter("@IfDelS", false);
                array[13] = new SqlParameter("@IsDelete", false);
                array[14] = new SqlParameter("@IsDelR", false);
                array[15] = new SqlParameter("@IsRead", false);
                array[16] = new SqlParameter("@SendTime", DateTime.Now);
                array[17] = new SqlParameter("@Result", SqlDbType.Int);
                array[17].Direction = ParameterDirection.ReturnValue;
                result = db.RunProcedure("SP_Admin_SendUserMoney", array);
                returnValue = (int)array[17].Value;
                result = (returnValue == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool TankAll()
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[0];
                result = db.RunProcedure("SP_Tank_All", sqlParameters);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool Test(string DutyName)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@DutyName", DutyName)
                };
                result = db.RunProcedure("SP_Test1", sqlParameters);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool UpdateAuction(AuctionInfo info, double cess)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[17]
                {
                    new SqlParameter("@AuctionID", info.AuctionID),
                    new SqlParameter("@AuctioneerID", info.AuctioneerID),
                    new SqlParameter("@AuctioneerName", (info.AuctioneerName == null) ? "" : info.AuctioneerName),
                    new SqlParameter("@BeginDate", info.BeginDate),
                    new SqlParameter("@BuyerID", info.BuyerID),
                    new SqlParameter("@BuyerName", (info.BuyerName == null) ? "" : info.BuyerName),
                    new SqlParameter("@IsExist", info.IsExist),
                    new SqlParameter("@ItemID", info.ItemID),
                    new SqlParameter("@Mouthful", info.Mouthful),
                    new SqlParameter("@PayType", info.PayType),
                    new SqlParameter("@Price", info.Price),
                    new SqlParameter("@Rise", info.Rise),
                    new SqlParameter("@ValidDate", info.ValidDate),
                    new SqlParameter("@Name", info.Name),
                    new SqlParameter("@Category", info.Category),
                    null,
                    new SqlParameter("@Cess", cess)
                };
                array[15] = new SqlParameter("@Result", SqlDbType.Int);
                array[15].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Auction_Update", array);
                result = ((int)array[15].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool UpdateUsersEventProcess(EventRewardProcessInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[5]
                {
                    new SqlParameter("@UserID", info.UserID),
                    new SqlParameter("@ActiveType", info.ActiveType),
                    new SqlParameter("@Conditions", info.Conditions),
                    new SqlParameter("@AwardGot", info.AwardGot),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[4].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_UpdateUsersEventProcess", array);
                result = ((int)array[4].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                BaseBussiness.log.Error("SP_UpdateUsersEventProcess", exception);
                return result;
            }
        }

        public bool UpdateBreakTimeWhereServerStop()
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[0].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Update_Marry_Room_Info_Sever_Stop", array);
                result = ((int)array[0].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("UpdateBreakTimeWhereServerStop", exception);
                return result;
            }
        }

        public bool UpdateBuyStore(int storeId)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@StoreID", storeId)
                };
                result = db.RunProcedure("SP_Update_Buy_Store", sqlParameters);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_Update_Buy_Store", exception);
                return result;
            }
        }

        public bool ResetQuests(int UserID)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", UserID)
                };
                result = db.RunProcedure("SP_Quest_Reset", sqlParameters);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_Quest_Reset", exception);
                return result;
            }
        }

        public bool UpdateCards(UsersCardInfo item)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[19]
                {
                    new SqlParameter("@CardID", item.CardID),
                    new SqlParameter("@UserID", item.UserID),
                    new SqlParameter("@TemplateID", item.TemplateID),
                    new SqlParameter("@Place", item.Place),
                    new SqlParameter("@Count", item.Count),
                    new SqlParameter("@Attack", item.Attack),
                    new SqlParameter("@Defence", item.Defence),
                    new SqlParameter("@Agility", item.Agility),
                    new SqlParameter("@Luck", item.Luck),
                    new SqlParameter("@Guard", item.Guard),
                    new SqlParameter("@Damage", item.Damage),
                    new SqlParameter("@Level", item.Level),
                    new SqlParameter("@CardGP", item.CardGP),
                    null,
                    new SqlParameter("@AttackReset", item.AttackReset),
                    new SqlParameter("@DefenceReset", item.DefenceReset),
                    new SqlParameter("@AgilityReset", item.AgilityReset),
                    new SqlParameter("@LuckReset", item.LuckReset),
                    new SqlParameter("@isFirstGet", item.isFirstGet)
                };
                array[13] = new SqlParameter("@Result", SqlDbType.Int);
                array[13].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_UpdateUserCard", array);
                result = ((int)array[13].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_UpdateUserCard", exception);
                return result;
            }
        }

        public int Updatecash(string UserName, int cash)
        {
            int result = 3;
            try
            {
                SqlParameter[] array = new SqlParameter[3]
                {
                    new SqlParameter("@UserName", UserName),
                    new SqlParameter("@Cash", cash),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[2].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Update_Cash", array);
                result = (int)array[2].Value;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool UpdateDbAchievementDataInfo(AchievementDataInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[4]
                {
                    new SqlParameter("@UserID", info.UserID),
                    new SqlParameter("@AchievementID", info.AchievementID),
                    new SqlParameter("@IsComplete", info.IsComplete),
                    new SqlParameter("@CompletedDate", info.CompletedDate)
                };
                result = db.RunProcedure("SP_Achievement_Data_Add", sqlParameters);
                info.IsDirty = false;
                return result;
            }
            catch (Exception exception)
            {
                BaseBussiness.log.Error("Init_UpdateDbAchievementDataInfo", exception);
                return result;
            }
        }

        public List<AchievementDataInfo> GetUserAchievementData(int userID)
        {
            List<AchievementDataInfo> list = new List<AchievementDataInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = userID;
                db.GetReader(ref ResultDataReader, "SP_Achievement_Data_All", array);
                while (ResultDataReader.Read())
                {
                    list.Add(new AchievementDataInfo
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        AchievementID = (int)ResultDataReader["AchievementID"],
                        IsComplete = (bool)ResultDataReader["IsComplete"],
                        CompletedDate = (DateTime)ResultDataReader["CompletedDate"],
                        IsDirty = false
                    });
                }
                return list;
            }
            catch (Exception exception)
            {
                BaseBussiness.log.Error("Init_GetUserAchievement", exception);
                return list;
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
        }

        public List<AchievementDataInfo> GetUserAchievementData(int userID, int id)
        {
            List<AchievementDataInfo> list = new List<AchievementDataInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[2]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4),
                    new SqlParameter("@AchievementID", id)
                };
                array[0].Value = userID;
                db.GetReader(ref ResultDataReader, "SP_Achievement_Data_Single", array);
                while (ResultDataReader.Read())
                {
                    list.Add(new AchievementDataInfo
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        AchievementID = (int)ResultDataReader["AchievementID"],
                        IsComplete = (bool)ResultDataReader["IsComplete"],
                        CompletedDate = (DateTime)ResultDataReader["CompletedDate"],
                        IsDirty = false
                    });
                }
                return list;
            }
            catch (Exception exception)
            {
                BaseBussiness.log.Error("Init_GetUserAchievementSingle", exception);
                return list;
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
        }

        public List<UsersRecordInfo> GetUserRecord(int userID)
        {
            List<UsersRecordInfo> list = new List<UsersRecordInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = userID;
                db.GetReader(ref ResultDataReader, "SP_Users_Record_All", array);
                while (ResultDataReader.Read())
                {
                    list.Add(new UsersRecordInfo
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        RecordID = (int)ResultDataReader["RecordID"],
                        Total = (int)ResultDataReader["Total"],
                        IsDirty = false
                    });
                }
                return list;
            }
            catch (Exception exception)
            {
                BaseBussiness.log.Error("Init_GetUserRecord", exception);
                return list;
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
        }

        public bool UpdateDbUserRecord(UsersRecordInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[3]
                {
                    new SqlParameter("@UserID", info.UserID),
                    new SqlParameter("@RecordID", info.RecordID),
                    new SqlParameter("@Total", info.Total)
                };
                result = db.RunProcedure("SP_Users_Record_Add", sqlParameters);
                info.IsDirty = false;
                return result;
            }
            catch (Exception exception)
            {
                BaseBussiness.log.Error("Init_UpdateDbUserRecord", exception);
                return result;
            }
        }

        public bool UpdateDbQuestDataInfo(QuestDataInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[11]
                {
                    new SqlParameter("@UserID", info.UserID),
                    new SqlParameter("@QuestID", info.QuestID),
                    new SqlParameter("@CompletedDate", info.CompletedDate),
                    new SqlParameter("@IsComplete", info.IsComplete),
                    new SqlParameter("@Condition1", (info.Condition1 > -1) ? info.Condition1 : 0),
                    new SqlParameter("@Condition2", (info.Condition2 > -1) ? info.Condition2 : 0),
                    new SqlParameter("@Condition3", (info.Condition3 > -1) ? info.Condition3 : 0),
                    new SqlParameter("@Condition4", (info.Condition4 > -1) ? info.Condition4 : 0),
                    new SqlParameter("@IsExist", info.IsExist),
                    new SqlParameter("@RepeatFinish", (info.RepeatFinish == -1) ? 1 : info.RepeatFinish),
                    new SqlParameter("@RandDobule", info.RandDobule)
                };
                result = db.RunProcedure("SP_QuestData_Add", sqlParameters);
                info.IsDirty = false;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool UpdateFriendHelpTimes(int ID)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[2]
                {
                    new SqlParameter("@UserID", ID),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[1].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_UpdateFriendHelpTimes", array);
                result = ((int)array[1].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_UpdateFriendHelpTimes", exception);
                return result;
            }
        }

        public bool UpdateGoods(ItemInfo item)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[36]
                {
                    new SqlParameter("@ItemID", item.ItemID),
                    new SqlParameter("@UserID", item.UserID),
                    new SqlParameter("@TemplateID", item.Template.TemplateID),
                    new SqlParameter("@Place", item.Place),
                    new SqlParameter("@AgilityCompose", item.AgilityCompose),
                    new SqlParameter("@AttackCompose", item.AttackCompose),
                    new SqlParameter("@BeginDate", item.BeginDate),
                    new SqlParameter("@Color", (item.Color == null) ? "" : item.Color),
                    new SqlParameter("@Count", item.Count),
                    new SqlParameter("@DefendCompose", item.DefendCompose),
                    new SqlParameter("@IsBinds", item.IsBinds),
                    new SqlParameter("@IsExist", item.IsExist),
                    new SqlParameter("@IsJudge", item.IsJudge),
                    new SqlParameter("@LuckCompose", item.LuckCompose),
                    new SqlParameter("@StrengthenLevel", item.StrengthenLevel),
                    new SqlParameter("@ValidDate", item.ValidDate),
                    new SqlParameter("@BagType", item.BagType),
                    new SqlParameter("@Skin", item.Skin),
                    new SqlParameter("@IsUsed", item.IsUsed),
                    new SqlParameter("@RemoveDate", item.RemoveDate),
                    new SqlParameter("@RemoveType", item.RemoveType),
                    new SqlParameter("@Hole1", item.Hole1),
                    new SqlParameter("@Hole2", item.Hole2),
                    new SqlParameter("@Hole3", item.Hole3),
                    new SqlParameter("@Hole4", item.Hole4),
                    new SqlParameter("@Hole5", item.Hole5),
                    new SqlParameter("@Hole6", item.Hole6),
                    new SqlParameter("@StrengthenTimes", item.StrengthenTimes),
                    new SqlParameter("@Hole5Level", item.Hole5Level),
                    new SqlParameter("@Hole5Exp", item.Hole5Exp),
                    new SqlParameter("@Hole6Level", item.Hole6Level),
                    new SqlParameter("@Hole6Exp", item.Hole6Exp),
                    new SqlParameter("@IsGold", item.IsGold),
                    new SqlParameter("@goldBeginTime", item.goldBeginTime),
                    new SqlParameter("@goldValidDate", item.goldValidDate),
                    new SqlParameter("@StrengthenExp", item.StrengthenExp)
                };
                result = db.RunProcedure("SP_Users_Items_Update", sqlParameters);
                item.IsDirty = false;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool UpdateLastVIPPackTime(int ID)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[3]
                {
                    new SqlParameter("@UserID", ID),
                    new SqlParameter("@LastVIPPackTime", DateTime.Now.Date),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[2].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_UpdateUserLastVIPPackTime", array);
                result = true;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_UpdateUserLastVIPPackTime", exception);
                return result;
            }
        }

        public bool UpdateMail(MailInfo mail, int oldMoney)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[30]
                {
                    new SqlParameter("@ID", mail.ID),
                    new SqlParameter("@Annex1", (mail.Annex1 == null) ? "" : mail.Annex1),
                    new SqlParameter("@Annex2", (mail.Annex2 == null) ? "" : mail.Annex2),
                    new SqlParameter("@Content", (mail.Content == null) ? "" : mail.Content),
                    new SqlParameter("@Gold", mail.Gold),
                    new SqlParameter("@IsExist", mail.IsExist),
                    new SqlParameter("@Money", mail.Money),
                    new SqlParameter("@Receiver", (mail.Receiver == null) ? "" : mail.Receiver),
                    new SqlParameter("@ReceiverID", mail.ReceiverID),
                    new SqlParameter("@Sender", (mail.Sender == null) ? "" : mail.Sender),
                    new SqlParameter("@SenderID", mail.SenderID),
                    new SqlParameter("@Title", (mail.Title == null) ? "" : mail.Title),
                    new SqlParameter("@IfDelS", false),
                    new SqlParameter("@IsDelete", false),
                    new SqlParameter("@IsDelR", false),
                    new SqlParameter("@IsRead", mail.IsRead),
                    new SqlParameter("@SendTime", mail.SendTime),
                    new SqlParameter("@Type", mail.Type),
                    new SqlParameter("@OldMoney", oldMoney),
                    new SqlParameter("@ValidDate", mail.ValidDate),
                    new SqlParameter("@Annex1Name", mail.Annex1Name),
                    new SqlParameter("@Annex2Name", mail.Annex2Name),
                    new SqlParameter("@Result", SqlDbType.Int),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                };
                array[22].Direction = ParameterDirection.ReturnValue;
                array[23] = new SqlParameter("@Annex3", (mail.Annex3 == null) ? "" : mail.Annex3);
                array[24] = new SqlParameter("@Annex4", (mail.Annex4 == null) ? "" : mail.Annex4);
                array[25] = new SqlParameter("@Annex5", (mail.Annex5 == null) ? "" : mail.Annex5);
                array[26] = new SqlParameter("@Annex3Name", (mail.Annex3Name == null) ? "" : mail.Annex3Name);
                array[27] = new SqlParameter("@Annex4Name", (mail.Annex4Name == null) ? "" : mail.Annex4Name);
                array[28] = new SqlParameter("@Annex5Name", (mail.Annex5Name == null) ? "" : mail.Annex5Name);
                array[29] = new SqlParameter("GiftToken", mail.GiftToken);
                db.RunProcedure("SP_Mail_Update", array);
                result = ((int)array[22].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool UpdateMarryInfo(MarryInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[6]
                {
                    new SqlParameter("@ID", info.ID),
                    new SqlParameter("@UserID", info.UserID),
                    new SqlParameter("@IsPublishEquip", info.IsPublishEquip),
                    new SqlParameter("@Introduction", info.Introduction),
                    new SqlParameter("@RegistTime", info.RegistTime.ToString("yyyy-MM-dd HH:mm:ss")),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[5].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_MarryInfo_Update", array);
                result = ((int)array[5].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool UpdateMarryRoomInfo(MarryRoomInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[9]
                {
                    new SqlParameter("@ID", info.ID),
                    new SqlParameter("@AvailTime", info.AvailTime),
                    new SqlParameter("@BreakTime", info.BreakTime),
                    new SqlParameter("@roomIntroduction", info.RoomIntroduction),
                    new SqlParameter("@isHymeneal", info.IsHymeneal),
                    new SqlParameter("@Name", info.Name),
                    new SqlParameter("@Pwd", info.Pwd),
                    new SqlParameter("@IsGunsaluteUsed", info.IsGunsaluteUsed),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[8].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Update_Marry_Room_Info", array);
                result = ((int)array[8].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("UpdateMarryRoomInfo", exception);
                return result;
            }
        }

        public bool UpdatePassWord(int userID, string password)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[2]
                {
                    new SqlParameter("@UserID", userID),
                    new SqlParameter("@Password", password)
                };
                result = db.RunProcedure("SP_Users_UpdatePassword", sqlParameters);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool UpdatePasswordInfo(int userID, string PasswordQuestion1, string PasswordAnswer1, string PasswordQuestion2, string PasswordAnswer2, int Count)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[6]
                {
                    new SqlParameter("@UserID", userID),
                    new SqlParameter("@PasswordQuestion1", PasswordQuestion1),
                    new SqlParameter("@PasswordAnswer1", PasswordAnswer1),
                    new SqlParameter("@PasswordQuestion2", PasswordQuestion2),
                    new SqlParameter("@PasswordAnswer2", PasswordAnswer2),
                    new SqlParameter("@FailedPasswordAttemptCount", Count)
                };
                result = db.RunProcedure("SP_Users_Password_Add", sqlParameters);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool UpdatePasswordTwo(int userID, string passwordTwo)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[2]
                {
                    new SqlParameter("@UserID", userID),
                    new SqlParameter("@PasswordTwo", passwordTwo)
                };
                result = db.RunProcedure("SP_Users_UpdatePasswordTwo", sqlParameters);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool UpdatePlayer(PlayerInfo player)
        {
            bool flag = false;
            try
            {
                if (player.Grade < 1)
                {
                    return flag;
                }
                SqlParameter[] array = new SqlParameter[72]
                {
                    new SqlParameter("@UserID", player.ID),
                    new SqlParameter("@Attack", player.Attack),
                    new SqlParameter("@Colors", (player.Colors == null) ? "" : player.Colors),
                    new SqlParameter("@ConsortiaID", player.ConsortiaID),
                    new SqlParameter("@Defence", player.Defence),
                    new SqlParameter("@Gold", player.Gold),
                    new SqlParameter("@GP", player.GP),
                    new SqlParameter("@Grade", player.Grade),
                    new SqlParameter("@Luck", player.Luck),
                    new SqlParameter("@Money", player.Money),
                    new SqlParameter("@Style", (player.Style == null) ? "" : player.Style),
                    new SqlParameter("@Agility", player.Agility),
                    new SqlParameter("@State", player.State),
                    new SqlParameter("@Hide", player.Hide),
                    new SqlParameter("@ExpendDate", (!player.ExpendDate.HasValue) ? "" : player.ExpendDate.ToString()),
                    new SqlParameter("@Win", player.Win),
                    new SqlParameter("@Total", player.Total),
                    new SqlParameter("@Escape", player.Escape),
                    new SqlParameter("@Skin", (player.Skin == null) ? "" : player.Skin),
                    new SqlParameter("@Offer", player.Offer),
                    new SqlParameter("@AntiAddiction", player.AntiAddiction),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                };
                array[20].Direction = ParameterDirection.InputOutput;
                array[21] = new SqlParameter("@Result", SqlDbType.Int);
                array[21].Direction = ParameterDirection.ReturnValue;
                array[22] = new SqlParameter("@RichesOffer", player.RichesOffer);
                array[23] = new SqlParameter("@RichesRob", player.RichesRob);
                array[24] = new SqlParameter("@CheckCount", player.CheckCount);
                array[24].Direction = ParameterDirection.InputOutput;
                array[25] = new SqlParameter("@MarryInfoID", player.MarryInfoID);
                array[26] = new SqlParameter("@DayLoginCount", player.DayLoginCount);
                array[27] = new SqlParameter("@Nimbus", player.Nimbus);
                array[28] = new SqlParameter("@LastAward", player.LastAward);
                array[29] = new SqlParameter("@GiftToken", player.GiftToken);
                array[30] = new SqlParameter("@QuestSite", player.QuestSite);
                array[31] = new SqlParameter("@PvePermission", player.PvePermission);
                array[32] = new SqlParameter("@FightPower", player.FightPower);
                array[33] = new SqlParameter("@AnswerSite", player.AnswerSite);
                array[34] = new SqlParameter("@LastAuncherAward", player.LastAward);
                array[35] = new SqlParameter("@hp", player.hp);
                array[36] = new SqlParameter("@ChatCount", player.ChatCount);
                array[37] = new SqlParameter("@SpaPubGoldRoomLimit", player.SpaPubGoldRoomLimit);
                array[38] = new SqlParameter("@LastSpaDate", player.LastSpaDate);
                array[39] = new SqlParameter("@FightLabPermission", player.FightLabPermission);
                array[40] = new SqlParameter("@SpaPubMoneyRoomLimit", player.SpaPubMoneyRoomLimit);
                array[41] = new SqlParameter("@IsInSpaPubGoldToday", player.IsInSpaPubGoldToday);
                array[42] = new SqlParameter("@IsInSpaPubMoneyToday", player.IsInSpaPubMoneyToday);
                array[43] = new SqlParameter("@AchievementPoint", player.AchievementPoint);
                array[44] = new SqlParameter("@LastWeekly", player.LastWeekly);
                array[45] = new SqlParameter("@LastWeeklyVersion", player.LastWeeklyVersion);
                array[46] = new SqlParameter("@WeaklessGuildProgressStr", player.WeaklessGuildProgressStr);
                array[47] = new SqlParameter("@IsOldPlayer", player.IsOldPlayer);
                array[48] = new SqlParameter("@VIPLevel", player.VIPLevel);
                array[49] = new SqlParameter("@VIPExp", player.VIPExp);
                array[50] = new SqlParameter("@Score", player.Score);
                array[51] = new SqlParameter("@OptionOnOff", player.OptionOnOff);
                array[52] = new SqlParameter("@isOldPlayerHasValidEquitAtLogin", player.isOldPlayerHasValidEquitAtLogin);
                array[53] = new SqlParameter("@badLuckNumber", player.badLuckNumber);
                array[54] = new SqlParameter("@luckyNum", player.luckyNum);
                array[55] = new SqlParameter("@lastLuckyNumDate", player.lastLuckyNumDate);
                array[56] = new SqlParameter("@lastLuckNum", player.lastLuckNum);
                array[57] = new SqlParameter("@IsShowConsortia", player.IsShowConsortia);
                array[58] = new SqlParameter("@NewDay", player.NewDay);
                array[59] = new SqlParameter("@Medal", player.medal);
                array[60] = new SqlParameter("@Honor", player.Honor);
                array[61] = new SqlParameter("@VIPNextLevelDaysNeeded", player.GetVIPNextLevelDaysNeeded(player.VIPLevel, player.VIPExp));
                array[62] = new SqlParameter("@IsRecharged", player.IsRecharged);
                array[63] = new SqlParameter("@IsGetAward", player.IsGetAward);
                array[64] = new SqlParameter("@typeVIP", player.typeVIP);
                array[65] = new SqlParameter("@evolutionGrade", player.evolutionGrade);
                array[66] = new SqlParameter("@evolutionExp", player.evolutionExp);
                array[67] = new SqlParameter("@hardCurrency", player.hardCurrency);
                array[68] = new SqlParameter("@EliteScore", player.EliteScore);
                array[69] = new SqlParameter("@UseOffer", player.UseOffer);
                array[70] = new SqlParameter("@ShopFinallyGottenTime", player.ShopFinallyGottenTime);
                array[71] = new SqlParameter("@MoneyLock", player.MoneyLock);
                db.RunProcedure("SP_Users_Update", array);
                flag = ((int)array[21].Value == 0);
                if (flag)
                {
                    player.AntiAddiction = (int)array[20].Value;
                    player.CheckCount = (int)array[24].Value;
                }
                player.IsDirty = false;
                return flag;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return flag;
                }
                BaseBussiness.log.Error("Init", exception);
                return flag;
            }
        }

        public bool UpdatePlayerGotRingProp(int groomID, int brideID)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[3]
                {
                    new SqlParameter("@GroomID", groomID),
                    new SqlParameter("@BrideID", brideID),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[2].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Update_GotRing_Prop", array);
                result = ((int)array[2].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("UpdatePlayerGotRingProp", exception);
                return result;
            }
        }

        public bool UpdatePlayerLastAward(int id, int type)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[2]
                {
                    new SqlParameter("@UserID", id),
                    new SqlParameter("@Type", type)
                };
                result = db.RunProcedure("SP_Users_LastAward", sqlParameters);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("UpdatePlayerAward", exception);
                return result;
            }
        }

        public bool UpdatePlayerMarry(PlayerInfo player)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[7]
                {
                    new SqlParameter("@UserID", player.ID),
                    new SqlParameter("@IsMarried", player.IsMarried),
                    new SqlParameter("@SpouseID", player.SpouseID),
                    new SqlParameter("@SpouseName", player.SpouseName),
                    new SqlParameter("@IsCreatedMarryRoom", player.IsCreatedMarryRoom),
                    new SqlParameter("@SelfMarryRoomID", player.SelfMarryRoomID),
                    new SqlParameter("@IsGotRing", player.IsGotRing)
                };
                result = db.RunProcedure("SP_Users_Marry", sqlParameters);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("UpdatePlayerMarry", exception);
                return result;
            }
        }

        public bool UpdatePlayerMarryApply(int UserID, string loveProclamation, bool isExist)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[4]
                {
                    new SqlParameter("@UserID", UserID),
                    new SqlParameter("@LoveProclamation", loveProclamation),
                    new SqlParameter("@isExist", isExist),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[3].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Update_Marry_Apply", array);
                result = ((int)array[3].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("UpdatePlayerMarryApply", exception);
                return result;
            }
        }

        public bool UpdateUserDrillInfo(UserDrillInfo g)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[6]
                {
                    new SqlParameter("@UserID", g.UserID),
                    new SqlParameter("@BeadPlace", g.BeadPlace),
                    new SqlParameter("@HoleExp", g.HoleExp),
                    new SqlParameter("@HoleLv", g.HoleLv),
                    new SqlParameter("@DrillPlace", g.DrillPlace),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[5].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_UpdateUserDrillInfo", array);
                result = true;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_UpdateUserDrillInfo", exception);
                return result;
            }
        }

        public bool UpdateUserMatchInfo(UserMatchInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[16]
                {
                    new SqlParameter("@ID", info.ID),
                    new SqlParameter("@UserID", info.UserID),
                    new SqlParameter("@dailyScore", info.dailyScore),
                    new SqlParameter("@dailyWinCount", info.dailyWinCount),
                    new SqlParameter("@dailyGameCount", info.dailyGameCount),
                    new SqlParameter("@DailyLeagueFirst", info.DailyLeagueFirst),
                    new SqlParameter("@DailyLeagueLastScore", info.DailyLeagueLastScore),
                    new SqlParameter("@weeklyScore", info.weeklyScore),
                    new SqlParameter("@weeklyGameCount", info.weeklyGameCount),
                    new SqlParameter("@weeklyRanking", info.weeklyRanking),
                    new SqlParameter("@addDayPrestge", info.addDayPrestge),
                    new SqlParameter("@totalPrestige", info.totalPrestige),
                    new SqlParameter("@restCount", info.restCount),
                    new SqlParameter("@leagueGrade", info.leagueGrade),
                    new SqlParameter("@leagueItemsGet", info.leagueItemsGet),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[15].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_UpdateUserMatch", array);
                result = ((int)array[15].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_UpdateUserMatch", exception);
                return result;
            }
        }

        public bool UpdateUserRank(UserRankInfo item)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[14]
                {
                    new SqlParameter("@ID", item.ID),
                    new SqlParameter("@UserID", item.UserID),
                    new SqlParameter("@UserRank", item.UserRank),
                    new SqlParameter("@Attack", item.Attack),
                    new SqlParameter("@Defence", item.Defence),
                    new SqlParameter("@Luck", item.Luck),
                    new SqlParameter("@Agility", item.Agility),
                    new SqlParameter("@HP", item.HP),
                    new SqlParameter("@Damage", item.Damage),
                    new SqlParameter("@Guard", item.Guard),
                    new SqlParameter("@BeginDate", item.BeginDate),
                    new SqlParameter("@Validate", item.Validate),
                    new SqlParameter("@IsExit", item.IsExit),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[13].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_UpdateUserRank", array);
                result = ((int)array[13].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_UpdateUserRank", exception);
                return result;
            }
        }

        public bool UpdateUserExtra(UsersExtraInfo ex)
        {
            bool result = false;
            try
            {
                result = db.RunProcedure("SP_Update_User_Extra", new SqlParameter[11]
                {
                    new SqlParameter("@UserID", ex.UserID),
                    new SqlParameter("@LastTimeHotSpring", ex.LastTimeHotSpring),
                    new SqlParameter("@MinHotSpring", ex.MinHotSpring),
                    new SqlParameter("@coupleBossEnterNum", ex.coupleBossEnterNum),
                    new SqlParameter("@coupleBossHurt", ex.coupleBossHurt),
                    new SqlParameter("@coupleBossBoxNum", ex.coupleBossBoxNum),
                    new SqlParameter("@LastFreeTimeHotSpring", ex.LastFreeTimeHotSpring),
                    new SqlParameter("@isGetAwardMarry", ex.isGetAwardMarry),
                    new SqlParameter("@isFirstAwardMarry", ex.isFirstAwardMarry),
                    new SqlParameter("@LeftRoutteCount", ex.LeftRoutteCount),
                    new SqlParameter("@LeftRoutteRate", ex.LeftRoutteRate)
                });
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool UpdateUserTexpInfo(TexpInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[10]
                {
                    new SqlParameter("@UserID", info.UserID),
                    new SqlParameter("@attTexpExp", info.attTexpExp),
                    new SqlParameter("@defTexpExp", info.defTexpExp),
                    new SqlParameter("@hpTexpExp", info.hpTexpExp),
                    new SqlParameter("@lukTexpExp", info.lukTexpExp),
                    new SqlParameter("@spdTexpExp", info.spdTexpExp),
                    new SqlParameter("@texpCount", info.texpCount),
                    new SqlParameter("@texpTaskCount", info.texpTaskCount),
                    new SqlParameter("@texpTaskDate", info.texpTaskDate),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[9].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_UserTexp_Update", array);
                result = ((int)array[9].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool UpdateVIPInfo(PlayerInfo p)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[10]
                {
                    new SqlParameter("@ID", p.ID),
                    new SqlParameter("@VIPLevel", p.VIPLevel),
                    new SqlParameter("@VIPExp", p.VIPExp),
                    new SqlParameter("@VIPOnlineDays", SqlDbType.BigInt),
                    new SqlParameter("@VIPOfflineDays", SqlDbType.BigInt),
                    new SqlParameter("@VIPExpireDay", p.VIPExpireDay),
                    new SqlParameter("@VIPLastDate", DateTime.Now),
                    new SqlParameter("@VIPNextLevelDaysNeeded", p.GetVIPNextLevelDaysNeeded(p.VIPLevel, p.VIPExp)),
                    new SqlParameter("@CanTakeVipReward", p.CanTakeVipReward),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[9].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_UpdateVIPInfo", array);
                result = true;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_UpdateVIPInfo", exception);
                return result;
            }
        }

        public int VIPLastdate(int ID)
        {
            int result = 0;
            try
            {
                SqlParameter[] array = new SqlParameter[2]
                {
                    new SqlParameter("@UserID", ID),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[1].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_VIPLastdate_Single", array);
                result = (int)array[1].Value;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_VIPLastdate_Single", exception);
                return result;
            }
        }

        public int VIPRenewal(string nickName, int renewalDays, int typeVIP, ref DateTime ExpireDayOut)
        {
            int result = 0;
            try
            {
                SqlParameter[] array = new SqlParameter[5]
                {
                    new SqlParameter("@NickName", nickName),
                    new SqlParameter("@RenewalDays", renewalDays),
                    new SqlParameter("@ExpireDayOut", DateTime.Now),
                    new SqlParameter("@typeVIP", typeVIP),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[2].Direction = ParameterDirection.Output;
                array[4].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_VIPRenewal_Single", array);
                ExpireDayOut = (DateTime)array[2].Value;
                result = (int)array[4].Value;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_VIPRenewal_Single", exception);
                return result;
            }
        }
        public int VIPRenewal2(string nickName, int renewalDays, int typeVIP, ref DateTime ExpireDayOut)
        {
            int result = 0;
            try
            {
                SqlParameter[] array = new SqlParameter[5]
                {
                    new SqlParameter("@NickName", nickName),
                    new SqlParameter("@RenewalDays", renewalDays),
                    new SqlParameter("@ExpireDayOut", DateTime.Now),
                    new SqlParameter("@typeVIP", typeVIP),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[2].Direction = ParameterDirection.Output;
                array[4].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_VIPRenewal_Single2", array);
                ExpireDayOut = (DateTime)array[2].Value;
                result = (int)array[4].Value;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_VIPRenewal_Single2", exception);
                return result;
            }
        }

        public bool UpdateAcademyPlayer(PlayerInfo player)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[8]
                {
                    new SqlParameter("@UserID", player.ID),
                    new SqlParameter("@apprenticeshipState", player.apprenticeshipState),
                    new SqlParameter("@masterID", player.masterID),
                    new SqlParameter("@masterOrApprentices", player.masterOrApprentices),
                    new SqlParameter("@graduatesCount", player.graduatesCount),
                    new SqlParameter("@honourOfMaster", player.honourOfMaster),
                    null,
                    new SqlParameter("@freezesDate", player.freezesDate)
                };
                array[6] = new SqlParameter("@Result", SqlDbType.Int);
                array[6].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_UsersAcademy_Update", array);
                result = ((int)array[6].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("UpdateAcademyPlayer", exception);
                return result;
            }
        }

        public void AddDailyRecord(DailyRecordInfo info)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[3]
                {
                    new SqlParameter("@UserID", info.UserID),
                    new SqlParameter("@Type", info.Type),
                    new SqlParameter("@Value", info.Value)
                };
                db.RunProcedure("SP_DailyRecordInfo_Add", sqlParameters);
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("AddDailyRecord", exception);
                }
            }
        }

        public bool DeleteDailyRecord(int UserID, int Type)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[2]
                {
                    new SqlParameter("@UserID", UserID),
                    new SqlParameter("@Type", Type)
                };
                result = db.RunProcedure("SP_DailyRecordInfo_Delete", sqlParameters);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_DailyRecordInfo_Delete", exception);
                return result;
            }
        }

        public DailyRecordInfo[] GetDailyRecord(int UserID)
        {
            List<DailyRecordInfo> list = new List<DailyRecordInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", UserID)
                };
                db.GetReader(ref ResultDataReader, "SP_DailyRecordInfo_Single", sqlParameters);
                while (ResultDataReader.Read())
                {
                    DailyRecordInfo item = new DailyRecordInfo
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        Type = (int)ResultDataReader["Type"],
                        Value = (string)ResultDataReader["Value"]
                    };
                    list.Add(item);
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetDailyRecord", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public string GetASSInfoSingle(int UserID)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", UserID)
                };
                db.GetReader(ref ResultDataReader, "SP_ASSInfo_Single", sqlParameters);
                if (ResultDataReader.Read())
                {
                    return ResultDataReader["IDNumber"].ToString();
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetASSInfoSingle", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return "";
        }

        public DailyLogListInfo GetDailyLogListSingle(int UserID)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", UserID)
                };
                db.GetReader(ref ResultDataReader, "SP_DailyLogList_Single", sqlParameters);
                if (ResultDataReader.Read())
                {
                    return new DailyLogListInfo
                    {
                        ID = (int)ResultDataReader["ID"],
                        UserID = (int)ResultDataReader["UserID"],
                        UserAwardLog = (int)ResultDataReader["UserAwardLog"],
                        DayLog = (string)ResultDataReader["DayLog"],
                        LastDate = (DateTime)ResultDataReader["LastDate"]
                    };
                }
            }
            catch (Exception exception)
            {
                BaseBussiness.log.Error("DailyLogList", exception);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public bool UpdateDailyLogList(DailyLogListInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[5]
                {
                    new SqlParameter("@UserID", info.UserID),
                    new SqlParameter("@UserAwardLog", info.UserAwardLog),
                    new SqlParameter("@DayLog", info.DayLog),
                    new SqlParameter("@LastDate", info.LastDate),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[4].Direction = ParameterDirection.ReturnValue;
                result = db.RunProcedure("SP_DailyLogList_Update", array);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_DailyLogList_Update", exception);
                return result;
            }
        }

        public bool UpdateBoxProgression(int userid, int boxProgression, int getBoxLevel, DateTime addGPLastDate, DateTime BoxGetDate, int alreadyBox)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[6]
                {
                    new SqlParameter("@UserID", userid),
                    new SqlParameter("@BoxProgression", boxProgression),
                    new SqlParameter("@GetBoxLevel", getBoxLevel),
                    new SqlParameter("@AddGPLastDate", DateTime.Now),
                    new SqlParameter("@BoxGetDate", BoxGetDate),
                    new SqlParameter("@AlreadyGetBox", alreadyBox)
                };
                result = db.RunProcedure("SP_User_Update_BoxProgression", sqlParameters);
                return result;
            }
            catch (Exception exception)
            {
                BaseBussiness.log.Error("User_Update_BoxProgression", exception);
                return result;
            }
        }

        public bool UpdatePlayerInfoHistory(PlayerInfoHistory info)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[4]
                {
                    new SqlParameter("@UserID", info.UserID),
                    new SqlParameter("@LastQuestsTime", info.LastQuestsTime),
                    new SqlParameter("@LastTreasureTime", info.LastTreasureTime),
                    new SqlParameter("@OutPut", SqlDbType.Int)
                };
                array[3].Direction = ParameterDirection.Output;
                db.RunProcedure("SP_User_Update_History", array);
                result = ((int)array[6].Value == 1);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("User_Update_BoxProgression", exception);
                return result;
            }
        }

        public bool AddAASInfo(AASInfo info)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[5]
                {
                    new SqlParameter("@UserID", info.UserID),
                    new SqlParameter("@Name", info.Name),
                    new SqlParameter("@IDNumber", info.IDNumber),
                    new SqlParameter("@State", info.State),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[4].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_ASSInfo_Add", array);
                result = ((int)array[4].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("UpdateAASInfo", exception);
                return result;
            }
        }

        public void AddUserLogEvent(int UserID, string UserName, string NickName, string Type, string Content)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[5]
                {
                    new SqlParameter("@UserID", UserID),
                    new SqlParameter("@UserName", UserName),
                    new SqlParameter("@NickName", NickName),
                    new SqlParameter("@Type", Type),
                    new SqlParameter("@Content", Content)
                };
                db.RunProcedure("SP_Insert_UsersLog", sqlParameters);
            }
            catch (Exception)
            {
            }
        }

        public Dictionary<int, List<string>> LoadCommands()
        {
            SqlDataReader ResultDataReader = null;
            Dictionary<int, List<string>> dictionary = new Dictionary<int, List<string>>();
            db.GetReader(ref ResultDataReader, "SP_GetAllCommands");
            while (ResultDataReader.Read())
            {
                string[] array = Convert.ToString(ResultDataReader["Commands"] ?? "").Split('$');
                List<string> list = new List<string>();
                string[] array2 = array;
                foreach (string item in array2)
                {
                    list.Add(item);
                }
                if (!dictionary.ContainsKey(Convert.ToInt32(ResultDataReader["UserID"] ?? ((object)0))))
                {
                    dictionary.Add(Convert.ToInt32(ResultDataReader["UserID"] ?? ((object)0)), list);
                }
            }
            return dictionary;
        }

        public UsersPetInfo[] GetUserPetSingles(int UserID)
        {
            List<UsersPetInfo> list = new List<UsersPetInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                array[0].Value = UserID;
                db.GetReader(ref ResultDataReader, "SP_Get_UserPet_By_ID", array);
                while (ResultDataReader.Read())
                {
                    list.Add(InitPet(ResultDataReader));
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public bool UpdateUserPet(UsersPetInfo item)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[39]
                {
                    new SqlParameter("@TemplateID", item.TemplateID),
                    new SqlParameter("@Name", (item.Name == null) ? "Error!" : item.Name),
                    new SqlParameter("@UserID", item.UserID),
                    new SqlParameter("@Attack", item.Attack),
                    new SqlParameter("@Defence", item.Defence),
                    new SqlParameter("@Luck", item.Luck),
                    new SqlParameter("@Agility", item.Agility),
                    new SqlParameter("@Blood", item.Blood),
                    new SqlParameter("@Damage", item.Damage),
                    new SqlParameter("@Guard", item.Guard),
                    new SqlParameter("@AttackGrow", item.AttackGrow),
                    new SqlParameter("@DefenceGrow", item.DefenceGrow),
                    new SqlParameter("@LuckGrow", item.LuckGrow),
                    new SqlParameter("@AgilityGrow", item.AgilityGrow),
                    new SqlParameter("@BloodGrow", item.BloodGrow),
                    new SqlParameter("@DamageGrow", item.DamageGrow),
                    new SqlParameter("@GuardGrow", item.GuardGrow),
                    new SqlParameter("@Level", item.Level),
                    new SqlParameter("@GP", item.GP),
                    new SqlParameter("@MaxGP", item.MaxGP),
                    new SqlParameter("@Hunger", item.Hunger),
                    new SqlParameter("@PetHappyStar", item.PetHappyStar),
                    new SqlParameter("@MP", item.MP),
                    new SqlParameter("@IsEquip", item.IsEquip),
                    new SqlParameter("@Place", item.Place),
                    new SqlParameter("@IsExit", item.IsExit),
                    new SqlParameter("@ID", item.ID),
                    new SqlParameter("@Skill", item.Skill),
                    new SqlParameter("@SkillEquip", item.SkillEquip),
                    new SqlParameter("@currentStarExp", item.currentStarExp),
                    new SqlParameter("@Result", SqlDbType.Int),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                };
                array[30].Direction = ParameterDirection.ReturnValue;
                array[31] = new SqlParameter("@breakGrade", item.breakGrade);
                array[32] = new SqlParameter("@breakAttack", item.breakAttack);
                array[33] = new SqlParameter("@breakDefence", item.breakDefence);
                array[34] = new SqlParameter("@breakAgility", item.breakAgility);
                array[35] = new SqlParameter("@breakLuck", item.breakLuck);
                array[36] = new SqlParameter("@breakBlood", item.breakBlood);
                array[37] = new SqlParameter("@eQPets", item.eQPets);
                array[38] = new SqlParameter("@BaseProp", item.BaseProp);
                db.RunProcedure("SP_UserPet_Update", array);
                result = ((int)array[30].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool AddUserPet(UsersPetInfo item)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[39]
                {
                    new SqlParameter("@TemplateID", item.TemplateID),
                    new SqlParameter("@Name", (item.Name == null) ? "Error!" : item.Name),
                    new SqlParameter("@UserID", item.UserID),
                    new SqlParameter("@Attack", item.Attack),
                    new SqlParameter("@Defence", item.Defence),
                    new SqlParameter("@Luck", item.Luck),
                    new SqlParameter("@Agility", item.Agility),
                    new SqlParameter("@Blood", item.Blood),
                    new SqlParameter("@Damage", item.Damage),
                    new SqlParameter("@Guard", item.Guard),
                    new SqlParameter("@AttackGrow", item.AttackGrow),
                    new SqlParameter("@DefenceGrow", item.DefenceGrow),
                    new SqlParameter("@LuckGrow", item.LuckGrow),
                    new SqlParameter("@AgilityGrow", item.AgilityGrow),
                    new SqlParameter("@BloodGrow", item.BloodGrow),
                    new SqlParameter("@DamageGrow", item.DamageGrow),
                    new SqlParameter("@GuardGrow", item.GuardGrow),
                    new SqlParameter("@Level", item.Level),
                    new SqlParameter("@GP", item.GP),
                    new SqlParameter("@MaxGP", item.MaxGP),
                    new SqlParameter("@Hunger", item.Hunger),
                    new SqlParameter("@PetHappyStar", item.PetHappyStar),
                    new SqlParameter("@MP", item.MP),
                    new SqlParameter("@IsEquip", item.IsEquip),
                    new SqlParameter("@Skill", item.Skill),
                    new SqlParameter("@SkillEquip", item.SkillEquip),
                    new SqlParameter("@Place", item.Place),
                    new SqlParameter("@IsExit", item.IsExit),
                    new SqlParameter("@ID", item.ID),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                };
                array[28].Direction = ParameterDirection.Output;
                array[29] = new SqlParameter("@currentStarExp", item.currentStarExp);
                array[30] = new SqlParameter("@Result", SqlDbType.Int);
                array[30].Direction = ParameterDirection.ReturnValue;
                array[31] = new SqlParameter("@breakGrade", item.breakGrade);
                array[32] = new SqlParameter("@breakAttack", item.breakAttack);
                array[33] = new SqlParameter("@breakDefence", item.breakDefence);
                array[34] = new SqlParameter("@breakAgility", item.breakAgility);
                array[35] = new SqlParameter("@breakLuck", item.breakLuck);
                array[36] = new SqlParameter("@breakBlood", item.breakBlood);
                array[37] = new SqlParameter("@eQPets", item.eQPets);
                array[38] = new SqlParameter("@BaseProp", item.BaseProp);
                result = db.RunProcedure("SP_User_Add_Pet", array);
                result = ((int)array[30].Value == 0);
                item.ID = (int)array[28].Value;
                item.IsDirty = false;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public UsersPetInfo InitPet(SqlDataReader reader)
        {
            return new UsersPetInfo
            {
                ID = (int)reader["ID"],
                TemplateID = (int)reader["TemplateID"],
                Name = reader["Name"].ToString(),
                UserID = (int)reader["UserID"],
                Attack = (int)reader["Attack"],
                AttackGrow = (int)reader["AttackGrow"],
                Agility = (int)reader["Agility"],
                AgilityGrow = (int)reader["AgilityGrow"],
                Defence = (int)reader["Defence"],
                DefenceGrow = (int)reader["DefenceGrow"],
                Luck = (int)reader["Luck"],
                LuckGrow = (int)reader["LuckGrow"],
                Blood = (int)reader["Blood"],
                BloodGrow = (int)reader["BloodGrow"],
                Damage = (int)reader["Damage"],
                DamageGrow = (int)reader["DamageGrow"],
                Guard = (int)reader["Guard"],
                GuardGrow = (int)reader["GuardGrow"],
                Level = (int)reader["Level"],
                GP = (int)reader["GP"],
                MaxGP = (int)reader["MaxGP"],
                Hunger = (int)reader["Hunger"],
                MP = (int)reader["MP"],
                Place = (int)reader["Place"],
                IsEquip = (bool)reader["IsEquip"],
                IsExit = (bool)reader["IsExit"],
                Skill = reader["Skill"].ToString(),
                SkillEquip = reader["SkillEquip"].ToString(),
                currentStarExp = (int)reader["currentStarExp"],
                breakGrade = (int)reader["breakGrade"],
                breakAttack = (int)reader["breakAttack"],
                breakDefence = (int)reader["breakDefence"],
                breakAgility = (int)reader["breakAgility"],
                breakLuck = (int)reader["breakLuck"],
                breakBlood = (int)reader["breakBlood"],
                eQPets = ((reader["eQPets"] == null) ? "" : reader["eQPets"].ToString()),
                BaseProp = ((reader["BaseProp"] == null) ? "" : reader["BaseProp"].ToString())
            };
        }

        public bool RegisterPlayer2(string userName, string passWord, string nickName, int attack, int defence, int agility, int luck, int cateogryId, string bStyle, string bPic, string gStyle, string armColor, string hairColor, string faceColor, string clothColor, string hatchColor, int sex, ref string msg, int validDate)
        {
            bool result = false;
            try
            {
                string[] array = bStyle.Split(',');
                string[] array2 = gStyle.Split(',');
                string[] array3 = bPic.Split(',');
                SqlParameter[] array4 = new SqlParameter[31];
                array4[0] = new SqlParameter("@UserName", userName);
                array4[1] = new SqlParameter("@PassWord", passWord);
                array4[2] = new SqlParameter("@NickName", nickName);
                array4[3] = new SqlParameter("@BArmID", int.Parse(array[0]));
                array4[4] = new SqlParameter("@BHairID", int.Parse(array[1]));
                array4[5] = new SqlParameter("@BFaceID", int.Parse(array[2]));
                array4[6] = new SqlParameter("@BClothID", int.Parse(array[3]));
                array4[7] = new SqlParameter("@BHatID", int.Parse(array[4]));
                array4[21] = new SqlParameter("@ArmPic", array3[0]);
                array4[22] = new SqlParameter("@HairPic", array3[1]);
                array4[23] = new SqlParameter("@FacePic", array3[2]);
                array4[24] = new SqlParameter("@ClothPic", array3[3]);
                array4[25] = new SqlParameter("@HatPic", array3[4]);
                array4[8] = new SqlParameter("@GArmID", int.Parse(array2[0]));
                array4[9] = new SqlParameter("@GHairID", int.Parse(array2[1]));
                array4[10] = new SqlParameter("@GFaceID", int.Parse(array2[2]));
                array4[11] = new SqlParameter("@GClothID", int.Parse(array2[3]));
                array4[12] = new SqlParameter("@GHatID", int.Parse(array2[4]));
                array4[13] = new SqlParameter("@ArmColor", armColor);
                array4[14] = new SqlParameter("@HairColor", hairColor);
                array4[15] = new SqlParameter("@FaceColor", faceColor);
                array4[16] = new SqlParameter("@ClothColor", clothColor);
                array4[17] = new SqlParameter("@HatColor", clothColor);
                array4[18] = new SqlParameter("@Sex", sex);
                array4[19] = new SqlParameter("@StyleDate", validDate);
                array4[26] = new SqlParameter("@CategoryID", cateogryId);
                array4[27] = new SqlParameter("@Attack", attack);
                array4[28] = new SqlParameter("@Defence", defence);
                array4[29] = new SqlParameter("@Agility", agility);
                array4[30] = new SqlParameter("@Luck", luck);
                array4[20] = new SqlParameter("@Result", SqlDbType.Int);
                array4[20].Direction = ParameterDirection.ReturnValue;
                result = db.RunProcedure("SP_Users_RegisterNotValidate2", array4);
                int num = (int)array4[20].Value;
                result = (num == 0);
                switch (num)
                {
                    case 3:
                        msg = LanguageMgr.GetTranslation("PlayerBussiness.RegisterPlayer.Msg3");
                        return result;
                    case 2:
                        msg = LanguageMgr.GetTranslation("PlayerBussiness.RegisterPlayer.Msg2");
                        return result;
                    default:
                        return result;
                }
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error($"{userName} {passWord} {nickName} {attack} {defence} {agility} {luck} {cateogryId} {bStyle} {bPic} {gStyle}");
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool UpdateUserReputeFightPower()
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[0].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Update_Repute_FightPower", array);
                result = ((int)array[0].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public UsersExtraInfo[] GetRankCaddy()
        {
            List<UsersExtraInfo> list = new List<UsersExtraInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                db.GetReader(ref ResultDataReader, "SP_Get_Rank_Caddy");
                while (ResultDataReader.Read())
                {
                    list.Add(new UsersExtraInfo
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        NickName = (string)ResultDataReader["NickName"],
                        TotalCaddyOpen = (int)ResultDataReader["badLuckNumber"]
                    });
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("SP_Get_Rank_Caddy", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public UserLabyrinthInfo GetSingleLabyrinth(int ID)
        {
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] array = new SqlParameter[1]
                {
                    new SqlParameter("@ID", SqlDbType.Int, 4)
                };
                array[0].Value = ID;
                db.GetReader(ref ResultDataReader, "SP_GetSingleLabyrinth", array);
                if (ResultDataReader.Read())
                {
                    return new UserLabyrinthInfo
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        myProgress = (int)ResultDataReader["myProgress"],
                        myRanking = (int)ResultDataReader["myRanking"],
                        completeChallenge = (bool)ResultDataReader["completeChallenge"],
                        isDoubleAward = (bool)ResultDataReader["isDoubleAward"],
                        currentFloor = (int)ResultDataReader["currentFloor"],
                        accumulateExp = (int)ResultDataReader["accumulateExp"],
                        remainTime = (int)ResultDataReader["remainTime"],
                        currentRemainTime = (int)ResultDataReader["currentRemainTime"],
                        cleanOutAllTime = (int)ResultDataReader["cleanOutAllTime"],
                        cleanOutGold = (int)ResultDataReader["cleanOutGold"],
                        tryAgainComplete = (bool)ResultDataReader["tryAgainComplete"],
                        isInGame = (bool)ResultDataReader["isInGame"],
                        isCleanOut = (bool)ResultDataReader["isCleanOut"],
                        serverMultiplyingPower = (bool)ResultDataReader["serverMultiplyingPower"],
                        LastDate = (DateTime)ResultDataReader["LastDate"],
                        ProcessAward = (string)ResultDataReader["ProcessAward"]
                    };
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("SP_GetSingleUserLabyrinth", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return null;
        }

        public bool AddUserLabyrinth(UserLabyrinthInfo laby)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[18]
                {
                    new SqlParameter("@UserID", laby.UserID),
                    new SqlParameter("@myProgress", laby.myProgress),
                    new SqlParameter("@myRanking", laby.myRanking),
                    new SqlParameter("@completeChallenge", laby.completeChallenge),
                    new SqlParameter("@isDoubleAward", laby.isDoubleAward),
                    new SqlParameter("@currentFloor", laby.currentFloor),
                    new SqlParameter("@accumulateExp", laby.accumulateExp),
                    new SqlParameter("@remainTime", laby.remainTime),
                    new SqlParameter("@currentRemainTime", laby.currentRemainTime),
                    new SqlParameter("@cleanOutAllTime", laby.cleanOutAllTime),
                    new SqlParameter("@cleanOutGold", laby.cleanOutGold),
                    new SqlParameter("@tryAgainComplete", laby.tryAgainComplete),
                    new SqlParameter("@isInGame", laby.isInGame),
                    new SqlParameter("@isCleanOut", laby.isCleanOut),
                    new SqlParameter("@serverMultiplyingPower", laby.serverMultiplyingPower),
                    new SqlParameter("@LastDate", laby.LastDate),
                    new SqlParameter("@ProcessAward", laby.ProcessAward),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[17].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Users_Labyrinth_Add", array);
                result = ((int)array[17].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public bool UpdateLabyrinthInfo(UserLabyrinthInfo laby)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[18]
                {
                    new SqlParameter("@UserID", laby.UserID),
                    new SqlParameter("@myProgress", laby.myProgress),
                    new SqlParameter("@myRanking", laby.myRanking),
                    new SqlParameter("@completeChallenge", laby.completeChallenge),
                    new SqlParameter("@isDoubleAward", laby.isDoubleAward),
                    new SqlParameter("@currentFloor", laby.currentFloor),
                    new SqlParameter("@accumulateExp", laby.accumulateExp),
                    new SqlParameter("@remainTime", laby.remainTime),
                    new SqlParameter("@currentRemainTime", laby.currentRemainTime),
                    new SqlParameter("@cleanOutAllTime", laby.cleanOutAllTime),
                    new SqlParameter("@cleanOutGold", laby.cleanOutGold),
                    new SqlParameter("@tryAgainComplete", laby.tryAgainComplete),
                    new SqlParameter("@isInGame", laby.isInGame),
                    new SqlParameter("@isCleanOut", laby.isCleanOut),
                    new SqlParameter("@serverMultiplyingPower", laby.serverMultiplyingPower),
                    new SqlParameter("@LastDate", laby.LastDate),
                    new SqlParameter("@ProcessAward", laby.ProcessAward),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[17].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_UpdateLabyrinthInfo", array);
                result = true;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("SP_UpdateLabyrinthInfo", exception);
                return result;
            }
        }

        public UserGiftInfo[] GetAllUserGifts(int userid, bool isReceive)
        {
            List<UserGiftInfo> list = new List<UserGiftInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[2]
                {
                    new SqlParameter("@UserID", userid),
                    new SqlParameter("@IsReceive", isReceive)
                };
                db.GetReader(ref ResultDataReader, "SP_Users_Gift_Single", sqlParameters);
                while (ResultDataReader.Read())
                {
                    list.Add(new UserGiftInfo
                    {
                        ID = (int)ResultDataReader["ID"],
                        ReceiverID = (int)ResultDataReader["ReceiverID"],
                        SenderID = (int)ResultDataReader["SenderID"],
                        TemplateID = (int)ResultDataReader["TemplateID"],
                        Count = (int)ResultDataReader["Count"],
                        CreateDate = (DateTime)ResultDataReader["CreateDate"],
                        LastUpdate = (DateTime)ResultDataReader["LastUpdate"]
                    });
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetAllUserGifts", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public UserGiftInfo[] GetAllUserReceivedGifts(int userid)
        {
            Dictionary<int, UserGiftInfo> dictionary = new Dictionary<int, UserGiftInfo>();
            SqlDataReader sqlDataReader = null;
            try
            {
                UserGiftInfo[] allUserGifts = GetAllUserGifts(userid, isReceive: true);
                if (allUserGifts != null)
                {
                    UserGiftInfo[] array = allUserGifts;
                    foreach (UserGiftInfo userGiftInfo in array)
                    {
                        if (dictionary.ContainsKey(userGiftInfo.TemplateID))
                        {
                            dictionary[userGiftInfo.TemplateID].Count += userGiftInfo.Count;
                        }
                        else
                        {
                            dictionary.Add(userGiftInfo.TemplateID, userGiftInfo);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("GetAllUserReceivedGifts", exception);
                }
            }
            finally
            {
                if (sqlDataReader != null && !sqlDataReader.IsClosed)
                {
                    sqlDataReader.Close();
                }
            }
            return dictionary.Values.ToArray();
        }

        public bool AddUserGift(UserGiftInfo info)
        {
            bool result = false;
            try
            {
                db.RunProcedure("SP_Users_Gift_Add", new SqlParameter[4]
                {
                    new SqlParameter("@SenderID", info.SenderID),
                    new SqlParameter("@ReceiverID", info.ReceiverID),
                    new SqlParameter("@TemplateID", info.TemplateID),
                    new SqlParameter("@Count", info.Count)
                });
                result = true;
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("AddUserGift", exception);
                return result;
            }
        }

        public bool UpdateUserCharmGP(int userId, int int_1)
        {
            bool result = false;
            try
            {
                SqlParameter[] array = new SqlParameter[3]
                {
                    new SqlParameter("@UserID", userId),
                    new SqlParameter("@CharmGP", int_1),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                array[2].Direction = ParameterDirection.ReturnValue;
                db.RunProcedure("SP_Users_UpdateCharmGP", array);
                result = ((int)array[2].Value == 0);
                return result;
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("AddUserGift", exception);
                return result;
            }
        }

        public bool ResetEliteGame(int point)
        {
            bool result = false;
            try
            {
                return db.RunProcedure("SP_EliteGame_Reset", new SqlParameter[1]
                {
                    new SqlParameter("@EliteScore", point)
                });
            }
            catch (Exception exception)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                {
                    return result;
                }
                BaseBussiness.log.Error("Init", exception);
                return result;
            }
        }

        public PlayerEliteGameInfo[] GetEliteScorePlayers()
        {
            List<PlayerEliteGameInfo> list = new List<PlayerEliteGameInfo>();
            SqlDataReader ResultDataReader = null;
            try
            {
                db.GetReader(ref ResultDataReader, "SP_Get_EliteScorePlayers");
                while (ResultDataReader.Read())
                {
                    list.Add(new PlayerEliteGameInfo
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        NickName = (string)ResultDataReader["NickName"],
                        GameType = (((int)ResultDataReader["Grade"] < 41) ? 1 : 2),
                        CurrentPoint = (int)ResultDataReader["EliteScore"],
                        Rank = 0,
                        ReadyStatus = false,
                        Status = 0,
                        Winer = 0
                    });
                }
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("SP_Get_EliteScorePlayers", exception);
                }
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                {
                    ResultDataReader.Close();
                }
            }
            return list.ToArray();
        }

        public PlayerInfo[] GetPlayerPage(int page, int size, ref int total, int order, int where, int userID, string key, ref bool resultValue)
        {
            List<PlayerInfo> list = new List<PlayerInfo>();
            try
            {
                string text = " IsExist=1 and IsFirst<> 0 ";
                if (userID != -1)
                {
                    text = text + " and UserID =" + userID + " ";
                }
                string str = "GP desc";
                switch (order)
                {
                    case 1:
                        str = "Offer desc";
                        break;
                    case 2:
                        str = "AddDayGP desc";
                        break;
                    case 3:
                        str = "AddWeekGP desc";
                        break;
                    case 4:
                        str = "AddDayOffer desc";
                        break;
                    case 5:
                        str = "AddWeekOffer desc";
                        break;
                    case 6:
                        str = "FightPower desc";
                        break;
                    case 7:
                        str = "EliteScore desc";
                        break;
                    case 8:
                        str = "State desc, graduatesCount desc, FightPower desc";
                        break;
                    case 9:
                        str = "NEWID()";
                        break;
                    case 10:
                        str = "State desc, GP asc, FightPower desc";
                        break;
                    case 11:
                        str = "AchievementPoint desc";
                        break;
                    case 12:
                        str = "charmGP desc";
                        break;
                }
                switch (where)
                {
                    case 0:
                        text += " ";
                        break;
                    case 1:
                        text += " and Grade >= 20 ";
                        break;
                    case 2:
                        text += " and Grade >= 5 and Grade < 17 ";
                        break;
                    case 3:
                        text += " and Grade >= 20 and apprenticeshipState != 3 and State = 1 ";
                        break;
                    case 4:
                        text += " and Grade >= 5 and Grade < 17 and masterID = 0 and State = 1 ";
                        break;
                }
                str += ",UserID";
                foreach (DataRow row in GetPage("V_Sys_Users_Detail", text, page, size, "*", str, key, ref total).Rows)
                {
                    list.Add(new PlayerInfo
                    {
                        Agility = (int)row["Agility"],
                        Attack = (int)row["Attack"],
                        Colors = ((row["Colors"] == null) ? "" : row["Colors"].ToString()),
                        ConsortiaID = (int)row["ConsortiaID"],
                        Defence = (int)row["Defence"],
                        Gold = (int)row["Gold"],
                        GP = (int)row["GP"],
                        Grade = (int)row["Grade"],
                        ID = (int)row["UserID"],
                        Luck = (int)row["Luck"],
                        Money = (int)row["Money"],
                        NickName = ((row["NickName"] == null) ? "" : row["NickName"].ToString()),
                        Sex = (bool)row["Sex"],
                        State = (int)row["State"],
                        Style = ((row["Style"] == null) ? "" : row["Style"].ToString()),
                        Hide = (int)row["Hide"],
                        Repute = (int)row["Repute"],
                        UserName = ((row["UserName"] == null) ? "" : row["UserName"].ToString()),
                        ConsortiaName = ((row["ConsortiaName"] == null) ? "" : row["ConsortiaName"].ToString()),
                        Offer = (int)row["Offer"],
                        UseOffer = (int)row["UseOffer"],
                        Skin = ((row["Skin"] == null) ? "" : row["Skin"].ToString()),
                        IsBanChat = (bool)row["IsBanChat"],
                        ReputeOffer = (int)row["ReputeOffer"],
                        ConsortiaRepute = (int)row["ConsortiaRepute"],
                        ConsortiaLevel = (int)row["ConsortiaLevel"],
                        StoreLevel = (int)row["StoreLevel"],
                        ShopLevel = (int)row["ShopLevel"],
                        SmithLevel = (int)row["SmithLevel"],
                        ConsortiaHonor = (int)row["ConsortiaHonor"],
                        RichesOffer = (int)row["RichesOffer"],
                        RichesRob = (int)row["RichesRob"],
                        DutyLevel = (int)row["DutyLevel"],
                        DutyName = ((row["DutyName"] == null) ? "" : row["DutyName"].ToString()),
                        Right = (int)row["Right"],
                        ChairmanName = ((row["ChairmanName"] == null) ? "" : row["ChairmanName"].ToString()),
                        Win = (int)row["Win"],
                        Total = (int)row["Total"],
                        Escape = (int)row["Escape"],
                        AddDayGP = (int)row["AddDayGP"],
                        AddDayOffer = (int)row["AddDayOffer"],
                        AddWeekGP = (int)row["AddWeekGP"],
                        AddWeekOffer = (int)row["AddWeekOffer"],
                        ConsortiaRiches = (int)row["ConsortiaRiches"],
                        CheckCount = (int)row["CheckCount"],
                        Nimbus = (int)row["Nimbus"],
                        GiftToken = (int)row["GiftToken"],
                        QuestSite = ((row["QuestSite"] == null) ? new byte[8000] : ((byte[])row["QuestSite"])),
                        PvePermission = ((row["PvePermission"] == null) ? "" : row["PvePermission"].ToString()),
                        FightPower = (int)row["FightPower"],
                        AchievementPoint = (int)row["AchievementPoint"],
                        Honor = (string)row["Honor"],
                        IsShowConsortia = (bool)row["IsShowConsortia"],
                        OptionOnOff = (int)row["OptionOnOff"],
                        badgeID = (int)row["badgeID"],
                        EliteScore = (int)row["EliteScore"],
                        apprenticeshipState = (int)row["apprenticeshipState"],
                        masterID = (int)row["masterID"],
                        graduatesCount = (int)row["graduatesCount"],
                        masterOrApprentices = ((row["masterOrApprentices"] == DBNull.Value) ? "" : row["masterOrApprentices"].ToString()),
                        honourOfMaster = ((row["honourOfMaster"] == DBNull.Value) ? "" : row["honourOfMaster"].ToString()),
                        IsMarried = (bool)row["IsMarried"],
                        typeVIP = byte.Parse(row["typeVIP"].ToString()),
                        VIPLevel = (int)row["VIPLevel"],
                        SpouseID = (int)row["SpouseID"],
                        SpouseName = ((row["SpouseName"] == DBNull.Value) ? "" : row["SpouseName"].ToString())
                    });
                }
                resultValue = true;
            }
            catch (Exception exception)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            return list.ToArray();
        }
    }
}
