// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MailPaymentCancelHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(118, "取消付款邮件")]
  public class MailPaymentCancelHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      GSPacketIn gsPacketIn = new GSPacketIn((short) 118, client.Player.PlayerCharacter.ID);
      int mailID = packet.ReadInt();
      int senderID = 0;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        if (playerBussiness.CancelPaymentMail(client.Player.PlayerCharacter.ID, mailID, ref senderID))
        {
          client.Out.SendMailResponse(senderID, eMailRespose.Receiver);
          packet.WriteBoolean(true);
        }
        else
          packet.WriteBoolean(false);
      }
      client.Out.SendTCP(packet);
      return 1;
    }
  }
}
