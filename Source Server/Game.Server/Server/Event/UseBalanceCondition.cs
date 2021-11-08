// Decompiled with JetBrains decompiler
// Type: Game.Server.Event.UseBalanceCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Event
{
  public class UseBalanceCondition : EventCondition
  {
    public UseBalanceCondition(EventLiveInfo eventLive, GamePlayer player)
      : base(eventLive, player)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.Paid += new GamePlayer.PlayerShopEventHandle(this.player_Shop);
    }

    private void player_Shop(
      int money,
      int gold,
      int offer,
      int gifttoken,
      int medal,
      string payGoods)
    {
      this.m_event = EventLiveMgr.GetSingleEvent(this.m_event.EventID);
      if (this.m_event.Condiction_Para1 == 0 && money >= this.m_event.Condiction_Para2)
        this.m_player.SendEventLiveRewards(this.m_event);
      if (this.m_event.Condiction_Para1 == 1 && this.m_event.Condiction_Para2 >= gold)
        this.m_player.SendEventLiveRewards(this.m_event);
      if (this.m_event.Condiction_Para1 == 2 && this.m_event.Condiction_Para2 >= offer)
        this.m_player.SendEventLiveRewards(this.m_event);
      if (this.m_event.Condiction_Para1 == 3 && this.m_event.Condiction_Para2 >= gifttoken)
        this.m_player.SendEventLiveRewards(this.m_event);
      if (this.m_event.Condiction_Para1 != 4 || this.m_event.Condiction_Para2 < medal)
        return;
      this.m_player.SendEventLiveRewards(this.m_event);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.Paid -= new GamePlayer.PlayerShopEventHandle(this.player_Shop);
    }
  }
}
