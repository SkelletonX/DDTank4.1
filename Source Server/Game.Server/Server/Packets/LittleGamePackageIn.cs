using System;

namespace Game.Server.Packets
{
    public enum LittleGamePackageIn
    {
        LOAD_WORLD_LIST = 1,
        ENTER_WORLD = 2,
        LOAD_COMPLETED = 3,
        LEAVE_WORLD = 4,
        PING = 6,
        MOVE = 32,
        POS_SYNC = 33,
        REPORT_SCORE = 64,
        CLICK = 65,
        CANCEL_CLICK = 66
    }
}