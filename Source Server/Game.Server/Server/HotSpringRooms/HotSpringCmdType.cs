// Decompiled with JetBrains decompiler
// Type: Game.Server.HotSpringRooms.HotSpringCmdType
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

namespace Game.Server.HotSpringRooms
{
  public enum HotSpringCmdType
  {
    TARGET_POINT = 1,
    HOTSPRING_ROOM_RENEWAL_FEE = 3,
    HOTSPRING_ROOM_INVITE = 4,
    HOTSPRING_ROOM_EDIT = 6,
    HOTSPRING_ROOM_TIME_UPDATE = 7,
    HOTSPRING_ROOM_ADMIN_REMOVE_PLAYER = 9,
    HOTSPRING_ROOM_PLAYER_CONTINUE = 10, // 0x0000000A
    CONTINU_BY_MONEY = 11, // 0x0000000B
    CONTINU_BY_MONEY_SUCCESS = 12, // 0x0000000C
  }
}
