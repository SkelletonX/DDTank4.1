// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.GameFightByGameCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
  public class GameFightByGameCondition : BaseCondition
  {
    public GameFightByGameCondition(BaseQuest quest, QuestConditionInfo info, int value)
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
      if (isWin)
      {
        if (game.RoomType == eRoomType.Freedom && this.m_info.Para1 == 2 && this.Value > 0)
          --this.Value;
        switch (game.GameType)
        {
          case eGameType.Free:
            if ((this.m_info.Para1 == 0 || this.m_info.Para1 == -1) && this.Value > 0)
            {
              --this.Value;
              break;
            }
            break;
          case eGameType.Guild:
            if ((this.m_info.Para1 == 1 || this.m_info.Para1 == -1) && this.Value > 0)
            {
              --this.Value;
              break;
            }
            break;
          case eGameType.Training:
            if ((this.m_info.Para1 == 2 || this.m_info.Para1 == -1) && this.Value > 0)
            {
              --this.Value;
              break;
            }
            break;
          case eGameType.Boss:
            if ((this.m_info.Para1 == 6 || this.m_info.Para1 == -1) && this.Value > 0)
            {
              --this.Value;
              break;
            }
            break;
          case eGameType.ALL:
            if ((this.m_info.Para1 == 4 || this.m_info.Para1 == -1) && this.Value > 0)
            {
              --this.Value;
              break;
            }
            break;
          case eGameType.Exploration:
            if ((this.m_info.Para1 == 5 || this.m_info.Para1 == -1) && this.Value > 0)
            {
              --this.Value;
              break;
            }
            break;
          case eGameType.Dungeon:
            if ((this.m_info.Para1 == 7 || this.m_info.Para1 == -1) && this.Value > 0)
            {
              --this.Value;
              break;
            }
            break;
        }
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
