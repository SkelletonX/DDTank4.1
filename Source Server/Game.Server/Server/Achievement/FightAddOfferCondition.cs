// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.FightAddOfferCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;

namespace Game.Server.Achievement
{
  internal class FightAddOfferCondition : BaseUserRecord
  {
    public FightAddOfferCondition(GamePlayer player, int type)
      : base(player, type)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.FightAddOfferEvent += new GamePlayer.PlayerFightAddOffer(this.player_FightAddOfferEvent);
    }

    private void player_FightAddOfferEvent(int offer)
    {
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, offer);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.FightAddOfferEvent -= new GamePlayer.PlayerFightAddOffer(this.player_FightAddOfferEvent);
    }
  }
}
