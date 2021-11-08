using System;

namespace Game.Server.Packets
{
    public enum LittleGamePackageOut
    {
        WORLD_LIST = 1,
        START_LOAD = 2,
        GAME_START = 3,
        SETCLOCK = 5,
        PONG = 6,
        NET_DELAY = 7,
        ADD_SPRITE = 16,
        REMOVE_SPRITE = 17,
        KICK_PLAYE = 18,
        MOVE = 32,
        UPDATE_POS = 33,
        GETSCORE = 49,
        ADD_OBJECT = 64,
        REMOVE_OBJECT = 65,
        INVOKE_OBJECT = 66,
        UPDATELIVINGSPROPERTY = 80,
        DoMovie = 81,
        DoAction = 96
    }
}