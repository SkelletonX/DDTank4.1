// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.GameFihgt2v2Condition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
  public class GameFihgt2v2Condition : BaseCondition
  {
    public GameFihgt2v2Condition(BaseQuest quest, QuestConditionInfo info, int value)
      : base(quest, info, value)
    {
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.GameOver2v2 += new GamePlayer.PlayerGameOverEvent2v2Handle(this.player_GameOver);
    }

    public override bool IsCompleted(GamePlayer player)
    {
      return this.Value <= 0;
    }

    private void player_GameOver(bool isWin)
    {
      if (this.m_info.Para1 == 1)
      {
        if (isWin)
          --this.Value;
      }
      else
        --this.Value;
      if (this.Value >= 0)
        return;
      this.Value = 0;
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.GameOver2v2 -= new GamePlayer.PlayerGameOverEvent2v2Handle(this.player_GameOver);
    }
  }
}
