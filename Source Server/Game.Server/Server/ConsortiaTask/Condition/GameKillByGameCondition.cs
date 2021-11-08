// Decompiled with JetBrains decompiler
// Type: Game.Server.ConsortiaTask.Condition.GameKillByGameCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.ConsortiaTask.Data;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.ConsortiaTask.Condition
{
  public class GameKillByGameCondition : BaseConsortiaTaskCondition
  {
    public GameKillByGameCondition(
      ConsortiaTaskUserDataInfo player,
      BaseConsortiaTask quest,
      ConsortiaTaskInfo info,
      int value)
      : base(player, quest, info, value)
    {
    }

    public override void AddTrigger(ConsortiaTaskUserDataInfo player)
    {
      player.Player.AfterKillingLiving += new GamePlayer.PlayerGameKillEventHandel(this.method_0);
    }

    public override void RemoveTrigger(ConsortiaTaskUserDataInfo player)
    {
      player.Player.AfterKillingLiving -= new GamePlayer.PlayerGameKillEventHandel(this.method_0);
    }

    private void method_0(
      AbstractGame abstractGame_0,
      int int_1,
      int int_2,
      bool bool_0,
      int int_3,
      bool isArea)
    {
      if (bool_0 || int_1 != 1)
        return;
      switch (abstractGame_0.GameType)
      {
        case eGameType.Free:
          if ((this.m_info.Para1 == 0 || this.m_info.Para1 == -1) && this.Value < this.m_info.Para2)
          {
            ++this.Value;
            break;
          }
          break;
        case eGameType.Guild:
          if ((this.m_info.Para1 == 1 || this.m_info.Para1 == -1) && this.Value < this.m_info.Para2)
          {
            ++this.Value;
            break;
          }
          break;
        case eGameType.Training:
          if ((this.m_info.Para1 == 2 || this.m_info.Para1 == -1) && this.Value < this.m_info.Para2)
          {
            ++this.Value;
            break;
          }
          break;
        case eGameType.Boss:
          if ((this.m_info.Para1 == 6 || this.m_info.Para1 == -1) && this.Value < this.m_info.Para2)
          {
            ++this.Value;
            break;
          }
          break;
        case eGameType.ALL:
          if ((this.m_info.Para1 == 4 || this.m_info.Para1 == -1) && this.Value < this.m_info.Para2)
          {
            ++this.Value;
            break;
          }
          break;
        case eGameType.Exploration:
          if ((this.m_info.Para1 == 5 || this.m_info.Para1 == -1) && this.Value < this.m_info.Para2)
          {
            ++this.Value;
            break;
          }
          break;
        case eGameType.Dungeon:
          if ((this.m_info.Para1 == 7 || this.m_info.Para1 == -1) && this.Value < this.m_info.Para2)
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
