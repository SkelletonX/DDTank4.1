﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.MissionKillChiefCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.GameObjects;

namespace Game.Server.Achievement
{
  public class MissionKillChiefCondition : BaseUserRecord
  {
    public MissionKillChiefCondition(GamePlayer player, int type)
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
      if (game.GameType != eGameType.Dungeon | isLiving || (type != 2 || id != 3317))
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.AfterKillingLiving -= new GamePlayer.PlayerGameKillEventHandel(this.player_AfterKillingLiving);
    }
  }
}
