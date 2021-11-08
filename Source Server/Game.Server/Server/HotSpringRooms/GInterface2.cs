// Decompiled with JetBrains decompiler
// Type: Game.Server.HotSpringRooms.GInterface2
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.HotSpringRooms
{
  public interface GInterface2
  {
    void OnGameData(HotSpringRoom game, GamePlayer player, GSPacketIn packet);

    void OnTick(HotSpringRoom room);
  }
}
