// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.CancelPickupAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.Battle;

namespace Game.Server.Rooms
{
  public class CancelPickupAction : IAction
  {
    private BaseRoom m_room;
    private BattleServer m_server;

    public CancelPickupAction(BattleServer server, BaseRoom room)
    {
      this.m_room = room;
      this.m_server = server;
    }

    public void Execute()
    {
      if (this.m_room.Game != null || this.m_server == null)
        return;
      this.m_room.BattleServer = (BattleServer) null;
      this.m_room.IsPlaying = false;
      this.m_room.SendCancelPickUp();
      RoomMgr.WaitingRoom.SendUpdateRoom(this.m_room);
    }
  }
}
