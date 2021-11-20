// Decompiled with JetBrains decompiler
// Type: Bussiness.ProduceBussiness
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Bussiness
{
    public class ProduceBussiness : BaseCrossBussiness
    {
        public ActiveAwardInfo[] GetAllActiveAwardInfo()
        {
            List<ActiveAwardInfo> activeAwardInfoList = new List<ActiveAwardInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Active_Award");
                while (ResultDataReader.Read())
                {
                    ActiveAwardInfo activeAwardInfo = new ActiveAwardInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        ActiveID = (int)ResultDataReader["ActiveID"],
                        AgilityCompose = (int)ResultDataReader["AgilityCompose"],
                        AttackCompose = (int)ResultDataReader["AttackCompose"],
                        Count = (int)ResultDataReader["Count"],
                        DefendCompose = (int)ResultDataReader["DefendCompose"],
                        Gold = (int)ResultDataReader["Gold"],
                        ItemID = (int)ResultDataReader["ItemID"],
                        LuckCompose = (int)ResultDataReader["LuckCompose"],
                        Mark = (int)ResultDataReader["Mark"],
                        Money = (int)ResultDataReader["Money"],
                        Sex = (int)ResultDataReader["Sex"],
                        StrengthenLevel = (int)ResultDataReader["StrengthenLevel"],
                        ValidDate = (int)ResultDataReader["ValidDate"],
                        GiftToken = (int)ResultDataReader["GiftToken"]
                    };
                    activeAwardInfoList.Add(activeAwardInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllActiveAwardInfo), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return activeAwardInfoList.ToArray();
        }

        public ActiveConditionInfo[] GetAllActiveConditionInfo()
        {
            List<ActiveConditionInfo> activeConditionInfoList = new List<ActiveConditionInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Active_Condition");
                while (ResultDataReader.Read())
                {
                    ActiveConditionInfo activeConditionInfo = new ActiveConditionInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        ActiveID = (int)ResultDataReader["ActiveID"],
                        Conditiontype = (int)ResultDataReader["Conditiontype"],
                        Condition = (int)ResultDataReader["Condition"],
                        LimitGrade = ResultDataReader["LimitGrade"].ToString() == null ? "" : ResultDataReader["LimitGrade"].ToString(),
                        AwardId = ResultDataReader["AwardId"].ToString() == null ? "" : ResultDataReader["AwardId"].ToString(),
                        IsMult = (bool)ResultDataReader["IsMult"],
                        StartTime = (DateTime)ResultDataReader["StartTime"],
                        EndTime = (DateTime)ResultDataReader["EndTime"]
                    };
                    activeConditionInfoList.Add(activeConditionInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllActiveConditionInfo), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return activeConditionInfoList.ToArray();
        }

        public AchievementInfo[] GetAllAchievement()
        {
            List<AchievementInfo> achievementInfoList = new List<AchievementInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Get_Achievement");
                while (ResultDataReader.Read())
                {
                    AchievementInfo achievementInfo = new AchievementInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        PlaceID = (int)ResultDataReader["PlaceID"],
                        Title = (string)ResultDataReader["Title"],
                        Detail = (string)ResultDataReader["Detail"],
                        NeedMinLevel = (int)ResultDataReader["NeedMinLevel"],
                        NeedMaxLevel = (int)ResultDataReader["NeedMaxLevel"],
                        PreAchievementID = (string)ResultDataReader["PreAchievementID"],
                        IsOther = (int)ResultDataReader["IsOther"],
                        AchievementType = (int)ResultDataReader["AchievementType"],
                        CanHide = (bool)ResultDataReader["CanHide"],
                        StartDate = (DateTime)ResultDataReader["StartDate"],
                        EndDate = (DateTime)ResultDataReader["EndDate"],
                        AchievementPoint = (int)ResultDataReader["AchievementPoint"],
                        IsActive = (int)ResultDataReader["IsActive"],
                        PicID = (int)ResultDataReader["PicID"],
                        IsShare = (bool)ResultDataReader["IsShare"]
                    };
                    achievementInfoList.Add(achievementInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return achievementInfoList.ToArray();
        }

        public AchievementInfo[] GetALlAchievement()
        {
            List<AchievementInfo> achievementInfoList = new List<AchievementInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Achievement_All");
                while (ResultDataReader.Read())
                    achievementInfoList.Add(this.InitAchievement(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"GetALlAchievement:", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return achievementInfoList.ToArray();
        }

        public AchievementCondictionInfo[] GetAllAchievementCondiction()
        {
            List<AchievementCondictionInfo> achievementCondictionInfoList = new List<AchievementCondictionInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Get_AchievementCondiction");
                while (ResultDataReader.Read())
                {
                    AchievementCondictionInfo achievementCondictionInfo = new AchievementCondictionInfo()
                    {
                        AchievementID = (int)ResultDataReader["AchievementID"],
                        CondictionID = (int)ResultDataReader["CondictionID"],
                        CondictionType = (int)ResultDataReader["CondictionType"],
                        Condiction_Para1 = (int)ResultDataReader["Condiction_Para1"],
                        Condiction_Para2 = (int)ResultDataReader["Condiction_Para2"]
                    };
                    achievementCondictionInfoList.Add(achievementCondictionInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return achievementCondictionInfoList.ToArray();
        }

        public AchievementConditionInfo[] GetALlAchievementCondition()
        {
            List<AchievementConditionInfo> achievementConditionInfoList = new List<AchievementConditionInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Achievement_Condition_All");
                while (ResultDataReader.Read())
                    achievementConditionInfoList.Add(this.InitAchievementCondition(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"GetALlAchievementCondition:", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return achievementConditionInfoList.ToArray();
        }

        public AchievementGoodsInfo[] GetAllAchievementGoods()
        {
            List<AchievementGoodsInfo> achievementGoodsInfoList = new List<AchievementGoodsInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Get_AchievementGoods");
                while (ResultDataReader.Read())
                {
                    AchievementGoodsInfo achievementGoodsInfo = new AchievementGoodsInfo()
                    {
                        AchievementID = (int)ResultDataReader["AchievementID"],
                        RewardType = (int)ResultDataReader["RewardType"],
                        RewardPara = (string)ResultDataReader["RewardPara"],
                        RewardValueId = (int)ResultDataReader["RewardValueId"],
                        RewardCount = (int)ResultDataReader["RewardCount"]
                    };
                    achievementGoodsInfoList.Add(achievementGoodsInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return achievementGoodsInfoList.ToArray();
        }

        public AchievementRewardInfo[] GetALlAchievementReward()
        {
            List<AchievementRewardInfo> achievementRewardInfoList = new List<AchievementRewardInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Achievement_Reward_All");
                while (ResultDataReader.Read())
                    achievementRewardInfoList.Add(this.InitAchievementReward(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetALlAchievementReward), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return achievementRewardInfoList.ToArray();
        }

        

        public BallInfo[] GetAllBall()
        {
            List<BallInfo> ballInfoList = new List<BallInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Ball_All");
                while (ResultDataReader.Read())
                {
                    BallInfo ballInfo = new BallInfo()
                    {
                        Amount = (int)ResultDataReader["Amount"],
                        ID = (int)ResultDataReader["ID"],
                        Name = ResultDataReader["Name"].ToString(),
                        Crater = ResultDataReader["Crater"] == null ? "" : ResultDataReader["Crater"].ToString(),
                        Power = (double)ResultDataReader["Power"],
                        Radii = (int)ResultDataReader["Radii"],
                        AttackResponse = (int)ResultDataReader["AttackResponse"],
                        BombPartical = ResultDataReader["BombPartical"].ToString(),
                        FlyingPartical = ResultDataReader["FlyingPartical"].ToString(),
                        IsSpin = (bool)ResultDataReader["IsSpin"],
                        Mass = (int)ResultDataReader["Mass"],
                        SpinV = (int)ResultDataReader["SpinV"],
                        SpinVA = (double)ResultDataReader["SpinVA"],
                        Wind = (int)ResultDataReader["Wind"],
                        DragIndex = (int)ResultDataReader["DragIndex"],
                        Weight = (int)ResultDataReader["Weight"],
                        Shake = (bool)ResultDataReader["Shake"],
                        Delay = (int)ResultDataReader["Delay"],
                        ShootSound = ResultDataReader["ShootSound"] == null ? "" : ResultDataReader["ShootSound"].ToString(),
                        BombSound = ResultDataReader["BombSound"] == null ? "" : ResultDataReader["BombSound"].ToString(),
                        ActionType = (int)ResultDataReader["ActionType"],
                        HasTunnel = (bool)ResultDataReader["HasTunnel"]
                    };
                    ballInfoList.Add(ballInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return ballInfoList.ToArray();
        }

        public ItemTemplateInfo[] GetSingleName(string ItemName)
        {
            List<ItemTemplateInfo> itemTemplateInfoList = new List<ItemTemplateInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@name", SqlDbType.NVarChar, 100)
                };
                SqlParameters[0].Value = (object)ItemName;
                this.db.GetReader(ref ResultDataReader, "SP_Items_Name_Single", SqlParameters);
                while (ResultDataReader.Read())
                    itemTemplateInfoList.Add(this.InitItemTemplateInfo(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return itemTemplateInfoList.ToArray();
        }

        public BallConfigInfo[] GetAllBallConfig()
        {
            List<BallConfigInfo> ballConfigInfoList = new List<BallConfigInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "[SP_Ball_Config_All]");
                while (ResultDataReader.Read())
                {
                    BallConfigInfo ballConfigInfo = new BallConfigInfo()
                    {
                        Common = (int)ResultDataReader["Common"],
                        TemplateID = (int)ResultDataReader["TemplateID"],
                        CommonAddWound = (int)ResultDataReader["CommonAddWound"],
                        CommonMultiBall = (int)ResultDataReader["CommonMultiBall"],
                        Special = (int)ResultDataReader["Special"]
                    };
                    ballConfigInfoList.Add(ballConfigInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return ballConfigInfoList.ToArray();
        }

        public CategoryInfo[] GetAllCategory()
        {
            List<CategoryInfo> categoryInfoList = new List<CategoryInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Items_Category_All");
                while (ResultDataReader.Read())
                {
                    CategoryInfo categoryInfo = new CategoryInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        Name = ResultDataReader["Name"] == null ? "" : ResultDataReader["Name"].ToString(),
                        Place = (int)ResultDataReader["Place"],
                        Remark = ResultDataReader["Remark"] == null ? "" : ResultDataReader["Remark"].ToString()
                    };
                    categoryInfoList.Add(categoryInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return categoryInfoList.ToArray();
        }

        public DailyAwardInfo[] GetAllDailyAward()
        {
            List<DailyAwardInfo> dailyAwardInfoList = new List<DailyAwardInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Daily_Award_All");
                while (ResultDataReader.Read())
                {
                    DailyAwardInfo dailyAwardInfo = new DailyAwardInfo()
                    {
                        Count = (int)ResultDataReader["Count"],
                        ID = (int)ResultDataReader["ID"],
                        IsBinds = (bool)ResultDataReader["IsBinds"],
                        TemplateID = (int)ResultDataReader["TemplateID"],
                        Type = (int)ResultDataReader["Type"],
                        ValidDate = (int)ResultDataReader["ValidDate"],
                        Sex = (int)ResultDataReader["Sex"],
                        Remark = ResultDataReader["Remark"] == null ? "" : ResultDataReader["Remark"].ToString(),
                        CountRemark = ResultDataReader["CountRemark"] == null ? "" : ResultDataReader["CountRemark"].ToString(),
                        GetWay = (int)ResultDataReader["GetWay"],
                        AwardDays = (int)ResultDataReader["AwardDays"]
                    };
                    dailyAwardInfoList.Add(dailyAwardInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"GetAllDaily", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return dailyAwardInfoList.ToArray();
        }

        public DailyAwardInfo[] GetSingleDailyAward(int awardDays)
        {
            List<DailyAwardInfo> dailyAwardInfoList = new List<DailyAwardInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@awardDays", (object) awardDays)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Daily_Award_Single", SqlParameters);
                while (ResultDataReader.Read())
                {
                    DailyAwardInfo dailyAwardInfo = new DailyAwardInfo()
                    {
                        Count = (int)ResultDataReader["Count"],
                        ID = (int)ResultDataReader["ID"],
                        IsBinds = (bool)ResultDataReader["IsBinds"],
                        TemplateID = (int)ResultDataReader["TemplateID"],
                        Type = (int)ResultDataReader["Type"],
                        ValidDate = (int)ResultDataReader["ValidDate"],
                        Sex = (int)ResultDataReader["Sex"],
                        Remark = ResultDataReader["Remark"] == null ? "" : ResultDataReader["Remark"].ToString(),
                        CountRemark = ResultDataReader["CountRemark"] == null ? "" : ResultDataReader["CountRemark"].ToString(),
                        GetWay = (int)ResultDataReader["GetWay"],
                        AwardDays = (int)ResultDataReader["AwardDays"]
                    };
                    dailyAwardInfoList.Add(dailyAwardInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"GetSingleDaily", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return dailyAwardInfoList.ToArray();
        }

        public DropCondiction[] GetAllDropCondictions()
        {
            List<DropCondiction> dropCondictionList = new List<DropCondiction>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Drop_Condiction_All");
                while (ResultDataReader.Read())
                    dropCondictionList.Add(this.InitDropCondiction(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return dropCondictionList.ToArray();
        }

        public DropItem[] GetAllDropItems()
        {
            List<DropItem> dropItemList = new List<DropItem>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Drop_Item_All");
                while (ResultDataReader.Read())
                    dropItemList.Add(this.InitDropItem(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return dropItemList.ToArray();
        }

        public EdictumInfo[] GetAllEdictum()
        {
            List<EdictumInfo> edictumInfoList = new List<EdictumInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Edictum_All");
                while (ResultDataReader.Read())
                    edictumInfoList.Add(this.InitEdictum(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllEdictum), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return edictumInfoList.ToArray();
        }

        public EventRewardGoodsInfo[] GetAllEventRewardGoods()
        {
            List<EventRewardGoodsInfo> eventRewardGoodsInfoList = new List<EventRewardGoodsInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Get_EventRewardGoods");
                while (ResultDataReader.Read())
                {
                    EventRewardGoodsInfo eventRewardGoodsInfo = new EventRewardGoodsInfo()
                    {
                        ActivityType = (int)ResultDataReader["ActivityType"],
                        SubActivityType = (int)ResultDataReader["SubActivityType"],
                        TemplateId = (int)ResultDataReader["TemplateId"],
                        StrengthLevel = (int)ResultDataReader["StrengthLevel"],
                        AttackCompose = (int)ResultDataReader["AttackCompose"],
                        DefendCompose = (int)ResultDataReader["DefendCompose"],
                        LuckCompose = (int)ResultDataReader["LuckCompose"],
                        AgilityCompose = (int)ResultDataReader["AgilityCompose"],
                        IsBind = (bool)ResultDataReader["IsBind"],
                        ValidDate = (int)ResultDataReader["ValidDate"],
                        Count = (int)ResultDataReader["Count"]
                    };
                    eventRewardGoodsInfoList.Add(eventRewardGoodsInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllEventRewardGoods), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return eventRewardGoodsInfoList.ToArray();
        }

        public DailyLeagueAwardItems[] GetAllDailyLeagueAwardItems()
        {
            List<DailyLeagueAwardItems> leagueAwardItemsList = new List<DailyLeagueAwardItems>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Get_DailyLeagueAwardItems");
                while (ResultDataReader.Read())
                {
                    DailyLeagueAwardItems leagueAwardItems = new DailyLeagueAwardItems()
                    {
                        ID = (int)ResultDataReader["ID"],
                        Class = (int)ResultDataReader["Class"],
                        TemplateID = (int)ResultDataReader["TemplateID"],
                        StrengthLevel = (int)ResultDataReader["StrengthLevel"],
                        AttackCompose = (int)ResultDataReader["AttackCompose"],
                        DefendCompose = (int)ResultDataReader["DefendCompose"],
                        LuckCompose = (int)ResultDataReader["LuckCompose"],
                        AgilityCompose = (int)ResultDataReader["AgilityCompose"],
                        IsBind = (bool)ResultDataReader["IsBind"],
                        ValidDate = (int)ResultDataReader["ValidDate"],
                        Count = (int)ResultDataReader["Count"]
                    };
                    leagueAwardItemsList.Add(leagueAwardItems);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllDailyLeagueAwardItems), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return leagueAwardItemsList.ToArray();
        }

        public EventRewardGoodsInfo[] GetEventRewardGoodsByType(
          int ActivityType,
          int SubActivityType)
        {
            List<EventRewardGoodsInfo> eventRewardGoodsInfoList = new List<EventRewardGoodsInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[2]
                {
          new SqlParameter("@ActivityType", (object) ActivityType),
          new SqlParameter("@SubActivityType", (object) SubActivityType)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Get_EventRewardGoods_Type", SqlParameters);
                while (ResultDataReader.Read())
                {
                    EventRewardGoodsInfo eventRewardGoodsInfo = new EventRewardGoodsInfo()
                    {
                        ActivityType = (int)ResultDataReader[nameof(ActivityType)],
                        SubActivityType = (int)ResultDataReader[nameof(SubActivityType)],
                        TemplateId = (int)ResultDataReader["TemplateId"],
                        StrengthLevel = (int)ResultDataReader["StrengthLevel"],
                        AttackCompose = (int)ResultDataReader["AttackCompose"],
                        DefendCompose = (int)ResultDataReader["DefendCompose"],
                        LuckCompose = (int)ResultDataReader["LuckCompose"],
                        AgilityCompose = (int)ResultDataReader["AgilityCompose"],
                        IsBind = (bool)ResultDataReader["IsBind"],
                        ValidDate = (int)ResultDataReader["ValidDate"],
                        Count = (int)ResultDataReader["Count"]
                    };
                    eventRewardGoodsInfoList.Add(eventRewardGoodsInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetEventRewardGoodsByType), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return eventRewardGoodsInfoList.ToArray();
        }

        public EventRewardInfo[] GetAllEventRewardInfo()
        {
            List<EventRewardInfo> eventRewardInfoList = new List<EventRewardInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Get_EventRewardInfo");
                while (ResultDataReader.Read())
                {
                    EventRewardInfo eventRewardInfo = new EventRewardInfo()
                    {
                        ActivityType = (int)ResultDataReader["ActivityType"],
                        SubActivityType = (int)ResultDataReader["SubActivityType"],
                        Condition = (int)ResultDataReader["Condition"]
                    };
                    eventRewardInfoList.Add(eventRewardInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllEventRewardInfo), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return eventRewardInfoList.ToArray();
        }

        public DailyLeagueAwardList[] GetAllDailyLeagueAwardList()
        {
            List<DailyLeagueAwardList> dailyLeagueAwardListList = new List<DailyLeagueAwardList>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Get_DailyLeagueAwardList");
                while (ResultDataReader.Read())
                {
                    DailyLeagueAwardList dailyLeagueAwardList = new DailyLeagueAwardList()
                    {
                        Class = (int)ResultDataReader["Class"],
                        Grade = (int)ResultDataReader["Grade"],
                        Score = (int)ResultDataReader["Score"],
                        Rank = (int)ResultDataReader["Rank"]
                    };
                    dailyLeagueAwardListList.Add(dailyLeagueAwardList);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllDailyLeagueAwardList), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return dailyLeagueAwardListList.ToArray();
        }

        public EventRewardInfo[] GetEventRewardInfoByType(
          int ActivityType,
          int SubActivityType)
        {
            List<EventRewardInfo> eventRewardInfoList = new List<EventRewardInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[2]
                {
          new SqlParameter("@ActivityType", (object) ActivityType),
          new SqlParameter("@SubActivityType", (object) SubActivityType)
                };
                this.db.GetReader(ref ResultDataReader, "SP_Get_EventRewardInfo_Type", SqlParameters);
                while (ResultDataReader.Read())
                {
                    EventRewardInfo eventRewardInfo = new EventRewardInfo()
                    {
                        ActivityType = (int)ResultDataReader[nameof(ActivityType)],
                        SubActivityType = (int)ResultDataReader[nameof(SubActivityType)],
                        Condition = (int)ResultDataReader["Condition"]
                    };
                    eventRewardInfoList.Add(eventRewardInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetEventRewardInfoByType), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return eventRewardInfoList.ToArray();
        }

        public FusionInfo[] GetAllFusion()
        {
            List<FusionInfo> fusionInfoList = new List<FusionInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Fusion_All");
                while (ResultDataReader.Read())
                {
                    FusionInfo fusionInfo = new FusionInfo()
                    {
                        FusionID = (int)ResultDataReader["FusionID"],
                        Item1 = (int)ResultDataReader["Item1"],
                        Item2 = (int)ResultDataReader["Item2"],
                        Item3 = (int)ResultDataReader["Item3"],
                        Item4 = (int)ResultDataReader["Item4"],
                        Formula = (int)ResultDataReader["Formula"],
                        Reward = (int)ResultDataReader["Reward"]
                    };
                    fusionInfoList.Add(fusionInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllFusion), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return fusionInfoList.ToArray();
        }

        public FusionInfo[] GetAllFusionDesc()
        {
            List<FusionInfo> fusionInfoList = new List<FusionInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Fusion_All_Desc");
                while (ResultDataReader.Read())
                {
                    FusionInfo fusionInfo = new FusionInfo()
                    {
                        FusionID = (int)ResultDataReader["FusionID"],
                        Item1 = (int)ResultDataReader["Item1"],
                        Item2 = (int)ResultDataReader["Item2"],
                        Item3 = (int)ResultDataReader["Item3"],
                        Item4 = (int)ResultDataReader["Item4"],
                        Formula = (int)ResultDataReader["Formula"],
                        Reward = (int)ResultDataReader["Reward"]
                    };
                    fusionInfoList.Add(fusionInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"GetAllFusion", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return fusionInfoList.ToArray();
        }

        public Items_Fusion_List_Info[] GetAllFusionList()
        {
            List<Items_Fusion_List_Info> itemsFusionListInfoList = new List<Items_Fusion_List_Info>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "GET_ItemFusion_All");
                while (ResultDataReader.Read())
                {
                    Items_Fusion_List_Info itemsFusionListInfo = new Items_Fusion_List_Info()
                    {
                        ID = (int)ResultDataReader["ID"],
                        TemplateID = (int)ResultDataReader["TemplateID"],
                        Show = (int)ResultDataReader["Show"],
                        Real = (int)ResultDataReader["Real"]
                    };
                    itemsFusionListInfoList.Add(itemsFusionListInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"GET_ItemFusion_All", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return itemsFusionListInfoList.ToArray();
        }

        public ItemTemplateInfo[] GetAllGoods()
        {
            List<ItemTemplateInfo> itemTemplateInfoList = new List<ItemTemplateInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Items_All");
                while (ResultDataReader.Read())
                    itemTemplateInfoList.Add(this.InitItemTemplateInfo(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return itemTemplateInfoList.ToArray();
        }

        public ItemTemplateInfo[] GetAllGoodsASC()
        {
            List<ItemTemplateInfo> itemTemplateInfoList = new List<ItemTemplateInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Items_All_ASC");
                while (ResultDataReader.Read())
                    itemTemplateInfoList.Add(this.InitItemTemplateInfo(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return itemTemplateInfoList.ToArray();
        }

        public HotSpringRoomInfo[] GetAllHotSpringRooms()
        {
            List<HotSpringRoomInfo> hotSpringRoomInfoList = new List<HotSpringRoomInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Get_HotSpring_Room");
                while (ResultDataReader.Read())
                {
                    HotSpringRoomInfo hotSpringRoomInfo = new HotSpringRoomInfo()
                    {
                        roomID = (int)ResultDataReader["roomID"],
                        roomNumber = (int)ResultDataReader["roomNumber"],
                        roomName = ResultDataReader["roomName"].ToString(),
                        roomPassword = ResultDataReader["roomPassword"] == DBNull.Value ? (string)null : (string)ResultDataReader["roomPassword"],
                        effectiveTime = (int)ResultDataReader["effectiveTime"],
                        curCount = (int)ResultDataReader["curCount"],
                        playerID = (int)ResultDataReader["playerID"],
                        playerName = (string)ResultDataReader["playerName"],
                        startTime = ResultDataReader["startTime"] == DBNull.Value ? DateTime.Now : (DateTime)ResultDataReader["startTime"],
                        endTime = ResultDataReader["endTime"] == DBNull.Value ? DateTime.Now.AddYears(1) : (DateTime)ResultDataReader["endTime"],
                        roomIntroduction = ResultDataReader["roomIntroduction"] == DBNull.Value ? "" : (string)ResultDataReader["roomIntroduction"],
                        roomType = (int)ResultDataReader["roomType"],
                        maxCount = (int)ResultDataReader["maxCount"]
                    };
                    hotSpringRoomInfoList.Add(hotSpringRoomInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllHotSpringRooms), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return hotSpringRoomInfoList.ToArray();
        }

        public ItemRecordTypeInfo[] GetAllItemRecordType()
        {
            List<ItemRecordTypeInfo> itemRecordTypeInfoList = new List<ItemRecordTypeInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Item_Record_Type_All");
                while (ResultDataReader.Read())
                    itemRecordTypeInfoList.Add(this.InitItemRecordType(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"GetAllItemRecordType:", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return itemRecordTypeInfoList.ToArray();
        }

        public ShopItemInfo[] GetALllShop()
        {
            List<ShopItemInfo> shopItemInfoList = new List<ShopItemInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Shop_All");
                while (ResultDataReader.Read())
                {
                    ShopItemInfo shopItemInfo = new ShopItemInfo()
                    {
                        ID = int.Parse(ResultDataReader["ID"].ToString()),
                        ShopID = int.Parse(ResultDataReader["ShopID"].ToString()),
                        GroupID = int.Parse(ResultDataReader["GroupID"].ToString()),
                        TemplateID = int.Parse(ResultDataReader["TemplateID"].ToString()),
                        BuyType = int.Parse(ResultDataReader["BuyType"].ToString()),
                        IsBind = int.Parse(ResultDataReader["IsBind"].ToString()),
                        Sort = 0,
                        IsVouch = int.Parse(ResultDataReader["IsVouch"].ToString()),
                        Label = (float)int.Parse(ResultDataReader["Label"].ToString()),
                        Beat = Decimal.Parse(ResultDataReader["Beat"].ToString()),
                        AUnit = int.Parse(ResultDataReader["AUnit"].ToString()),
                        APrice1 = int.Parse(ResultDataReader["APrice1"].ToString()),
                        AValue1 = int.Parse(ResultDataReader["AValue1"].ToString()),
                        APrice2 = int.Parse(ResultDataReader["APrice2"].ToString()),
                        AValue2 = int.Parse(ResultDataReader["AValue2"].ToString()),
                        APrice3 = int.Parse(ResultDataReader["APrice3"].ToString()),
                        AValue3 = int.Parse(ResultDataReader["AValue3"].ToString()),
                        BUnit = int.Parse(ResultDataReader["BUnit"].ToString()),
                        BPrice1 = int.Parse(ResultDataReader["BPrice1"].ToString()),
                        BValue1 = int.Parse(ResultDataReader["BValue1"].ToString()),
                        BPrice2 = int.Parse(ResultDataReader["BPrice2"].ToString()),
                        BValue2 = int.Parse(ResultDataReader["BValue2"].ToString()),
                        BPrice3 = int.Parse(ResultDataReader["BPrice3"].ToString()),
                        BValue3 = int.Parse(ResultDataReader["BValue3"].ToString()),
                        CUnit = int.Parse(ResultDataReader["CUnit"].ToString()),
                        CPrice1 = int.Parse(ResultDataReader["CPrice1"].ToString()),
                        CValue1 = int.Parse(ResultDataReader["CValue1"].ToString()),
                        CPrice2 = int.Parse(ResultDataReader["CPrice2"].ToString()),
                        CValue2 = int.Parse(ResultDataReader["CValue2"].ToString()),
                        CPrice3 = int.Parse(ResultDataReader["CPrice3"].ToString()),
                        CValue3 = int.Parse(ResultDataReader["CValue3"].ToString()),
                        IsContinue = bool.Parse(ResultDataReader["IsContinue"].ToString()),
                        IsCheap = bool.Parse(ResultDataReader["IsCheap"].ToString()),
                        LimitCount = int.Parse(ResultDataReader["LimitCount"].ToString()),
                        StartDate = DateTime.Parse(ResultDataReader["StartDate"].ToString()),
                        EndDate = DateTime.Parse(ResultDataReader["EndDate"].ToString())
                    };
                    shopItemInfoList.Add(shopItemInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return shopItemInfoList.ToArray();
        }

        public MissionInfo[] GetAllMissionInfo()
        {
            List<MissionInfo> missionInfoList = new List<MissionInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Mission_Info_All");
                while (ResultDataReader.Read())
                {
                    MissionInfo missionInfo = new MissionInfo()
                    {
                        Id = (int)ResultDataReader["ID"],
                        Name = ResultDataReader["Name"] == null ? "" : ResultDataReader["Name"].ToString(),
                        TotalCount = (int)ResultDataReader["TotalCount"],
                        TotalTurn = (int)ResultDataReader["TotalTurn"],
                        Script = ResultDataReader["Script"] == null ? "" : ResultDataReader["Script"].ToString(),
                        Success = ResultDataReader["Success"] == null ? "" : ResultDataReader["Success"].ToString(),
                        Failure = ResultDataReader["Failure"] == null ? "" : ResultDataReader["Failure"].ToString(),
                        Description = ResultDataReader["Description"] == null ? "" : ResultDataReader["Description"].ToString(),
                        IncrementDelay = (int)ResultDataReader["IncrementDelay"],
                        Delay = (int)ResultDataReader["Delay"],
                        Title = ResultDataReader["Title"] == null ? "" : ResultDataReader["Title"].ToString(),
                        Param1 = (int)ResultDataReader["Param1"],
                        Param2 = (int)ResultDataReader["Param2"]
                    };
                    missionInfoList.Add(missionInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllMissionInfo), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return missionInfoList.ToArray();
        }

        public NpcInfo[] GetAllNPCInfo()
        {
            List<NpcInfo> npcInfoList = new List<NpcInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_NPC_Info_All");
                while (ResultDataReader.Read())
                {
                    NpcInfo npcInfo = new NpcInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        Name = ResultDataReader["Name"] == null ? "" : ResultDataReader["Name"].ToString(),
                        Level = (int)ResultDataReader["Level"],
                        Camp = (int)ResultDataReader["Camp"],
                        Type = (int)ResultDataReader["Type"],
                        Blood = (int)ResultDataReader["Blood"],
                        X = (int)ResultDataReader["X"],
                        Y = (int)ResultDataReader["Y"],
                        Width = (int)ResultDataReader["Width"],
                        Height = (int)ResultDataReader["Height"],
                        MoveMin = (int)ResultDataReader["MoveMin"],
                        MoveMax = (int)ResultDataReader["MoveMax"],
                        BaseDamage = (int)ResultDataReader["BaseDamage"],
                        BaseGuard = (int)ResultDataReader["BaseGuard"],
                        Attack = (int)ResultDataReader["Attack"],
                        Defence = (int)ResultDataReader["Defence"],
                        Agility = (int)ResultDataReader["Agility"],
                        Lucky = (int)ResultDataReader["Lucky"],
                        ModelID = ResultDataReader["ModelID"] == null ? "" : ResultDataReader["ModelID"].ToString(),
                        ResourcesPath = ResultDataReader["ResourcesPath"] == null ? "" : ResultDataReader["ResourcesPath"].ToString(),
                        DropRate = ResultDataReader["DropRate"] == null ? "" : ResultDataReader["DropRate"].ToString(),
                        Experience = (int)ResultDataReader["Experience"],
                        Delay = (int)ResultDataReader["Delay"],
                        Immunity = (int)ResultDataReader["Immunity"],
                        Alert = (int)ResultDataReader["Alert"],
                        Range = (int)ResultDataReader["Range"],
                        Preserve = (int)ResultDataReader["Preserve"],
                        Script = ResultDataReader["Script"] == null ? "" : ResultDataReader["Script"].ToString(),
                        FireX = (int)ResultDataReader["FireX"],
                        FireY = (int)ResultDataReader["FireY"],
                        DropId = (int)ResultDataReader["DropId"],
                        CurrentBallId = (int)ResultDataReader["CurrentBallId"],
                        speed = (int)ResultDataReader["speed"]
                    };
                    npcInfoList.Add(npcInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllNPCInfo), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return npcInfoList.ToArray();
        }

        public PropInfo[] GetAllProp()
        {
            List<PropInfo> propInfoList = new List<PropInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Prop_All");
                while (ResultDataReader.Read())
                {
                    PropInfo propInfo = new PropInfo()
                    {
                        AffectArea = (int)ResultDataReader["AffectArea"],
                        AffectTimes = (int)ResultDataReader["AffectTimes"],
                        AttackTimes = (int)ResultDataReader["AttackTimes"],
                        BoutTimes = (int)ResultDataReader["BoutTimes"],
                        BuyGold = (int)ResultDataReader["BuyGold"],
                        BuyMoney = (int)ResultDataReader["BuyMoney"],
                        Category = (int)ResultDataReader["Category"],
                        Delay = (int)ResultDataReader["Delay"],
                        Description = ResultDataReader["Description"].ToString(),
                        Icon = ResultDataReader["Icon"].ToString(),
                        ID = (int)ResultDataReader["ID"],
                        Name = ResultDataReader["Name"].ToString(),
                        Parameter = (int)ResultDataReader["Parameter"],
                        Pic = ResultDataReader["Pic"].ToString(),
                        Property1 = (int)ResultDataReader["Property1"],
                        Property2 = (int)ResultDataReader["Property2"],
                        Property3 = (int)ResultDataReader["Property3"],
                        Random = (int)ResultDataReader["Random"],
                        Script = ResultDataReader["Script"].ToString()
                    };
                    propInfoList.Add(propInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return propInfoList.ToArray();
        }

        public QQtipsMessagesInfo[] GetAllQQtipsMessagesLoad()
        {
            List<QQtipsMessagesInfo> qqtipsMessagesInfoList = new List<QQtipsMessagesInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_QQtipsMessages_All");
                while (ResultDataReader.Read())
                    qqtipsMessagesInfoList.Add(this.InitQQtipsMessagesLoad(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllQQtipsMessagesLoad), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return qqtipsMessagesInfoList.ToArray();
        }

        public QuestInfo[] GetALlQuest()
        {
            List<QuestInfo> questInfoList = new List<QuestInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Quest_All");
                while (ResultDataReader.Read())
                    questInfoList.Add(this.InitQuest(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return questInfoList.ToArray();
        }

        public EventLiveInfo[] GetAllEventLive()
        {
            List<EventLiveInfo> eventLiveInfoList = new List<EventLiveInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Event_Live_All");
                while (ResultDataReader.Read())
                    eventLiveInfoList.Add(this.InitEventLiveInfo(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return eventLiveInfoList.ToArray();
        }

        public QuestConditionInfo[] GetAllQuestCondiction()
        {
            List<QuestConditionInfo> questConditionInfoList = new List<QuestConditionInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Quest_Condiction_All");
                while (ResultDataReader.Read())
                    questConditionInfoList.Add(this.InitQuestCondiction(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return questConditionInfoList.ToArray();
        }

        public QuestRateInfo[] GetAllQuestRate()
        {
            List<QuestRateInfo> questRateInfoList = new List<QuestRateInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Quest_Rate_All");
                while (ResultDataReader.Read())
                    questRateInfoList.Add(this.InitQuestRate(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return questRateInfoList.ToArray();
        }

        public QuestAwardInfo[] GetAllQuestGoods()
        {
            List<QuestAwardInfo> questAwardInfoList = new List<QuestAwardInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Quest_Goods_All");
                while (ResultDataReader.Read())
                    questAwardInfoList.Add(this.InitQuestGoods(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return questAwardInfoList.ToArray();
        }

        public EventLiveGoods[] GetAllEventLiveGoods()
        {
            List<EventLiveGoods> eventLiveGoodsList = new List<EventLiveGoods>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Event_LiveGoods_All");
                while (ResultDataReader.Read())
                    eventLiveGoodsList.Add(this.InitEventLiveGoods(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return eventLiveGoodsList.ToArray();
        }

        public List<RefineryInfo> GetAllRefineryInfo()
        {
            List<RefineryInfo> refineryInfoList = new List<RefineryInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Item_Refinery_All");
                while (ResultDataReader.Read())
                {
                    RefineryInfo refineryInfo = new RefineryInfo()
                    {
                        RefineryID = (int)ResultDataReader["RefineryID"]
                    };
                    refineryInfo.m_Equip.Add((int)ResultDataReader["Equip1"]);
                    refineryInfo.m_Equip.Add((int)ResultDataReader["Equip2"]);
                    refineryInfo.m_Equip.Add((int)ResultDataReader["Equip3"]);
                    refineryInfo.m_Equip.Add((int)ResultDataReader["Equip4"]);
                    refineryInfo.Item1 = (int)ResultDataReader["Item1"];
                    refineryInfo.Item2 = (int)ResultDataReader["Item2"];
                    refineryInfo.Item3 = (int)ResultDataReader["Item3"];
                    refineryInfo.Item1Count = (int)ResultDataReader["Item1Count"];
                    refineryInfo.Item2Count = (int)ResultDataReader["Item2Count"];
                    refineryInfo.Item3Count = (int)ResultDataReader["Item3Count"];
                    refineryInfo.m_Reward.Add((int)ResultDataReader["Material1"]);
                    refineryInfo.m_Reward.Add((int)ResultDataReader["Operate1"]);
                    refineryInfo.m_Reward.Add((int)ResultDataReader["Reward1"]);
                    refineryInfo.m_Reward.Add((int)ResultDataReader["Material2"]);
                    refineryInfo.m_Reward.Add((int)ResultDataReader["Operate2"]);
                    refineryInfo.m_Reward.Add((int)ResultDataReader["Reward2"]);
                    refineryInfo.m_Reward.Add((int)ResultDataReader["Material3"]);
                    refineryInfo.m_Reward.Add((int)ResultDataReader["Operate3"]);
                    refineryInfo.m_Reward.Add((int)ResultDataReader["Reward3"]);
                    refineryInfo.m_Reward.Add((int)ResultDataReader["Material4"]);
                    refineryInfo.m_Reward.Add((int)ResultDataReader["Operate4"]);
                    refineryInfo.m_Reward.Add((int)ResultDataReader["Reward4"]);
                    refineryInfoList.Add(refineryInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllRefineryInfo), ex);
            }
            finally
            {
                if (ResultDataReader != null && ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return refineryInfoList;
        }

        public StrengthenInfo[] GetAllRefineryStrengthen()
        {
            List<StrengthenInfo> strengthenInfoList = new List<StrengthenInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Item_Refinery_Strengthen_All");
                while (ResultDataReader.Read())
                {
                    StrengthenInfo strengthenInfo = new StrengthenInfo()
                    {
                        StrengthenLevel = (int)ResultDataReader["StrengthenLevel"],
                        Rock = (int)ResultDataReader["Rock"]
                    };
                    strengthenInfoList.Add(strengthenInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllRefineryStrengthen), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return strengthenInfoList.ToArray();
        }

        public SearchGoodsTempInfo[] GetAllSearchGoodsTemp()
        {
            List<SearchGoodsTempInfo> searchGoodsTempInfoList = new List<SearchGoodsTempInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_SearchGoodsTemp_All");
                while (ResultDataReader.Read())
                {
                    SearchGoodsTempInfo searchGoodsTempInfo = new SearchGoodsTempInfo()
                    {
                        StarID = (int)ResultDataReader["StarID"],
                        NeedMoney = (int)ResultDataReader["NeedMoney"],
                        DestinationReward = (int)ResultDataReader["DestinationReward"],
                        VIPLevel = (int)ResultDataReader["VIPLevel"],
                        ExtractNumber = ResultDataReader["ExtractNumber"] == null ? "" : ResultDataReader["ExtractNumber"].ToString()
                    };
                    searchGoodsTempInfoList.Add(searchGoodsTempInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"GetAllDaily", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return searchGoodsTempInfoList.ToArray();
        }

        public ShopGoodsShowListInfo[] GetAllShopGoodsShowList()
        {
            List<ShopGoodsShowListInfo> goodsShowListInfoList = new List<ShopGoodsShowListInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_ShopGoodsShowList_All");
                while (ResultDataReader.Read())
                    goodsShowListInfoList.Add(this.InitShopGoodsShowListInfo(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return goodsShowListInfoList.ToArray();
        }

        public StrengthenInfo[] GetAllStrengthen()
        {
            List<StrengthenInfo> strengthenInfoList = new List<StrengthenInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Item_Strengthen_All");
                while (ResultDataReader.Read())
                {
                    StrengthenInfo strengthenInfo = new StrengthenInfo()
                    {
                        StrengthenLevel = (int)ResultDataReader["StrengthenLevel"],
                        Rock = (int)ResultDataReader["Rock"],
                        Rock1 = (int)ResultDataReader["Rock1"],
                        Rock2 = (int)ResultDataReader["Rock2"],
                        Rock3 = (int)ResultDataReader["Rock3"],
                        StoneLevelMin = (int)ResultDataReader["StoneLevelMin"]
                    };
                    strengthenInfoList.Add(strengthenInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllStrengthen), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return strengthenInfoList.ToArray();
        }

        public StrengThenExpInfo[] GetAllStrengThenExp()
        {
            List<StrengThenExpInfo> strengThenExpInfoList = new List<StrengThenExpInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_StrengThenExp_All");
                while (ResultDataReader.Read())
                {
                    StrengThenExpInfo strengThenExpInfo = new StrengThenExpInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        Level = (int)ResultDataReader["Level"],
                        Exp = (int)ResultDataReader["Exp"],
                        NecklaceStrengthExp = (int)ResultDataReader["NecklaceStrengthExp"],
                        NecklaceStrengthPlus = (int)ResultDataReader["NecklaceStrengthPlus"]
                    };
                    strengThenExpInfoList.Add(strengThenExpInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"GetStrengThenExpInfo", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return strengThenExpInfoList.ToArray();
        }

        public StrengthenGoodsInfo[] GetAllStrengthenGoodsInfo()
        {
            List<StrengthenGoodsInfo> strengthenGoodsInfoList = new List<StrengthenGoodsInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Item_StrengthenGoodsInfo_All");
                while (ResultDataReader.Read())
                {
                    StrengthenGoodsInfo strengthenGoodsInfo = new StrengthenGoodsInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        Level = (int)ResultDataReader["Level"],
                        CurrentEquip = (int)ResultDataReader["CurrentEquip"],
                        GainEquip = (int)ResultDataReader["GainEquip"],
                        OrginEquip = (int)ResultDataReader["OrginEquip"]
                    };
                    strengthenGoodsInfoList.Add(strengthenGoodsInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllStrengthenGoodsInfo), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return strengthenGoodsInfoList.ToArray();
        }

        public LoadUserBoxInfo[] GetAllTimeBoxAward()
        {
            List<LoadUserBoxInfo> loadUserBoxInfoList = new List<LoadUserBoxInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_TimeBox_Award_All");
                while (ResultDataReader.Read())
                {
                    LoadUserBoxInfo loadUserBoxInfo = new LoadUserBoxInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        Type = (int)ResultDataReader["Type"],
                        Level = (int)ResultDataReader["Level"],
                        Condition = (int)ResultDataReader["Condition"],
                        TemplateID = (int)ResultDataReader["TemplateID"]
                    };
                    loadUserBoxInfoList.Add(loadUserBoxInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"GetAllDaily", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return loadUserBoxInfoList.ToArray();
        }

        public UserBoxInfo[] GetAllUserBox()
        {
            List<UserBoxInfo> userBoxInfoList = new List<UserBoxInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_TimeBox_Award_All");
                while (ResultDataReader.Read())
                {
                    UserBoxInfo userBoxInfo = new UserBoxInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        Type = (int)ResultDataReader["Type"],
                        Level = (int)ResultDataReader["Level"],
                        Condition = (int)ResultDataReader["Condition"],
                        TemplateID = (int)ResultDataReader["TemplateID"]
                    };
                    userBoxInfoList.Add(userBoxInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllUserBox), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return userBoxInfoList.ToArray();
        }

        public EventAwardInfo[] GetEventAwardInfos()
        {
            List<EventAwardInfo> eventAwardInfoList = new List<EventAwardInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_EventAwardItem_All");
                while (ResultDataReader.Read())
                {
                    EventAwardInfo eventAwardInfo = new EventAwardInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        ActivityType = (int)ResultDataReader["ActivityType"],
                        EventAwardInfo = (int)ResultDataReader["EventAwardInfo"],
                        TemplateID = (int)ResultDataReader["TemplateID"],
                        Count = (int)ResultDataReader["Count"],
                        ValidDate = (int)ResultDataReader["ValidDate"],
                        IsBinds = (bool)ResultDataReader["IsBinds"],
                        StrengthenLevel = (int)ResultDataReader["StrengthenLevel"],
                        AttackCompose = (int)ResultDataReader["AttackCompose"],
                        DefendCompose = (int)ResultDataReader["DefendCompose"],
                        AgilityCompose = (int)ResultDataReader["AgilityCompose"],
                        LuckCompose = (int)ResultDataReader["LuckCompose"],
                        Random = (int)ResultDataReader["Random"]
                    };
                    eventAwardInfoList.Add(eventAwardInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"GetAllEventAward", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return eventAwardInfoList.ToArray();
        }

        public ItemTemplateInfo[] GetFusionType()
        {
            List<ItemTemplateInfo> itemTemplateInfoList = new List<ItemTemplateInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Items_FusionType");
                while (ResultDataReader.Read())
                    itemTemplateInfoList.Add(this.InitItemTemplateInfo(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return itemTemplateInfoList.ToArray();
        }

        public ItemBoxInfo[] GetItemBoxInfos()
        {
            List<ItemBoxInfo> itemBoxInfoList = new List<ItemBoxInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_ItemsBox_All");
                while (ResultDataReader.Read())
                    itemBoxInfoList.Add(this.InitItemBoxInfo(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)("Init@Shop_Goods_Box：" + (object)ex));
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return itemBoxInfoList.ToArray();
        }

        public ItemTemplateInfo[] GetSingleCategory(int CategoryID)
        {
            List<ItemTemplateInfo> itemTemplateInfoList = new List<ItemTemplateInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@CategoryID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)CategoryID;
                this.db.GetReader(ref ResultDataReader, "SP_Items_Category_Single", SqlParameters);
                while (ResultDataReader.Read())
                    itemTemplateInfoList.Add(this.InitItemTemplateInfo(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return itemTemplateInfoList.ToArray();
        }

        public ItemTemplateInfo GetSingleGoods(int goodsID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@ID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)goodsID;
                this.db.GetReader(ref ResultDataReader, "SP_Items_Single", SqlParameters);
                if (ResultDataReader.Read())
                    return this.InitItemTemplateInfo(ResultDataReader);
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return (ItemTemplateInfo)null;
        }

        public ItemBoxInfo[] GetSingleItemsBox(int DataID)
        {
            List<ItemBoxInfo> itemBoxInfoList = new List<ItemBoxInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@ID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)DataID;
                this.db.GetReader(ref ResultDataReader, "SP_ItemsBox_Single", SqlParameters);
                while (ResultDataReader.Read())
                    itemBoxInfoList.Add(this.InitItemBoxInfo(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return itemBoxInfoList.ToArray();
        }

        public QuestInfo GetSingleQuest(int questID)
        {
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@QuestID", SqlDbType.Int, 4)
                };
                SqlParameters[0].Value = (object)questID;
                this.db.GetReader(ref ResultDataReader, "SP_Quest_Single", SqlParameters);
                if (ResultDataReader.Read())
                    return this.InitQuest(ResultDataReader);
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return (QuestInfo)null;
        }

        public AchievementInfo InitAchievement(SqlDataReader reader)
        {
            return new AchievementInfo()
            {
                ID = (int)reader["ID"],
                PlaceID = (int)reader["PlaceID"],
                Title = reader["Title"] == null ? "" : reader["Title"].ToString(),
                Detail = reader["Detail"] == null ? "" : reader["Detail"].ToString(),
                NeedMinLevel = (int)reader["NeedMinLevel"],
                NeedMaxLevel = (int)reader["NeedMaxLevel"],
                PreAchievementID = reader["PreAchievementID"] == null ? "" : reader["PreAchievementID"].ToString(),
                IsOther = (int)reader["IsOther"],
                AchievementType = (int)reader["AchievementType"],
                CanHide = (bool)reader["CanHide"],
                StartDate = (DateTime)reader["StartDate"],
                EndDate = (DateTime)reader["EndDate"],
                AchievementPoint = (int)reader["AchievementPoint"],
                IsActive = (int)reader["IsActive"],
                PicID = (int)reader["PicID"],
                IsShare = (bool)reader["IsShare"]
            };
        }

        public AchievementConditionInfo InitAchievementCondition(
          SqlDataReader reader)
        {
            return new AchievementConditionInfo()
            {
                AchievementID = (int)reader["AchievementID"],
                CondictionID = (int)reader["CondictionID"],
                CondictionType = (int)reader["CondictionType"],
                Condiction_Para1 = reader["Condiction_Para1"] == null ? "" : reader["Condiction_Para1"].ToString(),
                Condiction_Para2 = (int)reader["Condiction_Para2"]
            };
        }

        public AchievementRewardInfo InitAchievementReward(SqlDataReader reader)
        {
            return new AchievementRewardInfo()
            {
                AchievementID = (int)reader["AchievementID"],
                RewardType = (int)reader["RewardType"],
                RewardPara = reader["RewardPara"] == null ? "" : reader["RewardPara"].ToString(),
                RewardValueId = (int)reader["RewardValueId"],
                RewardCount = (int)reader["RewardCount"]
            };
        }

        public DropCondiction InitDropCondiction(SqlDataReader reader)
        {
            return new DropCondiction()
            {
                DropId = (int)reader["DropID"],
                CondictionType = (int)reader["CondictionType"],
                Para1 = (string)reader["Para1"],
                Para2 = (string)reader["Para2"]
            };
        }

        public DropItem InitDropItem(SqlDataReader reader)
        {
            return new DropItem()
            {
                Id = (int)reader["Id"],
                DropId = (int)reader["DropId"],
                ItemId = (int)reader["ItemId"],
                ValueDate = (int)reader["ValueDate"],
                IsBind = (bool)reader["IsBind"],
                Random = (int)reader["Random"],
                BeginData = (int)reader["BeginData"],
                EndData = (int)reader["EndData"],
                IsLogs = (bool)reader["IsLogs"],
                IsTips = (bool)reader["IsTips"]
            };
        }

        public EdictumInfo InitEdictum(SqlDataReader reader)
        {
            return new EdictumInfo()
            {
                ID = (int)reader["ID"],
                Title = reader["Title"] == null ? "" : reader["Title"].ToString(),
                BeginDate = (DateTime)reader["BeginDate"],
                BeginTime = (DateTime)reader["BeginTime"],
                EndDate = (DateTime)reader["EndDate"],
                EndTime = (DateTime)reader["EndTime"],
                Text = reader["Text"] == null ? "" : reader["Text"].ToString(),
                IsExist = (bool)reader["IsExist"]
            };
        }

        public ItemBoxInfo InitItemBoxInfo(SqlDataReader reader)
        {
            return new ItemBoxInfo()
            {
                ID = (int)reader["ID"],
                TemplateId = (int)reader["TemplateId"],
                IsSelect = (bool)reader["IsSelect"],
                IsBind = (bool)reader["IsBind"],
                ItemValid = (int)reader["ItemValid"],
                ItemCount = (int)reader["ItemCount"],
                StrengthenLevel = (int)reader["StrengthenLevel"],
                AttackCompose = (int)reader["AttackCompose"],
                DefendCompose = (int)reader["DefendCompose"],
                AgilityCompose = (int)reader["AgilityCompose"],
                LuckCompose = (int)reader["LuckCompose"],
                Random = (int)reader["Random"],
                IsTips = (int)reader["IsTips"],
                IsLogs = (bool)reader["IsLogs"]
            };
        }

        public ItemRecordTypeInfo InitItemRecordType(SqlDataReader reader)
        {
            return new ItemRecordTypeInfo()
            {
                RecordID = (int)reader["RecordID"],
                Name = reader["Name"] == null ? "" : reader["Name"].ToString(),
                Description = reader["Description"] == null ? "" : reader["Description"].ToString()
            };
        }

        public ItemTemplateInfo InitItemTemplateInfo(SqlDataReader reader)
        {
            ItemTemplateInfo itemTemplateInfo = new ItemTemplateInfo();
            itemTemplateInfo.AddTime = reader["AddTime"].ToString();
            itemTemplateInfo.Agility = (int)reader["Agility"];
            itemTemplateInfo.Attack = (int)reader["Attack"];
            itemTemplateInfo.CanDelete = (bool)reader["CanDelete"];
            itemTemplateInfo.CanDrop = (bool)reader["CanDrop"];
            itemTemplateInfo.CanEquip = (bool)reader["CanEquip"];
            itemTemplateInfo.CanUse = (bool)reader["CanUse"];
            itemTemplateInfo.CategoryID = (int)reader["CategoryID"];
            itemTemplateInfo.Colors = reader["Colors"].ToString();
            itemTemplateInfo.Defence = (int)reader["Defence"];
            itemTemplateInfo.Description = reader["Description"].ToString();
            itemTemplateInfo.Level = (int)reader["Level"];
            itemTemplateInfo.Luck = (int)reader["Luck"];
            itemTemplateInfo.MaxCount = (int)reader["MaxCount"];
            itemTemplateInfo.Name = reader["Name"].ToString();
            itemTemplateInfo.NeedSex = (int)reader["NeedSex"];
            itemTemplateInfo.Pic = reader["Pic"].ToString();
            itemTemplateInfo.Data = reader["Data"] == null ? "" : reader["Data"].ToString();
            itemTemplateInfo.Property1 = (int)reader["Property1"];
            itemTemplateInfo.Property2 = (int)reader["Property2"];
            itemTemplateInfo.Property3 = (int)reader["Property3"];
            itemTemplateInfo.Property4 = (int)reader["Property4"];
            itemTemplateInfo.Property5 = (int)reader["Property5"];
            itemTemplateInfo.Property6 = (int)reader["Property6"];
            itemTemplateInfo.Property7 = (int)reader["Property7"];
            itemTemplateInfo.Property8 = (int)reader["Property8"];
            itemTemplateInfo.Quality = (int)reader["Quality"];
            itemTemplateInfo.Script = reader["Script"].ToString();
            itemTemplateInfo.TemplateID = (int)reader["TemplateID"];
            itemTemplateInfo.CanCompose = (bool)reader["CanCompose"];
            itemTemplateInfo.CanStrengthen = (bool)reader["CanStrengthen"];
            itemTemplateInfo.NeedLevel = (int)reader["NeedLevel"];
            itemTemplateInfo.BindType = (int)reader["BindType"];
            itemTemplateInfo.FusionType = (int)reader["FusionType"];
            itemTemplateInfo.FusionRate = (int)reader["FusionRate"];
            itemTemplateInfo.FusionNeedRate = (int)reader["FusionNeedRate"];
            itemTemplateInfo.Hole = reader["Hole"] == null ? "" : reader["Hole"].ToString();
            itemTemplateInfo.RefineryLevel = (int)reader["RefineryLevel"];
            itemTemplateInfo.ReclaimValue = (int)reader["ReclaimValue"];
            itemTemplateInfo.ReclaimType = (int)reader["ReclaimType"];
            itemTemplateInfo.CanRecycle = (int)reader["CanRecycle"];
            itemTemplateInfo.IsDirty = false;
            return itemTemplateInfo;
        }

        public QQtipsMessagesInfo InitQQtipsMessagesLoad(SqlDataReader reader)
        {
            return new QQtipsMessagesInfo()
            {
                ID = (int)reader["ID"],
                title = reader["title"] == null ? "QQTips" : reader["title"].ToString(),
                content = reader["content"] == null ? "Thông báo, gợi ý hệ thống" : reader["content"].ToString(),
                maxLevel = (int)reader["maxLevel"],
                minLevel = (int)reader["minLevel"],
                outInType = (int)reader["outInType"],
                moduleType = (int)reader["moduleType"],
                inItemID = (int)reader["inItemID"],
                url = reader["url"] == null ? "http://gunny.zing.vn" : reader["url"].ToString()
            };
        }

        public QuestInfo InitQuest(SqlDataReader reader)
        {
            return new QuestInfo()
            {
                ID = (int)reader["ID"],
                QuestID = (int)reader["QuestID"],
                Title = reader["Title"] == null ? "" : reader["Title"].ToString(),
                Detail = reader["Detail"] == null ? "" : reader["Detail"].ToString(),
                Objective = reader["Objective"] == null ? "" : reader["Objective"].ToString(),
                NeedMinLevel = (int)reader["NeedMinLevel"],
                NeedMaxLevel = (int)reader["NeedMaxLevel"],
                PreQuestID = reader["PreQuestID"] == null ? "" : reader["PreQuestID"].ToString(),
                NextQuestID = reader["NextQuestID"] == null ? "" : reader["NextQuestID"].ToString(),
                IsOther = (int)reader["IsOther"],
                CanRepeat = (bool)reader["CanRepeat"],
                RepeatInterval = (int)reader["RepeatInterval"],
                RepeatMax = (int)reader["RepeatMax"],
                RewardGP = (int)reader["RewardGP"],
                RewardGold = (int)reader["RewardGold"],
                RewardGiftToken = (int)reader["RewardGiftToken"],
                RewardOffer = (int)reader["RewardOffer"],
                RewardRiches = (int)reader["RewardRiches"],
                RewardBuffID = (int)reader["RewardBuffID"],
                RewardBuffDate = (int)reader["RewardBuffDate"],
                RewardMoney = (int)reader["RewardMoney"],
                Rands = (Decimal)reader["Rands"],
                RandDouble = (int)reader["RandDouble"],
                TimeMode = (bool)reader["TimeMode"],
                StartDate = (DateTime)reader["StartDate"],
                EndDate = (DateTime)reader["EndDate"],
                MapID = (int)reader["MapID"],
                AutoEquip = (bool)reader["AutoEquip"],
                RewardMedal = (int)reader["RewardMedal"],
                Rank = reader["Rank"] == null ? "" : reader["Rank"].ToString(),
                StarLev = (int)reader["StarLev"],
                NotMustCount = (int)reader["NotMustCount"]
            };
        }

        public QuestConditionInfo InitQuestCondiction(SqlDataReader reader)
        {
            return new QuestConditionInfo()
            {
                QuestID = (int)reader["QuestID"],
                CondictionID = (int)reader["CondictionID"],
                CondictionTitle = reader["CondictionTitle"] == null ? "" : reader["CondictionTitle"].ToString(),
                CondictionType = (int)reader["CondictionType"],
                Para1 = (int)reader["Para1"],
                Para2 = (int)reader["Para2"],
                isOpitional = (bool)reader["isOpitional"]
            };
        }

        public QuestAwardInfo InitQuestGoods(SqlDataReader reader)
        {
            return new QuestAwardInfo()
            {
                QuestID = (int)reader["QuestID"],
                RewardItemID = (int)reader["RewardItemID"],
                IsSelect = (bool)reader["IsSelect"],
                IsBind = (bool)reader["IsBind"],
                RewardItemValid = (int)reader["RewardItemValid"],
                RewardItemCount = (int)reader["RewardItemCount"],
                StrengthenLevel = (int)reader["StrengthenLevel"],
                AttackCompose = (int)reader["AttackCompose"],
                DefendCompose = (int)reader["DefendCompose"],
                AgilityCompose = (int)reader["AgilityCompose"],
                LuckCompose = (int)reader["LuckCompose"],
                IsCount = (bool)reader["IsCount"]
            };
        }

        public QuestRateInfo InitQuestRate(SqlDataReader reader)
        {
            return new QuestRateInfo()
            {
                BindMoneyRate = reader["BindMoneyRate"] == null ? "" : reader["BindMoneyRate"].ToString(),
                ExpRate = reader["ExpRate"] == null ? "" : reader["ExpRate"].ToString(),
                GoldRate = reader["GoldRate"] == null ? "" : reader["GoldRate"].ToString(),
                ExploitRate = reader["ExploitRate"] == null ? "" : reader["ExploitRate"].ToString(),
                CanOneKeyFinishTime = (int)reader["CanOneKeyFinishTime"]
            };
        }

        public ShopGoodsShowListInfo InitShopGoodsShowListInfo(SqlDataReader reader)
        {
            return new ShopGoodsShowListInfo()
            {
                Type = (int)reader["Type"],
                ShopId = (int)reader["ShopId"]
            };
        }

        public EventLiveInfo InitEventLiveInfo(SqlDataReader reader)
        {
            return new EventLiveInfo()
            {
                EventID = (int)reader["EventID"],
                Description = reader["Description"].ToString(),
                CondictionType = (int)reader["CondictionType"],
                Condiction_Para1 = (int)reader["Condiction_Para1"],
                Condiction_Para2 = (int)reader["Condiction_Para2"],
                StartDate = (DateTime)reader["StartDate"],
                EndDate = (DateTime)reader["EndDate"]
            };
        }

        public EventLiveGoods InitEventLiveGoods(SqlDataReader reader)
        {
            return new EventLiveGoods()
            {
                EventID = (int)reader["EventID"],
                TemplateID = (int)reader["TemplateID"],
                ValidDate = (int)reader["ValidDate"],
                Count = (int)reader["Count"],
                StrengthenLevel = (int)reader["StrengthenLevel"],
                AttackCompose = (int)reader["AttackCompose"],
                DefendCompose = (int)reader["DefendCompose"],
                AgilityCompose = (int)reader["AgilityCompose"],
                LuckCompose = (int)reader["LuckCompose"],
                IsBind = (bool)reader["IsBind"]
            };
        }

        public SubActiveInfo[] GetAllSubActive()
        {
            List<SubActiveInfo> subActiveInfoList = new List<SubActiveInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_SubActive_All");
                while (ResultDataReader.Read())
                {
                    SubActiveInfo subActiveInfo = new SubActiveInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        ActiveID = (int)ResultDataReader["ActiveID"],
                        SubID = (int)ResultDataReader["SubID"],
                        IsOpen = (bool)ResultDataReader["IsOpen"],
                        StartDate = (DateTime)ResultDataReader["StartDate"],
                        StartTime = (DateTime)ResultDataReader["StartTime"],
                        EndDate = (DateTime)ResultDataReader["EndDate"],
                        EndTime = (DateTime)ResultDataReader["EndTime"],
                        IsContinued = (bool)ResultDataReader["IsContinued"],
                        ActiveInfo = ResultDataReader["ActiveInfo"] == null ? "" : ResultDataReader["ActiveInfo"].ToString()
                    };
                    subActiveInfoList.Add(subActiveInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init AllSubActive", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return subActiveInfoList.ToArray();
        }

        public CommunalActiveAwardInfo[] GetAllCommunalActiveAward()
        {
            List<CommunalActiveAwardInfo> communalActiveAwardInfoList = new List<CommunalActiveAwardInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_CommunalActiveAward_All");
                while (ResultDataReader.Read())
                {
                    CommunalActiveAwardInfo communalActiveAwardInfo = new CommunalActiveAwardInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        ActiveID = (int)ResultDataReader["ActiveID"],
                        IsArea = (int)ResultDataReader["IsArea"],
                        RandID = (int)ResultDataReader["RandID"],
                        TemplateID = (int)ResultDataReader["TemplateID"],
                        StrengthenLevel = (int)ResultDataReader["StrengthenLevel"],
                        AttackCompose = (int)ResultDataReader["AttackCompose"],
                        DefendCompose = (int)ResultDataReader["DefendCompose"],
                        AgilityCompose = (int)ResultDataReader["AgilityCompose"],
                        LuckCompose = (int)ResultDataReader["LuckCompose"],
                        Count = (int)ResultDataReader["Count"],
                        IsBind = (bool)ResultDataReader["IsBind"],
                        IsTime = (bool)ResultDataReader["IsTime"],
                        ValidDate = (int)ResultDataReader["ValidDate"]
                    };
                    communalActiveAwardInfoList.Add(communalActiveAwardInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllCommunalActiveAward), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return communalActiveAwardInfoList.ToArray();
        }

        public CommunalActiveExpInfo[] GetAllCommunalActiveExp()
        {
            List<CommunalActiveExpInfo> communalActiveExpInfoList = new List<CommunalActiveExpInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_CommunalActiveExp_All");
                while (ResultDataReader.Read())
                {
                    CommunalActiveExpInfo communalActiveExpInfo = new CommunalActiveExpInfo()
                    {
                        ActiveID = (int)ResultDataReader["ActiveID"],
                        Grade = (int)ResultDataReader["Grade"],
                        Exp = (int)ResultDataReader["Exp"],
                        AddExpPlus = (int)ResultDataReader["AddExpPlus"]
                    };
                    communalActiveExpInfoList.Add(communalActiveExpInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllCommunalActiveExp), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return communalActiveExpInfoList.ToArray();
        }

        public SubActiveConditionInfo[] GetAllSubActiveCondition(int ActiveID)
        {
            List<SubActiveConditionInfo> activeConditionInfoList = new List<SubActiveConditionInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                SqlParameter[] SqlParameters = new SqlParameter[1]
                {
          new SqlParameter("@ActiveID", (object) ActiveID)
                };
                this.db.GetReader(ref ResultDataReader, "SP_SubActiveCondition_All", SqlParameters);
                while (ResultDataReader.Read())
                {
                    SubActiveConditionInfo activeConditionInfo = new SubActiveConditionInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        ActiveID = (int)ResultDataReader[nameof(ActiveID)],
                        SubID = (int)ResultDataReader["SubID"],
                        ConditionID = (int)ResultDataReader["ConditionID"],
                        Type = (int)ResultDataReader["Type"],
                        Value = ResultDataReader["Value"] == null ? "" : ResultDataReader["Value"].ToString(),
                        AwardType = (int)ResultDataReader["AwardType"],
                        AwardValue = ResultDataReader["AwardValue"] == null ? "" : ResultDataReader["AwardValue"].ToString(),
                        IsValid = (bool)ResultDataReader["IsValid"]
                    };
                    activeConditionInfoList.Add(activeConditionInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init AllSubActive", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return activeConditionInfoList.ToArray();
        }

        public CommunalActiveInfo[] GetAllCommunalActive()
        {
            List<CommunalActiveInfo> communalActiveInfoList = new List<CommunalActiveInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_CommunalActive_All");
                while (ResultDataReader.Read())
                {
                    CommunalActiveInfo communalActiveInfo = new CommunalActiveInfo()
                    {
                        ActiveID = (int)ResultDataReader["ActiveID"],
                        BeginTime = (DateTime)ResultDataReader["BeginTime"],
                        EndTime = (DateTime)ResultDataReader["EndTime"],
                        LimitGrade = (int)ResultDataReader["LimitGrade"],
                        DayMaxScore = (int)ResultDataReader["DayMaxScore"],
                        MinScore = (int)ResultDataReader["MinScore"],
                        AddPropertyByMoney = (string)ResultDataReader["AddPropertyByMoney"],
                        AddPropertyByProp = (string)ResultDataReader["AddPropertyByProp"],
                        IsReset = (bool)ResultDataReader["IsReset"],
                        IsSendAward = (bool)ResultDataReader["IsSendAward"]
                    };
                    communalActiveInfoList.Add(communalActiveInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllCommunalActive), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return communalActiveInfoList.ToArray();
        }

        public FairBattleRewardInfo[] GetAllFairBattleReward()
        {
            List<FairBattleRewardInfo> battleRewardInfoList = new List<FairBattleRewardInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_FairBattleReward_All");
                while (ResultDataReader.Read())
                {
                    FairBattleRewardInfo battleRewardInfo = new FairBattleRewardInfo()
                    {
                        Prestige = (int)ResultDataReader["Prestige"],
                        Level = (int)ResultDataReader["Level"],
                        Name = (string)ResultDataReader["Name"],
                        PrestigeForWin = (int)ResultDataReader["PrestigeForWin"],
                        PrestigeForLose = (int)ResultDataReader["PrestigeForLose"],
                        Title = (string)ResultDataReader["Title"]
                    };
                    battleRewardInfoList.Add(battleRewardInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllFairBattleReward), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return battleRewardInfoList.ToArray();
        }

        public CardUpdateConditionInfo[] GetAllCardUpdateCondition()
        {
            List<CardUpdateConditionInfo> updateConditionInfoList = new List<CardUpdateConditionInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Get_CardUpdateCondiction");
                while (ResultDataReader.Read())
                {
                    CardUpdateConditionInfo updateConditionInfo = new CardUpdateConditionInfo()
                    {
                        Level = (int)ResultDataReader["Level"],
                        Exp = (int)ResultDataReader["Exp"],
                        MinExp = (int)ResultDataReader["MinExp"],
                        MaxExp = (int)ResultDataReader["MaxExp"],
                        UpdateCardCount = (int)ResultDataReader["UpdateCardCount"],
                        ResetCardCount = (int)ResultDataReader["ResetCardCount"],
                        ResetMoney = (int)ResultDataReader["ResetMoney"]
                    };
                    updateConditionInfoList.Add(updateConditionInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return updateConditionInfoList.ToArray();
        }

        public CardGrooveUpdateInfo[] GetAllCardGrooveUpdate()
        {
            List<CardGrooveUpdateInfo> grooveUpdateInfoList = new List<CardGrooveUpdateInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_CardGrooveUpdate_All");
                while (ResultDataReader.Read())
                    grooveUpdateInfoList.Add(this.InitCardGrooveUpdate(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllCardGrooveUpdate), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return grooveUpdateInfoList.ToArray();
        }

        public CardUpdateInfo[] GetAllCardUpdateInfo()
        {
            List<CardUpdateInfo> cardUpdateInfoList = new List<CardUpdateInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Get_CardUpdateInfo");
                while (ResultDataReader.Read())
                {
                    CardUpdateInfo cardUpdateInfo = new CardUpdateInfo()
                    {
                        Id = (int)ResultDataReader["Id"],
                        Level = (int)ResultDataReader["Level"],
                        Attack = (int)ResultDataReader["Attack"],
                        Defend = (int)ResultDataReader["Defend"],
                        Agility = (int)ResultDataReader["Agility"],
                        Lucky = (int)ResultDataReader["Lucky"],
                        Guard = (int)ResultDataReader["Guard"],
                        Damage = (int)ResultDataReader["Damage"]
                    };
                    cardUpdateInfoList.Add(cardUpdateInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)"Init", ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return cardUpdateInfoList.ToArray();
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

        public LevelInfo[] GetAllLevel()
        {
            List<LevelInfo> levelInfoList = new List<LevelInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Level_All");
                while (ResultDataReader.Read())
                {
                    LevelInfo levelInfo = new LevelInfo()
                    {
                        Grade = (int)ResultDataReader["Grade"],
                        GP = (int)ResultDataReader["GP"],
                        Blood = (int)ResultDataReader["Blood"]
                    };
                    levelInfoList.Add(levelInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllLevel), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return levelInfoList.ToArray();
        }

        public ExerciseInfo[] GetAllExercise()
        {
            List<ExerciseInfo> exerciseInfoList = new List<ExerciseInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Exercise_All");
                while (ResultDataReader.Read())
                {
                    ExerciseInfo exerciseInfo = new ExerciseInfo()
                    {
                        Grage = (int)ResultDataReader["Grage"],
                        GP = (int)ResultDataReader["GP"],
                        ExerciseA = (int)ResultDataReader["ExerciseA"],
                        ExerciseAG = (int)ResultDataReader["ExerciseAG"],
                        ExerciseD = (int)ResultDataReader["ExerciseD"],
                        ExerciseH = (int)ResultDataReader["ExerciseH"],
                        ExerciseL = (int)ResultDataReader["ExerciseL"]
                    };
                    exerciseInfoList.Add(exerciseInfo);
                }
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllExercise), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return exerciseInfoList.ToArray();
        }

        public PetConfig[] GetAllPetConfig()
        {
            List<PetConfig> petConfigList = new List<PetConfig>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_PetConfig_All");
                while (ResultDataReader.Read())
                    petConfigList.Add(new PetConfig()
                    {
                        Name = ResultDataReader["Name"].ToString(),
                        Value = ResultDataReader["Value"].ToString()
                    });
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllPetConfig), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return petConfigList.ToArray();
        }

        public PetLevel[] GetAllPetLevel()
        {
            List<PetLevel> petLevelList = new List<PetLevel>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_PetLevel_All");
                while (ResultDataReader.Read())
                    petLevelList.Add(new PetLevel()
                    {
                        Level = (int)ResultDataReader["Level"],
                        GP = (int)ResultDataReader["GP"]
                    });
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllPetLevel), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return petLevelList.ToArray();
        }

        public PetTemplateInfo[] GetAllPetTemplateInfo()
        {
            List<PetTemplateInfo> petTemplateInfoList = new List<PetTemplateInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_PetTemplateInfo_All");
                while (ResultDataReader.Read())
                    petTemplateInfoList.Add(new PetTemplateInfo()
                    {
                        TemplateID = (int)ResultDataReader["TemplateID"],
                        Name = (string)ResultDataReader["Name"],
                        KindID = (int)ResultDataReader["KindID"],
                        Description = (string)ResultDataReader["Description"],
                        Pic = (string)ResultDataReader["Pic"],
                        RareLevel = (int)ResultDataReader["RareLevel"],
                        MP = (int)ResultDataReader["MP"],
                        StarLevel = (int)ResultDataReader["StarLevel"],
                        GameAssetUrl = (string)ResultDataReader["GameAssetUrl"],
                        HighAgility = (int)ResultDataReader["HighAgility"],
                        HighAgilityGrow = (int)ResultDataReader["HighAgilityGrow"],
                        HighAttack = (int)ResultDataReader["HighAttack"],
                        HighAttackGrow = (int)ResultDataReader["HighAttackGrow"],
                        HighBlood = (int)ResultDataReader["HighBlood"],
                        HighBloodGrow = (int)ResultDataReader["HighBloodGrow"],
                        HighDamage = (int)ResultDataReader["HighDamage"],
                        HighDamageGrow = (int)ResultDataReader["HighDamageGrow"],
                        HighDefence = (int)ResultDataReader["HighDefence"],
                        HighDefenceGrow = (int)ResultDataReader["HighDefenceGrow"],
                        HighGuard = (int)ResultDataReader["HighGuard"],
                        HighGuardGrow = (int)ResultDataReader["HighGuardGrow"],
                        HighLuck = (int)ResultDataReader["HighLuck"],
                        HighLuckGrow = (int)ResultDataReader["HighLuckGrow"]
                    });
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllPetTemplateInfo), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return petTemplateInfoList.ToArray();
        }

        public PetSkillTemplateInfo[] GetAllPetSkillTemplateInfo()
        {
            List<PetSkillTemplateInfo> skillTemplateInfoList = new List<PetSkillTemplateInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_PetSkillTemplateInfo_All");
                while (ResultDataReader.Read())
                    skillTemplateInfoList.Add(new PetSkillTemplateInfo()
                    {
                        PetTemplateID = (int)ResultDataReader["PetTemplateID"],
                        KindID = (int)ResultDataReader["KindID"],
                        GetTypes = (int)ResultDataReader["GetType"],
                        SkillID = (int)ResultDataReader["SkillID"],
                        SkillBookID = (int)ResultDataReader["SkillBookID"],
                        MinLevel = (int)ResultDataReader["MinLevel"],
                        DeleteSkillIDs = ResultDataReader["DeleteSkillIDs"].ToString()
                    });
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllPetSkillTemplateInfo), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return skillTemplateInfoList.ToArray();
        }

        public PetSkillInfo[] GetAllPetSkillInfo()
        {
            List<PetSkillInfo> petSkillInfoList = new List<PetSkillInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_PetSkillInfo_All");
                while (ResultDataReader.Read())
                    petSkillInfoList.Add(new PetSkillInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        Name = ResultDataReader["Name"].ToString(),
                        ElementIDs = ResultDataReader["ElementIDs"].ToString(),
                        Description = ResultDataReader["Description"].ToString(),
                        BallType = (int)ResultDataReader["BallType"],
                        NewBallID = (int)ResultDataReader["NewBallID"],
                        CostMP = (int)ResultDataReader["CostMP"],
                        Pic = (int)ResultDataReader["Pic"],
                        Action = ResultDataReader["Action"].ToString(),
                        EffectPic = ResultDataReader["EffectPic"].ToString(),
                        Delay = (int)ResultDataReader["Delay"],
                        ColdDown = (int)ResultDataReader["ColdDown"],
                        GameType = (int)ResultDataReader["GameType"],
                        Probability = (int)ResultDataReader["Probability"],
                        Damage = (int)ResultDataReader["Damage"],
                        DamageCrit = (int)ResultDataReader["DamageCrit"]
                    });
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllPetSkillInfo), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return petSkillInfoList.ToArray();
        }

        public FightSpiritTemplateInfo[] GetAllFightSpiritTemplate()
        {
            List<FightSpiritTemplateInfo> fightSpiritTemplateInfos = new List<FightSpiritTemplateInfo>();
            SqlDataReader sqlDataReader = null;
            try
            {
                try
                {
                    this.db.GetReader(ref sqlDataReader, "SP_FightSpiritTemplate_All");
                    while (sqlDataReader.Read())
                    {
                        FightSpiritTemplateInfo fightSpiritTemplateInfo = new FightSpiritTemplateInfo()
                        {
                            FightSpiritID = (int)sqlDataReader["FightSpiritID"],
                            FightSpiritIcon = (string)sqlDataReader["FightSpiritIcon"],
                            Level = (int)sqlDataReader["Level"],
                            Exp = (int)sqlDataReader["Exp"],
                            Attack = (int)sqlDataReader["Attack"],
                            Defence = (int)sqlDataReader["Defence"],
                            Agility = (int)sqlDataReader["Agility"],
                            Lucky = (int)sqlDataReader["Lucky"],
                            Blood = (int)sqlDataReader["Blood"]
                        };
                        fightSpiritTemplateInfos.Add(fightSpiritTemplateInfo);
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    if (this.log.IsErrorEnabled)
                    {
                        this.log.Error("GetFightSpiritTemplateAll", exception);
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
            return fightSpiritTemplateInfos.ToArray();
        }

        public PetMoePropertyInfo[] GetAllPetMoeProperty()
        {
            List<PetMoePropertyInfo> petMoePropertyInfos = new List<PetMoePropertyInfo>();
            SqlDataReader sqlDataReader = null;
            try
            {
                try
                {
                    this.db.GetReader(ref sqlDataReader, "SP_Pet_Moe_Property_All");
                    while (sqlDataReader.Read())
                    {
                        petMoePropertyInfos.Add(this.InitPetMoePropertyInfo(sqlDataReader));
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    if (this.log.IsErrorEnabled)
                    {
                        this.log.Error("InitPetMoePropertyInfo", exception);
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
            return petMoePropertyInfos.ToArray();
        }

        public TotemInfo[] GetAllTotem()
        {
            List<TotemInfo> totemInfos = new List<TotemInfo>();
            SqlDataReader sqlDataReader = null;
            try
            {
                try
                {
                    this.db.GetReader(ref sqlDataReader, "SP_Totem_All");
                    while (sqlDataReader.Read())
                    {
                        TotemInfo totemInfo = new TotemInfo()
                        {
                            ID = (int)sqlDataReader["ID"],
                            ConsumeExp = (int)sqlDataReader["ConsumeExp"],
                            ConsumeHonor = (int)sqlDataReader["ConsumeHonor"],
                            AddAttack = (int)sqlDataReader["AddAttack"],
                            AddDefence = (int)sqlDataReader["AddDefence"],
                            AddAgility = (int)sqlDataReader["AddAgility"],
                            AddLuck = (int)sqlDataReader["AddLuck"],
                            AddBlood = (int)sqlDataReader["AddBlood"],
                            AddDamage = (int)sqlDataReader["AddDamage"],
                            AddGuard = (int)sqlDataReader["AddGuard"],
                            Random = (int)sqlDataReader["Random"],
                            Page = (int)sqlDataReader["Page"],
                            Layers = (int)sqlDataReader["Layers"],
                            Location = (int)sqlDataReader["Location"],
                            Point = (int)sqlDataReader["Point"]
                        };
                        totemInfos.Add(totemInfo);
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    if (this.log.IsErrorEnabled)
                    {
                        this.log.Error("GetTotemAll", exception);
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
            return totemInfos.ToArray();
        }

        public TotemHonorTemplateInfo[] GetAllTotemHonorTemplate()
        {
            List<TotemHonorTemplateInfo> totemHonorTemplateInfos = new List<TotemHonorTemplateInfo>();
            SqlDataReader sqlDataReader = null;
            try
            {
                try
                {
                    this.db.GetReader(ref sqlDataReader, "SP_TotemHonorTemplate_All");
                    while (sqlDataReader.Read())
                    {
                        TotemHonorTemplateInfo totemHonorTemplateInfo = new TotemHonorTemplateInfo()
                        {
                            ID = (int)sqlDataReader["ID"],
                            NeedMoney = (int)sqlDataReader["NeedMoney"],
                            Type = (int)sqlDataReader["Type"],
                            AddHonor = (int)sqlDataReader["AddHonor"]
                        };
                        totemHonorTemplateInfos.Add(totemHonorTemplateInfo);
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    if (this.log.IsErrorEnabled)
                    {
                        this.log.Error("GetTotemHonorTemplateInfo", exception);
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
            return totemHonorTemplateInfos.ToArray();
        }

        public PetSkillElementInfo[] GetAllPetSkillElementInfo()
        {
            List<PetSkillElementInfo> skillElementInfoList = new List<PetSkillElementInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_PetSkillElementInfo_All");
                while (ResultDataReader.Read())
                    skillElementInfoList.Add(new PetSkillElementInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        Name = ResultDataReader["Name"].ToString(),
                        EffectPic = ResultDataReader["EffectPic"].ToString(),
                        Description = ResultDataReader["Description"].ToString(),
                        Pic = (int)ResultDataReader["Pic"]
                    });
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllPetSkillElementInfo), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return skillElementInfoList.ToArray();
        }

        public PetFightPropertyInfo[] GetAllPetFightProperty()
        {
            List<PetFightPropertyInfo> fightPropertyInfoList = new List<PetFightPropertyInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_PetFightProperty_All");
                while (ResultDataReader.Read())
                    fightPropertyInfoList.Add(new PetFightPropertyInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        Exp = (int)ResultDataReader["Exp"],
                        Attack = (int)ResultDataReader["Attack"],
                        Agility = (int)ResultDataReader["Agility"],
                        Defence = (int)ResultDataReader["Defence"],
                        Lucky = (int)ResultDataReader["Lucky"],
                        Blood = (int)ResultDataReader["Blood"]
                    });
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllPetFightProperty), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return fightPropertyInfoList.ToArray();
        }

        public PetStarExpInfo[] GetAllPetStarExp()
        {
            List<PetStarExpInfo> petStarExpInfoList = new List<PetStarExpInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_PetStarExp_All");
                while (ResultDataReader.Read())
                    petStarExpInfoList.Add(new PetStarExpInfo()
                    {
                        Exp = (int)ResultDataReader["Exp"],
                        OldID = (int)ResultDataReader["OldID"],
                        NewID = (int)ResultDataReader["NewID"]
                    });
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllPetStarExp), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return petStarExpInfoList.ToArray();
        }

        public ConsortiaBuffTempInfo[] GetAllConsortiaBuffTemp()
        {
            List<ConsortiaBuffTempInfo> consortiaBuffTempInfoList = new List<ConsortiaBuffTempInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Consortia_Buff_Temp_All");
                while (ResultDataReader.Read())
                    consortiaBuffTempInfoList.Add(new ConsortiaBuffTempInfo()
                    {
                        id = (int)ResultDataReader["id"],
                        name = (string)ResultDataReader["name"],
                        descript = (string)ResultDataReader["descript"],
                        type = (int)ResultDataReader["type"],
                        level = (int)ResultDataReader["level"],
                        value = (int)ResultDataReader["value"],
                        riches = (int)ResultDataReader["riches"],
                        metal = (int)ResultDataReader["metal"],
                        pic = (int)ResultDataReader["pic"],
                        group = (int)ResultDataReader["group"]
                    });
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllConsortiaBuffTemp), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return consortiaBuffTempInfoList.ToArray();
        }

        public ConsortiaTaskInfo[] GetAllConsortiaTask()
        {
            List<ConsortiaTaskInfo> consortiaTaskInfoList = new List<ConsortiaTaskInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Consortia_Task_All");
                while (ResultDataReader.Read())
                    consortiaTaskInfoList.Add(new ConsortiaTaskInfo()
                    {
                        ID = (int)ResultDataReader["ID"],
                        Level = (int)ResultDataReader["Level"],
                        CondictionTitle = (string)ResultDataReader["CondictionTitle"],
                        CondictionType = (int)ResultDataReader["CondictionType"],
                        Para1 = (int)ResultDataReader["Para1"],
                        Para2 = (int)ResultDataReader["Para2"]
                    });
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllConsortiaTask), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return consortiaTaskInfoList.ToArray();
        }

        public ConsortiaLevelInfo[] GetAllConsortiaLevel()
        {
            List<ConsortiaLevelInfo> consortiaLevelInfoList = new List<ConsortiaLevelInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Consortia_Level_All");
                while (ResultDataReader.Read())
                    consortiaLevelInfoList.Add(new ConsortiaLevelInfo()
                    {
                        Count = (int)ResultDataReader["Count"],
                        Deduct = (int)ResultDataReader["Deduct"],
                        Level = (int)ResultDataReader["Level"],
                        NeedGold = (int)ResultDataReader["NeedGold"],
                        NeedItem = (int)ResultDataReader["NeedItem"],
                        Reward = (int)ResultDataReader["Reward"],
                        Riches = (int)ResultDataReader["Riches"],
                        ShopRiches = (int)ResultDataReader["ShopRiches"],
                        SmithRiches = (int)ResultDataReader["SmithRiches"],
                        StoreRiches = (int)ResultDataReader["StoreRiches"],
                        BufferRiches = (int)ResultDataReader["BufferRiches"]
                    });
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllConsortiaLevel), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return consortiaLevelInfoList.ToArray();
        }

        public ConsortiaBadgeConfigInfo[] GetAllConsortiaBadgeConfig()
        {
            List<ConsortiaBadgeConfigInfo> consortiaBadgeConfigInfoList = new List<ConsortiaBadgeConfigInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_Consortia_Badge_Config_All");
                while (ResultDataReader.Read())
                    consortiaBadgeConfigInfoList.Add(this.InitConsortiaBadgeConfig(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllConsortiaBadgeConfig), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return consortiaBadgeConfigInfoList.ToArray();
        }

        public PetMoePropertyInfo InitPetMoePropertyInfo(SqlDataReader dr)
        {
            return new PetMoePropertyInfo()
            {
                Level = (int)dr["Level"],
                Attack = (int)dr["Attack"],
                Lucky = (int)dr["Lucky"],
                Agility = (int)dr["Agility"],
                Blood = (int)dr["Blood"],
                Defence = (int)dr["Defence"],
                Guard = (int)dr["Guard"],
                Exp = (int)dr["Exp"]
            };
        }

        public ConsortiaBadgeConfigInfo InitConsortiaBadgeConfig(
          SqlDataReader reader)
        {
            return new ConsortiaBadgeConfigInfo()
            {
                BadgeID = (int)reader["BadgeID"],
                BadgeName = reader["BadgeName"] == null ? "" : reader["BadgeName"].ToString(),
                Cost = (int)reader["Cost"],
                LimitLevel = (int)reader["LimitLevel"],
                ValidDate = (int)reader["ValidDate"]
            };
        }

        public GoldEquipTemplateInfo[] GetAllGoldEquipTemplateLoad()
        {
            List<GoldEquipTemplateInfo> equipTemplateInfoList = new List<GoldEquipTemplateInfo>();
            SqlDataReader ResultDataReader = (SqlDataReader)null;
            try
            {
                this.db.GetReader(ref ResultDataReader, "SP_GoldEquipTemplateLoad_All");
                while (ResultDataReader.Read())
                    equipTemplateInfoList.Add(this.InitGoldEquipTemplateLoad(ResultDataReader));
            }
            catch (Exception ex)
            {
                if (this.log.IsErrorEnabled)
                    this.log.Error((object)nameof(GetAllGoldEquipTemplateLoad), ex);
            }
            finally
            {
                if (ResultDataReader != null && !ResultDataReader.IsClosed)
                    ResultDataReader.Close();
            }
            return equipTemplateInfoList.ToArray();
        }

        public GoldEquipTemplateInfo InitGoldEquipTemplateLoad(SqlDataReader reader)
        {
            return new GoldEquipTemplateInfo()
            {
                ID = (int)reader["ID"],
                OldTemplateId = (int)reader["OldTemplateId"],
                NewTemplateId = (int)reader["NewTemplateId"],
                CategoryID = (int)reader["CategoryID"],
                Strengthen = (int)reader["Strengthen"],
                Attack = (int)reader["Attack"],
                Defence = (int)reader["Defence"],
                Agility = (int)reader["Agility"],
                Luck = (int)reader["Luck"],
                Damage = (int)reader["Damage"],
                Guard = (int)reader["Guard"],
                Boold = (int)reader["Boold"],
                BlessID = (int)reader["BlessID"],
                Pic = reader["pic"] == DBNull.Value ? "" : reader["pic"].ToString()
            };
        }
    }
}
