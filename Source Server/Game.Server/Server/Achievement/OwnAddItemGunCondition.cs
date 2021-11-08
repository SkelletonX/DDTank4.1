// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.OwnAddItemGunCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.GameObjects;

namespace Game.Server.Achievement
{
  public class OwnAddItemGunCondition : BaseUserRecord
  {
    public OwnAddItemGunCondition(GamePlayer player, int type)
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
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, this.m_player.GetItemCount(7015) + this.m_player.GetItemCount(7016) + this.m_player.GetItemCount(7017) + this.m_player.GetItemCount(7018) + this.m_player.GetItemCount(7019) + this.m_player.GetItemCount(7020) + this.m_player.GetItemCount(7021) + this.m_player.GetItemCount(7022) + this.m_player.GetItemCount(7023));
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.AfterKillingLiving -= new GamePlayer.PlayerGameKillEventHandel(this.player_AfterKillingLiving);
    }
  }
}
