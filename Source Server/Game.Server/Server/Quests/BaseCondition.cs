// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.BaseCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using log4net;
using SqlDataProvider.Data;
using System.Reflection;

namespace Game.Server.Quests
{
    public class BaseCondition
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected QuestConditionInfo m_info;
        private BaseQuest m_quest;
        private int m_value;

        public BaseCondition(BaseQuest quest, QuestConditionInfo info, int value)
        {
            this.m_quest = quest;
            this.m_info = info;
            this.m_value = value;
        }

        public virtual void AddTrigger(GamePlayer player)
        {
        }

        public virtual bool CancelFinish(GamePlayer player)
        {
            return true;
        }

        public static BaseCondition CreateCondition(
          BaseQuest quest,
          QuestConditionInfo info,
          int value)
        {
            switch (info.CondictionType)
            {
                case 1:
                    return (BaseCondition)new OwnGradeCondition(quest, info, value);
                case 2:
                    return (BaseCondition)new ItemMountingCondition(quest, info, value);
                case 3:
                    return (BaseCondition)new UsingItemCondition(quest, info, value);
                case 4:
                    return (BaseCondition)new GameKillByRoomCondition(quest, info, value);
                case 5:
                    return (BaseCondition)new GameFightByRoomCondition(quest, info, value);
                case 6:
                    return (BaseCondition)new GameOverByRoomCondition(quest, info, value);
                case 7:
                    return (BaseCondition)new GameCopyOverCondition(quest, info, value);
                case 8:
                    return (BaseCondition)new GameCopyPassCondition(quest, info, value);
                case 9:
                    return (BaseCondition)new ItemStrengthenCondition(quest, info, value);
                case 10:
                    return (BaseCondition)new ShopCondition(quest, info, value);
                case 11:
                    return (BaseCondition)new ItemFusionCondition(quest, info, value);
                case 12:
                    return (BaseCondition)new ItemMeltCondition(quest, info, value);
                case 13:
                    return (BaseCondition)new GameMonsterCondition(quest, info, value);
                case 14:
                    return (BaseCondition)new OwnPropertyCondition(quest, info, value);
                case 15:
                    return (BaseCondition)new TurnPropertyCondition(quest, info, value);
                case 16:
                    return (BaseCondition)new DirectFinishCondition(quest, info, value);
                case 17:
                    return (BaseCondition)new OwnMarryCondition(quest, info, value);
                case 18:
                    return (BaseCondition)new OwnConsortiaCondition(quest, info, value);
                case 19:
                    return (BaseCondition)new ItemComposeCondition(quest, info, value);
                case 20:
                    return (BaseCondition)new ClientModifyCondition(quest, info, value);
                case 21:
                    return (BaseCondition)new GameMissionOverCondition(quest, info, value);
                case 22:
                    return (BaseCondition)new GameKillByGameCondition(quest, info, value);
                case 23:
                    return (BaseCondition)new GameFightByGameCondition(quest, info, value);
                case 24:
                    return (BaseCondition)new GameOverByGameCondition(quest, info, value);
                case 25:
                    return (BaseCondition)new ItemInsertCondition(quest, info, value);
                case 26:
                    return (BaseCondition)new MarryCondition(quest, info, value);
                case 27:
                    return (BaseCondition)new EnterSpaCondition(quest, info, value);
                case 28:
                    return (BaseCondition)new FightWifeHusbandCondition(quest, info, value);
                case 29:
                    return (BaseCondition)new AchievementCondition(quest, info, value);
                case 30:
                    return (BaseCondition)new GameFihgt2v2Condition(quest, info, value);
                case 31:
                    return (BaseCondition)new GameFightByGameCondition(quest, info, value);
                case 32:
                    return (BaseCondition)new SharePersonalStatusCondition(quest, info, value);
                case 33:
                    return (BaseCondition)new SendGiftForFriendCondition(quest, info, value);
                case 34:
                    return (BaseCondition)new GameFihgt2v2Condition(quest, info, value);
                case 35:
                    return (BaseCondition)new MasterApprenticeshipCondition(quest, info, value);
                case 36:
                    return (BaseCondition)new GameFightApprenticeshipCondition(quest, info, value);
                case 37:
                    return (BaseCondition)new GameFightMasterApprenticeshipCondition(quest, info, value);
                case 38:
                    return (BaseCondition)new CashCondition(quest, info, value);
                case 39:
                    return (BaseCondition)new NewGearCondition(quest, info, value);
                case 42:
                    return (BaseCondition)new AccuontInfoCondition(quest, info, value);
                case 43:
                    return (BaseCondition)new LoginMissionPurpleCondition(quest, info, value);
                case 44:
                    return (BaseCondition)new SetPasswordTwoCondition(quest, info, value);
                case 45:
                    return (BaseCondition)new FightWithPetCondition(quest, info, value);
                case 46:
                    return (BaseCondition)new CombiePetFeedCondition(quest, info, value);
                case 47:
                    return (BaseCondition)new FriendFarmCondition(quest, info, value);
                case 48:
                    return (BaseCondition)new AdoptPetCondition(quest, info, value);
                case 49:
                    return (BaseCondition)new CropPrimaryCondition(quest, info, value);
                case 50:
                    return (BaseCondition)new UpLevelPetCondition(quest, info, value);
                case 52:
                    return (BaseCondition)new UserSkillPetCondition(quest, info, value);
                case 54:
                    return (BaseCondition)new UserToemGemstoneCondition(quest, info, value);
                default:
                    System.Console.WriteLine("Condition Type:{0} Not Found", info.CondictionType);
                    return (BaseCondition)new UnknowQuestCondition(quest, info, value);
            }
        }

        public virtual bool Finish(GamePlayer player)
        {
            return true;
        }

        public virtual bool IsCompleted(GamePlayer player)
        {
            return false;
        }

        public virtual void RemoveTrigger(GamePlayer player)
        {
        }

        public virtual void Reset(GamePlayer player)
        {
            this.m_value = this.m_info.Para2;
        }

        public QuestConditionInfo Info
        {
            get
            {
                return this.m_info;
            }
        }

        public int Value
        {
            get
            {
                return this.m_value;
            }
            set
            {
                if (this.m_value == value)
                    return;
                this.m_value = value;
                this.m_quest.Update();
            }
        }
    }
}
