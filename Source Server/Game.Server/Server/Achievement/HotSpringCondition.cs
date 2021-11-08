// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.HotSpringCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;

namespace Game.Server.Achievement
{
  public class HotSpringCondition : BaseUserRecord
  {
    public HotSpringCondition(GamePlayer player, int type)
      : base(player, type)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.PlayerSpa += new GamePlayer.PlayerOwnSpaEventHandle(this.player_PlayerSpa);
    }

    private void player_PlayerSpa(int onlineTimeSpa)
    {
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, onlineTimeSpa);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.PlayerSpa -= new GamePlayer.PlayerOwnSpaEventHandle(this.player_PlayerSpa);
    }
  }
}
