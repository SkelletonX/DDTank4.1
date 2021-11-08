using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    public class CashCondition : BaseCondition
    {
        public CashCondition(BaseQuest quest, QuestConditionInfo info, int value)
          : base(quest, info, value)
        {
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.MoneyCharge += new GamePlayer.PlayerMoneyChargeHandle(this.player_MoneyCharge);
        }

        public override bool IsCompleted(GamePlayer player)
        {
            return this.Value <= 0;
        }

        private void player_MoneyCharge(int money)
        {
            this.Value -= money;
            if (this.m_info.Para2 > money)
                return;
            this.Value = 0;
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.MoneyCharge -= new GamePlayer.PlayerMoneyChargeHandle(this.player_MoneyCharge);
        }
    }
}
