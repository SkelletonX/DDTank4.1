// Decompiled with JetBrains decompiler
// Type: Game.Server.Event.UnknownCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Event
{
  public class UnknownCondition : EventCondition
  {
    public UnknownCondition(EventLiveInfo eventL, GamePlayer player)
      : base(eventL, player)
    {
    }
  }
}
