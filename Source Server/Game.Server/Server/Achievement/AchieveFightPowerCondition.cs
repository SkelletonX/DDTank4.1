// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.AchieveFightPowerCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;

namespace Game.Server.Achievement
{
  public class AchieveFightPowerCondition : BaseUserRecord
  {
    public AchieveFightPowerCondition(GamePlayer player, int type)
      : base(player, type)
    {
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.ItemStrengthen += new GamePlayer.PlayerItemStrengthenEventHandle(this.player_ItemStrengthen);
    }

    private void player_ItemStrengthen(int categoryID, int level)
    {
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.ItemStrengthen -= new GamePlayer.PlayerItemStrengthenEventHandle(this.player_ItemStrengthen);
    }
  }
}
