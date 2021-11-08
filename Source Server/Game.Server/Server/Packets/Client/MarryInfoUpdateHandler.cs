// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MarryInfoUpdateHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(213, "更新征婚信息")]
  public class MarryInfoUpdateHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.PlayerCharacter.MarryInfoID == 0)
        return 1;
      bool flag = packet.ReadBoolean();
      string str = packet.ReadString();
      int marryInfoId = client.Player.PlayerCharacter.MarryInfoID;
      string translateId = "MarryInfoUpdateHandler.Fail";
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        MarryInfo marryInfoSingle = playerBussiness.GetMarryInfoSingle(marryInfoId);
        if (marryInfoSingle == null)
        {
          translateId = "MarryInfoUpdateHandler.Msg1";
        }
        else
        {
          marryInfoSingle.IsPublishEquip = flag;
          marryInfoSingle.Introduction = str;
          marryInfoSingle.RegistTime = DateTime.Now;
          if (playerBussiness.UpdateMarryInfo(marryInfoSingle))
            translateId = "MarryInfoUpdateHandler.Succeed";
        }
        client.Out.SendMarryInfoRefresh(marryInfoSingle, marryInfoId, marryInfoSingle != null);
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(translateId));
      }
      return 0;
    }
  }
}
