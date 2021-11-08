// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.TankHandle.ContinuationCommand
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Packets;
using log4net;
using System.Reflection;

namespace Game.Server.SceneMarryRooms.TankHandle
{
  [MarryCommandAttbute(3)]
  public class ContinuationCommand : IMarryCommandHandler
  {
    protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public bool HandleCommand(
      TankMarryLogicProcessor process,
      GamePlayer player,
      GSPacketIn packet)
    {
      if (player.CurrentMarryRoom == null || player.PlayerCharacter.ID != player.CurrentMarryRoom.Info.GroomID && player.PlayerCharacter.ID != player.CurrentMarryRoom.Info.BrideID)
        return false;
      int time = packet.ReadInt();
      string[] strArray = GameProperties.PRICE_MARRY_ROOM.Split(',');
      if (strArray.Length < 3)
      {
        if (ContinuationCommand.log.IsErrorEnabled)
          ContinuationCommand.log.Error((object) "MarryRoomCreateMoney node in configuration file is wrong");
        return false;
      }
      int money;
      switch (time)
      {
        case 2:
          money = int.Parse(strArray[0]);
          break;
        case 3:
          money = int.Parse(strArray[1]);
          break;
        case 4:
          money = int.Parse(strArray[2]);
          break;
        default:
          money = int.Parse(strArray[2]);
          time = 4;
          break;
      }
      if (player.PlayerCharacter.Money + player.PlayerCharacter.MoneyLock < money)
      {
        player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("MarryApplyHandler.Msg1"));
        return false;
      }
      player.RemoveMoney(money);
      CountBussiness.InsertSystemPayCount(player.PlayerCharacter.ID, money, 0, 0, 0);
      player.CurrentMarryRoom.RoomContinuation(time);
      GSPacketIn packet1 = player.Out.SendContinuation(player, player.CurrentMarryRoom.Info);
      WorldMgr.GetPlayerById(player.PlayerCharacter.ID != player.CurrentMarryRoom.Info.GroomID ? player.CurrentMarryRoom.Info.GroomID : player.CurrentMarryRoom.Info.BrideID)?.Out.SendTCP(packet1);
      player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ContinuationCommand.Successed"));
      return true;
    }
  }
}
