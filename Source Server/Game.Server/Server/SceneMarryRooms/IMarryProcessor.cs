// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.IMarryProcessor
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.SceneMarryRooms
{
  public interface IMarryProcessor
  {
    void OnGameData(MarryRoom game, GamePlayer player, GSPacketIn packet);

    void OnTick(MarryRoom room);
  }
}
