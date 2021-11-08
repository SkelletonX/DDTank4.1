// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.ChangeAttackCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Achievement
{
  public class ChangeAttackCondition : BaseUserRecord
  {
    public ChangeAttackCondition(GamePlayer player, int type)
      : base(player, type)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.PlayerPropertyChanged += new GamePlayer.PlayerPropertyChangedEventHandel(this.player_PlayerPropertyChanged);
    }

    private void player_PlayerPropertyChanged(PlayerInfo character)
    {
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, character.Attack, 1);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.PlayerPropertyChanged -= new GamePlayer.PlayerPropertyChangedEventHandel(this.player_PlayerPropertyChanged);
    }
  }
}
