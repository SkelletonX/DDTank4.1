// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.KickPlayerAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

namespace Game.Server.Rooms
{
  public class KickPlayerAction : IAction
  {
    private int m_place;
    private BaseRoom m_room;

    public KickPlayerAction(BaseRoom room, int place)
    {
      this.m_room = room;
      this.m_place = place;
    }

    public void Execute()
    {
      this.m_room.RemovePlayerAtUnsafe(this.m_place);
    }
  }
}
