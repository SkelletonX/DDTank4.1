// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.TankMarryLogicProcessor
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.SceneMarryRooms.TankHandle;
using log4net;
using System;
using System.Reflection;

namespace Game.Server.SceneMarryRooms
{
  [MarryProcessor(9, "礼堂逻辑")]
  public class TankMarryLogicProcessor : AbstractMarryProcessor
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private MarryCommandMgr _commandMgr = new MarryCommandMgr();
    private ThreadSafeRandom random = new ThreadSafeRandom();
    public readonly int TIMEOUT = 60000;

    public override void OnGameData(MarryRoom room, GamePlayer player, GSPacketIn packet)
    {
      MarryCmdType marryCmdType = (MarryCmdType) packet.ReadByte();
      try
      {
        IMarryCommandHandler marryCommandHandler = this._commandMgr.LoadCommandHandler((int) marryCmdType);
        if (marryCommandHandler != null)
          marryCommandHandler.HandleCommand(this, player, packet);
        else
          TankMarryLogicProcessor.log.Error((object) string.Format("IP: {0}", (object) player.Client.TcpEndpoint));
      }
      catch (Exception ex)
      {
        TankMarryLogicProcessor.log.Error((object) string.Format("IP:{1}, OnGameData is Error: {0}", (object) ex.ToString(), (object) player.Client.TcpEndpoint));
      }
    }

    public override void OnTick(MarryRoom room)
    {
      try
      {
        if (room == null)
          return;
        room.KickAllPlayer();
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
          playerBussiness.DisposeMarryRoomInfo(room.Info.ID);
        GameServer.Instance.LoginServer.SendUpdatePlayerMarriedStates(room.Info.GroomID);
        GameServer.Instance.LoginServer.SendUpdatePlayerMarriedStates(room.Info.BrideID);
        GameServer.Instance.LoginServer.SendMarryRoomInfoToPlayer(room.Info.GroomID, false, room.Info);
        GameServer.Instance.LoginServer.SendMarryRoomInfoToPlayer(room.Info.BrideID, false, room.Info);
        MarryRoomMgr.RemoveMarryRoom(room);
        GSPacketIn pkg = new GSPacketIn((short) 254);
        pkg.WriteInt(room.Info.ID);
        WorldMgr.MarryScene.SendToALL(pkg);
        room.StopTimer();
      }
      catch (Exception ex)
      {
        if (!TankMarryLogicProcessor.log.IsErrorEnabled)
          return;
        TankMarryLogicProcessor.log.Error((object) nameof (OnTick), ex);
      }
    }
  }
}
