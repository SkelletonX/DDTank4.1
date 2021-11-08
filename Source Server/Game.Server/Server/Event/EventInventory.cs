// Decompiled with JetBrains decompiler
// Type: Game.Server.Event.EventInventory
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Server.GameObjects;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Event
{
  public class EventInventory
  {
    private object m_lock;
    private GamePlayer m_player;

    public EventInventory(GamePlayer player)
    {
      this.m_player = player;
      this.m_lock = new object();
    }

    public void LoadFromDatabase()
    {
      lock (this.m_lock)
      {
        foreach (EventLiveInfo eventLive in EventLiveMgr.GetAllEventInfo())
        {
          if (eventLive.StartDate < DateTime.Now && eventLive.EndDate > DateTime.Now)
            EventCondition.CreateCondition(eventLive, this.m_player);
        }
      }
    }
  }
}
