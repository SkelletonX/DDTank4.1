// Decompiled with JetBrains decompiler
// Type: Game.Server.Event.GameOverCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Logic;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Event
{
  public class GameOverCondition : EventCondition
  {
    public GameOverCondition(EventLiveInfo eventLive, GamePlayer player)
      : base(eventLive, player)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.MissionTurnOver += new GamePlayer.PlayerMissionTurnOverEventHandle(this.player_MissionOver);
    }

    private void player_MissionOver(AbstractGame game, int missionId, int turnCount)
    {
      this.m_event = EventLiveMgr.GetSingleEvent(this.m_event.EventID);
      if (missionId != this.m_event.Condiction_Para1 || game.GameType != eGameType.Dungeon)
        return;
      this.m_player.SendEventLiveRewards(this.m_event);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.MissionTurnOver -= new GamePlayer.PlayerMissionTurnOverEventHandle(this.player_MissionOver);
    }
  }
}
