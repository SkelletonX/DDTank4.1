﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.Mission12OverCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.GameObjects;

namespace Game.Server.Achievement
{
  public class Mission12OverCondition : BaseUserRecord
  {
    public Mission12OverCondition(GamePlayer player, int type)
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
      if (!(game.GameType == eGameType.Dungeon & isWin) || missionId != 5104 && missionId != 5204 && missionId != 5304)
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.MissionOver -= new GamePlayer.PlayerMissionOverEventHandle(this.player_MissionOver);
    }
  }
}
