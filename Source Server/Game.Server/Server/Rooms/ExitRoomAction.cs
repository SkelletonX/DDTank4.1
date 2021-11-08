// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.ExitRoomAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;

namespace Game.Server.Rooms
{
  public class ExitRoomAction : IAction
  {
    private BaseRoom baseRoom_0;
    private GamePlayer gamePlayer_0;
    private bool bool_0;

    public ExitRoomAction(BaseRoom room, GamePlayer player, bool isSystem)
    {
      this.baseRoom_0 = room;
      this.gamePlayer_0 = player;
      this.bool_0 = isSystem;
    }

    public void Execute()
    {
      this.baseRoom_0.RemovePlayerUnsafe(this.gamePlayer_0, this.bool_0);
      if (this.baseRoom_0.PlayerCount == 0)
        this.baseRoom_0.Stop();
      else
        RoomMgr.WaitingRoom.SendUpdateRoom(this.baseRoom_0.RoomType);
    }
  }
}
