using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    public class OwnGradeCondition : BaseCondition
    {
        public OwnGradeCondition(BaseQuest quest, QuestConditionInfo info, int value)
          : base(quest, info, value)
        {
        }

        public override void AddTrigger(GamePlayer player)
        {
        }

        public override bool IsCompleted(GamePlayer player)
        {
            if (player.Level < this.m_info.Para2)
                return false;
            this.Value = 0;
            return true;
        }

        private void player_UpdateGrade(int grade)
        {
        }

        public override void RemoveTrigger(GamePlayer player)
        {
        }
    }
}
