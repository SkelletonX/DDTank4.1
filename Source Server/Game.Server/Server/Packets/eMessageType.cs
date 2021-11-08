// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.eMessageType
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

namespace Game.Server.Packets
{
  public enum eMessageType
  {
    GM_NOTICE = 0,
    Normal = 0,
    BIGBUGLE_NOTICE = 1,
    ERROR = 1,
    ChatNormal = 2,
    SYS_TIP_NOTICE = 2,
    ChatERROR = 3,
    SYS_NOTICE = 3,
    ALERT = 4,
    DailyAward = 5,
    Defence = 6,
    CONSORTIA_NOTICE = 8,
    CROSS_NOTICE = 12, // 0x0000000C
  }
}
