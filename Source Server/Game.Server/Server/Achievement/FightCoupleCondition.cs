﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.FightCoupleCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.GameObjects;

namespace Game.Server.Achievement
{
  public class FightCoupleCondition : BaseUserRecord
  {
    public FightCoupleCondition(GamePlayer player, int type)
      : base(player, type)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.GameOver += new GamePlayer.PlayerGameOverEventHandle(this.player_GameOver);
    }

    private void player_GameOver(
      AbstractGame game,
      bool isWin,
      int gainXp,
      bool isSpanArea,
      bool isCouple)
    {
      if (((game.GameType == eGameType.Free ? 1 : (game.GameType == eGameType.Guild ? 1 : 0)) & (isWin ? 1 : 0) & (isCouple ? 1 : 0)) == 0)
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.GameOver -= new GamePlayer.PlayerGameOverEventHandle(this.player_GameOver);
    }
  }
}
