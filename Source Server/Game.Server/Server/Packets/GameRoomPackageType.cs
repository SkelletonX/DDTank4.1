// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.GameRoomPackageType
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

namespace Game.Server.Packets
{
  public enum GameRoomPackageType
  {
    GAME_ROOM_CREATE = 0,
    GAME_ROOM_LOGIN = 1,
    GAME_ROOM_SETUP_CHANGE = 2,
    GAME_ROOM_KICK = 3,
    GAME_ROOM_ADDPLAYER = 4,
    GAME_ROOM_REMOVEPLAYER = 5,
    GAME_TEAM = 6,
    GAME_START = 7,
    ROOMLIST_UPDATE = 9,
    GAME_ROOM_UPDATE_PLACE = 10, // 0x0000000A
    GAME_PICKUP_CANCEL = 11, // 0x0000000B
    GAME_PICKUP_STYLE = 12, // 0x0000000C
    GAME_PICKUP_WAIT = 13, // 0x0000000D
    ROOM_PASS = 14, // 0x0000000E
    GAME_PLAYER_STATE_CHANGE = 15, // 0x0000000F
    GAME_ROOM_FULL = 17, // 0x00000011
    SINGLE_ROOM_BEGIN = 18, // 0x00000012
    FAST_INVITE_CALL = 19, // 0x00000013
    GAME_ENERGY_NOT_ENOUGH = 20, // 0x00000014
    LAST_MISSION_FOR_WARRIORSARENA = 33, // 0x00000021
    PASSED_WARRIORSARENA_10 = 34, // 0x00000022
    No_WARRIORSARENA_TICKET = 35, // 0x00000023
  }
}
