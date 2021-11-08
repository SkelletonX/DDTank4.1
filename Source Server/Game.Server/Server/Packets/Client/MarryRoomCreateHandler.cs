// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MarryRoomCreateHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using Game.Server.SceneMarryRooms;
using log4net;
using SqlDataProvider.Data;
using System.Reflection;

namespace Game.Server.Packets.Client
{
  [PacketHandler(241, "礼堂创建")]
  public class MarryRoomCreateHandler : IPacketHandler
  {
    protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.PlayerCharacter.IsMarried && !client.Player.PlayerCharacter.IsCreatedMarryRoom)
      {
        if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
        {
          client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
          return 0;
        }
        if (client.Player.CurrentRoom != null)
          client.Player.CurrentRoom.RemovePlayerUnsafe(client.Player);
        if (client.Player.CurrentMarryRoom != null)
          client.Player.CurrentMarryRoom.RemovePlayer(client.Player);
        MarryRoomInfo info = new MarryRoomInfo()
        {
          Name = packet.ReadString(),
          Pwd = packet.ReadString(),
          MapIndex = packet.ReadInt(),
          AvailTime = packet.ReadInt(),
          MaxCount = packet.ReadInt(),
          GuestInvite = packet.ReadBoolean(),
          RoomIntroduction = packet.ReadString(),
          ServerID = GameServer.Instance.Configuration.ServerID,
          IsHymeneal = false
        };
        string[] strArray = GameProperties.PRICE_MARRY_ROOM.Split(',');
        if (strArray.Length < 3)
        {
          if (MarryRoomCreateHandler.log.IsErrorEnabled)
            MarryRoomCreateHandler.log.Error((object) "MarryRoomCreateMoney node in configuration file is wrong");
          return 1;
        }
        int money;
        switch (info.AvailTime)
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
            info.AvailTime = 4;
            break;
        }
        if (client.Player.MoneyDirect(money, false))
        {
          MarryRoom marryRoom = MarryRoomMgr.CreateMarryRoom(client.Player, info);
          if (marryRoom != null)
          {
            GSPacketIn packet1 = client.Player.Out.SendMarryRoomInfo(client.Player, marryRoom);
            client.Player.Out.SendMarryRoomLogin(client.Player, true);
            marryRoom.SendToScenePlayer(packet1);
            CountBussiness.InsertSystemPayCount(client.Player.PlayerCharacter.ID, money, 0, 0, 0);
            new PlayerBussiness().AddDailyRecord(new DailyRecordInfo()
            {
              UserID = client.Player.PlayerCharacter.ID,
              Type = 4,
              Value = client.Player.PlayerCharacter.SpouseName
            });
          }
          return 0;
        }
      }
      return 1;
    }
  }
}
