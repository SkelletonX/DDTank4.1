// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.GetLinkGoodsHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(119, "物品比较")]
  public class GetLinkGoodsHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int val = packet.ReadInt();
      packet.ReadString();
      int itemID = packet.ReadInt();
      GSPacketIn packet1 = new GSPacketIn((short) 119, client.Player.PlayerCharacter.ID);
      string nickName = client.Player.PlayerCharacter.NickName;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        packet1.WriteInt(val);
        if (val != 4)
        {
          if (val == 5)
          {
            packet.ReadString();
            packet1.WriteString(nickName);
            packet1.WriteInt(0);
            packet1.WriteInt(0);
            packet1.WriteInt(0);
            packet1.WriteInt(0);
            packet1.WriteInt(0);
            packet1.WriteInt(0);
            packet1.WriteInt(0);
            packet1.WriteInt(0);
            client.Out.SendTCP(packet1);
            return 0;
          }
          SqlDataProvider.Data.ItemInfo userItemSingle = playerBussiness.GetUserItemSingle(itemID);
          if (userItemSingle != null)
          {
            packet1.WriteString(nickName);
            packet1.WriteInt(userItemSingle.TemplateID);
            packet1.WriteInt(userItemSingle.ItemID);
            packet1.WriteInt(userItemSingle.StrengthenLevel);
            packet1.WriteInt(userItemSingle.AttackCompose);
            packet1.WriteInt(userItemSingle.AgilityCompose);
            packet1.WriteInt(userItemSingle.LuckCompose);
            packet1.WriteInt(userItemSingle.DefendCompose);
            packet1.WriteInt(userItemSingle.ValidDate);
            packet1.WriteBoolean(userItemSingle.IsBinds);
            packet1.WriteBoolean(userItemSingle.IsJudge);
            packet1.WriteBoolean(userItemSingle.IsUsed);
            if (userItemSingle.IsUsed)
              packet1.WriteString(userItemSingle.BeginDate.ToString());
            packet1.WriteInt(userItemSingle.Hole1);
            packet1.WriteInt(userItemSingle.Hole2);
            packet1.WriteInt(userItemSingle.Hole3);
            packet1.WriteInt(userItemSingle.Hole4);
            packet1.WriteInt(userItemSingle.Hole5);
            packet1.WriteInt(userItemSingle.Hole6);
            packet1.WriteString(userItemSingle.Template.Hole);
            packet1.WriteString(userItemSingle.Template.Pic);
            packet1.WriteInt(userItemSingle.RefineryLevel);
            packet1.WriteDateTime(DateTime.Now);
            packet1.WriteByte((byte) userItemSingle.Hole5Level);
            packet1.WriteInt(userItemSingle.Hole5Exp);
            packet1.WriteByte((byte) userItemSingle.Hole6Level);
            packet1.WriteInt(userItemSingle.Hole6Exp);
            client.Out.SendTCP(packet1);
          }
          return 1;
        }
        packet1.WriteString(nickName);
        packet1.WriteInt(0);
        packet1.WriteInt(0);
        packet1.WriteInt(0);
        packet1.WriteInt(0);
        packet1.WriteInt(0);
        client.Out.SendTCP(packet1);
        return 0;
      }
    }
  }
}
