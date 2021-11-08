// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.OwnMarryCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
  public class OwnMarryCondition : BaseCondition
  {
    public OwnMarryCondition(BaseQuest quest, QuestConditionInfo info, int value)
      : base(quest, info, value)
    {
    }

    public override void AddTrigger(GamePlayer player)
    {
    }

    public override bool IsCompleted(GamePlayer player)
    {
      if (!player.PlayerCharacter.IsMarried)
        return false;
      this.Value = 0;
      return true;
    }

    public override void RemoveTrigger(GamePlayer player)
    {
    }
  }
}
