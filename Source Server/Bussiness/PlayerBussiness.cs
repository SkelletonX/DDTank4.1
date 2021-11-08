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
        public bool ActivePlayer(
          ref PlayerInfo player,
          string userName,
          string passWord,
          bool sex,
          int gold,
          int money,
          string IP,
          string site)
        {
            bool flag = false;
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
                SqlParameter[] SqlParameters = new SqlParameter[21];
                SqlParameters[0] = new SqlParameter("@UserID", SqlDbType.Int);
                SqlParameters[0].Direction = ParameterDirection.Output;
                SqlParameters[1] = new SqlParameter("@Attack", (object)player.Attack);
                SqlParameters[2] = new SqlParameter("@Colors", player.Colors == null ? (object)"" : (object)player.Colors);
                SqlParameters[3] = new SqlParameter("@ConsortiaID", (object)player.ConsortiaID);
                SqlParameters[4] = new SqlParameter("@Defence", (object)player.Defence);
                SqlParameters[5] = new SqlParameter("@Gold", (object)player.Gold);
                SqlParameters[6] = new SqlParameter("@GP", (object)player.GP);
                SqlParameters[7] = new SqlParameter("@Grade", (object)player.Grade);
                SqlParameters[8] = new SqlParameter("@Luck", (object)player.Luck);
                SqlParameters[9] = new SqlParameter("@Money", (object)player.Money);
                SqlParameters[10] = new SqlParameter("@Style", player.Style == null ? (object)"" : (object)player.Style);
                SqlParameters[11] = new SqlParameter("@Agility", (object)player.Agility);
                SqlParameters[12] = new SqlParameter("@State", (object)player.State);
                SqlParameters[13] = new SqlParameter("@UserName", (object)userName);
                SqlParameters[14] = new SqlParameter("@PassWord", (object)passWord);
                SqlParameters[15] = new SqlParameter("@Sex", (object)sex);
                SqlParameters[16] = new SqlParameter("@Hide", (object)player.Hide);
                SqlParameters[17] = new SqlParameter("@ActiveIP", (object)IP);
                SqlParameters[18] = new SqlParameter("@Skin", player.Skin == null ? (object)"" : (object)player.Skin);
                SqlParameters[19] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[19].Direction = ParameterDirection.ReturnValue;
                SqlParameters[20] = new SqlParameter("@Site", (object)site);
                flag = this.db.RunProcedure("SP_Users_Active", SqlParameters);
                player.ID = (int)SqlParameters[0].Value;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool AddAuction(AuctionInfo info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[18];
                SqlParameters[0] = new SqlParameter("@AuctionID", (object)info.AuctionID);
                SqlParameters[0].Direction = ParameterDirection.Output;
                SqlParameters[1] = new SqlParameter("@AuctioneerID", (object)info.AuctioneerID);
                SqlParameters[2] = new SqlParameter("@AuctioneerName", info.AuctioneerName == null ? (object)"" : (object)info.AuctioneerName);
                SqlParameters[3] = new SqlParameter("@BeginDate", (object)info.BeginDate);
                SqlParameters[4] = new SqlParameter("@BuyerID", (object)info.BuyerID);
                SqlParameters[5] = new SqlParameter("@BuyerName", info.BuyerName == null ? (object)"" : (object)info.BuyerName);
                SqlParameters[6] = new SqlParameter("@IsExist", (object)info.IsExist);
                SqlParameters[7] = new SqlParameter("@ItemID", (object)info.ItemID);
                SqlParameters[8] = new SqlParameter("@Mouthful", (object)info.Mouthful);
                SqlParameters[9] = new SqlParameter("@PayType", (object)info.PayType);
                SqlParameters[10] = new SqlParameter("@Price", (object)info.Price);
                SqlParameters[11] = new SqlParameter("@Rise", (object)info.Rise);
                SqlParameters[12] = new SqlParameter("@ValidDate", (object)info.ValidDate);
                SqlParameters[13] = new SqlParameter("@TemplateID", (object)info.TemplateID);
                SqlParameters[14] = new SqlParameter("Name", (object)info.Name);
                SqlParameters[15] = new SqlParameter("Category", (object)info.Category);
                SqlParameters[16] = new SqlParameter("Random", (object)info.Random);
                SqlParameters[17] = new SqlParameter("goodsCount", (object)info.goodsCount);
                flag = this.db.RunProcedure("SP_Auction_Add", SqlParameters);
                info.AuctionID = (int)SqlParameters[0].Value;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool AddCards(UsersCardInfo item)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[19];
                SqlParameters[0] = new SqlParameter("@CardID", (object)item.CardID);
                SqlParameters[0].Direction = ParameterDirection.Output;
                SqlParameters[1] = new SqlParameter("@UserID", (object)item.UserID);
                SqlParameters[2] = new SqlParameter("@TemplateID", (object)item.TemplateID);
                SqlParameters[3] = new SqlParameter("@Place", (object)item.Place);
                SqlParameters[4] = new SqlParameter("@Count", (object)item.Count);
                SqlParameters[5] = new SqlParameter("@Attack", (object)item.Attack);
                SqlParameters[6] = new SqlParameter("@Defence", (object)item.Defence);
                SqlParameters[7] = new SqlParameter("@Agility", (object)item.Agility);
                SqlParameters[8] = new SqlParameter("@Luck", (object)item.Luck);
                SqlParameters[9] = new SqlParameter("@Guard", (object)item.Guard);
                SqlParameters[10] = new SqlParameter("@Damage", (object)item.Damage);
                SqlParameters[11] = new SqlParameter("@Level", (object)item.Level);
                SqlParameters[12] = new SqlParameter("@CardGP", (object)item.CardGP);
                SqlParameters[14] = new SqlParameter("@isFirstGet", (object)item.isFirstGet);
                SqlParameters[15] = new SqlParameter("@AttackReset", (object)item.AttackReset);
                SqlParameters[16] = new SqlParameter("@DefenceReset", (object)item.DefenceReset);
                SqlParameters[17] = new SqlParameter("@AgilityReset", (object)item.AgilityReset);
                SqlParameters[18] = new SqlParameter("@LuckReset", (object)item.LuckReset);
                SqlParameters[13] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[13].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_UserCard_Add", SqlParameters);
                flag = (int)SqlParameters[13].Value == 0;
                item.CardID = (int)SqlParameters[0].Value;
                item.IsDirty = false;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool AddChargeMoney(
          string chargeID,
          string userName,
          int money,
          string payWay,
          Decimal needMoney,
          ref int userID,
          ref int isResult,
          DateTime date,
          string IP,
          string nickName)
        {
            bool flag = false;
            userID = 0;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[10]
                {
          new SqlParameter("@ChargeID", (object) chargeID),
          new SqlParameter("@UserName", (object) userName),
          new SqlParameter("@Money", (object) money),
          new SqlParameter("@Date", (object) date.ToString("yyyy-MM-dd HH:mm:ss")),
          new SqlParameter("@PayWay", (object) payWay),
          new SqlParameter("@NeedMoney", (object) needMoney),
          new SqlParameter("@UserID", (object) userID),
          null,
          null,
          null
                };
                SqlParameters[6].Direction = ParameterDirection.InputOutput;
                SqlParameters[7] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[7].Direction = ParameterDirection.ReturnValue;
                SqlParameters[8] = new SqlParameter("@IP", (object)IP);
                SqlParameters[9] = new SqlParameter("@NickName", (object)nickName);
                flag = this.db.RunProcedure("SP_Charge_Money_Add", SqlParameters);
                userID = (int)SqlParameters[6].Value;
                isResult = (int)SqlParameters[7].Value;
                flag = isResult == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool AddChargeMoney(
          string chargeID,
          string userName,
          int money,
          string payWay,
          Decimal needMoney,
          ref int userID,
          ref int isResult,
          DateTime date,
          string IP,
          int UserID)
        {
            bool flag = false;
            userID = 0;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[10]
                {
          new SqlParameter("@ChargeID", (object) chargeID),
          new SqlParameter("@UserName", (object) userName),
          new SqlParameter("@Money", (object) money),
          new SqlParameter("@Date", (object) date.ToString("yyyy-MM-dd HH:mm:ss")),
          new SqlParameter("@PayWay", (object) payWay),
          new SqlParameter("@NeedMoney", (object) needMoney),
          new SqlParameter("@UserID", (object) userID),
          null,
          null,
          null
                };
                SqlParameters[6].Direction = ParameterDirection.InputOutput;
                SqlParameters[7] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[7].Direction = ParameterDirection.ReturnValue;
                SqlParameters[8] = new SqlParameter("@IP", (object)IP);
                SqlParameters[9] = new SqlParameter("@SourceUserID", (object)UserID);
                flag = this.db.RunProcedure("SP_Charge_Money_UserId_Add", SqlParameters);
                userID = (int)SqlParameters[6].Value;
                isResult = (int)SqlParameters[7].Value;
                flag = isResult == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool AddEatPets(EatPetsInfo info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@ID", (object)info.ID), new SqlParameter("@UserID", (object)info.UserID), new SqlParameter("@weaponExp", (object)info.weaponExp), new SqlParameter("@weaponLevel", (object)info.weaponLevel), new SqlParameter("@clothesExp", (object)info.clothesExp), new SqlParameter("@clothesLevel", (object)info.clothesLevel), new SqlParameter("@hatExp", (object)info.hatExp), new SqlParameter("@hatLevel", (object)info.hatLevel) };
                sqlParameter[0].Direction = ParameterDirection.Output;
                flag = this.db.RunProcedure("SP_Sys_Eat_Pets_Add", sqlParameter);
                info.ID = (int)sqlParameter[0].Value;
                info.IsDirty = false;
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("SP_Sys_Eat_Pets_Add", exception);
                }
            }
            return flag;
        }

        public bool AddFriends(FriendInfo info)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Users_Friends_Add", new SqlParameter[7]
                {
          new SqlParameter("@ID", (object) info.ID),
          new SqlParameter("@AddDate", (object) DateTime.Now),
          new SqlParameter("@FriendID", (object) info.FriendID),
          new SqlParameter("@IsExist", (object) true),
          new SqlParameter("@Remark", info.Remark == null ? (object) "" : (object) info.Remark),
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@Relation", (object) info.Relation)
                });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool AddGoods(SqlDataProvider.Data.ItemInfo item)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[35];
                SqlParameters[0] = new SqlParameter("@ItemID", (object)item.ItemID);
                SqlParameters[0].Direction = ParameterDirection.Output;
                SqlParameters[1] = new SqlParameter("@UserID", (object)item.UserID);
                SqlParameters[2] = new SqlParameter("@TemplateID", (object)item.Template.TemplateID);
                SqlParameters[3] = new SqlParameter("@Place", (object)item.Place);
                SqlParameters[4] = new SqlParameter("@AgilityCompose", (object)item.AgilityCompose);
                SqlParameters[5] = new SqlParameter("@AttackCompose", (object)item.AttackCompose);
                SqlParameters[6] = new SqlParameter("@BeginDate", (object)item.BeginDate);
                SqlParameters[7] = new SqlParameter("@Color", item.Color == null ? (object)"" : (object)item.Color);
                SqlParameters[8] = new SqlParameter("@Count", (object)item.Count);
                SqlParameters[9] = new SqlParameter("@DefendCompose", (object)item.DefendCompose);
                SqlParameters[10] = new SqlParameter("@IsBinds", (object)item.IsBinds);
                SqlParameters[11] = new SqlParameter("@IsExist", (object)item.IsExist);
                SqlParameters[12] = new SqlParameter("@IsJudge", (object)item.IsJudge);
                SqlParameters[13] = new SqlParameter("@LuckCompose", (object)item.LuckCompose);
                SqlParameters[14] = new SqlParameter("@StrengthenLevel", (object)item.StrengthenLevel);
                SqlParameters[15] = new SqlParameter("@ValidDate", (object)item.ValidDate);
                SqlParameters[16] = new SqlParameter("@BagType", (object)item.BagType);
                SqlParameters[17] = new SqlParameter("@Skin", item.Skin == null ? (object)"" : (object)item.Skin);
                SqlParameters[18] = new SqlParameter("@IsUsed", (object)item.IsUsed);
                SqlParameters[19] = new SqlParameter("@RemoveType", (object)item.RemoveType);
                SqlParameters[20] = new SqlParameter("@Hole1", (object)item.Hole1);
                SqlParameters[21] = new SqlParameter("@Hole2", (object)item.Hole2);
                SqlParameters[22] = new SqlParameter("@Hole3", (object)item.Hole3);
                SqlParameters[23] = new SqlParameter("@Hole4", (object)item.Hole4);
                SqlParameters[24] = new SqlParameter("@Hole5", (object)item.Hole5);
                SqlParameters[25] = new SqlParameter("@Hole6", (object)item.Hole6);
                SqlParameters[26] = new SqlParameter("@StrengthenTimes", (object)item.StrengthenTimes);
                SqlParameters[27] = new SqlParameter("@Hole5Level", (object)item.Hole5Level);
                SqlParameters[28] = new SqlParameter("@Hole5Exp", (object)item.Hole5Exp);
                SqlParameters[29] = new SqlParameter("@Hole6Level", (object)item.Hole6Level);
                SqlParameters[30] = new SqlParameter("@Hole6Exp", (object)item.Hole6Exp);
                SqlParameters[31] = new SqlParameter("@IsGold", (object)item.IsGold);
                SqlParameters[32] = new SqlParameter("@goldValidDate", (object)item.goldValidDate);
                SqlParameters[33] = new SqlParameter("@goldBeginTime", (object)item.goldBeginTime);
                SqlParameters[34] = new SqlParameter("@StrengthenExp", (object)item.StrengthenExp);
                flag = this.db.RunProcedure("SP_Users_Items_Add", SqlParameters);
                item.ItemID = (int)SqlParameters[0].Value;
                item.IsDirty = false;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool AddGoods(SqlDataProvider.Data.ItemInfo item, bool Strengthen)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[34];
                SqlParameters[0] = new SqlParameter("@ItemID", (object)item.ItemID);
                SqlParameters[0].Direction = ParameterDirection.Output;
                SqlParameters[1] = new SqlParameter("@UserID", (object)item.UserID);
                SqlParameters[2] = new SqlParameter("@TemplateID", (object)item.Template.TemplateID);
                SqlParameters[3] = new SqlParameter("@Place", (object)item.Place);
                SqlParameters[4] = new SqlParameter("@AgilityCompose", (object)item.AgilityCompose);
                SqlParameters[5] = new SqlParameter("@AttackCompose", (object)item.AttackCompose);
                SqlParameters[6] = new SqlParameter("@BeginDate", (object)item.BeginDate);
                SqlParameters[7] = new SqlParameter("@Color", item.Color == null ? (object)"" : (object)item.Color);
                SqlParameters[8] = new SqlParameter("@Count", (object)item.Count);
                SqlParameters[9] = new SqlParameter("@DefendCompose", (object)item.DefendCompose);
                SqlParameters[10] = new SqlParameter("@IsBinds", (object)item.IsBinds);
                SqlParameters[11] = new SqlParameter("@IsExist", (object)item.IsExist);
                SqlParameters[12] = new SqlParameter("@IsJudge", (object)item.IsJudge);
                SqlParameters[13] = new SqlParameter("@LuckCompose", (object)item.LuckCompose);
                SqlParameters[14] = new SqlParameter("@StrengthenLevel", (object)item.StrengthenLevel);
                SqlParameters[15] = new SqlParameter("@ValidDate", (object)item.ValidDate);
                SqlParameters[16] = new SqlParameter("@BagType", (object)item.BagType);
                SqlParameters[17] = new SqlParameter("@Skin", item.Skin == null ? (object)"" : (object)item.Skin);
                SqlParameters[18] = new SqlParameter("@IsUsed", (object)item.IsUsed);
                SqlParameters[19] = new SqlParameter("@RemoveType", (object)item.RemoveType);
                SqlParameters[20] = new SqlParameter("@Hole1", (object)item.Hole1);
                SqlParameters[21] = new SqlParameter("@Hole2", (object)item.Hole2);
                SqlParameters[22] = new SqlParameter("@Hole3", (object)item.Hole3);
                SqlParameters[23] = new SqlParameter("@Hole4", (object)item.Hole4);
                SqlParameters[24] = new SqlParameter("@Hole5", (object)item.Hole5);
                SqlParameters[25] = new SqlParameter("@Hole6", (object)item.Hole6);
                SqlParameters[26] = new SqlParameter("@StrengthenTimes", (object)item.StrengthenTimes);
                SqlParameters[27] = new SqlParameter("@Hole5Level", (object)item.Hole5Level);
                SqlParameters[28] = new SqlParameter("@Hole5Exp", (object)item.Hole5Exp);
                SqlParameters[29] = new SqlParameter("@Hole6Level", (object)item.Hole6Level);
                SqlParameters[30] = new SqlParameter("@Hole6Exp", (object)item.Hole6Exp);
                SqlParameters[31] = new SqlParameter("@IsGold", (object)item.IsGold);
                SqlParameters[32] = new SqlParameter("@goldValidDate", (object)item.goldValidDate);
                SqlParameters[33] = new SqlParameter("@goldBeginTime", (object)item.goldBeginTime);
                flag = this.db.RunProcedure("SP_Users_Items_Add", SqlParameters);
                item.ItemID = (int)SqlParameters[0].Value;
                item.IsDirty = false;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool AddMarryInfo(MarryInfo info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[5]
                {
          new SqlParameter("@ID", (object) info.ID),
          null,
          null,
          null,
          null
                };
                SqlParameters[0].Direction = ParameterDirection.Output;
                SqlParameters[1] = new SqlParameter("@UserID", (object)info.UserID);
                SqlParameters[2] = new SqlParameter("@IsPublishEquip", (object)info.IsPublishEquip);
                SqlParameters[3] = new SqlParameter("@Introduction", (object)info.Introduction);
                SqlParameters[4] = new SqlParameter("@RegistTime", (object)info.RegistTime);
                flag = this.db.RunProcedure("SP_MarryInfo_Add", SqlParameters);
                info.ID = (int)SqlParameters[0].Value;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(AddMarryInfo), ex);
            }
            return flag;
        }

        public bool AddStore(SqlDataProvider.Data.ItemInfo item)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[14];
                SqlParameters[0] = new SqlParameter("@ItemID", (object)item.ItemID);
                SqlParameters[0].Direction = ParameterDirection.Output;
                SqlParameters[1] = new SqlParameter("@UserID", (object)item.UserID);
                SqlParameters[2] = new SqlParameter("@TemplateID", (object)item.Template.TemplateID);
                SqlParameters[3] = new SqlParameter("@Place", (object)item.Place);
                SqlParameters[4] = new SqlParameter("@AgilityCompose", (object)item.AgilityCompose);
                SqlParameters[5] = new SqlParameter("@AttackCompose", (object)item.AttackCompose);
                SqlParameters[6] = new SqlParameter("@BeginDate", (object)item.BeginDate);
                SqlParameters[7] = new SqlParameter("@Color", item.Color == null ? (object)"" : (object)item.Color);
                SqlParameters[8] = new SqlParameter("@Count", (object)item.Count);
                SqlParameters[9] = new SqlParameter("@DefendCompose", (object)item.DefendCompose);
                SqlParameters[10] = new SqlParameter("@IsBinds", (object)item.IsBinds);
                SqlParameters[11] = new SqlParameter("@IsExist", (object)item.IsExist);
                SqlParameters[12] = new SqlParameter("@IsJudge", (object)item.IsJudge);
                SqlParameters[13] = new SqlParameter("@LuckCompose", (object)item.LuckCompose);
                flag = this.db.RunProcedure("SP_Users_Items_Add", SqlParameters);
                item.ItemID = (int)SqlParameters[0].Value;
                item.IsDirty = false;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool AddUserGemStone(UserGemStone item)
        {
            bool value = false;
            try
            {
                SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@ID", (object)item.ID), null, null, null, null, null };
                sqlParameter[0].Direction = ParameterDirection.Output;
                sqlParameter[1] = new SqlParameter("@UserID", (object)item.UserID);
                sqlParameter[2] = new SqlParameter("@FigSpiritId", (object)item.FigSpiritId);
                sqlParameter[3] = new SqlParameter("@FigSpiritIdValue", item.FigSpiritIdValue);
                sqlParameter[4] = new SqlParameter("@EquipPlace", (object)item.EquipPlace);
                sqlParameter[5] = new SqlParameter("@Result", SqlDbType.Int);
                sqlParameter[5].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Users_GemStones_Add", sqlParameter);
                value = (int)sqlParameter[5].Value == 0;
                item.ID = (int)sqlParameter[0].Value;
                item.IsDirty = false;
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("Init", exception);
                }
            }
            return value;
        }

        public bool AddUserMatchInfo(UserMatchInfo info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[16];
                SqlParameters[0] = new SqlParameter("@ID", (object)info.ID);
                SqlParameters[0].Direction = ParameterDirection.Output;
                SqlParameters[1] = new SqlParameter("@UserID", (object)info.UserID);
                SqlParameters[2] = new SqlParameter("@dailyScore", (object)info.dailyScore);
                SqlParameters[3] = new SqlParameter("@dailyWinCount", (object)info.dailyWinCount);
                SqlParameters[4] = new SqlParameter("@dailyGameCount", (object)info.dailyGameCount);
                SqlParameters[5] = new SqlParameter("@DailyLeagueFirst", (object)info.DailyLeagueFirst);
                SqlParameters[6] = new SqlParameter("@DailyLeagueLastScore", (object)info.DailyLeagueLastScore);
                SqlParameters[7] = new SqlParameter("@weeklyScore", (object)info.weeklyScore);
                SqlParameters[8] = new SqlParameter("@weeklyGameCount", (object)info.weeklyGameCount);
                SqlParameters[9] = new SqlParameter("@weeklyRanking", (object)info.weeklyRanking);
                SqlParameters[10] = new SqlParameter("@addDayPrestge", (object)info.addDayPrestge);
                SqlParameters[11] = new SqlParameter("@totalPrestige", (object)info.totalPrestige);
                SqlParameters[12] = new SqlParameter("@restCount", (object)info.restCount);
                SqlParameters[13] = new SqlParameter("@leagueGrade", (object)info.leagueGrade);
                SqlParameters[14] = new SqlParameter("@leagueItemsGet", (object)info.leagueItemsGet);
                SqlParameters[15] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[15].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_UserMatch_Add", SqlParameters);
                flag = (int)SqlParameters[15].Value == 0;
                info.ID = (int)SqlParameters[0].Value;
                info.IsDirty = false;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool AddUserRank(UserRankInfo item)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[14];
                SqlParameters[0] = new SqlParameter("@ID", (object)item.ID);
                SqlParameters[0].Direction = ParameterDirection.Output;
                SqlParameters[1] = new SqlParameter("@UserID", (object)item.UserID);
                SqlParameters[2] = new SqlParameter("@UserRank", (object)item.UserRank);
                SqlParameters[3] = new SqlParameter("@Attack", (object)item.Attack);
                SqlParameters[4] = new SqlParameter("@Defence", (object)item.Defence);
                SqlParameters[5] = new SqlParameter("@Luck", (object)item.Luck);
                SqlParameters[6] = new SqlParameter("@Agility", (object)item.Agility);
                SqlParameters[7] = new SqlParameter("@HP", (object)item.HP);
                SqlParameters[8] = new SqlParameter("@Damage", (object)item.Damage);
                SqlParameters[9] = new SqlParameter("@Guard", (object)item.Guard);
                SqlParameters[10] = new SqlParameter("@BeginDate", (object)item.BeginDate);
                SqlParameters[11] = new SqlParameter("@Validate", (object)item.Validate);
                SqlParameters[12] = new SqlParameter("@IsExit", (object)item.IsExit);
                SqlParameters[13] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[13].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_UserRank_Add", SqlParameters);
                flag = (int)SqlParameters[13].Value == 0;
                item.ID = (int)SqlParameters[0].Value;
                item.IsDirty = false;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool AddUserUserDrill(UserDrillInfo item)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[6]
                {
          new SqlParameter("@UserID", (object) item.UserID),
          new SqlParameter("@BeadPlace", (object) item.BeadPlace),
          new SqlParameter("@HoleExp", (object) item.HoleExp),
          new SqlParameter("@HoleLv", (object) item.HoleLv),
          new SqlParameter("@DrillPlace", (object) item.DrillPlace),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[5].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Users_UserDrill_Add", SqlParameters);
                flag = (int)SqlParameters[5].Value == 0;
                item.IsDirty = false;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool CancelPaymentMail(int userid, int mailID, ref int senderID)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[4]
                {
          new SqlParameter("@userid", (object) userid),
          new SqlParameter("@mailID", (object) mailID),
          new SqlParameter("@senderID", SqlDbType.Int),
          null
                };
                SqlParameters[2].Value = (object)senderID;
                SqlParameters[2].Direction = ParameterDirection.InputOutput;
                SqlParameters[3] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[3].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Mail_PaymentCancel", SqlParameters);
                flag = (int)SqlParameters[3].Value == 0;
                if (flag)
                    senderID = (int)SqlParameters[2].Value;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool ClearDatabase()
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Sys_Clear_All");
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool ChargeToUser(string userName, ref int money, string nickName)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[3]
                {
          new SqlParameter("@UserName", (object) userName),
          new SqlParameter("@money", SqlDbType.Int),
          null
                };
                SqlParameters[1].Direction = ParameterDirection.Output;
                SqlParameters[2] = new SqlParameter("@NickName", (object)nickName);
                flag = this.db.RunProcedure("SP_Charge_To_User", SqlParameters);
                money = (int)SqlParameters[1].Value;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool CheckAccount(string username, string password)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[3]
                {
          new SqlParameter("@Username", (object) username),
          new SqlParameter("@Password", (object) password),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[2].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_CheckAccount", SqlParameters);
                flag = (int)SqlParameters[2].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool CheckEmailIsValid(string Email)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[2]
                {
          new SqlParameter("@Email", (object) Email),
          new SqlParameter("@count", SqlDbType.BigInt)
                };
                SqlParameters[1].Direction = ParameterDirection.Output;
                this.db.RunProcedure(nameof(CheckEmailIsValid), SqlParameters);
                if (int.Parse(SqlParameters[1].Value.ToString()) == 0)
                    flag = true;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init CheckEmailIsValid", ex);
            }
            return flag;
        }

        public bool DeleteAuction(int auctionID, int userID, ref string msg)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[3]
                {
          new SqlParameter("@AuctionID", (object) auctionID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[2].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Auction_Delete", SqlParameters);
                int num = (int)SqlParameters[2].Value;
                flag = num == 0;
                switch (num)
                {
                    case 0:
                        msg = LanguageMgr.GetTranslation("PlayerBussiness.DeleteAuction.Msg1");
                        break;
                    case 1:
                        msg = LanguageMgr.GetTranslation("PlayerBussiness.DeleteAuction.Msg2");
                        break;
                    case 2:
                        msg = LanguageMgr.GetTranslation("PlayerBussiness.DeleteAuction.Msg3");
                        break;
                    default:
                        msg = LanguageMgr.GetTranslation("PlayerBussiness.DeleteAuction.Msg4");
                        break;
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool DeleteFriends(int UserID, int FriendID)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Users_Friends_Delete", new SqlParameter[2]
                {
          new SqlParameter("@ID", (object) FriendID),
          new SqlParameter("@UserID", (object) UserID)
                });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool DeleteGoods(int itemID)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Users_Items_Delete", new SqlParameter[1]
                {
          new SqlParameter("@ID", (object) itemID)
                });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool DeleteMail(int UserID, int mailID, out int senderID)
        {
            bool flag = false;
            senderID = 0;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[4]
                {
          new SqlParameter("@ID", (object) mailID),
          new SqlParameter("@UserID", (object) UserID),
          new SqlParameter("@SenderID", SqlDbType.Int),
          null
                };
                SqlParameters[2].Value = (object)senderID;
                SqlParameters[2].Direction = ParameterDirection.InputOutput;
                SqlParameters[3] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[3].Direction = ParameterDirection.ReturnValue;
                flag = this.db.RunProcedure("SP_Mail_Delete", SqlParameters);
                if ((int)SqlParameters[3].Value == 0)
                {
                    flag = true;
                    senderID = (int)SqlParameters[2].Value;
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool DeleteMail2(int UserID, int mailID, out int senderID)
        {
            bool flag = false;
            senderID = 0;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[4]
                {
          new SqlParameter("@ID", (object) mailID),
          new SqlParameter("@UserID", (object) UserID),
          new SqlParameter("@SenderID", SqlDbType.Int),
          null
                };
                SqlParameters[2].Value = (object)senderID;
                SqlParameters[2].Direction = ParameterDirection.InputOutput;
                SqlParameters[3] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[3].Direction = ParameterDirection.ReturnValue;
                flag = this.db.RunProcedure("SP_Mail_Delete", SqlParameters);
                if ((int)SqlParameters[3].Value == 0)
                {
                    flag = true;
                    senderID = (int)SqlParameters[2].Value;
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool DeleteMarryInfo(int ID, int userID, ref string msg)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[3]
                {
          new SqlParameter("@ID", (object) ID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[2].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_MarryInfo_Delete", SqlParameters);
                int num = (int)SqlParameters[2].Value;
                flag = num == 0;
                if (num == 0)
                    msg = LanguageMgr.GetTranslation("PlayerBussiness.DeleteAuction.Succeed");
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"DeleteAuction", ex);
            }
            return flag;
        }

        public bool DisableUser(string userName, bool isExit)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[3]
                {
          new SqlParameter("@UserName", (object) userName),
          new SqlParameter("@IsExist", (object) isExit),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[2].Direction = ParameterDirection.ReturnValue;
                flag = this.db.RunProcedure("SP_Disable_User", SqlParameters);
                if ((int)SqlParameters[2].Value == 0)
                    flag = true;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(DisableUser), ex);
            }
            return flag;
        }

        public bool DisposeMarryRoomInfo(int ID)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[2]
                {
          new SqlParameter("@ID", (object) ID),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[1].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Dispose_Marry_Room_Info", SqlParameters);
                flag = (int)SqlParameters[1].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(DisposeMarryRoomInfo), ex);
            }
            return flag;
        }

        public EatPetsInfo GetAllEatPetsByID(int ID)
        {
            SqlDataReader sqlDataReader = null;
            try
            {
                try
                {
                    SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@ID", SqlDbType.Int, 4) };
                    sqlParameter[0].Value = ID;
                    this.db.GetReader(ref sqlDataReader, "SP_Sys_Eat_Pets_All", sqlParameter);
                    if (sqlDataReader.Read())
                    {
                        return this.InitEatPetsInfo(sqlDataReader);
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    if (BaseBussiness.log.IsErrorEnabled)
                    {
                        BaseBussiness.log.Error("InitEatPetsInfo", exception);
                    }
                }
            }
            finally
            {
                if (sqlDataReader != null && !sqlDataReader.IsClosed)
                {
                    sqlDataReader.Close();
                }
            }
            return null;
        }

        public ConsortiaUserInfo[] GetAllMemberByConsortia(int ConsortiaID)
        {
            List<ConsortiaUserInfo> consortiaUserInfoList = new List<ConsortiaUserInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@ConsortiaID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)ConsortiaID;
                this.db.GetReader(ref ResultDataReader, "SP_Consortia_Users_All", SqlParameters);
                while (ResultDataReader.Read())
                    consortiaUserInfoList.Add(this.InitConsortiaUserInfo(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return consortiaUserInfoList.ToArray();
        }

        public UserMatchInfo[] GetAllUserMatchInfo()
        {
            List<UserMatchInfo> userMatchInfoList = new List<UserMatchInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            int num = 1;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_UserMatch_All_DESC");
                while (ResultDataReader.Read())
                {
                    UserMatchInfo userMatchInfo = new UserMatchInfo()
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        totalPrestige = (int)ResultDataReader["totalPrestige"],
                        rank = num
                    };
                    userMatchInfoList.Add(userMatchInfo);
                    ++num;
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"GetAllUserMatchDESC", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return userMatchInfoList.ToArray();
        }

        public AuctionInfo[] GetAuctionPage(
          int page,
          string name,
          int type,
          int pay,
          ref int total,
          int userID,
          int buyID,
          int order,
          bool sort,
          int size,
          string string_1)
        {
            List<AuctionInfo> auctionInfoList = new List<AuctionInfo>();
            try
            {
                string str1 = " IsExist=1 ";
                if (!string.IsNullOrEmpty(name))
                    str1 = str1 + " and Name like '%" + name + "%' ";
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
                        str1 = str1 + " and Category =" + (object)type + " ";
                        break;
                    case 21:
                        str1 += " and Category in(1,2,5,8,9) ";
                        break;
                    case 22:
                        str1 += " and Category in(13,15,6,4,3) ";
                        break;
                    case 23:
                        str1 += " and Category in(16,11,10) ";
                        break;
                    case 24:
                        str1 += " and Category in(8,9) ";
                        break;
                    case 25:
                        str1 += " and Category in (7,17) ";
                        break;
                    case 26:
                        str1 += " and TemplateId>=311000 and TemplateId<=313999";
                        break;
                    case 27:
                        str1 += " and TemplateId>=311000 and TemplateId<=311999 ";
                        break;
                    case 28:
                        str1 += " and TemplateId>=312000 and TemplateId<=312999 ";
                        break;
                    case 29:
                        str1 += " and TemplateId>=313000 and TempLateId<=313999";
                        break;
                    case 35:
                        str1 += " and TemplateID in (11560,11561,11562)";
                        break;
                    case 1100:
                        str1 += " and TemplateID in (11019,11021,11022,11023) ";
                        break;
                    case 1101:
                        str1 += " and TemplateID='11019' ";
                        break;
                    case 1102:
                        str1 += " and TemplateID='11021' ";
                        break;
                    case 1103:
                        str1 += " and TemplateID='11022' ";
                        break;
                    case 1104:
                        str1 += " and TemplateID='11023' ";
                        break;
                    case 1105:
                        str1 += " and TemplateID in (11001,11002,11003,11004,11005,11006,11007,11008,11009,11010,11011,11012,11013,11014,11015,11016) ";
                        break;
                    case 1106:
                        str1 += " and TemplateID in (11001,11002,11003,11004) ";
                        break;
                    case 1107:
                        str1 += " and TemplateID in (11005,11006,11007,11008) ";
                        break;
                    case 1108:
                        str1 += " and TemplateID in (11009,11010,11011,11012) ";
                        break;
                    case 1109:
                        str1 += " and TemplateID in (11013,11014,11015,11016) ";
                        break;
                    case 1110:
                        str1 += " and TemplateID='11024' ";
                        break;
                    case 1111:
                    case 1112:
                        str1 += " and Category in (11) and Property1 = 10";
                        break;
                    case 1113:
                        str1 += " and TemplateID in (314101,314102,314103,314104,314105,314106,314107,314108,314109,314110,314111,314112,314113,314114,314115,314116,314121,314122,314123,314124,314125,314126,314127,314128,314129,314130,314131,314132,314133,314134) ";
                        break;
                    case 1114:
                        str1 += " and TemplateID in (314117,314118,314119,314120,314135,314136,314137,314138,314139) ";
                        break;
                    case 1116:
                        str1 += " and TemplateID='11035' ";
                        break;
                    case 1117:
                        str1 += " and TemplateID='11036' ";
                        break;
                    case 1118:
                        str1 += " and TemplateID='11026' ";
                        break;
                    case 1119:
                        str1 += " and TemplateID='11027' ";
                        break;
                }
                if (pay != -1)
                    str1 = str1 + " and PayType =" + (object)pay + " ";
                if (userID != -1)
                    str1 = str1 + " and AuctioneerID =" + (object)userID + " ";
                if (buyID != -1)
                    str1 = str1 + " and (BuyerID =" + (object)buyID + " or AuctionID in (" + string_1 + ")) ";
                string str2 = "Category,Name,Price,dd,AuctioneerID";
                switch (order)
                {
                    case 0:
                        str2 = "Name";
                        break;
                    case 2:
                        str2 = "dd";
                        break;
                    case 3:
                        str2 = "AuctioneerName";
                        break;
                    case 4:
                        str2 = "Price";
                        break;
                    case 5:
                        str2 = "BuyerName";
                        break;
                }
                string str3 = str2 + (sort ? " desc" : "") + ",AuctionID ";
                SqlParameter[] SqlParameters = new SqlParameter[8]
                {
          new SqlParameter("@QueryStr", (object) "V_Auction_Scan"),
          new SqlParameter("@QueryWhere", (object) str1),
          new SqlParameter("@PageSize", (object) size),
          new SqlParameter("@PageCurrent", (object) page),
          new SqlParameter("@FdShow", (object) "*"),
          new SqlParameter("@FdOrder", (object) str3),
          new SqlParameter("@FdKey", (object) "AuctionID"),
          new SqlParameter("@TotalRow", (object) total)
                };
                SqlParameters[7].Direction = ParameterDirection.Output;
                DataTable dataTable = this.db.GetDataTable("Auction", "SP_CustomPage", SqlParameters);
                total = (int)SqlParameters[7].Value;
                foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                    auctionInfoList.Add(new AuctionInfo()
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
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return auctionInfoList.ToArray();
        }

        public AuctionInfo GetAuctionSingle(int auctionID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@AuctionID", (object) auctionID)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Auction_Single", SqlParameters);
                if (ResultDataReader.Read())
                    return this.InitAuctionInfo(ResultDataReader);
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (AuctionInfo)null;
        }

        public BestEquipInfo[] GetCelebByDayBestEquip()
        {
            List<BestEquipInfo> bestEquipInfoList = new List<BestEquipInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Users_BestEquip");
                while (ResultDataReader.Read())
                {
                    BestEquipInfo bestEquipInfo = new BestEquipInfo()
                    {
                        Date = (DateTime)ResultDataReader["RemoveDate"],
                        GP = (int)ResultDataReader["GP"],
                        Grade = (int)ResultDataReader["Grade"],
                        ItemName = ResultDataReader["Name"] == null ? "" : ResultDataReader["Name"].ToString(),
                        NickName = ResultDataReader["NickName"] == null ? "" : ResultDataReader["NickName"].ToString(),
                        Sex = (bool)ResultDataReader["Sex"],
                        Strengthenlevel = (int)ResultDataReader["Strengthenlevel"],
                        UserName = ResultDataReader["UserName"] == null ? "" : ResultDataReader["UserName"].ToString()
                    };
                    bestEquipInfoList.Add(bestEquipInfo);
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return bestEquipInfoList.ToArray();
        }

        public ChargeRecordInfo[] GetChargeRecordInfo(
          DateTime date,
          int SaveRecordSecond)
        {
            List<ChargeRecordInfo> chargeRecordInfoList = new List<ChargeRecordInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[2]
                {
          new SqlParameter("@Date", (object) date.ToString("yyyy-MM-dd HH:mm:ss")),
          new SqlParameter("@Second", (object) SaveRecordSecond)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Charge_Record", SqlParameters);
                while (ResultDataReader.Read())
                {
                    ChargeRecordInfo chargeRecordInfo = new ChargeRecordInfo()
                    {
                        BoyTotalPay = (int)ResultDataReader["BoyTotalPay"],
                        GirlTotalPay = (int)ResultDataReader["GirlTotalPay"],
                        PayWay = ResultDataReader["PayWay"] == null ? "" : ResultDataReader["PayWay"].ToString(),
                        TotalBoy = (int)ResultDataReader["TotalBoy"],
                        TotalGirl = (int)ResultDataReader["TotalGirl"]
                    };
                    chargeRecordInfoList.Add(chargeRecordInfo);
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return chargeRecordInfoList.ToArray();
        }

        public ExerciseInfo GetExerciseSingle(int Grade)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@Grage", (object) Grade)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Get_Exercise_By_Grade", SqlParameters);
                if (ResultDataReader.Read())
                    return new ExerciseInfo()
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
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"GetExerciseInfoSingle", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (ExerciseInfo)null;
        }

        public FriendInfo[] GetFriendsAll(int UserID)
        {
            List<FriendInfo> friendInfoList = new List<FriendInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)UserID;
                this.db.GetReader(ref ResultDataReader, "SP_Users_Friends", SqlParameters);
                while (ResultDataReader.Read())
                    friendInfoList.Add(new FriendInfo()
                    {
                        AddDate = (DateTime)ResultDataReader["AddDate"],
                        Colors = ResultDataReader["Colors"] == null ? "" : ResultDataReader["Colors"].ToString(),
                        FriendID = (int)ResultDataReader["FriendID"],
                        Grade = (int)ResultDataReader["Grade"],
                        Hide = (int)ResultDataReader["Hide"],
                        ID = (int)ResultDataReader["ID"],
                        IsExist = (bool)ResultDataReader["IsExist"],
                        NickName = ResultDataReader["NickName"] == null ? "" : ResultDataReader["NickName"].ToString(),
                        Remark = ResultDataReader["Remark"] == null ? "" : ResultDataReader["Remark"].ToString(),
                        Sex = (bool)ResultDataReader["Sex"] ? 1 : 0,
                        State = (int)ResultDataReader["State"],
                        Style = ResultDataReader["Style"] == null ? "" : ResultDataReader["Style"].ToString(),
                        UserID = (int)ResultDataReader[nameof(UserID)],
                        ConsortiaName = ResultDataReader["ConsortiaName"] == null ? "" : ResultDataReader["ConsortiaName"].ToString(),
                        Offer = (int)ResultDataReader["Offer"],
                        Win = (int)ResultDataReader["Win"],
                        Total = (int)ResultDataReader["Total"],
                        Escape = (int)ResultDataReader["Escape"],
                        Relation = (int)ResultDataReader["Relation"],
                        Repute = (int)ResultDataReader["Repute"],
                        UserName = ResultDataReader["UserName"] == null ? "" : ResultDataReader["UserName"].ToString(),
                        DutyName = ResultDataReader["DutyName"] == null ? "" : ResultDataReader["DutyName"].ToString(),
                        Nimbus = (int)ResultDataReader["Nimbus"],
                        apprenticeshipState = (int)ResultDataReader["apprenticeshipState"]
                    });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return friendInfoList.ToArray();
        }

        public FriendInfo[] GetFriendsBbs(string condictArray)
        {
            List<FriendInfo> friendInfoList = new List<FriendInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@SearchUserName", SqlDbType.NVarChar, 4000)
                };
                SqlParameters[0].Value = (object)condictArray;
                this.db.GetReader(ref ResultDataReader, "SP_Users_FriendsBbs", SqlParameters);
                while (ResultDataReader.Read())
                {
                    FriendInfo friendInfo = new FriendInfo()
                    {
                        NickName = ResultDataReader["NickName"] == null ? "" : ResultDataReader["NickName"].ToString(),
                        UserID = (int)ResultDataReader["UserID"],
                        UserName = ResultDataReader["UserName"] == null ? "" : ResultDataReader["UserName"].ToString(),
                        IsExist = (int)ResultDataReader["UserID"] > 0
                    };
                    friendInfoList.Add(friendInfo);
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return friendInfoList.ToArray();
        }

        public ArrayList GetFriendsGood(string UserName)
        {
            ArrayList arrayList = new ArrayList();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserName", SqlDbType.NVarChar)
                };
                SqlParameters[0].Value = (object)UserName;
                this.db.GetReader(ref ResultDataReader, "SP_Users_Friends_Good", SqlParameters);
                while (ResultDataReader.Read())
                    arrayList.Add(ResultDataReader[nameof(UserName)] == null ? (object)"" : (object)ResultDataReader[nameof(UserName)].ToString());
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return arrayList;
        }

        public Dictionary<int, int> GetFriendsIDAll(int UserID)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)UserID;
                this.db.GetReader(ref ResultDataReader, "SP_Users_Friends_All", SqlParameters);
                while (ResultDataReader.Read())
                {
                    if (!dictionary.ContainsKey((int)ResultDataReader["FriendID"]))
                        dictionary.Add((int)ResultDataReader["FriendID"], (int)ResultDataReader["Relation"]);
                    else
                        dictionary[(int)ResultDataReader["FriendID"]] = (int)ResultDataReader["Relation"];
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return dictionary;
        }

        public MailInfo[] GetMailBySenderID(int userID)
        {
            List<MailInfo> mailInfoList = new List<MailInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)userID;
                this.db.GetReader(ref ResultDataReader, "SP_Mail_BySenderID", SqlParameters);
                while (ResultDataReader.Read())
                    mailInfoList.Add(this.InitMail(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return mailInfoList.ToArray();
        }

        public MailInfo[] GetMailByUserID(int userID)
        {
            List<MailInfo> mailInfoList = new List<MailInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)userID;
                this.db.GetReader(ref ResultDataReader, "SP_Mail_ByUserID", SqlParameters);
                while (ResultDataReader.Read())
                    mailInfoList.Add(this.InitMail(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return mailInfoList.ToArray();
        }

        public MailInfo GetMailSingle(int UserID, int mailID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[2]
                {
          new SqlParameter("@ID", (object) mailID),
          new SqlParameter("@UserID", (object) UserID)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Mail_Single", SqlParameters);
                if (ResultDataReader.Read())
                    return this.InitMail(ResultDataReader);
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (MailInfo)null;
        }

        public MarryInfo[] GetMarryInfoPage(
          int page,
          string name,
          bool sex,
          int size,
          ref int total)
        {
            List<MarryInfo> marryInfoList = new List<MarryInfo>();
            try
            {
                string str1 = !sex ? " IsExist=1 and Sex=0 and UserExist=1" : " IsExist=1 and Sex=1 and UserExist=1";
                if (!string.IsNullOrEmpty(name))
                    str1 = str1 + " and NickName like '%" + name + "%' ";
                string str2 = "State desc,IsMarried";
                SqlParameter[] SqlParameters = new SqlParameter[8]
                {
          new SqlParameter("@QueryStr", (object) "V_Sys_Marry_Info"),
          new SqlParameter("@QueryWhere", (object) str1),
          new SqlParameter("@PageSize", (object) size),
          new SqlParameter("@PageCurrent", (object) page),
          new SqlParameter("@FdShow", (object) "*"),
          new SqlParameter("@FdOrder", (object) str2),
          new SqlParameter("@FdKey", (object) "ID"),
          new SqlParameter("@TotalRow", (object) total)
                };
                SqlParameters[7].Direction = ParameterDirection.Output;
                DataTable dataTable = this.db.GetDataTable("V_Sys_Marry_Info", "SP_CustomPage", SqlParameters);
                total = (int)SqlParameters[7].Value;
                foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                {
                    MarryInfo marryInfo = new MarryInfo()
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
                    marryInfoList.Add(marryInfo);
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return marryInfoList.ToArray();
        }

        public MarryInfo GetMarryInfoSingle(int ID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@ID", (object) ID)
                };
                this.db.GetReader(ref ResultDataReader, "SP_MarryInfo_Single", SqlParameters);
                if (ResultDataReader.Read())
                    return new MarryInfo()
                    {
                        ID = (int)ResultDataReader[nameof(ID)],
                        UserID = (int)ResultDataReader["UserID"],
                        IsPublishEquip = (bool)ResultDataReader["IsPublishEquip"],
                        Introduction = ResultDataReader["Introduction"].ToString(),
                        RegistTime = (DateTime)ResultDataReader["RegistTime"]
                    };
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(GetMarryInfoSingle), ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (MarryInfo)null;
        }

        public MarryProp GetMarryProp(int id)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", (object) id)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Select_Marry_Prop", SqlParameters);
                if (ResultDataReader.Read())
                    return new MarryProp()
                    {
                        IsMarried = (bool)ResultDataReader["IsMarried"],
                        SpouseID = (int)ResultDataReader["SpouseID"],
                        SpouseName = ResultDataReader["SpouseName"].ToString(),
                        IsCreatedMarryRoom = (bool)ResultDataReader["IsCreatedMarryRoom"],
                        SelfMarryRoomID = (int)ResultDataReader["SelfMarryRoomID"],
                        IsGotRing = (bool)ResultDataReader["IsGotRing"]
                    };
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(GetMarryProp), ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (MarryProp)null;
        }

        public MarryRoomInfo[] GetMarryRoomInfo()
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            List<MarryRoomInfo> marryRoomInfoList = new List<MarryRoomInfo>();
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Get_Marry_Room_Info");
                while (ResultDataReader.Read())
                {
                    MarryRoomInfo marryRoomInfo = new MarryRoomInfo()
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
                    marryRoomInfoList.Add(marryRoomInfo);
                }
                return marryRoomInfoList.ToArray();
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(GetMarryRoomInfo), ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (MarryRoomInfo[])null;
        }

        public MarryRoomInfo GetMarryRoomInfoSingle(int id)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@ID", (object) id)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Get_Marry_Room_Info_Single", SqlParameters);
                if (ResultDataReader.Read())
                    return new MarryRoomInfo()
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
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"GetMarryRoomInfo", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (MarryRoomInfo)null;
        }

        public void GetPasswordInfo(
          int userID,
          ref string PasswordQuestion1,
          ref string PasswordAnswer1,
          ref string PasswordQuestion2,
          ref string PasswordAnswer2,
          ref int Count)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", (object) userID)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Users_PasswordInfo", SqlParameters);
                while (ResultDataReader.Read())
                {
                    PasswordQuestion1 = ResultDataReader[nameof(PasswordQuestion1)] == null ? "" : ResultDataReader[nameof(PasswordQuestion1)].ToString();
                    PasswordAnswer1 = ResultDataReader[nameof(PasswordAnswer1)] == null ? "" : ResultDataReader[nameof(PasswordAnswer1)].ToString();
                    PasswordQuestion2 = ResultDataReader[nameof(PasswordQuestion2)] == null ? "" : ResultDataReader[nameof(PasswordQuestion2)].ToString();
                    PasswordAnswer2 = ResultDataReader[nameof(PasswordAnswer2)] == null ? "" : ResultDataReader[nameof(PasswordAnswer2)].ToString();
                    Count = !((DateTime)ResultDataReader["LastFindDate"] == DateTime.Today) ? 5 : (int)ResultDataReader["FailedPasswordAttemptCount"];
                }
            }
            catch (Exception ex)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                    return;
                BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
        }

        public MarryApplyInfo[] GetPlayerMarryApply(int UserID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            List<MarryApplyInfo> marryApplyInfoList = new List<MarryApplyInfo>();
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", (object) UserID)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Get_Marry_Apply", SqlParameters);
                while (ResultDataReader.Read())
                {
                    MarryApplyInfo marryApplyInfo = new MarryApplyInfo()
                    {
                        UserID = (int)ResultDataReader[nameof(UserID)],
                        ApplyUserID = (int)ResultDataReader["ApplyUserID"],
                        ApplyUserName = ResultDataReader["ApplyUserName"].ToString(),
                        ApplyType = (int)ResultDataReader["ApplyType"],
                        ApplyResult = (bool)ResultDataReader["ApplyResult"],
                        LoveProclamation = ResultDataReader["LoveProclamation"].ToString(),
                        ID = (int)ResultDataReader["Id"]
                    };
                    marryApplyInfoList.Add(marryApplyInfo);
                }
                return marryApplyInfoList.ToArray();
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(GetPlayerMarryApply), ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (MarryApplyInfo[])null;
        }

        public PlayerInfo[] GetPlayerMathPage(
          int page,
          int size,
          ref int total,
          ref bool resultValue)
        {
            List<PlayerInfo> playerInfoList = new List<PlayerInfo>();
            try
            {
                string queryWhere = "  ";
                string fdOreder = "weeklyScore desc";
                foreach (DataRow row in (InternalDataCollectionBase)this.GetPage("V_Sys_Users_Math", queryWhere, page, size, "*", fdOreder, "UserID", ref total).Rows)
                {
                    PlayerInfo playerInfo = new PlayerInfo()
                    {
                        ID = (int)row["UserID"],
                        Colors = row["Colors"] == null ? "" : row["Colors"].ToString(),
                        GP = (int)row["GP"],
                        Grade = (int)row["Grade"]
                    };
                    playerInfo.ID = (int)row["UserID"];
                    playerInfo.NickName = row["NickName"] == null ? "" : row["NickName"].ToString();
                    playerInfo.Sex = (bool)row["Sex"];
                    playerInfo.State = (int)row["State"];
                    playerInfo.Style = row["Style"] == null ? "" : row["Style"].ToString();
                    playerInfo.Hide = (int)row["Hide"];
                    playerInfo.Repute = (int)row["Repute"];
                    playerInfo.UserName = row["UserName"] == null ? "" : row["UserName"].ToString();
                    playerInfo.Skin = row["Skin"] == null ? "" : row["Skin"].ToString();
                    playerInfo.Win = (int)row["Win"];
                    playerInfo.Total = (int)row["Total"];
                    playerInfo.Nimbus = (int)row["Nimbus"];
                    playerInfo.FightPower = (int)row["FightPower"];
                    playerInfo.AchievementPoint = (int)row["AchievementPoint"];
                    playerInfo.typeVIP = Convert.ToByte(row["typeVIP"]);
                    playerInfo.VIPLevel = (int)row["VIPLevel"];
                    playerInfo.AddWeekLeagueScore = (int)row["weeklyScore"];
                    playerInfoList.Add(playerInfo);
                }
                resultValue = true;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return playerInfoList.ToArray();
        }

        public PlayerInfo[] GetPlayerPage(int page, int size, ref int total, int order, int userID, ref bool resultValue)
        {
            return GetPlayerPage(page, size, ref total, order, 0, userID, "UserID", ref resultValue);
        }

        public PlayerInfo[] GetPlayerPage(int page,int size,ref int total,int order,int where,int userID,string key,ref bool resultValue)
        {
            List<PlayerInfo> playerInfoList = new List<PlayerInfo>();
            try
            {
                string queryWhere = " IsExist=1 and IsFirst<> 0 ";
                if (userID != -1)
                    queryWhere = queryWhere + " and UserID =" + (object)userID + " ";
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
                        queryWhere += " ";
                        break;
                    case 1:
                        queryWhere += " and Grade >= 20 ";
                        break;
                    case 2:
                        queryWhere += " and Grade > 5 and Grade < 17 ";
                        break;
                    case 3:
                        queryWhere += " and Grade >= 20 and apprenticeshipState != 3 and State = 1 ";
                        break;
                    case 4:
                        queryWhere += " and Grade > 5 and Grade < 17 and masterID = 0 and State = 1 ";
                        break;
                }
                string fdOreder = str + ",UserID";
                foreach (DataRow row in (InternalDataCollectionBase)this.GetPage("V_Sys_Users_Detail", queryWhere, page, size, "*", fdOreder, "UserID", ref total).Rows)
                {
                    PlayerInfo playerInfo = new PlayerInfo();
                    playerInfo.Agility = (int)row["Agility"];
                    playerInfo.Attack = (int)row["Attack"];
                    playerInfo.Colors = ((row["Colors"] == null) ? "" : row["Colors"].ToString());
                    playerInfo.ConsortiaID = (int)row["ConsortiaID"];
                    playerInfo.Defence = (int)row["Defence"];
                    playerInfo.Gold = (int)row["Gold"];
                    playerInfo.GP = (int)row["GP"];
                    playerInfo.Grade = (int)row["Grade"];
                    playerInfo.ID = (int)row["UserID"];
                    playerInfo.Luck = (int)row["Luck"];
                    playerInfo.Money = (int)row["Money"];
                    playerInfo.NickName = ((row["NickName"] == null) ? "" : row["NickName"].ToString());
                    playerInfo.Sex = (bool)row["Sex"];
                    playerInfo.State = (int)row["State"];
                    playerInfo.Style = ((row["Style"] == null) ? "" : row["Style"].ToString());
                    playerInfo.Hide = (int)row["Hide"];
                    playerInfo.Repute = (int)row["Repute"];
                    playerInfo.UserName = ((row["UserName"] == null) ? "" : row["UserName"].ToString());
                    playerInfo.ConsortiaName = ((row["ConsortiaName"] == null) ? "" : row["ConsortiaName"].ToString());
                    playerInfo.Offer = (int)row["Offer"];
                    playerInfo.Skin = ((row["Skin"] == null) ? "" : row["Skin"].ToString());
                    playerInfo.IsBanChat = (bool)row["IsBanChat"];
                    playerInfo.ReputeOffer = (int)row["ReputeOffer"];
                    playerInfo.ConsortiaRepute = (int)row["ConsortiaRepute"];
                    playerInfo.ConsortiaLevel = (int)row["ConsortiaLevel"];
                    playerInfo.StoreLevel = (int)row["StoreLevel"];
                    playerInfo.ShopLevel = (int)row["ShopLevel"];
                    playerInfo.SmithLevel = (int)row["SmithLevel"];
                    playerInfo.ConsortiaHonor = (int)row["ConsortiaHonor"];
                    playerInfo.RichesOffer = (int)row["RichesOffer"];
                    playerInfo.RichesRob = (int)row["RichesRob"];
                    playerInfo.DutyLevel = (int)row["DutyLevel"];
                    playerInfo.DutyName = ((row["DutyName"] == null) ? "" : row["DutyName"].ToString());
                    playerInfo.Right = (int)row["Right"];
                    playerInfo.ChairmanName = ((row["ChairmanName"] == null) ? "" : row["ChairmanName"].ToString());
                    playerInfo.Win = (int)row["Win"];
                    playerInfo.Total = (int)row["Total"];
                    playerInfo.Escape = (int)row["Escape"];
                    playerInfo.AddDayGP = (((int)row["AddDayGP"] == 0) ? playerInfo.GP : ((int)row["AddDayGP"]));
                    playerInfo.AddDayOffer = (((int)row["AddDayOffer"] == 0) ? playerInfo.Offer : ((int)row["AddDayOffer"]));
                    playerInfo.AddWeekGP = (((int)row["AddWeekGP"] == 0) ? playerInfo.GP : ((int)row["AddWeekyGP"]));
                    playerInfo.AddWeekOffer = (((int)row["AddWeekOffer"] == 0) ? playerInfo.Offer : ((int)row["AddWeekOffer"]));
                    playerInfo.ConsortiaRiches = (int)row["ConsortiaRiches"];
                    playerInfo.CheckCount = (int)row["CheckCount"];
                    playerInfo.Nimbus = (int)row["Nimbus"];
                    playerInfo.GiftToken = (int)row["GiftToken"];
                    playerInfo.QuestSite = ((row["QuestSite"] == null) ? new byte[200] : ((byte[])row["QuestSite"]));
                    playerInfo.PvePermission = ((row["PvePermission"] == null) ? "" : row["PvePermission"].ToString());
                    playerInfo.FightPower = (int)row["FightPower"];
                    playerInfo.AchievementPoint = (int)row["AchievementPoint"];
                    playerInfo.AddWeekLeagueScore = (int)row["AddWeekLeagueScore"];
                    playerInfo.WeekLeagueRanking = (int)row["WeekLeagueRanking"];
                    playerInfo.apprenticeshipState = (int)row["apprenticeshipState"];
                    playerInfo.masterID = (int)row["masterID"];
                    playerInfo.graduatesCount = (int)row["graduatesCount"];
                    playerInfo.masterOrApprentices = ((row["masterOrApprentices"] == DBNull.Value) ? "" : row["masterOrApprentices"].ToString());
                    playerInfo.honourOfMaster = ((row["honourOfMaster"] == DBNull.Value) ? "" : row["honourOfMaster"].ToString());
                    playerInfo.typeVIP = Convert.ToByte(row["typeVIP"]);
                    playerInfo.VIPLevel = (int)row["VIPLevel"];
                    playerInfoList.Add(playerInfo);
                }
                resultValue = true;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return playerInfoList.ToArray();
        }

        public string GetSingleRandomName(int sex)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                if (sex > 1)
                    sex = 1;
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@Sex", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)sex;
                this.db.GetReader(ref ResultDataReader, "SP_GetSingle_RandomName", SqlParameters);
                if (ResultDataReader.Read())
                    return ResultDataReader["Name"] == null ? "unknown" : ResultDataReader["Name"].ToString();
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(GetSingleRandomName), ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (string)null;
        }

        public UserMatchInfo GetSingleUserMatchInfo(int UserID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)UserID;
                this.db.GetReader(ref ResultDataReader, "SP_GetSingleUserMatchInfo", SqlParameters);
                if (ResultDataReader.Read())
                    return new UserMatchInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        UserID = (int)ResultDataReader[nameof(UserID)],
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
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_GetSingleUserMatchInfo", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (UserMatchInfo)null;
        }

        public List<UserRankInfo> GetSingleUserRank(int UserID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            List<UserRankInfo> userRankInfoList = new List<UserRankInfo>();
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)UserID;
                this.db.GetReader(ref ResultDataReader, "SP_GetSingleUserRank", SqlParameters);
                while (ResultDataReader.Read())
                {
                    UserRankInfo userRankInfo = new UserRankInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        UserID = (int)ResultDataReader[nameof(UserID)],
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
                    userRankInfoList.Add(userRankInfo);
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_GetSingleUserRankInfo", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return userRankInfoList;
        }

        public UsersExtraInfo GetSingleUsersExtra(int UserID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)UserID;
                this.db.GetReader(ref ResultDataReader, "SP_GetSingleUsersExtra", SqlParameters);
                if (ResultDataReader.Read())
                    return new UsersExtraInfo()
                    {
                        UserID = (int)ResultDataReader[nameof(UserID)],
                        LastTimeHotSpring = (DateTime)ResultDataReader["LastTimeHotSpring"],
                        LastFreeTimeHotSpring = (DateTime)ResultDataReader["LastFreeTimeHotSpring"],
                        MinHotSpring = (int)ResultDataReader["MinHotSpring"],
                        coupleBossEnterNum = (int)ResultDataReader["coupleBossEnterNum"],
                        coupleBossHurt = (int)ResultDataReader["coupleBossHurt"],
                        coupleBossBoxNum = (int)ResultDataReader["coupleBossBoxNum"]
                    };
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_GetSingleUsersExtra", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (UsersExtraInfo)null;
        }

        public AchievementData[] GetUserAchievement(int userID)
        {
            List<AchievementData> achievementDataList = new List<AchievementData>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)userID;
                this.db.GetReader(ref ResultDataReader, "SP_Get_User_AchievementData", SqlParameters);
                while (ResultDataReader.Read())
                {
                    AchievementData achievementData = new AchievementData()
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        AchievementID = (int)ResultDataReader["AchievementID"],
                        IsComplete = (bool)ResultDataReader["IsComplete"],
                        CompletedDate = (DateTime)ResultDataReader["CompletedDate"]
                    };
                    achievementDataList.Add(achievementData);
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return achievementDataList.ToArray();
        }

        public SqlDataProvider.Data.ItemInfo[] GetUserBagByType(int UserID, int bagType)
        {
            List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[2]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4),
          null
                };
                SqlParameters[0].Value = (object)UserID;
                SqlParameters[1] = new SqlParameter("@BagType", (object)bagType);
                this.db.GetReader(ref ResultDataReader, "SP_Users_BagByType", SqlParameters);
                while (ResultDataReader.Read())
                    itemInfoList.Add(this.InitItem(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return itemInfoList.ToArray();
        }

        public List<SqlDataProvider.Data.ItemInfo> GetUserBeadEuqip(int UserID)
        {
            List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)UserID;
                this.db.GetReader(ref ResultDataReader, "SP_Users_Bead_Equip", SqlParameters);
                while (ResultDataReader.Read())
                    itemInfoList.Add(this.InitItem(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return itemInfoList;
        }

        public BufferInfo[] GetUserBuffer(int userID)
        {
            List<BufferInfo> bufferInfoList = new List<BufferInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)userID;
                this.db.GetReader(ref ResultDataReader, "SP_User_Buff_All", SqlParameters);
                while (ResultDataReader.Read())
                {
                    BufferInfo bufferInfo1 = new BufferInfo();
                    bufferInfo1.BeginDate = (DateTime)ResultDataReader["BeginDate"];
                    bufferInfo1.Data = ResultDataReader["Data"] == null ? "" : ResultDataReader["Data"].ToString();
                    bufferInfo1.Type = (int)ResultDataReader["Type"];
                    bufferInfo1.UserID = (int)ResultDataReader["UserID"];
                    bufferInfo1.ValidDate = (int)ResultDataReader["ValidDate"];
                    bufferInfo1.Value = (int)ResultDataReader["Value"];
                    bufferInfo1.IsExist = (bool)ResultDataReader["IsExist"];
                    bufferInfo1.ValidCount = (int)ResultDataReader["ValidCount"];
                    bufferInfo1.TemplateID = (int)ResultDataReader["TemplateID"];
                    bufferInfo1.IsDirty = false;
                    BufferInfo bufferInfo2 = bufferInfo1;
                    bufferInfoList.Add(bufferInfo2);
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return bufferInfoList.ToArray();
        }

        public UsersCardInfo GetUserCardByPlace(int Place)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@Place", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)Place;
                this.db.GetReader(ref ResultDataReader, "SP_Get_UserCard_By_Place", SqlParameters);
                if (ResultDataReader.Read())
                    return this.InitCard(ResultDataReader);
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (UsersCardInfo)null;
        }

        public List<UsersCardInfo> GetUserCardEuqip(int UserID)
        {
            List<UsersCardInfo> usersCardInfoList = new List<UsersCardInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)UserID;
                this.db.GetReader(ref ResultDataReader, "SP_Users_Items_Card_Equip", SqlParameters);
                while (ResultDataReader.Read())
                    usersCardInfoList.Add(this.InitCard(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return usersCardInfoList;
        }

        public UsersCardInfo[] GetUserCardSingles(int UserID)
        {
            List<UsersCardInfo> usersCardInfoList = new List<UsersCardInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)UserID;
                this.db.GetReader(ref ResultDataReader, "SP_Get_UserCard_By_ID", SqlParameters);
                while (ResultDataReader.Read())
                    usersCardInfoList.Add(this.InitCard(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return usersCardInfoList.ToArray();
        }

        public ConsortiaBufferInfo[] GetUserConsortiaBuffer(int ConsortiaID)
        {
            List<ConsortiaBufferInfo> consortiaBufferInfoList = new List<ConsortiaBufferInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@ConsortiaID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)ConsortiaID;
                this.db.GetReader(ref ResultDataReader, "SP_User_Consortia_Buff_All", SqlParameters);
                while (ResultDataReader.Read())
                    consortiaBufferInfoList.Add(new ConsortiaBufferInfo()
                    {
                        ConsortiaID = (int)ResultDataReader[nameof(ConsortiaID)],
                        BufferID = (int)ResultDataReader["BufferID"],
                        IsOpen = (bool)ResultDataReader["IsOpen"],
                        BeginDate = (DateTime)ResultDataReader["BeginDate"],
                        ValidDate = (int)ResultDataReader["ValidDate"],
                        Type = (int)ResultDataReader["Type"],
                        Value = (int)ResultDataReader["Value"],
                        Group = (int)ResultDataReader["Group"]
                    });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init SP_User_Consortia_Buff_All", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return consortiaBufferInfoList.ToArray();
        }

        public ConsortiaBufferInfo[] GetUserConsortiaBufferLess(
          int ConsortiaID,
          int LessID)
        {
            List<ConsortiaBufferInfo> consortiaBufferInfoList = new List<ConsortiaBufferInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[2]
                {
          new SqlParameter("@ConsortiaID", SqlDbType.Int, 4),
          null
                };
                SqlParameters[0].Value = (object)ConsortiaID;
                SqlParameters[1] = new SqlParameter("@LessID", (object)LessID);
                this.db.GetReader(ref ResultDataReader, "SP_User_Consortia_Buff_AllL", SqlParameters);
                while (ResultDataReader.Read())
                    consortiaBufferInfoList.Add(new ConsortiaBufferInfo()
                    {
                        ConsortiaID = (int)ResultDataReader[nameof(ConsortiaID)],
                        BufferID = (int)ResultDataReader["BufferID"],
                        IsOpen = (bool)ResultDataReader["IsOpen"],
                        BeginDate = (DateTime)ResultDataReader["BeginDate"],
                        ValidDate = (int)ResultDataReader["ValidDate"],
                        Type = (int)ResultDataReader["Type"],
                        Value = (int)ResultDataReader["Value"],
                        Group = (int)ResultDataReader["Group"]
                    });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init SP_User_Consortia_Buff_AllL", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return consortiaBufferInfoList.ToArray();
        }

        public ConsortiaBufferInfo GetUserConsortiaBufferSingle(int ID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@ID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)ID;
                this.db.GetReader(ref ResultDataReader, "SP_User_Consortia_Buff_Single", SqlParameters);
                if (ResultDataReader.Read())
                    return new ConsortiaBufferInfo()
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
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init SP_User_Consortia_Buff_Single", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (ConsortiaBufferInfo)null;
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

        public List<SqlDataProvider.Data.ItemInfo> GetUserEuqipByNick(string Nick)
        {
            List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@NickName", SqlDbType.NVarChar, 200)
                };
                SqlParameters[0].Value = (object)Nick;
                this.db.GetReader(ref ResultDataReader, "SP_Users_Items_Equip_By_Nick", SqlParameters);
                while (ResultDataReader.Read())
                    itemInfoList.Add(this.InitItem(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return itemInfoList;
        }

        public EventRewardProcessInfo[] GetUserEventProcess(int userID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            List<EventRewardProcessInfo> rewardProcessInfoList = new List<EventRewardProcessInfo>();
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)userID;
                this.db.GetReader(ref ResultDataReader, "SP_Get_User_EventProcess", SqlParameters);
                while (ResultDataReader.Read())
                {
                    EventRewardProcessInfo rewardProcessInfo = new EventRewardProcessInfo()
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        ActiveType = (int)ResultDataReader["ActiveType"],
                        Conditions = (int)ResultDataReader["Conditions"],
                        AwardGot = (int)ResultDataReader["AwardGot"]
                    };
                    rewardProcessInfoList.Add(rewardProcessInfo);
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return rewardProcessInfoList.ToArray();
        }

        public UserInfo GetUserInfo(int UserId)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            UserInfo userInfo = new UserInfo()
            {
                UserID = UserId
            };
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", (object) UserId)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Get_User_Info", SqlParameters);
                while (ResultDataReader.Read())
                {
                    userInfo.UserID = int.Parse(ResultDataReader["UserID"].ToString());
                    userInfo.UserEmail = ResultDataReader["UserEmail"] == null ? "" : ResultDataReader["UserEmail"].ToString();
                    userInfo.UserPhone = ResultDataReader["UserPhone"] == null ? "" : ResultDataReader["UserPhone"].ToString();
                    userInfo.UserOther1 = ResultDataReader["UserOther1"] == null ? "" : ResultDataReader["UserOther1"].ToString();
                    userInfo.UserOther2 = ResultDataReader["UserOther2"] == null ? "" : ResultDataReader["UserOther2"].ToString();
                    userInfo.UserOther3 = ResultDataReader["UserOther3"] == null ? "" : ResultDataReader["UserOther3"].ToString();
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return userInfo;
        }

        public SqlDataProvider.Data.ItemInfo[] GetUserItem(int UserID)
        {
            List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)UserID;
                this.db.GetReader(ref ResultDataReader, "SP_Users_Items_All", SqlParameters);
                while (ResultDataReader.Read())
                    itemInfoList.Add(this.InitItem(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return itemInfoList.ToArray();
        }

        //public SqlDataProvider.Data.ItemInfo GetUserItemSingle(int itemID)
        //{
        //    SqlDataReader ResultDataReader = (SqlDataReader)null;
        //    try
        //    {
        //        SqlParameter[] SqlParameters = new SqlParameter[1]
        //        {
        //  new SqlParameter("@ID", SqlDbType.Int, 4)
        //        };
        //        SqlParameters[0].Value = (object)itemID;
        //        this.db.GetReader(ref ResultDataReader, "SP_Users_Items_Single", SqlParameters);
        //        if (ResultDataReader.Read())
        //            return this.InitItem(ResultDataReader);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (BaseBussiness.log.IsErrorEnabled)
        //            BaseBussiness.log.Error((object)"Init", ex);
        //    }
        //    finally
        //    {
        //        if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
        //            ResultDataReader.Close();
        //    }
        //    return (SqlDataProvider.Data.ItemInfo)null;
        //}

        public LevelInfo GetUserLevelSingle(int Grade)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@Grade", (object) Grade)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Get_Level_By_Grade", SqlParameters);
                if (ResultDataReader.Read())
                    return new LevelInfo()
                    {
                        Grade = (int)ResultDataReader[nameof(Grade)],
                        GP = (int)ResultDataReader["GP"],
                        Blood = (int)ResultDataReader["Blood"]
                    };
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"GetLevelInfoSingle", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (LevelInfo)null;
        }

        public PlayerLimitInfo GetUserLimitByUserName(string userName)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserName", SqlDbType.NVarChar, 200)
                };
                SqlParameters[0].Value = (object)userName;
                this.db.GetReader(ref ResultDataReader, "SP_Users_LimitByUserName", SqlParameters);
                if (ResultDataReader.Read())
                    return new PlayerLimitInfo()
                    {
                        ID = (int)ResultDataReader["UserID"],
                        NickName = (string)ResultDataReader["NickName"]
                    };
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (PlayerLimitInfo)null;
        }

        public PlayerInfo[] GetUserLoginList(string userName)
        {
            List<PlayerInfo> playerInfoList = new List<PlayerInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserName", SqlDbType.NVarChar, 200)
                };
                SqlParameters[0].Value = (object)userName;
                this.db.GetReader(ref ResultDataReader, "SP_Users_LoginList", SqlParameters);
                while (ResultDataReader.Read())
                    playerInfoList.Add(this.InitPlayerInfo(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return playerInfoList.ToArray();
        }

        public QuestDataInfo[] GetUserQuest(int userID)
        {
            List<QuestDataInfo> questDataInfoList1 = new List<QuestDataInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)userID;
                this.db.GetReader(ref ResultDataReader, "SP_QuestData_All", SqlParameters);
                while (ResultDataReader.Read())
                {
                    List<QuestDataInfo> questDataInfoList2 = questDataInfoList1;
                    QuestDataInfo questDataInfo = new QuestDataInfo();
                    questDataInfo.CompletedDate = (DateTime)ResultDataReader["CompletedDate"];
                    questDataInfo.IsComplete = (bool)ResultDataReader["IsComplete"];
                    questDataInfo.Condition1 = (int)ResultDataReader["Condition1"];
                    questDataInfo.Condition2 = (int)ResultDataReader["Condition2"];
                    questDataInfo.Condition3 = (int)ResultDataReader["Condition3"];
                    questDataInfo.Condition4 = (int)ResultDataReader["Condition4"];
                    questDataInfo.QuestID = (int)ResultDataReader["QuestID"];
                    questDataInfo.UserID = (int)ResultDataReader["UserId"];
                    questDataInfo.IsExist = (bool)ResultDataReader["IsExist"];
                    questDataInfo.RandDobule = (int)ResultDataReader["RandDobule"];
                    questDataInfo.RepeatFinish = (int)ResultDataReader["RepeatFinish"];
                    questDataInfo.IsDirty = false;
                    questDataInfoList2.Add(questDataInfo);
                }
            }
            catch (Exception ex)
            {
                BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return questDataInfoList1.ToArray();
        }

        public QuestDataInfo GetUserQuestSiger(int userID, int QuestID)
        {
            QuestDataInfo questDataInfo = new QuestDataInfo();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[2]
                {
          new SqlParameter("@UserID", SqlDbType.Int),
          new SqlParameter("@QuestID", SqlDbType.Int)
                };
                SqlParameters[0].Value = (object)userID;
                SqlParameters[1].Value = (object)QuestID;
                this.db.GetReader(ref ResultDataReader, "SP_QuestData_One", SqlParameters);
                if (ResultDataReader.Read())
                    return new QuestDataInfo()
                    {
                        CompletedDate = (DateTime)ResultDataReader["CompletedDate"],
                        IsComplete = (bool)ResultDataReader["IsComplete"],
                        Condition1 = (int)ResultDataReader["Condition1"],
                        Condition2 = (int)ResultDataReader["Condition2"],
                        Condition3 = (int)ResultDataReader["Condition3"],
                        Condition4 = (int)ResultDataReader["Condition4"],
                        QuestID = (int)ResultDataReader[nameof(QuestID)],
                        UserID = (int)ResultDataReader["UserId"],
                        IsExist = (bool)ResultDataReader["IsExist"],
                        RandDobule = (int)ResultDataReader["RandDobule"],
                        RepeatFinish = (int)ResultDataReader["RepeatFinish"]
                    };
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (QuestDataInfo)null;
        }

        public PlayerInfo GetUserSingleByNickName(string nickName)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@NickName", SqlDbType.NVarChar, 200)
                };
                SqlParameters[0].Value = (object)nickName;
                this.db.GetReader(ref ResultDataReader, "SP_Users_SingleByNickName", SqlParameters);
                if (ResultDataReader.Read())
                    return this.InitPlayerInfo(ResultDataReader);
            }
            catch
            {
                throw new Exception();
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (PlayerInfo)null;
        }

        public PlayerInfo GetUserSingleByUserID(int UserID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)UserID;
                this.db.GetReader(ref ResultDataReader, "SP_Users_SingleByUserID", SqlParameters);
                if (ResultDataReader.Read())
                    return this.InitPlayerInfo(ResultDataReader);
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (PlayerInfo)null;
        }

        public PlayerInfo GetUserSingleByUserName(string userName)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserName", SqlDbType.NVarChar, 200)
                };
                SqlParameters[0].Value = (object)userName;
                this.db.GetReader(ref ResultDataReader, "SP_Users_SingleByUserName", SqlParameters);
                if (ResultDataReader.Read())
                    return this.InitPlayerInfo(ResultDataReader);
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (PlayerInfo)null;
        }

        public TexpInfo GetUserTexpInfoSingle(int ID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", (object) ID)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Get_UserTexp_By_ID", SqlParameters);
                if (ResultDataReader.Read())
                    return new TexpInfo()
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
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"GetTexpInfoSingle", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (TexpInfo)null;
        }

        public UserVIPInfo GetUserVIP(int userID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)userID;
                this.db.GetReader(ref ResultDataReader, "SP_Get_User_VIP", SqlParameters);
                if (ResultDataReader.Read())
                    return new UserVIPInfo()
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
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (UserVIPInfo)null;
        }

        public UsersCardInfo[] GetSingleUserCard(int UserID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            List<UsersCardInfo> usersCardInfoList = new List<UsersCardInfo>();
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)UserID;
                this.db.GetReader(ref ResultDataReader, "SP_GetSingleUserCard", SqlParameters);
                while (ResultDataReader.Read())
                {
                    UsersCardInfo usersCardInfo = this.InitCard(ResultDataReader);
                    usersCardInfoList.Add(usersCardInfo);
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(GetSingleUserCard), ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return usersCardInfoList.ToArray();
        }


        public AuctionInfo InitAuctionInfo(SqlDataReader reader)
        {
            return new AuctionInfo()
            {
                AuctioneerID = (int)reader["AuctioneerID"],
                AuctioneerName = reader["AuctioneerName"] == null ? "" : reader["AuctioneerName"].ToString(),
                AuctionID = (int)reader["AuctionID"],
                BeginDate = (DateTime)reader["BeginDate"],
                BuyerID = (int)reader["BuyerID"],
                BuyerName = reader["BuyerName"] == null ? "" : reader["BuyerName"].ToString(),
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
            return new UsersCardInfo()
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
            return new CardGrooveUpdateInfo()
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
            return new CardTemplateInfo()
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

        public EatPetsInfo InitEatPetsInfo(SqlDataReader dr)
        {
            return new EatPetsInfo()
            {
                ID = (int)dr["ID"],
                UserID = (int)dr["UserID"],
                weaponExp = (int)dr["weaponExp"],
                weaponLevel = (int)dr["weaponLevel"],
                clothesExp = (int)dr["clothesExp"],
                clothesLevel = (int)dr["clothesLevel"],
                hatExp = (int)dr["hatExp"],
                hatLevel = (int)dr["hatLevel"]
            };
        }

        public UserGemStone InitGemStones(SqlDataReader reader)
        {
            return new UserGemStone()
            {
                ID = (int)reader["ID"],
                UserID = (int)reader["UserID"],
                FigSpiritId = (int)reader["FigSpiritId"],
                FigSpiritIdValue = (string)reader["FigSpiritIdValue"],
                EquipPlace = (int)reader["EquipPlace"]
            };
        }

        public ConsortiaUserInfo InitConsortiaUserInfo(SqlDataReader dr)
        {
            ConsortiaUserInfo consortiaUserInfo = new ConsortiaUserInfo()
            {
                ID = (int)dr["ID"],
                ConsortiaID = (int)dr["ConsortiaID"],
                DutyID = (int)dr["DutyID"],
                DutyName = dr["DutyName"].ToString(),
                IsExist = (bool)dr["IsExist"],
                RatifierID = (int)dr["RatifierID"],
                RatifierName = dr["RatifierName"].ToString(),
                Remark = dr["Remark"].ToString(),
                UserID = (int)dr["UserID"],
                UserName = dr["UserName"].ToString(),
                Grade = (int)dr["Grade"],
                GP = (int)dr["GP"],
                Repute = (int)dr["Repute"],
                State = (int)dr["State"],
                Right = (int)dr["Right"],
                Offer = (int)dr["Offer"],
                Colors = dr["Colors"].ToString(),
                Style = dr["Style"].ToString(),
                Hide = (int)dr["Hide"]
            };
            consortiaUserInfo.Skin = dr["Skin"] == null ? "" : consortiaUserInfo.Skin;
            consortiaUserInfo.Level = (int)dr["Level"];
            consortiaUserInfo.LastDate = (DateTime)dr["LastDate"];
            consortiaUserInfo.Sex = (bool)dr["Sex"];
            consortiaUserInfo.IsBanChat = (bool)dr["IsBanChat"];
            consortiaUserInfo.Win = (int)dr["Win"];
            consortiaUserInfo.Total = (int)dr["Total"];
            consortiaUserInfo.Escape = (int)dr["Escape"];
            consortiaUserInfo.RichesOffer = (int)dr["RichesOffer"];
            consortiaUserInfo.RichesRob = (int)dr["RichesRob"];
            consortiaUserInfo.LoginName = dr["LoginName"] == null ? "" : dr["LoginName"].ToString();
            consortiaUserInfo.Nimbus = (int)dr["Nimbus"];
            consortiaUserInfo.FightPower = (int)dr["FightPower"];
            consortiaUserInfo.typeVIP = Convert.ToByte(dr["typeVIP"]);
            consortiaUserInfo.VIPLevel = (int)dr["VIPLevel"];
            return consortiaUserInfo;
        }

        public SqlDataProvider.Data.ItemInfo InitItem(SqlDataReader reader)
        {
            SqlDataProvider.Data.ItemInfo itemInfo = new SqlDataProvider.Data.ItemInfo(ItemMgr.FindItemTemplate((int)reader["TemplateID"]));
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
            return new MailInfo()
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
                Annex1Name = reader["Annex1Name"] == null ? "" : reader["Annex1Name"].ToString(),
                Annex2Name = reader["Annex2Name"] == null ? "" : reader["Annex2Name"].ToString(),
                Annex3 = reader["Annex3"].ToString(),
                Annex4 = reader["Annex4"].ToString(),
                Annex5 = reader["Annex5"].ToString(),
                Annex3Name = reader["Annex3Name"] == null ? "" : reader["Annex3Name"].ToString(),
                Annex4Name = reader["Annex4Name"] == null ? "" : reader["Annex4Name"].ToString(),
                Annex5Name = reader["Annex5Name"] == null ? "" : reader["Annex5Name"].ToString(),
                AnnexRemark = reader["AnnexRemark"] == null ? "" : reader["AnnexRemark"].ToString()
            };
        }

        public PlayerInfo InitPlayerInfo(SqlDataReader reader)
        {
            PlayerInfo playerInfo1 = new PlayerInfo();
            playerInfo1.Password = (string)reader["Password"];
            playerInfo1.IsConsortia = (bool)reader["IsConsortia"];
            playerInfo1.Agility = (int)reader["Agility"];
            playerInfo1.Attack = (int)reader["Attack"];
            playerInfo1.hp = (int)reader["hp"];
            playerInfo1.Colors = reader["Colors"] == null ? "" : reader["Colors"].ToString();
            playerInfo1.ConsortiaID = (int)reader["ConsortiaID"];
            playerInfo1.Defence = (int)reader["Defence"];
            playerInfo1.Gold = (int)reader["Gold"];
            playerInfo1.GP = (int)reader["GP"];
            playerInfo1.Grade = (int)reader["Grade"];
            playerInfo1.ID = (int)reader["UserID"];
            playerInfo1.Luck = (int)reader["Luck"];
            playerInfo1.Money = (int)reader["Money"];
            playerInfo1.NickName = (string)reader["NickName"] == null ? "" : (string)reader["NickName"];
            playerInfo1.Sex = (bool)reader["Sex"];
            playerInfo1.State = (int)reader["State"];
            playerInfo1.Style = reader["Style"] == null ? "" : reader["Style"].ToString();
            playerInfo1.Hide = (int)reader["Hide"];
            playerInfo1.Repute = (int)reader["Repute"];
            playerInfo1.UserName = reader["UserName"] == null ? "" : reader["UserName"].ToString();
            playerInfo1.ConsortiaName = reader["ConsortiaName"] == null ? "" : reader["ConsortiaName"].ToString();
            playerInfo1.Offer = (int)reader["Offer"];
            playerInfo1.Win = (int)reader["Win"];
            playerInfo1.Total = (int)reader["Total"];
            playerInfo1.Escape = (int)reader["Escape"];
            playerInfo1.Skin = reader["Skin"] == null ? "" : reader["Skin"].ToString();
            playerInfo1.IsBanChat = (bool)reader["IsBanChat"];
            playerInfo1.ReputeOffer = (int)reader["ReputeOffer"];
            playerInfo1.ConsortiaRepute = (int)reader["ConsortiaRepute"];
            playerInfo1.ConsortiaLevel = (int)reader["ConsortiaLevel"];
            playerInfo1.StoreLevel = (int)reader["StoreLevel"];
            playerInfo1.ShopLevel = (int)reader["ShopLevel"];
            playerInfo1.SmithLevel = (int)reader["SmithLevel"];
            playerInfo1.ConsortiaHonor = (int)reader["ConsortiaHonor"];
            playerInfo1.RichesOffer = (int)reader["RichesOffer"];
            playerInfo1.RichesRob = (int)reader["RichesRob"];
            playerInfo1.AntiAddiction = (int)reader["AntiAddiction"];
            playerInfo1.DutyLevel = (int)reader["DutyLevel"];
            playerInfo1.DutyName = reader["DutyName"] == null ? "" : reader["DutyName"].ToString();
            playerInfo1.Right = (int)reader["Right"];
            playerInfo1.ChairmanName = reader["ChairmanName"] == null ? "" : reader["ChairmanName"].ToString();
            playerInfo1.AddDayGP = (int)reader["AddDayGP"];
            playerInfo1.AddDayOffer = (int)reader["AddDayOffer"];
            playerInfo1.AddWeekGP = (int)reader["AddWeekGP"];
            playerInfo1.AddWeekOffer = (int)reader["AddWeekOffer"];
            playerInfo1.ConsortiaRiches = (int)reader["ConsortiaRiches"];
            playerInfo1.CheckCount = (int)reader["CheckCount"];
            playerInfo1.IsMarried = (bool)reader["IsMarried"];
            playerInfo1.SpouseID = (int)reader["SpouseID"];
            playerInfo1.SpouseName = reader["SpouseName"] == null ? "" : reader["SpouseName"].ToString();
            playerInfo1.MarryInfoID = (int)reader["MarryInfoID"];
            playerInfo1.IsCreatedMarryRoom = (bool)reader["IsCreatedMarryRoom"];
            playerInfo1.DayLoginCount = (int)reader["DayLoginCount"];
            playerInfo1.PasswordTwo = reader["PasswordTwo"] == null ? "" : reader["PasswordTwo"].ToString();
            playerInfo1.SelfMarryRoomID = (int)reader["SelfMarryRoomID"];
            playerInfo1.IsGotRing = (bool)reader["IsGotRing"];
            playerInfo1.Rename = (bool)reader["Rename"];
            playerInfo1.ConsortiaRename = (bool)reader["ConsortiaRename"];
            playerInfo1.IsDirty = false;
            playerInfo1.IsFirst = (int)reader["IsFirst"];
            playerInfo1.Nimbus = (int)reader["Nimbus"];
            playerInfo1.LastAward = (DateTime)reader["LastAward"];
            playerInfo1.GiftToken = (int)reader["GiftToken"];
            playerInfo1.QuestSite = reader["QuestSite"] == null ? new byte[200] : (byte[])reader["QuestSite"];
            playerInfo1.PvePermission = reader["PvePermission"] == null ? "" : reader["PvePermission"].ToString();
            playerInfo1.FightPower = (int)reader["FightPower"];
            playerInfo1.PasswordQuest1 = reader["PasswordQuestion1"] == null ? "" : reader["PasswordQuestion1"].ToString();
            playerInfo1.PasswordQuest2 = reader["PasswordQuestion2"] == null ? "" : reader["PasswordQuestion2"].ToString();
            PlayerInfo playerInfo2 = playerInfo1;
            DateTime dateTime1 = (DateTime)reader["LastFindDate"];
            DateTime dateTime2 = DateTime.Today;
            DateTime date = dateTime2.Date;
            int num = !(dateTime1 != date) ? (int)reader["FailedPasswordAttemptCount"] : 5;
            playerInfo2.FailedPasswordAttemptCount = num;
            playerInfo1.AnswerSite = (int)reader["AnswerSite"];
            playerInfo1.medal = (int)reader["Medal"];
            playerInfo1.ChatCount = (int)reader["ChatCount"];
            playerInfo1.SpaPubGoldRoomLimit = (int)reader["SpaPubGoldRoomLimit"];
            playerInfo1.LastSpaDate = (DateTime)reader["LastSpaDate"];
            playerInfo1.FightLabPermission = (string)reader["FightLabPermission"];
            playerInfo1.SpaPubMoneyRoomLimit = (int)reader["SpaPubMoneyRoomLimit"];
            playerInfo1.IsInSpaPubGoldToday = (bool)reader["IsInSpaPubGoldToday"];
            playerInfo1.IsInSpaPubMoneyToday = (bool)reader["IsInSpaPubMoneyToday"];
            playerInfo1.AchievementPoint = (int)reader["AchievementPoint"];
            playerInfo1.LastWeekly = (DateTime)reader["LastWeekly"];
            playerInfo1.LastWeeklyVersion = (int)reader["LastWeeklyVersion"];
            playerInfo1.badgeID = (int)reader["BadgeID"];
            playerInfo1.typeVIP = Convert.ToByte(reader["typeVIP"]);
            playerInfo1.VIPLevel = (int)reader["VIPLevel"];
            playerInfo1.VIPExp = (int)reader["VIPExp"];
            playerInfo1.VIPExpireDay = (DateTime)reader["VIPExpireDay"];
            playerInfo1.VIPNextLevelDaysNeeded = (int)reader["VIPNextLevelDaysNeeded"];
            playerInfo1.LastVIPPackTime = (DateTime)reader["LastVIPPackTime"];
            playerInfo1.CanTakeVipReward = (bool)reader["CanTakeVipReward"];
            playerInfo1.WeaklessGuildProgressStr = (string)reader["WeaklessGuildProgressStr"];
            playerInfo1.IsOldPlayer = (bool)reader["IsOldPlayer"];
            playerInfo1.LastDate = (DateTime)reader["LastDate"];
            playerInfo1.VIPLastDate = (DateTime)reader["VIPLastDate"];
            playerInfo1.Score = (int)reader["Score"];
            playerInfo1.OptionOnOff = (int)reader["OptionOnOff"];
            playerInfo1.isOldPlayerHasValidEquitAtLogin = (bool)reader["isOldPlayerHasValidEquitAtLogin"];
            playerInfo1.badLuckNumber = (int)reader["badLuckNumber"];
            playerInfo1.OnlineTime = (int)reader["OnlineTime"];
            playerInfo1.luckyNum = (int)reader["luckyNum"];
            playerInfo1.lastLuckyNumDate = (DateTime)reader["lastLuckyNumDate"];
            playerInfo1.lastLuckNum = (int)reader["lastLuckNum"];
            playerInfo1.IsShowConsortia = (bool)reader["IsShowConsortia"];
            playerInfo1.NewDay = (DateTime)reader["NewDay"];
            playerInfo1.Honor = (string)reader["Honor"];
            playerInfo1.BoxGetDate = (DateTime)reader["BoxGetDate"];
            playerInfo1.AlreadyGetBox = (int)reader["AlreadyGetBox"];
            playerInfo1.BoxProgression = (int)reader["BoxProgression"];
            playerInfo1.GetBoxLevel = (int)reader["GetBoxLevel"];
            playerInfo1.IsRecharged = (bool)reader["IsRecharged"];
            playerInfo1.IsGetAward = (bool)reader["IsGetAward"];
            playerInfo1.apprenticeshipState = (int)reader["apprenticeshipState"];
            playerInfo1.masterID = (int)reader["masterID"];
            playerInfo1.masterOrApprentices = reader["masterOrApprentices"] == DBNull.Value ? "" : (string)reader["masterOrApprentices"];
            playerInfo1.graduatesCount = (int)reader["graduatesCount"];
            playerInfo1.honourOfMaster = reader["honourOfMaster"] == DBNull.Value ? "" : (string)reader["honourOfMaster"];
            playerInfo1.freezesDate = reader["freezesDate"] == DBNull.Value ? DateTime.Now : (DateTime)reader["freezesDate"];
            playerInfo1.charmGP = reader["charmGP"] == DBNull.Value ? 0 : (int)reader["charmGP"];
            playerInfo1.evolutionGrade = (int)reader["evolutionGrade"];
            playerInfo1.evolutionExp = (int)reader["evolutionExp"];
            playerInfo1.hardCurrency = (int)reader["hardCurrency"];
            playerInfo1.EliteScore = (int)reader["EliteScore"];
            PlayerInfo playerInfo3 = playerInfo1;
            DateTime dateTime3;
            if (reader["ShopFinallyGottenTime"] != DBNull.Value)
            {
                dateTime3 = (DateTime)reader["ShopFinallyGottenTime"];
            }
            else
            {
                dateTime2 = DateTime.Now;
                dateTime3 = dateTime2.AddDays(-1.0);
            }
            playerInfo3.ShopFinallyGottenTime = dateTime3;
            playerInfo1.MoneyLock = reader["MoneyLock"] == DBNull.Value ? 0 : (int)reader["MoneyLock"];
            playerInfo1.totemId = (int)reader["totemId"];//Totem
            playerInfo1.myHonor = (int)reader["myHonor"];//Totem
            playerInfo1.MaxBuyHonor = (int)reader["MaxBuyHonor"];//Totem
            return playerInfo1;
        }

        public bool InsertMarryRoomInfo(MarryRoomInfo info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[20];
                SqlParameters[0] = new SqlParameter("@ID", (object)info.ID);
                SqlParameters[0].Direction = ParameterDirection.InputOutput;
                SqlParameters[1] = new SqlParameter("@Name", (object)info.Name);
                SqlParameters[2] = new SqlParameter("@PlayerID", (object)info.PlayerID);
                SqlParameters[3] = new SqlParameter("@PlayerName", (object)info.PlayerName);
                SqlParameters[4] = new SqlParameter("@GroomID", (object)info.GroomID);
                SqlParameters[5] = new SqlParameter("@GroomName", (object)info.GroomName);
                SqlParameters[6] = new SqlParameter("@BrideID", (object)info.BrideID);
                SqlParameters[7] = new SqlParameter("@BrideName", (object)info.BrideName);
                SqlParameters[8] = new SqlParameter("@Pwd", (object)info.Pwd);
                SqlParameters[9] = new SqlParameter("@AvailTime", (object)info.AvailTime);
                SqlParameters[10] = new SqlParameter("@MaxCount", (object)info.MaxCount);
                SqlParameters[11] = new SqlParameter("@GuestInvite", (object)info.GuestInvite);
                SqlParameters[12] = new SqlParameter("@MapIndex", (object)info.MapIndex);
                SqlParameters[13] = new SqlParameter("@BeginTime", (object)info.BeginTime);
                SqlParameters[14] = new SqlParameter("@BreakTime", (object)info.BreakTime);
                SqlParameters[15] = new SqlParameter("@RoomIntroduction", (object)info.RoomIntroduction);
                SqlParameters[16] = new SqlParameter("@ServerID", (object)info.ServerID);
                SqlParameters[17] = new SqlParameter("@IsHymeneal", (object)info.IsHymeneal);
                SqlParameters[18] = new SqlParameter("@IsGunsaluteUsed", (object)info.IsGunsaluteUsed);
                SqlParameters[19] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[19].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Insert_Marry_Room_Info", SqlParameters);
                flag = (int)SqlParameters[19].Value == 0;
                if (flag)
                    info.ID = (int)SqlParameters[0].Value;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(InsertMarryRoomInfo), ex);
            }
            return flag;
        }

        public bool InsertPlayerMarryApply(MarryApplyInfo info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[7]
                {
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@ApplyUserID", (object) info.ApplyUserID),
          new SqlParameter("@ApplyUserName", (object) info.ApplyUserName),
          new SqlParameter("@ApplyType", (object) info.ApplyType),
          new SqlParameter("@ApplyResult", (object) info.ApplyResult),
          new SqlParameter("@LoveProclamation", (object) info.LoveProclamation),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[6].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Insert_Marry_Apply", SqlParameters);
                flag = (int)SqlParameters[6].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(InsertPlayerMarryApply), ex);
            }
            return flag;
        }

        public bool InsertUserTexpInfo(TexpInfo info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[10]
                {
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@attTexpExp", (object) info.attTexpExp),
          new SqlParameter("@defTexpExp", (object) info.defTexpExp),
          new SqlParameter("@hpTexpExp", (object) info.hpTexpExp),
          new SqlParameter("@lukTexpExp", (object) info.lukTexpExp),
          new SqlParameter("@spdTexpExp", (object) info.spdTexpExp),
          new SqlParameter("@texpCount", (object) info.texpCount),
          new SqlParameter("@texpTaskCount", (object) info.texpTaskCount),
          new SqlParameter("@texpTaskDate", (object) info.texpTaskDate.ToString("yyyy-MM-dd HH:mm:ss")),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[9].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_UserTexp_Add", SqlParameters);
                flag = (int)SqlParameters[9].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"InsertTexpInfo", ex);
            }
            return flag;
        }

        public int PullDown(int activeID, string awardID, int userID, ref string msg)
        {
            int num = 1;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[4]
                {
          new SqlParameter("@ActiveID", (object) activeID),
          new SqlParameter("@AwardID", (object) awardID),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[3].Direction = ParameterDirection.ReturnValue;
                if (!this.db.RunProcedure("SP_Active_PullDown", SqlParameters))
                    return num;
                num = (int)SqlParameters[3].Value;
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
                        break;
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return num;
        }

        public bool AddActiveNumber(string AwardID, int ActiveID)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[3]
                {
          new SqlParameter("@AwardID", (object) AwardID),
          new SqlParameter("@ActiveID", (object) ActiveID),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[2].Direction = ParameterDirection.ReturnValue;
                flag = this.db.RunProcedure("SP_Active_Number_Add", SqlParameters);
                flag = (int)SqlParameters[2].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public PlayerInfo LoginGame(string username, string password)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[2]
                {
          new SqlParameter("@UserName", (object) username),
          new SqlParameter("@Password", (object) password)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Users_Login", SqlParameters);
                if (ResultDataReader.Read())
                    return this.InitPlayerInfo(ResultDataReader);
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (PlayerInfo)null;
        }

        public PlayerInfo LoginGame(
          string username,
          ref int isFirst,
          ref bool isExist,
          ref bool isError,
          bool firstValidate,
          ref DateTime forbidDate,
          string nickname)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[4]
                {
          new SqlParameter("@UserName", (object) username),
          new SqlParameter("@Password", (object) ""),
          new SqlParameter("@FirstValidate", (object) firstValidate),
          new SqlParameter("@Nickname", (object) nickname)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Users_LoginWeb", SqlParameters);
                if (ResultDataReader.Read())
                {
                    isFirst = (int)ResultDataReader["IsFirst"];
                    isExist = (bool)ResultDataReader["IsExist"];
                    forbidDate = (DateTime)ResultDataReader["ForbidDate"];
                    if (isFirst > 1)
                        --isFirst;
                    return this.InitPlayerInfo(ResultDataReader);
                }
            }
            catch (Exception ex)
            {
                isError = true;
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (PlayerInfo)null;
        }

        public PlayerInfo LoginGame(
          string username,
          ref int isFirst,
          ref bool isExist,
          ref bool isError,
          bool firstValidate,
          ref DateTime forbidDate,
          ref string nickname,
          string ActiveIP)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[5]
                {
          new SqlParameter("@UserName", (object) username),
          new SqlParameter("@Password", (object) ""),
          new SqlParameter("@FirstValidate", (object) firstValidate),
          new SqlParameter("@Nickname", (object) nickname),
          new SqlParameter("@ActiveIP", (object) ActiveIP)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Users_LoginWeb", SqlParameters);
                if (ResultDataReader.Read())
                {
                    isFirst = (int)ResultDataReader["IsFirst"];
                    isExist = (bool)ResultDataReader["IsExist"];
                    forbidDate = (DateTime)ResultDataReader["ForbidDate"];
                    nickname = (string)ResultDataReader["NickName"];
                    if (isFirst > 1)
                        --isFirst;
                    return this.InitPlayerInfo(ResultDataReader);
                }
            }
            catch (Exception ex)
            {
                isError = true;
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (PlayerInfo)null;
        }

        public bool RegisterPlayer(
          string userName,
          string passWord,
          string nickName,
          string bStyle,
          string gStyle,
          string armColor,
          string hairColor,
          string faceColor,
          string clothColor,
          string hatColor,
          int sex,
          ref string msg,
          int validDate)
        {
            bool flag = false;
            try
            {
                string[] strArray1 = bStyle.Split(',');
                string[] strArray2 = gStyle.Split(',');
                SqlParameter[] SqlParameters = new SqlParameter[21]
                {
          new SqlParameter("@UserName", (object) userName),
          new SqlParameter("@PassWord", (object) passWord),
          new SqlParameter("@NickName", (object) nickName),
          new SqlParameter("@BArmID", (object) int.Parse(strArray1[0])),
          new SqlParameter("@BHairID", (object) int.Parse(strArray1[1])),
          new SqlParameter("@BFaceID", (object) int.Parse(strArray1[2])),
          new SqlParameter("@BClothID", (object) int.Parse(strArray1[3])),
          new SqlParameter("@BHatID", (object) int.Parse(strArray1[4])),
          new SqlParameter("@GArmID", (object) int.Parse(strArray2[0])),
          new SqlParameter("@GHairID", (object) int.Parse(strArray2[1])),
          new SqlParameter("@GFaceID", (object) int.Parse(strArray2[2])),
          new SqlParameter("@GClothID", (object) int.Parse(strArray2[3])),
          new SqlParameter("@GHatID", (object) int.Parse(strArray2[4])),
          new SqlParameter("@ArmColor", (object) armColor),
          new SqlParameter("@HairColor", (object) hairColor),
          new SqlParameter("@FaceColor", (object) faceColor),
          new SqlParameter("@ClothColor", (object) clothColor),
          new SqlParameter("@HatColor", (object) clothColor),
          new SqlParameter("@Sex", (object) sex),
          new SqlParameter("@StyleDate", (object) validDate),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[20].Direction = ParameterDirection.ReturnValue;
                flag = this.db.RunProcedure("SP_Users_RegisterNotValidate", SqlParameters);
                int num = (int)SqlParameters[20].Value;
                flag = num == 0;
                if (num == 2)
                {
                    msg = LanguageMgr.GetTranslation("PlayerBussiness.RegisterPlayer.Msg2");
                    return flag;
                }
                if (num != 3)
                    return flag;
                msg = LanguageMgr.GetTranslation("PlayerBussiness.RegisterPlayer.Msg3");
                return flag;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool RegisterUser(
          string UserName,
          string NickName,
          string Password,
          bool Sex,
          int Money,
          int GiftToken,
          int Gold)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[8]
                {
          new SqlParameter("@UserName", (object) UserName),
          new SqlParameter("@Password", (object) Password),
          new SqlParameter("@NickName", (object) NickName),
          new SqlParameter("@Sex", (object) Sex),
          new SqlParameter("@Money", (object) Money),
          new SqlParameter("@GiftToken", (object) GiftToken),
          new SqlParameter("@Gold", (object) Gold),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[7].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Account_Register", SqlParameters);
                if ((int)SqlParameters[7].Value == 0)
                    flag = true;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init Register", ex);
            }
            return flag;
        }

        public bool RegisterUserInfo(UserInfo userinfo)
        {
            try
            {
                return this.db.RunProcedure("SP_User_Info_Add", new SqlParameter[6]
                {
          new SqlParameter("@UserID", (object) userinfo.UserID),
          new SqlParameter("@UserEmail", (object) userinfo.UserEmail),
          new SqlParameter("@UserPhone", userinfo.UserPhone == null ? (object) string.Empty : (object) userinfo.UserPhone),
          new SqlParameter("@UserOther1", userinfo.UserOther1 == null ? (object) string.Empty : (object) userinfo.UserOther1),
          new SqlParameter("@UserOther2", userinfo.UserOther2 == null ? (object) string.Empty : (object) userinfo.UserOther2),
          new SqlParameter("@UserOther3", userinfo.UserOther3 == null ? (object) string.Empty : (object) userinfo.UserOther3)
                });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return false;
        }

        public PlayerInfo ReLoadPlayer(int ID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@ID", (object) ID)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Users_Reload", SqlParameters);
                if (ResultDataReader.Read())
                    return this.InitPlayerInfo(ResultDataReader);
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (PlayerInfo)null;
        }

        public bool RemoveIsArrange(int ID)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[2]
                {
          new SqlParameter("@UserID", (object) ID),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[1].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_RemoveIsArrange", SqlParameters);
                flag = (int)SqlParameters[1].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_RemoveIsArrange", ex);
            }
            return flag;
        }

        public bool RemoveTreasureDataByUser(int ID)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[2]
                {
          new SqlParameter("@UserID", (object) ID),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[1].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_RemoveTreasureDataByUser", SqlParameters);
                flag = (int)SqlParameters[1].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_RemoveTreasureDataByUser", ex);
            }
            return flag;
        }

        public bool RenameNick(string userName, string nickName, string newNickName)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[4]
                {
          new SqlParameter("@UserName", (object) userName),
          new SqlParameter("@NickName", (object) nickName),
          new SqlParameter("@NewNickName", (object) newNickName),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[3].Direction = ParameterDirection.ReturnValue;
                flag = this.db.RunProcedure("SP_Users_RenameNick2", SqlParameters);
                flag = (int)SqlParameters[3].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(RenameNick), ex);
            }
            return flag;
        }

        public bool RenameNick(string userName, string nickName, string newNickName, ref string msg)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[4]
                {
          new SqlParameter("@UserName", (object) userName),
          new SqlParameter("@NickName", (object) nickName),
          new SqlParameter("@NewNickName", (object) newNickName),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[3].Direction = ParameterDirection.ReturnValue;
                flag = this.db.RunProcedure("SP_Users_RenameNick", SqlParameters);
                int num = (int)SqlParameters[3].Value;
                flag = num == 0;
                if (num - 4 > 1)
                    return flag;
                msg = LanguageMgr.GetTranslation("PlayerBussiness.RenameNick.Msg4");
                return flag;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(RenameNick), ex);
            }
            return flag;
        }

        public bool ResetCommunalActive(int ActiveID, bool IsReset)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[3]
                {
          new SqlParameter("@ActiveID", (object) ActiveID),
          new SqlParameter("@IsReset", (object) IsReset),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[2].Direction = ParameterDirection.ReturnValue;
                flag = this.db.RunProcedure("SP_ReCommunalActive", SqlParameters);
                flag = (int)SqlParameters[2].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init CommunalActive", ex);
            }
            return flag;
        }

        public bool ResetDragonBoat()
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_ReDragonBoat_Data");
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init ResetDragonBoat", ex);
            }
            return flag;
        }

        public bool ResetLuckStarRank()
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_ReLuckStar_Rank_Info");
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init ResetLuckStar", ex);
            }
            return flag;
        }

        public bool SaveBuffer(BufferInfo info)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_User_Buff_Add", new SqlParameter[9]
                {
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@Type", (object) info.Type),
          new SqlParameter("@BeginDate", (object) info.BeginDate),
          new SqlParameter("@Data", info.Data == null ? (object) "" : (object) info.Data),
          new SqlParameter("@IsExist", (object) info.IsExist),
          new SqlParameter("@ValidDate", (object) info.ValidDate),
          new SqlParameter("@ValidCount", (object) info.ValidCount),
          new SqlParameter("@Value", (object) info.Value),
          new SqlParameter("@TemplateID", (object) info.TemplateID)
                });
                info.IsDirty = false;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool SaveConsortiaBuffer(ConsortiaBufferInfo info)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_User_Consortia_Buff_Add", new SqlParameter[8]
                {
          new SqlParameter("@ConsortiaID", (object) info.ConsortiaID),
          new SqlParameter("@BufferID", (object) info.BufferID),
          new SqlParameter("@IsOpen", (object) (info.IsOpen ? 1 : 0)),
          new SqlParameter("@BeginDate", (object) info.BeginDate),
          new SqlParameter("@ValidDate", (object) info.ValidDate),
          new SqlParameter("@Type ", (object) info.Type),
          new SqlParameter("@Value", (object) info.Value),
          new SqlParameter("@Group", (object) info.Group)
                });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool SavePlayerMarryNotice(MarryApplyInfo info, int answerId, ref int id)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[9]
                {
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@ApplyUserID", (object) info.ApplyUserID),
          new SqlParameter("@ApplyUserName", (object) info.ApplyUserName),
          new SqlParameter("@ApplyType", (object) info.ApplyType),
          new SqlParameter("@ApplyResult", (object) info.ApplyResult),
          new SqlParameter("@LoveProclamation", (object) info.LoveProclamation),
          new SqlParameter("@AnswerId", (object) answerId),
          new SqlParameter("@ouototal", SqlDbType.Int),
          null
                };
                SqlParameters[7].Direction = ParameterDirection.Output;
                SqlParameters[8] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[8].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Insert_Marry_Notice", SqlParameters);
                id = (int)SqlParameters[7].Value;
                flag = (int)SqlParameters[8].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(SavePlayerMarryNotice), ex);
            }
            return flag;
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
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@NoticeUserID", SqlDbType.NVarChar, 4000)
                };
                SqlParameters[0].Direction = ParameterDirection.Output;
                this.db.RunProcedure("SP_Mail_Scan", SqlParameters);
                noticeUserID = SqlParameters[0].Value.ToString();
                flag = true;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool SendMail(MailInfo mail)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[29];
                SqlParameters[0] = new SqlParameter("@ID", (object)mail.ID);
                SqlParameters[0].Direction = ParameterDirection.Output;
                SqlParameters[1] = new SqlParameter("@Annex1", mail.Annex1 == null ? (object)"" : (object)mail.Annex1);
                SqlParameters[2] = new SqlParameter("@Annex2", mail.Annex2 == null ? (object)"" : (object)mail.Annex2);
                SqlParameters[3] = new SqlParameter("@Content", mail.Content == null ? (object)"" : (object)mail.Content);
                SqlParameters[4] = new SqlParameter("@Gold", (object)mail.Gold);
                SqlParameters[5] = new SqlParameter("@IsExist", (object)true);
                SqlParameters[6] = new SqlParameter("@Money", (object)mail.Money);
                SqlParameters[7] = new SqlParameter("@Receiver", mail.Receiver == null ? (object)"" : (object)mail.Receiver);
                SqlParameters[8] = new SqlParameter("@ReceiverID", (object)mail.ReceiverID);
                SqlParameters[9] = new SqlParameter("@Sender", mail.Sender == null ? (object)"" : (object)mail.Sender);
                SqlParameters[10] = new SqlParameter("@SenderID", (object)mail.SenderID);
                SqlParameters[11] = new SqlParameter("@Title", mail.Title == null ? (object)"" : (object)mail.Title);
                SqlParameters[12] = new SqlParameter("@IfDelS", (object)false);
                SqlParameters[13] = new SqlParameter("@IsDelete", (object)false);
                SqlParameters[14] = new SqlParameter("@IsDelR", (object)false);
                SqlParameters[15] = new SqlParameter("@IsRead", (object)false);
                SqlParameters[16] = new SqlParameter("@SendTime", (object)DateTime.Now);
                SqlParameters[17] = new SqlParameter("@Type", (object)mail.Type);
                SqlParameters[18] = new SqlParameter("@Annex1Name", mail.Annex1Name == null ? (object)"" : (object)mail.Annex1Name);
                SqlParameters[19] = new SqlParameter("@Annex2Name", mail.Annex2Name == null ? (object)"" : (object)mail.Annex2Name);
                SqlParameters[20] = new SqlParameter("@Annex3", mail.Annex3 == null ? (object)"" : (object)mail.Annex3);
                SqlParameters[21] = new SqlParameter("@Annex4", mail.Annex4 == null ? (object)"" : (object)mail.Annex4);
                SqlParameters[22] = new SqlParameter("@Annex5", mail.Annex5 == null ? (object)"" : (object)mail.Annex5);
                SqlParameters[23] = new SqlParameter("@Annex3Name", mail.Annex3Name == null ? (object)"" : (object)mail.Annex3Name);
                SqlParameters[24] = new SqlParameter("@Annex4Name", mail.Annex4Name == null ? (object)"" : (object)mail.Annex4Name);
                SqlParameters[25] = new SqlParameter("@Annex5Name", mail.Annex5Name == null ? (object)"" : (object)mail.Annex5Name);
                SqlParameters[26] = new SqlParameter("@ValidDate", (object)mail.ValidDate);
                SqlParameters[27] = new SqlParameter("@AnnexRemark", mail.AnnexRemark == null ? (object)"" : (object)mail.AnnexRemark);
                SqlParameters[28] = new SqlParameter("@GiftToken", (object)mail.GiftToken);
                flag = this.db.RunProcedure("SP_Mail_Send", SqlParameters);
                mail.ID = (int)SqlParameters[0].Value;
                using (CenterServiceClient centerServiceClient = new CenterServiceClient())
                    centerServiceClient.MailNotice(mail.ReceiverID);
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool SendMailAndItem(MailInfo mail, SqlDataProvider.Data.ItemInfo item, ref int returnValue)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[34]
                {
          new SqlParameter("@ItemID", (object) item.ItemID),
          new SqlParameter("@UserID", (object) item.UserID),
          new SqlParameter("@TemplateID", (object) item.TemplateID),
          new SqlParameter("@Place", (object) item.Place),
          new SqlParameter("@AgilityCompose", (object) item.AgilityCompose),
          new SqlParameter("@AttackCompose", (object) item.AttackCompose),
          new SqlParameter("@BeginDate", (object) item.BeginDate),
          new SqlParameter("@Color", item.Color == null ? (object) "" : (object) item.Color),
          new SqlParameter("@Count", (object) item.Count),
          new SqlParameter("@DefendCompose", (object) item.DefendCompose),
          new SqlParameter("@IsBinds", (object) item.IsBinds),
          new SqlParameter("@IsExist", (object) item.IsExist),
          new SqlParameter("@IsJudge", (object) item.IsJudge),
          new SqlParameter("@LuckCompose", (object) item.LuckCompose),
          new SqlParameter("@StrengthenLevel", (object) item.StrengthenLevel),
          new SqlParameter("@ValidDate", (object) item.ValidDate),
          new SqlParameter("@BagType", (object) item.BagType),
          new SqlParameter("@ID", (object) mail.ID),
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
                SqlParameters[17].Direction = ParameterDirection.Output;
                SqlParameters[18] = new SqlParameter("@Annex1", mail.Annex1 == null ? (object)"" : (object)mail.Annex1);
                SqlParameters[19] = new SqlParameter("@Annex2", mail.Annex2 == null ? (object)"" : (object)mail.Annex2);
                SqlParameters[20] = new SqlParameter("@Content", mail.Content == null ? (object)"" : (object)mail.Content);
                SqlParameters[21] = new SqlParameter("@Gold", (object)mail.Gold);
                SqlParameters[22] = new SqlParameter("@Money", (object)mail.Money);
                SqlParameters[23] = new SqlParameter("@Receiver", mail.Receiver == null ? (object)"" : (object)mail.Receiver);
                SqlParameters[24] = new SqlParameter("@ReceiverID", (object)mail.ReceiverID);
                SqlParameters[25] = new SqlParameter("@Sender", mail.Sender == null ? (object)"" : (object)mail.Sender);
                SqlParameters[26] = new SqlParameter("@SenderID", (object)mail.SenderID);
                SqlParameters[27] = new SqlParameter("@Title", mail.Title == null ? (object)"" : (object)mail.Title);
                SqlParameters[28] = new SqlParameter("@IfDelS", (object)false);
                SqlParameters[29] = new SqlParameter("@IsDelete", (object)false);
                SqlParameters[30] = new SqlParameter("@IsDelR", (object)false);
                SqlParameters[31] = new SqlParameter("@IsRead", (object)false);
                SqlParameters[32] = new SqlParameter("@SendTime", (object)DateTime.Now);
                SqlParameters[33] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[33].Direction = ParameterDirection.ReturnValue;
                flag = this.db.RunProcedure("SP_Admin_SendUserItem", SqlParameters);
                returnValue = (int)SqlParameters[33].Value;
                flag = returnValue == 0;
                if (!flag)
                    return flag;
                using (CenterServiceClient centerServiceClient = new CenterServiceClient())
                    centerServiceClient.MailNotice(mail.ReceiverID);
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public int SendMailAndItem(
          string title,
          string content,
          int userID,
          int gold,
          int money,
          string param)
        {
            int num = 1;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[8]
                {
          new SqlParameter("@Title", (object) title),
          new SqlParameter("@Content", (object) content),
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Gold", (object) gold),
          new SqlParameter("@Money", (object) money),
          new SqlParameter("@GiftToken", (object)0),
          new SqlParameter("@Param", (object) param),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[7].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Admin_SendAllItem", SqlParameters);
                num = (int)SqlParameters[7].Value;
                if ((uint)num > 0U)
                    return num;
                using (CenterServiceClient centerServiceClient = new CenterServiceClient())
                    centerServiceClient.MailNotice(userID);
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return num;
        }





        public List<ItemInfo> GetUserBeadEquip(int UserID)
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
                db.GetReader(ref ResultDataReader, "SP_Users_BeadItems_Equip", array);
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

        public int SendMailAndItem(
          string title,
          string content,
          int UserID,
          int templateID,
          int count,
          int validDate,
          int gold,
          int money,
          int StrengthenLevel,
          int AttackCompose,
          int DefendCompose,
          int AgilityCompose,
          int LuckCompose,
          bool isBinds)
        {
            MailInfo mail = new MailInfo()
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
            SqlDataProvider.Data.ItemInfo itemInfo1 = new SqlDataProvider.Data.ItemInfo((ItemTemplateInfo)null);
            itemInfo1.AgilityCompose = AgilityCompose;
            itemInfo1.AttackCompose = AttackCompose;
            itemInfo1.BeginDate = DateTime.Now;
            itemInfo1.Color = "";
            itemInfo1.DefendCompose = DefendCompose;
            itemInfo1.IsDirty = false;
            itemInfo1.IsExist = true;
            itemInfo1.IsJudge = true;
            itemInfo1.LuckCompose = LuckCompose;
            itemInfo1.StrengthenLevel = StrengthenLevel;
            itemInfo1.TemplateID = templateID;
            itemInfo1.ValidDate = validDate;
            itemInfo1.Count = count;
            itemInfo1.IsBinds = isBinds;
            SqlDataProvider.Data.ItemInfo itemInfo2 = itemInfo1;
            int returnValue = 1;
            this.SendMailAndItem(mail, itemInfo2, ref returnValue);
            return returnValue;
        }

        public int SendMailAndItemByNickName(
          string title,
          string content,
          string nickName,
          int gold,
          int money,
          string param)
        {
            PlayerInfo singleByNickName = this.GetUserSingleByNickName(nickName);
            return singleByNickName == null ? 2 : this.SendMailAndItem(title, content, singleByNickName.ID, gold, money, param);
        }

        public int SendMailAndItemByNickName(
          string title,
          string content,
          string NickName,
          int templateID,
          int count,
          int validDate,
          int gold,
          int money,
          int StrengthenLevel,
          int AttackCompose,
          int DefendCompose,
          int AgilityCompose,
          int LuckCompose,
          bool isBinds)
        {
            PlayerInfo singleByNickName = this.GetUserSingleByNickName(NickName);
            return singleByNickName == null ? 2 : this.SendMailAndItem(title, content, singleByNickName.ID, templateID, count, validDate, gold, money, StrengthenLevel, AttackCompose, DefendCompose, AgilityCompose, LuckCompose, isBinds);
        }

        public int SendMailAndItemByUserName(
          string title,
          string content,
          string userName,
          int gold,
          int money,
          string param)
        {
            PlayerInfo singleByUserName = this.GetUserSingleByUserName(userName);
            return singleByUserName == null ? 2 : this.SendMailAndItem(title, content, singleByUserName.ID, gold, money, param);
        }

        public int SendMailAndItemByUserName(
          string title,
          string content,
          string userName,
          int templateID,
          int count,
          int validDate,
          int gold,
          int money,
          int StrengthenLevel,
          int AttackCompose,
          int DefendCompose,
          int AgilityCompose,
          int LuckCompose,
          bool isBinds)
        {
            PlayerInfo singleByUserName = this.GetUserSingleByUserName(userName);
            return singleByUserName == null ? 2 : this.SendMailAndItem(title, content, singleByUserName.ID, templateID, count, validDate, gold, money, StrengthenLevel, AttackCompose, DefendCompose, AgilityCompose, LuckCompose, isBinds);
        }

        public bool SendMailAndMoney(MailInfo mail, ref int returnValue)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[18];
                SqlParameters[0] = new SqlParameter("@ID", (object)mail.ID);
                SqlParameters[0].Direction = ParameterDirection.Output;
                SqlParameters[1] = new SqlParameter("@Annex1", mail.Annex1 == null ? (object)"" : (object)mail.Annex1);
                SqlParameters[2] = new SqlParameter("@Annex2", mail.Annex2 == null ? (object)"" : (object)mail.Annex2);
                SqlParameters[3] = new SqlParameter("@Content", mail.Content == null ? (object)"" : (object)mail.Content);
                SqlParameters[4] = new SqlParameter("@Gold", (object)mail.Gold);
                SqlParameters[5] = new SqlParameter("@IsExist", (object)true);
                SqlParameters[6] = new SqlParameter("@Money", (object)mail.Money);
                SqlParameters[7] = new SqlParameter("@Receiver", mail.Receiver == null ? (object)"" : (object)mail.Receiver);
                SqlParameters[8] = new SqlParameter("@ReceiverID", (object)mail.ReceiverID);
                SqlParameters[9] = new SqlParameter("@Sender", mail.Sender == null ? (object)"" : (object)mail.Sender);
                SqlParameters[10] = new SqlParameter("@SenderID", (object)mail.SenderID);
                SqlParameters[11] = new SqlParameter("@Title", mail.Title == null ? (object)"" : (object)mail.Title);
                SqlParameters[12] = new SqlParameter("@IfDelS", (object)false);
                SqlParameters[13] = new SqlParameter("@IsDelete", (object)false);
                SqlParameters[14] = new SqlParameter("@IsDelR", (object)false);
                SqlParameters[15] = new SqlParameter("@IsRead", (object)false);
                SqlParameters[16] = new SqlParameter("@SendTime", (object)DateTime.Now);
                SqlParameters[17] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[17].Direction = ParameterDirection.ReturnValue;
                flag = this.db.RunProcedure("SP_Admin_SendUserMoney", SqlParameters);
                returnValue = (int)SqlParameters[17].Value;
                flag = returnValue == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool TankAll()
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Tank_All", new SqlParameter[0]);
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool Test(string DutyName)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Test1", new SqlParameter[1]
                {
          new SqlParameter("@DutyName", (object) DutyName)
                });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool UpdateEatPets(EatPetsInfo info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@ID", (object)info.ID), new SqlParameter("@UserID", (object)info.UserID), new SqlParameter("@weaponExp", (object)info.weaponExp), new SqlParameter("@weaponLevel", (object)info.weaponLevel), new SqlParameter("@clothesExp", (object)info.clothesExp), new SqlParameter("@clothesLevel", (object)info.clothesLevel), new SqlParameter("@hatExp", (object)info.hatExp), new SqlParameter("@hatLevel", (object)info.hatLevel) };
                flag = this.db.RunProcedure("SP_Sys_Eat_Pets_Update", sqlParameter);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("SP_Sys_Eat_Pets_Update", exception);
                }
            }
            return flag;
        }

        public bool UpdateAuction(AuctionInfo info, double cess)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[17]
                {
          new SqlParameter("@AuctionID", (object) info.AuctionID),
          new SqlParameter("@AuctioneerID", (object) info.AuctioneerID),
          new SqlParameter("@AuctioneerName", info.AuctioneerName == null ? (object) "" : (object) info.AuctioneerName),
          new SqlParameter("@BeginDate", (object) info.BeginDate),
          new SqlParameter("@BuyerID", (object) info.BuyerID),
          new SqlParameter("@BuyerName", info.BuyerName == null ? (object) "" : (object) info.BuyerName),
          new SqlParameter("@IsExist", (object) info.IsExist),
          new SqlParameter("@ItemID", (object) info.ItemID),
          new SqlParameter("@Mouthful", (object) info.Mouthful),
          new SqlParameter("@PayType", (object) info.PayType),
          new SqlParameter("@Price", (object) info.Price),
          new SqlParameter("@Rise", (object) info.Rise),
          new SqlParameter("@ValidDate", (object) info.ValidDate),
          new SqlParameter("@Name", (object) info.Name),
          new SqlParameter("@Category", (object) info.Category),
          null,
          new SqlParameter("@Cess", (object) cess)
                };
                SqlParameters[15] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[15].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Auction_Update", SqlParameters);
                flag = (int)SqlParameters[15].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool UpdateUsersEventProcess(EventRewardProcessInfo info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[5]
                {
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@ActiveType", (object) info.ActiveType),
          new SqlParameter("@Conditions", (object) info.Conditions),
          new SqlParameter("@AwardGot", (object) info.AwardGot),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[4].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_UpdateUsersEventProcess", SqlParameters);
                flag = (int)SqlParameters[4].Value == 0;
            }
            catch (Exception ex)
            {
                BaseBussiness.log.Error((object)"SP_UpdateUsersEventProcess", ex);
            }
            return flag;
        }

        public bool UpdateBreakTimeWhereServerStop()
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[0].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Update_Marry_Room_Info_Sever_Stop", SqlParameters);
                flag = (int)SqlParameters[0].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(UpdateBreakTimeWhereServerStop), ex);
            }
            return flag;
        }

        public bool UpdateBuyStore(int storeId)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Update_Buy_Store", new SqlParameter[1]
                {
          new SqlParameter("@StoreID", (object) storeId)
                });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_Update_Buy_Store", ex);
            }
            return flag;
        }

        public bool ResetQuests(int UserID)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Quest_Reset", new SqlParameter[1]
                {
          new SqlParameter("@UserID", (object) UserID)
                });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_Quest_Reset", ex);
            }
            return flag;
        }

        public bool UpdateCards(UsersCardInfo item)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[19]
                {
          new SqlParameter("@CardID", (object) item.CardID),
          new SqlParameter("@UserID", (object) item.UserID),
          new SqlParameter("@TemplateID", (object) item.TemplateID),
          new SqlParameter("@Place", (object) item.Place),
          new SqlParameter("@Count", (object) item.Count),
          new SqlParameter("@Attack", (object) item.Attack),
          new SqlParameter("@Defence", (object) item.Defence),
          new SqlParameter("@Agility", (object) item.Agility),
          new SqlParameter("@Luck", (object) item.Luck),
          new SqlParameter("@Guard", (object) item.Guard),
          new SqlParameter("@Damage", (object) item.Damage),
          new SqlParameter("@Level", (object) item.Level),
          new SqlParameter("@CardGP", (object) item.CardGP),
          null,
          new SqlParameter("@AttackReset", (object) item.AttackReset),
          new SqlParameter("@DefenceReset", (object) item.DefenceReset),
          new SqlParameter("@AgilityReset", (object) item.AgilityReset),
          new SqlParameter("@LuckReset", (object) item.LuckReset),
          new SqlParameter("@isFirstGet", (object) item.isFirstGet)
                };
                SqlParameters[13] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[13].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_UpdateUserCard", SqlParameters);
                flag = (int)SqlParameters[13].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_UpdateUserCard", ex);
            }
            return flag;
        }

        public int Updatecash(string UserName, int cash)
        {
            int num = 3;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[3]
                {
          new SqlParameter("@UserName", (object) UserName),
          new SqlParameter("@Cash", (object) cash),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[2].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Update_Cash", SqlParameters);
                num = (int)SqlParameters[2].Value;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return num;
        }

        public bool UpdateDbAchievementDataInfo(AchievementDataInfo info)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Achievement_Data_Add", new SqlParameter[4]
                {
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@AchievementID", (object) info.AchievementID),
          new SqlParameter("@IsComplete", (object) info.IsComplete),
          new SqlParameter("@CompletedDate", (object) info.CompletedDate)
                });
                info.IsDirty = false;
            }
            catch (Exception ex)
            {
                BaseBussiness.log.Error((object)"Init_UpdateDbAchievementDataInfo", ex);
            }
            return flag;
        }

        public List<AchievementDataInfo> GetUserAchievementData(int userID)
        {
            List<AchievementDataInfo> achievementDataInfoList1 = new List<AchievementDataInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)userID;
                this.db.GetReader(ref ResultDataReader, "SP_Achievement_Data_All", SqlParameters);
                while (ResultDataReader.Read())
                {
                    List<AchievementDataInfo> achievementDataInfoList2 = achievementDataInfoList1;
                    AchievementDataInfo achievementDataInfo = new AchievementDataInfo();
                    achievementDataInfo.UserID = (int)ResultDataReader["UserID"];
                    achievementDataInfo.AchievementID = (int)ResultDataReader["AchievementID"];
                    achievementDataInfo.IsComplete = (bool)ResultDataReader["IsComplete"];
                    achievementDataInfo.CompletedDate = (DateTime)ResultDataReader["CompletedDate"];
                    achievementDataInfo.IsDirty = false;
                    achievementDataInfoList2.Add(achievementDataInfo);
                }
            }
            catch (Exception ex)
            {
                BaseBussiness.log.Error((object)"Init_GetUserAchievement", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return achievementDataInfoList1;
        }

        public List<AchievementDataInfo> GetUserAchievementData(
          int userID,
          int id)
        {
            List<AchievementDataInfo> achievementDataInfoList1 = new List<AchievementDataInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[2]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4),
          new SqlParameter("@AchievementID", (object) id)
                };
                SqlParameters[0].Value = (object)userID;
                this.db.GetReader(ref ResultDataReader, "SP_Achievement_Data_Single", SqlParameters);
                while (ResultDataReader.Read())
                {
                    List<AchievementDataInfo> achievementDataInfoList2 = achievementDataInfoList1;
                    AchievementDataInfo achievementDataInfo = new AchievementDataInfo();
                    achievementDataInfo.UserID = (int)ResultDataReader["UserID"];
                    achievementDataInfo.AchievementID = (int)ResultDataReader["AchievementID"];
                    achievementDataInfo.IsComplete = (bool)ResultDataReader["IsComplete"];
                    achievementDataInfo.CompletedDate = (DateTime)ResultDataReader["CompletedDate"];
                    achievementDataInfo.IsDirty = false;
                    achievementDataInfoList2.Add(achievementDataInfo);
                }
            }
            catch (Exception ex)
            {
                BaseBussiness.log.Error((object)"Init_GetUserAchievementSingle", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return achievementDataInfoList1;
        }

        public List<UsersRecordInfo> GetUserRecord(int userID)
        {
            List<UsersRecordInfo> usersRecordInfoList1 = new List<UsersRecordInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)userID;
                this.db.GetReader(ref ResultDataReader, "SP_Users_Record_All", SqlParameters);
                while (ResultDataReader.Read())
                {
                    List<UsersRecordInfo> usersRecordInfoList2 = usersRecordInfoList1;
                    UsersRecordInfo usersRecordInfo = new UsersRecordInfo();
                    usersRecordInfo.UserID = (int)ResultDataReader["UserID"];
                    usersRecordInfo.RecordID = (int)ResultDataReader["RecordID"];
                    usersRecordInfo.Total = (int)ResultDataReader["Total"];
                    usersRecordInfo.IsDirty = false;
                    usersRecordInfoList2.Add(usersRecordInfo);
                }
            }
            catch (Exception ex)
            {
                BaseBussiness.log.Error((object)"Init_GetUserRecord", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return usersRecordInfoList1;
        }

        public bool UpdateDbUserRecord(UsersRecordInfo info)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Users_Record_Add", new SqlParameter[3]
                {
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@RecordID", (object) info.RecordID),
          new SqlParameter("@Total", (object) info.Total)
                });
                info.IsDirty = false;
            }
            catch (Exception ex)
            {
                BaseBussiness.log.Error((object)"Init_UpdateDbUserRecord", ex);
            }
            return flag;
        }

        public bool UpdateDbQuestDataInfo(QuestDataInfo info)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_QuestData_Add", new SqlParameter[11]
                {
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@QuestID", (object) info.QuestID),
          new SqlParameter("@CompletedDate", (object) info.CompletedDate),
          new SqlParameter("@IsComplete", (object) info.IsComplete),
          new SqlParameter("@Condition1", (object) (info.Condition1 <= -1 ? 0 : info.Condition1)),
          new SqlParameter("@Condition2", (object) (info.Condition2 <= -1 ? 0 : info.Condition2)),
          new SqlParameter("@Condition3", (object) (info.Condition3 <= -1 ? 0 : info.Condition3)),
          new SqlParameter("@Condition4", (object) (info.Condition4 <= -1 ? 0 : info.Condition4)),
          new SqlParameter("@IsExist", (object) info.IsExist),
          new SqlParameter("@RepeatFinish", (object) (info.RepeatFinish == -1 ? 1 : info.RepeatFinish)),
          new SqlParameter("@RandDobule", (object) info.RandDobule)
                });
                info.IsDirty = false;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool UpdateFriendHelpTimes(int ID)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[2]
                {
          new SqlParameter("@UserID", (object) ID),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[1].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_UpdateFriendHelpTimes", SqlParameters);
                flag = (int)SqlParameters[1].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_UpdateFriendHelpTimes", ex);
            }
            return flag;
        }

        public bool UpdateGemStoneInfo(UserGemStone g)
        {
            bool flag = false;
            try
            {
                SqlParameter[] sqlParameter = new SqlParameter[] 
                {
                    new SqlParameter("@ID", (object)g.ID),
                    new SqlParameter("@UserID", (object)g.UserID),
                    new SqlParameter("@FigSpiritId", (object)g.FigSpiritId),
                    new SqlParameter("@FigSpiritIdValue", g.FigSpiritIdValue),
                    new SqlParameter("@EquipPlace", (object)g.EquipPlace),
                    new SqlParameter("@Result", SqlDbType.Int)
                };
                sqlParameter[5].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_UpdateGemStoneInfo", sqlParameter);
                flag = true;
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error("SP_UpdateGemStoneInfo", exception);
                }
            }
            return flag;
        }

        public bool UpdateGoods(SqlDataProvider.Data.ItemInfo item)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Users_Items_Update", new SqlParameter[36]
                {
          new SqlParameter("@ItemID", (object) item.ItemID),
          new SqlParameter("@UserID", (object) item.UserID),
          new SqlParameter("@TemplateID", (object) item.Template.TemplateID),
          new SqlParameter("@Place", (object) item.Place),
          new SqlParameter("@AgilityCompose", (object) item.AgilityCompose),
          new SqlParameter("@AttackCompose", (object) item.AttackCompose),
          new SqlParameter("@BeginDate", (object) item.BeginDate),
          new SqlParameter("@Color", item.Color == null ? (object) "" : (object) item.Color),
          new SqlParameter("@Count", (object) item.Count),
          new SqlParameter("@DefendCompose", (object) item.DefendCompose),
          new SqlParameter("@IsBinds", (object) item.IsBinds),
          new SqlParameter("@IsExist", (object) item.IsExist),
          new SqlParameter("@IsJudge", (object) item.IsJudge),
          new SqlParameter("@LuckCompose", (object) item.LuckCompose),
          new SqlParameter("@StrengthenLevel", (object) item.StrengthenLevel),
          new SqlParameter("@ValidDate", (object) item.ValidDate),
          new SqlParameter("@BagType", (object) item.BagType),
          new SqlParameter("@Skin", (object) item.Skin),
          new SqlParameter("@IsUsed", (object) item.IsUsed),
          new SqlParameter("@RemoveDate", (object) item.RemoveDate),
          new SqlParameter("@RemoveType", (object) item.RemoveType),
          new SqlParameter("@Hole1", (object) item.Hole1),
          new SqlParameter("@Hole2", (object) item.Hole2),
          new SqlParameter("@Hole3", (object) item.Hole3),
          new SqlParameter("@Hole4", (object) item.Hole4),
          new SqlParameter("@Hole5", (object) item.Hole5),
          new SqlParameter("@Hole6", (object) item.Hole6),
          new SqlParameter("@StrengthenTimes", (object) item.StrengthenTimes),
          new SqlParameter("@Hole5Level", (object) item.Hole5Level),
          new SqlParameter("@Hole5Exp", (object) item.Hole5Exp),
          new SqlParameter("@Hole6Level", (object) item.Hole6Level),
          new SqlParameter("@Hole6Exp", (object) item.Hole6Exp),
          new SqlParameter("@IsGold", (object) item.IsGold),
          new SqlParameter("@goldBeginTime", (object) item.goldBeginTime),
          new SqlParameter("@goldValidDate", (object) item.goldValidDate),
          new SqlParameter("@StrengthenExp", (object) item.StrengthenExp)
                });
                item.IsDirty = false;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool UpdateLastVIPPackTime(int ID)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[3]
                {
          new SqlParameter("@UserID", (object) ID),
          new SqlParameter("@LastVIPPackTime", (object) DateTime.Now.Date),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[2].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_UpdateUserLastVIPPackTime", SqlParameters);
                flag = true;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_UpdateUserLastVIPPackTime", ex);
            }
            return flag;
        }

        public bool UpdateMail(MailInfo mail, int oldMoney)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[30]
                {
          new SqlParameter("@ID", (object) mail.ID),
          new SqlParameter("@Annex1", mail.Annex1 == null ? (object) "" : (object) mail.Annex1),
          new SqlParameter("@Annex2", mail.Annex2 == null ? (object) "" : (object) mail.Annex2),
          new SqlParameter("@Content", mail.Content == null ? (object) "" : (object) mail.Content),
          new SqlParameter("@Gold", (object) mail.Gold),
          new SqlParameter("@IsExist", (object) mail.IsExist),
          new SqlParameter("@Money", (object) mail.Money),
          new SqlParameter("@Receiver", mail.Receiver == null ? (object) "" : (object) mail.Receiver),
          new SqlParameter("@ReceiverID", (object) mail.ReceiverID),
          new SqlParameter("@Sender", mail.Sender == null ? (object) "" : (object) mail.Sender),
          new SqlParameter("@SenderID", (object) mail.SenderID),
          new SqlParameter("@Title", mail.Title == null ? (object) "" : (object) mail.Title),
          new SqlParameter("@IfDelS", (object) false),
          new SqlParameter("@IsDelete", (object) false),
          new SqlParameter("@IsDelR", (object) false),
          new SqlParameter("@IsRead", (object) mail.IsRead),
          new SqlParameter("@SendTime", (object) mail.SendTime),
          new SqlParameter("@Type", (object) mail.Type),
          new SqlParameter("@OldMoney", (object) oldMoney),
          new SqlParameter("@ValidDate", (object) mail.ValidDate),
          new SqlParameter("@Annex1Name", (object) mail.Annex1Name),
          new SqlParameter("@Annex2Name", (object) mail.Annex2Name),
          new SqlParameter("@Result", SqlDbType.Int),
          null,
          null,
          null,
          null,
          null,
          null,
          null
                };
                SqlParameters[22].Direction = ParameterDirection.ReturnValue;
                SqlParameters[23] = new SqlParameter("@Annex3", mail.Annex3 == null ? (object)"" : (object)mail.Annex3);
                SqlParameters[24] = new SqlParameter("@Annex4", mail.Annex4 == null ? (object)"" : (object)mail.Annex4);
                SqlParameters[25] = new SqlParameter("@Annex5", mail.Annex5 == null ? (object)"" : (object)mail.Annex5);
                SqlParameters[26] = new SqlParameter("@Annex3Name", mail.Annex3Name == null ? (object)"" : (object)mail.Annex3Name);
                SqlParameters[27] = new SqlParameter("@Annex4Name", mail.Annex4Name == null ? (object)"" : (object)mail.Annex4Name);
                SqlParameters[28] = new SqlParameter("@Annex5Name", mail.Annex5Name == null ? (object)"" : (object)mail.Annex5Name);
                SqlParameters[29] = new SqlParameter("GiftToken", (object)mail.GiftToken);
                this.db.RunProcedure("SP_Mail_Update", SqlParameters);
                flag = (int)SqlParameters[22].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool UpdatePayMail(MailInfo mail, int oldMoney)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[29]
                {
          new SqlParameter("@ID", (object) mail.ID),
          new SqlParameter("@Annex1", (object) (mail.Annex1 ?? "")),
          new SqlParameter("@Annex2", (object) (mail.Annex2 ?? "")),
          new SqlParameter("@Content", (object) (mail.Content ?? "")),
          new SqlParameter("@Gold", (object) mail.Gold),
          new SqlParameter("@IsExist", (object) mail.IsExist),
          new SqlParameter("@Money", (object) oldMoney),
          new SqlParameter("@Receiver", (object) (mail.Receiver ?? "")),
          new SqlParameter("@ReceiverID", (object) mail.ReceiverID),
          new SqlParameter("@Sender", (object) (mail.Sender ?? "")),
          new SqlParameter("@SenderID", (object) mail.SenderID),
          new SqlParameter("@Title", (object) (mail.Title ?? "")),
          new SqlParameter("@IfDelS", (object) false),
          new SqlParameter("@IsDelete", (object) false),
          new SqlParameter("@IsDelR", (object) false),
          new SqlParameter("@IsRead", (object) mail.IsRead),
          new SqlParameter("@SendTime", (object) mail.SendTime),
          new SqlParameter("@Type", (object) mail.Type),
          new SqlParameter("@ValidDate", (object) mail.ValidDate),
          new SqlParameter("@Annex1Name", (object) mail.Annex1Name),
          new SqlParameter("@Annex2Name", (object) mail.Annex2Name),
          new SqlParameter("@Result", SqlDbType.Int),
          null,
          null,
          null,
          null,
          null,
          null,
          null
                };
                SqlParameters[21].Direction = ParameterDirection.ReturnValue;
                SqlParameters[22] = new SqlParameter("@Annex3", (object)(mail.Annex3 ?? ""));
                SqlParameters[23] = new SqlParameter("@Annex4", (object)(mail.Annex4 ?? ""));
                SqlParameters[24] = new SqlParameter("@Annex5", (object)(mail.Annex5 ?? ""));
                SqlParameters[25] = new SqlParameter("@Annex3Name", (object)(mail.Annex3Name ?? ""));
                SqlParameters[26] = new SqlParameter("@Annex4Name", (object)(mail.Annex4Name ?? ""));
                SqlParameters[27] = new SqlParameter("@Annex5Name", (object)(mail.Annex5Name ?? ""));
                SqlParameters[28] = new SqlParameter("GiftToken", (object)mail.GiftToken);
                this.db.RunProcedure("SP_Mail_Update_Pay", SqlParameters);
                flag = (int)SqlParameters[21].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool UpdateMarryInfo(MarryInfo info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[6]
                {
          new SqlParameter("@ID", (object) info.ID),
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@IsPublishEquip", (object) info.IsPublishEquip),
          new SqlParameter("@Introduction", (object) info.Introduction),
          new SqlParameter("@RegistTime", (object) info.RegistTime.ToString("yyyy-MM-dd HH:mm:ss")),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[5].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_MarryInfo_Update", SqlParameters);
                flag = (int)SqlParameters[5].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool UpdateMarryRoomInfo(MarryRoomInfo info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[9]
                {
          new SqlParameter("@ID", (object) info.ID),
          new SqlParameter("@AvailTime", (object) info.AvailTime),
          new SqlParameter("@BreakTime", (object) info.BreakTime),
          new SqlParameter("@roomIntroduction", (object) info.RoomIntroduction),
          new SqlParameter("@isHymeneal", (object) info.IsHymeneal),
          new SqlParameter("@Name", (object) info.Name),
          new SqlParameter("@Pwd", (object) info.Pwd),
          new SqlParameter("@IsGunsaluteUsed", (object) info.IsGunsaluteUsed),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[8].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Update_Marry_Room_Info", SqlParameters);
                flag = (int)SqlParameters[8].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(UpdateMarryRoomInfo), ex);
            }
            return flag;
        }

        public bool UpdatePassWord(int userID, string password)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Users_UpdatePassword", new SqlParameter[2]
                {
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@Password", (object) password)
                });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool UpdatePasswordInfo(
          int userID,
          string PasswordQuestion1,
          string PasswordAnswer1,
          string PasswordQuestion2,
          string PasswordAnswer2,
          int Count)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Users_Password_Add", new SqlParameter[6]
                {
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@PasswordQuestion1", (object) PasswordQuestion1),
          new SqlParameter("@PasswordAnswer1", (object) PasswordAnswer1),
          new SqlParameter("@PasswordQuestion2", (object) PasswordQuestion2),
          new SqlParameter("@PasswordAnswer2", (object) PasswordAnswer2),
          new SqlParameter("@FailedPasswordAttemptCount", (object) Count)
                });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool UpdatePasswordTwo(int userID, string passwordTwo)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Users_UpdatePasswordTwo", new SqlParameter[2]
                {
          new SqlParameter("@UserID", (object) userID),
          new SqlParameter("@PasswordTwo", (object) passwordTwo)
                });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool UpdatePlayer(PlayerInfo player)
        {
            bool flag = false;
            try
            {
                if (player.Grade < 1)
                    return flag;
                SqlParameter[] SqlParameters = new SqlParameter[75];
                SqlParameters[0] = new SqlParameter("@UserID", (object)player.ID);
                SqlParameters[1] = new SqlParameter("@Attack", (object)player.Attack);
                SqlParameters[2] = new SqlParameter("@Colors", player.Colors == null ? (object)"" : (object)player.Colors);
                SqlParameters[3] = new SqlParameter("@ConsortiaID", (object)player.ConsortiaID);
                SqlParameters[4] = new SqlParameter("@Defence", (object)player.Defence);
                SqlParameters[5] = new SqlParameter("@Gold", (object)player.Gold);
                SqlParameters[6] = new SqlParameter("@GP", (object)player.GP);
                SqlParameters[7] = new SqlParameter("@Grade", (object)player.Grade);
                SqlParameters[8] = new SqlParameter("@Luck", (object)player.Luck);
                SqlParameters[9] = new SqlParameter("@Money", (object)player.Money);
                SqlParameters[10] = new SqlParameter("@Style", player.Style == null ? (object)"" : (object)player.Style);
                SqlParameters[11] = new SqlParameter("@Agility", (object)player.Agility);
                SqlParameters[12] = new SqlParameter("@State", (object)player.State);
                SqlParameters[13] = new SqlParameter("@Hide", (object)player.Hide);
                SqlParameters[14] = new SqlParameter("@ExpendDate", !player.ExpendDate.HasValue ? (object)"" : (object)player.ExpendDate.ToString());
                SqlParameters[15] = new SqlParameter("@Win", (object)player.Win);
                SqlParameters[16] = new SqlParameter("@Total", (object)player.Total);
                SqlParameters[17] = new SqlParameter("@Escape", (object)player.Escape);
                SqlParameters[18] = new SqlParameter("@Skin", player.Skin == null ? (object)"" : (object)player.Skin);
                SqlParameters[19] = new SqlParameter("@Offer", (object)player.Offer);
                SqlParameters[20] = new SqlParameter("@AntiAddiction", (object)player.AntiAddiction);
                SqlParameters[20].Direction = ParameterDirection.InputOutput;
                SqlParameters[21] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[21].Direction = ParameterDirection.ReturnValue;
                SqlParameters[22] = new SqlParameter("@RichesOffer", (object)player.RichesOffer);
                SqlParameters[23] = new SqlParameter("@RichesRob", (object)player.RichesRob);
                SqlParameters[24] = new SqlParameter("@CheckCount", (object)player.CheckCount);
                SqlParameters[24].Direction = ParameterDirection.InputOutput;
                SqlParameters[25] = new SqlParameter("@MarryInfoID", (object)player.MarryInfoID);
                SqlParameters[26] = new SqlParameter("@DayLoginCount", (object)player.DayLoginCount);
                SqlParameters[27] = new SqlParameter("@Nimbus", (object)player.Nimbus);
                SqlParameters[28] = new SqlParameter("@LastAward", (object)player.LastAward);
                SqlParameters[29] = new SqlParameter("@GiftToken", (object)player.GiftToken);
                SqlParameters[30] = new SqlParameter("@QuestSite", (object)player.QuestSite);
                SqlParameters[31] = new SqlParameter("@PvePermission", (object)player.PvePermission);
                SqlParameters[32] = new SqlParameter("@FightPower", (object)player.FightPower);
                SqlParameters[33] = new SqlParameter("@AnswerSite", (object)player.AnswerSite);
                SqlParameters[34] = new SqlParameter("@LastAuncherAward", (object)player.LastAward);
                SqlParameters[35] = new SqlParameter("@hp", (object)player.hp);
                SqlParameters[36] = new SqlParameter("@ChatCount", (object)player.ChatCount);
                SqlParameters[37] = new SqlParameter("@SpaPubGoldRoomLimit", (object)player.SpaPubGoldRoomLimit);
                SqlParameters[38] = new SqlParameter("@LastSpaDate", (object)player.LastSpaDate);
                SqlParameters[39] = new SqlParameter("@FightLabPermission", (object)player.FightLabPermission);
                SqlParameters[40] = new SqlParameter("@SpaPubMoneyRoomLimit", (object)player.SpaPubMoneyRoomLimit);
                SqlParameters[41] = new SqlParameter("@IsInSpaPubGoldToday", (object)player.IsInSpaPubGoldToday);
                SqlParameters[42] = new SqlParameter("@IsInSpaPubMoneyToday", (object)player.IsInSpaPubMoneyToday);
                SqlParameters[43] = new SqlParameter("@AchievementPoint", (object)player.AchievementPoint);
                SqlParameters[44] = new SqlParameter("@LastWeekly", (object)player.LastWeekly);
                SqlParameters[45] = new SqlParameter("@LastWeeklyVersion", (object)player.LastWeeklyVersion);
                SqlParameters[46] = new SqlParameter("@WeaklessGuildProgressStr", (object)player.WeaklessGuildProgressStr);
                SqlParameters[47] = new SqlParameter("@IsOldPlayer", (object)player.IsOldPlayer);
                SqlParameters[48] = new SqlParameter("@VIPLevel", (object)player.VIPLevel);
                SqlParameters[49] = new SqlParameter("@VIPExp", (object)player.VIPExp);
                SqlParameters[50] = new SqlParameter("@Score", (object)player.Score);
                SqlParameters[51] = new SqlParameter("@OptionOnOff", (object)player.OptionOnOff);
                SqlParameters[52] = new SqlParameter("@isOldPlayerHasValidEquitAtLogin", (object)player.isOldPlayerHasValidEquitAtLogin);
                SqlParameters[53] = new SqlParameter("@badLuckNumber", (object)player.badLuckNumber);
                SqlParameters[54] = new SqlParameter("@luckyNum", (object)player.luckyNum);
                SqlParameters[55] = new SqlParameter("@lastLuckyNumDate", (object)player.lastLuckyNumDate);
                SqlParameters[56] = new SqlParameter("@lastLuckNum", (object)player.lastLuckNum);
                SqlParameters[57] = new SqlParameter("@IsShowConsortia", (object)player.IsShowConsortia);
                SqlParameters[58] = new SqlParameter("@NewDay", (object)player.NewDay);
                SqlParameters[59] = new SqlParameter("@Medal", (object)player.medal);
                SqlParameters[60] = new SqlParameter("@Honor", (object)player.Honor);
                SqlParameters[61] = new SqlParameter("@VIPNextLevelDaysNeeded", (object)player.GetVIPNextLevelDaysNeeded(player.VIPLevel, player.VIPExp));
                SqlParameters[62] = new SqlParameter("@IsRecharged", (object)player.IsRecharged);
                SqlParameters[63] = new SqlParameter("@IsGetAward", (object)player.IsGetAward);
                SqlParameters[64] = new SqlParameter("@typeVIP", (object)player.typeVIP);
                SqlParameters[65] = new SqlParameter("@evolutionGrade", (object)player.evolutionGrade);
                SqlParameters[66] = new SqlParameter("@evolutionExp", (object)player.evolutionExp);
                SqlParameters[67] = new SqlParameter("@hardCurrency", (object)player.hardCurrency);
                SqlParameters[68] = new SqlParameter("@EliteScore", (object)player.EliteScore);
                SqlParameters[69] = new SqlParameter("@UseOffer", (object)player.UseOffer);
                SqlParameters[70] = new SqlParameter("@ShopFinallyGottenTime", (object)player.ShopFinallyGottenTime);
                SqlParameters[71] = new SqlParameter("@MoneyLock", (object)player.MoneyLock);
                SqlParameters[72] = new SqlParameter("@totemId", (object)player.totemId);
                SqlParameters[73] = new SqlParameter("@myHonor", (object)player.myHonor);
                SqlParameters[74] = new SqlParameter("@MaxBuyHonor", (object)player.MaxBuyHonor);
                this.db.RunProcedure("SP_Users_Update", SqlParameters);
                flag = (int)SqlParameters[21].Value == 0;
                if (flag)
                {
                    player.AntiAddiction = (int)SqlParameters[20].Value;
                    player.CheckCount = (int)SqlParameters[24].Value;
                }
                player.IsDirty = false;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool UpdatePlayerGotRingProp(int groomID, int brideID)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[3]
                {
          new SqlParameter("@GroomID", (object) groomID),
          new SqlParameter("@BrideID", (object) brideID),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[2].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Update_GotRing_Prop", SqlParameters);
                flag = (int)SqlParameters[2].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(UpdatePlayerGotRingProp), ex);
            }
            return flag;
        }

        public bool UpdatePlayerLastAward(int id, int type)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Users_LastAward", new SqlParameter[2]
                {
          new SqlParameter("@UserID", (object) id),
          new SqlParameter("@Type", (object) type)
                });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"UpdatePlayerAward", ex);
            }
            return flag;
        }

        public bool UpdatePlayerMarry(PlayerInfo player)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Users_Marry", new SqlParameter[7]
                {
          new SqlParameter("@UserID", (object) player.ID),
          new SqlParameter("@IsMarried", (object) player.IsMarried),
          new SqlParameter("@SpouseID", (object) player.SpouseID),
          new SqlParameter("@SpouseName", (object) player.SpouseName),
          new SqlParameter("@IsCreatedMarryRoom", (object) player.IsCreatedMarryRoom),
          new SqlParameter("@SelfMarryRoomID", (object) player.SelfMarryRoomID),
          new SqlParameter("@IsGotRing", (object) player.IsGotRing)
                });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(UpdatePlayerMarry), ex);
            }
            return flag;
        }

        public bool UpdatePlayerMarryApply(int UserID, string loveProclamation, bool isExist)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[4]
                {
          new SqlParameter("@UserID", (object) UserID),
          new SqlParameter("@LoveProclamation", (object) loveProclamation),
          new SqlParameter("@isExist", (object) isExist),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[3].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Update_Marry_Apply", SqlParameters);
                flag = (int)SqlParameters[3].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(UpdatePlayerMarryApply), ex);
            }
            return flag;
        }

        public bool UpdateUserDrillInfo(UserDrillInfo g)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[6]
                {
          new SqlParameter("@UserID", (object) g.UserID),
          new SqlParameter("@BeadPlace", (object) g.BeadPlace),
          new SqlParameter("@HoleExp", (object) g.HoleExp),
          new SqlParameter("@HoleLv", (object) g.HoleLv),
          new SqlParameter("@DrillPlace", (object) g.DrillPlace),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[5].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_UpdateUserDrillInfo", SqlParameters);
                flag = true;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_UpdateUserDrillInfo", ex);
            }
            return flag;
        }

        public bool UpdateUserMatchInfo(UserMatchInfo info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[16]
                {
          new SqlParameter("@ID", (object) info.ID),
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@dailyScore", (object) info.dailyScore),
          new SqlParameter("@dailyWinCount", (object) info.dailyWinCount),
          new SqlParameter("@dailyGameCount", (object) info.dailyGameCount),
          new SqlParameter("@DailyLeagueFirst", (object) info.DailyLeagueFirst),
          new SqlParameter("@DailyLeagueLastScore", (object) info.DailyLeagueLastScore),
          new SqlParameter("@weeklyScore", (object) info.weeklyScore),
          new SqlParameter("@weeklyGameCount", (object) info.weeklyGameCount),
          new SqlParameter("@weeklyRanking", (object) info.weeklyRanking),
          new SqlParameter("@addDayPrestge", (object) info.addDayPrestge),
          new SqlParameter("@totalPrestige", (object) info.totalPrestige),
          new SqlParameter("@restCount", (object) info.restCount),
          new SqlParameter("@leagueGrade", (object) info.leagueGrade),
          new SqlParameter("@leagueItemsGet", (object) info.leagueItemsGet),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[15].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_UpdateUserMatch", SqlParameters);
                flag = (int)SqlParameters[15].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_UpdateUserMatch", ex);
            }
            return flag;
        }

        public bool UpdateUserRank(UserRankInfo item)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[14]
                {
          new SqlParameter("@ID", (object) item.ID),
          new SqlParameter("@UserID", (object) item.UserID),
          new SqlParameter("@UserRank", (object) item.UserRank),
          new SqlParameter("@Attack", (object) item.Attack),
          new SqlParameter("@Defence", (object) item.Defence),
          new SqlParameter("@Luck", (object) item.Luck),
          new SqlParameter("@Agility", (object) item.Agility),
          new SqlParameter("@HP", (object) item.HP),
          new SqlParameter("@Damage", (object) item.Damage),
          new SqlParameter("@Guard", (object) item.Guard),
          new SqlParameter("@BeginDate", (object) item.BeginDate),
          new SqlParameter("@Validate", (object) item.Validate),
          new SqlParameter("@IsExit", (object) item.IsExit),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[13].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_UpdateUserRank", SqlParameters);
                flag = (int)SqlParameters[13].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_UpdateUserRank", ex);
            }
            return flag;
        }

        public bool UpdateUserExtra(UsersExtraInfo ex)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_Update_User_Extra", new SqlParameter[11]
                {
          new SqlParameter("@UserID", (object) ex.UserID),
          new SqlParameter("@LastTimeHotSpring", (object) ex.LastTimeHotSpring),
          new SqlParameter("@MinHotSpring", (object) ex.MinHotSpring),
          new SqlParameter("@coupleBossEnterNum", (object) ex.coupleBossEnterNum),
          new SqlParameter("@coupleBossHurt", (object) ex.coupleBossHurt),
          new SqlParameter("@coupleBossBoxNum", (object) ex.coupleBossBoxNum),
          new SqlParameter("@LastFreeTimeHotSpring", (object) ex.LastFreeTimeHotSpring),
          new SqlParameter("@isGetAwardMarry", (object) ex.isGetAwardMarry),
          new SqlParameter("@isFirstAwardMarry", (object) ex.isFirstAwardMarry),
          new SqlParameter("@LeftRoutteCount", (object) ex.LeftRoutteCount),
          new SqlParameter("@LeftRoutteRate", (object) ex.LeftRoutteRate)
                });
            }
            catch (Exception ex1)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex1);
            }
            return flag;
        }

        public bool UpdateUserTexpInfo(TexpInfo info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[10]
                {
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@attTexpExp", (object) info.attTexpExp),
          new SqlParameter("@defTexpExp", (object) info.defTexpExp),
          new SqlParameter("@hpTexpExp", (object) info.hpTexpExp),
          new SqlParameter("@lukTexpExp", (object) info.lukTexpExp),
          new SqlParameter("@spdTexpExp", (object) info.spdTexpExp),
          new SqlParameter("@texpCount", (object) info.texpCount),
          new SqlParameter("@texpTaskCount", (object) info.texpTaskCount),
          new SqlParameter("@texpTaskDate", (object) info.texpTaskDate),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[9].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_UserTexp_Update", SqlParameters);
                flag = (int)SqlParameters[9].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool UpdateVIPInfo(PlayerInfo p)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[10]
                {
          new SqlParameter("@ID", (object) p.ID),
          new SqlParameter("@VIPLevel", (object) p.VIPLevel),
          new SqlParameter("@VIPExp", (object) p.VIPExp),
          new SqlParameter("@VIPOnlineDays", SqlDbType.BigInt),
          new SqlParameter("@VIPOfflineDays", SqlDbType.BigInt),
          new SqlParameter("@VIPExpireDay", (object) p.VIPExpireDay),
          new SqlParameter("@VIPLastDate", (object) DateTime.Now),
          new SqlParameter("@VIPNextLevelDaysNeeded", (object) p.GetVIPNextLevelDaysNeeded(p.VIPLevel, p.VIPExp)),
          new SqlParameter("@CanTakeVipReward", (object) p.CanTakeVipReward),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[9].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_UpdateVIPInfo", SqlParameters);
                flag = true;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_UpdateVIPInfo", ex);
            }
            return flag;
        }

        public int VIPLastdate(int ID)
        {
            int num = 0;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[2]
                {
          new SqlParameter("@UserID", (object) ID),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[1].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_VIPLastdate_Single", SqlParameters);
                num = (int)SqlParameters[1].Value;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_VIPLastdate_Single", ex);
            }
            return num;
        }

        public int VIPRenewal(
          string nickName,
          int renewalDays,
          int typeVIP,
          ref DateTime ExpireDayOut)
        {
            int num = 0;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[5]
                {
          new SqlParameter("@NickName", (object) nickName),
          new SqlParameter("@RenewalDays", (object) renewalDays),
          new SqlParameter("@ExpireDayOut", (object) DateTime.Now),
          new SqlParameter("@typeVIP", (object) typeVIP),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[2].Direction = ParameterDirection.Output;
                SqlParameters[4].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_VIPRenewal_Single", SqlParameters);
                ExpireDayOut = (DateTime)SqlParameters[2].Value;
                num = (int)SqlParameters[4].Value;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_VIPRenewal_Single", ex);
            }
            return num;
        }
        public int VIPRenewal2(
          string nickName,
          int renewalDays,
          int typeVIP,
          ref DateTime ExpireDayOut)
        {
            int num = 0;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[5]
                {
          new SqlParameter("@NickName", (object) nickName),
          new SqlParameter("@RenewalDays", (object) renewalDays),
          new SqlParameter("@ExpireDayOut", (object) DateTime.Now),
          new SqlParameter("@typeVIP", (object) typeVIP),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[2].Direction = ParameterDirection.Output;
                SqlParameters[4].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_VIPRenewal_Single2", SqlParameters);
                ExpireDayOut = (DateTime)SqlParameters[2].Value;
                num = (int)SqlParameters[4].Value;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_VIPRenewal_Single2", ex);
            }
            return num;
        }

        public bool UpdateAcademyPlayer(PlayerInfo player)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[8]
                {
          new SqlParameter("@UserID", (object) player.ID),
          new SqlParameter("@apprenticeshipState", (object) player.apprenticeshipState),
          new SqlParameter("@masterID", (object) player.masterID),
          new SqlParameter("@masterOrApprentices", (object) player.masterOrApprentices),
          new SqlParameter("@graduatesCount", (object) player.graduatesCount),
          new SqlParameter("@honourOfMaster", (object) player.honourOfMaster),
          null,
          new SqlParameter("@freezesDate", (object) player.freezesDate)
                };
                SqlParameters[6] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[6].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_UsersAcademy_Update", SqlParameters);
                flag = (int)SqlParameters[6].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(UpdateAcademyPlayer), ex);
            }
            return flag;
        }

        public void AddDailyRecord(DailyRecordInfo info)
        {
            try
            {
                this.db.RunProcedure("SP_DailyRecordInfo_Add", new SqlParameter[3]
                {
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@Type", (object) info.Type),
          new SqlParameter("@Value", (object) info.Value)
                });
            }
            catch (Exception ex)
            {
                if (!BaseBussiness.log.IsErrorEnabled)
                    return;
                BaseBussiness.log.Error((object)nameof(AddDailyRecord), ex);
            }
        }

        public bool DeleteDailyRecord(int UserID, int Type)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_DailyRecordInfo_Delete", new SqlParameter[2]
                {
          new SqlParameter("@UserID", (object) UserID),
          new SqlParameter("@Type", (object) Type)
                });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_DailyRecordInfo_Delete", ex);
            }
            return flag;
        }

        public DailyRecordInfo[] GetDailyRecord(int UserID)
        {
            List<DailyRecordInfo> dailyRecordInfoList = new List<DailyRecordInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", (object) UserID)
                };
                this.db.GetReader(ref ResultDataReader, "SP_DailyRecordInfo_Single", SqlParameters);
                while (ResultDataReader.Read())
                {
                    DailyRecordInfo dailyRecordInfo = new DailyRecordInfo()
                    {
                        UserID = (int)ResultDataReader[nameof(UserID)],
                        Type = (int)ResultDataReader["Type"],
                        Value = (string)ResultDataReader["Value"]
                    };
                    dailyRecordInfoList.Add(dailyRecordInfo);
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(GetDailyRecord), ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return dailyRecordInfoList.ToArray();
        }

        public string GetASSInfoSingle(int UserID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", (object) UserID)
                };
                this.db.GetReader(ref ResultDataReader, "SP_ASSInfo_Single", SqlParameters);
                if (ResultDataReader.Read())
                    return ResultDataReader["IDNumber"].ToString();
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(GetASSInfoSingle), ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return "";
        }

        public DailyLogListInfo GetDailyLogListSingle(int UserID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", (object) UserID)
                };
                this.db.GetReader(ref ResultDataReader, "SP_DailyLogList_Single", SqlParameters);
                if (ResultDataReader.Read())
                    return new DailyLogListInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        UserID = (int)ResultDataReader[nameof(UserID)],
                        UserAwardLog = (int)ResultDataReader["UserAwardLog"],
                        DayLog = (string)ResultDataReader["DayLog"],
                        LastDate = (DateTime)ResultDataReader["LastDate"]
                    };
            }
            catch (Exception ex)
            {
                BaseBussiness.log.Error((object)"DailyLogList", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (DailyLogListInfo)null;
        }

        public bool UpdateDailyLogList(DailyLogListInfo info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[5]
                {
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@UserAwardLog", (object) info.UserAwardLog),
          new SqlParameter("@DayLog", (object) info.DayLog),
          new SqlParameter("@LastDate", (object) info.LastDate),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[4].Direction = ParameterDirection.ReturnValue;
                flag = this.db.RunProcedure("SP_DailyLogList_Update", SqlParameters);
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_DailyLogList_Update", ex);
            }
            return flag;
        }

        public bool UpdateBoxProgression(
          int userid,
          int boxProgression,
          int getBoxLevel,
          DateTime addGPLastDate,
          DateTime BoxGetDate,
          int alreadyBox)
        {
            bool flag = false;
            try
            {
                flag = this.db.RunProcedure("SP_User_Update_BoxProgression", new SqlParameter[6]
                {
          new SqlParameter("@UserID", (object) userid),
          new SqlParameter("@BoxProgression", (object) boxProgression),
          new SqlParameter("@GetBoxLevel", (object) getBoxLevel),
          new SqlParameter("@AddGPLastDate", (object) DateTime.Now),
          new SqlParameter("@BoxGetDate", (object) BoxGetDate),
          new SqlParameter("@AlreadyGetBox", (object) alreadyBox)
                });
            }
            catch (Exception ex)
            {
                BaseBussiness.log.Error((object)"User_Update_BoxProgression", ex);
            }
            return flag;
        }

        public bool UpdatePlayerInfoHistory(PlayerInfoHistory info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[4]
                {
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@LastQuestsTime", (object) info.LastQuestsTime),
          new SqlParameter("@LastTreasureTime", (object) info.LastTreasureTime),
          new SqlParameter("@OutPut", SqlDbType.Int)
                };
                SqlParameters[3].Direction = ParameterDirection.Output;
                this.db.RunProcedure("SP_User_Update_History", SqlParameters);
                flag = (int)SqlParameters[6].Value == 1;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"User_Update_BoxProgression", ex);
            }
            return flag;
        }

        public bool AddAASInfo(AASInfo info)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[5]
                {
          new SqlParameter("@UserID", (object) info.UserID),
          new SqlParameter("@Name", (object) info.Name),
          new SqlParameter("@IDNumber", (object) info.IDNumber),
          new SqlParameter("@State", (object) info.State),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[4].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_ASSInfo_Add", SqlParameters);
                flag = (int)SqlParameters[4].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"UpdateAASInfo", ex);
            }
            return flag;
        }

        public void AddUserLogEvent(
          int UserID,
          string UserName,
          string NickName,
          string Type,
          string Content)
        {
            try
            {
                this.db.RunProcedure("SP_Insert_UsersLog", new SqlParameter[5]
                {
          new SqlParameter("@UserID", (object) UserID),
          new SqlParameter("@UserName", (object) UserName),
          new SqlParameter("@NickName", (object) NickName),
          new SqlParameter("@Type", (object) Type),
          new SqlParameter("@Content", (object) Content)
                });
            }
            catch (Exception ex)
            {
            }
        }

        public Dictionary<int, List<string>> LoadCommands()
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            Dictionary<int, List<string>> dictionary = new Dictionary<int, List<string>>();
            this.db.GetReader(ref ResultDataReader, "SP_GetAllCommands");
            while (ResultDataReader.Read())
            {
                string[] strArray = Convert.ToString(ResultDataReader["Commands"] ?? (object)"").Split('$');
                List<string> stringList = new List<string>();
                foreach (string str in strArray)
                    stringList.Add(str);
                if (!dictionary.ContainsKey(Convert.ToInt32(ResultDataReader["UserID"] ?? (object)0)))
                    dictionary.Add(Convert.ToInt32(ResultDataReader["UserID"] ?? (object)0), stringList);
            }
            return dictionary;
        }

        public UsersPetInfo[] GetUserPetSingles(int UserID)
        {
            List<UsersPetInfo> usersPetInfoList = new List<UsersPetInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@UserID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)UserID;
                this.db.GetReader(ref ResultDataReader, "SP_Get_UserPet_By_ID", SqlParameters);
                while (ResultDataReader.Read())
                    usersPetInfoList.Add(this.InitPet(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return usersPetInfoList.ToArray();
        }

        public bool UpdateUserPet(UsersPetInfo item)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[39]
                {
          new SqlParameter("@TemplateID", (object) item.TemplateID),
          new SqlParameter("@Name", item.Name == null ? (object) "Error!" : (object) item.Name),
          new SqlParameter("@UserID", (object) item.UserID),
          new SqlParameter("@Attack", (object) item.Attack),
          new SqlParameter("@Defence", (object) item.Defence),
          new SqlParameter("@Luck", (object) item.Luck),
          new SqlParameter("@Agility", (object) item.Agility),
          new SqlParameter("@Blood", (object) item.Blood),
          new SqlParameter("@Damage", (object) item.Damage),
          new SqlParameter("@Guard", (object) item.Guard),
          new SqlParameter("@AttackGrow", (object) item.AttackGrow),
          new SqlParameter("@DefenceGrow", (object) item.DefenceGrow),
          new SqlParameter("@LuckGrow", (object) item.LuckGrow),
          new SqlParameter("@AgilityGrow", (object) item.AgilityGrow),
          new SqlParameter("@BloodGrow", (object) item.BloodGrow),
          new SqlParameter("@DamageGrow", (object) item.DamageGrow),
          new SqlParameter("@GuardGrow", (object) item.GuardGrow),
          new SqlParameter("@Level", (object) item.Level),
          new SqlParameter("@GP", (object) item.GP),
          new SqlParameter("@MaxGP", (object) item.MaxGP),
          new SqlParameter("@Hunger", (object) item.Hunger),
          new SqlParameter("@PetHappyStar", (object) item.PetHappyStar),
          new SqlParameter("@MP", (object) item.MP),
          new SqlParameter("@IsEquip", (object) item.IsEquip),
          new SqlParameter("@Place", (object) item.Place),
          new SqlParameter("@IsExit", (object) item.IsExit),
          new SqlParameter("@ID", (object) item.ID),
          new SqlParameter("@Skill", (object) item.Skill),
          new SqlParameter("@SkillEquip", (object) item.SkillEquip),
          new SqlParameter("@currentStarExp", (object) item.currentStarExp),
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
                SqlParameters[30].Direction = ParameterDirection.ReturnValue;
                SqlParameters[31] = new SqlParameter("@breakGrade", (object)item.breakGrade);
                SqlParameters[32] = new SqlParameter("@breakAttack", (object)item.breakAttack);
                SqlParameters[33] = new SqlParameter("@breakDefence", (object)item.breakDefence);
                SqlParameters[34] = new SqlParameter("@breakAgility", (object)item.breakAgility);
                SqlParameters[35] = new SqlParameter("@breakLuck", (object)item.breakLuck);
                SqlParameters[36] = new SqlParameter("@breakBlood", (object)item.breakBlood);
                SqlParameters[37] = new SqlParameter("@eQPets", (object)item.eQPets);
                SqlParameters[38] = new SqlParameter("@BaseProp", (object)item.BaseProp);
                this.db.RunProcedure("SP_UserPet_Update", SqlParameters);
                flag = (int)SqlParameters[30].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool AddUserPet(UsersPetInfo item)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[39]
                {
          new SqlParameter("@TemplateID", (object) item.TemplateID),
          new SqlParameter("@Name", item.Name == null ? (object) "Error!" : (object) item.Name),
          new SqlParameter("@UserID", (object) item.UserID),
          new SqlParameter("@Attack", (object) item.Attack),
          new SqlParameter("@Defence", (object) item.Defence),
          new SqlParameter("@Luck", (object) item.Luck),
          new SqlParameter("@Agility", (object) item.Agility),
          new SqlParameter("@Blood", (object) item.Blood),
          new SqlParameter("@Damage", (object) item.Damage),
          new SqlParameter("@Guard", (object) item.Guard),
          new SqlParameter("@AttackGrow", (object) item.AttackGrow),
          new SqlParameter("@DefenceGrow", (object) item.DefenceGrow),
          new SqlParameter("@LuckGrow", (object) item.LuckGrow),
          new SqlParameter("@AgilityGrow", (object) item.AgilityGrow),
          new SqlParameter("@BloodGrow", (object) item.BloodGrow),
          new SqlParameter("@DamageGrow", (object) item.DamageGrow),
          new SqlParameter("@GuardGrow", (object) item.GuardGrow),
          new SqlParameter("@Level", (object) item.Level),
          new SqlParameter("@GP", (object) item.GP),
          new SqlParameter("@MaxGP", (object) item.MaxGP),
          new SqlParameter("@Hunger", (object) item.Hunger),
          new SqlParameter("@PetHappyStar", (object) item.PetHappyStar),
          new SqlParameter("@MP", (object) item.MP),
          new SqlParameter("@IsEquip", (object) item.IsEquip),
          new SqlParameter("@Skill", (object) item.Skill),
          new SqlParameter("@SkillEquip", (object) item.SkillEquip),
          new SqlParameter("@Place", (object) item.Place),
          new SqlParameter("@IsExit", (object) item.IsExit),
          new SqlParameter("@ID", (object) item.ID),
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
                SqlParameters[28].Direction = ParameterDirection.Output;
                SqlParameters[29] = new SqlParameter("@currentStarExp", (object)item.currentStarExp);
                SqlParameters[30] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[30].Direction = ParameterDirection.ReturnValue;
                SqlParameters[31] = new SqlParameter("@breakGrade", (object)item.breakGrade);
                SqlParameters[32] = new SqlParameter("@breakAttack", (object)item.breakAttack);
                SqlParameters[33] = new SqlParameter("@breakDefence", (object)item.breakDefence);
                SqlParameters[34] = new SqlParameter("@breakAgility", (object)item.breakAgility);
                SqlParameters[35] = new SqlParameter("@breakLuck", (object)item.breakLuck);
                SqlParameters[36] = new SqlParameter("@breakBlood", (object)item.breakBlood);
                SqlParameters[37] = new SqlParameter("@eQPets", (object)item.eQPets);
                SqlParameters[38] = new SqlParameter("@BaseProp", (object)item.BaseProp);
                flag = this.db.RunProcedure("SP_User_Add_Pet", SqlParameters);
                flag = (int)SqlParameters[30].Value == 0;
                item.ID = (int)SqlParameters[28].Value;
                item.IsDirty = false;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public UsersPetInfo InitPet(SqlDataReader reader)
        {
            return new UsersPetInfo()
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
                eQPets = reader["eQPets"] == null ? "" : reader["eQPets"].ToString(),
                BaseProp = reader["BaseProp"] == null ? "" : reader["BaseProp"].ToString()
            };
        }

        public bool RegisterPlayer2(
          string userName,
          string passWord,
          string nickName,
          int attack,
          int defence,
          int agility,
          int luck,
          int cateogryId,
          string bStyle,
          string bPic,
          string gStyle,
          string armColor,
          string hairColor,
          string faceColor,
          string clothColor,
          string hatchColor,
          int sex,
          ref string msg,
          int validDate)
        {
            bool flag = false;
            try
            {
                string[] strArray1 = bStyle.Split(',');
                string[] strArray2 = gStyle.Split(',');
                string[] strArray3 = bPic.Split(',');
                SqlParameter[] SqlParameters = new SqlParameter[31];
                SqlParameters[0] = new SqlParameter("@UserName", (object)userName);
                SqlParameters[1] = new SqlParameter("@PassWord", (object)passWord);
                SqlParameters[2] = new SqlParameter("@NickName", (object)nickName);
                SqlParameters[3] = new SqlParameter("@BArmID", (object)int.Parse(strArray1[0]));
                SqlParameters[4] = new SqlParameter("@BHairID", (object)int.Parse(strArray1[1]));
                SqlParameters[5] = new SqlParameter("@BFaceID", (object)int.Parse(strArray1[2]));
                SqlParameters[6] = new SqlParameter("@BClothID", (object)int.Parse(strArray1[3]));
                SqlParameters[7] = new SqlParameter("@BHatID", (object)int.Parse(strArray1[4]));
                SqlParameters[21] = new SqlParameter("@ArmPic", (object)strArray3[0]);
                SqlParameters[22] = new SqlParameter("@HairPic", (object)strArray3[1]);
                SqlParameters[23] = new SqlParameter("@FacePic", (object)strArray3[2]);
                SqlParameters[24] = new SqlParameter("@ClothPic", (object)strArray3[3]);
                SqlParameters[25] = new SqlParameter("@HatPic", (object)strArray3[4]);
                SqlParameters[8] = new SqlParameter("@GArmID", (object)int.Parse(strArray2[0]));
                SqlParameters[9] = new SqlParameter("@GHairID", (object)int.Parse(strArray2[1]));
                SqlParameters[10] = new SqlParameter("@GFaceID", (object)int.Parse(strArray2[2]));
                SqlParameters[11] = new SqlParameter("@GClothID", (object)int.Parse(strArray2[3]));
                SqlParameters[12] = new SqlParameter("@GHatID", (object)int.Parse(strArray2[4]));
                SqlParameters[13] = new SqlParameter("@ArmColor", (object)armColor);
                SqlParameters[14] = new SqlParameter("@HairColor", (object)hairColor);
                SqlParameters[15] = new SqlParameter("@FaceColor", (object)faceColor);
                SqlParameters[16] = new SqlParameter("@ClothColor", (object)clothColor);
                SqlParameters[17] = new SqlParameter("@HatColor", (object)clothColor);
                SqlParameters[18] = new SqlParameter("@Sex", (object)sex);
                SqlParameters[19] = new SqlParameter("@StyleDate", (object)validDate);
                SqlParameters[26] = new SqlParameter("@CategoryID", (object)cateogryId);
                SqlParameters[27] = new SqlParameter("@Attack", (object)attack);
                SqlParameters[28] = new SqlParameter("@Defence", (object)defence);
                SqlParameters[29] = new SqlParameter("@Agility", (object)agility);
                SqlParameters[30] = new SqlParameter("@Luck", (object)luck);
                SqlParameters[20] = new SqlParameter("@Result", SqlDbType.Int);
                SqlParameters[20].Direction = ParameterDirection.ReturnValue;
                flag = this.db.RunProcedure("SP_Users_RegisterNotValidate2", SqlParameters);
                int num = (int)SqlParameters[20].Value;
                flag = num == 0;
                switch (num)
                {
                    case 2:
                        msg = LanguageMgr.GetTranslation("PlayerBussiness.RegisterPlayer.Msg2");
                        break;
                    case 3:
                        msg = LanguageMgr.GetTranslation("PlayerBussiness.RegisterPlayer.Msg3");
                        break;
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                {
                    BaseBussiness.log.Error((object)string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}", (object)userName, (object)passWord, (object)nickName, (object)attack, (object)defence, (object)agility, (object)luck, (object)cateogryId, (object)bStyle, (object)bPic, (object)gStyle));
                    BaseBussiness.log.Error((object)"Init", ex);
                }
            }
            return flag;
        }

        public bool UpdateUserReputeFightPower()
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[0].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Update_Repute_FightPower", SqlParameters);
                flag = (int)SqlParameters[0].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public UsersExtraInfo[] GetRankCaddy()
        {
            List<UsersExtraInfo> usersExtraInfoList = new List<UsersExtraInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Get_Rank_Caddy");
                while (ResultDataReader.Read())
                    usersExtraInfoList.Add(new UsersExtraInfo()
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        NickName = (string)ResultDataReader["NickName"],
                        TotalCaddyOpen = (int)ResultDataReader["badLuckNumber"]
                    });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_Get_Rank_Caddy", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return usersExtraInfoList.ToArray();
        }

        public List<UserGemStone> GetSingleGemstones(int ID)
        {
            List<UserGemStone> userGemStones = new List<UserGemStone>();
            SqlDataReader sqlDataReader = null;
            try
            {
                try
                {
                    SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@ID", SqlDbType.Int, 4) };
                    sqlParameter[0].Value = ID;
                    this.db.GetReader(ref sqlDataReader, "SP_GetSingleGemStone", sqlParameter);
                    while (sqlDataReader.Read())
                    {
                        userGemStones.Add(this.InitGemStones(sqlDataReader));
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    if (BaseBussiness.log.IsErrorEnabled)
                    {
                        BaseBussiness.log.Error("SP_GetSingleUserGemStones", exception);
                    }
                }
            }
            finally
            {
                if (sqlDataReader != null && !sqlDataReader.IsClosed)
                {
                    sqlDataReader.Close();
                }
            }
            return userGemStones;
        }

        public List<UserGemStone> GetSingleGemStones(int ID)
        {
            List<UserGemStone> userGemStones = new List<UserGemStone>();
            SqlDataReader sqlDataReader = null;
            try
            {
                try
                {
                    SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@ID", SqlDbType.Int, 4) };
                    sqlParameter[0].Value = ID;
                    this.db.GetReader(ref sqlDataReader, "SP_GetSingleGemStone", sqlParameter);
                    while (sqlDataReader.Read())
                    {
                        userGemStones.Add(this.InitGemStones(sqlDataReader));
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    if (BaseBussiness.log.IsErrorEnabled)
                    {
                        BaseBussiness.log.Error("SP_GetSingleUserGemStones", exception);
                    }
                }
            }
            finally
            {
                if (sqlDataReader != null && !sqlDataReader.IsClosed)
                {
                    sqlDataReader.Close();
                }
            }
            return userGemStones;
        }

        public UserLabyrinthInfo GetSingleLabyrinth(int ID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@ID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)ID;
                this.db.GetReader(ref ResultDataReader, "SP_GetSingleLabyrinth", SqlParameters);
                if (ResultDataReader.Read())
                    return new UserLabyrinthInfo()
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
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_GetSingleUserLabyrinth", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return (UserLabyrinthInfo)null;
        }

        public bool AddUserLabyrinth(UserLabyrinthInfo laby)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[18]
                {
          new SqlParameter("@UserID", (object) laby.UserID),
          new SqlParameter("@myProgress", (object) laby.myProgress),
          new SqlParameter("@myRanking", (object) laby.myRanking),
          new SqlParameter("@completeChallenge", (object) laby.completeChallenge),
          new SqlParameter("@isDoubleAward", (object) laby.isDoubleAward),
          new SqlParameter("@currentFloor", (object) laby.currentFloor),
          new SqlParameter("@accumulateExp", (object) laby.accumulateExp),
          new SqlParameter("@remainTime", (object) laby.remainTime),
          new SqlParameter("@currentRemainTime", (object) laby.currentRemainTime),
          new SqlParameter("@cleanOutAllTime", (object) laby.cleanOutAllTime),
          new SqlParameter("@cleanOutGold", (object) laby.cleanOutGold),
          new SqlParameter("@tryAgainComplete", (object) laby.tryAgainComplete),
          new SqlParameter("@isInGame", (object) laby.isInGame),
          new SqlParameter("@isCleanOut", (object) laby.isCleanOut),
          new SqlParameter("@serverMultiplyingPower", (object) laby.serverMultiplyingPower),
          new SqlParameter("@LastDate", (object) laby.LastDate),
          new SqlParameter("@ProcessAward", (object) laby.ProcessAward),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[17].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Users_Labyrinth_Add", SqlParameters);
                flag = (int)SqlParameters[17].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public bool UpdateLabyrinthInfo(UserLabyrinthInfo laby)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[18]
                {
          new SqlParameter("@UserID", (object) laby.UserID),
          new SqlParameter("@myProgress", (object) laby.myProgress),
          new SqlParameter("@myRanking", (object) laby.myRanking),
          new SqlParameter("@completeChallenge", (object) laby.completeChallenge),
          new SqlParameter("@isDoubleAward", (object) laby.isDoubleAward),
          new SqlParameter("@currentFloor", (object) laby.currentFloor),
          new SqlParameter("@accumulateExp", (object) laby.accumulateExp),
          new SqlParameter("@remainTime", (object) laby.remainTime),
          new SqlParameter("@currentRemainTime", (object) laby.currentRemainTime),
          new SqlParameter("@cleanOutAllTime", (object) laby.cleanOutAllTime),
          new SqlParameter("@cleanOutGold", (object) laby.cleanOutGold),
          new SqlParameter("@tryAgainComplete", (object) laby.tryAgainComplete),
          new SqlParameter("@isInGame", (object) laby.isInGame),
          new SqlParameter("@isCleanOut", (object) laby.isCleanOut),
          new SqlParameter("@serverMultiplyingPower", (object) laby.serverMultiplyingPower),
          new SqlParameter("@LastDate", (object) laby.LastDate),
          new SqlParameter("@ProcessAward", (object) laby.ProcessAward),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[17].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_UpdateLabyrinthInfo", SqlParameters);
                flag = true;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_UpdateLabyrinthInfo", ex);
            }
            return flag;
        }

        public UserGiftInfo[] GetAllUserGifts(int userid, bool isReceive)
        {
            List<UserGiftInfo> userGiftInfoList = new List<UserGiftInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[2]
                {
          new SqlParameter("@UserID", (object) userid),
          new SqlParameter("@IsReceive", (object) isReceive)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Users_Gift_Single", SqlParameters);
                while (ResultDataReader.Read())
                    userGiftInfoList.Add(new UserGiftInfo()
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
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(GetAllUserGifts), ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return userGiftInfoList.ToArray();
        }

        public UserGiftInfo[] GetAllUserReceivedGifts(int userid)
        {
            Dictionary<int, UserGiftInfo> dictionary = new Dictionary<int, UserGiftInfo>();
            SqlDataReader sqlDataReader = (SqlDataReader)null;
            try
            {
                UserGiftInfo[] allUserGifts = this.GetAllUserGifts(userid, true);
                if (allUserGifts != null)
                {
                    foreach (UserGiftInfo userGiftInfo in allUserGifts)
                    {
                        if (dictionary.ContainsKey(userGiftInfo.TemplateID))
                            dictionary[userGiftInfo.TemplateID].Count += userGiftInfo.Count;
                        else
                            dictionary.Add(userGiftInfo.TemplateID, userGiftInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(GetAllUserReceivedGifts), ex);
            }
            finally
            {
                if ((sqlDataReader == null ? 0U : (!sqlDataReader.IsClosed ? 1U : 0U)) > 0U)
                    sqlDataReader.Close();
            }
            return dictionary.Values.ToArray<UserGiftInfo>();
        }

        public bool AddUserGift(UserGiftInfo info)
        {
            bool flag = false;
            try
            {
                this.db.RunProcedure("SP_Users_Gift_Add", new SqlParameter[4]
                {
          new SqlParameter("@SenderID", (object) info.SenderID),
          new SqlParameter("@ReceiverID", (object) info.ReceiverID),
          new SqlParameter("@TemplateID", (object) info.TemplateID),
          new SqlParameter("@Count", (object) info.Count)
                });
                flag = true;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)nameof(AddUserGift), ex);
            }
            return flag;
        }

        public bool UpdateUserCharmGP(int userId, int int_1)
        {
            bool flag = false;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[3]
                {
          new SqlParameter("@UserID", (object) userId),
          new SqlParameter("@CharmGP", (object) int_1),
          new SqlParameter("@Result", SqlDbType.Int)
                };
                SqlParameters[2].Direction = ParameterDirection.ReturnValue;
                this.db.RunProcedure("SP_Users_UpdateCharmGP", SqlParameters);
                flag = (int)SqlParameters[2].Value == 0;
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"AddUserGift", ex);
            }
            return flag;
        }

        public bool ResetEliteGame(int point)
        {
            bool flag = false;
            try
            {
                return this.db.RunProcedure("SP_EliteGame_Reset", new SqlParameter[1]
                {
          new SqlParameter("@EliteScore", (object) point)
                });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"Init", ex);
            }
            return flag;
        }

        public PlayerEliteGameInfo[] GetEliteScorePlayers()
        {
            List<PlayerEliteGameInfo> playerEliteGameInfoList = new List<PlayerEliteGameInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Get_EliteScorePlayers");
                while (ResultDataReader.Read())
                    playerEliteGameInfoList.Add(new PlayerEliteGameInfo()
                    {
                        UserID = (int)ResultDataReader["UserID"],
                        NickName = (string)ResultDataReader["NickName"],
                        GameType = (int)ResultDataReader["Grade"] >= 41 ? 2 : 1,
                        CurrentPoint = (int)ResultDataReader["EliteScore"],
                        Rank = 0,
                        ReadyStatus = false,
                        Status = 0,
                        Winer = 0
                    });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"SP_Get_EliteScorePlayers", ex);
            }
            finally
            {
                if ((ResultDataReader == null ? 0U : (!ResultDataReader.IsClosed ? 1U : 0U)) > 0U)
                    ResultDataReader.Close();
            }
            return playerEliteGameInfoList.ToArray();
        }

        public TangCapVip[] GetAllTangCap()
        {
            List<TangCapVip> tangCapVipList = new List<TangCapVip>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_TangCapVip_All");
                while (ResultDataReader.Read())
                    tangCapVipList.Add(new TangCapVip()
                    {
                        ID = (int)ResultDataReader["ID"],
                        Exp = (int)ResultDataReader["Exp"],
                        ExpPM = (int)ResultDataReader["ExpPM"],
                        MaCong = (int)ResultDataReader["MaCong"],
                        MaKhang = (int)ResultDataReader["MaKhang"],
                        Mau = (int)ResultDataReader["Mau"],
                        MaCongPM = (int)ResultDataReader["MaCongPM"],
                        MaKhangPM = (int)ResultDataReader["MaKhangPM"],
                        MauPM = (int)ResultDataReader["MauPM"]
                    });
            }
            catch (Exception ex)
            {
                if (BaseBussiness.log.IsErrorEnabled)
                    BaseBussiness.log.Error((object)"TangCapVipProperty", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return tangCapVipList.ToArray();
        }
    }
}
