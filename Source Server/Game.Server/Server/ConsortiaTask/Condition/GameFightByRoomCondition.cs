// Decompiled with JetBrains decompiler
// Type: Game.Server.ConsortiaTask.Condition.GameFightByRoomCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.ConsortiaTask.Data;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.ConsortiaTask.Condition
{
  public class GameFightByRoomCondition : BaseConsortiaTaskCondition
  {
    public GameFightByRoomCondition(
      ConsortiaTaskUserDataInfo player,
      BaseConsortiaTask quest,
      ConsortiaTaskInfo info,
      int value)
      : base(player, quest, info, value)
    {
    }

    public override void AddTrigger(ConsortiaTaskUserDataInfo player)
    {
      player.Player.GameOver += new GamePlayer.PlayerGameOverEventHandle(this.method_0);
    }

    public override void RemoveTrigger(ConsortiaTaskUserDataInfo player)
    {
      player.Player.GameOver -= new GamePlayer.PlayerGameOverEventHandle(this.method_0);
    }

    private void method_0(
      AbstractGame abstractGame_0,
      bool bool_0,
      int int_1,
      bool isArea,
      bool isCouple)
    {
      switch (abstractGame_0.RoomType)
      {
        case eRoomType.Match:
          if ((this.m_info.Para1 == 0 || this.m_info.Para1 == -1) && this.Value < this.m_info.Para2)
          {
            ++this.Value;
            break;
          }
          break;
        case eRoomType.Freedom:
          if ((this.m_info.Para1 == 1 || this.m_info.Para1 == -1) && this.Value < this.m_info.Para2)
          {
            ++this.Value;
            break;
          }
          break;
        case eRoomType.Exploration:
          if ((this.m_info.Para1 == 2 || this.m_info.Para1 == -1) && this.Value < this.m_info.Para2)
          {
            ++this.Value;
            break;
          }
          break;
        case eRoomType.Boss:
          if ((this.m_info.Para1 == 3 || this.m_info.Para1 == -1) && this.Value < this.m_info.Para2)
          {
            ++this.Value;
            break;
          }
          break;
        case eRoomType.Dungeon:
          if ((this.m_info.Para1 == 4 || this.m_info.Para1 == -1) && this.Value < this.m_info.Para2)
          {
            ++this.Value;
            break;
          }
          break;
        case eRoomType.Freshman:
          if ((this.m_info.Para1 == 2 || this.m_info.Para1 == -1) && this.Value < this.m_info.Para2)
          {
            ++this.Value;
            break;
          }
          break;
      }
      if (this.Value <= this.m_info.Para2)
        return;
      this.Value = this.m_info.Para2;
    }
  }
}
