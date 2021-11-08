// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.DivorceApplyHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(248, "离婚")]
  internal class DivorceApplyHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      bool flag = packet.ReadBoolean();
      if (!client.Player.PlayerCharacter.IsMarried)
        return 1;
      if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
      {
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
        return 0;
      }
      if (client.Player.PlayerCharacter.IsCreatedMarryRoom)
      {
        client.Player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("DivorceApplyHandler.Msg2"));
        return 1;
      }
      int money = GameProperties.PRICE_DIVORCED;
      if (flag)
        money = GameProperties.PRICE_DIVORCED_DISCOUNT;
      if (!client.Player.MoneyDirect(money, false))
        return 1;
      CountBussiness.InsertSystemPayCount(client.Player.PlayerCharacter.ID, money, 0, 0, 3);
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        PlayerInfo userSingleByUserId = playerBussiness.GetUserSingleByUserID(client.Player.PlayerCharacter.SpouseID);
        if (userSingleByUserId == null || userSingleByUserId.Sex == client.Player.PlayerCharacter.Sex)
          return 1;
        MarryApplyInfo info = new MarryApplyInfo()
        {
          UserID = client.Player.PlayerCharacter.SpouseID,
          ApplyUserID = client.Player.PlayerCharacter.ID,
          ApplyUserName = client.Player.PlayerCharacter.NickName,
          ApplyType = 3,
          LoveProclamation = "",
          ApplyResult = false
        };
        new PlayerBussiness().AddDailyRecord(new DailyRecordInfo()
        {
          UserID = client.Player.PlayerCharacter.ID,
          Type = 27,
          Value = client.Player.PlayerCharacter.SpouseName
        });
        int id = 0;
        if (playerBussiness.SavePlayerMarryNotice(info, 0, ref id))
        {
          GameServer.Instance.LoginServer.SendUpdatePlayerMarriedStates(userSingleByUserId.ID);
          client.Player.LoadMarryProp();
        }
      }
      client.Player.QuestInventory.ClearMarryQuest();
      client.Player.Out.SendPlayerDivorceApply(client.Player, true, true);
      client.Player.SendMessage(LanguageMgr.GetTranslation("DivorceApplyHandler.Msg3"));
      return 0;
    }
  }
}
