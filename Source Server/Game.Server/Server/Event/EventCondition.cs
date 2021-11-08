// Decompiled with JetBrains decompiler
// Type: Game.Server.Event.EventCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Server.GameObjects;
using log4net;
using SqlDataProvider.Data;
using System.Reflection;

namespace Game.Server.Event
{
  public class EventCondition
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected EventLiveInfo m_event;
    protected GamePlayer m_player;

    public EventCondition(EventLiveInfo eventLive, GamePlayer player)
    {
      this.m_event = EventLiveMgr.GetSingleEvent(eventLive.EventID);
      this.m_player = player;
    }

    public virtual void AddTrigger(GamePlayer player)
    {
    }

    public static EventCondition CreateCondition(
      EventLiveInfo eventLive,
      GamePlayer player)
    {
      switch (eventLive.CondictionType)
      {
        case 1:
          return (EventCondition) new EventStrengthenCondition(eventLive, player);
        case 2:
          return (EventCondition) new GameOverCondition(eventLive, player);
        case 3:
          return (EventCondition) new MoneyChargeCondition(eventLive, player);
        case 4:
          return (EventCondition) new GameKillCondition(eventLive, player);
        case 5:
          return (EventCondition) new PlayerLoginCondition(eventLive, player);
        case 6:
          return (EventCondition) new UseBalanceCondition(eventLive, player);
        case 7:
          return (EventCondition) new FusionItemCondition(eventLive, player);
        case 8:
          return (EventCondition) new LevelUpCondition(eventLive, player);
        default:
          return (EventCondition) new UnknownCondition(eventLive, player);
      }
    }

    public virtual void RemoveTrigger(GamePlayer player)
    {
    }
  }
}
