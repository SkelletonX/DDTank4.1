using Game.Logic;
using Game.Server.Rooms;

internal class RoomSetupChangeAction : IAction
{
    private BaseRoom baseRoom_0;
    private eRoomType eRoomType_0;
    private byte byte_0;
    private eHardLevel eHardLevel_0;
    private int int_0;
    private string string_0;
    private string string_1;
    private bool bool_0;
    private int int_1;
    private int int_2;

    public RoomSetupChangeAction(
      BaseRoom baseRoom_1,
      eRoomType eRoomType_1,
      byte byte_1,
      eHardLevel eHardLevel_1,
      int int_3,
      int int_4,
      string string_2,
      string string_3,
      bool bool_1,
      int int_5)
    {
        this.baseRoom_0 = baseRoom_1;
        this.eRoomType_0 = eRoomType_1;
        this.byte_0 = byte_1;
        this.eHardLevel_0 = eHardLevel_1;
        this.int_1 = int_3;
        this.int_0 = int_4;
        this.string_0 = string_2;
        this.string_1 = string_3;
        this.bool_0 = bool_1;
        this.int_2 = int_5;
    }

    public void Execute()
    {
        this.baseRoom_0.Name = this.string_0;
        this.baseRoom_0.RoomType = this.eRoomType_0;
        this.baseRoom_0.TimeMode = this.byte_0;
        this.baseRoom_0.HardLevel = this.eHardLevel_0;
        this.baseRoom_0.LevelLimits = this.int_1;
        this.baseRoom_0.MapId = this.int_0;
        this.baseRoom_0.Password = this.string_1;
        this.baseRoom_0.isCrosszone = this.bool_0;
        this.baseRoom_0.currentFloor = this.int_2;
        this.baseRoom_0.UpdateRoomGameType();
        this.baseRoom_0.SendRoomSetupChange(this.baseRoom_0);
        RoomMgr.WaitingRoom.SendUpdateCurrentRoom(this.baseRoom_0);
    }
}