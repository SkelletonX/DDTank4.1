// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.PetPackageType
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

namespace Game.Server.Packets
{
  public enum PetPackageType
  {
    UPDATE_PET = 1,
    ADD_PET = 2,
    MOVE_PETBAG = 3,
    FEED_PET = 4,
    REFRESH_PET = 5,
    ADOPT_PET = 6,
    EQUIP_PET_SKILL = 7,
    RELEASE_PET = 8,
    RENAME_PET = 9,
    PAY_SKILL = 16, // 0x00000010
    FIGHT_PET = 17, // 0x00000011
    REVER_PET = 18, // 0x00000012
    ADD_PET_EQUIP = 20, // 0x00000014
    DEL_PET_EQUIP = 21, // 0x00000015
    PET_RISINGSTAR = 22, // 0x00000016
    PET_EVOLUTION = 23, // 0x00000017
    PET_FORMINFO = 24, // 0x00000018
    PET_FOLLOW = 25, // 0x00000019
    PET_BREAK = 31, // 0x0000001F
    PET_WAKE = 32, // 0x00000020
    EAT_PETS = 33, // 0x00000021
    PET_BREAK_INFO = 34, // 0x00000022
  }
}
