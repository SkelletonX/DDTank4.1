// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Actions.ActionType
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

namespace Game.Logic.Phy.Actions
{
  public enum ActionType
  {
    NULLSHOOT = -1, // 0xFFFFFFFF
    PICK = 1,
    BOMB = 2,
    START_MOVE = 3,
    FLY_OUT = 4,
    KILL_PLAYER = 5,
    TRANSLATE = 6,
    FORZEN = 7,
    CHANGE_SPEED = 8,
    UNFORZEN = 9,
    DANDER = 10, // 0x0000000A
    CURE = 11, // 0x0000000B
    DEFENCE = 12, // 0x0000000C
    UNANGLE = 13, // 0x0000000D
    DO_ACTION = 14, // 0x0000000E
    PLAYBUFFER = 15, // 0x0000000F
    Laser = 16, // 0x00000010
    BOMBED = 17, // 0x00000011
    PUP = 18, // 0x00000012
    AUP = 19, // 0x00000013
    PET = 20, // 0x00000014
  }
}
