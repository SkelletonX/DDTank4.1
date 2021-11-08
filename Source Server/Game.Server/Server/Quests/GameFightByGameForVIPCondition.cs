// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.GameFightByGameForVIPCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
  public class GameFightByGameForVIPCondition : BaseCondition
  {
    public GameFightByGameForVIPCondition(BaseQuest quest, QuestConditionInfo info, int value)
      : base(quest, info, value)
    {
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.GameOver += new GamePlayer.PlayerGameOverEventHandle(this.method_0);
    }

    public override bool IsCompleted(GamePlayer player)
    {
      return this.Value <= 0;
    }

    private void method_0(
      AbstractGame abstractGame_0,
      bool bool_0,
      int int_1,
      bool isSpanArea,
      bool isCouple)
    {
      if (!bool_0)
        return;
      switch (abstractGame_0.GameType)
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
      if (this.Value >= 0)
        return;
      this.Value = 0;
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.GameOver -= new GamePlayer.PlayerGameOverEventHandle(this.method_0);
    }
  }
}
