// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.CreateRoomAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Logic;
using Game.Server.GameObjects;

namespace Game.Server.Rooms
{
  public class CreateRoomAction : IAction
  {
    private string m_name;
    private string m_password;
    private GamePlayer m_player;
    private eRoomType m_roomType;
    private byte m_timeType;

    public CreateRoomAction(
      GamePlayer player,
      string name,
      string password,
      eRoomType roomType,
      byte timeType)
    {
      this.m_player = player;
      this.m_name = name;
      this.m_password = password;
      this.m_roomType = roomType;
      this.m_timeType = timeType;
    }

    public void Execute()
    {
      if (this.m_player.CurrentRoom != null)
        this.m_player.CurrentRoom.RemovePlayerUnsafe(this.m_player);
      if (!this.m_player.IsActive)
        return;
      BaseRoom[] rooms = RoomMgr.Rooms;
      BaseRoom room = (BaseRoom) null;
      for (int index = 0; index < rooms.Length; ++index)
      {
        if (!rooms[index].IsUsing)
        {
          room = rooms[index];
          break;
        }
      }
      if (room == null)
        return;
      RoomMgr.WaitingRoom.RemovePlayer(this.m_player);
      room.Start();
      if (this.m_roomType == eRoomType.Exploration)
      {
        room.HardLevel = eHardLevel.Normal;
        room.LevelLimits = (int) room.GetLevelLimit(this.m_player);
      }
      room.UpdateRoom(this.m_name, this.m_password, this.m_roomType, this.m_timeType, 0);
      this.m_player.Out.SendRoomCreate(room);
      room.AddPlayerUnsafe(this.m_player);
      RoomMgr.WaitingRoom.SendUpdateRoom(room);
    }
  }
}
