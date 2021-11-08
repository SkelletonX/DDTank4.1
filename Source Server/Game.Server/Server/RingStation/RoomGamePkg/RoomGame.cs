// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.RoomGamePkg.RoomGame
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.RingStation.RoomGamePkg
{
  public class RoomGame
  {
    private static object _syncStop = new object();
    private IGameProcessor _processor = (IGameProcessor) new TankGameLogicProcessor();

    protected void OnTick(object obj)
    {
      this._processor.OnTick(this);
    }

    public void ProcessData(RingStationGamePlayer player, GSPacketIn data)
    {
      lock (RoomGame._syncStop)
        this._processor.OnGameData(this, player, data);
    }
  }
}
