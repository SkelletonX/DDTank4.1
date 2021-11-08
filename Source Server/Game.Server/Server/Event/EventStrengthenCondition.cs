// Decompiled with JetBrains decompiler
// Type: Game.Server.Event.EventStrengthenCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Event
{
  public class EventStrengthenCondition : EventCondition
  {
    public EventStrengthenCondition(EventLiveInfo eventLive, GamePlayer player)
      : base(eventLive, player)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.ItemStrengthen += new GamePlayer.PlayerItemStrengthenEventHandle(this.player_ItemStrengthen);
    }

    private void player_ItemStrengthen(int categoryID, int level)
    {
      this.m_event = EventLiveMgr.GetSingleEvent(this.m_event.EventID);
      if (this.m_event.Condiction_Para1 != categoryID || this.m_event.Condiction_Para2 != level)
        return;
      this.m_player.SendEventLiveRewards(this.m_event);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.ItemStrengthen -= new GamePlayer.PlayerItemStrengthenEventHandle(this.player_ItemStrengthen);
    }
  }
}
