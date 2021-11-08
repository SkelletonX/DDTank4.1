// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.Mission2KillCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.GameObjects;

namespace Game.Server.Achievement
{
  public class Mission2KillCondition : BaseUserRecord
  {
    public Mission2KillCondition(GamePlayer player, int type)
      : base(player, type)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.AfterKillingLiving += new GamePlayer.PlayerGameKillEventHandel(this.player_AfterKillingLiving);
    }

    private void player_AfterKillingLiving(
      AbstractGame game,
      int type,
      int id,
      bool isLiving,
      int demage,
      bool isSpanArea)
    {
      int num1;
      if (!(game.GameType != eGameType.Dungeon | isLiving) && type == 2)
      {
        int num2;
        switch (id)
        {
          case 1003:
          case 1106:
            num1 = 0;
            goto label_9;
          case 1207:
            num1 = 0;
            goto label_9;
          case 1308:
            num2 = 0;
            break;
          default:
            num2 = 1;
            break;
        }
        num1 = num2;
      }
      else
        num1 = 1;
label_9:
      if ((uint) num1 > 0U)
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.AfterKillingLiving -= new GamePlayer.PlayerGameKillEventHandel(this.player_AfterKillingLiving);
    }
  }
}
