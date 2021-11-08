// Decompiled with JetBrains decompiler
// Type: Game.Server.Event.FusionItemCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Event
{
  public class FusionItemCondition : EventCondition
  {
    public FusionItemCondition(EventLiveInfo eventLive, GamePlayer player)
      : base(eventLive, player)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.ItemFusion += new GamePlayer.PlayerItemFusionEventHandle(this.player_ItemFusion);
    }

    private void player_ItemFusion(int templateID)
    {
      this.m_event = EventLiveMgr.GetSingleEvent(this.m_event.EventID);
      if (this.m_event.Condiction_Para1 != templateID)
        return;
      this.m_player.SendEventLiveRewards(this.m_event);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.ItemFusion -= new GamePlayer.PlayerItemFusionEventHandle(this.player_ItemFusion);
    }
  }
}
