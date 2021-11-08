// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.UpdatePlayerStateAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;

namespace Game.Server.Rooms
{
  public class UpdatePlayerStateAction : IAction
  {
    private GamePlayer m_player;
    private BaseRoom m_room;
    private byte m_state;

    public UpdatePlayerStateAction(GamePlayer player, BaseRoom room, byte state)
    {
      this.m_player = player;
      this.m_state = state;
      this.m_room = room;
    }

    public void Execute()
    {
      if (this.m_player.CurrentRoom.RoomId != this.m_room.RoomId)
        return;
      this.m_room.UpdatePlayerState(this.m_player, this.m_state, true);
    }
  }
}
