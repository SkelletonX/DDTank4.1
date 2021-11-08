// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.GameMissionOverCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
  public class GameMissionOverCondition : BaseCondition
  {
    public GameMissionOverCondition(BaseQuest quest, QuestConditionInfo info, int value)
      : base(quest, info, value)
    {
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.MissionTurnOver += new GamePlayer.PlayerMissionTurnOverEventHandle(this.player_MissionOver);
    }

    public override bool IsCompleted(GamePlayer player)
    {
      return this.Value <= 0;
    }

    private void player_MissionOver(AbstractGame game, int missionId, int turnCount)
    {
      if (missionId != this.m_info.Para1 && this.m_info.Para1 != -1 || (turnCount > this.m_info.Para2 || this.Value <= 0))
        return;
      this.Value = 0;
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.MissionTurnOver -= new GamePlayer.PlayerMissionTurnOverEventHandle(this.player_MissionOver);
    }
  }
}
