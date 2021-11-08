// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.SharePersonalStatusCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
  public class SharePersonalStatusCondition : BaseCondition
  {
    public SharePersonalStatusCondition(BaseQuest quest, QuestConditionInfo info, int value)
      : base(quest, info, value)
    {
    }

    public override void AddTrigger(GamePlayer player)
    {
    }

    public override bool IsCompleted(GamePlayer player)
    {
      return this.Value <= 0;
    }

    private void player_MissionOver(AbstractGame game, int missionId, int turnCount)
    {
    }

    public override void RemoveTrigger(GamePlayer player)
    {
    }
  }
}
