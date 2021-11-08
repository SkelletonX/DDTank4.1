// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.StartMissionCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.GameObjects;

namespace Game.Server.Achievement
{
  public class StartMissionCondition : BaseUserRecord
  {
    public StartMissionCondition(GamePlayer player, int type)
      : base(player, type)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.MissionTurnOver += new GamePlayer.PlayerMissionTurnOverEventHandle(this.player_MissionTurnOver);
    }

    private void player_MissionTurnOver(AbstractGame game, int missionId, int turnNum)
    {
      if (game.RoomType == eRoomType.Freshman || game.RoomType == eRoomType.FightLab)
        return;
      ++this.m_player.missionPlayed;
      if (this.m_player.missionPlayed != 2)
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.MissionTurnOver -= new GamePlayer.PlayerMissionTurnOverEventHandle(this.player_MissionTurnOver);
    }
  }
}
