// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.FightOneBloodIsWinCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.GameObjects;

namespace Game.Server.Achievement
{
  public class FightOneBloodIsWinCondition : BaseUserRecord
  {
    public FightOneBloodIsWinCondition(GamePlayer player, int type)
      : base(player, type)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.FightOneBloodIsWin += new GamePlayer.PlayerFightOneBloodIsWin(this.player_OneBloodIsWin);
    }

    private void player_OneBloodIsWin(eRoomType roomType, bool isWin)
    {
      if (!(roomType == eRoomType.Match & isWin))
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.FightOneBloodIsWin -= new GamePlayer.PlayerFightOneBloodIsWin(this.player_OneBloodIsWin);
    }
  }
}
