// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.LabyrinthPackageType
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

namespace Game.Server.Packets
{
  public enum LabyrinthPackageType
  {
    DOUBLE_REWARD = 1,
    REQUEST_UPDATE = 2,
    CLEAN_OUT = 3,
    SPEEDED_UP_CLEAN_OUT = 4,
    STOP_CLEAN_OUT = 5,
    RESET_LABYRINTH = 6,
    PUSH_CLEAN_OUT_INFO = 7,
    CLEAN_OUT_COMPLETE = 8,
    TRY_AGAIN = 9,
  }
}
