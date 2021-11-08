// Decompiled with JetBrains decompiler
// Type: Game.Server.Event.ExerciseUpCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Event
{
  public class ExerciseUpCondition : EventCondition
  {
    public ExerciseUpCondition(EventLiveInfo eventLive, GamePlayer player)
      : base(eventLive, player)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.AfterUsingItem += new GamePlayer.PlayerItemPropertyEventHandle(this.player_Exercise);
    }

    private void player_Exercise(int templateID, int count)
    {
      this.m_event = EventLiveMgr.GetSingleEvent(this.m_event.EventID);
      if (this.m_event.Condiction_Para1 != templateID)
        return;
      this.m_player.SendEventLiveRewards(this.m_event);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.AfterUsingItem -= new GamePlayer.PlayerItemPropertyEventHandle(this.player_Exercise);
    }
  }
}
