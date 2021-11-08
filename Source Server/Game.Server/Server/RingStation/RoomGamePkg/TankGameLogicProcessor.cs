// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.RoomGamePkg.TankGameLogicProcessor
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.RingStation.RoomGamePkg.TankHandle;
using log4net;
using System;
using System.Reflection;

namespace Game.Server.RingStation.RoomGamePkg
{
  [GameProcessor(9, "礼堂逻辑")]
  public class TankGameLogicProcessor : AbstractGameProcessor
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private GameCommandMgr _commandMgr = new GameCommandMgr();
    public readonly int TIMEOUT = 60000;

    public override void OnGameData(RoomGame room, RingStationGamePlayer player, GSPacketIn packet)
    {
      GameCmdType code = (GameCmdType) packet.Code;
      try
      {
        IGameCommandHandler gameCommandHandler = this._commandMgr.LoadCommandHandler((int) code);
        if (gameCommandHandler != null)
        {
          gameCommandHandler.HandleCommand(this, player, packet);
        }
        else
        {
          Console.WriteLine("______________ERROR______________");
          Console.WriteLine("LoadCommandHandler not found!");
          Console.WriteLine("_______________END_______________");
        }
      }
      catch (Exception ex)
      {
      }
    }

    public override void OnTick(RoomGame room)
    {
    }
  }
}
