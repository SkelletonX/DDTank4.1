// Decompiled with JetBrains decompiler
// Type: Game.Server.Event.PlayerLoginCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Server.GameObjects;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Event
{
  public class PlayerLoginCondition : EventCondition
  {
    public PlayerLoginCondition(EventLiveInfo eventLive, GamePlayer player)
      : base(eventLive, player)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.PlayerLogin += new GamePlayer.PlayerLoginEventHandle(this.player_Login);
    }

    private void player_Login()
    {
      this.m_event = EventLiveMgr.GetSingleEvent(this.m_event.EventID);
      if (this.m_event.Condiction_Para1 != 0 && (DateTime.Now - this.m_player.PlayerCharacter.NewDay).TotalDays < (double) this.m_event.Condiction_Para1)
        return;
      this.m_player.SendEventLiveRewards(this.m_event);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.PlayerLogin -= new GamePlayer.PlayerLoginEventHandle(this.player_Login);
    }
  }
}
