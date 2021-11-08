// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.Mission2OverCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.GameObjects;

namespace Game.Server.Achievement
{
  public class Mission2OverCondition : BaseUserRecord
  {
    public Mission2OverCondition(GamePlayer player, int type)
      : base(player, type)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.MissionOver += new GamePlayer.PlayerMissionOverEventHandle(this.player_MissionOver);
    }

    private void player_MissionOver(AbstractGame game, int missionId, bool isWin)
    {
      int num1;
      if (game.GameType == eGameType.Dungeon & isWin)
      {
        int num2;
        switch (missionId)
        {
          case 1073:
          case 1176:
            num1 = 0;
            goto label_9;
          case 1277:
            num1 = 0;
            goto label_9;
          case 1378:
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
      player.MissionOver -= new GamePlayer.PlayerMissionOverEventHandle(this.player_MissionOver);
    }
  }
}
