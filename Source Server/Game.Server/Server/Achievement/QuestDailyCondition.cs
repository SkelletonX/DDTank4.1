// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.QuestDailyCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using Game.Server.Quests;

namespace Game.Server.Achievement
{
  internal class QuestDailyCondition : BaseUserRecord
  {
    public QuestDailyCondition(GamePlayer player, int type)
      : base(player, type)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.PlayerQuestFinish += new GamePlayer.PlayerQuestFinishEventHandel(this.player_PlayerQuestFinish);
    }

    private void player_PlayerQuestFinish(BaseQuest baseQuest)
    {
      if (baseQuest.Info.QuestID != 2)
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.PlayerQuestFinish -= new GamePlayer.PlayerQuestFinishEventHandel(this.player_PlayerQuestFinish);
    }
  }
}
