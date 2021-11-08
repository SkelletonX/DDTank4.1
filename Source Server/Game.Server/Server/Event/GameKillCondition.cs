// Decompiled with JetBrains decompiler
// Type: Game.Server.Event.GameKillCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Logic;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Event
{
  public class GameKillCondition : EventCondition
  {
    public GameKillCondition(EventLiveInfo eventLive, GamePlayer player)
      : base(eventLive, player)
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
      this.m_event = EventLiveMgr.GetSingleEvent(this.m_event.EventID);
      if (isLiving || type != 1)
        return;
      switch (game.GameType)
      {
        case eGameType.Free:
          if ((uint) this.m_event.Condiction_Para1 > 0U)
            break;
          ++this.m_player.playersKilled;
          if (this.m_player.playersKilled < this.m_event.Condiction_Para2)
            break;
          this.m_player.SendEventLiveRewards(this.m_event);
          this.m_player.playersKilled = 0;
          break;
        case eGameType.Guild:
          if (this.m_event.Condiction_Para1 != 1)
            break;
          ++this.m_player.playersKilled;
          if (this.m_player.playersKilled < this.m_event.Condiction_Para2)
            break;
          this.m_player.SendEventLiveRewards(this.m_event);
          this.m_player.playersKilled = 0;
          break;
        case eGameType.ALL:
          if (this.m_event.Condiction_Para1 != 2)
            break;
          ++this.m_player.playersKilled;
          if (this.m_player.playersKilled < this.m_event.Condiction_Para2)
            break;
          this.m_player.SendEventLiveRewards(this.m_event);
          this.m_player.playersKilled = 0;
          break;
      }
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.AfterKillingLiving -= new GamePlayer.PlayerGameKillEventHandel(this.player_AfterKillingLiving);
    }
  }
}
