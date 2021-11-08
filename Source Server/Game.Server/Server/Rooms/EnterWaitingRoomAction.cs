// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.EnterWaitingRoomAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;

namespace Game.Server.Rooms
{
  public class EnterWaitingRoomAction : IAction
  {
    private GamePlayer m_player;

    public EnterWaitingRoomAction(GamePlayer player)
    {
      this.m_player = player;
    }

    public void Execute()
    {
      if (this.m_player == null)
        return;
      if (this.m_player.CurrentRoom != null)
        this.m_player.CurrentRoom.RemovePlayerUnsafe(this.m_player);
      BaseWaitingRoom waitingRoom = RoomMgr.WaitingRoom;
      if (!waitingRoom.AddPlayer(this.m_player))
        return;
      this.m_player.Out.SendUpdateRoomList(RoomMgr.GetAllRooms());
      this.m_player.Out.SendSceneAddPlayer(this.m_player);
      foreach (GamePlayer player in waitingRoom.GetPlayersSafe())
      {
        if (player != this.m_player)
          this.m_player.Out.SendSceneAddPlayer(player);
      }
    }
  }
}
