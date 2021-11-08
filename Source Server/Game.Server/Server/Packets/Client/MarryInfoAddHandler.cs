// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MarryInfoAddHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(236, "添加征婚信息")]
  public class MarryInfoAddHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if ((uint) client.Player.PlayerCharacter.MarryInfoID > 0U)
        return 1;
      bool flag = packet.ReadBoolean();
      string str = packet.ReadString();
      int id = client.Player.PlayerCharacter.ID;
      eMessageType type = eMessageType.GM_NOTICE;
      string translateId = "MarryInfoAddHandler.Fail";
      int num = 10000;
      if (num > client.Player.PlayerCharacter.Gold)
      {
        type = eMessageType.BIGBUGLE_NOTICE;
        translateId = "MarryInfoAddHandler.Msg1";
      }
      else
      {
        MarryInfo info = new MarryInfo()
        {
          UserID = id,
          IsPublishEquip = flag,
          Introduction = str,
          RegistTime = DateTime.Now
        };
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          if (playerBussiness.AddMarryInfo(info))
          {
            client.Player.RemoveGold(num);
            translateId = "MarryInfoAddHandler.Msg2";
            client.Player.PlayerCharacter.MarryInfoID = info.ID;
            client.Out.SendMarryInfoRefresh(info, info.ID, true);
          }
        }
      }
      client.Out.SendMessage(type, LanguageMgr.GetTranslation(translateId));
      return 0;
    }
  }
}
