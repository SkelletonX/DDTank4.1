// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.UpdateRoomPosAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

namespace Game.Server.Rooms
{
  public class UpdateRoomPosAction : IAction
  {
    private BaseRoom baseRoom_0;
    private int int_0;
    private bool bool_0;

    public UpdateRoomPosAction(BaseRoom room, int pos, bool isOpened)
    {
      this.baseRoom_0 = room;
      this.int_0 = pos;
      this.bool_0 = isOpened;
    }

    public void Execute()
    {
      if (this.baseRoom_0.PlayerCount <= 0 || !this.baseRoom_0.UpdatePosUnsafe(this.int_0, this.bool_0))
        return;
      RoomMgr.WaitingRoom.SendUpdateCurrentRoom(this.baseRoom_0);
    }
  }
}
