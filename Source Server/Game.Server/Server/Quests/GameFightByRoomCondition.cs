// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.GameFightByRoomCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
  public class GameFightByRoomCondition : BaseCondition
  {
    public GameFightByRoomCondition(BaseQuest quest, QuestConditionInfo info, int value)
      : base(quest, info, value)
    {
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.GameOver += new GamePlayer.PlayerGameOverEventHandle(this.player_GameOver);
    }

    public override bool IsCompleted(GamePlayer player)
    {
      return this.Value <= 0;
    }

    private void player_GameOver(
      AbstractGame game,
      bool isWin,
      int gainXp,
      bool isSpanArea,
      bool isCouple)
    {
      switch (game.RoomType)
      {
        case eRoomType.Match:
          if ((this.m_info.Para1 == 0 || this.m_info.Para1 == -1) && this.Value > 0)
          {
            --this.Value;
            break;
          }
          break;
        case eRoomType.Freedom:
          if ((this.m_info.Para1 == 1 || this.m_info.Para1 == -1) && this.Value > 0)
          {
            --this.Value;
            break;
          }
          break;
        case eRoomType.Exploration:
          if ((this.m_info.Para1 == 2 || this.m_info.Para1 == -1) && this.Value > 0)
          {
            --this.Value;
            break;
          }
          break;
        case eRoomType.Dungeon:
          if ((this.m_info.Para1 == 4 || this.m_info.Para1 == -1) && this.Value > 0)
          {
            --this.Value;
            break;
          }
          break;
      }
      if (this.Value >= 0)
        return;
      this.Value = 0;
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.GameOver -= new GamePlayer.PlayerGameOverEventHandle(this.player_GameOver);
    }
  }
}
