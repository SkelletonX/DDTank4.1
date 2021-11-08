// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.BigBugleHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(72, "大喇叭")]
  public class BigBugleHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      SqlDataProvider.Data.ItemInfo itemByTemplateId = client.Player.PropBag.GetItemByTemplateID(0, num);
      if (DateTime.Compare(client.Player.LastChatTime.AddSeconds(5.0), DateTime.Now) > 0)
      {
        client.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("BigBugleHandler.Msg"));
        return 1;
      }
      GSPacketIn packet1 = new GSPacketIn((short) 72);
      if (itemByTemplateId != null)
      {
        packet.ReadInt();
        packet.ReadString();
        string str = packet.ReadString();
        client.Player.PropBag.RemoveCountFromStack(itemByTemplateId, 1);
        packet1.WriteInt(itemByTemplateId.Template.Property2);
        packet1.WriteInt(client.Player.PlayerCharacter.ID);
        packet1.WriteString(client.Player.PlayerCharacter.NickName);
        packet1.WriteString(str);
        GameServer.Instance.LoginServer.SendPacket(packet1);
        client.Player.LastChatTime = DateTime.Now;
        foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
        {
          packet1.ClientID = allPlayer.PlayerCharacter.ID;
          allPlayer.Out.SendTCP(packet1);
        }
      }
      else
      {
        packet.ReadString();
        string str = packet.ReadString();
        SqlDataProvider.Data.ItemInfo itemByCategoryId = client.Player.PropBag.GetItemByCategoryID(0, 11, 4);
        client.Player.PropBag.RemoveCountFromStack(itemByCategoryId, 1);
        packet1.WriteInt(client.Player.ZoneId);
        packet1.WriteInt(client.Player.PlayerCharacter.ID);
        packet1.WriteString(client.Player.PlayerCharacter.NickName);
        packet1.WriteString(str);
        packet1.WriteString(client.Player.ZoneName);
        GameServer.Instance.LoginServer.SendPacket(packet1);
        client.Player.LastChatTime = DateTime.Now;
        foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
        {
          packet1.ClientID = allPlayer.PlayerCharacter.ID;
          allPlayer.Out.SendTCP(packet1);
        }
      }
      client.Player.OnUsingItem(num, 1);
      return 0;
    }
  }
}
