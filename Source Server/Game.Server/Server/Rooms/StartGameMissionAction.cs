// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.StartGameMissionAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;

namespace Game.Server.Rooms
{
  public class StartGameMissionAction : IAction
  {
    private BaseRoom m_room;

    public StartGameMissionAction(BaseRoom room)
    {
      this.m_room = room;
    }

    public void Execute()
    {
      this.m_room.Game.MissionStart((IGamePlayer) this.m_room.Host);
    }
  }
}
