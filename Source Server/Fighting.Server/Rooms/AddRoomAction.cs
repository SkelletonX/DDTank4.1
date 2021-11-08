// Decompiled with JetBrains decompiler
// Type: Fighting.Server.Rooms.AddRoomAction
// Assembly: Fighting.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8F1EB855-F1B7-44B3-B212-6508DAF33CC5
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Fight\Fighting.Server.dll

namespace Fighting.Server.Rooms
{
  public class AddRoomAction : IAction
  {
    private ProxyRoom m_room;

    public AddRoomAction(ProxyRoom room)
    {
      this.m_room = room;
    }

    public void Execute()
    {
      ProxyRoomMgr.AddRoomUnsafe(this.m_room);
      if (!this.m_room.startWithNpc)
        return;
      ProxyRoomMgr.StartWithNpcUnsafe(this.m_room);
    }
  }
}
