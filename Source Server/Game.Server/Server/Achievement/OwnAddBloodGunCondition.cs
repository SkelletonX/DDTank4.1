// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.OwnAddBloodGunCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;

namespace Game.Server.Achievement
{
  public class OwnAddBloodGunCondition : BaseUserRecord
  {
    public OwnAddBloodGunCondition(GamePlayer player, int type)
      : base(player, type)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, player.GetItemCount(17002));
    }
  }
}
